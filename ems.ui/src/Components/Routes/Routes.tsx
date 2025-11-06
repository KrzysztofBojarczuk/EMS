import { createBrowserRouter } from "react-router-dom";
import App from "../../App";

import React from "react";
import LoginPage from "../Pages/LoginPage/LoginPage";
import RegisterPage from "../Pages/RegisterPage/RegisterPage";
import ListEmployee from "../Employee/ListEmployee/ListEmployee";
import ProtectedRoute from "./ProtectedRoute";
import AdministrationPanel from "../Admin/AdministrationPanel";
import Budget from "../Budget/BudgetTransaction";
import ListAddress from "../Address/ListAddress/ListAddress";
import ListTask from "../Task/ListTask/ListTask";
import LocalReservation from "../LocalReservation/LocalReservationList/LocalReservation";
import ListVehicle from "../Vehicle/ListVehicles/ListVehicle";

export const router = createBrowserRouter([
  {
    path: "/",
    element: <App />,
    children: [
      {
        path: "Admin",
        element: (
          <ProtectedRoute allowedRoles={["Admin"]}>
            <AdministrationPanel />
          </ProtectedRoute>
        ),
      },
      {
        path: "Task",
        element: (
          <ProtectedRoute allowedRoles={["User"]}>
            <ListTask />
          </ProtectedRoute>
        ),
      },
      {
        path: "LocalsAndReservations",
        element: (
          <ProtectedRoute allowedRoles={["User"]}>
            <LocalReservation />
          </ProtectedRoute>
        ),
      },
      {
        path: "Budget",
        element: (
          <ProtectedRoute allowedRoles={["User"]}>
            <Budget />
          </ProtectedRoute>
        ),
      },
      {
        path: "Employee",
        element: (
          <ProtectedRoute allowedRoles={["User"]}>
            <ListEmployee />
          </ProtectedRoute>
        ),
      },
      {
        path: "Address",
        element: (
          <ProtectedRoute allowedRoles={["User"]}>
            <ListAddress />
          </ProtectedRoute>
        ),
      },
      {
        path: "Vehicle",
        element: (
          <ProtectedRoute allowedRoles={["User"]}>
            <ListVehicle />
          </ProtectedRoute>
        ),
      },
      { path: "login", element: <LoginPage /> },
      { path: "register", element: <RegisterPage /> },
    ],
  },
]);
