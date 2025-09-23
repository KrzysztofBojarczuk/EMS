import React from "react";
import { useForm, Controller } from "react-hook-form";
import { UserPostAddressService } from "../../../Services/AddressService.tsx";
import { InputText } from "primereact/inputtext";
import { InputMask } from "primereact/inputmask";
import { Button } from "primereact/button";
import { AddressGet, AddressPost } from "../../../Models/Address.ts";

type Props = {
  onClose: () => void;
  onAddSuccess: () => void;
};

const AddAddress: React.FC<Props> = ({ onClose, onAddSuccess }) => {
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

  const onSubmit = async (data: AddressPost) => {
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
              <InputMask {...field} mask="999" placeholder="Number" />
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
              <InputMask {...field} mask="99-999" placeholder="Zip-Code" />
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
