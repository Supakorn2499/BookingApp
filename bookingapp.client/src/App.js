import React from "react";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import Layout from "./Layout";
import Dashboard from "./components/pages/Dashboard";
import Booking from "./components/pages/Booking";

export default function App() {
  return (
    <Router>
      <Routes>
        <Route path="/" element={<Layout />}>
          <Route path="dashboard" element={<Dashboard />} />
          <Route path="booking" element={<Booking />} />
          {/* เพิ่มเส้นทางเพิ่มเติมตามต้องการ */}
        </Route>
      </Routes>
    </Router>
  );
}
