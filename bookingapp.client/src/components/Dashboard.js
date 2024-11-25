import { useState } from "react";
import { ChevronLeftIcon, ChevronRightIcon } from "@heroicons/react/20/solid";

const Dashboard = ({ language = "th" }) => {
  const translations = {
    th: {
      title: "แดชบอร์ด",
      overview: "ภาพรวม",
      sales: "ยอดขาย",
      metrics: "ตัวชี้วัด",
    },
    en: {
      title: "Dashboard",
      overview: "Overview",
      sales: "Sales",
      metrics: "Metrics",
    },
  };

  const currentLanguage = translations[language] ? language : "th";
  const t = translations[currentLanguage];

  return (
    <div className="mt-2 md:flex md:items-center md:justify-between">
      <div className="min-w-0 flex-1">
        <h2 className="text-2xl/7 font-bold text-gray-900 sm:truncate sm:text-3xl sm:tracking-tight">
          {t.title}
        </h2>
      </div>
    </div>
  );
};

export default Dashboard;
