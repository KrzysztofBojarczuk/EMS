import { Tag } from "primereact/tag";
import { VehicleType } from "../../Enum/VehicleType";
import { VehicleGet } from "../../Models/Vehicle";

export const vehicleTypeToText: Record<VehicleType, string> = {
  [VehicleType.Car]: "Car",
  [VehicleType.Van]: "Van",
  [VehicleType.Truck]: "Truck",
  [VehicleType.Forklift]: "Forklift",
  [VehicleType.Excavator]: "Excavator",
  [VehicleType.Tractor]: "Tractor",
  [VehicleType.Motorcycle]: "Motorcycle",
  [VehicleType.ElectricCart]: "Electric Cart",
  [VehicleType.Trailer]: "Trailer",
  [VehicleType.Other]: "Other",
};

export const getVehicleTypeSeverity = (vehicle: VehicleGet) => {
  switch (vehicle.vehicleType) {
    case VehicleType.Car:
    case VehicleType.Van:
      return "success";
    case VehicleType.Truck:
    case VehicleType.Trailer:
      return "warning";
    case VehicleType.Forklift:
    case VehicleType.Excavator:
    case VehicleType.Tractor:
      return "info";
    case VehicleType.Motorcycle:
    case VehicleType.ElectricCart:
      return "secondary";
    case VehicleType.Other:
    default:
      return "danger";
  }
};

export const vehicleTypeBodyTemplate = (rowData: VehicleGet) => (
  <Tag
    value={vehicleTypeToText[rowData.vehicleType]}
    severity={getVehicleTypeSeverity(rowData)}
  />
);
