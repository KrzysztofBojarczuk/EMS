import { CategoryType } from "../Enum/CategoryType";

export interface TransactionGet {
  id: string;
  name: string;
  createdAt: string;
  category: CategoryType;
  amount: number;
}

export interface TransactionPost {
  name: string;
  createdAt: string;
  category: CategoryType;
  amount: number;
}
