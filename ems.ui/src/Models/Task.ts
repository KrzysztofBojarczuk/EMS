import { StatusOfTask } from "../Enum/StatusOfTask";
import { AddressGet, AddressPost } from "./Address";

export interface TaskGet {
  id: string;
  name: string;
  description: string;
  status: StatusOfTask;
  startDate: string;
  endDate: string;
  address: AddressGet;
}

export interface TaskPost {
  name: string;
  description: string;
  employeeListIds: string[];
  startDate: string;
  endDate: string;
  addressId: string;
}
