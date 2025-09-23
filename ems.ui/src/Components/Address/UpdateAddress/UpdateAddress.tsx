import React from "react";
import { useForm, Controller } from "react-hook-form";
import { Button } from "primereact/button";
import { UserUpdateAddressService } from "../../../Services/AddressService.tsx";
import { AddressGet, AddressPost } from "../../../Models/Address.ts";
import { InputText } from "primereact/inputtext";
import { InputMask } from "primereact/inputmask";

interface UpdateAddressProps {
  address: AddressGet;
  onClose: () => void;
  onUpdateSuccess: () => void;
}

const UpdateAddress: React.FC<UpdateAddressProps> = ({
  address,
  onClose,
  onUpdateSuccess,
}) => {
  const {
    control,
    handleSubmit,
    formState: { errors },
    reset,
  } = useForm({
    defaultValues: {
      city: address.city,
      street: address.street,
      number: address.number,
      zipCode: address.zipCode,
    },
  });

  const onSubmit = async (data: AddressPost) => {
    await UserUpdateAddressService(data, address.id);
    onUpdateSuccess();
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
              <InputMask {...field} mask="999" />
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
              <InputMask {...field} mask="99-999" />
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

export default UpdateAddress;
