import React from "react";
import { useForm, Controller } from "react-hook-form";
import { Button } from "primereact/button";
import { InputText } from "primereact/inputtext";
import { InputNumber } from "primereact/inputnumber";
import { Calendar } from "primereact/calendar";
import { Dropdown } from "primereact/dropdown";
import { VehiclePost } from "../../../Models/Vehicle";
import { PostVehicleService } from "../../../Services/VehicleService";
import { VehicleType } from "../../../Enum/VehicleType";
import { Checkbox } from "primereact/checkbox";

type Props = {
  onClose: () => void;
  onAddSuccess: () => void;
};

const AddVehicle: React.FC<Props> = ({ onClose, onAddSuccess }) => {
  const {
    control,
    handleSubmit,
    formState: { errors },
    reset,
  } = useForm<VehiclePost>({
    defaultValues: {
      brand: "",
      model: "",
      name: "",
      registrationNumber: "",
      mileage: 0,
      vehicleType: VehicleType.Car,
      dateOfProduction: "",
      isAvailable: true,
    },
  });

  const onSubmit = async (data: VehiclePost) => {
    const payload = {
      ...data,
      dateOfProduction: new Date(data.dateOfProduction).toISOString(),
    };
    await PostVehicleService(payload);
    onAddSuccess();
    onClose();
    reset();
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
                {...field}
                dateFormat="yy-mm-dd"
                showIcon
                placeholder="Date of Production"
                value={field.value ? new Date(field.value) : undefined}
                onChange={(e) =>
                  field.onChange(
                    e.value ? e.value.toISOString().split("T")[0] : ""
                  )
                }
              />
              {errors.dateOfProduction && (
                <small className="p-error">
                  {errors.dateOfProduction.message}
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
                <label htmlFor=" isAvailable">Available</label>
              </>
            )}
          />
        </div>
        <div className="inline-flex flex-column gap-2">
          <Button label="Submit" type="submit" />
        </div>
      </div>
    </form>
  );
};

export default AddVehicle;
