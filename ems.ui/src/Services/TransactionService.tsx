import axios from "axios";
import { BudgetGet, BudgetPost } from "../Models/Budget";
import { TransactionGet, TransactionPost } from "../Models/Transaction";

const api = "https://localhost:7256/api/";

export const UserTransactionService = async () => {
  const response = await axios.get<TransactionGet>(api + "Transaction");

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
