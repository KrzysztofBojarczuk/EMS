import React, { JSX, useEffect, useState } from "react";
import { VehicleGet } from "../../../Models/Vehicle";
import {
  DeleteVehicleService,
  GetUserVehiclesService,
} from "../../../Services/VehicleService";
import { Paginator, PaginatorPageChangeEvent } from "primereact/paginator";
import { IconField } from "primereact/iconfield";
import { InputIcon } from "primereact/inputicon";
import { InputText } from "primereact/inputtext";
import { Column } from "primereact/column";
import { DataTable } from "primereact/datatable";
import { SelectButton } from "primereact/selectbutton";
import { VehicleType } from "../../../Enum/VehicleType";
import {
  dateBodyTemplate,
  vehicleTypeBodyTemplate,
  vehicleTypeToText,
} from "../../Utils/VehicleTemplates";
import { formatDate } from "../../Utils/DateUtils";
import { Calendar } from "primereact/calendar";
import { Dropdown } from "primereact/dropdown";
import { Button } from "primereact/button";
import ConfirmationDialog from "../../Confirmation/ConfirmationDialog";
import { Dialog } from "primereact/dialog";
import AddVehicle from "../AddVehicle/AddVehicle";
import UpdateVehicle from "../UpdateVehicle/UpdateVehicle";
import { formatCurrency } from "../../Utils/Currency";

interface VehicleTypeOption {
  name: string;
  value: VehicleType;
}

