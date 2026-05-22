import { Component, OnInit, computed, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule, FormBuilder, Validators, FormGroup } from '@angular/forms';
import { BranchService } from './branch.service';
import { Branch } from './branch.model';
import { ToastService } from '../../../core/toast.service';

@Component({
  selector: 'app-branch-management',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule],
  templateUrl: './branch-management.component.html',
  styleUrls: ['./branch-management.component.scss']
})
export class BranchManagementComponent implements OnInit {
  private branchService = inject(BranchService);
  private fb = inject(FormBuilder);
  private toastService = inject(ToastService);

  branches = signal<Branch[]>([]);
  isLoading = signal<boolean>(false);
  
  isModalOpen = signal<boolean>(false);
  modalTitle = signal<string>('Add Branch');
  editingBranchId = signal<number | null>(null);

  branchForm: FormGroup = this.fb.group({
    name: ['', Validators.required],
    location: ['', Validators.required]
  });

  ngOnInit() {
    this.loadBranches();
  }

  loadBranches() {
    this.isLoading.set(true);
    this.branchService.getBranches().subscribe({
      next: (response) => {
        if (response.success) {
          this.branches.set(response.data);
        }
        this.isLoading.set(false);
      },
      error: () => {
        this.isLoading.set(false);
      }
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
}
