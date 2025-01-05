import axios from "axios";
import { UserGet } from "../Models/User";

const api = "https://localhost:7256/api/";

export const UserGetService = async (searchTerm: string) => {
  const response = await axios.get<UserGet[]>(api + "account/GetAllUser", {
    params: { searchTerm },
  });

  return response.data;
};

export const UserDeleteService = async (id: string) => {
  const response = await axios.delete(`${api}account/${id}`);
  return response;
};

export const GetNumberOfUsersService = async () => {
  const response = await axios.get<number>(api + "account/GetNumberOfUsers");
  return response.data;
};
