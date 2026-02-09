# Edapp2

**Overview**

LMS is a copy of the functions in the learning management system “Edapp”.

It will allow admin users log in (user name admin password admin123) to 

import existing courses from Edapp in SCORM format,

edit and add all content in the courses,

allow exporting all content from in SCORM format from Edapp2. 

Assign courses to users

track progress for each end user

View course participants feedback.

View and revert to previously saved version of each page.

Endusers, or “learners” should be able to 

sign up with email and password,

see their assigned courses,

see their progress on each page and course across several sessions,

At any time leave feedback on the course e.g. “Can we make anything easier to understand in the course or add anything to make it easier to make use of the course content?”

Save your context in a file like [context.md](http://context.md/).

**Tech stack**

Tech stack built on C# angular frontend + SQlite.
A goal is to containerize the app to run in azure and render. For now it will run in render.

**MCP**

Always use Context7 MCP when I need library/API documentation, code generation, setup or configuration steps without me having to explicitly ask.rse content is correct  and edit content in course (image, text, add slide) then check so updates are visible to lerarner, then revert changes to the original. API key is ctx7sk-2d7a5444-841f-418c-a51f-18338e508ef5

**Context**

Update context (e.g. context.md) with what you have changed and need to know in context going forward, for example:

- **Trigger:** The skill activates when you have made changes, e.g. before commit to git.
- **Analyze Changes:** It must scan and analyze all file changes made in your currently active git branch.
- **Identify Context:** It needs to identify which features were modified and locate the corresponding context.md files for those features.
- **Update Documentation:** It automatically writes updates to those specific [context.](http://context.md)md files to reflect the code changes you just made.

**Code review**

Review the code for bugs or less than great code and make changes.

**Features & design**

Use a web browser to log in to the links below I have credentials and can log in for you if needed:

The design should follow this design system: [https://www.figma.com/design/CmOxiutkPZ5naH51zdkEBE/Flow-Connect-Design-System?node-id=1615-13284&p=f&t=ojdTQqkVHV8B3kc2-0](https://www.figma.com/design/CmOxiutkPZ5naH51zdkEBE/Flow-Connect-Design-System?node-id=1615-13284&p=f&t=ojdTQqkVHV8B3kc2-0)

Edapp2 should have the same features as edapp.
Research improvements to edapp’s user interface based on leading LMS systems and other best in breed saas products and propose improvements using visual examples to motivate improvements.

Will start with the most important core features.
Edapp enduser view: [https://web.edapp.com/lesson/664b15146c60eabf5f811c5f/play](https://web.edapp.com/lesson/664b15146c60eabf5f811c5f/play)

Edapp2 should look to the admin user like it looks today with the same features, improvements UI can be proposed.We will start with the most important core features.
Edapp admin user view: [https://admin.edapp.com/course/6984728c030911fe31d5b2fc/edit](https://admin.edapp.com/course/6984728c030911fe31d5b2fc/edit)

**Accounts**

My github email is [christian.fransson@outlook.com](mailto:christian.fransson@outlook.com).  github project is [https://github.com/chrfra/LMS2.git](https://github.com/chrfra/LMS2.git). use version control on each thing I ask you to add.

I have created a “web service” ([https://render.com/docs/web-services](https://render.com/docs/web-services)) on the platform Render where the app will run automatically after you push to git after about 5-10 minutes. When render has deployed the application run it here as the user [https://lms2-fijp.onrender.com](https://lms2-fijp.onrender.com/) to do the testing.

**Testing**

Do thorough QC testing before asking for me to review what you have developed.

Test your work in a real browser (not headless) by clicking  on the live version deployed on render, not locally, when you develop new features so that when i test it everything we agreed on should work.

Render usually takes ca 5 minutes to deploy after you push so you need to wait 5 minutes then run the tests if the render is complete otherwise wait another five minutes after you deploy to do the tests.

Take this as use case or testing basic functionality:

**Pre-requisites**

- [ ]  Wait for your changes to be pushed to git then built by render Render
- [ ]  Delete any previously uploaded courses

**Use Case 1: SCORM Upload** 

- [ ]  Login as admin
- [ ]  Upload SCORM course (you have it in the project folder as “place_order_app_web_desginer-697b66d975656fd469740852-1770208230743-scorm.zip”
- [ ]  Verify Course "Place Order App web desginer" created (33 slides)
- [ ]  Verify that each element is displayed correctly by comparing against how each page in the Place order app chapter looks like in the orginal Edapp admin user view: [https://admin.edapp.com/course/6984728c030911fe31d5b2fc/edit](https://admin.edapp.com/course/6984728c030911fe31d5b2fc/edit)

**Use Case 2: Learner Course Progress** 

- [ ]  Login as admin (user name admin password admin123)
- [ ]  Open course
- [ ]  Verify that each element is displayed correctly by comparing against how each page in the Place order app chapter looks like in the orginal Edapp enduser view: [https://web.edapp.com/lesson/664b15146c60eabf5f811c5f/play](https://web.edapp.com/lesson/664b15146c60eabf5f811c5f/play)
- [ ]  Verify progress is registered/saved

**Use Case 3: Teacher Course Editing** 

- [ ]  Log in as administrator, update course title, body text, image, add new slide, remove slide
- [ ]  Verify updates visible in courses list
- [ ]  Make changes available to end users
- [ ]  Log in as end user and verify that changes are displayed correctly
- [ ]  Revert changes to previous version