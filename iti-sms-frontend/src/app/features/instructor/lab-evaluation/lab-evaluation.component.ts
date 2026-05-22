import { Component, OnInit, signal, inject, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { InstructorService } from '../../../core/services/instructor.service';
import { LabEvaluationGridDto, LabEvaluationDto } from '../../../core/models/lab-evaluation.model';
import { ToastService } from '../../../core/toast.service';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { TabsModule } from 'primeng/tabs';
import { TextareaModule } from 'primeng/textarea';
import { TooltipModule } from 'primeng/tooltip';

@Component({
  selector: 'app-lab-evaluation',
  standalone: true,
  imports: [
    CommonModule, RouterModule, ReactiveFormsModule,
    ButtonModule, DialogModule, TabsModule,
    TextareaModule, TooltipModule
  ],
  templateUrl: './lab-evaluation.component.html',
  styleUrls: ['./lab-evaluation.component.css']
})
export class LabEvaluationComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private instructorService = inject(InstructorService);
  private toastService = inject(ToastService);
  private fb = inject(FormBuilder);

  courseId = signal(0);
  trackId = signal(0);
  grid = signal<LabEvaluationGridDto | null>(null);
  isLoading = signal(false);

  // Notes dialog state
  isNotesOpen = signal(false);
  savingCell = signal<string | null>(null);  // "studentId-labNumber"
  activeStudentId = signal('');
  activeLabNumber = signal(0);
  activeStudentName = signal('');

  notesForm: FormGroup = this.fb.group({
    score: [null, [Validators.required, Validators.min(0), Validators.max(1)]],
    techNotes: ['', Validators.required],
    softSkillsNotes: ['', Validators.required]
  });

  labNumbers = computed(() => {
    const n = this.grid()?.numberOfLabs ?? 0;
    return Array.from({ length: n }, (_, i) => i + 1);
  });

  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      this.courseId.set(Number(params.get('courseId')));
    });
    this.route.queryParamMap.subscribe(params => {
      this.trackId.set(Number(params.get('trackId')));
      this.loadGrid();
    });
  }

  loadGrid() {
    if (!this.courseId() || !this.trackId()) return;
    this.isLoading.set(true);
    this.instructorService.getLabEvaluations(this.courseId(), this.trackId()).subscribe({
      next: (grid) => {
        this.grid.set(grid);
        this.isLoading.set(false);
      },
      error: () => {
        this.toastService.error('Failed to load evaluation grid.');
        this.isLoading.set(false);
      }
    });
  }

  getEval(studentId: string, labNumber: number): LabEvaluationDto | undefined {
    return this.grid()?.evaluations.find(
      e => e.studentId === studentId && e.labNumber === labNumber
    );
  }

  openNotes(studentId: string, studentName: string, labNumber: number) {
    this.activeStudentId.set(studentId);
    this.activeStudentName.set(studentName);
    this.activeLabNumber.set(labNumber);

    const existing = this.getEval(studentId, labNumber);
    this.notesForm.reset({
      score: existing?.score ?? null,
      techNotes: existing?.techNotes ?? '',
      softSkillsNotes: existing?.softSkillsNotes ?? ''
    });
    this.isNotesOpen.set(true);
  }

  saveNotes() {
    if (this.notesForm.invalid) return;
    const { score, techNotes, softSkillsNotes } = this.notesForm.value;
    const key = `${this.activeStudentId()}-${this.activeLabNumber()}`;
    this.savingCell.set(key);

    this.instructorService.upsertLabEvaluation(this.courseId(), {
      studentId: this.activeStudentId(),
      labNumber: this.activeLabNumber(),
      score,
      techNotes,
      softSkillsNotes
    }).subscribe({
      next: () => {
        this.toastService.success('Evaluation saved.');
        this.isNotesOpen.set(false);
        this.loadGrid();
        this.savingCell.set(null);
      },
      error: () => {
        this.toastService.error('Failed to save evaluation.');
        this.savingCell.set(null);
      }
    });
  }

  cellKey(studentId: string, labNumber: number): string {
    return `${studentId}-${labNumber}`;
  }

  isSavingCell(studentId: string, labNumber: number): boolean {
    return this.savingCell() === this.cellKey(studentId, labNumber);
  }
}
