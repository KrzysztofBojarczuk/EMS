import React, { useState, useEffect, JSX } from "react";
import { DataTable } from "primereact/datatable";
import { Column } from "primereact/column";
import { EmployeeGet } from "../../../Models/Employee";
import {
  UserGetEmployeesService,
  UserDeleteEmployeesService,
} from "../../../Services/EmployeeService.tsx";
import { Dialog } from "primereact/dialog";
import { Button } from "primereact/button";
import AddEmployee from "../AddEmployee/AddEmployee.tsx";
import { InputText } from "primereact/inputtext";
import ConfirmationDialog from "../../Confirmation/ConfirmationDialog.tsx";
import UpdateEmployee from "../UpdateEmployee/UpdateEmployee.tsx";
import { IconField } from "primereact/iconfield";
import { InputIcon } from "primereact/inputicon";

interface Props {}

const EmployeeList: React.FC<Props> = (props: Props): JSX.Element => {
  const [employees, setEmployees] = useState<EmployeeGet[]>([]);
  const [searchTerm, setSearchTerm] = useState("");
  const [visible, setVisible] = useState<boolean>(false);
  const [confirmVisible, setConfirmVisible] = useState<boolean>(false);
  const [deleteId, setDeleteId] = useState<string | null>(null);
  const [selectedEmployee, setSelectedEmployee] = useState<EmployeeGet | null>(
    null
  );
  const [updateVisible, setUpdateVisible] = useState(false);

  const fetchEmployees = async () => {
    const data = await UserGetEmployeesService(searchTerm);
    setEmployees(data);
  };

  useEffect(() => {
    fetchEmployees();
  }, [searchTerm]);

  const showDeleteConfirmation = (id: string) => {
    setDeleteId(id);
    setConfirmVisible(true);
  };

  const handleConfirmDelete = async () => {
    if (deleteId) {
      await UserDeleteEmployeesService(deleteId);
      fetchEmployees();
    }
    setConfirmVisible(false);
    setDeleteId(null);
  };

  const showUpdateDialog = (employee: EmployeeGet) => {
    setSelectedEmployee(employee);
    setUpdateVisible(true);
  };

  return (
    <div className="xl:m-4 lg:m-4 md:m-2">
      <div className="flex justify-content-start xl:flex-row lg:flex-row md:flex-column sm:flex-column gap-3 my-4">
        <IconField iconPosition="left">
          <InputIcon className="pi pi-search"> </InputIcon>
          <InputText
            value={searchTerm}
            onChange={(e) => setSearchTerm(e.target.value)}
            placeholder="Search"
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
                onClick={() => showDeleteConfirmation(rowData.id)}
              ></i>
            </>
          )}
        ></Column>
      </DataTable>

      <ConfirmationDialog
        visible={confirmVisible}
        header="Confirm Deletion of Employee"
        message="Are you sure you want to delete this employee?"
        onConfirm={handleConfirmDelete}
        onCancel={() => setConfirmVisible(false)}
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
