import React, { useState, useEffect, JSX } from "react";
import { DataTable } from "primereact/datatable";
import { Column } from "primereact/column";
import { EmployeeGet } from "../../../Models/Employee";
import { UserGetEmployeesService } from "../../../Services/EmployeeService.tsx";
import { Dialog } from "primereact/dialog";
import { Button } from "primereact/button";
import AddEmployee from "../AddEmployee/AddEmployee.tsx";

interface Props {}

const EmployeeList: React.FC<Props> = (props: Props): JSX.Element => {
  const [employees, setEmployees] = useState<EmployeeGet[]>([]);
  const [visible, setVisible] = useState(false);

  useEffect(() => {
    UserGetEmployeesService().then((data) => setEmployees(data));
  }, []);

  return (
    <div className="card m-4">
      <div>
        <Button
          label="Add Employee"
          onClick={() => setVisible(true)}
          className="mb-4"
        />

        <Dialog
          header="Add Employee"
          visible={visible}
          style={{ width: "50vw" }}
          onHide={() => {
            if (!visible) return;
            setVisible(false);
          }}
        >
          <AddEmployee onClose={() => setVisible(false)} />
        </Dialog>
      </div>
      <DataTable value={employees} tableStyle={{ minWidth: "50rem" }}>
        <Column field="id" header="Id"></Column>
        <Column field="name" header="Name"></Column>
        <Column field="email" header="Email"></Column>
        <Column field="phone" header="Phone"></Column>
        <Column field="salary" header="Salary"></Column>
      </DataTable>
    </div>
  );
};

export default EmployeeList;
