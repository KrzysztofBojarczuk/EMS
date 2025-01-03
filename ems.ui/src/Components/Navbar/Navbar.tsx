import React from "react";
import { Menubar } from "primereact/menubar";
import { MenuItem } from "primereact/menuitem";
import { useNavigate } from "react-router";
import { useAuth } from "../../Context/useAuth.tsx";

type Props = {};

const Navbar = (props: Props) => {
  const navigate = useNavigate();
  const { isLoggedIn, user, logout } = useAuth();

  const items: MenuItem[] = [];

  if (isLoggedIn()) {
    if (user?.roles.includes("Admin")) {
      items.push({
        label: "Admin",
        icon: "pi pi-users",
        command: () => navigate("/Admin"),
      });
    }

    if (user?.roles.includes("User")) {
      items.push(
        {
          label: "Task",
          icon: "pi pi-home",
          command: () => navigate("/Task"),
        },
        {
          label: "Employees",
          icon: "pi pi-user",
          command: () => navigate("/Employee"),
        }
      );
    }

    items.push(
      {
        label: `Welcome, ${user?.userName}`,
        icon: "pi pi-user",
      },
      {
        label: "Logout",
        icon: "pi pi-sign-out",
        command: () => {
          logout();
          navigate("/");
        },
      }
    );
  } else {
    items.push(
      {
        label: "Login",
        icon: "pi pi-sign-in",
        command: () => navigate("/login"),
      },
      {
        label: "Signup",
        icon: "pi pi-user-plus",
        command: () => navigate("/register"),
      }
    );
  }

  const start = (
    <div className="flex align-items-center">
      <i className="pi pi-briefcase mr-2"></i>
      <h3>EMS</h3>
    </div>
  );

  return (
    <Menubar
      className="flex justify-content-center"
      model={items}
      start={start}
    />
  );
};

export default Navbar;
