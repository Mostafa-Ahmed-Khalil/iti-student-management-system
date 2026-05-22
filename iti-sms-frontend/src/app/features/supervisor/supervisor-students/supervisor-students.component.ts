import { Component, OnInit, OnDestroy, signal, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Subscription } from 'rxjs';

import { UserService } from '../../admin/user-management/user.service';
import { User, CreateUserRequest } from '../../admin/user-management/user.model';
import { ToastService } from '../../../core/toast.service';

import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { CardModule } from 'primeng/card';
import { DialogModule } from 'primeng/dialog';
import { InputTextModule } from 'primeng/inputtext';
import { PasswordModule } from 'primeng/password';
import { TooltipModule } from 'primeng/tooltip';

@Component({
  selector: 'app-supervisor-students',
  standalone: true,
  imports: [
    CommonModule, ReactiveFormsModule,
    TableModule, ButtonModule, CardModule, DialogModule,
    InputTextModule, PasswordModule, TooltipModule
  ],
  templateUrl: './supervisor-students.component.html',
  styleUrls: ['./supervisor-students.component.css']
})
export class SupervisorStudentsComponent implements OnInit, OnDestroy {
  private userService = inject(UserService);
  private toastService = inject(ToastService);
  private fb = inject(FormBuilder);
  private nameSub?: Subscription;

  students = signal<User[]>([]);
  isLoading = signal<boolean>(false);
  isSaving = signal<boolean>(false);
  isModalOpen = signal<boolean>(false);

  studentForm: FormGroup = this.fb.group({
    fullName: ['', Validators.required],
    email: [{ value: '', disabled: true }, [Validators.required, Validators.email]],
    password: [{ value: '', disabled: true }, [Validators.required, Validators.minLength(6)]]
  });

  ngOnInit() {
    this.loadStudents();
    this.setupAutoFill();
  }

  ngOnDestroy() {
    this.nameSub?.unsubscribe();
  }

  private setupAutoFill() {
    this.nameSub = this.studentForm.get('fullName')!.valueChanges.subscribe((fullName: string) => {
      if (!fullName?.trim()) {
        this.studentForm.patchValue({ email: '', password: '' }, { emitEvent: false });
        return;
      }

      const parts = fullName.trim().toLowerCase().split(/\s+/);
      const firstName = parts[0] ?? '';
      const secondName = parts[1] ?? '';

      // Email: a.rady@iti.edu
      const email = secondName
        ? `${firstName.charAt(0)}.${secondName}@iti.edu`
        : `${firstName}@iti.edu`;

      // Password: afaf + Rady + Pass123!
      const firstPart = firstName;                                              // all lowercase
      const secondPart = secondName
        ? secondName.charAt(0).toUpperCase() + secondName.slice(1)             // Capitalize first letter
        : '';
      const password = `${firstPart}${secondPart}Pass123!`;

      this.studentForm.patchValue({ email, password }, { emitEvent: false });
    });
  }

  loadStudents() {
    this.isLoading.set(true);
    this.userService.getUsers(1, 200, 'Student').subscribe({
      next: (users) => {
        this.students.set(users);
        this.isLoading.set(false);
      },
      error: () => {
        this.toastService.error('Failed to load students.');
        this.isLoading.set(false);
      }
    });
  }

  openAddModal() {
    this.studentForm.reset({ fullName: '', email: '', password: '' });
    this.isModalOpen.set(true);
  }

  closeModal() {
    this.isModalOpen.set(false);
    this.studentForm.reset();
  }

  saveStudent() {
    const rawValue = this.studentForm.getRawValue(); // includes disabled fields
    if (!rawValue.fullName || !rawValue.email || !rawValue.password) return;

    this.isSaving.set(true);

    const request: CreateUserRequest = {
      fullName: rawValue.fullName,
      email: rawValue.email,
      password: rawValue.password,
      roles: ['Student']
    };

    this.userService.createUser(request).subscribe({
      next: () => {
        this.toastService.success('Student created successfully.');
        this.loadStudents();
        this.closeModal();
        this.isSaving.set(false);
      },
      error: () => {
        this.toastService.error('Failed to create student.');
        this.isSaving.set(false);
      }
    });
  }
}
