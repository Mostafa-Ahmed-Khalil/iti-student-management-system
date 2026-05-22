import { Component, OnInit, computed, inject, signal, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule, FormBuilder, Validators, FormGroup } from '@angular/forms';
import { BranchService } from './branch.service';
import { Branch } from './branch.model';
import { ToastService } from '../../../core/toast.service';
import { UserService } from '../user-management/user.service';
import { User } from '../user-management/user.model';
import { AuthStore } from '../../../core/auth/auth.store';

import { TableModule } from 'primeng/table';
import { DialogModule } from 'primeng/dialog';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { SelectModule } from 'primeng/select';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { ConfirmationService } from 'primeng/api';
import { CardModule } from 'primeng/card';

@Component({
  selector: 'app-branch-management',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule, TableModule, DialogModule, ButtonModule, InputTextModule, SelectModule, ConfirmDialogModule, CardModule],
  providers: [ConfirmationService],
  templateUrl: './branch-management.component.html',
  styleUrls: ['./branch-management.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class BranchManagementComponent implements OnInit {
  private branchService = inject(BranchService);
  private fb = inject(FormBuilder);
  private toastService = inject(ToastService);
  private userService = inject(UserService);
  private authStore = inject(AuthStore);

  isAdmin = computed(() => this.authStore.activeRole() === 'Admin');

  branches = signal<Branch[]>([]);
  isLoading = signal<boolean>(false);
  
  isModalOpen = signal<boolean>(false);
  modalTitle = signal<string>('Add Branch');
  editingBranchId = signal<number | null>(null);

  isAssignManagerModalOpen = signal<boolean>(false);
  managers = signal<User[]>([]);
  selectedBranch = signal<Branch | null>(null);

  availableManagers = computed(() => {
    const selected = this.selectedBranch();
    const assignedManagerIds = new Set(
      this.branches()
        .filter(b => b.managerId && b.id !== selected?.id)
        .map(b => b.managerId!)
    );
    return this.managers().filter(m => !assignedManagerIds.has(m.id));
  });

  assignManagerForm: FormGroup = this.fb.group({
    userId: ['', Validators.required]
  });

  branchForm: FormGroup = this.fb.group({
    name: ['', Validators.required],
    location: ['', Validators.required]
  });

  ngOnInit() {
    this.loadBranches();
    if (this.isAdmin()) {
      this.loadManagers();
    }
  }

  loadManagers() {
    this.userService.getUsers().subscribe({
      next: (users) => {
        this.managers.set(users.filter(u => u.roles.includes('Branch Manager')));
      }
    });
  }

  loadBranches() {
    this.isLoading.set(true);
    this.branchService.getBranches().subscribe({
      next: (response) => {
        if (response.success) {
          // Merge: keep locally-known managerName if server doesn't return one yet
          const current = this.branches();
          const merged = response.data.map((b: Branch) => {
            const existing = current.find(c => c.id === b.id);
            return {
              ...b,
              managerName: b.managerName ?? existing?.managerName,
              managerId: b.managerId ?? existing?.managerId
            };
          });
          this.branches.set(merged);
        }
        this.isLoading.set(false);
      },
      error: () => { this.isLoading.set(false); }
    });
  }

  openAddModal() {
    this.modalTitle.set('Add Branch');
    this.editingBranchId.set(null);
    this.branchForm.reset();
    this.isModalOpen.set(true);
  }

  openEditModal(branch: Branch) {
    this.modalTitle.set('Edit Branch');
    this.editingBranchId.set(branch.id);
    this.branchForm.patchValue({
      name: branch.name,
      location: branch.location
    });
    this.isModalOpen.set(true);
  }

  closeModal() {
    this.isModalOpen.set(false);
  }

  openAssignManagerModal(branch: Branch) {
    this.selectedBranch.set(branch);
    this.editingBranchId.set(branch.id);
    this.assignManagerForm.reset();
    if (branch.managerId) {
      this.assignManagerForm.patchValue({ userId: branch.managerId });
    }
    this.isAssignManagerModalOpen.set(true);
  }

  closeAssignManagerModal() {
    this.isAssignManagerModalOpen.set(false);
  }

  assignManager() {
    if (this.assignManagerForm.invalid) return;

    const branchId = this.editingBranchId();
    const userId = this.assignManagerForm.value.userId;
    if (!branchId || !userId) return;

    // Find the selected manager's name for optimistic update
    const manager = this.managers().find(m => m.id === userId);

    this.isLoading.set(true);
    this.toastService.loading('Assigning manager...');

    this.branchService.assignManager(branchId, userId).subscribe({
      next: (response) => {
        this.toastService.clearLoading();
        if (response.success) {
          // Optimistically update the branch in the local signal so
          // the manager chip appears immediately (even before API restart)
          this.branches.update(list =>
            list.map(b =>
              b.id === branchId
                ? { ...b, managerId: userId, managerName: manager?.fullName ?? manager?.email }
                : b
            )
          );
          this.toastService.success('Manager assigned successfully');
          this.closeAssignManagerModal();
          // Also refresh from server in background
          this.loadBranches();
        }
      },
      error: () => {
        this.toastService.clearLoading();
        this.toastService.error('Failed to assign manager');
        this.isLoading.set(false);
      },
      complete: () => this.isLoading.set(false)
    });
  }

  saveBranch() {
    if (this.branchForm.invalid) return;

    this.isLoading.set(true);
    const request = this.branchForm.value;
    const id = this.editingBranchId();

    if (id) {
      this.branchService.updateBranch(id, request).subscribe({
        next: (response) => {
          if (response.success) {
            this.toastService.success('Branch updated successfully');
            this.loadBranches();
            this.closeModal();
          }
        },
        complete: () => this.isLoading.set(false)
      });
    } else {
      this.branchService.createBranch(request).subscribe({
        next: (response) => {
          if (response.success) {
            this.toastService.success('Branch created successfully');
            this.loadBranches();
            this.closeModal();
          }
        },
        complete: () => this.isLoading.set(false)
      });
    }
  }

  deactivateBranch(branch: Branch) {
    if (confirm(`Are you sure you want to deactivate ${branch.name}?`)) {
      this.isLoading.set(true);
      this.branchService.deleteBranch(branch.id).subscribe({
        next: (response) => {
          if (response.success) {
            this.toastService.success('Branch deactivated successfully');
            this.loadBranches();
          }
        },
        complete: () => this.isLoading.set(false)
      });
    }
  }

  reactivateBranch(branch: Branch) {
    if (confirm(`Are you sure you want to reactivate ${branch.name}?`)) {
      this.isLoading.set(true);
      this.branchService.reactivateBranch(branch.id).subscribe({
        next: (response) => {
          if (response.success) {
            this.toastService.success('Branch reactivated successfully');
            this.loadBranches();
          }
        },
        complete: () => this.isLoading.set(false)
      });
    }
  }
}
