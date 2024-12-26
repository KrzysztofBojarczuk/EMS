import React from "react";
import "./App.css";
import "primeflex/primeflex.css";
import Task from "./Components/Task/Task.tsx";
import EmployeeList from "./Components/EmployeeList/EmployeeList.tsx";

function App() {
  return (
    <div className="App">
      <EmployeeList />
    </div>
  );
}

export default App;
