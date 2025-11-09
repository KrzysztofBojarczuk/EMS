import { Controller, useForm } from "react-hook-form";
import { LocalGet, LocalPost } from "../../../Models/Local";
import React, { useEffect } from "react";
import { InputText } from "primereact/inputtext";
import { InputNumber } from "primereact/inputnumber";
import { Checkbox } from "primereact/checkbox";
import { Button } from "primereact/button";
import { UpdateLocalService } from "../../../Services/LocalReservationService";

interface UpdateLocalProps {
  local: LocalGet;
  onClose: () => void;
  onUpdateSuccess: () => void;
}

const UpdateLocal: React.FC<UpdateLocalProps> = ({
  local,
  onClose,
  onUpdateSuccess,
}) => {
  const {
    control,
    handleSubmit,
    formState: { errors },
    reset,
  } = useForm<LocalPost>({
    defaultValues: {
      description: local.description,
      localNumber: local.localNumber,
      surface: local.surface,
      needsRepair: local.needsRepair,
    },
  });

  const onSubmit = async (data: LocalPost) => {
    await UpdateLocalService(local.id, data);
    onUpdateSuccess();
    onClose();
  };

  useEffect(() => {
    reset({
      description: local.description,
      localNumber: local.localNumber,
      surface: local.surface,
      needsRepair: local.needsRepair,
    });
  }, [local, reset]);

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
                value={field.value ?? 0}
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
                value={field.value ?? 0}
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
          <Button label="Update" type="submit" />
        </div>
      </div>
    </form>
  );
};

export default UpdateLocal;
