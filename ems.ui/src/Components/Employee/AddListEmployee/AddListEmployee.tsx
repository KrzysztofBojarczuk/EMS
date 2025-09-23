import React, { useEffect, useState } from "react";
import { EmployeeGet } from "../../../Models/Employee";
import {
  UserGetEmployeesForListService,
  UserPostListEmployeesService,
} from "../../../Services/EmployeeService.tsx";
import { Checkbox, CheckboxChangeEvent } from "primereact/checkbox";
import { Controller, useForm } from "react-hook-form";
import { InputText } from "primereact/inputtext";
import { Button } from "primereact/button";
import { InputIcon } from "primereact/inputicon";
import { EmployeeListPost } from "../../../Models/EmployeeList.ts";

type Props = {
  onClose: () => void;
  onAddSuccess: () => void;
};

const AddListEmployee: React.FC<Props> = ({ onClose, onAddSuccess }) => {
  const [errorMessage, setErrorMessage] = React.useState<string | null>(null);

  const {
    control,
    handleSubmit,
    formState: { errors },
    reset,
  } = useForm({
    defaultValues: {
      name: "",
      employeeIds: [],
    },
  });

  const [employees, setEmployees] = useState<EmployeeGet[]>([]);
  const [searchTerm, setSearchTerm] = useState("");
  const [selectedEmployees, setSelectedEmployees] = useState<string[]>([]);

  const onSelectedEmployee = (e: CheckboxChangeEvent) => {
    let _selectedEmployees = [...selectedEmployees];

    if (e.checked) {
      _selectedEmployees.push(e.value.id);
    } else {
      _selectedEmployees = _selectedEmployees.filter((id) => id !== e.value.id);
    }

    setSelectedEmployees(_selectedEmployees);
  };

  const fetchEmployees = async () => {
    const data = await UserGetEmployeesForListService(searchTerm);
    setEmployees(data);
  };

  useEffect(() => {
    fetchEmployees();
  }, [searchTerm]);

  const onSubmit = async (data: EmployeeListPost) => {
    const postData = {
      name: data.name,
      employeeIds: selectedEmployees,
    };

    try {
      await UserPostListEmployeesService(postData);
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
        <div className="card flex justify-content-center">
          <div className="flex flex-column gap-3">
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
            <InputText
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
              placeholder="Search"
            />
            {employees.map((item) => {
              return (
                <div key={item.id} className="flex align-items-center">
                  <Checkbox
                    inputId={item.id}
                    name="id"
                    value={item}
                    onChange={onSelectedEmployee}
                    checked={selectedEmployees.includes(item.id)}
                  />
                  <label htmlFor={item.id} className="ml-2">
                    {item.name}
                  </label>
                </div>
              );
            })}
          </div>
        </div>
        {errorMessage && <p className="text-red mt-2">{errorMessage}</p>}
        <div className="inline-flex flex-column gap-2">
          <Button
            label="Submit"
            type="submit"
            disabled={selectedEmployees.length === 0}
          />
        </div>
      </div>
    </form>
  );
};

export default AddListEmployee;
