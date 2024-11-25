"use client";

import { useState } from "react";
import {
  Dialog,
  DialogBackdrop,
  DialogPanel,
  TransitionChild,
} from "@headlessui/react";
import {
  Bars3Icon,
  CalendarIcon,
  ChartPieIcon,
  DocumentDuplicateIcon,
  FolderIcon,
  HomeIcon,
  UsersIcon,
  XMarkIcon,
  ReceiptPercentIcon,
  ReceiptRefundIcon,
  CurrencyDollarIcon,
  BellAlertIcon,
  ClipboardDocumentListIcon,
} from "@heroicons/react/24/outline";
import Dashboard from "./components/Dashboard";
import Reports from "./components/Reports";

function classNames(...classes) {
  return classes.filter(Boolean).join(" ");
}

const App = () => {
  const [sidebarOpen, setSidebarOpen] = useState(false);
  const [currentLanguage, setCurrentLanguage] = useState("th");
  const [activeComponent, setActiveComponent] = useState("dashboard");

  const translations = {
    th: {
      dashboard: "แดชบอร์ด",
      booking: "จองพื้นที่",
      quotation: "ใบเสนอราคา",
      contract: "สัญญาเช่า",
      payment: "ชำระเงิน",
      expenses: "ค่าใช้จ่าย",
      invoice: "แจ้งหนี้",
      reports: "รายงาน",
      company: "บริษัท",
      customer: "ลูกค้า",
      products: "สินค้าและบริการ",
      others: "อื่นๆ",
      database: "ฐานข้อมูล",
      language: "English",
    },
    en: {
      dashboard: "Dashboard",
      booking: "Booking",
      quotation: "Quotation",
      contract: "Contract",
      payment: "Payment",
      expenses: "Expenses",
      invoice: "Invoice",
      reports: "Reports",
      company: "Company",
      customer: "Customer",
      products: "Products & Service",
      others: "Others",
      database: "Database",
      language: "ไทย",
    },
  };

  const t = translations[currentLanguage];
  const toggleLanguage = () => {
    setCurrentLanguage(currentLanguage === "th" ? "en" : "th");
  };

  const navigation = [
    { name: t.dashboard, href: "#", icon: HomeIcon, current: true },
    { name: t.booking, href: "#", icon: CalendarIcon, current: false },
    {
      name: t.quotation,
      href: "#",
      icon: DocumentDuplicateIcon,
      current: false,
    },
    {
      name: t.contract,
      href: "#",
      icon: DocumentDuplicateIcon,
      current: false,
    },
    { name: t.payment, href: "#", icon: CurrencyDollarIcon, current: false },
    {
      name: t.expenses,
      href: "#",
      icon: ClipboardDocumentListIcon,
      current: false,
    },
    { name: t.invoice, href: "#", icon: BellAlertIcon, current: false },
    { name: t.reports, href: "#", icon: ChartPieIcon, current: false },
  ];
  const teams = [
    { id: 1, name: t.company, href: "#", initial: "C", current: false },
    { id: 2, name: t.customer, href: "#", initial: "M", current: false },
    { id: 3, name: t.products, href: "#", initial: "P", current: false },
    { id: 4, name: t.others, href: "#", initial: "O", current: false },
  ];

  const renderComponent = () => {
    switch (activeComponent) {
      case "dashboard":
        return <Dashboard language={currentLanguage} />;
      case "reports":
        return <Reports language={currentLanguage} />;
      default:
        return <Dashboard language={currentLanguage} />;
    }
  };
  return (
    <>
      <div>
        <Dialog
          open={sidebarOpen}
          onClose={setSidebarOpen}
          className="relative z-50 lg:hidden"
        >
          <DialogBackdrop
            transition
            className="fixed inset-0 bg-gray-900/80 transition-opacity duration-300 ease-linear data-[closed]:opacity-0"
          />

          <div className="fixed inset-0 flex">
            <DialogPanel
              transition
              className="relative mr-16 flex w-full max-w-xs flex-1 transform transition duration-300 ease-in-out data-[closed]:-translate-x-full"
            >
              <TransitionChild>
                <div className="absolute left-full top-0 flex w-16 justify-center pt-5 duration-300 ease-in-out data-[closed]:opacity-0">
                  <button
                    type="button"
                    onClick={() => setSidebarOpen(false)}
                    className="-m-2.5 p-2.5"
                  >
                    <span className="sr-only">Close sidebar</span>
                    <XMarkIcon
                      aria-hidden="true"
                      className="size-6 text-white"
                    />
                  </button>
                </div>
              </TransitionChild>
              {/* Sidebar component, swap this element with another sidebar if you like */}
              <div className="flex grow flex-col gap-y-5 overflow-y-auto bg-indigo-600 px-6 pb-2">
                <div className="flex h-16 shrink-0 items-center">
                  <img
                    alt="Your Company"
                    src="https://tailwindui.com/plus/img/logos/mark.svg?color=white"
                    className="h-8 w-auto"
                  />
                </div>
                <nav className="flex flex-1 flex-col">
                  <ul role="list" className="flex flex-1 flex-col gap-y-7">
                    <li>
                      <ul role="list" className="-mx-2 space-y-1">
                        {navigation.map((item) => (
                          <li key={item.name}>
                            <a
                              href={item.href}
                              className={classNames(
                                item.current
                                  ? "bg-indigo-700 text-white"
                                  : "text-indigo-200 hover:bg-indigo-700 hover:text-white",
                                "group flex gap-x-3 rounded-md p-2 text-sm/6 font-semibold"
                              )}
                            >
                              <item.icon
                                aria-hidden="true"
                                className={classNames(
                                  item.current
                                    ? "text-white"
                                    : "text-indigo-200 group-hover:text-white",
                                  "size-6 shrink-0"
                                )}
                              />
                              {item.name}
                            </a>
                          </li>
                        ))}
                      </ul>
                    </li>
                    <li>
                      <div className="text-xs/6 font-semibold text-indigo-200">
                        {t.database}
                      </div>
                      <ul role="list" className="-mx-2 mt-2 space-y-1">
                        {teams.map((team) => (
                          <li key={team.name}>
                            <a
                              href={team.href}
                              className={classNames(
                                team.current
                                  ? "bg-indigo-700 text-white"
                                  : "text-indigo-200 hover:bg-indigo-700 hover:text-white",
                                "group flex gap-x-3 rounded-md p-2 text-sm/6 font-semibold"
                              )}
                            >
                              <span className="flex size-6 shrink-0 items-center justify-center rounded-lg border border-indigo-400 bg-indigo-500 text-[0.625rem] font-medium text-white">
                                {team.initial}
                              </span>
                              <span className="truncate">{team.name}</span>
                            </a>
                          </li>
                        ))}
                      </ul>
                    </li>
                  </ul>
                </nav>
              </div>
            </DialogPanel>
          </div>
        </Dialog>

        {/* Static sidebar for desktop */}
        <div className="hidden lg:fixed lg:inset-y-0 lg:z-50 lg:flex lg:w-72 lg:flex-col">
          {/* Sidebar component, swap this element with another sidebar if you like */}
          <div className="flex grow flex-col gap-y-5 overflow-y-auto bg-indigo-600 px-6">
            <div className="flex h-16 shrink-0 items-center">
              <img
                alt="Your Company"
                src="https://tailwindui.com/plus/img/logos/mark.svg?color=white"
                className="h-8 w-auto"
              />
            </div>
            <nav className="flex flex-1 flex-col">
              <ul role="list" className="flex flex-1 flex-col gap-y-7">
                <li>
                  <ul role="list" className="-mx-2 space-y-1">
                    {navigation.map((item) => (
                      <li key={item.name}>
                        <a
                          href={item.href}
                          className={classNames(
                            item.current
                              ? "bg-indigo-700 text-white"
                              : "text-indigo-200 hover:bg-indigo-700 hover:text-white",
                            "group flex gap-x-3 rounded-md p-2 text-sm/6 font-semibold"
                          )}
                        >
                          <item.icon
                            aria-hidden="true"
                            className={classNames(
                              item.current
                                ? "text-white"
                                : "text-indigo-200 group-hover:text-white",
                              "size-6 shrink-0"
                            )}
                          />
                          {item.name}
                        </a>
                      </li>
                    ))}
                  </ul>
                </li>
                <li>
                  <div className="text-xs/6 font-semibold text-indigo-200">
                    {t.database}
                  </div>
                  <ul role="list" className="-mx-2 mt-2 space-y-1">
                    {teams.map((team) => (
                      <li key={team.name}>
                        <a
                          href={team.href}
                          className={classNames(
                            team.current
                              ? "bg-indigo-700 text-white"
                              : "text-indigo-200 hover:bg-indigo-700 hover:text-white",
                            "group flex gap-x-3 rounded-md p-2 text-sm/6 font-semibold"
                          )}
                        >
                          <span className="flex size-6 shrink-0 items-center justify-center rounded-lg border border-indigo-400 bg-indigo-500 text-[0.625rem] font-medium text-white">
                            {team.initial}
                          </span>
                          <span className="truncate">{team.name}</span>
                        </a>
                      </li>
                    ))}
                  </ul>
                </li>
                <li className="-mx-6 mt-auto">
                  <a
                    href="#"
                    className="flex items-center gap-x-4 px-6 py-3 text-sm/6 font-semibold text-white hover:bg-indigo-700"
                  >
                    <img
                      alt=""
                      src="https://images.unsplash.com/photo-1472099645785-5658abf4ff4e?ixlib=rb-1.2.1&ixid=eyJhcHBfaWQiOjEyMDd9&auto=format&fit=facearea&facepad=2&w=256&h=256&q=80"
                      className="size-8 rounded-full bg-indigo-700"
                    />
                    <span className="sr-only">Your profile</span>
                    <span aria-hidden="true">ศุภกร อ้นวิเชียร</span>
                  </a>
                </li>
              </ul>
            </nav>
          </div>
        </div>

        <div className="sticky top-0 z-40 flex items-center gap-x-6 bg-indigo-600 px-4 py-4 shadow-sm sm:px-6 lg:hidden">
          <button
            type="button"
            onClick={() => setSidebarOpen(true)}
            className="-m-2.5 p-2.5 text-indigo-200 lg:hidden"
          >
            <span className="sr-only">Open sidebar</span>
            <Bars3Icon aria-hidden="true" className="size-6" />
          </button>
          <div className="flex-1 text-sm/6 font-semibold text-white">
            {t.dashboard}
          </div>
          <a href="#">
            <span className="sr-only">Your profile</span>
            <img
              alt=""
              src="https://images.unsplash.com/photo-1472099645785-5658abf4ff4e?ixlib=rb-1.2.1&ixid=eyJhcHBfaWQiOjEyMDd9&auto=format&fit=facearea&facepad=2&w=256&h=256&q=80"
              className="size-8 rounded-full bg-indigo-700"
            />
          </a>
        </div>

        <main className="py-10 lg:pl-72">
          <div className="px-4 sm:px-6 lg:px-8">
            <button
              onClick={toggleLanguage}
              variant="outline"
              className="flex items-center space-x-2"
            >
              <span>{t.language}</span>
            </button>
            <h1 className="text-3xl font-bold mb-6">{t[activeComponent]}</h1>
            {renderComponent()}
          </div>
        </main>
      </div>
    </>
  );
};

export default App;
