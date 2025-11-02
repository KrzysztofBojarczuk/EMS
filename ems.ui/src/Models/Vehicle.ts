import { VehicleType } from "../Enum/VehicleType";

export interface VehicleGet {
  id: string;
  brand: string;
  model: string;
  name: string;
  registrationNumber: string;
  vehicleType: VehicleType;
  dateOfProduction: string;
  isAvailable: boolean;
}

export interface VehiclePost {
  name: string;
  registrationNumber: string;
  vehicleType: VehicleType;
  dateOfProduction: string;
  isAvailable?: boolean;
}

export interface PaginatedVehicleResponse {
  vehicleGet: VehicleGet[];
  totalItems: number;
  totalPages: number;
  pageIndex: number;
}
