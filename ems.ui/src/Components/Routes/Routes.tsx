import { createBrowserRouter } from "react-router-dom";
import App from "../../App.tsx";

import React from "react";
import LoginPage from "../Pages/LoginPage/LoginPage.tsx";
import RegisterPage from "../Pages/RegisterPage/RegisterPage.tsx";
import ListEmployee from "../Employee/ListEmployee/ListEmployee.tsx";
import ProtectedRoute from "./ProtectedRoute.tsx";
import AdministrationPanel from "../Admin/AdministrationPanel.tsx";
import Budget from "../Budget/BudgetTransaction.tsx";
import ListAddress from "../Address/ListAddress/ListAddress.tsx";
import ListTask from "../Task/ListTask/ListTask.tsx";
import LocalReservation from "../LocalReservation/LocalReservationList/LocalReservation.tsx";

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
      { path: "login", element: <LoginPage /> },
      { path: "register", element: <RegisterPage /> },
    ],
  },
]);
