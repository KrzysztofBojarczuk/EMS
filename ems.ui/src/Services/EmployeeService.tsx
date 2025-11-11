import axios from "axios";
import {
  EmployeeGet,
  EmployeePost,
  PaginatedEmployeeResponse,
} from "../Models/Employee";
import { EmployeeListGet, EmployeeListPost } from "../Models/EmployeeList";

const api = "https://localhost:7256/api/";

export const GetEmployeesService = async (
  pageNumber: number,
  pageSize: number,
  searchTerm?: string
) => {
  const response = await axios.get<PaginatedEmployeeResponse>(
    api + "Employee",
    {
      params: { pageNumber, pageSize, searchTerm },
    }
  );

  return response.data;
};

export const UserGetEmployeesService = async (
  pageNumber: number,
  pageSize: number,
  searchTerm?: string,
  sortOrderSalary?: string | null
) => {
  const response = await axios.get<PaginatedEmployeeResponse>(
    api + "Employee/User",
    {
      params: { pageNumber, pageSize, searchTerm, sortOrderSalary },
    }
  );

  return response.data;
};

export const UserGetEmployeesForListService = async (searchTerm?: string) => {
  const response = await axios.get<EmployeeGet[]>(api + "Employee/UserList", {
    params: { searchTerm },
  });

  return response.data;
};

export const PostEmployeesService = async (employeePost: EmployeePost) => {
  return await axios.post<EmployeePost>(api + "Employee", employeePost);
};

export const DeleteEmployeesService = async (id: string) => {
  const response = await axios.delete(`${api}Employee/${id}`);
  return response.data;
};

export const UpdateEmployeesService = async (
  id: string,
  employeePost: EmployeePost
) => {
  const response = await axios.put<EmployeePost>(
    `${api}Employee/${id}`,
    employeePost
  );
  return response.data;
};

export const UserGetListEmployeesService = async (searchTerm: string) => {
  const response = await axios.get<EmployeeListGet[]>(
    api + "Employee/UserEmployeeList",
    {
      params: { searchTerm },
    }
  );

  return response.data;
};

export const UserGetListForTaskEmployeesService = async (
  searchTerm: string
) => {
  const response = await axios.get<EmployeeListGet[]>(
    api + "Employee/UserEmployeeListForTask",
    {
      params: { searchTerm },
    }
  );

  return response.data;
};

export const PostListEmployeesService = async (
  employeeListPost: EmployeeListPost
) => {
  return await axios.post<EmployeeListPost>(
    api + "Employee/AddEmployeeList",
    employeeListPost
  );
};

export const DeleteEmployeesListService = async (id: string) => {
  const response = await axios.delete(`${api}Employee/EmployeeList/${id}`);
  return response.data;
};
