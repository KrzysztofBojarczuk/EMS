import React, { useState } from "react";
import { useForm, Controller } from "react-hook-form";
import { Button } from "primereact/button";
import { Calendar } from "primereact/calendar";
import { Dropdown } from "primereact/dropdown";
import { ReservationPost } from "../../../Models/Reservation";
import { PostReservationService } from "../../../Services/LocalReservationService";
import { InputText } from "primereact/inputtext";

interface Props {
  selectedLocalId: string;
  onClose: () => void;
  onAddSuccess: () => void;
}

const AddReservation = ({ selectedLocalId, onClose, onAddSuccess }: Props) => {
  const [errorMessage, setErrorMessage] = React.useState<string | null>(null);

  const {
    control,
    handleSubmit,
    formState: { errors },
    reset,
  } = useForm<ReservationPost>({
    defaultValues: {
      localId: selectedLocalId,
      description: "",
      checkInDate: undefined,
      checkOutDate: undefined,
    },
  });

  const onSubmit = async (data: ReservationPost) => {
    try {
      await PostReservationService(data);
      onAddSuccess();
      onClose();
      reset();
    } catch (error: any) {
      if (error.response && error.response.data) {
        setErrorMessage(error.response.data);
      }
    }
  };

  return (
    <form onSubmit={handleSubmit(onSubmit)}>
      <div className="flex flex-column px-8 py-5 gap-4">
        <Controller
          name="description"
          control={control}
          rules={{ required: "Description is required" }}
          render={({ field }) => (
            <div className="inline-flex flex-column gap-2">
              <InputText {...field} placeholder="Enter description" />
              {errors.description && (
                <small className="p-error">{errors.description.message}</small>
              )}
            </div>
          )}
        />
        <Controller
          name="checkInDate"
          control={control}
          rules={{ required: "Check-in date and time are required" }}
          render={({ field }) => (
            <div className="inline-flex flex-column gap-2">
              <Calendar
                value={field.value ? new Date(field.value) : null}
                onChange={(e) => field.onChange(e.value)}
                showIcon
                dateFormat="dd/mm/yy"
                showTime
                hourFormat="24"
                placeholder="Check-in Date & Time"
              />
              {errors.checkInDate && (
                <small className="p-error">{errors.checkInDate.message}</small>
              )}
            </div>
          )}
        />
        <Controller
          name="checkOutDate"
          control={control}
          rules={{ required: "Check-out date and time are required" }}
          render={({ field }) => (
            <div className="inline-flex flex-column gap-2">
              <Calendar
                value={field.value ? new Date(field.value) : null}
                onChange={(e) => field.onChange(e.value)}
                showIcon
                dateFormat="dd/mm/yy"
                showTime
                hourFormat="24"
                placeholder="Check-out Date & Time"
              />
              {errors.checkOutDate && (
                <small className="p-error">{errors.checkOutDate.message}</small>
              )}
            </div>
          )}
        />
        {errorMessage && <p className="text-red mt-2">{errorMessage}</p>}
        <div className="inline-flex flex-column gap-2">
          <Button label="Submit" type="submit" />
        </div>
      </div>
    </form>
  );
};

export default AddReservation;
