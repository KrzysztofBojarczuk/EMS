import React, { JSX, useState, useEffect } from "react";
import { TreeTable } from "primereact/treetable";
import { EmployeeGet } from "../../../Models/Employee";
import { fetchUserEmployees } from "../../../Services/EmployeeService.tsx";

interface Props {}

const EmployeeList: React.FC<Props> = (props: Props): JSX.Element => {
  const [employees, setEmployees] = useState<EmployeeGet[]>([]);

  useEffect(() => {
    fetchUserEmployees().then((data) => setEmployees(data));
  }, []);

  return (
    <div className="card">
      <TreeTable
        value={employees}
        tableStyle={{ minWidth: "50rem" }}
      ></TreeTable>
    </div>
  );
};

export default EmployeeList;
