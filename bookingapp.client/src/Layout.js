import { useState, useContext } from "react";
import { Link, Outlet, useLocation } from "react-router-dom";
import { LanguageContext } from "./LanguageContext";
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
import translations from "./resources/LangMenu";
import { Menu, MenuButton, MenuItem, MenuItems } from "@headlessui/react";
import { EllipsisVerticalIcon } from "@heroicons/react/20/solid";
import { AuthContext } from "./AuthContext";

function classNames(...classes) {
  return classes.filter(Boolean).join(" ");
}

const App = () => {

  const [sidebarOpen, setSidebarOpen] = useState(false);
  const location = useLocation();
  const { language, toggleLanguage } = useContext(LanguageContext);
  const { logout } = useContext(AuthContext);
  const t = translations[language];

  const navigation = [
    {
      key: "dashboard",
      name: t.dashboard,
      href: "/dashboard",
      icon: HomeIcon,
      current: true,
    },
    {
      key: "booking",
      name: t.booking,
      href: "/booking",
      icon: CalendarIcon,
      current: false,
    },
    {
      key: "quotation",
      name: t.quotation,
      href: "/quotation",
      icon: DocumentDuplicateIcon,
      current: false,
    },
    {
      key: "contract",
      name: t.contract,
      href: "/contract",
      icon: DocumentDuplicateIcon,
      current: false,
    },
    {
      key: "payment",
      name: t.payment,
      href: "/payment",
      icon: CurrencyDollarIcon,
      current: false,
    },
    {
      key: "expenses",
      name: t.expenses,
      href: "/expenses",
      icon: ClipboardDocumentListIcon,
      current: false,
    },
    {
      key: "invoice",
      name: t.invoice,
      href: "/invoice",
      icon: BellAlertIcon,
      current: false,
    },
    {
      key: "reports",
      name: t.reports,
      href: "/reports",
      icon: ChartPieIcon,
      current: false,
    },
  ];
  const teams = [
    {
      id: 1,
      key: "company",
      name: t.company,
      href: "/company",
      initial: "C",
      current: false,
    },
    {
      id: 2,
      key: "customer",
      name: t.customer,
      href: "/customer",
      initial: "M",
      current: false,
    },
    {
      id: 3,
      key: "products",
      name: t.products,
      href: "/product",
      initial: "P",
      current: false,
    },
    {
      id: 4,
      key: "others",
      name: t.others,
      href: "/others",
      initial: "O",
      current: false,
    },
  ];
  const handleLogout = async (e) => {
    logout();
  }

  return (
    <div className="h-screen">
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
                  <XMarkIcon aria-hidden="true" className="size-6 text-white" />
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
                        <li key={item.key}>
                          <Link
                            to={item.href}
                            className={classNames(
                              location.pathname === item.href
                                ? "bg-indigo-700 text-white"
                                : "text-indigo-200 hover:bg-indigo-700 hover:text-white",
                              "group flex gap-x-3 rounded-md p-2 text-sm/6 font-semibold"
                            )}
                          >
                            <item.icon
                              aria-hidden="true"
                              className={classNames(
                                location.pathname === item.href
                                  ? "text-white"
                                  : "text-indigo-200 group-hover:text-white",
                                "size-6 shrink-0"
                              )}
                            />
                            {item.name}
                          </Link>
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
                        <li key={team.key}>
                          <Link
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
                          </Link>
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
                    <li key={item.key}>
                      <Link
                        to={item.href}
                        className={classNames(
                          location.pathname === item.href
                            ? "bg-indigo-700 text-white"
                            : "text-indigo-200 hover:bg-indigo-700 hover:text-white",
                          "group flex gap-x-3 rounded-md p-2 text-sm/6 font-semibold"
                        )}
                      >
                        <item.icon
                          aria-hidden="true"
                          className={classNames(
                            location.pathname === item.href
                              ? "text-white"
                              : "text-indigo-200 group-hover:text-white",
                            "size-6 shrink-0"
                          )}
                        />
                        {item.name}
                      </Link>
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
                    <li key={team.key}>
                      <Link
                        to={team.href}
                        className={classNames(
                          location.pathname === team.href
                            ? "bg-indigo-700 text-white"
                            : "text-indigo-200 hover:bg-indigo-700 hover:text-white",
                          "group flex gap-x-3 rounded-md p-2 text-sm/6 font-semibold"
                        )}
                      >
                        <span className="flex size-6 shrink-0 items-center justify-center rounded-lg border border-indigo-400 bg-indigo-500 text-[0.625rem] font-medium text-white">
                          {team.initial}
                        </span>
                        <span className="truncate">{team.name}</span>
                      </Link>
                    </li>
                  ))}
                </ul>
              </li>
              <li className="-mx-6 mt-auto">
                <div
                  href="#"
                  className="flex items-center gap-x-4 px-6 py-3 text-sm/6 font-semibold text-white hover:bg-indigo-700 justify-between"
                >
                  <img
                    alt=""
                    src="https://images.unsplash.com/photo-1472099645785-5658abf4ff4e?ixlib=rb-1.2.1&ixid=eyJhcHBfaWQiOjEyMDd9&auto=format&fit=facearea&facepad=2&w=256&h=256&q=80"
                    className="size-8 rounded-full bg-indigo-700"
                  />
                  <span className="sr-only">Your profile</span>
                  <span aria-hidden="true">ศุภกร อ้นวิเชียร</span>

                  <Menu as="div" className="relative inline-block text-left">
                    <div>
                      <MenuButton className="flex items-center rounded-full bg-gray-100 text-gray-400 hover:text-gray-600 focus:outline-none focus:ring-2 focus:ring-indigo-500 focus:ring-offset-2 focus:ring-offset-gray-100">
                        <span className="sr-only">Open options</span>
                        <EllipsisVerticalIcon
                          aria-hidden="true"
                          className="size-5"
                        />
                      </MenuButton>
                    </div>

                    <MenuItems
                      transition
                      className="absolute bottom-7 right-0 z-10 mt-2 w-56 origin-top-right rounded-md bg-white shadow-lg ring-1 ring-black/5 transition focus:outline-none data-[closed]:scale-95 data-[closed]:transform data-[closed]:opacity-0 data-[enter]:duration-100 data-[leave]:duration-75 data-[enter]:ease-out data-[leave]:ease-in"
                    >
                      <div className="py-1">
                        <MenuItem>
                          <button
                            onClick={toggleLanguage}
                            className="block w-full px-4 py-2 text-sm text-left text-gray-700 data-[focus]:bg-gray-100 data-[focus]:text-gray-900 data-[focus]:outline-none"
                          >
                            {language === "en" ? "เปลี่ยนภาษา : ไทย" : "Switch language : English"}
                          </button>
                        </MenuItem>
                        <MenuItem>
                            <button
                              onClick={handleLogout}
                              className="block w-full px-4 py-2 text-left text-sm text-gray-700 data-[focus]:bg-gray-100 data-[focus]:text-gray-900 data-[focus]:outline-none"
                            >
                              {t.signout_button}
                            </button>
                          </MenuItem>
                      </div>
                    </MenuItems>
                  </Menu>
                </div>
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
          <Outlet context={{ language }} />
        </div>
      </main>
    </div>
  );
};

export default App;
