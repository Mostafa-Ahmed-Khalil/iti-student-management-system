export interface Course {
  id: number;
  name: string;
  trackId: number;
  lecturerId?: string;
  lecturerName?: string;
  labAssistants: { id: string; name: string }[];
  lectureHours: number;
  labHours: number;
  numberOfLectures: number;
  numberOfLabs: number;
  isActive: boolean;
}
