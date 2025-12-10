import axios from "axios";
import { BudgetGet, BudgetPost } from "../Models/Budget";

const api = "https://localhost:7256/api/";

export const PostBudgetService = async (budgetPost: BudgetPost) => {
  const response = await axios.post<BudgetPost>(api + "Budget", budgetPost);
  return response.data;
};

export const GetUserBudgetService = async () => {
  const response = await axios.get<BudgetGet>(api + "Budget/User");
  return response.data;
};

export const DeleteBudgetService = async (id: string) => {
  const response = await axios.delete(`${api}Budget/${id}`);
  return response.data;
};
