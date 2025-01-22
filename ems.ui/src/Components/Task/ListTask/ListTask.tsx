import React, { useEffect, useState } from "react";
import { TreeTable } from "primereact/treetable";
import { Column } from "primereact/column";
import { TaskGet } from "../../../Models/Task";
import { InputText } from "primereact/inputtext";
import { Button } from "primereact/button";
import { Dialog } from "primereact/dialog";
import {
  UserGetTaskService,
  UserDeleteTaskService,
} from "../../../Services/TaskService.tsx";
import AddTask from "../AddTask/AddTask.tsx";
import {
  DataTable,
  DataTableExpandedRows,
  DataTableRowEvent,
  DataTableValueArray,
} from "primereact/datatable";
import { IconField } from "primereact/iconfield";
import { InputIcon } from "primereact/inputicon";
import { AddressGet } from "../../../Models/Address.ts";
import ConfirmationDialog from "../../Confirmation/ConfirmationDialog.tsx";
import { StatusOfTask } from "../../../Enum/StatusOfTask.ts";
import { Tag } from "primereact/tag";

type Props = {};

const ListTask = (props: Props) => {
  const [tasks, setTasks] = useState<TaskGet[]>([]);
  const [searchTerm, setSearchTerm] = useState("");
  const [visible, setVisible] = useState<boolean>(false);
  const [deleteId, setDeleteId] = useState<string | null>(null);
  const [confirmVisible, setConfirmVisible] = useState<boolean>(false);

  const [expandedRows, setExpandedRows] = useState<
    DataTableExpandedRows | DataTableValueArray | undefined
  >(undefined);

  const fetchTask = async () => {
    const data = await UserGetTaskService(searchTerm);
    setTasks(data);
    console.log(data);
  };

  useEffect(() => {
    fetchTask();
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
      await UserDeleteTaskService(deleteId);
      fetchTask();
    }
    setConfirmVisible(false);
    setDeleteId(null);
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

  const rowExpansionTemplate = (data) => {
    return (
      <div className="p-3">
        <div>
          <h4>Task Details</h4>
          <p>
            <strong>Description:</strong> {data.description}
          </p>
        </div>
        <div className="mt-5">
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
          <AddTask onClose={() => setVisible(false)} onAddSuccess={fetchTask} />
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
      <ConfirmationDialog
        visible={confirmVisible}
        header="Confirm Deletion of Taske"
        message="Are you sure you want to delete this Task?"
        onConfirm={handleConfirmDelete}
        onCancel={() => setConfirmVisible(false)}
      />
    </div>
  );
};

export default ListTask;
