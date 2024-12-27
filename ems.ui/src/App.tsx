import React from "react";
import "./App.css";
import "primeflex/primeflex.css";
import Task from "./Components/Task/Task.tsx";
import EmployeeList from "./Components/EmployeeList/EmployeeList.tsx";
import Search from "./Components/Search/Search.tsx";

function App() {
  return (
    <div className="App">
      <Search />
      <EmployeeList />
    </div>
  );
}

export default App;
