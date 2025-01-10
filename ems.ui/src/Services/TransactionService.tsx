import axios from "axios";
import { BudgetGet, BudgetPost } from "../Models/Budget";
import { TransactionGet, TransactionPost } from "../Models/Transaction";
import { UserGetBudgetService } from "./BudgetService.tsx";

const api = "https://localhost:7256/api/";

export const UserGetTransactionService = async (
  budgetId: string,
  searchTerm?: string,
  category?: string[]
) => {
  const response = await axios.get<TransactionGet[]>(
    `${api}Transaction/${budgetId}`,
    {
      params: {
        category: category,
        searchTerm: searchTerm,
      },
    }
  );
  return response.data;
};

export const UserPostTransactionService = async (
  budgetId: string,
  transactionPost: TransactionPost
) => {
  await UserGetBudgetService();
  return await axios.post<TransactionPost>(
    `${api}Transaction/${budgetId}`,
    transactionPost
  );
};

export const UserDeleteTransactionService = async (id: string) => {
  const response = await axios.delete(`${api}Transaction/${id}`);
  return response;
};
