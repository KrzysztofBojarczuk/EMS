import axios from "axios";
import { EmployeeGet } from "../Models/Employee";

const api = "https://localhost:7256/api/";

export const UserEmployeesService = async () => {
  const response = await axios.get<EmployeeGet[]>(api + "Employees/User");

  return response.data;
};
