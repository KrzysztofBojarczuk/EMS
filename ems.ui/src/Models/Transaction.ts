import { CategoryType } from "../Enum/CategoryType";

export interface TransactionGet {
  id: string;
  name: string;
  creationDate: string;
  category: CategoryType;
  amount: number;
}

export interface TransactionPost {
  name: string;
  creationDate: string;
  category: CategoryType;
  amount: number;
}
