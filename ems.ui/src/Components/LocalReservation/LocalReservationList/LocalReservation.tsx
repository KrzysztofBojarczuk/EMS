import React, { useEffect, useRef, useState } from "react";
import { LocalGet } from "../../../Models/Local.ts";
import { Panel } from "primereact/panel";
import { UserGetLocalService } from "../../../Services/LocalReservationService.tsx";
import { InputText } from "primereact/inputtext";
import { DataTable } from "primereact/datatable";
import { Paginator } from "primereact/paginator";
import { Column } from "primereact/column";
import { IconField } from "primereact/iconfield";
import { InputIcon } from "primereact/inputicon";
import { Button } from "primereact/button";
import { Dialog } from "primereact/dialog";
import AddLocal from "../AddLocal/AddLocal.tsx";

type Props = {};

const LocalReservation = (props: Props) => {
  const [local, setLocals] = useState<LocalGet[]>([]);
  const [visibleLocal, setVisibleLocal] = useState<boolean>(false);
  const [searchLocalTerm, setSearchLocalTerm] = useState("");

  const [firstLocal, setFirstLocal] = useState(0);
  const [rowsLocal, setRowsLocal] = useState(10);
  const [totalLocals, setTotalLocals] = useState(0);

  const localPanelRef = useRef<Panel>(null);

  const fetchLocal = async (page: number, size: number) => {
    const data = await UserGetLocalService(page, size, searchLocalTerm);
    setLocals(data.localGet);
    setTotalLocals(data.totalItems);
  };

  useEffect(() => {
    fetchLocal(1, rowsLocal);
  }, [searchLocalTerm]);

  const onPageChangeLocals = (event: any) => {
    setFirstLocal(event.first);
    setRowsLocal(event.rows);
    fetchLocal(event.page + 1, event.rows);
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
              onAddSuccess={() => fetchLocal(1, rowsLocal)}
            />
          </Dialog>
        </div>
        <DataTable value={local} paginator rows={5}>
          <Column field="id" header="Id" />
          <Column field="description" header="Description" />
          <Column field="localNumber" header="Local Number" />
          <Column field="surface" header="Surface" />
          <Column field="needsRepair" header="Needs Repair" />
        </DataTable>
        <Paginator
          first={firstLocal}
          rows={rowsLocal}
          totalRecords={totalLocals}
          onPageChange={onPageChangeLocals}
          rowsPerPageOptions={[5, 10, 20, 30]}
          style={{ border: "none" }}
        />
      </Panel>
    </div>
  );
};
export default LocalReservation;
