import axios from "axios";
import { BudgetGet, BudgetPost } from "../Models/Budget";

const api = "https://localhost:7256/api/";

export const UserGetBudgetService = async () => {
  const response = await axios.get<BudgetGet>(api + "Budget/User");
  return response.data;
};

export const UserPostBudgetService = async (budgetPost: BudgetPost) => {
  return await axios.post<BudgetPost>(api + "Budget", budgetPost);
};

export const UserDeleteBudgetService = async (id: string) => {
  const response = await axios.delete(`${api}Budget/${id}`);
  return response;
};
