import axios from "axios";
import { PaginatedTaskResponse, TaskGet, TaskPost } from "../Models/Task";
import { StatusOfTask } from "../Enum/StatusOfTask";

const api = "https://localhost:7256/api/";

export const PostTaskService = async (taskPost: TaskPost) => {
  const response = await axios.post<TaskPost>(api + "Task", taskPost);
  return response.data;
};

export const GetUserTasksService = async (
  pageNumber: number,
  pageSize: number,
  searchTerm?: string,
  statusOfTask?: string[],
  sortOrder?: string | null,
): Promise<PaginatedTaskResponse> => {
  const params = new URLSearchParams();

  params.append("pageNumber", pageNumber.toString());
  params.append("pageSize", pageSize.toString());

  if (searchTerm?.trim()) params.append("searchTerm", searchTerm.trim());

  if (statusOfTask && statusOfTask.length > 0)
    statusOfTask.forEach((status) =>
      params.append("statusOfTask", status.toString()),
    );

  if (sortOrder) params.append("sortOrder", sortOrder);

  const response = await axios.get<PaginatedTaskResponse>(
    `${api}Task/User?${params.toString()}`,
  );
  return response.data;
};

export const GetAllTasksService = async (
  pageNumber: number,
  pageSize: number,
  searchTerm?: string,
  statusOfTask?: string[],
  sortOrder?: string | null,
) => {
  const params = new URLSearchParams();

  params.append("pageNumber", pageNumber.toString());
  params.append("pageSize", pageSize.toString());

  if (searchTerm?.trim()) params.append("searchTerm", searchTerm.trim());

  if (statusOfTask && statusOfTask.length > 0)
    statusOfTask.forEach((status) =>
      params.append("statusOfTask", status.toString()),
    );

  if (sortOrder) params.append("sortOrder", sortOrder);

  const response = await axios.get<PaginatedTaskResponse>(
    `${api}Task?${params.toString()}`,
  );
  return response.data;
};

export const UpdateTaskService = async (id: string, taskPost: TaskPost) => {
  const response = await axios.put<TaskPost>(`${api}Task/${id}`, taskPost);
  return response.data;
};

export const UpdateTaskStatusService = async (id: string, status: number) => {
  const response = await axios.patch(`${api}Task/${id}/status`, status, {
    headers: {
      "Content-Type": "application/json",
    },
  });
  return response.data;
};

export const DeleteTaskService = async (id: string) => {
  const response = await axios.delete(`${api}Task/${id}`);
  return response.data;
};
