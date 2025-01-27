import React, { useState, useEffect, JSX } from "react";
import {
  DataTable,
  DataTableExpandedRows,
  DataTableValueArray,
} from "primereact/datatable";
import { Column } from "primereact/column";
import { EmployeeGet } from "../../../Models/Employee";
import {
  UserGetEmployeesService,
  UserDeleteEmployeesService,
  UserGetListEmployeesService,
  UserDeleteEmployeesListService,
} from "../../../Services/EmployeeService.tsx";
import { Dialog } from "primereact/dialog";
import { Button } from "primereact/button";
import AddEmployee from "../AddEmployee/AddEmployee.tsx";
import { InputText } from "primereact/inputtext";
import ConfirmationDialog from "../../Confirmation/ConfirmationDialog.tsx";
import UpdateEmployee from "../UpdateEmployee/UpdateEmployee.tsx";
import { IconField } from "primereact/iconfield";
import { InputIcon } from "primereact/inputicon";
import AddListEmployee from "../AddListEmployee/AddListEmployee.tsx";
import { EmployeeListGet } from "../../../Models/EmployeeList.tsx";
import { Card } from "primereact/card";

interface Props {}

const EmployeeList: React.FC<Props> = (props: Props): JSX.Element => {
  const [employees, setEmployees] = useState<EmployeeGet[]>([]);
  const [employeesList, setEmployeesList] = useState<EmployeeListGet[]>([]);
  const [searchTerm, setSearchTerm] = useState("");
  const [searchTermList, setSearchTermList] = useState("");
  const [visible, setVisible] = useState<boolean>(false);
  const [visibleListEmploeyee, setVisibleListEmploeyee] =
    useState<boolean>(false);
  const [confirmEmployeeVisible, setConfirmEmployeeVisible] =
    useState<boolean>(false);
  const [confirmListEmployeeVisible, setConfirmListEmployeeVisible] =
    useState<boolean>(false);
  const [deleteId, setDeleteId] = useState<string | null>(null);
  const [selectedEmployee, setSelectedEmployee] = useState<EmployeeGet | null>(
    null
  );
  const [updateVisible, setUpdateVisible] = useState(false);

  const [expandedRows, setExpandedRows] = useState<
    DataTableExpandedRows | DataTableValueArray | undefined
  >(undefined);

  const fetchEmployees = async () => {
    const data = await UserGetEmployeesService(searchTerm);
    setEmployees(data);
  };

  useEffect(() => {
    fetchEmployees();
  }, [searchTerm]);

  const fetchEmployeesList = async () => {
    const data = await UserGetListEmployeesService(searchTermList);

    const transformedList: EmployeeListGet[] = data.map((employee) => ({
      id: employee.id,
      name: employee.name,
      employees: [employee],
    }));
    setEmployeesList(transformedList);
  };

  useEffect(() => {
    fetchEmployeesList();
  }, [searchTermList]);

  const showDeleteEmployeeConfirmation = (id: string) => {
    setDeleteId(id);
    setConfirmEmployeeVisible(true);
  };

  const showDeleteListEmployeeConfirmation = (id: string) => {
    setDeleteId(id);
    setConfirmListEmployeeVisible(true);
  };

  const handleConfirmDeleteEmployee = async () => {
    if (deleteId) {
      await UserDeleteEmployeesService(deleteId);
      fetchEmployees();
    }
    setConfirmEmployeeVisible(false);
    setDeleteId(null);
  };

  const handleConfirmDeleteListEmployee = async () => {
    if (deleteId) {
      await UserDeleteEmployeesListService(deleteId);
      fetchEmployeesList();
    }
    setConfirmListEmployeeVisible(false);
    setDeleteId(null);
  };

  const showUpdateDialog = (employee: EmployeeGet) => {
    setSelectedEmployee(employee);
    setUpdateVisible(true);
  };

  const allowExpansion = (rowData: EmployeeListGet) => {
    return rowData.id!.length > 0;
  };

  const rowExpansionTemplate = (data) => {
    return (
      <div>
        {data.employees && data.employees.length > 0
          ? data.employees.map((group) => (
              <div className="flex flex-row flex-wrap" key={group.id}>
                {group.employees && group.employees.length > 0 ? (
                  group.employees.map((employee) => (
                    <div key={employee.id}>
                      <Card
                        className="flex flex-column m-2"
                        style={{ border: "2px solid #81c784" }}
                      >
                        <p>
                          <strong>Employee Name:</strong> {employee.name}
                        </p>
                        <p>
                          <strong>Email:</strong> {employee.email}
                        </p>
                        <p>
                          <strong>Phone:</strong> {employee.phone}
                        </p>
                        <p>
                          <strong>Salary:</strong> {employee.salary}
                        </p>
                      </Card>
                    </div>
                  ))
                ) : (
                  <div>No Employees in this list</div>
                )}
              </div>
            ))
          : null}
      </div>
    );
  };

  return (
    <div className="xl:m-4 lg:m-4 md:m-2">
      <div className="flex justify-content-start xl:flex-row lg:flex-row md:flex-column sm:flex-column gap-3 my-4">
        <IconField iconPosition="left">
          <InputText
            value={searchTerm}
            onChange={(e) => setSearchTerm(e.target.value)}
            placeholder="Search Name"
          />
        </IconField>
        <Button label="Add Employee" onClick={() => setVisible(true)} />

        <Dialog
          header="Add Employee"
          visible={visible}
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
        <Column
          header="Action"
          body={(rowData) => (
            <>
              <i
                className="pi pi-pencil"
                style={{
                  fontSize: "1.5rem",
                  cursor: "pointer",
                  marginRight: "10px",
                }}
                onClick={() => showUpdateDialog(rowData)}
              ></i>
              <i
                className="pi pi-trash"
                style={{ fontSize: "1.5rem", cursor: "pointer" }}
                onClick={() => showDeleteEmployeeConfirmation(rowData.id)}
              ></i>
            </>
          )}
        ></Column>
      </DataTable>

      <Dialog
        header="Add Employee"
        visible={visibleListEmploeyee}
        onHide={() => setVisibleListEmploeyee(false)}
      >
        <AddListEmployee
          onClose={() => setVisibleListEmploeyee(false)}
          onAddSuccess={fetchEmployeesList}
        />
      </Dialog>
      <div className="flex justify-content-start xl:flex-row lg:flex-row md:flex-column sm:flex-column gap-3 my-4">
        <IconField iconPosition="left">
          <InputText
            value={searchTermList}
            onChange={(e) => setSearchTermList(e.target.value)}
            placeholder="Search Name"
          />
        </IconField>
        <Button
          label="Create List Employees"
          onClick={() => setVisibleListEmploeyee(true)}
        />
      </div>
      <DataTable
        value={employeesList}
        expandedRows={expandedRows}
        dataKey="id"
        onRowToggle={(e) => setExpandedRows(e.data)}
        rowExpansionTemplate={rowExpansionTemplate}
        tableStyle={{ minWidth: "50rem" }}
      >
        <Column expander={allowExpansion} style={{ width: "5rem" }} />
        <Column field="id" header="Id"></Column>
        <Column field="name" header="Name"></Column>
        <Column
          header="Action"
          body={(rowData) => (
            <>
              <i
                className="pi pi-trash"
                style={{ fontSize: "1.5rem", cursor: "pointer" }}
                onClick={() => showDeleteListEmployeeConfirmation(rowData.id)}
              ></i>
            </>
          )}
        ></Column>
      </DataTable>
      <ConfirmationDialog
        visible={confirmListEmployeeVisible}
        header="Confirm Deletion of List Employee"
        message="Are you sure you want to delete this List employee?"
        onConfirm={handleConfirmDeleteListEmployee}
        onCancel={() => setConfirmEmployeeVisible(false)}
      />
      <ConfirmationDialog
        visible={confirmEmployeeVisible}
        header="Confirm Deletion of Employee"
        message="Are you sure you want to delete this employee?"
        onConfirm={handleConfirmDeleteEmployee}
        onCancel={() => setConfirmListEmployeeVisible(false)}
      />
      <Dialog
        header="Update Employee"
        visible={updateVisible}
        onHide={() => setUpdateVisible(false)}
      >
        {selectedEmployee && (
          <UpdateEmployee
            employee={selectedEmployee}
            onClose={() => setUpdateVisible(false)}
            onUpdateSuccess={fetchEmployees}
          />
        )}
      </Dialog>
    </div>
  );
};

export default EmployeeList;
