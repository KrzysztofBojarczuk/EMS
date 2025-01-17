import axios from "axios";
import { TaskGet, TaskPost } from "../Models/Task";

const api = "https://localhost:7256/api/";

export const GetTaskService = async (searchTerm: string) => {
  const response = await axios.get<TaskGet[]>(api + "Task", {
    params: { searchTerm },
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

export const UserDeleteTaskService = async (id: string) => {
  const response = await axios.delete(`${api}Task/${id}`);
  return response;
};

export const UserUpdateTaskService = async (taskPost: TaskPost, id: string) => {
  return await axios.put<TaskPost>(`${api}Task/${id}`, taskPost);
};
