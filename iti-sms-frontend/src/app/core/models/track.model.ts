export interface Track {
  id: number;
  name: string;
  startDate: string;
  branchId: number;
  isActive: boolean;
  supervisorId?: string;
  supervisorName?: string;
}
