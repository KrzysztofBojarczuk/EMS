export interface PaginatedList<T> {
  totalItems: number;
  pageIndex: number;
  totalPages: number;
  items: T[];
}
