import axios from "axios";
import { AddressGet, AddressPost } from "../Models/Address";

const api = "https://localhost:7256/api/";

export const UserGetAddressService = async (searchTerm: string) => {
  const response = await axios.get<AddressGet[]>(api + "Address/User", {
    params: { searchTerm },
  });

  return response.data;
};

export const UserPostAddressService = async (addressPost: AddressPost) => {
  return await axios.post<AddressPost>(api + "Adress", addressPost);
};

export const UserDeleteAddressService = async (id: string) => {
  const response = await axios.delete(`${api}Address/${id}`);
  return response;
};

export const UserUpdateAddressService = async (
  addressPost: AddressPost,
  id: string
) => {
  return await axios.put<AddressPost>(`${api}Address/${id}`, addressPost);
};
