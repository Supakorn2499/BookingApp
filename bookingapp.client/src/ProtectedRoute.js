import React, { useContext } from "react";
import { Navigate, Outlet } from "react-router-dom";
import { AuthContext } from "./AuthContext";
import { LanguageContext } from "./LanguageContext";

export default function ProtectedRoute() {
  const { isAuthenticated } = useContext(AuthContext);
  const { language } = useContext(LanguageContext);

  return isAuthenticated ? (
    <Outlet context={{ language }} />
  ) : (
    <Navigate to="/login" />
  );
}
