import React from "react";
import { useForm, Controller } from "react-hook-form";
import { Button } from "primereact/button";
import { InputText } from "primereact/inputtext";
import { InputNumber } from "primereact/inputnumber";
import { TransactionPost } from "../../../Models/Transaction";
import { PostTransactionService } from "../../../Services/TransactionService";
import { CategoryType } from "../../../Enum/CategoryType";

type Props = {
  budgetId: string;
  onClose: () => void;
  onAddSuccess: () => void;
};

const AddTransaction: React.FC<Props> = ({
  budgetId,
  onClose,
  onAddSuccess,
}) => {
  const {
    control,
    handleSubmit,
    formState: { errors },
    reset,
  } = useForm({
    defaultValues: {
      name: "",
      creationDate: new Date().toISOString().split("T")[0],
      category: CategoryType.Income,
      amount: 0,
    },
  });

  const onSubmit = async (data: TransactionPost) => {
    const payload = { ...data, creationDate: data.creationDate };
    await PostTransactionService(budgetId, payload);
    onAddSuccess();
    onClose();
    reset();
  };

  return (
    <form onSubmit={handleSubmit(onSubmit)}>
      <div className="flex flex-column px-8 py-5 gap-4">
        <Controller
          name="name"
          control={control}
          rules={{ required: "Name is required" }}
          render={({ field }) => (
            <div className="inline-flex flex-column gap-2">
              <InputText {...field} placeholder="Name" />
              {errors.name && (
                <small className="p-error">{errors.name.message}</small>
              )}
            </div>
          )}
        />

        <Controller
          name="amount"
          control={control}
          rules={{
            required: "Amount is required",
            min: { value: 0, message: "Amount must be greater than 0" },
          }}
          render={({ field }) => (
            <div className="inline-flex flex-column gap-2">
              <InputNumber
                mode="currency"
                currency="EUR"
                locale="de-DE"
                placeholder="Amount"
                onValueChange={(e) => field.onChange(e.value)}
              />
              {errors.amount && (
                <small className="p-error">{errors.amount.message}</small>
              )}
            </div>
          )}
        />
        <Controller
          name="category"
          control={control}
          render={({ field }) => (
            <div className="inline-flex flex-column gap-2">
              <select {...field} className="p-inputtext">
                <option value={CategoryType.Income}>Income</option>
                <option value={CategoryType.Expense}>Expense</option>
              </select>
            </div>
          )}
        />
        <div className="inline-flex flex-column gap-2">
          <Button label="Submit" type="submit" />
        </div>
      </div>
    </form>
  );
};

export default AddTransaction;
