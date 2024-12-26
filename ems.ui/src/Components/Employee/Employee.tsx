import React from "react";

interface Props {
  name: string;
  email: string;
  phone: string;
  salary: number;
}

const Employee = ({ name, email, phone, salary }: Props) => {
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
