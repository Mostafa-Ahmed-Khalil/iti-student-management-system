import { Component, OnInit, inject, signal, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { UserService } from './user.service';
import { User, CreateUserRequest } from './user.model';
import { ToastService } from '../../../core/toast.service';

import { TableModule } from 'primeng/table';
import { DialogModule } from 'primeng/dialog';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { PasswordModule } from 'primeng/password';
import { CheckboxModule } from 'primeng/checkbox';
import { CardModule } from 'primeng/card';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { ConfirmationService } from 'primeng/api';
import { TooltipModule } from 'primeng/tooltip';

@Component({
  selector: 'app-user-management',
  standalone: true,
  imports: [CommonModule, FormsModule, TableModule, DialogModule, ButtonModule,
    InputTextModule, PasswordModule, CheckboxModule, CardModule, ConfirmDialogModule, TooltipModule],
  providers: [ConfirmationService],
  templateUrl: './user-management.component.html',
  styleUrls: ['./user-management.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class UserManagementComponent implements OnInit {
  private readonly userService = inject(UserService);
  private readonly toastService = inject(ToastService);
  private readonly confirmationService = inject(ConfirmationService);

  users = signal<User[]>([]);
  isLoading = signal<boolean>(false);

  // --- Create ---
  showCreateModal = false;
  newUser: CreateUserRequest = { email: '', password: '', fullName: '', roles: [] };

  // --- Edit user info ---
  showEditModal = false;
  editingUser: User | null = null;
  editFullName = '';
  editEmail = '';

  // --- Edit roles ---
  showRolesModal = false;
  selectedUser: User | null = null;
  editingRoles: string[] = [];

  availableRoles = ['Admin', 'Branch Manager', 'Technical Supervisor', 'Instructor', 'Student'];

  ngOnInit() { this.loadUsers(); }

  loadUsers() {
    this.isLoading.set(true);
    this.userService.getUsers().subscribe({
      next: (users) => { this.users.set(users); this.isLoading.set(false); },
      error: () => { this.toastService.error('Failed to load users'); this.isLoading.set(false); }
    });
  }

  // ─── Create ────────────────────────────────────────────────────────────────
  openCreateModal() {
    this.newUser = { email: '', password: '', fullName: '', roles: [] };
    this.showCreateModal = true;
  }

  closeCreateModal() { this.showCreateModal = false; }

  toggleNewUserRole(role: string) {
    const i = this.newUser.roles.indexOf(role);
    i > -1 ? this.newUser.roles.splice(i, 1) : this.newUser.roles.push(role);
  }

  createUser() {
    if (!this.newUser.email || !this.newUser.password || !this.newUser.fullName || this.newUser.roles.length === 0) {
      this.toastService.error('Please fill all fields and select at least one role');
      return;
    }
    this.isLoading.set(true);
    this.toastService.loading('Creating user...');
    this.userService.createUser(this.newUser).subscribe({
      next: () => {
        this.toastService.clearLoading();
        this.toastService.success('User created successfully');
        this.closeCreateModal();
        this.loadUsers();
      },
      error: () => { this.toastService.clearLoading(); this.isLoading.set(false); }
    });
  }

  // ─── Edit user info ─────────────────────────────────────────────────────────
  openEditModal(user: User) {
    this.editingUser = user;
    this.editFullName = user.fullName;
    this.editEmail = user.email;
    this.showEditModal = true;
  }

  closeEditModal() { this.showEditModal = false; this.editingUser = null; }

  saveUser() {
    if (!this.editingUser || !this.editFullName.trim() || !this.editEmail.trim()) {
      this.toastService.error('Full name and email are required');
      return;
    }
    this.isLoading.set(true);
    this.toastService.loading('Saving user...');
    this.userService.updateUser(this.editingUser.id, this.editFullName, this.editEmail).subscribe({
      next: (updated) => {
        this.toastService.clearLoading();
        this.toastService.success('User updated successfully');
        // optimistic update
        this.users.update(list => list.map(u => u.id === updated.id ? { ...u, fullName: updated.fullName, email: updated.email } : u));
        this.closeEditModal();
        this.isLoading.set(false);
      },
      error: () => { this.toastService.clearLoading(); this.isLoading.set(false); }
    });
  }

  // ─── Delete ──────────────────────────────────────────────────────────────────
  confirmDelete(user: User) {
    this.confirmationService.confirm({
      message: `Are you sure you want to delete <strong>${user.fullName}</strong>? This cannot be undone.`,
      header: 'Delete User',
      icon: 'pi pi-exclamation-triangle',
      acceptButtonStyleClass: 'p-button-danger',
      accept: () => this.deleteUser(user)
    });
  }

  private deleteUser(user: User) {
    this.isLoading.set(true);
    this.toastService.loading('Deleting user...');
    this.userService.deleteUser(user.id).subscribe({
      next: () => {
        this.toastService.clearLoading();
        this.toastService.success('User deleted successfully');
        this.users.update(list => list.filter(u => u.id !== user.id));
        this.isLoading.set(false);
      },
      error: () => { this.toastService.clearLoading(); this.isLoading.set(false); }
    });
  }

  // ─── Edit roles ──────────────────────────────────────────────────────────────
  openRolesModal(user: User) {
    this.selectedUser = user;
    this.editingRoles = [...user.roles];
    this.showRolesModal = true;
  }

  closeRolesModal() { this.showRolesModal = false; this.selectedUser = null; }

  toggleEditUserRole(role: string) {
    const i = this.editingRoles.indexOf(role);
    i > -1 ? this.editingRoles.splice(i, 1) : this.editingRoles.push(role);
  }

  saveRoles() {
    if (!this.selectedUser) return;
    if (this.editingRoles.length === 0) {
      this.toastService.error('User must have at least one role');
      return;
    }
    this.isLoading.set(true);
    this.toastService.loading('Updating roles...');
    this.userService.assignRoles(this.selectedUser.id, this.editingRoles).subscribe({
      next: () => {
        this.toastService.clearLoading();
        this.toastService.success('Roles updated successfully');
        const uid = this.selectedUser!.id;
        this.users.update(list => list.map(u => u.id === uid ? { ...u, roles: [...this.editingRoles] } : u));
        this.closeRolesModal();
        this.isLoading.set(false);
      },
      error: () => { this.toastService.clearLoading(); this.isLoading.set(false); }
    });
  }
}
