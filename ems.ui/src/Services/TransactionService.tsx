import axios from "axios";
import { BudgetGet, BudgetPost } from "../Models/Budget";
import { TransactionGet, TransactionPost } from "../Models/Transaction";

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
  transactionPost: TransactionPost
) => {
  return await axios.post<TransactionPost>(
    api + "Transaction",
    transactionPost
  );
};
