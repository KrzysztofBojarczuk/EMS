import { Tag } from "primereact/tag";
import { StatusOfTask } from "../../Enum/StatusOfTask.ts";
import { TaskGet } from "../../Models/Task";
import { formatDate } from "./DateUtils.ts";

export const statusToText: Record<StatusOfTask, string> = {
  [StatusOfTask.Active]: "Active",
  [StatusOfTask.Done]: "Done",
  [StatusOfTask.Archive]: "Archive",
};

export const getStatusOfTask = (task: TaskGet) => {
  switch (task.status) {
    case StatusOfTask.Active:
      return "success";
    case StatusOfTask.Done:
      return "warning";
    case StatusOfTask.Archive:
      return "info";
    default:
      return undefined;
  }
};

export const statusOfTaskBodyTemplate = (rowData: TaskGet) => (
  <Tag
    value={statusToText[rowData.status]}
    severity={getStatusOfTask(rowData)}
  ></Tag>
);

export const dateBodyTemplate = (
  rowData: TaskGet,
  field: "startDate" | "endDate"
) => {
  return formatDate(rowData[field]);
};
