import React, { JSX, SyntheticEvent } from "react";
import AddEmployee from "./AddEmployee/AddEmployee.tsx";

interface Props {
  name: string;
  email: string;
  phone: string;
  salary: number;
  onEmployeeCreate: (e: SyntheticEvent) => void;
}

// state to obiekt, który służy do przechowywania danych dynamicznych, które mogą zmieniać się w czasie i wpływają na sposób renderowania komponentu.
const Employee: React.FC<Props> = ({
  name,
  email,
  phone,
  salary,
  onEmployeeCreate,
}: Props): JSX.Element => {
  return (
    <div>
      <AddEmployee onEmployeeCreate={onEmployeeCreate} />
      <b></b>
      Employee
      {name}
      {email}
      {phone}
      {salary}
    </div>
  );
};

export default Employee;
