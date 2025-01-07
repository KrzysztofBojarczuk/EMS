import React, { useEffect, useState } from "react";
import { TabView, TabPanel } from "primereact/tabview";
import Transaction from "./Transaction.tsx";
import { BudgetGet, BudgetPost } from "../../Models/Budget.ts";
import {
  UserGetBudgetService,
  UserDeleteBudgetService,
  UserPostBudgetService,
} from "../../Services/BudgetService.tsx";
import { Button } from "primereact/button";
import ConfirmationDialog from "../Confirmation/ConfirmationDialog.tsx";
import { InputNumber } from "primereact/inputnumber";
type Props = {};

function Budget({}: Props) {
  const [budgetUser, setBudgetUser] = useState<BudgetGet | null>(null);
  const [isDialogVisible, setIsDialogVisible] = useState(false);

  const fetchBudgetUser = async () => {
    const data = await UserGetBudgetService();
    setBudgetUser(data);
  };

  useEffect(() => {
    fetchBudgetUser();
  }, []);

  const handleSubmitAddBudget = async () => {
    const newBudget: BudgetPost = {
      budget: 0,
    };
    await UserPostBudgetService(newBudget);
    fetchBudgetUser();
  };

  const handleConfirmDeleteBudget = async () => {
    if (budgetUser?.id) {
      await UserDeleteBudgetService(budgetUser.id);
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

  return (
    <div className="m-4">
      {budgetUser ? (
        <div>
          <div className="mb-4">
            <span className="font-bold text-xl">
              {new Intl.NumberFormat("pl-PL", {
                style: "currency",
                currency: "PLN",
              }).format(budgetUser.budget)}
            </span>
          </div>
          <TabView>
            <TabPanel header="Transaction">
              <Transaction />
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
        visible={isDialogVisible}
        header="Confirm Deletion"
        message="Are you sure you want to delete this budget?"
        onConfirm={handleConfirmDeleteBudget}
        onCancel={handleCancelDeleteBudget}
      />
    </div>
  );
}
export default Budget;
