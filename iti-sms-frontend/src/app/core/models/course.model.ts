export interface Course {
  id: number;
  name: string;
  trackId: number;
  instructorId?: string;
  instructorName?: string;
  lectureHours: number;
  labHours: number;
  numberOfLectures: number;
  numberOfLabs: number;
  isActive: boolean;
}
