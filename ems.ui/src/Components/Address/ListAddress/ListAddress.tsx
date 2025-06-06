import React, { useState, useEffect, JSX } from "react";
import { DataTable } from "primereact/datatable";
import { Column } from "primereact/column";
import { Dialog } from "primereact/dialog";
import { Button } from "primereact/button";
import { InputText } from "primereact/inputtext";
import { AddressGet } from "../../../Models/Address";
import {
  UserDeleteAddressService,
  UserGetAddressService,
} from "../../../Services/AddressService.tsx";
import ConfirmationDialog from "../../Confirmation/ConfirmationDialog.tsx";
import AddAddress from "../AddAddress/AddAddress.tsx";
import UpdateEmployee from "../../Employee/UpdateEmployee/UpdateEmployee.tsx";
import UpdateAddress from "../UpdateAddress/UpdateAddress.tsx";
import { IconField } from "primereact/iconfield";
import { InputIcon } from "primereact/inputicon";
import { Paginator } from "primereact/paginator";

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
  const [totalAddress, setTotalAdresses] = useState(0);

  const fetchAddreses = async (page: number, size: number) => {
    const data = await UserGetAddressService(page, size, searchTerm);
    setAddresses(data.addressGet);
    setTotalAdresses(data.totalItems);
  };

  useEffect(() => {
    fetchAddreses(1, rowsAddress);
  }, [searchTerm]);

  const onPageChangeAddreses = (event: any) => {
    setFirstAddress(event.first);
    setRowsAddress(event.rows);
    fetchAddreses(event.page + 1, event.rows);
  };

  const showDeleteConfirmation = (id: string) => {
    setDeleteId(id);
    setConfirmVisible(true);
  };

  const handleConfirmDelete = async () => {
    if (deleteId) {
      await UserDeleteAddressService(deleteId);
      fetchAddreses(1, rowsAddress);
    }
    setConfirmVisible(false);
    setDeleteId(null);
  };

  const showUpdateDialog = (address: AddressGet) => {
    setSelectedAddress(address);
    setUpdateVisible(true);
  };

  return (
    <div className="xl:m-4 lg:m-4 md:m-2">
      <div className="flex justify-content-start xl:flex-row lg:flex-row md:flex-column sm:flex-column gap-3 my-4">
        <IconField iconPosition="left">
          <InputIcon className="pi pi-search"> </InputIcon>
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
          onHide={() => {
            if (!visible) return;
            setVisible(false);
          }}
        >
          <AddAddress
            onClose={() => setVisible(false)}
            onAddSuccess={() => fetchAddreses(1, rowsAddress)}
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
        onPageChange={onPageChangeAddreses}
        rowsPerPageOptions={[5, 10, 20, 30]}
        style={{ border: "none" }}
      />

      <ConfirmationDialog
        visible={confirmVisible}
        header="Confirm Deletion of Employee"
        message="Are you sure you want to delete this employee?"
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
            onUpdateSuccess={() => fetchAddreses(1, rowsAddress)}
          />
        )}
      </Dialog>
    </div>
  );
};

export default ListAddress;
