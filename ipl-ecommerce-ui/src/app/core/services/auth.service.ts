import {
  Injectable,
  inject
} from '@angular/core';

import {
  HttpClient
} from '@angular/common/http';

import {
  Observable,
  tap
} from 'rxjs';

import {
  Router
} from '@angular/router';


export interface LoginRequest {
  email: string;
  password: string;
}


export interface LoginResponse {
  token: string;
}


export interface RegisterRequest {
  firstName: string;
  lastName: string;
  email: string;
  password: string;
}


@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private readonly http =
    inject(HttpClient);

  private readonly router =
    inject(Router);

  private readonly apiUrl =
    'http://localhost:5221/api/auth';


  login(
    request: LoginRequest
  ): Observable<LoginResponse> {

  return this.http
    .post<LoginResponse>(
      `${this.apiUrl}/login`,
      request
    )
    .pipe(

      tap(response => {

        localStorage.setItem(
          'token',
          response.token
        );

      })

    );
  }


  register(
    request: RegisterRequest
  ): Observable<any> {

    return this.http.post(
      `${this.apiUrl}/register`,
      request
    );
  }


  logout(): void {

    localStorage.removeItem('token');

    this.router.navigateByUrl('/login');
  }


  getToken(): string | null {

    return localStorage.getItem(
      'token'
    );
  }

  isLoggedIn(): boolean {

    return !!this.getToken();
  }

  isAuthenticated(): boolean {

    return this.isLoggedIn();
  }
}