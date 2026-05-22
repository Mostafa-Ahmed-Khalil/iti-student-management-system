import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { Branch, CreateBranchRequest, UpdateBranchRequest } from './branch.model';
import { ApiResponse } from '../../../core/models/api-response.model';

@Injectable({
  providedIn: 'root'
})
export class BranchService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiBaseUrl}/branches`;

  getBranches(): Observable<ApiResponse<Branch[]>> {
    return this.http.get<ApiResponse<Branch[]>>(this.apiUrl);
  }

  createBranch(request: CreateBranchRequest): Observable<ApiResponse<Branch>> {
    return this.http.post<ApiResponse<Branch>>(this.apiUrl, request);
  }

  updateBranch(id: number, request: UpdateBranchRequest): Observable<ApiResponse<any>> {
    return this.http.put<ApiResponse<any>>(`${this.apiUrl}/${id}`, request);
  }

  deleteBranch(id: number): Observable<ApiResponse<any>> {
    return this.http.delete<ApiResponse<any>>(`${this.apiUrl}/${id}`);
  }

  reactivateBranch(id: number): Observable<ApiResponse<any>> {
    return this.http.put<ApiResponse<any>>(`${this.apiUrl}/${id}/reactivate`, {});
  }

  assignManager(branchId: number, userId: string): Observable<ApiResponse<any>> {
    return this.http.post<ApiResponse<any>>(`${this.apiUrl}/${branchId}/managers`, { userId });
  }
}
