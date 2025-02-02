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
import { TaskGet } from "../../Models/Task.ts";
import {
  GetTaskService,
  DeleteTaskService,
} from "../../Services/TaskService.tsx";
import { Tag } from "primereact/tag";
import { StatusOfTask } from "../../Enum/StatusOfTask.ts";

const AdministrationPanel: React.FC = (): JSX.Element => {
  const [numberUser, setNumberUsers] = useState<number>(0);
  const [users, setUsers] = useState<UserGet[]>([]);
  const [employees, setEmployees] = useState<EmployeeGet[]>([]);
  const [tasks, setTasks] = useState<TaskGet[]>([]);

  const [searchUserTerm, setSearchUserTerm] = useState("");
  const [searchEmployeeTerm, setSearchEmployeeTerm] = useState("");
  const [searchTaskTerm, setSearchTaskTerm] = useState("");

  const [deleteUserId, setDeleteUserId] = useState<string | null>(null);
  const [deleteEmployeeId, setDeleteEmployeeId] = useState<string | null>(null);
  const [deleteTaskId, setDeleteTaskId] = useState<string | null>(null);

  const [confirmUserVisible, setConfirmUserVisible] = useState(false);
  const [confirmEmployeeVisible, setConfirmEmployeeVisible] = useState(false);
  const [confirmTaskVisible, setConfirmTaskVisible] = useState(false);

  const [firstEmployee, setFirstEmployee] = useState(0);
  const [rowsEmployee, setRowsEmployee] = useState(10);
  const [totalEmployees, setTotalEmployees] = useState(0);

  const [firstTask, setFirstTask] = useState(0);
  const [rowsTask, setRowsTask] = useState(10);
  const [totalTasks, setTotalTasks] = useState(0);

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

  const fetchUsers = async () => {
    const data = await UserGetService(searchUserTerm);
    setUsers(data);
  };

  useEffect(() => {
    fetchUsers();
  }, [searchUserTerm]);

  const fetchEmployees = async (page: number, size: number) => {
    const data = await GetEmployeesService(page, size, searchEmployeeTerm);
    setEmployees(data.employeeGet);
    setTotalEmployees(data.totalItems);
  };

  useEffect(() => {
    fetchEmployees(1, rowsEmployee);
  }, [searchEmployeeTerm]);

  const fetchTasks = async (page: number, size: number) => {
    const data = await GetTaskService(page, size, searchTaskTerm);
    setTasks(data.taskGet);
    setTotalTasks(data.totalItems);
  };

  useEffect(() => {
    fetchTasks(1, rowsTask);
  }, [searchTaskTerm]);

  const showUserDeleteConfirmation = (id: string) => {
    setDeleteUserId(id);
    setConfirmUserVisible(true);
  };

  const showEmployeeDeleteConfirmation = (id: string) => {
    setDeleteEmployeeId(id);
    setConfirmEmployeeVisible(true);
  };

  const showTaskDeleteConfirmation = (id: string) => {
    setDeleteTaskId(id);
    setConfirmTaskVisible(true);
  };

  const handleDeleteUser = async () => {
    if (deleteUserId) {
      await UserDeleteService(deleteUserId);
      fetchUsers();
      fetchNumberUsers();
    }
    setConfirmUserVisible(false);
    setDeleteUserId(null);
  };

  const handleDeleteEmployee = async () => {
    if (deleteEmployeeId) {
      await UserDeleteEmployeesService(deleteEmployeeId);
      fetchEmployees(1, rowsEmployee);
    }
    setConfirmEmployeeVisible(false);
    setDeleteEmployeeId(null);
  };

  const handleDeleteTask = async () => {
    if (deleteTaskId) {
      await DeleteTaskService(deleteTaskId);
      fetchTasks(1, rowsTask);
    }
    setConfirmTaskVisible(false);
    setDeleteTaskId(null);
  };

  const onPageChangeEmployees = (event: any) => {
    setFirstEmployee(event.first);
    setRowsEmployee(event.rows);
    fetchEmployees(event.page + 1, event.rows);
  };

  const onPageChangeTasks = (event: any) => {
    setFirstTask(event.first);
    setRowsTask(event.rows);
    fetchTasks(event.page + 1, event.rows);
  };

  const statusToText = {
    [StatusOfTask.Active]: "Active",
    [StatusOfTask.Done]: "Done",
    [StatusOfTask.Archive]: "Archive",
  };

  const statusOfTaskBodyTemplate = (rowData) => {
    return (
      <Tag
        value={statusToText[rowData.status]}
        severity={getStatusOfTask(rowData)}
      ></Tag>
    );
  };

  const getStatusOfTask = (task) => {
    switch (task.status) {
      case StatusOfTask.Active:
        return "success";
      case StatusOfTask.Done:
        return "warning";
      case StatusOfTask.Archive:
        return "info";
      default:
        return null;
    }
  };

  const formatDate = (value: Date) => {
    return value.toLocaleDateString("en-EN", {
      day: "2-digit",
      month: "long",
      year: "numeric",
    });
  };

  const dateBodyTemplate = (rowData: TaskGet) => {
    return formatDate(new Date(rowData.startDate));
  };

  return (
    <div className="card m-4">
      <Panel ref={userPanelRef} header="Users" toggleable collapsed>
        <InputText
          value={searchUserTerm}
          onChange={(e) => setSearchUserTerm(e.target.value)}
          placeholder="Search Users"
        />
        <DataTable value={users} paginator rows={5}>
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
          <Column
            header="Action"
            body={(rowData) => (
              <i
                className="pi pi-trash"
                style={{ fontSize: "1.5rem", cursor: "pointer" }}
                onClick={() => showEmployeeDeleteConfirmation(rowData.id)}
              ></i>
            )}
          />
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
            body={dateBodyTemplate}
            header="Start Date"
          ></Column>

          <Column
            field="endDate"
            dataType="date"
            body={dateBodyTemplate}
            header="End Date"
          ></Column>
          <Column
            field="status"
            header="Status"
            body={statusOfTaskBodyTemplate}
          ></Column>
          <Column
            header="Action"
            body={(rowData) => (
              <i
                className="pi pi-trash"
                style={{ fontSize: "1.5rem", cursor: "pointer" }}
                onClick={() => showTaskDeleteConfirmation(rowData.id)}
              ></i>
            )}
          />
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
        onConfirm={handleDeleteUser}
        onCancel={() => setConfirmUserVisible(false)}
      />

      <ConfirmationDialog
        visible={confirmEmployeeVisible}
        header="Confirm Employee Deletion"
        message="Are you sure you want to delete this employee?"
        onConfirm={handleDeleteEmployee}
        onCancel={() => setConfirmEmployeeVisible(false)}
      />

      <ConfirmationDialog
        visible={confirmTaskVisible}
        header="Confirm Task Deletion"
        message="Are you sure you want to delete this task?"
        onConfirm={handleDeleteTask}
        onCancel={() => setConfirmTaskVisible(false)}
      />
    </div>
  );
};

export default AdministrationPanel;
