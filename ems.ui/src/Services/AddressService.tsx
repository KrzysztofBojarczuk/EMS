import axios from "axios";
import {
  AddressGet,
  AddressPost,
  PaginatedAddressResponse,
} from "../Models/Address";

const api = "https://localhost:7256/api/";

export const PostAddressService = async (addressPost: AddressPost) => {
  const response = await axios.post<AddressPost>(api + "Address", addressPost);
  return response.data;
};

export const GetUserAddressService = async (
  pageNumber: number,
  pageSize: number,
  searchTerm?: string
) => {
  const response = await axios.get<PaginatedAddressResponse>(
    api + "Address/User",
    {
      params: { pageNumber, pageSize, searchTerm },
    }
  );

  return response.data;
};

export const GetUserAddressForTaskService = async (searchTerm?: string) => {
  const response = await axios.get<AddressGet[]>(
    api + "Address/UserAddressesForTask",
    {
      params: { searchTerm },
    }
  );

  return response.data;
};

export const UpdateAddressService = async (
  id: string,
  addressPost: AddressPost
) => {
  const response = await axios.put<AddressPost>(
    `${api}Address/${id}`,
    addressPost
  );
  return response.data;
};

export const DeleteAddressService = async (id: string) => {
  const response = await axios.delete(`${api}Address/${id}`);
  return response.data;
};
