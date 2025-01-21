import { AddressGet, AddressPost } from "./Address";

export interface TaskGet {
  id: string;
  name: string;
  description: string;
}

export interface TaskPost {
  name: string;
  description: string;
  addressId: string;
}
