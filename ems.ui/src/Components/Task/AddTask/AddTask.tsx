import React, { useEffect, useState } from "react";
import { Controller, useForm } from "react-hook-form";
import { Button } from "primereact/button";
import { InputText } from "primereact/inputtext";
import { InputTextarea } from "primereact/inputtextarea";
import { AutoComplete } from "primereact/autocomplete";
import { Calendar } from "primereact/calendar";
import { MultiSelect } from "primereact/multiselect";
import { AddressGet } from "../../../Models/Address";
import { EmployeeListGet } from "../../../Models/EmployeeList";
import { GetUserListForTaskEmployeesService } from "../../../Services/EmployeeService";
import { GetUserAddressesForTaskService } from "../../../Services/AddressService";
import { PostTaskService } from "../../../Services/TaskService";
import { GetUserVehicleForTaskService } from "../../../Services/VehicleService";
import { VehicleGet } from "../../../Models/Vehicle";

interface Props {
  onClose: () => void;
  onAddSuccess: () => void;
}

const AddTask = ({ onClose, onAddSuccess }: Props) => {
  const [value, setValue] = useState("");
  const [items, setItems] = useState<string[]>([]);
  const [addresses, setAddresses] = useState<AddressGet[]>([]);
  const [selectedListEmployees, setSelectedListEmployees] = useState<string[]>(
    [],
  );
  const [employeesList, setEmployeesList] = useState<EmployeeListGet[]>([]);
  const [searchTermList, setSearchTermList] = useState("");

  const [vehicles, setVehicles] = useState<VehicleGet[]>([]);
  const [selectedVehicles, setSelectedVehicles] = useState<string[]>([]);
  const [vehicleSearchTerm, setVehicleSearchTerm] = useState("");

  // const onSelectedListEmployee = (e: CheckboxChangeEvent) => {
  //   let _selectedListEmployees = [...selectedListEmployees];

  //   if (e.checked) {
  //     _selectedListEmployees.push(e.value.id);
  //   } else {
  //     _selectedListEmployees = _selectedListEmployees.filter(
  //       (id) => id !== e.value.id
  //     );
  //   }

  //   setSelectedListEmployees(_selectedListEmployees);
  // };

  const fetchEmployeesList = async () => {
    const data = await GetUserListForTaskEmployeesService(searchTermList);
    setEmployeesList(data);
  };

  const fetchVehicles = async () => {
    const data = await GetUserVehicleForTaskService(vehicleSearchTerm);
    setVehicles(data);
  };

  useEffect(() => {
    fetchVehicles();
  }, [vehicleSearchTerm]);

  useEffect(() => {
    fetchEmployeesList();
  }, [searchTermList]);

  const fetchAddresses = async () => {
    const data = await GetUserAddressesForTaskService();
    setAddresses(data);
  };

  useEffect(() => {
    fetchAddresses();
  }, []);

  const {
    control,
    handleSubmit,
    formState: { errors },
    reset,
    setValue: setFormValue,
  } = useForm({
    defaultValues: {
      name: "",
      description: "",
      employeeListIds: [],
      vehicleIds: [],
      startDate: null,
      endDate: null,
      address: {
        id: "",
        city: "",
        street: "",
        number: "",
        zipCode: "",
      },
    },
  });

  const searchAddress = async (event: any) => {
    const query = event.query;
    const filtered = addresses.filter((item) =>
      `${item.city}, ${item.street}, ${item.zipCode}`
        .toLowerCase()
        .includes(query.toLowerCase()),
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
    } else {
      setFormValue("address.id", "");
      setFormValue("address.city", "");
      setFormValue("address.street", "");
      setFormValue("address.number", "");
      setFormValue("address.zipCode", "");
    }
    setValue(event.value);
  };

  const onSubmit = async (data: any) => {
    await PostTaskService({
      name: data.name,
      description: data.description,
      employeeListIds: selectedListEmployees,
      vehicleIds: selectedVehicles,
      startDate: data.startDate.toISOString(),
      endDate: data.endDate.toISOString(),
      addressId: data.address.id || undefined,
    });
    onAddSuccess();
    onClose();
    reset();
  };

  return (
    <form onSubmit={handleSubmit(onSubmit)}>
      <div className="formgrid grid">
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
                    id="listemployeeIds"
                    value={selectedListEmployees}
                    options={employeesList}
                    onChange={(e) => {
                      setSelectedListEmployees(e.value);
                      field.onChange(e.value);
                    }}
                    onFilter={(e) => setSearchTermList(e.filter)}
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
                  value={selectedVehicles}
                  options={vehicles}
                  onChange={(e) => {
                    setSelectedVehicles(e.value);
                    field.onChange(e.value);
                  }}
                  onFilter={(e) => setVehicleSearchTerm(e.filter)}
                  optionLabel="name"
                  optionValue="id"
                  filter
                  placeholder="Select Vehicles"
                  itemTemplate={(option) => (
                    <div>
                      <strong>
                        {option.brand} {option.model} {option.name}
                      </strong>
                    </div>
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
              <Button label="Submit" type="submit" />
            </div>
          </div>
        </div>
      </div>
    </form>
  );
};

export default AddTask;
