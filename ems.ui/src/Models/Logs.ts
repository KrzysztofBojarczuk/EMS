export interface LogGet {
  id: string;
  userId?: string;
  username?: string;
  action: string;
  createdAt: string;
}

export interface PaginatedLogResponse {
  logs: LogGet[];
  totalItems: number;
  totalPages: number;
  pageIndex: number;
}
