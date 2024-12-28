import React, { SyntheticEvent } from "react";
import { Message } from "primereact/message";
import { InputText } from "primereact/inputtext";
import { Button } from "primereact/button";

type Props = {
  onEmployeeCreate: (e: SyntheticEvent) => void;
};

const AddEmployee = ({ onEmployeeCreate }: Props) => {
  return (
    <div className="card">
      <form onSubmit={onEmployeeCreate}>
        <div className="flex flex-column align-items-center mb-3 gap-2">
          <label htmlFor="username" className="p-sr-only">
            Username
          </label>
          <InputText
            id="username"
            placeholder="Username"
            className="p-invalid mr-2"
          />
          <Message severity="error" text="Username is required" />
        </div>
        <Button label="Submit" />
      </form>
    </div>
  );
};

export default AddEmployee;
