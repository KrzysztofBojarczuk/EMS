import React from "react";
import { Navigate } from "react-router";
import { useLocation } from "react-router-dom";
import { useAuth } from "../../Context/useAuth";

type Props = {
  children: React.ReactNode;
  allowedRoles: string[];
};

const ProtectedRoute = ({ children, allowedRoles }: Props) => {
  const { user, isLoggedIn } = useAuth();

  if (!isLoggedIn() || !user) {
    return <Navigate to="/login" />;
  }

  const userRoles = user?.roles || [];
  const hasAccess = allowedRoles.some((role) => userRoles.includes(role));

  if (!hasAccess) {
    return <Navigate to="/" />;
  }

  return <>{children}</>;
};

export default ProtectedRoute;
