import React, { useState } from "react";
import { InputText } from "primereact/inputtext";
import { InputMask } from "primereact/inputmask";
import { InputNumber } from "primereact/inputnumber";
import { Button } from "primereact/button";
import { UserPostEmployeesService } from "../../../Services/EmployeeService.tsx";

const AddEmployee = ({ onClose }) => {
  const [employee, setEmployee] = useState({
    name: "",
    email: "",
    phone: "",
    salary: 0,
  });

  const handleInputChange = (e) => {
    const { id, value } = e.target;
    setEmployee((prev) => ({ ...prev, [id]: value }));
  };

  const handleSalaryChange = (e) => {
    setEmployee((prev) => ({ ...prev, salary: e.value }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    await UserPostEmployeesService(employee);
    onClose();
  };

  return (
    <form onSubmit={handleSubmit}>
      <div className="flex flex-column align-items-center mb-3 gap-2">
        <InputText
          id="name"
          placeholder="Name"
          value={employee.name}
          onChange={handleInputChange}
          required
        />
        <InputText
          id="email"
          placeholder="Email"
          value={employee.email}
          onChange={handleInputChange}
          required
        />
        <InputMask
          id="phone"
          placeholder="Phone"
          mask="999-999-999"
          value={employee.phone}
          onChange={handleInputChange}
        />
        <InputNumber
          inputId="salary"
          placeholder="Salary"
          mode="currency"
          currency="PLN"
          locale="pl-PL"
          value={employee.salary}
          onValueChange={handleSalaryChange}
        />
      </div>
      <Button label="Submit" type="submit" />
    </form>
  );
};

export default AddEmployee;
