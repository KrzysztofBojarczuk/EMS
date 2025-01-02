import React, { useState, useEffect, JSX } from "react";
import { DataTable } from "primereact/datatable";
import { Column } from "primereact/column";
import { EmployeeGet } from "../../../Models/Employee";
import { UserGetEmployeesService } from "../../../Services/EmployeeService.tsx";
import { Dialog } from "primereact/dialog";
import { Button } from "primereact/button";
import AddEmployee from "../AddEmployee/AddEmployee.tsx";
import { InputText } from "primereact/inputtext";

interface Props {}

const EmployeeList: React.FC<Props> = (props: Props): JSX.Element => {
  const [employees, setEmployees] = useState<EmployeeGet[]>([]);
  const [searchTerm, setSearchTerm] = useState("");
  const [visible, setVisible] = useState(false);

  const fetchEmployees = async () => {
    const data = await UserGetEmployeesService(searchTerm);
    setEmployees(data);
  };

  useEffect(() => {
    fetchEmployees();
  }, [searchTerm]);

  return (
    <div className="card m-4">
      <div className="flex align-items-center justify-content-start mb-4">
        <InputText
          value={searchTerm}
          onChange={(e) => setSearchTerm(e.target.value)}
          className="mr-4"
          placeholder="Search"
        />
        <Button
          label="Add Employee"
          onClick={() => setVisible(true)}
          className="mr-4"
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
          <AddEmployee
            onClose={() => setVisible(false)}
            onAddSuccess={fetchEmployees}
          />
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
