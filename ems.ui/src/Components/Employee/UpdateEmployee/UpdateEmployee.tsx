import React, { useState } from "react";
import { InputText } from "primereact/inputtext";
import { InputMask, InputMaskChangeEvent } from "primereact/inputmask";
import { InputNumber } from "primereact/inputnumber";
import { Button } from "primereact/button";
import { UserUpdateEmployeesService } from "../../../Services/EmployeeService.tsx";
import { EmployeeGet } from "../../../Models/Employee";

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
  const [updatedEmployee, setUpdatedEmployee] = useState<EmployeeGet>({
    id: employee.id,
    name: employee.name,
    email: employee.email,
    phone: employee.phone,
    salary: employee.salary,
  });

  const handleInputChange = (
    e: React.ChangeEvent<HTMLInputElement> | InputMaskChangeEvent
  ) => {
    const { id, value } = e.target;
    setUpdatedEmployee((prev) => ({ ...prev, [id]: value }));
  };

  const handleSalaryChange = (e: any) => {
    setUpdatedEmployee((prev) => ({ ...prev, salary: e.value }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    await UserUpdateEmployeesService(updatedEmployee, updatedEmployee.id);
    onUpdateSuccess();
    onClose();
  };

  return (
    <form onSubmit={handleSubmit}>
      <div className="flex flex-column align-items-center mb-3 gap-2">
        <InputText
          id="name"
          placeholder="Name"
          value={updatedEmployee.name}
          onChange={handleInputChange}
          required
        />
        <InputText
          id="email"
          placeholder="Email"
          type="email"
          value={updatedEmployee.email}
          onChange={handleInputChange}
          required
        />
        <InputMask
          id="phone"
          placeholder="Phone"
          mask="999-999-999"
          value={updatedEmployee.phone}
          onChange={handleInputChange}
          required
        />
        <InputNumber
          inputId="salary"
          placeholder="Salary"
          mode="currency"
          currency="PLN"
          locale="pl-PL"
          value={updatedEmployee.salary}
          onValueChange={handleSalaryChange}
          required
        />
      </div>
      <Button label="Update" type="submit" />
    </form>
  );
};

export default UpdateEmployee;
