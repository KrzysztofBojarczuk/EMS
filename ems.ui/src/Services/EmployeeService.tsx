import axios from "axios";
import {
  EmployeeGet,
  EmployeePost,
  PaginatedEmployeeResponse,
} from "../Models/Employee";
import { EmployeeListGet, EmployeeListPost } from "../Models/EmployeeList";

const api = "https://localhost:7256/api/";

export const PostEmployeesService = async (employeePost: EmployeePost) => {
  const response = await axios.post<EmployeePost>(
    api + "Employee",
    employeePost,
  );
  return response.data;
};

export const PostEmployeeListService = async (
  employeeListPost: EmployeeListPost,
) => {
  const response = await axios.post<EmployeeListPost>(
    api + "Employee/AddEmployeeList",
    employeeListPost,
  );
  return response.data;
};

export const GetUserEmployeesService = async (
  pageNumber: number,
  pageSize: number,
  searchTerm?: string,
  sortOrder?: string | null,
) => {
  const response = await axios.get<PaginatedEmployeeResponse>(
    api + "Employee/User",
    {
      params: { pageNumber, pageSize, searchTerm, sortOrder },
    },
  );
  return response.data;
};

export const GetAllEmployeesService = async (
  pageNumber: number,
  pageSize: number,
  searchTerm?: string,
  sortOrder?: string | null,
) => {
  const response = await axios.get<PaginatedEmployeeResponse>(
    api + "Employee",
    {
      params: { pageNumber, pageSize, searchTerm, sortOrder },
    },
  );
  return response.data;
};

export const GetUserListEmployeesService = async (searchTerm: string) => {
  const response = await axios.get<EmployeeListGet[]>(
    api + "Employee/UserEmployeeList",
    {
      params: { searchTerm },
    },
  );
  return response.data;
};

export const GetUserListForTaskEmployeesService = async (
  searchTerm: string,
) => {
  const response = await axios.get<EmployeeListGet[]>(
    api + "Employee/UserEmployeeListForTask",
    {
      params: { searchTerm },
    },
  );
  return response.data;
};

export const GetUserEmployeesForListAddService = async (
  searchTerm?: string,
) => {
  const response = await axios.get<EmployeeGet[]>(
    api + "Employee/UserListAdd",
    {
      params: { searchTerm },
    },
  );

  return response.data;
};

export const GetUserEmployeesForListUpdateService = async (
  id: string,
  searchTerm: string,
) => {
  const response = await axios.get<EmployeeGet[]>(
    api + `Employee/UserListUpdate/${id}`,
    { params: { searchTerm } },
  );
  return response.data;
};

export const UpdateEmployeesService = async (
  id: string,
  employeePost: EmployeePost,
) => {
  const response = await axios.put<EmployeePost>(
    `${api}Employee/${id}`,
    employeePost,
  );
  return response.data;
};

export const UpdateEmployeeListService = async (
  id: string,
  employeeListPost: EmployeeListPost,
) => {
  const response = await axios.put(
    `${api}Employee/EmployeeList/${id}`,
    employeeListPost,
  );
  return response.data;
};

export const DeleteEmployeesService = async (id: string) => {
  const response = await axios.delete(`${api}Employee/${id}`);
  return response.data;
};

export const DeleteEmployeesListService = async (id: string) => {
  const response = await axios.delete(`${api}Employee/EmployeeList/${id}`);
  return response.data;
};
