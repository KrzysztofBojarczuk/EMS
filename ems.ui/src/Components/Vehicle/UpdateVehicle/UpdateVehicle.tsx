import React, { useEffect } from "react";
import { useForm, Controller } from "react-hook-form";
import { Button } from "primereact/button";
import { InputText } from "primereact/inputtext";
import { InputNumber } from "primereact/inputnumber";
import { Calendar } from "primereact/calendar";
import { Checkbox } from "primereact/checkbox";
import { VehicleGet, VehiclePost } from "../../../Models/Vehicle";
import { VehicleType } from "../../../Enum/VehicleType";
import { UpdateVehicleService } from "../../../Services/VehicleService";

interface Props {
  vehicle: VehicleGet;
  onClose: () => void;
  onUpdateSuccess: () => void;
}

const UpdateVehicle = ({ vehicle, onClose, onUpdateSuccess }: Props) => {
  const {
    control,
    handleSubmit,
    formState: { errors },
    reset,
  } = useForm<VehiclePost>({
    defaultValues: {
      brand: vehicle.brand,
      model: vehicle.model,
      name: vehicle.name,
      registrationNumber: vehicle.registrationNumber,
      mileage: vehicle.mileage,
      vehicleType: vehicle.vehicleType,
      dateOfProduction: vehicle.dateOfProduction,
      insuranceOcValidUntil: vehicle.insuranceOcValidUntil,
      insuranceOcCost: vehicle.insuranceOcCost,
      technicalInspectionValidUntil: vehicle.technicalInspectionValidUntil,
      isAvailable: vehicle.isAvailable,
    },
  });

  useEffect(() => {
    reset({
      brand: vehicle.brand,
      model: vehicle.model,
      name: vehicle.name,
      registrationNumber: vehicle.registrationNumber,
      mileage: vehicle.mileage,
      vehicleType: vehicle.vehicleType,
      dateOfProduction: vehicle.dateOfProduction,
      insuranceOcValidUntil: vehicle.insuranceOcValidUntil,
      insuranceOcCost: vehicle.insuranceOcCost,
      technicalInspectionValidUntil: vehicle.technicalInspectionValidUntil,
      isAvailable: vehicle.isAvailable,
    });
  }, [vehicle, reset]);

  const onSubmit = async (data: VehiclePost) => {
    await UpdateVehicleService(vehicle.id, data);
    onUpdateSuccess();
    onClose();
  };

  return (
    <form onSubmit={handleSubmit(onSubmit)}>
      <div className="flex flex-column px-8 py-5 gap-4">
        <Controller
          name="brand"
          control={control}
          rules={{ required: "Brand is required" }}
          render={({ field }) => (
            <div className="inline-flex flex-column gap-2">
              <InputText {...field} placeholder="Brand" />
              {errors.brand && (
                <small className="p-error">{errors.brand.message}</small>
              )}
            </div>
          )}
        />
        <Controller
          name="model"
          control={control}
          rules={{ required: "Model is required" }}
          render={({ field }) => (
            <div className="inline-flex flex-column gap-2">
              <InputText {...field} placeholder="Model" />
              {errors.model && (
                <small className="p-error">{errors.model.message}</small>
              )}
            </div>
          )}
        />
        <Controller
          name="name"
          control={control}
          rules={{ required: "Name is required" }}
          render={({ field }) => (
            <div className="inline-flex flex-column gap-2">
              <InputText {...field} placeholder="Name" />
              {errors.name && (
                <small className="p-error">{errors.name.message}</small>
              )}
            </div>
          )}
        />
        <Controller
          name="registrationNumber"
          control={control}
          rules={{ required: "Registration number is required" }}
          render={({ field }) => (
            <div className="inline-flex flex-column gap-2">
              <InputText {...field} placeholder="Registration Number" />
              {errors.registrationNumber && (
                <small className="p-error">
                  {errors.registrationNumber.message}
                </small>
              )}
            </div>
          )}
        />
        <Controller
          name="mileage"
          control={control}
          rules={{
            required: "Mileage is required",
            min: { value: 0, message: "Mileage cannot be negative" },
          }}
          render={({ field }) => (
            <div className="inline-flex flex-column gap-2">
              <InputNumber
                placeholder="Mileage"
                useGrouping={false}
                value={field.value}
                onValueChange={(e) => field.onChange(e.value)}
              />
              {errors.mileage && (
                <small className="p-error">{errors.mileage.message}</small>
              )}
            </div>
          )}
        />
        <Controller
          name="vehicleType"
          control={control}
          render={({ field }) => (
            <div className="inline-flex flex-column gap-2">
              <select {...field} className="p-inputtext">
                <option value={VehicleType.Car}>Car</option>
                <option value={VehicleType.Van}>Van</option>
                <option value={VehicleType.Truck}>Truck</option>
                <option value={VehicleType.Forklift}>Forklift</option>
                <option value={VehicleType.Excavator}>Excavator</option>
                <option value={VehicleType.Tractor}>Tractor</option>
                <option value={VehicleType.Motorcycle}>Motorcycle</option>
                <option value={VehicleType.ElectricCart}>Electric Cart</option>
                <option value={VehicleType.Trailer}>Trailer</option>
                <option value={VehicleType.Other}>Other</option>
              </select>
            </div>
          )}
        />
        <Controller
          name="dateOfProduction"
          control={control}
          rules={{ required: "Production date is required" }}
          render={({ field }) => (
            <div className="inline-flex flex-column gap-2">
              <Calendar
                value={field.value ? new Date(field.value) : null}
                onChange={(e) => field.onChange(e.value)}
                placeholder="Date of Production"
                showIcon
                dateFormat="dd/mm/yy"
              />
              {errors.dateOfProduction && (
                <small className="p-error">
                  {errors.dateOfProduction.message}
                </small>
              )}
            </div>
          )}
        />
        <Controller
          name="insuranceOcValidUntil"
          control={control}
          rules={{ required: "Insurance OC date is required" }}
          render={({ field }) => (
            <div className="inline-flex flex-column gap-2">
              <Calendar
                value={field.value ? new Date(field.value) : null}
                onChange={(e) => field.onChange(e.value)}
                placeholder="Insurance OC Valid Until"
                dateFormat="dd/mm/yy"
                showIcon
              />
              {errors.insuranceOcValidUntil && (
                <small className="p-error">
                  {errors.insuranceOcValidUntil.message}
                </small>
              )}
            </div>
          )}
        />
        <Controller
          name="insuranceOcCost"
          control={control}
          rules={{
            required: "Insurance OC cost is required",
            min: { value: 0, message: "Insurance cost cannot be negative" },
          }}
          render={({ field }) => (
            <div className="inline-flex flex-column gap-2">
              <InputNumber
                mode="currency"
                currency="EUR"
                locale="de-DE"
                placeholder="Insurance OC Cost"
                useGrouping={false}
                value={field.value}
                onValueChange={(e) => field.onChange(e.value)}
              />
              {errors.insuranceOcCost && (
                <small className="p-error">
                  {errors.insuranceOcCost.message}
                </small>
              )}
            </div>
          )}
        />
        <Controller
          name="technicalInspectionValidUntil"
          control={control}
          rules={{ required: "Technical inspection date is required" }}
          render={({ field }) => (
            <div className="inline-flex flex-column gap-2">
              <Calendar
                value={field.value ? new Date(field.value) : null}
                onChange={(e) => field.onChange(e.value)}
                placeholder="Technical Inspection Valid Until"
                dateFormat="dd/mm/yy"
                showIcon
              />
              {errors.technicalInspectionValidUntil && (
                <small className="p-error">
                  {errors.technicalInspectionValidUntil.message}
                </small>
              )}
            </div>
          )}
        />
        <div className="field-checkbox">
          <Controller
            name="isAvailable"
            control={control}
            render={({ field }) => (
              <>
                <Checkbox
                  inputId="isAvailable"
                  onChange={(e) => field.onChange(e.checked)}
                  checked={!!field.value}
                />
                <label htmlFor="isAvailable">Available</label>
              </>
            )}
          />
        </div>

        <div className="inline-flex flex-column gap-2">
          <Button label="Update" type="submit" />
        </div>
      </div>
    </form>
  );
};

export default UpdateVehicle;
