export interface LabEvaluationDto {
  id: number;
  studentId: string;
  studentName: string;
  labNumber: number;
  score: number;
  techNotes: string;
  softSkillsNotes?: string | null;
  evaluatorId?: string;
  evaluatorName?: string;
}

export interface LabEvaluationGridDto {
  courseId: number;
  numberOfLabs: number;
  students: { id: string; fullName: string; email: string }[];
  evaluations: LabEvaluationDto[];
}

export interface UpsertLabEvaluationRequest {
  studentId: string;
  labNumber: number;
  score: number;
  techNotes: string;
  softSkillsNotes: string;
}

export interface InstructorCourseDto {
  id: number;
  name: string;
  trackId: number;
  trackName: string;
  numberOfLabs: number;
  numberOfLectures: number;
  labHours: number;
  lectureHours: number;
}
