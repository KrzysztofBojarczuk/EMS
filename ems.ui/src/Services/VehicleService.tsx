import axios from "axios";
import {
  PaginatedVehicleResponse,
  VehicleGet,
  VehiclePost,
} from "../Models/Vehicle";

const api = "https://localhost:7256/api/";

export const UserPostVehicleService = async (vehiclePost: VehiclePost) => {
  const response = await axios.post<VehiclePost>(api + "Vehicle", vehiclePost);
  return response.data;
};

export const UserGetVehicleService = async (
  pageNumber: number,
  pageSize: number,
  vehicleType?: string[],
  searchTerm?: string
) => {
  const url = `${api}Vehicle/User?pageNumber=${pageNumber}&pageSize=${pageSize}&searchTerm=${
    searchTerm ?? ""
  }${
    vehicleType
      ? `&${vehicleType.map((type) => `vehicleType=${type}`).join("&")}`
      : ""
  }`;

  const response = await axios.get<PaginatedVehicleResponse>(url);
  return response.data;
};

export const UserGetVehicleForTaskService = async (searchTerm?: string) => {
  const response = await axios.get<VehicleGet[]>(
    api + "Vehicle/UserVehiclesForTask",
    {
      params: { searchTerm },
    }
  );

  return response.data;
};

export const UserDeleteVehicleService = async (id: string) => {
  const response = await axios.delete(`${api}Vehicle/${id}`);
  return response.data;
};

export const UserUpdateVehicleService = async (
  vehiclePost: VehiclePost,
  id: string
) => {
  const response = await axios.put<VehiclePost>(
    `${api}Vehicle/${id}`,
    vehiclePost
  );
  return response.data;
};
