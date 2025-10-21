import axios from "axios";
import { PaginatedUserResponse, UserGet } from "../Models/User";

const api = "https://localhost:7256/api/";

export const UserGetService = async (
  pageNumber: number,
  pageSize: number,
  searchTerm: string
) => {
  const response = await axios.get<PaginatedUserResponse>(
    api + "user/GetAllUser",
    {
      params: { pageNumber, pageSize, searchTerm },
    }
  );
  return response.data;
};

export const UserDeleteService = async (id: string) => {
  const response = await axios.delete(`${api}user/${id}`);
  return response.data;
};

export const GetNumberOfUsersService = async () => {
  const response = await axios.get<number>(api + "user/GetNumberOfUsers");
  return response.data;
};
