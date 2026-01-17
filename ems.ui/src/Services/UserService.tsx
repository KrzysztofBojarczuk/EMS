import axios from "axios";
import { PaginatedUserResponse, UserGet } from "../Models/User";

const api = "https://localhost:7256/api/";

export const GetAllUsersService = async (
  pageNumber: number,
  pageSize: number,
  searchTerm?: string,
  sortOrder?: string | null,
) => {
  const params = new URLSearchParams();

  params.append("pageNumber", pageNumber.toString());
  params.append("pageSize", pageSize.toString());

  if (searchTerm?.trim()) params.append("searchTerm", searchTerm.trim());

  if (sortOrder) params.append("sortOrder", sortOrder);

  const response = await axios.get<PaginatedUserResponse>(
    `${api}User/GetAllUser?${params.toString()}`,
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
