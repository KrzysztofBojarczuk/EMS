import React, { JSX, useEffect, useRef, useState } from "react";
import { InputText } from "primereact/inputtext";
import {
  DataTable,
  DataTableExpandedRows,
  DataTableValueArray,
} from "primereact/datatable";
import { Panel } from "primereact/panel";
import { Paginator, PaginatorPageChangeEvent } from "primereact/paginator";
import { EmployeeGet } from "../../Models/Employee";
import { TaskGet } from "../../Models/Task";
import { UserGet } from "../../Models/User";
import {
  GetNumberOfUsersService,
  DeleteUserService,
  GetAllUsersService,
} from "../../Services/UserService";
import { GetAllEmployeesService } from "../../Services/EmployeeService";
import { GetAllTasksService } from "../../Services/TaskService";
import { Column } from "primereact/column";
import {
  dateBodyTemplate,
  statusOfTaskBodyTemplate,
} from "../Utils/TaskTemplates";
import ConfirmationDialog from "../Confirmation/ConfirmationDialog";
import { IconField } from "primereact/iconfield";
import { InputIcon } from "primereact/inputicon";
import { formatCurrency } from "../Utils/Currency";
import { formatDate } from "../Utils/DateUtils";
import { LogGet } from "../../Models/Logs";
import { GetLogsService } from "../../Services/LogsService";
import { Calendar } from "primereact/calendar";
import { Dropdown } from "primereact/dropdown";
import { Button } from "primereact/button";

