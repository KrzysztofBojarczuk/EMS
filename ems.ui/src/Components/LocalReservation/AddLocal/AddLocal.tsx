import React, { useState } from "react";
import { useForm, Controller } from "react-hook-form";
import { Button } from "primereact/button";
import { InputText } from "primereact/inputtext";
import { Checkbox } from "primereact/checkbox";
import {
  InputNumber,
  InputNumberValueChangeEvent,
} from "primereact/inputnumber";
import { UserPostLocalService } from "../../../Services/LocalReservationService.tsx";

interface LocalPost {
  description: string;
  localNumber: number;
  surface: number;
  needsRepair: boolean;
}

type Props = {
  onClose: () => void;
  onAddSuccess: () => void;
};

const AddLocal: React.FC<Props> = ({ onClose, onAddSuccess }) => {
  const [value1, setValue1] = useState<number>(0);
  const [value2, setValue2] = useState<number>(0);

  const {
    control,
    handleSubmit,
    formState: { errors },
    reset,
  } = useForm<LocalPost>({
    defaultValues: {
      description: "",
      localNumber: 0,
      surface: 0,
      needsRepair: false,
    },
  });

  const onSubmit = async (data: LocalPost) => {
    await UserPostLocalService(data);
    onAddSuccess();
    onClose();
    reset();
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
          name="localNumber"
          control={control}
          rules={{ required: "Local number is required" }}
          render={({ field }) => (
            <div className="inline-flex flex-column gap-2">
              <InputNumber
                onValueChange={(e) => field.onChange(e.value)}
                placeholder="Enter local number"
              />
              {errors.localNumber && (
                <small className="p-error">{errors.localNumber.message}</small>
              )}
            </div>
          )}
        />
        <Controller
          name="surface"
          control={control}
          rules={{ required: "Surface is required" }}
          render={({ field }) => (
            <div className="inline-flex flex-column gap-2">
              <InputNumber
                onValueChange={(e) => field.onChange(e.value)}
                placeholder="Enter surface"
              />
              {errors.surface && (
                <small className="p-error">{errors.surface.message}</small>
              )}
            </div>
          )}
        />

        <div className="field-checkbox">
          <Controller
            name="needsRepair"
            control={control}
            render={({ field }) => (
              <>
                <Checkbox
                  inputId="needsRepair"
                  onChange={(e) => field.onChange(e.checked)}
                  checked={field.value}
                />
                <label htmlFor="needsRepair">Needs Repair</label>
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

export default AddLocal;
