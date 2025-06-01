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

export const UserGetTaskService = async (
  pageNumber: number,
  pageSize: number,
  searchTerm: string
) => {
  const response = await axios.get<PaginatedTaskResponse>(api + "Task/User", {
    params: { pageNumber, pageSize, searchTerm },
  });

  return response.data;
};

export const UserPostTaskService = async (taskPost: TaskPost) => {
  const response = await axios.post<TaskPost>(api + "Task", taskPost);
  return response.data;
};

export const DeleteTaskService = async (id: string) => {
  const response = await axios.delete(`${api}Task/${id}`);
  return response.data;
};

export const UserUpdateTaskService = async (taskPost: TaskPost, id: string) => {
  const response = await axios.put<TaskPost>(`${api}Task/${id}`, taskPost);
  return response.data;
};

export const UserUpdateTaskStatusService = async (
  id: string,
  status: number
) => {
  const response = await axios.patch(`${api}Task/${id}/status`, status, {
    headers: {
      "Content-Type": "application/json",
    },
  });
  return response.data;
};
