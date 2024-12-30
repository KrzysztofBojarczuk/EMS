import React from "react";
import { Navigate } from "react-router";
import { useLocation } from "react-router-dom";
import { useAuth } from "../../Context/useAuth.tsx";

type Props = { children: React.ReactNode };

const ProtectedRoute = ({ children }: Props) => {
  const location = useLocation();
  const { isLoggedIn } = useAuth();
  return isLoggedIn() ? (
    <>{children}</>
  ) : (
    <Navigate to="/login" state={{ from: location }} replace />
  );
};

export default ProtectedRoute;
