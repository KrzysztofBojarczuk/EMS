import React, { useEffect, useState } from "react";
import { Controller, useForm } from "react-hook-form";
import { Button } from "primereact/button";
import { InputText } from "primereact/inputtext";
import { InputTextarea } from "primereact/inputtextarea";
import { AutoComplete } from "primereact/autocomplete";
import { Calendar } from "primereact/calendar";
import { MultiSelect } from "primereact/multiselect";
import { TaskGet, TaskPost } from "../../../Models/Task";
import { EmployeeListGet } from "../../../Models/EmployeeList";
import { AddressGet } from "../../../Models/Address";
import { VehicleGet } from "../../../Models/Vehicle";
import { GetUserListForTaskUpdateEmployeesService } from "../../../Services/EmployeeService";
import { GetUserVehicleForTaskUpdateService } from "../../../Services/VehicleService";
import { GetUserAddressesForTaskService } from "../../../Services/AddressService";
import { UpdateTaskService } from "../../../Services/TaskService";

interface Props {
  task: TaskGet;
  onClose: () => void;
  onUpdateSuccess: () => void;
}

const UpdateTask = ({ task, onClose, onUpdateSuccess }: Props) => {
  const [value, setValue] = useState("");
  const [items, setItems] = useState<string[]>([]);
  const [addresses, setAddresses] = useState<AddressGet[]>([]);
  const [employeesList, setEmployeesList] = useState<EmployeeListGet[]>([]);
  const [searchEmployeeListTerm, setSearchEmployeeListTerm] = useState("");
  const [vehicles, setVehicles] = useState<VehicleGet[]>([]);
  const [searchVehicleTerm, setSearchVehicleTerm] = useState("");

  const {
    control,
    handleSubmit,
    formState: { errors },
    setValue: setFormValue,
  } = useForm({
    defaultValues: {
      name: task.name,
      description: task.description,
      employeeListIds: task.employeeLists.map((e) => e.id),
      vehicleIds: task.vehicles.map((v) => v.id),
      startDate: new Date(task.startDate),
      endDate: new Date(task.endDate),
      address: {
        id: task.address?.id || "",
        city: task.address?.city || "",
        street: task.address?.street || "",
        number: task.address?.number || "",
        zipCode: task.address?.zipCode || "",
      },
    },
  });

  const fetchEmployeesList = async () => {
    const data = await GetUserListForTaskUpdateEmployeesService(
      task.id,
      searchEmployeeListTerm,
    );
    setEmployeesList(data);
  };

  const fetchVehicles = async () => {
    const data = await GetUserVehicleForTaskUpdateService(
      task.id,
      searchVehicleTerm,
    );
    setVehicles(data);
  };

  const fetchAddresses = async () => {
    const data = await GetUserAddressesForTaskService();
    setAddresses(data);
  };

  useEffect(() => {
    fetchEmployeesList();
  }, [searchEmployeeListTerm]);

  useEffect(() => {
    fetchVehicles();
  }, [searchVehicleTerm]);

  useEffect(() => {
    fetchAddresses();
  }, []);

  const searchAddress = (event: any) => {
    const query = event.query.toLowerCase();

    const filtered = addresses.filter((item) =>
      `${item.city}, ${item.street}, ${item.zipCode}`
        .toLowerCase()
        .includes(query),
    );

    setItems(
      filtered.map((item) => `${item.city}, ${item.street}, ${item.zipCode}`),
    );
  };

  const handleAddressChange = (event: any) => {
    const selected = addresses.find(
      (item) => `${item.city}, ${item.street}, ${item.zipCode}` === event.value,
    );

    if (selected) {
      setFormValue("address.id", selected.id);
      setFormValue("address.city", selected.city);
      setFormValue("address.street", selected.street);
      setFormValue("address.number", selected.number);
      setFormValue("address.zipCode", selected.zipCode);
    }

    setValue(event.value);
  };

  const onSubmit = async (data: any) => {
    const payload: TaskPost = {
      name: data.name,
      description: data.description,
      employeeListIds: data.employeeListIds,
      vehicleIds: data.vehicleIds,
      startDate: data.startDate,
      endDate: data.endDate,
      addressId: data.address.id,
    };

    await UpdateTaskService(task.id, payload);
    onUpdateSuccess();
    onClose();
  };

  return (
    <form onSubmit={handleSubmit(onSubmit)}>
      <div className="form grid">
        <div className="col-6">
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
              rules={{ required: "Description is required" }}
              render={({ field }) => (
                <div className="inline-flex flex-column gap-2">
                  <InputTextarea
                    {...field}
                    rows={5}
                    cols={30}
                    placeholder="Description"
                  />
                  {errors.description && (
                    <small className="p-error">
                      {errors.description.message}
                    </small>
                  )}
                </div>
              )}
            />
            <Controller
              name="employeeListIds"
              control={control}
              rules={{ required: "List Employees is required" }}
              render={({ field }) => (
                <div className="inline-flex flex-column gap-2">
                  <MultiSelect
                    {...field}
                    options={employeesList}
                    onFilter={(e) => setSearchEmployeeListTerm(e.filter)}
                    optionLabel="name"
                    optionValue="id"
                    filter
                    placeholder="Select List Employees"
                  />
                  {errors.employeeListIds && (
                    <small className="p-error">
                      {errors.employeeListIds.message}
                    </small>
                  )}
                </div>
              )}
            />
            <Controller
              name="vehicleIds"
              control={control}
              render={({ field }) => (
                <MultiSelect
                  {...field}
                  options={vehicles}
                  onFilter={(e) => setSearchVehicleTerm(e.filter)}
                  optionLabel="name"
                  optionValue="id"
                  filter
                  placeholder="Select Vehicles"
                  itemTemplate={(option) => (
                    <strong>
                      {option.brand} {option.model} {option.name}
                    </strong>
                  )}
                />
              )}
            />
            <Controller
              name="startDate"
              control={control}
              rules={{ required: "Start date is required" }}
              render={({ field }) => (
                <div className="inline-flex flex-column gap-2">
                  <Calendar
                    value={field.value ? new Date(field.value) : null}
                    onChange={(e) => field.onChange(e.value)}
                    dateFormat="dd/mm/yy"
                    showIcon
                    placeholder="Select Start Date"
                  />
                  {errors.startDate && (
                    <small className="p-error">
                      {errors.startDate.message}
                    </small>
                  )}
                </div>
              )}
            />
            <Controller
              name="endDate"
              control={control}
              rules={{ required: "End date is required" }}
              render={({ field }) => (
                <div className="inline-flex flex-column gap-2">
                  <Calendar
                    value={field.value ? new Date(field.value) : null}
                    onChange={(e) => field.onChange(e.value)}
                    dateFormat="dd/mm/yy"
                    showIcon
                    placeholder="Select End Date"
                  />
                  {errors.endDate && (
                    <small className="p-error">{errors.endDate.message}</small>
                  )}
                </div>
              )}
            />
          </div>
        </div>
        <div className="col-6">
          <div className="flex flex-column px-8 py-5 gap-4">
            <AutoComplete
              value={value}
              suggestions={items}
              completeMethod={searchAddress}
              onChange={handleAddressChange}
              dropdown
              placeholder="Search for an address"
            />
            <Controller
              name="address.city"
              control={control}
              render={({ field }) => (
                <div className="inline-flex flex-column gap-2">
                  <label htmlFor="city">
                    <strong>City:</strong>
                  </label>
                  <InputText {...field} disabled />
                </div>
              )}
            />
            <Controller
              name="address.street"
              control={control}
              render={({ field }) => (
                <div className="inline-flex flex-column gap-2">
                  <label htmlFor="street">
                    <strong>Street:</strong>
                  </label>
                  <InputText {...field} disabled />
                </div>
              )}
            />
            <Controller
              name="address.number"
              control={control}
              render={({ field }) => (
                <div className="inline-flex flex-column gap-2">
                  <label htmlFor="number">
                    <strong>Number:</strong>
                  </label>
                  <InputText {...field} disabled />
                </div>
              )}
            />
            <Controller
              name="address.zipCode"
              control={control}
              render={({ field }) => (
                <div className="inline-flex flex-column gap-2">
                  <label htmlFor="zipCode">
                    <strong>Zip Code:</strong>
                  </label>
                  <InputText {...field} disabled />
                </div>
              )}
            />
            <div className="inline-flex flex-column gap-2 mt-8">
              <Button label="Update" type="submit" />
            </div>
          </div>
        </div>
      </div>
    </form>
  );
};

export default UpdateTask;
