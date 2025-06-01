import React, { useState } from "react";
import { useForm, Controller } from "react-hook-form";
import { Button } from "primereact/button";
import { Calendar } from "primereact/calendar";
import { Dropdown } from "primereact/dropdown";
import { UserPostReservationService } from "../../../Services/LocalReservationService.tsx";
import { ReservationPost } from "../../../Models/Reservation.ts";
import { LocalGet } from "../../../Models/Local.ts";

type Props = {
  selectedLocalId: string;
  onClose: () => void;
  onReservationSuccess: () => void;
};

const MakeReservation: React.FC<Props> = ({
  selectedLocalId,
  onClose,
  onReservationSuccess,
}) => {
  const [errorMessage, setErrorMessage] = React.useState<string | null>(null);

  const {
    control,
    handleSubmit,
    formState: { errors },
    reset,
  } = useForm<ReservationPost>({
    defaultValues: {
      localId: selectedLocalId,
      checkInDate: undefined,
      checkOutDate: undefined,
    },
  });

  const onSubmit = async (data: ReservationPost) => {
    try {
      await UserPostReservationService(data);
      onReservationSuccess();
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
          name="checkInDate"
          control={control}
          rules={{ required: "Check-in date and time are required" }}
          render={({ field }) => (
            <div className="inline-flex flex-column gap-2">
              <Calendar
                value={field.value ? new Date(field.value) : null}
                onChange={(e) => field.onChange(e.value)}
                placeholder="Check-in Date & Time"
                showIcon
                dateFormat="yy-mm-dd"
                showTime
                hourFormat="24"
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
                placeholder="Check-out Date & Time"
                showIcon
                dateFormat="yy-mm-dd"
                showTime
                hourFormat="24"
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

export default MakeReservation;
