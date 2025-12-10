import axios from "axios";
import { PaginatedUserResponse, UserGet } from "../Models/User";

const api = "https://localhost:7256/api/";

export const GetAllUsersService = async (
  pageNumber: number,
  pageSize: number,
  searchTerm?: string
) => {
  const response = await axios.get<PaginatedUserResponse>(
    api + "user/GetAllUser",
    {
      params: { pageNumber, pageSize, searchTerm },
    }
  );
  return response.data;
};

export const GetNumberOfUsersService = async () => {
  const response = await axios.get<number>(api + "user/GetNumberOfUsers");
  return response.data;
};

export const DeleteUserService = async (id: string) => {
  const response = await axios.delete(`${api}user/${id}`);
  return response.data;
};