const ListVehicle = () => {
  const [vehicle, setVehicles] = useState<VehicleGet[]>([]);
  const [searchVehicleTerm, setSearchVehicleTerm] = useState("");

  const [vehicleType, seVehicleType] = useState<string[]>([]);
  const [dateFrom, setDateFrom] = useState<Date | null>(null);
  const [dateTo, setDateTo] = useState<Date | null>(null);
  const [sortOrder, setSortOrder] = useState<string | null>(null);

  const [firstVehicle, setFirstVehicle] = useState(0);
  const [rowsVehicle, setRowsVehicle] = useState(10);
  const [totalVehicles, setTotalVehicles] = useState(0);

  const [deleteVehicleId, setDeleteVehicleId] = useState<string | null>(null);
  const [confirmVehicleVisible, setConfirmVehicleVisible] = useState(false);

  const [visibleVehicle, setVisibleVehicle] = useState<boolean>(false);

  const [selectedVehicle, setSelectedVehicle] = useState<VehicleGet | null>(
    null
  );
  const [updateVisible, setUpdateVisible] = useState(false);

  const resetFilters = () => {
    setDateFrom(null);
    setDateTo(null);
    setSortOrder(null);
    setSearchVehicleTerm("");
    seVehicleType([]);
  };

  const sortOptions = [
    { label: "None", value: null },
    { label: "Mileage ↑ (Lowest first)", value: "mileage_asc" },
    { label: "Mileage ↓ (Highest first)", value: "mileage_desc" },
    { label: "Date Production ↑ (Oldest first)", value: "date_asc" },
    { label: "Date Production ↓ (Newest first)", value: "date_desc" },
    { label: "OC Validity ↑ (Oldest first)", value: "insurance_oc_asc" },
    { label: "OC Validity ↓ (Newest first)", value: "insurance_oc_desc" },
    { label: "Technical Inspection ↑ (Oldest first)", value: "inspection_asc" },
    {
      label: "Technical Inspection ↓ (Newest first)",
      value: "inspection_desc",
    },
    { label: "Insurance Cost ↑ (Lowest first)", value: "insurance_cost_asc" },
    { label: "Insurance Cost ↓ (Highest first)", value: "insurance_cost_desc" },
  ];

  const vehicleTypeOptions: VehicleTypeOption[] = Object.entries(
    vehicleTypeToText as Record<VehicleType, string>
  ).map(([key, value]) => ({
    name: value as string,
    value: Number(key) as VehicleType,
  }));

  const fetchVehicle = async (page: number, size: number) => {
    const data = await GetUserVehiclesService(
      page,
      size,
      searchVehicleTerm,
      vehicleType,
      dateFrom,
      dateTo,
      sortOrder
    );
    setVehicles(data.vehicleGet);
    setTotalVehicles(data.totalItems);
  };

  const goToPageVehicle = (page: number, rows: number) => {
    const newFirst = (page - 1) * rows;
    setFirstVehicle(newFirst);
    fetchVehicle(page, rows);
  };

  useEffect(() => {
    goToPageVehicle(1, rowsVehicle);
  }, [searchVehicleTerm, vehicleType, dateFrom, dateTo, sortOrder]);

  const onPageChangeVehicles = (event: PaginatorPageChangeEvent) => {
    setFirstVehicle(event.first);
    setRowsVehicle(event.rows);
    goToPageVehicle(event.page + 1, event.rows);
  };

  const selectButtonVehicleType = (selectedVehicleType: string[]) => {
    seVehicleType(selectedVehicleType);
  };

  const handleDeleteVehicle = async () => {
    if (deleteVehicleId) {
      await DeleteVehicleService(deleteVehicleId);

      const totalAfterDelete = totalVehicles - 1;
      const maxPage = Math.ceil(totalAfterDelete / rowsVehicle);
      let currentPage = Math.floor(firstVehicle / rowsVehicle) + 1;

      if (currentPage > maxPage) {
        currentPage = maxPage;
      }

      goToPageVehicle(currentPage, rowsVehicle);
    }

    setConfirmVehicleVisible(false);
    setDeleteVehicleId(null);
  };

  const showVehicleDeleteConfirmation = (id: string) => {
    setDeleteVehicleId(id);
    setConfirmVehicleVisible(true);
  };

  const handleAddSuccess = () => {
    const currentPage = Math.floor(firstVehicle / rowsVehicle) + 1;
    goToPageVehicle(currentPage, rowsVehicle);
  };

  const showUpdateDialog = (vehicle: VehicleGet) => {
    setSelectedVehicle(vehicle);
    setUpdateVisible(true);
  };

  const handleUpdateSuccess = () => {
    const currentPage = Math.floor(firstVehicle / rowsVehicle) + 1;
    goToPageVehicle(currentPage, rowsVehicle);
    setUpdateVisible(false);
  };

  return (
    <div className="xl:m-4 lg:m-4 md:m-2">
      <div className="flex justify-content-start xl:flex-row lg:flex-row md:flex-column sm:flex-column gap-3 my-4">
        <IconField iconPosition="left">
          <InputIcon className="pi pi-search" />
          <InputText
            value={searchVehicleTerm}
            onChange={(e) => setSearchVehicleTerm(e.target.value)}
            placeholder="Search"
          />
        </IconField>
        <SelectButton
          value={vehicleType}
          onChange={(e) => selectButtonVehicleType(e.value)}
          optionLabel="name"
          options={vehicleTypeOptions}
          multiple
          className="mr-4"
        />
        <Calendar
          value={dateFrom}
          onChange={(e) => setDateFrom(e.value as Date)}
          placeholder="Date production from"
          showIcon
          dateFormat="dd/mm/yy"
        />
        <Calendar
          value={dateTo}
          onChange={(e) => setDateTo(e.value as Date)}
          placeholder="Date production to"
          showIcon
          dateFormat="dd/mm/yy"
        />
        <Dropdown
          value={sortOrder}
          options={sortOptions}
          onChange={(e) => setSortOrder(e.value)}
          placeholder="Sorting"
        />
        <Button
          label="Reset Filters"
          icon="pi pi-refresh"
          onClick={resetFilters}
        />
        <Button label="Add Vehicle" onClick={() => setVisibleVehicle(true)} />
        <Dialog
          header="Add Vehicle"
          visible={visibleVehicle}
          onHide={() => {
            if (!visibleVehicle) return;
            setVisibleVehicle(false);
          }}
        >
          <AddVehicle
            onClose={() => setVisibleVehicle(false)}
            onAddSuccess={handleAddSuccess}
          />
        </Dialog>
      </div>
      <DataTable value={vehicle} tableStyle={{ minWidth: "50rem" }}>
        <Column field="id" header="Id"></Column>
        <Column field="brand" header="Brand"></Column>
        <Column field="model" header="Model"></Column>
        <Column field="name" header="Name"></Column>
        <Column
          field="registrationNumber"
          header="Registration Number"
        ></Column>
        <Column field="mileage" header="Mileage"></Column>
        <Column
          field="vehicleType"
          header="Vehicle Type"
          body={vehicleTypeBodyTemplate}
        ></Column>
        <Column
          field="dateOfProduction"
          header="Date Of Production"
          body={(rowData) => dateBodyTemplate(rowData, "dateOfProduction")}
        ></Column>
        <Column
          field="insuranceOcValidUntil"
          header="OC Valid Until"
          body={(rowData) => formatDate(rowData.insuranceOcValidUntil)}
        ></Column>
        <Column
          field="insuranceOcCost"
          header="OC Cost"
          body={(rowData) => formatCurrency(rowData.insuranceOcCost)}
        ></Column>
        <Column
          field="technicalInspectionValidUntil"
          header="Technical Inspection"
          body={(rowData) => formatDate(rowData.technicalInspectionValidUntil)}
        ></Column>
        <Column
          field="isAvailable"
          header="Availability"
          body={(rowData) => (rowData.isAvailable ? "Yes" : "No")}
        ></Column>
        <Column
          header="Action"
          body={(rowData) => (
            <>
              <i
                className="pi pi-pencil"
                style={{
                  fontSize: "1.5rem",
                  cursor: "pointer",
                  marginRight: "10px",
                }}
                onClick={() => showUpdateDialog(rowData)}
              ></i>
              <i
                className="pi pi-trash"
                style={{ fontSize: "1.5rem", cursor: "pointer" }}
                onClick={() => showVehicleDeleteConfirmation(rowData.id)}
              ></i>
            </>
          )}
        />
      </DataTable>
      <Paginator
        first={firstVehicle}
        rows={rowsVehicle}
        totalRecords={totalVehicles}
        onPageChange={onPageChangeVehicles}
        rowsPerPageOptions={[5, 10, 20, 30]}
        style={{ border: "none" }}
      />
      <ConfirmationDialog
        visible={confirmVehicleVisible}
        header="Confirm Vehicle Deletion"
        message="Are you sure you want to delete this vehicle?"
        onConfirm={handleDeleteVehicle}
        onCancel={() => setConfirmVehicleVisible(false)}
      />
      <Dialog
        header="Update Vehicle"
        visible={updateVisible}
        onHide={() => setUpdateVisible(false)}
      >
        {selectedVehicle && (
          <UpdateVehicle
            vehicle={selectedVehicle}
            onClose={() => setUpdateVisible(false)}
            onUpdateSuccess={handleUpdateSuccess}
          />
        )}
      </Dialog>
    </div>
  );
};

export default ListVehicle;
