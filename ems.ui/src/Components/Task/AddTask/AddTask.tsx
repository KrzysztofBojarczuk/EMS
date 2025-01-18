import React, { useEffect, useState } from "react";
import { Controller, useForm } from "react-hook-form";
import { UserPostTaskService } from "../../../Services/TaskService.tsx";
import { Button } from "primereact/button";
import { InputText } from "primereact/inputtext";
import { InputTextarea } from "primereact/inputtextarea";
import { AutoComplete } from "primereact/autocomplete";
import { UserGetAddressService } from "../../../Services/AddressService.tsx";
import { AddressGet } from "../../../Models/Address.ts";
type Props = {};

const AddTask = ({ onClose, onAddSuccess }) => {
  const [value, setValue] = useState("");
  const [items, setItems] = useState<AddressGet[] | string[]>([]);
  const [addresses, setAddresses] = useState<AddressGet[]>([]);

  const fetchAddreses = async () => {
    const data = await UserGetAddressService();
    setAddresses(data);
  };

  useEffect(() => {
    fetchAddreses();
  }, []);

  const {
    control,
    handleSubmit,
    formState: { errors },
    reset,
  } = useForm({
    defaultValues: {
      name: "",
      description: "",
    },
  });

  const search = async (event) => {
    const query = event.query;
    const filtered = await UserGetAddressService(query);

    setItems(
      filtered.map((item) => `${item.city}, ${item.street}, ${item.zipCode}`)
    );
  };

  const onSubmit = async (data) => {
    await UserPostTaskService(data);
    onAddSuccess();
    onClose();
    reset();
  };

  return (
    <div className="flex flex-wrap">
      <div className="flex">
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
              name="description"
              control={control}
              rules={{ required: "Name is required" }}
              render={({ field }) => (
                <div className="inline-flex flex-column gap-2">
                  <InputTextarea
                    {...field}
                    placeholder=" description"
                    rows={5}
                    cols={30}
                  />
                  {errors.name && (
                    <small className="p-error">{errors.name.message}</small>
                  )}
                </div>
              )}
            />

            <div className="inline-flex flex-column gap-2">
              <Button label="Submit" type="submit" />
            </div>
          </div>
        </form>
      </div>
      <div className="flex">
        <div className="flex flex-column px-8 py-5 gap-4">
          <AutoComplete
            value={value}
            suggestions={items}
            completeMethod={search}
            onChange={(e) => setValue(e.value)}
            placeholder="Search for an address"
            dropdown
          />
        </div>
      </div>
    </div>
  );
};

export default AddTask;
