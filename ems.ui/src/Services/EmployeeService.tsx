import axios from "axios";
import { EmployeeGet, EmployeePost } from "../Models/Employee";

const api = "https://localhost:7256/api/";

export const GetEmployeesService = async (searchTerm: string) => {
  const response = await axios.get<EmployeeGet[]>(api + "Employee", {
    params: { searchTerm },
  });

  return response.data;
};

export const UserGetEmployeesService = async (searchTerm: string) => {
  const response = await axios.get<EmployeeGet[]>(api + "Employee/User", {
    params: { searchTerm },
  });

  return response.data;
};

export const UserPostEmployeesService = async (employeePost: EmployeePost) => {
  return await axios.post<EmployeePost>(api + "Employee", employeePost);
};

export const UserDeleteEmployeesService = async (id: string) => {
  const response = await axios.delete(`${api}Employee/${id}`);
  return response;
};

export const UserUpdateEmployeesService = async (
  employeePost: EmployeePost,
  id: string
) => {
  return await axios.put<EmployeePost>(`${api}Employee/${id}`, employeePost);
};
