import { VehicleType } from "../Enum/VehicleType";

export interface VehicleGet {
  id: string;
  brand: string;
  model: string;
  name: string;
  registrationNumber: string;
  mileage: number;
  vehicleType: VehicleType;
  dateOfProduction: string;
  insuranceOcValidUntil: string;
  insuranceOcCost: number;
  technicalInspectionValidUntil: string;
  isAvailable: boolean;
}

export interface VehiclePost {
  brand: string;
  model: string;
  name: string;
  registrationNumber: string;
  mileage: number;
  vehicleType: VehicleType;
  dateOfProduction: string;
  insuranceOcValidUntil: string;
  insuranceOcCost: number;
  technicalInspectionValidUntil: string;
  isAvailable?: boolean;
}

export interface PaginatedVehicleResponse {
  vehicleGet: VehicleGet[];
  totalItems: number;
  totalPages: number;
  pageIndex: number;
}

export interface UserVehiclesStats {
  activeVehicles: number;
  inactiveVehicles: number;
  averageVehicleAge: number;
  totalInsuranceCost: number;
  averageInsuranceCost: number;
}
