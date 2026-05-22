import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { environment } from '../../../environments/environment';
import { ApiResponse } from '../models/api-response.model';
import { Enrollment } from '../models/enrollment.model';

@Injectable({
  providedIn: 'root'
})
export class EnrollmentService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiBaseUrl}/enrollments`;

  getEnrolledStudents(trackId: number): Observable<Enrollment[]> {
    return this.http.get<ApiResponse<Enrollment[]>>(`${this.apiUrl}/tracks/${trackId}/students`).pipe(
      map(response => response.data)
    );
  }

  enrollStudents(trackId: number, studentIds: string[]): Observable<void> {
    return this.http.post<ApiResponse<void>>(`${this.apiUrl}/tracks/${trackId}/enrollments`, { studentIds }).pipe(
      map(() => void 0)
    );
  }

  unenrollStudent(trackId: number, studentId: string): Observable<void> {
    return this.http.delete<ApiResponse<void>>(`${this.apiUrl}/tracks/${trackId}/enrollments/${studentId}`).pipe(
      map(() => void 0)
    );
  }
}
