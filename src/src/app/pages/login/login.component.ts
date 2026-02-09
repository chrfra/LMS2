import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
  imports: [CommonModule, FormsModule]
})
export class LoginComponent implements OnInit {
  username: string = '';
  password: string = '';
  loading: boolean = false;
  submitted: boolean = false;
  error: string = '';
  returnUrl: string = '';

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private authService: AuthService
  ) {
    // Redirect to home if already logged in
    if (this.authService.currentUserValue) {
      this.router.navigate(['/']);
    }
  }

  ngOnInit(): void {
    // Get return url from route parameters or default to '/'
    this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
  }

  onSubmit(): void {
    this.submitted = true;
    this.error = '';

    if (!this.username || !this.password) {
      this.error = 'Please enter both username and password';
      return;
    }

    this.loading = true;
    this.authService.login(this.username, this.password).subscribe({
      next: (response) => {
        this.router.navigate([this.returnUrl]);
      },
      error: (error) => {
        this.error = error.error?.message || 'Login failed. Please try again.';
        this.loading = false;
      }
    });
  }
}
