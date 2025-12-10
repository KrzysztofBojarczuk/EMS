import React, { useState, useEffect } from "react";
import { DataTable } from "primereact/datatable";
import { Column } from "primereact/column";
import { Dialog } from "primereact/dialog";
import { Button } from "primereact/button";
import { InputText } from "primereact/inputtext";
import { AddressGet } from "../../../Models/Address";
import { IconField } from "primereact/iconfield";
import { InputIcon } from "primereact/inputicon";
import { Paginator, PaginatorPageChangeEvent } from "primereact/paginator";
import {
  DeleteAddressService,
  GetUserAddressService,
} from "../../../Services/AddressService";
import AddAddress from "../AddAddress/AddAddress";
import ConfirmationDialog from "../../Confirmation/ConfirmationDialog";
import UpdateAddress from "../UpdateAddress/UpdateAddress";

type Props = {};

const ListAddress = (props: Props) => {
  const [addresses, setAddresses] = useState<AddressGet[]>([]);
  const [searchTerm, setSearchTerm] = useState("");
  const [confirmVisible, setConfirmVisible] = useState<boolean>(false);
  const [deleteId, setDeleteId] = useState<string | null>(null);
  const [visible, setVisible] = useState<boolean>(false);
  const [selectedAddress, setSelectedAddress] = useState<AddressGet | null>(
    null
  );
  const [updateVisible, setUpdateVisible] = useState(false);

  const [firstAddress, setFirstAddress] = useState(0);
  const [rowsAddress, setRowsAddress] = useState(10);
  const [totalAddress, setTotalAddresses] = useState(0);

  const fetchAddresses = async (page: number, size: number) => {
    const data = await GetUserAddressService(page, size, searchTerm);
    setAddresses(data.addressGet);
    setTotalAddresses(data.totalItems);
  };

  const goToPage = (page: number, rows: number) => {
    const newFirst = (page - 1) * rows;
    setFirstAddress(newFirst);
    fetchAddresses(page, rows);
  };

  useEffect(() => {
    goToPage(1, rowsAddress);
  }, [searchTerm, rowsAddress]);

  const onPageChangeAddresses = (event: PaginatorPageChangeEvent) => {
    setRowsAddress(event.rows);
    goToPage(event.page + 1, event.rows);
  };

  const showDeleteConfirmation = (id: string) => {
    setDeleteId(id);
    setConfirmVisible(true);
  };

  const handleConfirmDelete = async () => {
    if (deleteId) {
      await DeleteAddressService(deleteId);

      const totalAfterDelete = totalAddress - 1;
      const maxPage = Math.ceil(totalAfterDelete / rowsAddress);
      let currentPage = Math.floor(firstAddress / rowsAddress) + 1;

      if (currentPage > maxPage) {
        currentPage = maxPage;
      }

      goToPage(currentPage, rowsAddress);
    }
    setDeleteId(null);
    setConfirmVisible(false);
  };

  const showUpdateDialog = (address: AddressGet) => {
    setSelectedAddress(address);
    setUpdateVisible(true);
  };

  const handleAddSuccess = () => {
    const currentPage = Math.floor(firstAddress / rowsAddress) + 1;
    goToPage(currentPage, rowsAddress);
  };

  const handleUpdateSuccess = () => {
    const currentPage = Math.floor(firstAddress / rowsAddress) + 1;
    goToPage(currentPage, rowsAddress);
    setUpdateVisible(false);
  };

  return (
    <div className="xl:m-4 lg:m-4 md:m-2">
      <div className="flex justify-content-start xl:flex-row lg:flex-row md:flex-column sm:flex-column gap-3 my-4">
        <IconField iconPosition="left">
          <InputIcon className="pi pi-search" />
          <InputText
            value={searchTerm}
            onChange={(e) => setSearchTerm(e.target.value)}
            placeholder="Search"
          />
        </IconField>
        <Button label="Add Address" onClick={() => setVisible(true)} />

        <Dialog
          header="Add Address"
          visible={visible}
          onHide={() => setVisible(false)}
        >
          <AddAddress
            onClose={() => setVisible(false)}
            onAddSuccess={handleAddSuccess}
          />
        </Dialog>
      </div>

      <DataTable value={addresses} tableStyle={{ minWidth: "50rem" }}>
        <Column field="id" header="Id"></Column>
        <Column field="city" header="City"></Column>
        <Column field="street" header="Street"></Column>
        <Column field="number" header="Number"></Column>
        <Column field="zipCode" header="ZIP Code"></Column>
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
                onClick={() => showDeleteConfirmation(rowData.id)}
              ></i>
            </>
          )}
        ></Column>
      </DataTable>

      <Paginator
        first={firstAddress}
        rows={rowsAddress}
        totalRecords={totalAddress}
        onPageChange={onPageChangeAddresses}
        rowsPerPageOptions={[5, 10, 20, 30]}
        style={{ border: "none" }}
      />

      <ConfirmationDialog
        visible={confirmVisible}
        header="Confirm Deletion of Address"
        message="Are you sure you want to delete this address?"
        onConfirm={handleConfirmDelete}
        onCancel={() => setConfirmVisible(false)}
      />

      <Dialog
        header="Update Address"
        visible={updateVisible}
        onHide={() => setUpdateVisible(false)}
      >
        {selectedAddress && (
          <UpdateAddress
            address={selectedAddress}
            onClose={() => setUpdateVisible(false)}
            onUpdateSuccess={handleUpdateSuccess}
          />
        )}
      </Dialog>
    </div>
  );
};

export default ListAddress;
