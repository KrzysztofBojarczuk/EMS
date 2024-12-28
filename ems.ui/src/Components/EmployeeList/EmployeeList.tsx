import React, { JSX } from "react";
import Employee from "../Employee/Employee.tsx";

interface Props {}

const EmployeeList: React.FC<Props> = (props: Props): JSX.Element => {
  return (
    <div>
      {/* <Employee
        name="Tomek Kowalski"
        email="tomek.kowalski@example.com"
        phone="+48 123 456 789"
        salary={5000}
      />
      <Employee
        name="Anna Nowak"
        email="anna.nowak@example.com"
        phone="+48 987 654 321"
        salary={6000}
      />
      <Employee
        name="Jan Wiśniewski"
        email="jan.wisniewski@example.com"
        phone="+48 111 222 333"
        salary={4500}
      /> */}
    </div>
  );
};

export default EmployeeList;
