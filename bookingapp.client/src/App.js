import React, { useEffect, useState } from "react";
import { BrowserRouter, Routes, Route } from "react-router-dom";
import Layout from "./Layout";
import Dashboard from "./pages/Dashboard";
import Booking from "./pages/Booking";
import Login from "./pages/Login";
import Product from "./pages/Product";
import { LanguageProvider } from "./LanguageContext";
import { AuthProvider } from "./AuthContext";
import ProtectedRoute from "./ProtectedRoute";

export default function App() {

  return (
    <LanguageProvider>
      <AuthProvider>
        <BrowserRouter>
          <Routes>
            <Route path="/login" element={<Login />} />
            <Route element={<ProtectedRoute />}>
              <Route element={<Layout />}>
              <Route index path="/" element={<Dashboard />} />
                <Route path="dashboard" element={<Dashboard />} />
                <Route path="booking" element={<Booking />} />
                <Route path="product" element={<Product />} />
              </Route>
            </Route>
          </Routes>
        </BrowserRouter>
      </AuthProvider>
    </LanguageProvider>
  );
}
