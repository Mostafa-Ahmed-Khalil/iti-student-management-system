import { Component, OnInit, signal, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { InstructorService } from '../../../core/services/instructor.service';
import { InstructorCourseDto } from '../../../core/models/lab-evaluation.model';
import { ToastService } from '../../../core/toast.service';
import { CardModule } from 'primeng/card';
import { ButtonModule } from 'primeng/button';

@Component({
  selector: 'app-instructor-courses',
  standalone: true,
  imports: [CommonModule, RouterModule, CardModule, ButtonModule],
  templateUrl: './instructor-courses.component.html',
  styleUrls: ['./instructor-courses.component.css']
})
export class InstructorCoursesComponent implements OnInit {
  private instructorService = inject(InstructorService);
  private toastService = inject(ToastService);

  courses = signal<InstructorCourseDto[]>([]);
  isLoading = signal(false);

  ngOnInit() {
    this.loadCourses();
  }

  loadCourses() {
    this.isLoading.set(true);
    this.instructorService.getMyCourses().subscribe({
      next: (courses) => {
        this.courses.set(courses);
        this.isLoading.set(false);
      },
      error: () => {
        this.toastService.error('Failed to load your courses.');
        this.isLoading.set(false);
      }
    });
  }
}
