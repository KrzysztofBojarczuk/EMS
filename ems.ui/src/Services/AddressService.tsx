import axios from "axios";
import {
  AddressGet,
  AddressPost,
  PaginatedAddressesponse,
} from "../Models/Address";

const api = "https://localhost:7256/api/";

export const UserGetAddressService = async (
  pageNumber: number,
  pageSize: number,
  searchTerm?: string
) => {
  const response = await axios.get<PaginatedAddressesponse>(
    api + "Address/User",
    {
      params: { pageNumber, pageSize, searchTerm },
    }
  );

  return response.data;
};

export const UserPostAddressService = async (addressPost: AddressPost) => {
  return await axios.post<AddressPost>(api + "Address", addressPost);
};

export const UserGetAddressForTaskService = async (searchTerm?: string) => {
  const response = await axios.get<AddressGet[]>(
    api + "Address/UserAddressesForTask",
    {
      params: { searchTerm },
    }
  );

  return response.data;
};

export const UserDeleteAddressService = async (id: string) => {
  const response = await axios.delete(`${api}Address/${id}`);
  return response.data;
};

export const UserUpdateAddressService = async (
  addressPost: AddressPost,
  id: string
) => {
  const response = await axios.put<AddressPost>(
    `${api}Address/${id}`,
    addressPost
  );
  return response.data;
};
