import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Track } from '../models/track.model';
import { environment } from '../../../environments/environment';

export interface ApiResponse<T> {
  success: boolean;
  data: T;
  message: string;
}

@Injectable({
  providedIn: 'root'
})
export class TrackService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiBaseUrl}`;

  getMyBranch(): Observable<ApiResponse<any>> {
    return this.http.get<ApiResponse<any>>(`${this.apiUrl}/users/me/branch`);
  }

  getTracksByBranch(branchId: number): Observable<ApiResponse<Track[]>> {
    return this.http.get<ApiResponse<Track[]>>(`${this.apiUrl}/branches/${branchId}/tracks`);
  }

  createTrack(branchId: number, track: Partial<Track>): Observable<ApiResponse<Track>> {
    return this.http.post<ApiResponse<Track>>(`${this.apiUrl}/branches/${branchId}/tracks`, track);
  }

  updateTrack(id: number, track: Partial<Track>): Observable<ApiResponse<any>> {
    return this.http.put<ApiResponse<any>>(`${this.apiUrl}/tracks/${id}`, track);
  }

  deleteTrack(id: number): Observable<ApiResponse<any>> {
    return this.http.delete<ApiResponse<any>>(`${this.apiUrl}/tracks/${id}`);
  }
}
