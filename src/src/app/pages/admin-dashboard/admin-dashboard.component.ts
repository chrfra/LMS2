import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { CourseService } from '../services/course.service';

@Component({
  selector: 'app-admin-dashboard',
  templateUrl: './admin-dashboard.component.html',
  styleUrls: ['./admin-dashboard.component.css']
})
export class AdminDashboardComponent implements OnInit {
  courses: any[] = [];
  loading: boolean = false;
  uploading: boolean = false;
  error: string = '';
  message: string = '';
  selectedFile: File | null = null;

  constructor(
    private authService: AuthService,
    private courseService: CourseService,
    private router: Router
  ) { }

  ngOnInit(): void {
    // Check if user is admin
    if (this.authService.currentUserValue?.role !== 'Admin') {
      this.router.navigate(['/']);
    }
    this.loadCourses();
  }

  loadCourses(): void {
    this.loading = true;
    this.error = '';
    this.courseService.getCourses().subscribe({
      next: (data) => {
        this.courses = data;
        this.loading = false;
      },
      error: (error) => {
        this.error = 'Failed to load courses. Please try again.';
        this.loading = false;
      }
    });
  }

  onFileSelected(event: any): void {
    const file = event.target.files[0];
    if (file && file.name.endsWith('.zip')) {
      this.selectedFile = file;
      this.error = '';
    } else {
      this.error = 'Please select a valid .zip SCORM package';
      this.selectedFile = null;
    }
  }

  uploadCourse(): void {
    if (!this.selectedFile) {
      this.error = 'Please select a file to upload';
      return;
    }

    this.uploading = true;
    this.error = '';
    this.message = '';

    this.courseService.uploadCourse(this.selectedFile).subscribe({
      next: (response) => {
        this.message = `Course "${response.title}" uploaded successfully with ${response.pageCount} pages.`;
        this.selectedFile = null;
        this.uploading = false;
        const fileInput = document.getElementById('fileInput') as HTMLInputElement;
        if (fileInput) {
          fileInput.value = '';
        }
        this.loadCourses();
      },
      error: (error) => {
        this.error = error.error?.message || 'Failed to upload course. Please try again.';
        this.uploading = false;
      }
    });
  }

  deleteCourse(id: number, title: string): void {
    if (confirm(`Are you sure you want to delete the course "${title}"?`)) {
      this.courseService.deleteCourse(id).subscribe({
        next: () => {
          this.message = `Course "${title}" deleted successfully.`;
          this.loadCourses();
        },
        error: (error) => {
          this.error = error.error?.message || 'Failed to delete course. Please try again.';
        }
      });
    }
  }

  viewCourse(id: number): void {
    this.router.navigate(['/course', id]);
  }

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}
