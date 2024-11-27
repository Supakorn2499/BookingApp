import React, { createContext, useState } from "react";

export const LanguageContext = createContext();

// Provider เพื่อให้ Layout หรือ Outlet เข้าถึงภาษา
export const LanguageProvider = ({ children }) => {
  const [language, setLanguage] = useState("th");

  const toggleLanguage = () => {
    setLanguage((prev) => (prev === "en" ? "th" : "en"));
  };

  return (
    <LanguageContext.Provider value={{ language, toggleLanguage }}>
      {children}
    </LanguageContext.Provider>
  );
};
