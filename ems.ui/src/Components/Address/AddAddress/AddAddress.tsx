import React from "react";
import { useForm, Controller } from "react-hook-form";
import { UserPostAddressService } from "../../../Services/AddressService.tsx";
import { InputText } from "primereact/inputtext";
import { Button } from "primereact/button";

type Props = {};

const AddAddress = ({ onClose, onAddSuccess }) => {
  const {
    control,
    handleSubmit,
    formState: { errors },
    reset,
  } = useForm({
    defaultValues: {
      city: "",
      street: "",
      number: "",
      zipCode: "",
    },
  });

  const onSubmit = async (data) => {
    await UserPostAddressService(data);
    onAddSuccess();
    onClose();
    reset();
  };

  return (
    <form onSubmit={handleSubmit(onSubmit)}>
      <div className="flex flex-column px-8 py-5 gap-4">
        <Controller
          name="city"
          control={control}
          rules={{ required: "City is required" }}
          render={({ field }) => (
            <div className="inline-flex flex-column gap-2">
              <InputText {...field} placeholder="City" />
              {errors.city && (
                <small className="p-error">{errors.city.message}</small>
              )}
            </div>
          )}
        />
        <Controller
          name="street"
          control={control}
          rules={{ required: "Street is required" }}
          render={({ field }) => (
            <div className="inline-flex flex-column gap-2">
              <InputText {...field} placeholder="Street" />
              {errors.street && (
                <small className="p-error">{errors.street.message}</small>
              )}
            </div>
          )}
        />
        <Controller
          name="number"
          control={control}
          rules={{ required: "Number is required" }}
          render={({ field }) => (
            <div className="inline-flex flex-column gap-2">
              <InputText {...field} placeholder="Number" />
              {errors.number && (
                <small className="p-error">{errors.number.message}</small>
              )}
            </div>
          )}
        />
        <Controller
          name="zipCode"
          control={control}
          rules={{ required: "Zip Code is required" }}
          render={({ field }) => (
            <div className="inline-flex flex-column gap-2">
              <InputText {...field} placeholder="Zip Code" />
              {errors.zipCode && (
                <small className="p-error">{errors.zipCode.message}</small>
              )}
            </div>
          )}
        />
        <div className="inline-flex flex-column gap-2">
          <Button label="Submit" type="submit" />
        </div>
      </div>
    </form>
  );
};

export default AddAddress;
