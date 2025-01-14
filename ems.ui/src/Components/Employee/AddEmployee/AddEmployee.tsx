import React from "react";
import { useForm, Controller } from "react-hook-form";
import { InputText } from "primereact/inputtext";
import { InputMask } from "primereact/inputmask";
import { InputNumber } from "primereact/inputnumber";
import { Button } from "primereact/button";
import { UserPostEmployeesService } from "../../../Services/EmployeeService.tsx";

const AddEmployee = ({ onClose, onAddSuccess }) => {
  const {
    control,
    handleSubmit,
    formState: { errors },
    reset,
  } = useForm({
    defaultValues: {
      name: "",
      email: "",
      phone: "",
      salary: 0,
    },
  });

  const onSubmit = async (data) => {
    await UserPostEmployeesService(data);
    onAddSuccess();
    onClose();
    reset();
  };

  return (
    <form onSubmit={handleSubmit(onSubmit)}>
      <div className="flex flex-column px-8 py-5 gap-4">
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
          name="email"
          control={control}
          rules={{
            required: "Email is required",
            pattern: {
              value: /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/,
              message: "Invalid email format",
            },
          }}
          render={({ field }) => (
            <div className="inline-flex flex-column gap-2">
              <InputText {...field} placeholder="Email" />
              {errors.email && (
                <small className="p-error">{errors.email.message}</small>
              )}
            </div>
          )}
        />
        <Controller
          name="phone"
          control={control}
          rules={{ required: "Phone is required" }}
          render={({ field }) => (
            <div className="inline-flex flex-column gap-2">
              <InputMask {...field} mask="999-999-999" placeholder="Phone" />
              {errors.phone && (
                <small className="p-error">{errors.phone.message}</small>
              )}
            </div>
          )}
        />
        <Controller
          name="salary"
          control={control}
          rules={{
            required: "Salary is required",
            min: { value: 1, message: "Salary must be greater than 0" },
          }}
          render={({ field }) => (
            <div className="inline-flex flex-column gap-2">
              <InputNumber
                mode="currency"
                currency="EUR"
                locale="de-DE"
                placeholder="Salary"
                onValueChange={(e) => field.onChange(e.value)}
              />
              {errors.salary && (
                <small className="p-error">{errors.salary.message}</small>
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

export default AddEmployee;