const AdministrationPanel: React.FC = (): JSX.Element => {
  const [numberUser, setNumberUsers] = useState<number>(0);
  const [users, setUsers] = useState<UserGet[]>([]);
  const [employees, setEmployees] = useState<EmployeeGet[]>([]);
  const [tasks, setTasks] = useState<TaskGet[]>([]);
  const [logs, setLogs] = useState<LogGet[]>([]);

  const [searchUserTerm, setSearchUserTerm] = useState("");
  const [searchEmployeeTerm, setSearchEmployeeTerm] = useState("");
  const [searchTaskTerm, setSearchTaskTerm] = useState("");
  const [searchLogTerm, setSearchLogTerm] = useState("");

  const [dateFrom, setDateFrom] = useState<Date | null>(null);
  const [dateTo, setDateTo] = useState<Date | null>(null);
  const [sortOrder, setSortOrderLog] = useState<string | null>(null);

  const [deleteUserId, setDeleteUserId] = useState<string | null>(null);

  const [confirmUserVisible, setConfirmUserVisible] = useState(false);

  const [firstEmployee, setFirstEmployee] = useState(0);
  const [rowsEmployee, setRowsEmployee] = useState(10);
  const [totalEmployees, setTotalEmployees] = useState(0);

  const [firstTask, setFirstTask] = useState(0);
  const [rowsTask, setRowsTask] = useState(10);
  const [totalTasks, setTotalTasks] = useState(0);

  const [firstUser, setFirstUser] = useState(0);
  const [rowsUser, setRowsUser] = useState(10);
  const [totalUsers, setTotalUsers] = useState(0);

  const [firstLog, setFirstLog] = useState(0);
  const [rowsLog, setRowsLog] = useState(10);
  const [totalLog, setTotalLog] = useState(0);

  const userPanelRef = useRef<Panel>(null);
  const employeePanelRef = useRef<Panel>(null);
  const taskPanelRef = useRef<Panel>(null);

  const [expandedRows, setExpandedRows] = useState<
    DataTableExpandedRows | DataTableValueArray | undefined
  >(undefined);

  const resetFilters = () => {
    setDateFrom(null);
    setDateTo(null);
    setSortOrderLog(null);
    setSearchLogTerm("");
  };

  const sortOptions = [
    { label: "None", value: null },
    { label: "Date ↑ (Oldest first)", value: "creatdate_asc" },
    { label: "Date ↓ (Newest first)", value: "creatdate_desc" },
  ];

  const fetchNumberUsers = async () => {
    const data = await GetNumberOfUsersService();
    setNumberUsers(data);
  };

  useEffect(() => {
    fetchNumberUsers();
  }, []);

  const fetchUsers = async (page: number, size: number) => {
    const data = await GetAllUsersService(page, size, searchUserTerm);
    setUsers(data.userGet);
    setTotalUsers(data.totalItems);
  };

  const goToPageUser = (page: number, rows: number) => {
    const newFirst = (page - 1) * rows;
    setFirstUser(newFirst);
    fetchUsers(page, rows);
  };

  useEffect(() => {
    goToPageUser(1, rowsUser);
  }, [searchUserTerm]);

  const fetchEmployees = async (page: number, size: number) => {
    const data = await GetAllEmployeesService(page, size, searchEmployeeTerm);
    setEmployees(data.employeeGet);
    setTotalEmployees(data.totalItems);
  };

  const goToPageEmployees = (page: number, rows: number) => {
    const newFirst = (page - 1) * rows;
    setFirstEmployee(newFirst);
    fetchEmployees(page, rows);
  };

  useEffect(() => {
    goToPageEmployees(1, rowsEmployee);
  }, [searchEmployeeTerm]);

  const fetchTasks = async (page: number, size: number) => {
    const data = await GetAllTasksService(page, size, searchTaskTerm);
    setTasks(data.taskGet);
    setTotalTasks(data.totalItems);
  };

  const goToPageTasks = (page: number, rows: number) => {
    const newFirst = (page - 1) * rows;
    setFirstTask(newFirst);
    fetchTasks(page, rows);
  };

  useEffect(() => {
    goToPageTasks(1, rowsTask);
  }, [searchTaskTerm]);

  const fetchLogs = async (page: number, size: number) => {
    const data = await GetLogsService(
      page,
      size,
      searchLogTerm,
      dateFrom,
      dateTo,
      sortOrder
    );
    setLogs(data.logs);
    setTotalLog(data.totalItems);
  };

  const goToPageLogs = (page: number, rows: number) => {
    const newFirst = (page - 1) * rows;
    setFirstLog(newFirst);
    fetchLogs(page, rows);
  };

  useEffect(() => {
    goToPageLogs(1, rowsLog);
  }, [searchLogTerm, dateFrom, dateTo, sortOrder]);

  const showUserDeleteConfirmation = (id: string) => {
    setDeleteUserId(id);
    setConfirmUserVisible(true);
  };

  const handleConfirmDeleteUser = async () => {
    if (deleteUserId) {
      await DeleteUserService(deleteUserId);

      const totalAfterDelete = totalUsers - 1;
      const maxPage = Math.ceil(totalAfterDelete / rowsUser);
      let currentPage = Math.floor(firstUser / rowsUser) + 1;

      if (currentPage > maxPage) {
        currentPage = maxPage;
      }

      goToPageUser(currentPage, rowsUser);
      fetchNumberUsers();
    }
    setConfirmUserVisible(false);
    setDeleteUserId(null);
  };

  const onPageChangeLogs = (event: PaginatorPageChangeEvent) => {
    setFirstLog(event.first);
    setRowsLog(event.rows);
    fetchLogs(event.page + 1, event.rows);
  };

  const onPageChangeEmployees = (event: PaginatorPageChangeEvent) => {
    setFirstEmployee(event.first);
    setRowsEmployee(event.rows);
    fetchEmployees(event.page + 1, event.rows);
  };

  const onPageChangeTasks = (event: PaginatorPageChangeEvent) => {
    setFirstTask(event.first);
    setRowsTask(event.rows);
    fetchTasks(event.page + 1, event.rows);
  };

  const onPageChangeUsers = (event: PaginatorPageChangeEvent) => {
    setFirstUser(event.first);
    setRowsUser(event.rows);
    fetchUsers(event.page + 1, event.rows);
  };

  const allowExpansion = (rowData: TaskGet) => {
    return rowData.id!.length > 0;
  };

  const rowExpansionTemplate = (log: LogGet) => {
    return (
      <div style={{ padding: "1rem" }}>
        <h5>Request Data</h5>
        <p>{log.requestData}</p>
      </div>
    );
  };

  return (
    <div className="xl:m-4 lg:m-4 md:m-2">
      <Panel ref={userPanelRef} header="Users" toggleable collapsed>
        <IconField iconPosition="left">
          <InputIcon className="pi pi-search"> </InputIcon>
          <InputText
            value={searchUserTerm}
            onChange={(e) => setSearchUserTerm(e.target.value)}
            placeholder="Search"
          />
        </IconField>
        <DataTable value={users}>
          <Column field="id" header="Id" />
          <Column field="userName" header="User name" />
          <Column field="email" header="Email" />
          <Column
            header="Action"
            body={(rowData) => (
              <i
                className="pi pi-trash"
                style={{ fontSize: "1.5rem", cursor: "pointer" }}
                onClick={() => showUserDeleteConfirmation(rowData.id)}
              ></i>
            )}
          />
        </DataTable>
        <Paginator
          first={firstUser}
          rows={rowsUser}
          totalRecords={totalUsers}
          onPageChange={onPageChangeUsers}
          rowsPerPageOptions={[5, 10, 20, 30]}
          style={{ border: "none" }}
        />
      </Panel>

      <Panel ref={employeePanelRef} header="Employees" toggleable collapsed>
        <IconField iconPosition="left">
          <InputIcon className="pi pi-search"> </InputIcon>
          <InputText
            value={searchEmployeeTerm}
            onChange={(e) => setSearchEmployeeTerm(e.target.value)}
            placeholder="Search"
          />
        </IconField>
        <DataTable value={employees}>
          <Column field="id" header="Id" />
          <Column field="name" header="Name" />
          <Column field="email" header="Email" />
          <Column field="phone" header="Phone"></Column>
          <Column
            field="salary"
            header="Salary"
            body={(rowData) => formatCurrency(rowData.salary)}
          ></Column>
          <Column field="age" header="Age"></Column>
          <Column
            field="employmentDate"
            header="Employment Date"
            body={(rowData) => formatDate(rowData.employmentDate)}
          ></Column>
          <Column
            field="medicalCheckValidUntil"
            header="Medical Check"
            body={(rowData) => formatDate(rowData.medicalCheckValidUntil)}
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
      </Panel>

      <Panel ref={taskPanelRef} header="Tasks" toggleable collapsed>
        <IconField iconPosition="left">
          <InputIcon className="pi pi-search"> </InputIcon>
          <InputText
            value={searchTaskTerm}
            onChange={(e) => setSearchTaskTerm(e.target.value)}
            placeholder="Search"
          />
        </IconField>
        <DataTable value={tasks}>
          <Column field="id" header="Id"></Column>
          <Column field="name" header="Name"></Column>
          <Column
            field="startDate"
            dataType="date"
            header="Start Date"
            body={(rowData) => dateBodyTemplate(rowData, "startDate")}
          ></Column>

          <Column
            field="endDate"
            dataType="date"
            header="End Date"
            body={(rowData) => dateBodyTemplate(rowData, "endDate")}
          ></Column>
          <Column
            field="status"
            header="Status"
            body={statusOfTaskBodyTemplate}
          ></Column>
        </DataTable>
        <Paginator
          first={firstTask}
          rows={rowsTask}
          totalRecords={totalTasks}
          onPageChange={onPageChangeTasks}
          rowsPerPageOptions={[5, 10, 20, 30]}
          style={{ border: "none" }}
        />
      </Panel>

      <Panel ref={userPanelRef} header="Logs" toggleable collapsed>
        <div className="flex justify-content-start xl:flex-row lg:flex-row md:flex-column sm:flex-column gap-3 my-4">
          <IconField iconPosition="left">
            <InputIcon className="pi pi-search" />
            <InputText
              value={searchLogTerm}
              onChange={(e) => setSearchLogTerm(e.target.value)}
              placeholder="Search"
            />
          </IconField>
          <Calendar
            value={dateFrom}
            onChange={(e) => setDateFrom(e.value as Date)}
            placeholder="Date created from"
            showIcon
            dateFormat="dd/mm/yy"
          />
          <Calendar
            value={dateTo}
            onChange={(e) => setDateTo(e.value as Date)}
            placeholder="Date created to"
            showIcon
            dateFormat="dd/mm/yy"
          />
          <Dropdown
            value={sortOrder}
            options={sortOptions}
            onChange={(e) => setSortOrderLog(e.value)}
            placeholder="Sorting"
          />
          <Button
            label="Reset Filters"
            icon="pi pi-refresh"
            onClick={resetFilters}
          />
        </div>
        <DataTable
          value={logs}
          expandedRows={expandedRows}
          onRowToggle={(e) => setExpandedRows(e.data)}
          rowExpansionTemplate={rowExpansionTemplate}
          dataKey="id"
          tableStyle={{ minWidth: "50rem" }}
        >
          <Column expander={allowExpansion} style={{ width: "5rem" }} />
          <Column field="id" header="Id"></Column>
          <Column field="username" header="Username" />
          <Column field="action" header="Action" />
          <Column field="status" header="Status" />
          <Column field="ipAddress" header="IP Address" />
          <Column
            field="createdAt"
            header="Created At"
            body={(rowData) => formatDate(rowData.createdAt)}
          />
        </DataTable>
        <Paginator
          first={firstLog}
          rows={rowsLog}
          totalRecords={totalLog}
          onPageChange={onPageChangeLogs}
          rowsPerPageOptions={[5, 10, 20, 30]}
          style={{ border: "none" }}
        />
      </Panel>

      <ConfirmationDialog
        visible={confirmUserVisible}
        header="Confirm User Deletion"
        message="Are you sure you want to delete this user?"
        onConfirm={handleConfirmDeleteUser}
        onCancel={() => setConfirmUserVisible(false)}
      />
    </div>
  );
};

export default AdministrationPanel;
