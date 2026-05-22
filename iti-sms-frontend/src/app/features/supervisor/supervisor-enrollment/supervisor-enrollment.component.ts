import { Component, OnInit, signal, inject, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';

import { UserService } from '../../admin/user-management/user.service';
import { EnrollmentService } from '../../../core/services/enrollment.service';
import { TrackService } from '../../../core/services/track.service';
import { ToastService } from '../../../core/toast.service';
import { User } from '../../admin/user-management/user.model';
import { Enrollment } from '../../../core/models/enrollment.model';

import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { CardModule } from 'primeng/card';
import { MultiSelectModule } from 'primeng/multiselect';
import { TooltipModule } from 'primeng/tooltip';

@Component({
  selector: 'app-supervisor-enrollment',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    FormsModule,
    TableModule,
    ButtonModule,
    CardModule,
    MultiSelectModule,
    TooltipModule
  ],
  templateUrl: './supervisor-enrollment.component.html',
  styleUrls: ['./supervisor-enrollment.component.css']
})
export class SupervisorEnrollmentComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private userService = inject(UserService);
  private enrollmentService = inject(EnrollmentService);
  private trackService = inject(TrackService);
  private toastService = inject(ToastService);

  trackId = signal<number>(0);
  trackName = signal<string>('');
  
  allStudents = signal<User[]>([]);
  enrolledStudents = signal<Enrollment[]>([]);
  
  selectedStudentIds = signal<string[]>([]);
  
  isLoading = signal<boolean>(false);
  isActionLoading = signal<boolean>(false);

  availableStudents = computed(() => {
    const enrolledIds = new Set(this.enrolledStudents().map(e => e.id));
    return this.allStudents().filter(s => !enrolledIds.has(s.id));
  });

  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      const id = Number(params.get('trackId'));
      if (id) {
        this.trackId.set(id);
        this.loadData();
      }
    });
  }

  loadData() {
    this.isLoading.set(true);
    
    this.trackService.getMyTracks().subscribe({
      next: (res) => {
        if (res.success) {
          const match = res.data.find(t => t.id === this.trackId());
          if (match) {
            this.trackName.set(match.name);
          }
        }
      }
    });

    this.enrollmentService.getEnrolledStudents(this.trackId()).subscribe({
      next: (enrolled) => {
        this.enrolledStudents.set(enrolled);
        
        this.userService.getUsers(1, 1000, 'Student').subscribe({
          next: (all) => {
            this.allStudents.set(all);
            this.isLoading.set(false);
          },
          error: () => {
            this.toastService.error('Failed to load system students.');
            this.isLoading.set(false);
          }
        });
      },
      error: () => {
        this.toastService.error('Failed to load enrolled students.');
        this.isLoading.set(false);
      }
    });
  }

  enrollStudents() {
    const studentIds = this.selectedStudentIds();
    if (!studentIds || studentIds.length === 0) return;

    this.isActionLoading.set(true);
    this.enrollmentService.enrollStudents(this.trackId(), studentIds).subscribe({
      next: () => {
        this.toastService.success('Students enrolled successfully.');
        this.selectedStudentIds.set([]);
        this.isActionLoading.set(false);
        this.loadData();
      },
      error: (err: any) => {
        this.toastService.error(err.error?.message || 'Failed to enroll students.');
        this.isActionLoading.set(false);
      }
    });
  }

  unenrollStudent(studentId: string) {
    this.isActionLoading.set(true);
    this.enrollmentService.unenrollStudent(this.trackId(), studentId).subscribe({
      next: () => {
        this.toastService.success('Student removed from track.');
        this.isActionLoading.set(false);
        this.loadData();
      },
      error: (err: any) => {
        this.toastService.error(err.error?.message || 'Failed to remove student.');
        this.isActionLoading.set(false);
      }
    });
  }
}
