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

export interface PaginatedList<T> {
  totalItems: number;
  pageIndex: number;
  totalPages: number;
  hasNextPage: boolean;
  hasPreviousPage: boolean;
  items: T[];
}

export interface UserVehiclesStats {
  activeVehicles: number;
  inactiveVehicles: number;
  averageVehicleAge: number;
  totalInsuranceCost: number;
  averageInsuranceCost: number;
}
