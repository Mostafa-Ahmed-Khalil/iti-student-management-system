import { Component, OnInit, signal, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';

import { CourseService } from '../../../core/services/course.service';
import { UserService } from '../../admin/user-management/user.service';
import { Course } from '../../../core/models/course.model';
import { User } from '../../admin/user-management/user.model';
import { ToastService } from '../../../core/toast.service';

import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { CardModule } from 'primeng/card';
import { DialogModule } from 'primeng/dialog';
import { InputTextModule } from 'primeng/inputtext';
import { SelectModule } from 'primeng/select';
import { MultiSelectModule } from 'primeng/multiselect';
import { InputNumberModule } from 'primeng/inputnumber';

@Component({
  selector: 'app-course-management',
  standalone: true,
  imports: [
    CommonModule, RouterModule, ReactiveFormsModule,
    TableModule, ButtonModule, CardModule, DialogModule,
    InputTextModule, SelectModule, MultiSelectModule, InputNumberModule
  ],
  templateUrl: './course-management.component.html',
  styleUrls: ['./course-management.component.css']
})
export class CourseManagementComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private courseService = inject(CourseService);
  private userService = inject(UserService);
  private toastService = inject(ToastService);
  private fb = inject(FormBuilder);

  trackId = signal<number>(0);
  courses = signal<Course[]>([]);
  instructors = signal<User[]>([]);
  isLoading = signal<boolean>(false);
  
  isModalOpen = signal<boolean>(false);
  isSaving = signal<boolean>(false);
  modalTitle = signal<string>('Add Course');
  editingCourseId = signal<number | null>(null);

  courseForm: FormGroup = this.fb.group({
    name: ['', Validators.required],
    lectureHours: [0, [Validators.required, Validators.min(0)]],
    labHours: [0, [Validators.required, Validators.min(0)]],
    lecturerId: ['', Validators.required],
    labAssistantIds: [[]]
  });

  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      const id = Number(params.get('id'));
      if (id) {
        this.trackId.set(id);
        this.loadCourses(id);
      }
    });
    this.loadInstructors();
  }

  loadCourses(trackId: number) {
    this.isLoading.set(true);
    this.courseService.getCoursesByTrack(trackId).subscribe({
      next: (res) => {
        if (res.success) {
          const sortedCourses = res.data.sort((a, b) => a.name.localeCompare(b.name));
          this.courses.set(sortedCourses);
        }
        this.isLoading.set(false);
      },
      error: () => {
        this.toastService.error('Failed to load courses.');
        this.isLoading.set(false);
      }
    });
  }

  loadInstructors() {
    this.userService.getUsers(1, 200, 'Instructor').subscribe({
      next: (users) => {
        this.instructors.set(users);
      },
      error: () => {
        this.toastService.error('Failed to load instructors.');
      }
    });
  }

  openAddModal() {
    this.modalTitle.set('Add Course');
    this.editingCourseId.set(null);
    this.courseForm.reset({ lectureHours: 0, labHours: 0 });
    this.isModalOpen.set(true);
  }

  openEditModal(course: Course) {
    this.modalTitle.set('Edit Course');
    this.editingCourseId.set(course.id);
    this.courseForm.patchValue({
      name: course.name,
      lectureHours: course.lectureHours,
      labHours: course.labHours,
      lecturerId: course.lecturerId,
      labAssistantIds: course.labAssistants?.map(a => a.id) || []
    });
    this.isModalOpen.set(true);
  }

  closeModal() {
    this.isModalOpen.set(false);
    this.courseForm.reset();
  }

  saveCourse() {
    if (this.courseForm.invalid) return;

    this.isSaving.set(true);
    const courseData = this.courseForm.value;
    const currentTrackId = this.trackId();
    const editingId = this.editingCourseId();

    if (editingId) {
      this.courseService.updateCourse(editingId, { ...courseData, isActive: true }).subscribe({
        next: (res) => {
          if (res.success) {
            this.toastService.success('Course updated successfully.');
            this.loadCourses(currentTrackId);
            this.closeModal();
          }
          this.isSaving.set(false);
        },
        error: () => {
          this.toastService.error('Failed to update course.');
          this.isSaving.set(false);
        }
      });
    } else {
      this.courseService.createCourse(currentTrackId, courseData).subscribe({
        next: (res) => {
          if (res.success) {
            this.toastService.success('Course created successfully.');
            this.loadCourses(currentTrackId);
            this.closeModal();
          }
          this.isSaving.set(false);
        },
        error: () => {
          this.toastService.error('Failed to create course.');
          this.isSaving.set(false);
        }
      });
    }
  }

  deactivateCourse(course: Course) {
    if (confirm(`Are you sure you want to deactivate ${course.name}?`)) {
      this.courseService.deleteCourse(course.id).subscribe({
        next: (res) => {
          if (res.success) {
            this.toastService.success('Course deactivated successfully.');
            this.loadCourses(this.trackId());
          }
        },
        error: () => this.toastService.error('Failed to deactivate course.')
      });
    }
  }
}
