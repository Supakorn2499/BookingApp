import React, { useEffect, useState } from "react";
import { BrowserRouter, Routes, Route } from "react-router-dom";
import Layout from "./Layout";
import Dashboard from "./pages/Dashboard";
import Booking from "./pages/Booking";
import Login from "./pages/Login";
import Product from "./pages/Product";
import ProductGroup from "./pages/ProductGroup";
import { LanguageProvider } from "./LanguageContext";
import { AuthProvider } from "./AuthContext";
import ProtectedRoute from "./ProtectedRoute";
import Vattype from "./pages/Vattype";
import Paytype from "./pages/Paytype";
import Bank from "./pages/Bank";
import ProdType from "./pages/ProductType";
import BankBranch from "./pages/BankBranch";
import BookBank from "./pages/Bookbank";

export default function App() {
  return (
    <BrowserRouter>
      <LanguageProvider>
        <AuthProvider>
          <Routes>
            <Route path="/login" element={<Login />} />
            <Route element={<ProtectedRoute />}>
              <Route element={<Layout />}>
                <Route index path="/" element={<Dashboard />} />
                <Route path="dashboard" element={<Dashboard />} />
                <Route path="booking" element={<Booking />} />
                <Route path="product" element={<Product />} />
                <Route path="productgroup" element={<ProductGroup />} />
                <Route path="Vattype" element={<Vattype />} />
                <Route path="Paytype" element={<Paytype />} />
                <Route path="Bank" element={<Bank />} />
                <Route path="ProdType" element={<ProdType />} />
                <Route path="BankBranch" element={<BankBranch />} />
                <Route path="BookBank" element={<BookBank />} />
              </Route>
            </Route>
          </Routes>
        </AuthProvider>
      </LanguageProvider>
    </BrowserRouter>
  );
}
