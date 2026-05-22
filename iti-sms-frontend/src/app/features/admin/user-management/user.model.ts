export interface User {
  id: string;
  email: string;
  fullName: string;
  roles: string[];
}

export interface CreateUserRequest {
  email: string;
  password?: string;
  fullName: string;
  roles: string[];
}
