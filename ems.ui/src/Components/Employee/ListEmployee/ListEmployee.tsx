import React, { useState, useEffect, JSX } from "react";
import {
  DataTable,
  DataTableExpandedRows,
  DataTableValueArray,
} from "primereact/datatable";
import { Column } from "primereact/column";
import { EmployeeGet } from "../../../Models/Employee";
import { Dialog } from "primereact/dialog";
import { Button } from "primereact/button";
import { InputText } from "primereact/inputtext";
import { IconField } from "primereact/iconfield";
import { InputIcon } from "primereact/inputicon";
import { Card } from "primereact/card";
import { Paginator, PaginatorPageChangeEvent } from "primereact/paginator";
import { EmployeeListGet } from "../../../Models/EmployeeList";
import {
  DeleteEmployeesListService,
  DeleteEmployeesService,
  GetUserEmployeesService,
  GetUserListEmployeesService,
} from "../../../Services/EmployeeService";
import AddEmployee from "../AddEmployee/AddEmployee";
import AddListEmployee from "../AddListEmployee/AddListEmployee";
import ConfirmationDialog from "../../Confirmation/ConfirmationDialog";
import UpdateEmployee from "../UpdateEmployee/UpdateEmployee";
import { Dropdown } from "primereact/dropdown";

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

  const [firstEmployee, setFirstEmployee] = useState(0);
  const [rowsEmployee, setRowsEmployee] = useState(10);
  const [totalEmployees, setTotalEmployees] = useState(0);

  const [expandedRows, setExpandedRows] = useState<
    DataTableExpandedRows | DataTableValueArray | undefined
  >(undefined);

  const [sortOrderSalary, setSortOrderSalary] = useState<string | null>(null);

  const sortSalaryOptions = [
    { label: "None", value: null },
    { label: "Salary ↑", value: "salary_asc" },
    { label: "Salary ↓", value: "salary_desc" },
  ];

  const resetFilters = () => {
    setSearchTerm("");
    setSortOrderSalary(null);
  };

  const fetchEmployees = async (page: number, size: number) => {
    const data = await GetUserEmployeesService(
      page,
      size,
      searchTerm,
      sortOrderSalary
    );
    setEmployees(data.employeeGet);
    setTotalEmployees(data.totalItems);
  };

  const goToPage = (page: number, rows: number) => {
    const newFirst = (page - 1) * rows;
    setFirstEmployee(newFirst);
    fetchEmployees(page, rows);
  };

  useEffect(() => {
    goToPage(1, rowsEmployee);
  }, [searchTerm, sortOrderSalary]);

  const fetchEmployeesList = async () => {
    const data = await GetUserListEmployeesService(searchTermList);

    setEmployeesList(data);
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
      await DeleteEmployeesService(deleteId);

      const totalAfterDelete = totalEmployees - 1;
      const maxPage = Math.ceil(totalAfterDelete / rowsEmployee);
      let currentPage = Math.floor(firstEmployee / rowsEmployee) + 1;

      if (currentPage > maxPage) {
        currentPage = maxPage;
      }

      goToPage(currentPage, rowsEmployee);
    }
    setConfirmEmployeeVisible(false);
    setDeleteId(null);
  };

  const handleAddSuccess = () => {
    const currentPage = Math.floor(firstEmployee / rowsEmployee) + 1;
    goToPage(currentPage, rowsEmployee);
  };

  const handleUpdateSuccess = () => {
    const currentPage = Math.floor(firstEmployee / rowsEmployee) + 1;
    goToPage(currentPage, rowsEmployee);
    setUpdateVisible(false);
  };

  const handleConfirmDeleteListEmployee = async () => {
    if (deleteId) {
      await DeleteEmployeesListService(deleteId);
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

  const onPageChangeEmployees = (event: PaginatorPageChangeEvent) => {
    setFirstEmployee(event.first);
    setRowsEmployee(event.rows);
    fetchEmployees(event.page + 1, event.rows);
  };

  const rowExpansionTemplate = (data: EmployeeListGet) => {
    return (
      <div>
        {data.employees ? (
          <div className="flex flex-row flex-wrap">
            {data.employees.map((employee) => (
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
            ))}
          </div>
        ) : (
          <div>No Employees in this list</div>
        )}
      </div>
    );
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
        <Dropdown
          value={sortOrderSalary}
          options={sortSalaryOptions}
          onChange={(e) => setSortOrderSalary(e.value)}
          placeholder="Sort by Salary"
          showClear
        />
        <Button
          label="Reset Filters"
          icon="pi pi-refresh"
          onClick={resetFilters}
        />
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
            onAddSuccess={handleAddSuccess}
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
      <Paginator
        first={firstEmployee}
        rows={rowsEmployee}
        totalRecords={totalEmployees}
        onPageChange={onPageChangeEmployees}
        rowsPerPageOptions={[5, 10, 20, 30]}
        style={{ border: "none" }}
      />
      <Dialog
        header="Add List Employees"
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
          <InputIcon className="pi pi-search"> </InputIcon>
          <InputText
            value={searchTermList}
            onChange={(e) => setSearchTermList(e.target.value)}
            placeholder="Search"
          />
        </IconField>
        <Button
          label="Add List Employees"
          onClick={() => setVisibleListEmploeyee(true)}
        />
      </div>
      <DataTable
        value={employeesList}
        paginator
        rows={10}
        rowsPerPageOptions={[5, 10, 25, 50]}
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
        header="Confirm Deletion of List Employees"
        message="Are you sure you want to delete this List employees?"
        onConfirm={handleConfirmDeleteListEmployee}
        onCancel={() => setConfirmListEmployeeVisible(false)}
      />
      <ConfirmationDialog
        visible={confirmEmployeeVisible}
        header="Confirm Deletion of Employee"
        message="Are you sure you want to delete this employee?"
        onConfirm={handleConfirmDeleteEmployee}
        onCancel={() => setConfirmEmployeeVisible(false)}
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
            onUpdateSuccess={handleUpdateSuccess}
          />
        )}
      </Dialog>
    </div>
  );
};

export default EmployeeList;
