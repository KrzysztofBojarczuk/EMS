import React, { JSX, useEffect, useRef, useState } from "react";
import { UserGet } from "../../Models/User.ts";
import {
  UserGetService,
  UserDeleteService,
  GetNumberOfUsersService,
} from "../../Services/UserService.tsx";
import { InputText } from "primereact/inputtext";
import { DataTable } from "primereact/datatable";
import { Column } from "primereact/column";
import { EmployeeGet } from "../../Models/Employee.ts";
import {
  GetEmployeesService,
  UserDeleteEmployeesService,
} from "../../Services/EmployeeService.tsx";
import ConfirmationDialog from "../Confirmation/ConfirmationDialog.tsx";
import { Button } from "primereact/button";
import { Panel } from "primereact/panel";

type Props = {};

const AdministrationPanel: React.FC<Props> = (props: Props): JSX.Element => {
  const [numberUser, setNumberUsers] = useState<number>(0);
  const [user, setUsers] = useState<UserGet[]>([]);
  const [searchUserTerm, setSearchUserTerm] = useState("");
  const [searchEmployeeTerm, setSearchEmployeeTerm] = useState("");
  const [employees, setEmployees] = useState<EmployeeGet[]>([]);
  const [deleteId, setDeleteId] = useState<string | null>(null);
  const [deleteType, setDeleteType] = useState<"user" | "employee" | null>(
    null
  );
  const [confirmVisible, setConfirmVisible] = useState(false);
  const userPanelRef = useRef<Panel>(null);
  const employeePanelRef = useRef<Panel>(null);

  const fetchNumberUsers = async () => {
    const data = await GetNumberOfUsersService();
    setNumberUsers(data);
  };

  useEffect(() => {
    fetchNumberUsers();
  }, []);

  const fetchUser = async () => {
    const data = await UserGetService(searchUserTerm);
    setUsers(data);
  };

  useEffect(() => {
    fetchUser();
  }, [searchUserTerm]);

  const fetchEmployees = async () => {
    const data = await GetEmployeesService(searchEmployeeTerm);
    setEmployees(data);
  };

  useEffect(() => {
    fetchEmployees();
  }, [searchEmployeeTerm]);

  return (
    <div className="card m-4">
      <div className="my-4">
        <Button
          label="Expand Users"
          onClick={() => userPanelRef.current?.expand(undefined)}
        />
        <Button
          label="Collapse Users"
          className="ml-4"
          onClick={() => userPanelRef.current?.collapse(undefined)}
        />
      </div>
      <Panel ref={userPanelRef} header="Users" toggleable collapsed>
        <div className="flex align-items-center justify-content-start mb-4">
          <InputText
            value={searchUserTerm}
            onChange={(e) => setSearchUserTerm(e.target.value)}
            className="mr-4"
            placeholder="Search Users"
          />
          <div className="hover:text-green-300">
            Number of users: {numberUser}
          </div>
        </div>
        <DataTable value={user} tableStyle={{ minWidth: "50rem" }}>
          <Column field="id" header="Id"></Column>
          <Column field="userName" header="User name"></Column>
          <Column field="email" header="Email"></Column>
        </DataTable>
      </Panel>
      <div className="my-4">
        <Button
          label="Expand Employees"
          onClick={() => employeePanelRef.current?.expand(undefined)}
        />
        <Button
          label="Collapse Employees"
          className="ml-4"
          onClick={() => employeePanelRef.current?.collapse(undefined)}
        />
      </div>
      <Panel ref={employeePanelRef} header="Employees" toggleable collapsed>
        <div className="mt-4">
          <InputText
            value={searchEmployeeTerm}
            onChange={(e) => setSearchEmployeeTerm(e.target.value)}
            className="mr-4"
            placeholder="Search Employees"
          />
        </div>
        <DataTable value={employees} tableStyle={{ minWidth: "50rem" }}>
          <Column field="id" header="Id"></Column>
          <Column field="name" header="Name"></Column>
          <Column field="email" header="Email"></Column>
          <Column field="phone" header="Phone"></Column>
          <Column field="salary" header="Salary"></Column>
        </DataTable>
      </Panel>
    </div>
  );
};

export default AdministrationPanel;
