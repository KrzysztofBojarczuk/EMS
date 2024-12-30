import React from "react";
import "./App.css";
import "primeflex/primeflex.css";
import Navbar from "./Components/Navbar/Navbar.tsx";
import { Outlet } from "react-router";
import { UserProvider } from "./Context/useAuth.tsx";
import { ToastContainer } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";

function App() {
  return (
    <>
      <UserProvider>
        <Navbar />
        <Outlet />
      </UserProvider>
    </>
  );
}

export default App;
