import { TestBed } from '@angular/core/testing';
import { AuthService } from './auth.service';

describe('AuthService', () => {
  let service: AuthService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AuthService);
    localStorage.clear();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should expose isAuthenticated() for the app template', () => {
    expect(typeof service.isAuthenticated).toBe('function');
    expect(service.isAuthenticated()).toBeFalse();

    localStorage.setItem('token', 'abc123');
    expect(service.isAuthenticated()).toBeTrue();
  });
});
