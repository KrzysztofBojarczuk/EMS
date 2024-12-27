import React, { JSX } from "react";

interface Props {
  name: string;
  email: string;
  phone: string;
  salary: number;
}

// state to obiekt, który służy do przechowywania danych dynamicznych, które mogą zmieniać się w czasie i wpływają na sposób renderowania komponentu.
const Employee: React.FC<Props> = ({
  name,
  email,
  phone,
  salary,
}: Props): JSX.Element => {
  return (
    <div>
      Employee
      {name}
      {email}
      {phone}
      {salary}
    </div>
  );
};

export default Employee;
