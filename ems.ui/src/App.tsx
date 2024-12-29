import React from "react";
import "./App.css";
import "primeflex/primeflex.css";
import Task from "./Components/Task/Task.tsx";
import Search from "./Components/Search/Search.tsx";
import ListEmployee from "./Components/Employee/ListEmployee/ListEmployee.tsx";
import AddEmployee from "./Components/Employee/AddEmployee/AddEmployee.tsx";
import Navbar from "./Components/Navbar/Navbar.tsx";

function App() {
  return (
    <div className="App">
      <Navbar />
      <Search />
      <ListEmployee />
      {/* <AddEmployee /> */}
    </div>
  );
}

export default App;
