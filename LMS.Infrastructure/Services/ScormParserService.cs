using ICSharpCode.SharpZipLib.Zip;
using System.Xml.Linq;
using LMS.Core.Models;

namespace LMS.Infrastructure.Services
{
    public interface IScormParserService
    {
        Task<(Course Course, List<Page> Pages)> ParseScormPackageAsync(Stream zipStream, string fileName);
    }

    public class ScormParserService : IScormParserService
    {
        private readonly string _uploadDirectory = "data/scorm-uploads";

        public ScormParserService()
        {
            if (!Directory.Exists(_uploadDirectory))
            {
                Directory.CreateDirectory(_uploadDirectory);
            }
        }

        public async Task<(Course Course, List<Page> Pages)> ParseScormPackageAsync(Stream zipStream, string fileName)
        {
            var course = new Course();
            var pages = new List<Page>();

            try
            {
                // Save the zip file temporarily
                var tempFolder = Path.Combine(_uploadDirectory, Path.GetFileNameWithoutExtension(fileName) + "_" + Guid.NewGuid().ToString());
                Directory.CreateDirectory(tempFolder);

                // Extract the zip file
                using (var zipFile = new ZipFile(zipStream))
                {
                    zipFile.IsStreamOwner = false;  // Don't close the input stream
                    foreach (ZipEntry entry in zipFile)
                    {
                        if (!entry.IsDirectory)
                        {
                            var entryPath = Path.Combine(tempFolder, entry.Name);
                            var dir = Path.GetDirectoryName(entryPath);
                            if (!Directory.Exists(dir))
                            {
                                Directory.CreateDirectory(dir!);
                            }

                            using (var entryStream = zipFile.GetInputStream(entry))
                            using (var fileStream = File.Create(entryPath))
                            {
                                await entryStream.CopyToAsync(fileStream);
                            }
                        }
                    }
                }

                // Parse imsmanifest.xml
                var manifestPath = Path.Combine(tempFolder, "imsmanifest.xml");
                if (!File.Exists(manifestPath))
                {
                    throw new InvalidOperationException("imsmanifest.xml not found in SCORM package");
                }

                var manifest = XDocument.Load(manifestPath);
                
                // Extract course metadata
                var ns = manifest.Root?.Name.NamespaceName ?? "";
                var metadataElement = manifest.Root?.Element(XName.Get("metadata", ns)) ?? 
                                     manifest.Root?.Element("metadata");
                
                if (metadataElement != null)
                {
                    var titleElement = metadataElement.Element(XName.Get("title", ns)) ?? 
                                     metadataElement.Element("title");
                    if (titleElement != null)
                    {
                        course.Title = titleElement.Value;
                    }
                }

                // If no title found, use file name
                if (string.IsNullOrEmpty(course.Title))
                {
                    course.Title = Path.GetFileNameWithoutExtension(fileName);
                }

                course.Description = $"Imported from SCORM package: {fileName}";

                // Parse course structure
                var organizations = manifest.Root?.Element(XName.Get("organizations", ns)) ?? 
                                   manifest.Root?.Element("organizations");
                
                if (organizations != null)
                {
                    var organization = organizations.Element(XName.Get("organization", ns)) ?? 
                                     organizations.Element("organization");
                    
                    if (organization != null)
                    {
                        var items = organization.Elements(XName.Get("item", ns))
                            .Concat(organization.Elements("item")).ToList();
                        
                        int pageOrder = 0;
                        foreach (var item in items)
                        {
                            var page = ParseScormItem(item, tempFolder, pageOrder++, ns);
                            if (page != null)
                            {
                                pages.Add(page);
                            }
                        }
                    }
                }

                // Store the original SCORM data as base64 for future export
                zipStream.Seek(0, SeekOrigin.Begin);
                byte[] zipBytes = new byte[zipStream.Length];
                await zipStream.ReadExactlyAsync(zipBytes);
                course.ScormPackageData = Convert.ToBase64String(zipBytes);

                return (course, pages);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error parsing SCORM package: {ex.Message}", ex);
            }
        }

        private Page? ParseScormItem(XElement item, string basePath, int order, string ns)
        {
            var identifierref = item.Attribute("identifierref")?.Value ?? item.Attribute("href")?.Value;
            var titleElement = item.Element(XName.Get("title", ns)) ?? item.Element("title");
            var title = titleElement?.Value ?? $"Page {order + 1}";

            var page = new Page
            {
                Title = title,
                Order = order,
                Content = $"<div><h1>{title}</h1><p>Content loaded from SCORM resource...</p></div>"
            };

            return page;
        }
    }
}

