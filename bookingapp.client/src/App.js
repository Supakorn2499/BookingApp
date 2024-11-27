import React from "react";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import Layout from "./Layout";
import Dashboard from "./pages/Dashboard";
import Booking from "./pages/Booking";
import Login from "./pages/Login";
import Product from "./pages/Product";
import { LanguageProvider } from "./LanguageContext";

export default function App() {
  return (
    <LanguageProvider>
      <Router>
        <Routes>
          <Route index path="/" element={<Login />} />
          <Route element={<Layout />}>
            <Route path="dashboard" element={<Dashboard />} />
            <Route path="booking" element={<Booking />} />
            <Route path="product" element={<Product />} />
          </Route>
        </Routes>
      </Router>
    </LanguageProvider>
  );
}
