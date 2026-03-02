import axios from "axios";
import {
  PaginatedVehicleResponse,
  UserVehiclesStats,
  VehicleGet,
  VehiclePost,
} from "../Models/Vehicle";

const api = "https://localhost:7256/api/";

export const PostVehicleService = async (vehiclePost: VehiclePost) => {
  const response = await axios.post<VehiclePost>(api + "Vehicle", vehiclePost);
  return response.data;
};

export const GetUserVehiclesService = async (
  pageNumber: number,
  pageSize: number,
  searchTerm?: string,
  vehicleType?: string[],
  dateFrom?: Date | null,
  dateTo?: Date | null,
  sortOrder?: string | null,
) => {
  const params = new URLSearchParams();

  params.append("pageNumber", pageNumber.toString());
  params.append("pageSize", pageSize.toString());

  if (searchTerm) params.append("searchTerm", searchTerm);

  if (vehicleType && vehicleType.length > 0) {
    vehicleType.forEach((type) => params.append("vehicleType", type));
  }

  if (dateFrom) params.append("dateFrom", dateFrom.toISOString());
  if (dateTo) params.append("dateTo", dateTo.toISOString());

  if (sortOrder) params.append("sortOrder", sortOrder);

  const response = await axios.get<PaginatedVehicleResponse>(
    `${api}Vehicle/User?${params.toString()}`,
  );
  return response.data;
};

export const GetUserVehicleForTaskAddService = async (searchTerm?: string) => {
  const response = await axios.get<VehicleGet[]>(
    api + "Vehicle/UserVehiclesForTaskAdd",
    {
      params: { searchTerm },
    },
  );

  return response.data;
};

export const GetUserVehicleForTaskUpdateService = async (
  id: string,
  searchTerm?: string,
) => {
  const response = await axios.get<VehicleGet[]>(
    `${api}Vehicle/UserVehiclesForTaskUpdate/${id}`,
    {
      params: { searchTerm },
    },
  );

  return response.data;
};

export const GetUserVehicleStatsService = async () => {
  const response = await axios.get<UserVehiclesStats>(
    api + "Vehicle/UserVehiclesStats",
  );
  return response.data;
};

export const UpdateVehicleService = async (
  id: string,
  vehiclePost: VehiclePost,
) => {
  const response = await axios.put<VehiclePost>(
    `${api}Vehicle/${id}`,
    vehiclePost,
  );
  return response.data;
};

export const DeleteVehicleService = async (id: string) => {
  const response = await axios.delete(`${api}Vehicle/${id}`);
  return response.data;
};
