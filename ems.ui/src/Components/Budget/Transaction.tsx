import React, { useEffect, useState } from "react";
import { DataView, DataViewLayoutOptions } from "primereact/dataview";
import { TransactionGet } from "../../Models/Transaction";
import { UserGetTransactionService } from "../../Services/TransactionService.tsx";
import { SelectButton } from "primereact/selectbutton";
import { InputText } from "primereact/inputtext";
import { Tag } from "primereact/tag";
import { Button } from "primereact/button";
import { classNames } from "primereact/utils";
import { CategoryType } from "../../Enum/CategoryType.ts";
import ConfirmationDialog from "../Confirmation/ConfirmationDialog.tsx";

type Props = {
  transactionId: string;
};

const Transaction = ({ transactionId }: Props) => {
  const [transaction, setTransaction] = useState<TransactionGet[]>([]);
  const [searchTerm, setSearchTerm] = useState<string>("");
  const [category, setCategory] = useState<string[]>([]);
  const [confirmVisible, setConfirmVisible] = useState(false);
  const [deleteId, setDeleteId] = useState<string | null>(null);

  const [value, setValue] = useState(null);
  const items = [
    { name: "Income", value: 1 },
    { name: "Expense", value: 2 },
  ];

  const showDeleteConfirmation = (id: string) => {
    setDeleteId(id);
    setConfirmVisible(true);
  };

  const handleConfirmDelete = async () => {
    if (deleteId) {
      // await UserDeleteEmployeesService(deleteId);
      // fetchEmployees();
    }
    setConfirmVisible(false);
    setDeleteId(null);
  };

  const fetchTransaction = async () => {
    const data = await UserGetTransactionService(
      transactionId,
      searchTerm,
      category
    );
    setTransaction(data);
  };

  useEffect(() => {
    fetchTransaction();
  }, [transactionId, searchTerm, category]);

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

  const itemTemplate = (transaction, index) => {
    return (
      <div className="col-12" key={transaction.id}>
        <div
          className={classNames(
            "flex flex-column xl:flex-row xl:align-items-start p-4 gap-4",
            { "border-top-1 surface-border": index !== 0 }
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
                  value={transaction.inventoryStatus}
                  severity={getSeverity(transaction)}
                >
                  {categoryToText[transaction.category]}
                </Tag>
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
            </div>
          </div>
        </div>
        <ConfirmationDialog
          visible={confirmVisible}
          header="Confirm Deletion of Transaction"
          message="Are you sure you want to delete this Transaction?"
          onConfirm={handleConfirmDelete}
          onCancel={() => setConfirmVisible(false)}
        />
      </div>
    );
  };

  const listTemplate = (items: TransactionGet[]) => {
    if (!items || items.length === 0) return null;

    let list = items.map((transaction, index) => {
      return itemTemplate(transaction, index);
    });

    return <div className="grid grid-nogutter">{list}</div>;
  };

  return (
    <div>
      <div className="flex justify-content-center xl:flex-row lg:flex-row md:flex-column sm:flex-column gap-3">
        <InputText
          value={searchTerm}
          onChange={(e) => setSearchTerm(e.target.value)}
          className="mr-4"
          placeholder="Search"
        />
        <SelectButton
          value={value}
          onChange={(e) => setValue(e.value)}
          optionLabel="name"
          options={items}
          multiple
          className="mr-4"
        />
        <Button label="Add Transaction" />
      </div>
      <DataView
        value={transaction}
        listTemplate={listTemplate}
        paginator
        rows={5}
      />
    </div>
  );
};

export default Transaction;
