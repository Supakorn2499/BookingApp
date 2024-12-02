import { useState, useEffect } from "react";
import api from "./apiConfig.js";
import {
  BarsArrowUpIcon,
  ChevronLeftIcon,
  ChevronRightIcon,
} from "@heroicons/react/20/solid";
import { formatDateTime, formatNumber } from "./Utilitys";
import { Link, useLocation } from "react-router-dom";
import  PaytypeForm from "../components/PaytypeForm.js";
import { useOutletContext } from "react-router-dom";
import Swal from "sweetalert2";

const Paytype = () => {
  const [data, setDatas] = useState([]);
  const [searchTerm, setSearchTerm] = useState("");
  const [page, setPage] = useState(1);
  const [totalPages, setTotalPages] = useState(0);
  const [totalRecords, settotalRecords] = useState(0);
  const [isLoading, setIsLoading] = useState(false);
  const [popupVisible, setPopupVisible] = useState(false);
  const [deleteId, setDeleteId] = useState(null);
  const [deleteName, setDeleteName] = useState(null);
  const [errorMessage, setErrorMessage] = useState("");
  const { language } = useOutletContext();

  // แปลภาษา
  const translations = {
    th: {
      title: "ประเภทการชำระ",
      navbar: "ธนาคาร",
      keyword: "ค้นหาด้วยรหัส/ชื่อ",
      create: "เพิ่มใหม่",
      code: "รหัส",
      name1: "ชื่อไทย",
      name2: "ชื่ออังกฤษ",
      status: "สถานะ",
      createdate: "วันที่สร้าง",
      createby: "สร้างโดย",
      delete: "ลบ",
      update: "แก้ไข",
      firstpage: "หน้าแรก",
      prevpage: "ก่อนหน้า",
      lastpage: "หน้าสุดท้าย",
      number: "จำนวน",
      page: "หน้า",
      of: "จาก",
      nextpage: "ถัดไป",
      record: "รายการ",
      reset: "รีเซ็ต",
      back: "หน้าหลัก",
      loading: "กำลังโหลดข้อมูล...",
      nodata: "ไม่พบข้อมูล",
      confirmtitle: "ยืนยันการลบ",
      confirm_msg: "คุณต้องการลบข้อมูล ",
      cancel: "ยกเลิก",
      popupformtitle_addnew: "เพิ่มใหม่",
      popupformtitle_update: "แก้ไข",
      swal_title: "แจ้งเตือน",
      swal_failed_msg: "เกิดข้อผิดพลาด",
      swal_success_msg: "บันทึกข้อมูลสำเร็จ",
      swal_button_clsoe: "ปิด",
      swal_button_ok: "ตกลง",
      swal_button_cancel: "ยกเลิก",
    },
    en: {
      title: "Payment Type",
      navbar: "Bank",
      keyword: "Code/Name",
      create: "Add New",
      code: "Code",
      name1: "Name (Thail)",
      name2: "Name (English)",
      status: "Status",
      createdate: "Create Date",
      createby: "Create by",
      delete: "Delete",
      update: "Update",
      firstpage: "Home",
      prevpage: "Prevpage",
      lastpage: "LastPage",
      number: "Number",
      page: "Page",
      of: "Of",
      nextpage: "Next",
      record: "Records",
      reset: "Clear",
      back: "Back",
      loading: "Loading...",
      nodata: "Data not found.",
      confirmtitle: "Confirmation",
      confirm_msg: "You want to delete ",
      cancel: "Cancel",
      popupformtitle_addnew: "Add New",
      popupformtitle_update: "Update",
      swal_title: "Information",
      swal_failed_msg: "Failed to create data. Please try again.",
      swal_success_msg: "successfully!",
      swal_button_clsoe: "Close",
      swal_button_ok: "Ok",
      swal_button_cancel: "Cancel",
    },
  };

  const currentLanguage = translations[language] ? language : "th";
  const translation = translations[currentLanguage];

  // ฟอร์มจัดการข้อมูล
  const [formVisible, setFormVisible] = useState(false);
  const [formMode, setFormMode] = useState("add");
  const [formData, setFormData] = useState(null);

  const ITEMS_PER_PAGE = 15;

  // ค้นหาข้อมูล
  const fetchDatas = async (pageNumber, search = "") => {
    setIsLoading(true);
    setErrorMessage("");
    try {
      const { data } = await api.get("/Paytype/Search", {
        params: {
          pageNumber: pageNumber,
          pageSize: ITEMS_PER_PAGE,
          keyword: search,
        },
      });

      setDatas(data.data || []);
      setTotalPages(data.totalPages || 1);
      settotalRecords(data.totalRecords || 0);
    } catch (error) {
      setErrorMessage("ไม่สามารถโหลดข้อมูล");
      console.error("Error fetching data:", error);
    } finally {
      setIsLoading(false);
    }
  };

  // Monitor การเปลี่ยนแปลงการคีย์ค้นหาข้อมูล
  useEffect(() => {
    fetchDatas(page, searchTerm.trim());
  }, [page, searchTerm]);

  const handleResetSearch = () => {
    setSearchTerm("");
    setPage(1);
  };

  //เปลี่ยนเพจแสดงข้อมูลของตาราง
  const handlePageChange = (newPage) => {
    if (newPage >= 1 && newPage <= totalPages) setPage(newPage);
  };

  const handleClosForm = (Boolean) => {
    setFormVisible(Boolean);
  };

  //เพิ่มข้อมูลใหม่
  const handleCreate  = async (formData) => {
    console.log(JSON.stringify(formData));
    try {
      const response = await api.post(
        "/Paytype/Create",
        JSON.stringify(formData),
        { headers: { "Content-Type": "application/json" } }
      );
    } catch (error) {
      console.error("Error updating  :", error);
      Swal.fire({
        icon: "error",
        title: translation.swal_failed_title,
        text: translation.swal_failed_msg,
        confirmButtonText: translation.swal_button_clsoe,
      });
    } finally {
      fetchDatas(page, searchTerm.trim());
      handleClosForm(false);
      setFormData(null);
      Swal.fire({
        icon: "success",
        title: translation.swal_success_title,
        text: translation.swal_success_msg,
        confirmButtonText: translation.swal_button_clsoe,
      });
    }
  };

  // แก้ไขข้อมูล
  const handleEdit  = async (formData) => {
    try {
      //console.log(JSON.stringify(formData));
      const response = await api.put(
        "/Paytype/Update?id=" + formData.id,
        JSON.stringify(formData),
        { headers: { "Content-Type": "application/json" } }
      );
    } catch (error) {
      console.error("Error updating  :", error);
      Swal.fire({
        icon: "error",
        title: translation.swal_failed_title,
        text: translation.swal_failed_msg,
        confirmButtonText: translation.swal_button_clsoe,
      });
    } finally {
      fetchDatas(page, searchTerm.trim());
      handleClosForm(false);
      setFormData(null);
      Swal.fire({
        icon: "success",
        title: translation.swal_success_title,
        text: translation.swal_success_msg,
        confirmButtonText: translation.swal_button_clsoe,
      });
    }
  };

  // ลบข้อมูลออกจากระบบ
  const handleDelete = async () => {
    try {
      await api.delete(`/Paytype/Delete/?id=${deleteId}`);
      setDatas((prev) => prev.filter((data) => data.id !== deleteId));
    } catch (error) {
      console.error("Error deleting data:", error);
    } finally {
      setPopupVisible(false);
      setDeleteId(null);
      setDeleteName(null);
      handleResetSearch();
      Swal.fire({
        icon: "success",
        title: translation.swal_success_title,
        text: translation.swal_success_msg,
        confirmButtonText: translation.swal_button_clsoe,
      });
    }
  };

  const confirmDelete = (id, name) => {
    setDeleteId(id);
    setDeleteName(name);
    setPopupVisible(true);
  };

  return (
    <div>
      <nav aria-label="Back" className="sm:hidden">
        <Link
          href="#"
          className="flex items-center text-sm font-medium text-gray-500 hover:text-gray-700"
        >
          <ChevronLeftIcon
            aria-hidden="true"
            className="-ml-1 mr-1 size-5 shrink-0 text-gray-400"
          />
          {translation.back}
        </Link>
      </nav>
      <nav aria-label="Breadcrumb" className="hidden sm:flex">
        <ol role="list" className="flex items-center space-x-4">
          <li>
            <div className="flex">
              <Link
                to="/bank"
                className="text-sm font-medium text-gray-500 hover:text-gray-700"
              >
                {translation.navbar}
              </Link>
            </div>
          </li>
          <li>
            <div className="flex items-center">
              <ChevronRightIcon
                aria-hidden="true"
                className="size-5 shrink-0 text-gray-400"
              />
              <span className="ml-4 text-sm font-medium text-gray-500 hover:text-gray-700">
                {translation.title}
              </span>
            </div>
          </li>
        </ol>
      </nav>

      <div className="mt-2 md:flex md:items-center md:justify-between">
        <div className="min-w-0 flex-1">
          <h2 className="text-2xl/7 font-bold text-gray-900 sm:truncate sm:text-3xl sm:tracking-tight">
            {translation.title}
          </h2>
        </div>
      </div>

      <div className="sm:flex sm:items-centerm mb-2">
        <div className="sm:flex-auto">
          <div className="mt-2 flex rounded-md shadow-sm">
            <div className="relative flex grow items-stretch focus-within:z-10">
              <div className="pointer-events-none absolute inset-y-0 left-0 flex items-center pl-3">
                <BarsArrowUpIcon
                  aria-hidden="true"
                  className="size-5 text-gray-400"
                />
              </div>
              <input
                type="text"
                placeholder={translation.keyword}
                value={searchTerm}
                onChange={(e) => setSearchTerm(e.target.value)}
                className="block w-full rounded-none rounded-l-md border-0 py-1.5 pl-10 text-gray-900 ring-1 ring-inset ring-gray-300 placeholder:text-gray-400 focus:ring-2 focus:ring-inset focus:ring-blue-400 sm:text-sm/6"
              />
            </div>
            <button
              onClick={handleResetSearch}
              className="relative -ml-px inline-flex items-center gap-x-1.5 rounded-r-md px-3 py-2 text-sm font-semibold text-gray-900 ring-1 ring-inset ring-gray-300 hover:bg-gray-50"
            >
              {translation.reset}
            </button>
            <button
              className="ml-4 rounded-md bg-blue-600 px-3 py-2 text-sm font-semibold text-white shadow-sm hover:bg-blue-500 focus-visible:outline focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-blue-600"
              onClick={() => {
                setFormMode("add");
                setFormVisible(true);
              }}
            >
              {translation.create}
            </button>
          </div>
        </div>
      </div>

      {isLoading ? (
        <p className="text-center">{translation.loading}</p>
      ) : errorMessage ? (
        <p className="text-center text-red-500">{errorMessage}</p>
      ) : data.length === 0 ? (
        <div className="mt-2 ring-1 ring-gray-300 sm:mx-0 sm:rounded-sm overflow-x-auto">
          <table className="min-w-full divide-y  divide-gray-300">
            <thead>
              <tr className="divide-x divide-gray-200">
                <th className="py-1.5 pl-4 pr-4 text-left text-sm font-semibold text-gray-900">
                  {translation.code}
                </th>
                <th className="px-4 py-1 text-left text-sm font-semibold text-gray-900">
                  {translation.name1}
                </th>
                <th className="px-4 py-1 text-center text-sm font-semibold text-gray-900">
                  {translation.name2}
                </th>
                <th className="px-4 py-1 text-center text-sm font-semibold text-gray-900">
                  {translation.active}
                </th>

                <th className="px-4 py-1 text-center text-sm font-semibold text-gray-900">
                  {translation.delete}
                </th>
                <th className="px-4 py-1 text-center text-sm font-semibold text-gray-900">
                  {translation.update}
                </th>
              </tr>
            </thead>
            <tbody className="divide-y divide-gray-200 bg-white">
              <tr className="divide-x divide-gray-200">
                <td
                  className="py-1.5 pl-4 pr-4 text-center text-sm"
                  colSpan={9}
                >
                  <span className="text-center">{translation.nodata}</span>
                </td>
              </tr>
            </tbody>
          </table>
        </div>
      ) : (
        <div className="min-w-full mt-2 ring-1 ring-gray-300 sm:mx-0 sm:rounded-sm overflow-x-auto">
          <table className="min-w-full divide-y  divide-gray-300">
            <thead>
              <tr className="divide-x divide-gray-200">
                <th className="py-1.5 pl-4 pr-4 text-left text-sm font-semibold text-gray-900">
                  {translation.code}
                </th>
                <th className="px-4 py-1 text-left text-sm font-semibold text-gray-900">
                  {translation.name1}
                </th>
                <th className="px-4 py-1 text-center text-sm font-semibold text-gray-900">
                  {translation.name2}
                </th>

                <th className="px-4 py-1 text-center text-sm font-semibold text-gray-900">
                  {translation.active}
                </th>

                <th className="px-4 py-1 text-center text-sm font-semibold text-gray-900">
                  {translation.delete}
                </th>
                <th className="px-4 py-1 text-center text-sm font-semibold text-gray-900">
                  {translation.update}
                </th>
              </tr>
            </thead>
            <tbody className="divide-y divide-gray-200 bg-white">
              { data.map((d) => (
                <tr key={d.id} className="divide-x divide-gray-200">
                  <td className="whitespace-nowrap p-1 text-sm font-medium text-gray-900 ">
                    {d.code}
                  </td>
                  <td className="whitespace-nowrap p-1 text-sm text-gray-600">
                    { d.name1}
                  </td>
                  <td className="whitespace-nowrap p-1 text-sm text-gray-600">
                    { d.name2}
                  </td>
                  <td className="whitespace-nowrap p-1 text-sm text-center text-gray-600">
                    { d.active === "Y" ? "Active" : "Inactive"}
                  </td>
                  <td className="whitespace-nowrap p-1 text-sm text-center text-gray-600">
                    <button
                      className="text-red-500 font-semibold"
                      onClick={() =>
                        confirmDelete(
                           d.id,
                           d.code + " : " +  d.name1
                        )
                      }
                    >
                      <span>{translation.delete}</span>
                    </button>
                  </td>
                  <td className="whitespace-nowrap p-1 text-sm text-center text-gray-600">
                    <button
                      className="text-blue-500 font-semibold"
                      onClick={() => {
                        setFormMode("edit");
                        setFormData({
                          id:  d.id,
                          companyid: 1,
                          name1:  d.name1,
                          name2:  d.name2,
                          code: d.code,
                          active: d.active,
                          isdeposit: d.isdeposit,
                          iswithdraw: d.iswithdraw,
                          group: d.group,
                          inout: d.inout,
                        });
                        setFormVisible(true);
                      }}
                    >
                      <span>{translation.update}</span>
                    </button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}

      <div className="flex justify-center mt-4">
        <button
          className="mr-2 px-4 py-2 text-sm font-semibold bg-gray-200 rounded-lg hover:bg-gray-300"
          disabled={page === 1 || isLoading}
          onClick={() => handlePageChange(1)}
        >
          {translation.firstpage}
        </button>
        <button
          className="px-4 py-2 text-sm font-semibold bg-gray-200 rounded-lg hover:bg-gray-300"
          disabled={page === 1 || isLoading}
          onClick={() => handlePageChange(page - 1)}
        >
          {translation.prevpage}
        </button>
        <span className="mx-4">
          {translation.page} {page} {translation.of} {totalPages}
        </span>
        <button
          className="mr-2 px-4 py-2 text-sm font-semibold bg-gray-200 rounded-lg hover:bg-gray-300"
          disabled={page === totalPages || isLoading}
          onClick={() => handlePageChange(page + 1)}
        >
          {translation.nextpage}
        </button>
        <button
          className="px-4 py-2 text-sm font-semibold bg-gray-200 rounded-lg hover:bg-gray-300"
          disabled={page === totalPages || isLoading}
          onClick={() => handlePageChange(totalPages)}
        >
          {translation.lastpage}
        </button>
        <span className="mx-4">
          {" "}
          {translation.number} {totalRecords} {translation.record}
        </span>
      </div>

      {popupVisible && (
        <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center">
          <div className="bg-white p-6 rounded-lg shadow-lg">
            <h2 className="text-2xl font-bold mb-4">
              {translation.confirmtitle}
            </h2>
            <p>
              {translation.confirm_msg} {deleteName} ?
            </p>
            <div className="flex justify-end space-x-4 mt-4">
              <button
                className="bg-gray-300 px-4 py-2 rounded-lg hover:bg-gray-400"
                onClick={() => setPopupVisible(false)}
              >
                {translation.cancel}
              </button>
              <button
                className="bg-red-500 text-white px-4 py-2 rounded-lg hover:bg-red-600"
                onClick={handleDelete}
              >
                {translation.delete}
              </button>
            </div>
          </div>
        </div>
      )}

      {formVisible && (
        <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center">
          <div className="bg-white p-6 rounded-lg shadow-lg w-full max-w-lg">
            <h2 className="text-2xl font-bold mb-4">
              {formMode === "add"
                ? translation.popupformtitle_addnew
                : translation.popupformtitle_update}
            </h2>
            {formMode === "add" ? (
              < PaytypeForm
                initialData={null}
                onSubmit={handleCreate}
                onCancel={handleClosForm}
                language
              />
            ) : (
              < PaytypeForm
                initialData={formData}
                onSubmit={handleEdit}
                onCancel={handleClosForm}
                language
              />
            )}
          </div>
        </div>
      )}
    </div>
  );
};

export default Paytype;
