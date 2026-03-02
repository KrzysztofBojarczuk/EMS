import React, { useEffect, useState } from "react";
import { TabView, TabPanel } from "primereact/tabview";
import { Button } from "primereact/button";
import { InputText } from "primereact/inputtext";
import { SelectButton } from "primereact/selectbutton";
import { DataView, DataViewLayoutOptions } from "primereact/dataview";
import { Tag } from "primereact/tag";
import { Dialog } from "primereact/dialog";
import { classNames } from "primereact/utils";
import "./BudgetTransaction.css";
import { IconField } from "primereact/iconfield";
import { InputIcon } from "primereact/inputicon";
import { BudgetGet, BudgetPost } from "../../Models/Budget";
import { TransactionGet } from "../../Models/Transaction";
import {
  DeleteBudgetService,
  GetUserBudgetService,
  PostBudgetService,
} from "../../Services/BudgetService";
import {
  DeleteTransactionService,
  GetUserTransactionsByBudgetIdService,
} from "../../Services/TransactionService";
import { CategoryType } from "../../Enum/CategoryType";
import ConfirmationDialog from "../Confirmation/ConfirmationDialog";
import AddTransaction from "./AddTransaction/AddTransaction";
import { Dropdown } from "primereact/dropdown";
import { sortOptionsTransactions } from "../Utils/SortOptions";
import { Calendar } from "primereact/calendar";
import { formatDate, formatDateTime } from "../Utils/DateUtils";

