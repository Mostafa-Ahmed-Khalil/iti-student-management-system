import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { ApiResponse } from '../../../core/models/api-response.model';
import { User, CreateUserRequest } from './user.model';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = `${environment.apiBaseUrl}/users`;

  getUsers(page: number = 1, pageSize: number = 50, role?: string): Observable<User[]> {
    let params = new HttpParams()
      .set('page', page.toString())
      .set('pageSize', pageSize.toString());

    if (role) {
      params = params.set('role', role);
    }

    return this.http.get<ApiResponse<User[]>>(this.apiUrl, { params }).pipe(
      map(response => response.data)
    );
  }

  createUser(request: CreateUserRequest): Observable<User> {
    return this.http.post<ApiResponse<User>>(this.apiUrl, request).pipe(
      map(response => response.data)
    );
  }

  assignRoles(userId: string, roles: string[]): Observable<void> {
    return this.http.put<ApiResponse<void>>(`${this.apiUrl}/${userId}/roles`, roles).pipe(
      map(() => void 0)
    );
  }

  updateUser(userId: string, fullName: string, email: string): Observable<User> {
    return this.http.put<ApiResponse<User>>(`${this.apiUrl}/${userId}`, { fullName, email }).pipe(
      map(r => r.data)
    );
  }

  deleteUser(userId: string): Observable<void> {
    return this.http.delete<ApiResponse<void>>(`${this.apiUrl}/${userId}`).pipe(
      map(() => void 0)
    );
  }
}
