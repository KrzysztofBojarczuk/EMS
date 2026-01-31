import React, { useEffect, useState } from "react";
import { useForm, Controller } from "react-hook-form";
import { Checkbox, CheckboxChangeEvent } from "primereact/checkbox";
import { InputText } from "primereact/inputtext";
import { Button } from "primereact/button";
import { IconField } from "primereact/iconfield";
import { InputIcon } from "primereact/inputicon";
import { EmployeeGet } from "../../../Models/Employee";
import {
  EmployeeListGet,
  EmployeeListPost,
} from "../../../Models/EmployeeList";
import {
  GetUserEmployeesForListUpdateService,
  UpdateEmployeeListService,
} from "../../../Services/EmployeeService";

interface Props {
  employeeList: EmployeeListGet;
  onClose: () => void;
  onUpdateSuccess: () => void;
}

type FormValues = {
  name: string;
};

const UpdateEmployeeList = ({
  employeeList,
  onClose,
  onUpdateSuccess,
}: Props) => {
  const [employees, setEmployees] = useState<EmployeeGet[]>([]);
  const [selectedEmployees, setSelectedEmployees] = useState<string[]>([]);
  const [searchTerm, setSearchTerm] = useState("");
  const [errorMessage, setErrorMessage] = useState<string | null>(null);

  const {
    control,
    handleSubmit,
    formState: { errors },
  } = useForm<FormValues>({
    defaultValues: { name: employeeList.name },
  });

  const fetchEmployees = async () => {
    const data = await GetUserEmployeesForListUpdateService(
      employeeList.id,
      searchTerm,
    );
    setEmployees(data);
  };

  useEffect(() => {
    setSelectedEmployees(employeeList.employees.map((e) => e.id));
  }, [employeeList.id]);

  useEffect(() => {
    fetchEmployees();
  }, [searchTerm]);

  const onCheckboxChange = (e: CheckboxChangeEvent) => {
    const id = e.value as string;

    setSelectedEmployees((prev) =>
      e.checked ? [...prev, id] : prev.filter((x) => x !== id),
    );
  };

  const onSubmit = async (data: FormValues) => {
    const payload: EmployeeListPost = {
      name: data.name,
      employeeIds: selectedEmployees,
    };

    try {
      await UpdateEmployeeListService(employeeList.id, payload);
      onUpdateSuccess();
      onClose();
    } catch (err: any) {
      setErrorMessage(err.response.data);
    }
  };

  return (
    <form onSubmit={handleSubmit(onSubmit)}>
      <div className="card flex justify-content-center">
        <div className="flex flex-column gap-3">
          <Controller
            name="name"
            control={control}
            rules={{ required: "Name is required" }}
            render={({ field }) => (
              <div className="inline-flex flex-column gap-2">
                <InputText {...field} placeholder="List Name" />
                {errors.name && (
                  <small className="p-error">{errors.name.message}</small>
                )}
              </div>
            )}
          />
          <IconField iconPosition="left">
            <InputIcon className="pi pi-search" />
            <InputText
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
              placeholder="Search"
            />
          </IconField>
          {employees.map((item) => (
            <div key={item.id} className="flex align-items-center">
              <Checkbox
                inputId={item.id}
                value={item.id}
                checked={selectedEmployees.includes(item.id)}
                onChange={onCheckboxChange}
              />
              <label htmlFor={item.id} className="ml-2">
                {item.name}
              </label>
            </div>
          ))}
          {errorMessage && <p className="text-red mt-2">{errorMessage}</p>}
          <div className="inline-flex flex-column gap-2">
            <Button label="Update" type="submit" />
          </div>
        </div>
      </div>
    </form>
  );
};

export default UpdateEmployeeList;
