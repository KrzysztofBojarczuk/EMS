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

type Props = {};

const ListTask = (props: Props) => {
  const [tasks, setTasks] = useState<TaskGet[]>([]);
  const [searchTerm, setSearchTerm] = useState("");
  const [visible, setVisible] = useState<boolean>(false);
  const [expandedRows, setExpandedRows] = useState<
    DataTableExpandedRows | DataTableValueArray | undefined
  >(undefined);

  const fetchTask = async () => {
    const data = await UserGetTaskService(searchTerm);
    setTasks(data);
  };

  useEffect(() => {
    fetchTask();
  }, [searchTerm]);

  const allowExpansion = (rowData: TaskGet) => {
    return rowData.id!.length > 0;
  };

  const rowExpansionTemplate = (data) => {
    return (
      <div className="p-3">
        <h5>Task Details</h5>
        <p>
          <strong>Description:</strong> {data.description}
        </p>
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
      </DataTable>
    </div>
  );
};

export default ListTask;
