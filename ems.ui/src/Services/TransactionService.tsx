import axios from "axios";
import { BudgetGet, BudgetPost } from "../Models/Budget";
import { TransactionGet, TransactionPost } from "../Models/Transaction";
import { GetUserBudgetService } from "./BudgetService";

const api = "https://localhost:7256/api/";

export const PostTransactionService = async (
  budgetId: string,
  transactionPost: TransactionPost
) => {
  await GetUserBudgetService();
  const response = await axios.post<TransactionPost>(
    `${api}Transaction/${budgetId}`,
    transactionPost
  );
  return response.data;
};

export const GetUserTransactionsByBudgetIdService = async (
  budgetId: string,
  searchTerm?: string,
  category?: string[],
  dateFrom?: Date | null,
  dateTo?: Date | null,
  amountFrom?: number | null,
  amountTo?: number | null,
  sortOrder?: string | null
) => {
  const params = new URLSearchParams();

  if (searchTerm) params.append("searchTerm", searchTerm);

  if (category && category.length > 0) {
    category.forEach((cat) => params.append("category", cat));
  }

  if (dateFrom && dateTo) {
    params.append("dateFrom", dateFrom.toISOString());
    params.append("dateTo", dateTo.toISOString());
  }

  if (amountFrom != null && amountTo != null) {
    params.append("amountFrom", amountFrom.toString());
    params.append("amountTo", amountTo.toString());
  }

  if (sortOrder) params.append("sortOrder", sortOrder);

  const response = await axios.get<TransactionGet[]>(
    `${api}Transaction/${budgetId}?${params.toString()}`
  );
  return response.data;
};

export const DeleteTransactionService = async (id: string) => {
  const response = await axios.delete(`${api}Transaction/${id}`);
  return response.data;
};
