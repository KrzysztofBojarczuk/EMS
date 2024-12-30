import { createBrowserRouter } from "react-router-dom";
import App from "../../App.tsx";
import Task from "../Task/Task.tsx";
import React from "react";
import LoginPage from "../Pages/LoginPage/LoginPage.tsx";
import RegisterPage from "../Pages/RegisterPage/RegisterPage.tsx";
import ListEmployee from "../Employee/ListEmployee/ListEmployee.tsx";
import ProtectedRoute from "./ProtectedRoute.tsx";

export const router = createBrowserRouter([
  {
    path: "/",
    element: <App />,
    children: [
      { path: "", element: <Task /> },
      {
        path: "Employee",
        element: (
          <ProtectedRoute>
            <ListEmployee />
          </ProtectedRoute>
        ),
      },
      { path: "login", element: <LoginPage /> },
      { path: "register", element: <RegisterPage /> },
    ],
  },
]);
