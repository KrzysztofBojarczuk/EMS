import React, { SyntheticEvent } from "react";
import { Message } from "primereact/message";
import { InputText } from "primereact/inputtext";
import { Button } from "primereact/button";
import { InputMask } from "primereact/inputmask";
import { InputNumber } from "primereact/inputnumber";
type Props = {
  onEmployeeCreate: (e: SyntheticEvent) => void;
};

const AddEmployee = ({ onEmployeeCreate }: Props) => {
  return (
    <div className="card">
      <form onSubmit={onEmployeeCreate}>
        <div className="flex flex-column align-items-center mb-3 gap-2">
          <InputText
            id="username"
            placeholder="Username"
            className="p-invalid mr-2"
          />

          <InputText
            id="Email"
            placeholder="Email"
            className="p-invalid mr-2"
          />
          <InputMask
            id="ssn"
            placeholder="Phone"
            mask="999-999-999"
          ></InputMask>
          <InputNumber
            inputId="currency-PLN"
            placeholder="Salary"
            mode="currency"
            currency="PLN"
            locale="pl-PL"
          />
        </div>
        <Button label="Submit" />
      </form>
    </div>
  );
};

export default AddEmployee;
