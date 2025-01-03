import React, { JSX, useEffect, useState } from "react";
import { UserGet } from "../../Models/User.ts";
import { UserGetService } from "../../Services/UserService.tsx";
import { InputText } from "primereact/inputtext";
import { DataTable } from "primereact/datatable";
import { Column } from "primereact/column";

type Props = {};

const AdministrationPanel: React.FC<Props> = (props: Props): JSX.Element => {
  const [user, setUsers] = useState<UserGet[]>([]);
  const [searchTerm, setSearchTerm] = useState("");

  const fetchUser = async () => {
    const data = await UserGetService(searchTerm);
    setUsers(data);
  };

  useEffect(() => {
    fetchUser();
  }, [searchTerm]);
  return (
    <div className="card m-4">
      <div className="flex align-items-center justify-content-start mb-4">
        <InputText
          value={searchTerm}
          onChange={(e) => setSearchTerm(e.target.value)}
          className="mr-4"
          placeholder="Search"
        />
      </div>
      <DataTable value={user} tableStyle={{ minWidth: "50rem" }}>
        <Column field="id" header="Id"></Column>
        <Column field="userName" header="User name"></Column>
        <Column field="email" header="Email"></Column>
      </DataTable>
    </div>
  );
};

export default AdministrationPanel;
