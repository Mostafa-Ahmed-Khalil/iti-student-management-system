export interface Branch {
  id: number;
  name: string;

  isActive: boolean;
  managerId?: string;
  managerName?: string;
}

export interface CreateBranchRequest {
  name: string;

}

export interface UpdateBranchRequest {
  name: string;

}
