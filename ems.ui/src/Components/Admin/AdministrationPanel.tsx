import React, { JSX, useEffect, useState } from "react";
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

type Props = {};

const AdministrationPanel: React.FC<Props> = (props: Props): JSX.Element => {
  //Tworzy zmienną numberUser, która przechowuje aktualną liczbę użytkowników, oraz funkcję setNumberUsers, która pozwala na aktualizację tej liczby.
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

  //Funkcja fetchNumberUsers jest asynchroniczna i korzysta z serwisu GetNumberOfUsersService, aby pobrać liczbę użytkowników z backendu.
  const fetchNumberUsers = async () => {
    const data = await GetNumberOfUsersService();
    setNumberUsers(data);
  };

  //useEffect uruchamia funkcję fetchNumberUsers po pierwszym wyrenderowaniu komponentu. Dzięki temu liczba użytkowników jest pobierana automatycznie, gdy komponent zostanie załadowany.
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

  const showDeleteConfirmation = (id: string, type: "user" | "employee") => {
    setDeleteId(id);
    setDeleteType(type);
    setConfirmVisible(true);
  };

  const handleDeleteUser = async () => {
    if (deleteId) {
      await UserDeleteService(deleteId);
      fetchUser();
      fetchEmployees();
      fetchNumberUsers();
    }
    setConfirmVisible(false);
    setDeleteId(null);
    setDeleteType(null);
  };

  const handleDeleteEmployee = async () => {
    if (deleteId) {
      await UserDeleteEmployeesService(deleteId);
      fetchEmployees();
    }
    setConfirmVisible(false);
    setDeleteId(null);
    setDeleteType(null);
  };

  return (
    <div className="card m-4">
      <div>
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
          <Column
            header="Action"
            body={(rowData) => (
              <>
                <i
                  className="pi pi-trash"
                  style={{ fontSize: "1.5rem", cursor: "pointer" }}
                  onClick={() => showDeleteConfirmation(rowData.id, "user")}
                ></i>
              </>
            )}
          ></Column>
        </DataTable>
      </div>

      <div className="mt-4">
        <div className="flex align-items-center justify-content-start mb-4">
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
          <Column
            header="Action"
            body={(rowData) => (
              <>
                <i
                  className="pi pi-trash"
                  style={{ fontSize: "1.5rem", cursor: "pointer" }}
                  onClick={() => showDeleteConfirmation(rowData.id, "employee")}
                ></i>
              </>
            )}
          ></Column>
        </DataTable>
      </div>

      <ConfirmationDialog
        visible={confirmVisible}
        header={`Confirm Deletion of ${
          deleteType === "user" ? "User" : "Employee"
        }`}
        message={`Are you sure you want to delete this ${
          deleteType === "user" ? "user" : "employee"
        }?`}
        onConfirm={
          deleteType === "user" ? handleDeleteUser : handleDeleteEmployee
        }
        onCancel={() => {
          setConfirmVisible(false);
          setDeleteId(null);
          setDeleteType(null);
        }}
      />
    </div>
  );
};

export default AdministrationPanel;
