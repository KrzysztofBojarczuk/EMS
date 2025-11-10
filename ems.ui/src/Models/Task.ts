import { StatusOfTask } from "../Enum/StatusOfTask";
import { AddressGet, AddressPost } from "./Address";
import { EmployeeListGet } from "./EmployeeList";
import { VehicleGet } from "./Vehicle";

export interface TaskGet {
  id: string;
  name: string;
  description: string;
  status: StatusOfTask;
  startDate: string;
  endDate: string;
  address: AddressGet;
  employeeLists: EmployeeListGet[];
  vehicles: VehicleGet[];
}

export interface TaskPost {
  name: string;
  description: string;
  employeeListIds: string[];
  vehicleIds: string[];
  startDate: string;
  endDate: string;
  addressId: string;
}

export interface PaginatedTaskResponse {
  taskGet: TaskGet[];
  totalItems: number;
  totalPages: number;
  pageIndex: number;
}
