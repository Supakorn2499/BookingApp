import React, { createContext, useState, useEffect } from "react";

// สร้าง Context
export const AuthContext = createContext();

// Provider เพื่อแชร์สถานะการล็อกอินให้ทุกส่วนของแอป
export const AuthProvider = ({ children }) => {
  const [isAuthenticated, setIsAuthenticated] = useState(true);

  // ฟังก์ชันตรวจสอบความถูกต้องของ token
  const validateToken = (token) => {
    try {
      const payload = JSON.parse(atob(token.split(".")[1]));
      return payload.exp * 1000 > Date.now(); // exp (เวลาหมดอายุ) ต้องมากกว่าปัจจุบัน
    } catch (error) {
      console.error("Invalid token:", error);
      return false;
    }
  };

  // ตรวจสอบ token ใน localStorage เมื่อโหลดแอปครั้งแรก
  useEffect(() => {
    const token = localStorage.getItem("token");
    if (token) {
      const isValid = validateToken(token);
      setIsAuthenticated(isValid);
    } else {
      setIsAuthenticated(false);
    }
  }, []);

  // ฟังก์ชันเข้าสู่ระบบ
  const login = (token) => {
    localStorage.setItem("token", token);
    setIsAuthenticated(true);
  };

  // ฟังก์ชันออกจากระบบ
  const logout = () => {
    localStorage.removeItem("token");
    setIsAuthenticated(false);
  };

  return (
    <AuthContext.Provider value={{ isAuthenticated, login, logout }}>
      {children}
    </AuthContext.Provider>
  );
};
