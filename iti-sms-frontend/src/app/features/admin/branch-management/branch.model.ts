export interface Branch {
  id: number;
  name: string;
  location: string;
  isActive: boolean;
  createdAt: string;
}

export interface CreateBranchRequest {
  name: string;
  location: string;
}

export interface UpdateBranchRequest {
  name: string;
  location: string;
}
