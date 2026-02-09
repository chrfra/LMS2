using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LMS.Core.Models;
using LMS.Infrastructure.Data;
using LMS.Infrastructure.Services;

namespace LMS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CoursesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IScormParserService _scormParser;

        public CoursesController(AppDbContext context, IScormParserService scormParser)
        {
            _context = context;
            _scormParser = scormParser;
        }

        [HttpGet]
        public async Task<IActionResult> GetCourses()
        {
            var courses = await _context.Courses
                .Select(c => new
                {
                    c.Id,
                    c.Title,
                    c.Description,
                    PageCount = c.Pages.Count,
                    c.CreatedAt,
                    c.UpdatedAt
                })
                .ToListAsync();

            return Ok(courses);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCourse(int id)
        {
            var course = await _context.Courses
                .Include(c => c.Pages.OrderBy(p => p.Order))
                .FirstOrDefaultAsync(c => c.Id == id);

            if (course == null)
            {
                return NotFound(new { message = "Course not found" });
            }

            return Ok(new
            {
                course.Id,
                course.Title,
                course.Description,
                Pages = course.Pages.Select(p => new
                {
                    p.Id,
                    p.Title,
                    p.Order,
                    p.Content
                })
            });
        }

        [HttpGet("{courseId}/pages/{pageId}")]
        public async Task<IActionResult> GetPage(int courseId, int pageId)
        {
            var page = await _context.Pages
                .Include(p => p.Course)
                .FirstOrDefaultAsync(p => p.Id == pageId && p.CourseId == courseId);

            if (page == null)
            {
                return NotFound(new { message = "Page not found" });
            }

            return Ok(new
            {
                page.Id,
                page.Title,
                page.Order,
                page.Content,
                page.CourseId
            });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("upload")]
        public async Task<IActionResult> UploadCourse(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(new { message = "No file uploaded" });
            }

            if (!file.FileName.EndsWith(".zip"))
            {
                return BadRequest(new { message = "Only .zip files are supported" });
            }

            try
            {
                using (var stream = file.OpenReadStream())
                {
                    var (course, pages) = await _scormParser.ParseScormPackageAsync(stream, file.FileName);

                    _context.Courses.Add(course);
                    await _context.SaveChangesAsync();

                    foreach (var page in pages)
                    {
                        page.CourseId = course.Id;
                        _context.Pages.Add(page);
                    }

                    await _context.SaveChangesAsync();

                    return CreatedAtAction(nameof(GetCourse), new { id = course.Id }, new
                    {
                        course.Id,
                        course.Title,
                        course.Description,
                        PageCount = pages.Count
                    });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error uploading course: {ex.Message}" });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound(new { message = "Course not found" });
            }

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Course deleted successfully" });
        }
    }
}
