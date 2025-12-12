export interface EmployeeGet {
  id: string;
  name: string;
  email: string;
  phone: string;
  salary: number;
  age: number;
  employmentDate: string;
  medicalCheckValidUntil: string;
}

export interface EmployeePost {
  name: string;
  email: string;
  phone: string;
  salary: number;
  age: number;
  employmentDate: string;
  medicalCheckValidUntil: string;
}

export interface PaginatedEmployeeResponse {
  employeeGet: EmployeeGet[];
  totalItems: number;
  totalPages: number;
  pageIndex: number;
}
