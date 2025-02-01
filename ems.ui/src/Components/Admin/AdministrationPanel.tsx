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
import { Paginator } from "primereact/paginator";

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

  const [pageNumber, setPageNumber] = useState(1);
  const [pageSize] = useState(10);
  const [totalPages, setTotalPages] = useState(1);
  const [totalItems, setTotalItems] = useState(0);

  const [first, setFirst] = useState(0);
  const [rows, setRows] = useState(10);

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

  const fetchEmployees = async (page: number, size: number) => {
    const data = await GetEmployeesService(page, size, searchEmployeeTerm);
    setEmployees(data.employeeGet);
    setTotalItems(data.totalItems);
    setTotalPages(Math.ceil(data.totalItems / size));
  };

  useEffect(() => {
    fetchEmployees(pageNumber, pageSize);
  }, [searchEmployeeTerm, pageNumber, pageSize]);

  const showDeleteConfirmation = (id: string, type: "user" | "employee") => {
    setDeleteId(id);
    setDeleteType(type);
    setConfirmVisible(true);
  };

  const handleDeleteUser = async () => {
    if (deleteId) {
      await UserDeleteService(deleteId);
      fetchUser();
      fetchNumberUsers();
    }
    setConfirmVisible(false);
    setDeleteId(null);
    setDeleteType(null);
  };

  const handleDeleteEmployee = async () => {
    if (deleteId) {
      await UserDeleteEmployeesService(deleteId);
      fetchEmployees(pageNumber, pageSize);
    }
    setConfirmVisible(false);
    setDeleteId(null);
    setDeleteType(null);
  };

  const onPageChange = (event: any) => {
    setPageNumber(event.page + 1);
    setFirst(event.first);
    setRows(event.rows);
    fetchEmployees(event.page + 1, event.rows);
  };

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
        <DataTable
          value={user}
          paginator
          rows={5}
          rowsPerPageOptions={[5, 10, 25, 50]}
          tableStyle={{ minWidth: "50rem" }}
        >
          <Column field="id" header="Id" />
          <Column field="userName" header="User name" />
          <Column field="email" header="Email" />
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
          />
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
          <Column field="id" header="Id" />
          <Column field="name" header="Name" />
          <Column field="email" header="Email" />
          <Column field="phone" header="Phone" />
          <Column field="salary" header="Salary" />
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
          />
        </DataTable>

        <Paginator
          first={first}
          rows={rows}
          totalRecords={totalItems}
          rowsPerPageOptions={[5, 10, 20, 30]}
          onPageChange={onPageChange}
          style={{ border: "none" }}
        />
      </Panel>

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
