import { LocalPost, PaginatedLocalResponse } from "../Models/Local";
import axios from "axios";

const api = "https://localhost:7256/api/";

export const UserGetLocalService = async (
  pageNumber: number,
  pageSize: number,
  searchTerm: string
) => {
  const response = await axios.get<PaginatedLocalResponse>(api + "Local", {
    params: { pageNumber, pageSize, searchTerm },
  });

  return response.data;
};

export const UserPostLocalService = async (localPost: LocalPost) => {
  console.log(localPost);
  return await axios.post<LocalPost>(api + "Local", localPost);
};
