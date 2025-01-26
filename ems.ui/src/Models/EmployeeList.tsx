import { EmployeeGet } from "./Employee";

export interface EmployeeListGet {
  id: string;
  name: string;
  employees: EmployeeGet[];
}

export interface EmployeeListPost {
  name: string;
  employeeIds: string[];
}