const BudgetTransaction = () => {
  const [budgetUser, setBudgetUser] = useState<BudgetGet | null>(null);
  const [transaction, setTransaction] = useState<TransactionGet[]>([]);
  const [searchTerm, setSearchTerm] = useState<string>("");
  const [category, setCategory] = useState<string[]>([]);
  const [confirmVisible, setConfirmVisible] = useState<boolean>(false);
  const [deleteId, setDeleteId] = useState<string | null>(null);
  const [visible, setVisible] = useState<boolean>(false);
  const [value, setValue] = useState(null);
  const [isDialogVisible, setIsDialogVisible] = useState<boolean>(false);

  const [dateFrom, setDateFrom] = useState<Date | null>(null);
  const [dateTo, setDateTo] = useState<Date | null>(null);
  const [amountFrom, setAmountFrom] = useState<number | null>(null);
  const [amountTo, setAmountTo] = useState<number | null>(null);
  const [sortOrderTransaction, setSortOrderTransaction] = useState<
    string | null
  >(null);

  const items = [
    { name: "Income", value: 1 },
    { name: "Expense", value: 2 },
  ];

  const resetFilters = () => {
    setSearchTerm("");
    setCategory([]);
    setDateFrom(null);
    setDateTo(null);
    setAmountFrom(null);
    setAmountTo(null);
    setSortOrderTransaction(null);
  };

  const fetchBudgetUser = async () => {
    const data = await GetUserBudgetService();
    setBudgetUser(data);
  };

  const fetchTransaction = async () => {
    if (budgetUser) {
      const data = await GetUserTransactionsByBudgetIdService(
        budgetUser.id,
        searchTerm,
        category,
        dateFrom,
        dateTo,
        amountFrom,
        amountTo,
        sortOrderTransaction,
      );
      setTransaction(data);
      setConfirmVisible(false);
    }
  };

  useEffect(() => {
    fetchBudgetUser();
  }, []);

  useEffect(() => {
    fetchTransaction();
  }, [
    budgetUser,
    searchTerm,
    category,
    dateFrom,
    dateTo,
    amountFrom,
    amountTo,
    sortOrderTransaction,
  ]);

  const handleSubmitAddBudget = async () => {
    const newBudget: BudgetPost = {
      budget: 0,
    };
    await PostBudgetService(newBudget);
    await fetchBudgetUser();
  };

  const handleConfirmDeleteBudget = async () => {
    if (budgetUser?.id) {
      await DeleteBudgetService(budgetUser.id);
      setBudgetUser(null);
      setIsDialogVisible(false);
    }
  };

  const handleCancelDeleteBudget = () => {
    setIsDialogVisible(false);
  };

  const handleDeleteBudget = () => {
    setIsDialogVisible(true);
  };

  const showDeleteConfirmation = (id: string) => {
    setDeleteId(id);
    setConfirmVisible(true);
  };

  const handleConfirmDeleteTransaction = async () => {
    if (deleteId) {
      await DeleteTransactionService(deleteId);
      await fetchTransaction();
      await fetchBudgetUser();
    }
    setConfirmVisible(false);
    setDeleteId(null);
  };

  const categoryToText = {
    [CategoryType.Income]: "Income",
    [CategoryType.Expense]: "Expense",
  };

  const getSeverity = (transaction: TransactionGet) => {
    switch (transaction.category) {
      case CategoryType.Income:
        return "success";
      case CategoryType.Expense:
        return "danger";
      default:
        return null;
    }
  };

  const buttonToggleCategory = (selectedCategories: string[]) => {
    setCategory(selectedCategories);
  };

  const itemTemplate = (transaction: TransactionGet, index: number) => {
    return (
      <div className="col-12" key={transaction.id}>
        <div
          className={classNames(
            "flex flex-column xl:flex-row xl:align-items-start p-4 gap-4",
            { "border-top-1 surface-border": index !== 0 },
          )}
        >
          <div className="flex flex-column sm:flex-row justify-content-between align-items-center xl:align-items-start flex-1 gap-4">
            <div className="flex flex-column align-items-center sm:align-items-start gap-3">
              <div className="text-2xl font-bold text-900">
                {transaction.name}
              </div>
              <div className="flex align-items-center gap-3">
                <span className="flex align-items-center gap-2">
                  <i className="pi pi-money-bill"></i>
                </span>
                <Tag
                  severity={getSeverity(transaction)}
                  value={categoryToText[transaction.category]}
                ></Tag>
              </div>
            </div>
            <div className="flex sm:flex-column align-items-center sm:align-items-end gap-3 sm:gap-2">
              <span className="text-2xl font-semibold">
                {transaction.category === 1
                  ? "+"
                  : transaction.category === 2
                    ? "-"
                    : ""}
                {transaction.amount}
                <i className="pi pi-euro ml-2"></i>
                <i
                  className="pi pi-trash ml-2"
                  style={{ fontSize: "1.0rem", cursor: "pointer" }}
                  onClick={() => showDeleteConfirmation(transaction.id)}
                ></i>
              </span>
              <p>{formatDateTime(transaction.createdAt)}</p>
            </div>
          </div>
        </div>
      </div>
    );
  };

  const listTemplate = (items: TransactionGet[]) => {
    if (!items || items.length === 0) return null;
    return <div>{items.map(itemTemplate)}</div>;
  };

  return (
    <div className="flex justify-content-center xl:m-4 lg:m-4 md:m-2">
      {budgetUser ? (
        <div>
          <div className="mb-4">
            <span className="font-bold text-xl">
              {new Intl.NumberFormat("pl-PL", {
                style: "currency",
                currency: "EUR",
              }).format(budgetUser.budget)}
            </span>
          </div>
          <TabView>
            <TabPanel header="Transaction">
              <div className="grid gap-3 my-4">
                <IconField iconPosition="left">
                  <InputIcon className="pi pi-search" />
                  <InputText
                    value={searchTerm}
                    onChange={(e) => setSearchTerm(e.target.value)}
                    placeholder="Search"
                  />
                </IconField>
                <SelectButton
                  value={category}
                  onChange={(e) => buttonToggleCategory(e.value)}
                  optionLabel="name"
                  options={items}
                  multiple
                />
              </div>
              <div className="grid gap-3 my-4">
                <Calendar
                  value={dateFrom}
                  onChange={(e) => setDateFrom(e.value as Date)}
                  placeholder="Date from"
                  dateFormat="dd/mm/yy"
                />

                <Calendar
                  value={dateTo}
                  onChange={(e) => setDateTo(e.value as Date)}
                  placeholder="Date to"
                  dateFormat="dd/mm/yy"
                />
              </div>
              <div className="grid gap-3 my-4">
                <InputText
                  type="number"
                  placeholder="Amount from"
                  value={amountFrom != null ? amountFrom.toString() : ""}
                  onChange={(e) =>
                    setAmountFrom(
                      e.target.value ? Number(e.target.value) : null,
                    )
                  }
                />
                <InputText
                  type="number"
                  placeholder="Amount to"
                  value={amountTo != null ? amountTo.toString() : ""}
                  onChange={(e) =>
                    setAmountTo(e.target.value ? Number(e.target.value) : null)
                  }
                />
              </div>
              <div className="grid gap-3 my-4">
                <Dropdown
                  value={sortOrderTransaction}
                  options={sortOptionsTransactions}
                  onChange={(e) => setSortOrderTransaction(e.value)}
                  placeholder="Sorting"
                />

                <Button
                  label="Reset Filters"
                  icon="pi pi-refresh"
                  onClick={resetFilters}
                />
                <Button
                  label="Add Transaction"
                  onClick={() => setVisible(true)}
                />
              </div>

              <Dialog
                header="Add Transaction"
                visible={visible}
                onHide={() => setVisible(false)}
              >
                <AddTransaction
                  budgetId={budgetUser.id}
                  onClose={() => setVisible(false)}
                  onAddSuccess={() => {
                    fetchTransaction();
                    fetchBudgetUser();
                  }}
                />
              </Dialog>
              <DataView
                value={transaction}
                listTemplate={listTemplate}
                paginator
                rows={20}
              />
            </TabPanel>
            <TabPanel header="Planned Expense">
              <div>
                <p className="m-0">
                  Sed ut perspiciatis unde omnis iste natus error sit voluptatem
                  accusantium doloremque laudantium...
                </p>
              </div>
            </TabPanel>
          </TabView>
          <Button
            className="mt-4"
            label="DELETE BUDGET"
            type="submit"
            onClick={handleDeleteBudget}
          />
        </div>
      ) : (
        <div className="text-center">
          <Button
            label="ADD BUDGET"
            type="submit"
            onClick={handleSubmitAddBudget}
          />
        </div>
      )}
      <ConfirmationDialog
        visible={confirmVisible}
        header="Confirm Deletion of Transaction"
        message="Are you sure you want to delete this Transaction?"
        onConfirm={handleConfirmDeleteTransaction}
        onCancel={() => setConfirmVisible(false)}
      />
      <ConfirmationDialog
        visible={isDialogVisible}
        header="Confirm Deletion"
        message="Are you sure you want to delete this budget?"
        onConfirm={handleConfirmDeleteBudget}
        onCancel={handleCancelDeleteBudget}
      />
    </div>
  );
};

export default BudgetTransaction;
