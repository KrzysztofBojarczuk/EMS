import { AddressPost } from "./Address";

export interface TaskGet {
  id: string;
  name: string;
  description: string;
}

export interface TaskPost {
  name: string;
  description: string;
  address: AddressPost;
}
