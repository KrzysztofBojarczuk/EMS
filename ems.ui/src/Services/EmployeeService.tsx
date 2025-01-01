import axios from "axios";
import { EmployeeGet, EmployeePost } from "../Models/Employee";

const api = "https://localhost:7256/api/";

export const UserGetEmployeesService = async () => {
  const response = await axios.get<EmployeeGet[]>(api + "Employees/User");

  return response.data;
};

export const UserPostEmployeesService = async (employeePost: EmployeePost) => {
  return await axios.post<EmployeePost>(api + "Employees", employeePost);
};
