import React, { useEffect, useState } from "react";
import { TreeTable } from "primereact/treetable";
import { Column } from "primereact/column";
import { TaskGet } from "../../../Models/Task";
import { InputText } from "primereact/inputtext";
import { Button } from "primereact/button";
import { Dialog } from "primereact/dialog";
import {
  DataTable,
  DataTableExpandedRows,
  DataTableRowEvent,
  DataTableValueArray,
} from "primereact/datatable";
import { IconField } from "primereact/iconfield";
import { InputIcon } from "primereact/inputicon";

import { Tag } from "primereact/tag";
import { Dropdown } from "primereact/dropdown";
import { SplitButton } from "primereact/splitbutton";
import { Paginator } from "primereact/paginator";
import {
  DeleteTaskService,
  UserGetTaskService,
  UserUpdateTaskStatusService,
} from "../../../Services/TaskService";
import { StatusOfTask } from "../../../Enum/StatusOfTask";
import {
  dateBodyTemplate,
  statusOfTaskBodyTemplate,
} from "../../Utils/TaskTemplates";
import ConfirmationDialog from "../../Confirmation/ConfirmationDialog";
import AddTask from "../AddTask/AddTask";

type Props = {};

const ListTask = (props: Props) => {
  const [tasks, setTasks] = useState<TaskGet[]>([]);
  const [searchTerm, setSearchTerm] = useState("");
  const [visible, setVisible] = useState<boolean>(false);
  const [deleteId, setDeleteId] = useState<string | null>(null);
  const [confirmVisible, setConfirmVisible] = useState<boolean>(false);

  const [firstTask, setFirstTask] = useState(0);
  const [rowsTask, setRowsTask] = useState(10);
  const [totalTasks, setTotalTasks] = useState(0);

  const [expandedRows, setExpandedRows] = useState<
    DataTableExpandedRows | DataTableValueArray | undefined
  >(undefined);

  const fetchTasks = async (page: number, size: number) => {
    const data = await UserGetTaskService(page, size, searchTerm);
    setTasks(data.taskGet);
    setTotalTasks(data.totalItems);
  };

  const goToPage = (page: number, rows: number) => {
    const newFirst = (page - 1) * rows;
    setFirstTask(newFirst);
    fetchTasks(page, rows);
  };

  useEffect(() => {
    goToPage(1, rowsTask);
  }, [searchTerm]);

  const allowExpansion = (rowData: TaskGet) => {
    return rowData.id!.length > 0;
  };

  const showDeleteConfirmation = (id: string) => {
    setDeleteId(id);
    setConfirmVisible(true);
  };

  const handleConfirmDelete = async () => {
    if (deleteId) {
      await DeleteTaskService(deleteId);

      const totalAfterDelete = totalTasks - 1;
      const maxPage = Math.ceil(totalAfterDelete / rowsTask);
      let currentPage = Math.floor(firstTask / rowsTask) + 1;

      if (currentPage > maxPage) {
        currentPage = maxPage;
      }

      goToPage(currentPage, rowsTask);
    }
    setConfirmVisible(false);
    setDeleteId(null);
  };

  const handleAddSuccess = () => {
    const currentPage = Math.floor(firstTask / rowsTask) + 1;
    goToPage(currentPage, rowsTask);
  };

  const onPageChangeTasks = (event: any) => {
    setFirstTask(event.first);
    setRowsTask(event.rows);
    fetchTasks(event.page + 1, event.rows);
  };

  const rowExpansionTemplate = (data: TaskGet) => {
    const handleStatusChange = async (
      taskId: string,
      newStatus: StatusOfTask
    ) => {
      await UserUpdateTaskStatusService(taskId, newStatus);
      // const updatedTasks = tasks.map((task) => {
      //   if (task.id === taskId) {
      //     return { ...task, status: newStatus };
      //   }
      //   return task;
      // });
      // setTasks(updatedTasks);
      fetchTasks(1, rowsTask);
    };

    return (
      <div className="grid">
        <div className="col ml-4">
          <div>
            <h4>Task Details</h4>
            <p>
              <strong>Description:</strong> {data.description}
            </p>
          </div>
        </div>
        <div className="col">
          <h4>Address</h4>
          {data.address ? (
            <>
              <p>
                <strong>City:</strong> {data.address.city}
              </p>
              <p>
                <strong>Street:</strong> {data.address.street}
              </p>
              <p>
                <strong>Number:</strong> {data.address.number}
              </p>
              <p>
                <strong>Zip Code:</strong> {data.address.zipCode}
              </p>
            </>
          ) : (
            <p>No available address for this task.</p>
          )}
        </div>
        <div className="col">
          <Dropdown
            value={data.status}
            options={[
              { label: "Active", value: 1 },
              { label: "Done", value: 2 },
              { label: "Archive", value: 3 },
            ]}
            onChange={(e) => handleStatusChange(data.id, e.value)}
            placeholder="Select a status"
          />
        </div>
        <div className="col">
          <h4>Assigned Employee Lists</h4>
          {data.employeeLists && data.employeeLists.length > 0 ? (
            data.employeeLists.map((employeeList) => (
              <SplitButton
                key={employeeList.id}
                label={employeeList.name}
                className="m-2"
                severity="info"
                model={
                  employeeList.employees?.map((employee) => ({
                    label: employee.name,
                    icon: "pi pi-user",
                    key: employee.id,
                  })) ?? []
                }
              />
            ))
          ) : (
            <p>No Employee List assigned.</p>
          )}
        </div>
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
        <Button label="Add Task" onClick={() => setVisible(true)} />
        <Dialog
          header="Add Task"
          visible={visible}
          onHide={() => {
            if (!visible) return;
            setVisible(false);
          }}
        >
          <AddTask
            onClose={() => setVisible(false)}
            onAddSuccess={handleAddSuccess}
          />
        </Dialog>
      </div>
      <DataTable
        value={tasks}
        expandedRows={expandedRows}
        onRowToggle={(e) => setExpandedRows(e.data)}
        rowExpansionTemplate={rowExpansionTemplate}
        dataKey="id"
        tableStyle={{ minWidth: "50rem" }}
      >
        <Column expander={allowExpansion} style={{ width: "5rem" }} />
        <Column field="id" header="Id"></Column>
        <Column field="name" header="Name"></Column>
        <Column
          field="startDate"
          body={(rowData) => dateBodyTemplate(rowData, "startDate")}
          header="Start Date"
        ></Column>
        <Column
          field="endDate"
          body={(rowData) => dateBodyTemplate(rowData, "endDate")}
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
            <>
              <i
                className="pi pi-trash"
                style={{ fontSize: "1.5rem", cursor: "pointer" }}
                onClick={() => showDeleteConfirmation(rowData.id)}
              ></i>
            </>
          )}
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
      <ConfirmationDialog
        visible={confirmVisible}
        header="Confirm Deletion of Task"
        message="Are you sure you want to delete this Task?"
        onConfirm={handleConfirmDelete}
        onCancel={() => setConfirmVisible(false)}
      />
    </div>
  );
};

export default ListTask;
