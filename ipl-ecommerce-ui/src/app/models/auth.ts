export interface RegisterRequest {
  firstName: string;
  lastName: string;
  email: string;
  password: string;
}

export interface RegisterResponse {
  userId?: number;
  firstName?: string;
  lastName?: string;
  email?: string;
  message?: string;
  token?: string;
}