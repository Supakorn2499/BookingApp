import React, { createContext, useState, useEffect } from 'react';

// สร้าง Context
export const AuthContext = createContext();

// Provider เพื่อแชร์สถานะการล็อกอินให้ทุกส่วนของแอป
export const AuthProvider = ({ children }) => {
  const [isAuthenticated, setIsAuthenticated] = useState(false);

  // ตรวจสอบ token ใน localStorage เมื่อโหลดแอปครั้งแรก
  useEffect(() => {
    const token = localStorage.getItem('token');
    if (token) {
      // สมมติว่ามีฟังก์ชันตรวจสอบ token validity
      const isValid = validateToken(token);
      setIsAuthenticated(isValid);
    }
  }, []);

  // ฟังก์ชันสมมติสำหรับตรวจสอบ token
  const validateToken = (token) => {
    // เช็คว่า token ยังไม่หมดอายุ
    // ตัวอย่างสมมติ: token เป็น JSON Web Token (JWT)
    try {
      const payload = JSON.parse(atob(token.split('.')[1]));
      return payload.exp * 1000 > Date.now();
    } catch (error) {
      return false;
    }
  };

  // ฟังก์ชันเข้าสู่ระบบ
  const login = (token) => {
    localStorage.setItem('token', token);
    setIsAuthenticated(true);
  };

  // ฟังก์ชันออกจากระบบ
  const logout = () => {
    localStorage.removeItem('token');
    setIsAuthenticated(false);
  };

  return (
    <AuthContext.Provider value={{ isAuthenticated, login, logout }}>
      {children}
    </AuthContext.Provider>
  );
};
