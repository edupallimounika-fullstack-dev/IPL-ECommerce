import {
  Component,
  inject,
  signal
} from '@angular/core';

import {
  FormBuilder,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';

import {
  Router,
  RouterLink
} from '@angular/router';

import {
  AuthService
} from '../../../core/services/auth.service';

import {
  RegisterRequest
} from '../../../models/auth';


@Component({
  selector: 'app-register',

  standalone: true,

  imports: [
    ReactiveFormsModule,
    RouterLink
  ],

  templateUrl: './register.html'
})
export class RegisterComponent {

  private readonly fb =
    inject(FormBuilder);

  private readonly authService =
    inject(AuthService);

  private readonly router =
    inject(Router);


  isSubmitting =
    signal(false);

  errorMessage =
    signal('');

  successMessage =
    signal('');


  form =
    this.fb.nonNullable.group({

      firstName: [
        '',
        [
          Validators.required,
          Validators.maxLength(100)
        ]
      ],

      lastName: [
        '',
        [
          Validators.required,
          Validators.maxLength(100)
        ]
      ],

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
          Validators.required,
          Validators.minLength(6)
        ]
      ]

    });


  register(): void {

    if (this.form.invalid) {

      this.form.markAllAsTouched();

      return;
    }


    this.isSubmitting.set(true);

    this.errorMessage.set('');

    this.successMessage.set('');


    const request: RegisterRequest =
      this.form.getRawValue();


    console.log(
      'REGISTER REQUEST:',
      request
    );


    this.authService
      .register(request)
      .subscribe({

        next: (response: { message?: string; userId?: number; email?: string; token?: string }) => {

          console.log(
            'REGISTRATION SUCCESS:',
            response
          );


          this.isSubmitting.set(false);

          this.successMessage.set(
            'Registration successful. Redirecting to login...'
          );


          setTimeout(() => {

            this.router.navigate([
              '/login'
            ]);

          }, 1000);
        },


        error: (error: { error?: { detail?: string; message?: string } }) => {

          console.error(
            'REGISTRATION ERROR:',
            error
          );


          this.isSubmitting.set(false);


          this.errorMessage.set(
            error?.error?.detail ??
            error?.error?.message ??
            'Registration failed.'
          );
        }

      });
  }
}