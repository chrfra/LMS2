import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CourseService } from '../services/course.service';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-course-viewer',
  templateUrl: './course-viewer.component.html',
  styleUrls: ['./course-viewer.component.css']
})
export class CourseViewerComponent implements OnInit {
  course: any = null;
  currentPageIndex: number = 0;
  loading: boolean = false;
  error: string = '';

  constructor(
    private courseService: CourseService,
    private authService: AuthService,
    private route: ActivatedRoute,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.loadCourse();
  }

  loadCourse(): void {
    const courseId = this.route.snapshot.paramMap.get('id');
    if (!courseId) {
      this.error = 'Course ID not found';
      return;
    }

    this.loading = true;
    this.courseService.getCourse(parseInt(courseId)).subscribe({
      next: (data) => {
        this.course = data;
        this.loading = false;
      },
      error: (error) => {
        this.error = 'Failed to load course. Please try again.';
        this.loading = false;
      }
    });
  }

  get currentPage(): any {
    if (!this.course || !this.course.pages || this.currentPageIndex < 0 || this.currentPageIndex >= this.course.pages.length) {
      return null;
    }
    return this.course.pages[this.currentPageIndex];
  }

  get currentPageNumber(): number {
    return this.currentPageIndex + 1;
  }

  get totalPages(): number {
    return this.course?.pages?.length || 0;
  }

  goToPreviousPage(): void {
    if (this.currentPageIndex > 0) {
      this.currentPageIndex--;
    }
  }

  goToNextPage(): void {
    if (this.currentPageIndex < this.totalPages - 1) {
      this.currentPageIndex++;
    }
  }

  goToPage(index: number): void {
    if (index >= 0 && index < this.totalPages) {
      this.currentPageIndex = index;
    }
  }

  goBack(): void {
    this.router.navigate(['/dashboard']);
  }

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}
