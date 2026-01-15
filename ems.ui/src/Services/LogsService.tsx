import axios from "axios";
import { StatusOfTask } from "../Enum/StatusOfTask";
import { PaginatedLogResponse } from "../Models/Logs";

const api = "https://localhost:7256/api/";

export const GetLogsService = async (
  pageNumber: number,
  pageSize: number,
  searchTerm?: string,
  dateFrom?: Date | null,
  dateTo?: Date | null,
  sortOrder?: string | null
) => {
  const params = new URLSearchParams();

  params.append("pageNumber", pageNumber.toString());
  params.append("pageSize", pageSize.toString());

  if (searchTerm) params.append("searchTerm", searchTerm);

  if (dateFrom) params.append("dateFrom", dateFrom.toISOString());
  if (dateTo) params.append("dateTo", dateTo.toISOString());

  if (sortOrder) params.append("sortOrder", sortOrder);

  const response = await axios.get<PaginatedLogResponse>(
    `${api}Logs?${params.toString()}`
  );

  console.log(response.data);
  return response.data;
};
