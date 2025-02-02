import axios from "axios";
import { PaginatedTaskResponse, TaskGet, TaskPost } from "../Models/Task";

const api = "https://localhost:7256/api/";

export const GetTaskService = async (
  pageNumber: number,
  pageSize: number,
  searchTerm: string
) => {
  const response = await axios.get<PaginatedTaskResponse>(api + "Task", {
    params: { pageNumber, pageSize, searchTerm },
  });

  return response.data;
};

export const UserGetTaskService = async (searchTerm: string) => {
  const response = await axios.get<TaskGet[]>(api + "Task/User", {
    params: { searchTerm },
  });

  return response.data;
};

export const UserPostTaskService = async (taskPost: TaskPost) => {
  return await axios.post<TaskPost>(api + "Task", taskPost);
};

export const DeleteTaskService = async (id: string) => {
  const response = await axios.delete(`${api}Task/${id}`);
  return response;
};

export const UserUpdateTaskService = async (taskPost: TaskPost, id: string) => {
  return await axios.put<TaskPost>(`${api}Task/${id}`, taskPost);
};

export const UserUpdateTaskStatusService = async (
  id: string,
  status: number
) => {
  return await axios.patch(`${api}Task/${id}/status`, status, {
    headers: {
      "Content-Type": "application/json",
    },
  });
};
