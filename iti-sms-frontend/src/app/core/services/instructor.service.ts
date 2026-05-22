import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { environment } from '../../../environments/environment';
import { ApiResponse } from '../models/api-response.model';
import { InstructorCourseDto, LabEvaluationGridDto, UpsertLabEvaluationRequest } from '../models/lab-evaluation.model';

@Injectable({ providedIn: 'root' })
export class InstructorService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = `${environment.apiBaseUrl}/courses`;

  getMyCourses(): Observable<InstructorCourseDto[]> {
    return this.http.get<ApiResponse<InstructorCourseDto[]>>(`${this.apiUrl}/me`).pipe(
      map(r => r.data)
    );
  }

  getLabEvaluations(courseId: number, trackId: number): Observable<LabEvaluationGridDto> {
    return this.http.get<ApiResponse<LabEvaluationGridDto>>(`${this.apiUrl}/${courseId}/evaluations?trackId=${trackId}`).pipe(
      map(r => r.data)
    );
  }

  upsertLabEvaluation(courseId: number, request: UpsertLabEvaluationRequest): Observable<void> {
    return this.http.put<ApiResponse<void>>(`${this.apiUrl}/${courseId}/evaluations`, request).pipe(
      map(() => void 0)
    );
  }
}
