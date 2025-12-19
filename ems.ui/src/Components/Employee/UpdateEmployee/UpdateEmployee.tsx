import React, { useEffect } from "react";
import { useForm, Controller } from "react-hook-form";
import { InputText } from "primereact/inputtext";
import { InputMask } from "primereact/inputmask";
import { InputNumber } from "primereact/inputnumber";
import { Button } from "primereact/button";
import { EmployeeGet, EmployeePost } from "../../../Models/Employee";
import { UpdateEmployeesService } from "../../../Services/EmployeeService";
import { Calendar } from "primereact/calendar";

interface UpdateEmployeeProps {
  employee: EmployeeGet;
  onClose: () => void;
  onUpdateSuccess: () => void;
}

const UpdateEmployee: React.FC<UpdateEmployeeProps> = ({
  employee,
  onClose,
  onUpdateSuccess,
}) => {
  const {
    control,
    handleSubmit,
    formState: { errors },
    reset,
  } = useForm<EmployeePost>({
    defaultValues: {
      name: employee.name,
      email: employee.email,
      phone: employee.phone,
      salary: employee.salary,
      dateOfBirth: employee.dateOfBirth,
      employmentDate: employee.employmentDate,
      medicalCheckValidUntil: employee.medicalCheckValidUntil,
    },
  });

  const onSubmit = async (data: EmployeePost) => {
    await UpdateEmployeesService(employee.id, data);
    onUpdateSuccess();
    onClose();
  };

  useEffect(() => {
    reset({
      name: employee.name,
      email: employee.email,
      phone: employee.phone,
      salary: employee.salary,
      dateOfBirth: employee.dateOfBirth,
      employmentDate: employee.employmentDate,
      medicalCheckValidUntil: employee.medicalCheckValidUntil,
    });
  }, [employee, reset]);

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
                onValueChange={(e) => field.onChange(e.value)}
                placeholder="Salary"
              />
              {errors.salary && (
                <small className="p-error">{errors.salary.message}</small>
              )}
            </div>
          )}
        />
        <Controller
          name="dateOfBirth"
          control={control}
          rules={{ required: "Date Of Birth date is required" }}
          render={({ field }) => (
            <div className="inline-flex flex-column gap-2">
              <Calendar
                value={field.value ? new Date(field.value) : null}
                onChange={(e) => field.onChange(e.value)}
                dateFormat="dd/mm/yy"
                showIcon
                placeholder="Date Of Birth"
              />
              {errors.dateOfBirth && (
                <small className="p-error">{errors.dateOfBirth.message}</small>
              )}
            </div>
          )}
        />
        <Controller
          name="employmentDate"
          control={control}
          rules={{ required: "Employment date is required" }}
          render={({ field }) => (
            <div className="inline-flex flex-column gap-2">
              <Calendar
                value={field.value ? new Date(field.value) : null}
                onChange={(e) => field.onChange(e.value)}
                dateFormat="dd/mm/yy"
                showIcon
                placeholder="Employment Date"
              />
              {errors.employmentDate && (
                <small className="p-error">
                  {errors.employmentDate.message}
                </small>
              )}
            </div>
          )}
        />
        <Controller
          name="medicalCheckValidUntil"
          control={control}
          rules={{ required: "Medical check date is required" }}
          render={({ field }) => (
            <div className="inline-flex flex-column gap-2">
              <Calendar
                value={field.value ? new Date(field.value) : null}
                onChange={(e) => field.onChange(e.value)}
                dateFormat="dd/mm/yy"
                showIcon
                placeholder="Medical Check Valid Until"
              />
              {errors.medicalCheckValidUntil && (
                <small className="p-error">
                  {errors.medicalCheckValidUntil.message}
                </small>
              )}
            </div>
          )}
        />
        <div className="inline-flex flex-column gap-2">
          <Button label="Update" type="submit" />
        </div>
      </div>
    </form>
  );
};

export default UpdateEmployee;
