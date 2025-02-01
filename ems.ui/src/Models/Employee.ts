export interface EmployeeGet {
  id: string;
  name: string;
  email: string;
  phone: string;
  salary: number;
}

export interface EmployeePost {
  name: string;
  email: string;
  phone: string;
  salary: number;
}

export interface PaginatedEmployeeResponse {
  employeeGet: EmployeeGet[];
  totalItems: number;
  totalPages: number;
  pageIndex: number;
}
