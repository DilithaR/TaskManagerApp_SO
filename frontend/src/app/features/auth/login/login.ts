import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../../core/auth.service';

@Component({
  selector: 'app-login',
  imports: [FormsModule],
  templateUrl: './login.html',
  styleUrl: './login.css',
})
export class Login {
  private auth = inject(AuthService);
  private router = inject(Router);

  username = 'admin';
  password = 'admin123';
  error = '';

  submit() {
    this.auth.login(this.username, this.password).subscribe({
      next: () => this.router.navigateByUrl('/tasks'),
      error: () => (this.error = 'Login failed'),
    });
  }
}