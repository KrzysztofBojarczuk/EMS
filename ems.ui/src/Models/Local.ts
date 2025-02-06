export interface LocalGet {
  id: string;
  description: string;
  localNumber: number;
  surface: number;
  needsRepair: boolean;
}

export interface LocalPost {
  description: string;
  localNumber: number;
  surface: number;
  needsRepair: boolean;
}

export interface PaginatedLocalResponse {
  localGet: LocalGet[];
  totalItems: number;
  totalPages: number;
  pageIndex: number;
}
