import React, { JSX, useEffect, useRef, useState } from "react";
import { InputText } from "primereact/inputtext";
import { DataTable } from "primereact/datatable";
import { Panel } from "primereact/panel";
import { Paginator, PaginatorPageChangeEvent } from "primereact/paginator";
import { EmployeeGet } from "../../Models/Employee";
import { TaskGet } from "../../Models/Task";
import { UserGet } from "../../Models/User";
import {
  GetNumberOfUsersService,
  UserDeleteService,
  UserGetService,
} from "../../Services/UserService";
import { GetEmployeesService } from "../../Services/EmployeeService";
import { GetTaskService } from "../../Services/TaskService";
import { Column } from "primereact/column";
import {
  dateBodyTemplate,
  statusOfTaskBodyTemplate,
} from "../Utils/TaskTemplates";
import ConfirmationDialog from "../Confirmation/ConfirmationDialog";

const AdministrationPanel: React.FC = (): JSX.Element => {
  const [numberUser, setNumberUsers] = useState<number>(0);
  const [users, setUsers] = useState<UserGet[]>([]);
  const [employees, setEmployees] = useState<EmployeeGet[]>([]);
  const [tasks, setTasks] = useState<TaskGet[]>([]);

  const [searchUserTerm, setSearchUserTerm] = useState("");
  const [searchEmployeeTerm, setSearchEmployeeTerm] = useState("");
  const [searchTaskTerm, setSearchTaskTerm] = useState("");

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

  const userPanelRef = useRef<Panel>(null);
  const employeePanelRef = useRef<Panel>(null);
  const taskPanelRef = useRef<Panel>(null);

  const fetchNumberUsers = async () => {
    const data = await GetNumberOfUsersService();
    setNumberUsers(data);
  };

  useEffect(() => {
    fetchNumberUsers();
  }, []);

  const fetchUsers = async (page: number, size: number) => {
    const data = await UserGetService(page, size, searchUserTerm);
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
    const data = await GetEmployeesService(page, size, searchEmployeeTerm);
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
    const data = await GetTaskService(page, size, searchTaskTerm);
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

  const showUserDeleteConfirmation = (id: string) => {
    setDeleteUserId(id);
    setConfirmUserVisible(true);
  };

  const handleConfirmDeleteUser = async () => {
    if (deleteUserId) {
      await UserDeleteService(deleteUserId);

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

  return (
    <div className="card m-4">
      <Panel ref={userPanelRef} header="Users" toggleable collapsed>
        <InputText
          value={searchUserTerm}
          onChange={(e) => setSearchUserTerm(e.target.value)}
          placeholder="Search Users"
        />
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
        <InputText
          value={searchEmployeeTerm}
          onChange={(e) => setSearchEmployeeTerm(e.target.value)}
          placeholder="Search Employees"
        />
        <DataTable value={employees}>
          <Column field="id" header="Id" />
          <Column field="name" header="Name" />
          <Column field="email" header="Email" />
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
        <InputText
          value={searchTaskTerm}
          onChange={(e) => setSearchTaskTerm(e.target.value)}
          placeholder="Search Tasks"
        />
        <DataTable value={tasks}>
          <Column field="id" header="Id"></Column>
          <Column field="name" header="Name"></Column>
          <Column
            field="startDate"
            dataType="date"
            body={(rowData) => dateBodyTemplate(rowData, "startDate")}
            header="Start Date"
          ></Column>

          <Column
            field="endDate"
            dataType="date"
            body={(rowData) => dateBodyTemplate(rowData, "endDate")}
            header="End Date"
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
