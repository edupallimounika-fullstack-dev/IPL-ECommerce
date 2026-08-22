import { Component, inject } from '@angular/core';

import {
  FormBuilder,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';

import { RouterLink,Router } from '@angular/router';

import {
  AuthService
} from '../../../core/services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    RouterLink
  ],
  templateUrl: './login.html'
})
export class LoginComponent {

  private readonly fb =
    inject(FormBuilder);

  private readonly authService =
    inject(AuthService);

  private readonly router =
    inject(Router);

  errorMessage = '';

  isLoading = false;

  loginForm =
    this.fb.nonNullable.group({

      email: [
        '',
        [
          Validators.required,
          Validators.email
        ]
      ],

      password: [
        '',
        [
          Validators.required
        ]
      ]

    });

  login(): void {

    if (this.loginForm.invalid) {

      this.loginForm.markAllAsTouched();

      return;
    }

    this.errorMessage = '';

    this.isLoading = true;

    this.authService
      .login(this.loginForm.getRawValue())
      .subscribe({

        next: () => {

          this.isLoading = false;

          // Login successful
          this.router.navigate(['/products']);
        },

        error: error => {

          this.isLoading = false;

          console.error(
            'Login failed',
            error
          );

          this.errorMessage =
            error?.error?.detail ??
            error?.error?.message ??
            'Invalid email or password.';
        }

      });
  }
}