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

export const GetUserTransactionByBudgetIdService = async (
  budgetId: string,
  searchTerm?: string,
  category?: string[]
) => {
  const url = `${api}Transaction/${budgetId}?searchTerm=${searchTerm}${
    category && category.length > 0
      ? `&${category.map((cat) => `category=${cat}`).join("&")}`
      : ""
  }`;

  const response = await axios.get<TransactionGet[]>(url);
  return response.data;
};

export const DeleteTransactionService = async (id: string) => {
  const response = await axios.delete(`${api}Transaction/${id}`);
  return response.data;
};
