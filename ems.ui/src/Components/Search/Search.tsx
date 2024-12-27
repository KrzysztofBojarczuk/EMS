import React, { ChangeEvent, JSX, useState } from "react";
import { InputText } from "primereact/inputtext";
import { Button } from "primereact/button";
type Props = {};

const Search: React.FC<Props> = (props: Props): JSX.Element => {
  const [search, setsearch] = useState<string>("");

  const handleChange = (e: ChangeEvent<HTMLInputElement>) => {
    setsearch(e.target.value);
    console.log("%csrcComponentsSearchSearch.tsx:11 e", "color: #007acc;", e);
  };

  //Funkcja onClick obsługuje zdarzenie kliknięcia przycisku.
  //Przycisk Button z przypisanym zdarzeniem onClick
  const onClick = (e: any) => {
    console.log("%csrcComponentsSearchSearch.tsx:20 e", "color: #007acc;", e);
  };

  return (
    <div>
      <InputText value={search} onChange={(e) => handleChange(e)} />
      Search
      <Button onClick={(e) => onClick(e)} />
    </div>
  );
};

export default Search;
