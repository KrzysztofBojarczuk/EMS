import React, { useEffect, useRef, useState } from "react";
import { Panel } from "primereact/panel";
import { InputText } from "primereact/inputtext";
import {
  DataTable,
  DataTableExpandedRows,
  DataTableValueArray,
} from "primereact/datatable";
import { Paginator } from "primereact/paginator";
import { Column } from "primereact/column";
import { IconField } from "primereact/iconfield";
import { InputIcon } from "primereact/inputicon";
import { Button } from "primereact/button";
import { Dialog } from "primereact/dialog";

import { Card } from "primereact/card";
import { PaginatorPageChangeEvent } from "primereact/paginator";
import { LocalGet } from "../../../Models/Local";
import { ReservationGet } from "../../../Models/Reservation";
import {
  DeleteLocalService,
  DeleteReservationService,
  UserGetLocalService,
  UserGetReservationService,
} from "../../../Services/LocalReservationService";
import { formatDateTime } from "../../Utils/DateUtils";
import AddLocal from "../AddLocal/AddLocal";
import AddReservation from "../AddReservation/AddReservation";
import ConfirmationDialog from "../../Confirmation/ConfirmationDialog";
import UpdateLocal from "../UpdateLocal/UpdateLocal";

type Props = {};

const LocalReservation = (props: Props) => {
  const [local, setLocals] = useState<LocalGet[]>([]);
  const [reservations, setReservations] = useState<ReservationGet[]>([]);

  const [searchLocalTerm, setSearchLocalTerm] = useState("");
  const [searchReservationTerm, setSearchReservationTerm] = useState("");

  const [deleteLocalId, setDeleteLocalId] = useState<string | null>(null);
  const [deleteReservationId, setDeleteReservationId] = useState<string | null>(
    null
  );

  const [selectedLocal, setSelectedLocal] = useState<LocalGet | null>(null);
  const [updateVisible, setUpdateVisible] = useState(false);

  const [firstLocal, setFirstLocal] = useState(0);
  const [rowsLocal, setRowsLocal] = useState(10);
  const [totalLocals, setTotalLocals] = useState(0);

  const [firstReservation, setFirstReservation] = useState(0);
  const [rowsReservation, setRowsReservation] = useState(10);
  const [totalReservation, setTotalReservations] = useState(0);

  const [selectedLocalId, setSelectedLocalId] = useState<string | null>(null);

  const [visibleReservation, setVisibleReservation] = useState<boolean>(false);
  const [visibleLocal, setVisibleLocal] = useState<boolean>(false);

  const [confirmReservationVisible, setConfirmReservationVisible] =
    useState(false);
  const [confirmLocalVisible, setConfirmLocalVisible] = useState(false);

  const [expandedRows, setExpandedRows] = useState<
    DataTableExpandedRows | DataTableValueArray | undefined
  >(undefined);

  const localPanelRef = useRef<Panel>(null);
  const reservationPanelRef = useRef<Panel>(null);

  const allowExpansion = (rowData: LocalGet) => {
    return rowData.id!.length > 0;
  };

  const fetchLocal = async (page: number, size: number) => {
    const data = await UserGetLocalService(page, size, searchLocalTerm);
    setLocals(data.localGet);
    setTotalLocals(data.totalItems);
  };

  const goToPageLocal = (page: number, rows: number) => {
    const newFirst = (page - 1) * rows;
    setFirstLocal(newFirst);
    fetchLocal(page, rows);
  };

  const goToPageReservation = (page: number, rows: number) => {
    const newFirst = (page - 1) * rows;
    setFirstReservation(newFirst);
    fetchReservations(page, rows);
  };

  useEffect(() => {
    goToPageLocal(1, rowsLocal);
  }, [searchLocalTerm]);

  const fetchReservations = async (page: number, size: number) => {
    const data = await UserGetReservationService(
      page,
      size,
      searchReservationTerm
    );
    setReservations(data.reservationGet);
    setTotalReservations(data.totalItems);
  };

  useEffect(() => {
    goToPageReservation(1, rowsReservation);
  }, [searchReservationTerm]);

  const onPageChangeLocals = (event: PaginatorPageChangeEvent) => {
    setFirstLocal(event.first);
    setRowsLocal(event.rows);
    goToPageLocal(event.page + 1, event.rows);
  };

  const handleAddSuccess = () => {
    const currentPage = Math.floor(firstLocal / rowsLocal) + 1;
    goToPageLocal(currentPage, rowsLocal);
  };

  const handleUpdateSuccess = () => {
    const currentPage = Math.floor(firstLocal / rowsLocal) + 1;
    goToPageLocal(currentPage, rowsLocal);
    setUpdateVisible(false);
  };

  const handleAddReservationSuccess = () => {
    const currentLocalPage = Math.floor(firstLocal / rowsLocal) + 1;
    goToPageLocal(currentLocalPage, rowsLocal);

    const currentReservationPage =
      Math.floor(firstReservation / rowsReservation) + 1;
    goToPageReservation(currentReservationPage, rowsReservation);
  };

  const onPageChangeReservations = (event: PaginatorPageChangeEvent) => {
    setFirstReservation(event.first);
    setRowsReservation(event.rows);
    fetchReservations(event.page + 1, event.rows);
  };

  const makeReservation = (localId: string) => {
    setSelectedLocalId(localId);
    setVisibleReservation(true);
  };

  const showReservationDeleteConfirmation = (id: string) => {
    setDeleteReservationId(id);
    setConfirmReservationVisible(true);
  };

  const handleDeleteReservation = async () => {
    if (deleteReservationId) {
      await DeleteReservationService(deleteReservationId);

      const totalAfterDelete = totalReservation - 1;
      const maxPage = Math.ceil(totalAfterDelete / rowsReservation);
      let currentPage = Math.floor(firstReservation / rowsReservation) + 1;

      if (currentPage > maxPage) {
        currentPage = maxPage;
      }

      goToPageReservation(currentPage, rowsReservation);
    }

    setConfirmReservationVisible(false);
    setDeleteReservationId(null);
  };

  const showLocalDeleteConfirmation = (id: string) => {
    setDeleteLocalId(id);
    setConfirmLocalVisible(true);
  };

  const handleDeleteLocal = async () => {
    if (deleteLocalId) {
      await DeleteLocalService(deleteLocalId);

      const totalAfterDelete = totalLocals - 1;
      const maxPage = Math.ceil(totalAfterDelete / rowsLocal);
      let currentPage = Math.floor(firstLocal / rowsLocal) + 1;

      if (currentPage > maxPage) {
        currentPage = maxPage;
      }

      goToPageLocal(currentPage, rowsLocal);

      const currentReservationPage =
        Math.floor(firstReservation / rowsReservation) + 1;
      goToPageReservation(currentReservationPage, rowsReservation);
    }

    setConfirmLocalVisible(false);
    setDeleteLocalId(null);
  };

  const showUpdateDialog = (local: LocalGet) => {
    setSelectedLocal(local);
    setUpdateVisible(true);
  };

  const rowExpansionTemplate = (data: LocalGet) => {
    return (
      <div>
        {data.reservations && data.reservations.length > 0 ? (
          <div className="flex flex-row flex-wrap">
            {data.reservations.map((reservation, index) => (
              <div key={index}>
                <Card
                  className="flex flex-column m-2"
                  style={{ border: "2px solid #81c784" }}
                >
                  <p className="text-green-400">
                    Reservation ID: {reservation.id}
                  </p>
                  <p>Check-in: {formatDateTime(reservation.checkInDate)}</p>
                  <span>
                    Check-out: {formatDateTime(reservation.checkOutDate)}
                  </span>
                </Card>
              </div>
            ))}
          </div>
        ) : (
          <div>No data</div>
        )}
      </div>
    );
  };

  return (
    <div className="xl:m-4 lg:m-4 md:m-2">
      <Panel ref={localPanelRef} header="Loclas" toggleable collapsed>
        <div className="flex justify-content-start xl:flex-row lg:flex-row md:flex-column sm:flex-column gap-3 my-4">
          <IconField iconPosition="left">
            <InputIcon className="pi pi-search"> </InputIcon>
            <InputText
              value={searchLocalTerm}
              onChange={(e) => setSearchLocalTerm(e.target.value)}
              placeholder="Search Locals"
            />
          </IconField>
          <Button label="Add Local" onClick={() => setVisibleLocal(true)} />
          <Dialog
            header="Add Local"
            visible={visibleLocal}
            onHide={() => {
              if (!visibleLocal) return;
              setVisibleLocal(false);
            }}
          >
            <AddLocal
              onClose={() => setVisibleLocal(false)}
              onAddSuccess={handleAddSuccess}
            />
          </Dialog>
        </div>
        <DataTable
          value={local}
          expandedRows={expandedRows}
          onRowToggle={(e) => setExpandedRows(e.data)}
          rowExpansionTemplate={rowExpansionTemplate}
        >
          <Column expander={allowExpansion} style={{ width: "5rem" }}></Column>
          <Column field="id" header="Id"></Column>
          <Column field="description" header="Description"></Column>
          <Column field="localNumber" header="Local Number"></Column>
          <Column field="surface" header="Surface"></Column>
          <Column field="needsRepair" header="Needs Repair"></Column>
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
                  className="pi pi-warehouse"
                  style={{
                    fontSize: "1.5rem",
                    cursor: "pointer",
                    marginRight: "10px",
                  }}
                  onClick={() => makeReservation(rowData.id)}
                ></i>
                <i
                  className="pi pi-trash"
                  style={{ fontSize: "1.5rem", cursor: "pointer" }}
                  onClick={() => showLocalDeleteConfirmation(rowData.id)}
                ></i>
              </>
            )}
          ></Column>
        </DataTable>
        <Paginator
          first={firstLocal}
          rows={rowsLocal}
          totalRecords={totalLocals}
          onPageChange={onPageChangeLocals}
          rowsPerPageOptions={[5, 10, 20, 30]}
          style={{ border: "none" }}
        />
        <Dialog
          header="Make a Reservation"
          visible={visibleReservation}
          onHide={() => setVisibleReservation(false)}
        >
          {selectedLocalId && (
            <AddReservation
              selectedLocalId={selectedLocalId}
              onClose={() => setVisibleReservation(false)}
              onAddSuccess={handleAddReservationSuccess}
            />
          )}
        </Dialog>
      </Panel>
      <Panel
        ref={reservationPanelRef}
        header="All Reservations"
        toggleable
        collapsed
      >
        <div className="flex justify-content-start xl:flex-row lg:flex-row md:flex-column sm:flex-column gap-3 my-4">
          <IconField iconPosition="left">
            <InputIcon className="pi pi-search"> </InputIcon>
            <InputText
              value={searchReservationTerm}
              onChange={(e) => setSearchReservationTerm(e.target.value)}
              placeholder="Search Reservation"
            />
          </IconField>
        </div>
        <DataTable value={reservations}>
          <Column field="id" header="ID"></Column>
          <Column
            field="checkInDate"
            header="Check In Date"
            body={(rowData) => formatDateTime(rowData.checkInDate)}
          ></Column>
          <Column
            field="checkOutDate"
            header="Check Out Date"
            body={(rowData) => formatDateTime(rowData.checkOutDate)}
          ></Column>
          <Column
            header="Action"
            body={(rowData) => (
              <i
                className="pi pi-trash"
                style={{ fontSize: "1.5rem", cursor: "pointer" }}
                onClick={() => showReservationDeleteConfirmation(rowData.id)}
              ></i>
            )}
          ></Column>
        </DataTable>
        <Paginator
          first={firstReservation}
          rows={rowsReservation}
          totalRecords={totalReservation}
          onPageChange={onPageChangeReservations}
          rowsPerPageOptions={[5, 10, 20, 30]}
          style={{ border: "none" }}
        />
      </Panel>
      <ConfirmationDialog
        visible={confirmLocalVisible}
        header="Confirm Local Deletion"
        message="Are you sure you want to delete this local?"
        onConfirm={handleDeleteLocal}
        onCancel={() => setConfirmLocalVisible(false)}
      />
      <ConfirmationDialog
        visible={confirmReservationVisible}
        header="Confirm Task Deletion"
        message="Are you sure you want to delete this reservation?"
        onConfirm={handleDeleteReservation}
        onCancel={() => setConfirmReservationVisible(false)}
      />

      <Dialog
        header="Update Local"
        visible={updateVisible}
        onHide={() => setUpdateVisible(false)}
      >
        {selectedLocal && (
          <UpdateLocal
            local={selectedLocal}
            onClose={() => setUpdateVisible(false)}
            onUpdateSuccess={handleUpdateSuccess}
          />
        )}
      </Dialog>
    </div>
  );
};
export default LocalReservation;
