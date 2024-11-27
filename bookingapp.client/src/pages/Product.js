import { useState, useEffect } from "react";
import api from "./apiConfig.js";
import {
  BarsArrowUpIcon,
  ChevronLeftIcon,
  ChevronRightIcon,
} from "@heroicons/react/20/solid";
import { formatDateTime, formatNumber } from "./Utilitys";
import { Link, useLocation } from "react-router-dom";
import ProductForm from "../components/ProductForm.js";

const Product = ({ language = "th" }) => {
  const [products, setProducts] = useState([]);
  const [searchTerm, setSearchTerm] = useState("");
  const [page, setPage] = useState(1);
  const [totalPages, setTotalPages] = useState(0);
  const [totalRecords, settotalRecords] = useState(0);
  const [isLoading, setIsLoading] = useState(false);
  const [popupVisible, setPopupVisible] = useState(false);
  const [deleteId, setDeleteId] = useState(null);
  const [deleteName, setDeleteName] = useState(null);
  const [errorMessage, setErrorMessage] = useState("");

  const translations = {
    th: {
      title: "สินค้าและบริการ",
      mainpage: "สินค้า",
      keyword: "รหัสสินค้า/ชื่อสินค้า",
      create: "เพิ่มสินค้าใหม่",
      code: "รหัสสินค้า",
      name1: "ชื่อสินค้าไทย",
      name2: "ชื่อสินค้าอังกฤษ",
      price: "ราคา",
      active: "สถานะ",
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
      nodata: "ไม่มีข้อมูล",
      confirmtitle : "ยืนยันการลบ",
      confirm_msg: "คุณต้องการลบสินค้า",
      cancel: "ยกเลิก",
      popupformtitle_addnew : "เพิ่มสินค้าใหม่",
      popupformtitle_update : "แก้ไขสินค้า",
    },
    en: {
      title: "Product and Server",
      mainpage: "Product",
      keyword: "Code/Name",
      create: "Add New",
      code: "Code",
      name1: "Name (Thail)",
      name2: "Name (English)",
      price: "Price",
      active: "Status",
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
      confirmtitle : "Confirmation",
      confirm_msg: "You want to delete the product.",
      cancel: "Cancel",
      popupformtitle_addnew : "Add New Product",
      popupformtitle_update : "Update Product",
    },
  };

  const currentLanguage = translations[language] ? language : "th";
  const t = translations[currentLanguage];

  // ฟอร์มจัดการสินค้า
  const [formVisible, setFormVisible] = useState(false);
  const [formMode, setFormMode] = useState("add");
  const [formData, setFormData] = useState(null);

  const ITEMS_PER_PAGE = 20;

  // ค้นหาข้อมูล
  const fetchProducts = async (pageNumber, search = "") => {
    setIsLoading(true);
    setErrorMessage("");
    try {
      const { data } = await api.get("/Product/Search", {
        params: {
          pageNumber: pageNumber,
          pageSize: ITEMS_PER_PAGE,
          keyword: search,
        },
      });

      setProducts(data.data || []);
      setTotalPages(data.totalPages || 1);
      settotalRecords(data.totalRecords || 0);
    } catch (error) {
      setErrorMessage("ไม่สามารถโหลดข้อมูลสินค้าได้");
      console.error("Error fetching products:", error);
    } finally {
      setIsLoading(false);
    }
  };

  useEffect(() => {
    fetchProducts(page, searchTerm.trim());
  }, [page, searchTerm]);

  const handleResetSearch = () => {
    setSearchTerm("");
    setPage(1);
  };

  const handleDelete = async () => {
    try {
      await api.delete(`/Product/Delete/?id=${deleteId}`);
      setProducts((prev) => prev.filter((product) => product.id !== deleteId));
    } catch (error) {
      console.error("Error deleting product:", error);
    } finally {
      setPopupVisible(false);
      setDeleteId(null);
      setDeleteName(null);
      handleResetSearch();
    }
  };

  const confirmDelete = (id, name) => {
    setDeleteId(id);
    setDeleteName(name);
    setPopupVisible(true);
  };

  const handlePageChange = (newPage) => {
    if (newPage >= 1 && newPage <= totalPages) setPage(newPage);
  };

  const handleClosForm = (Boolean) => {
    setFormVisible(Boolean);
  };

  const handleCreateProduct = async (formData) => {
    try {
      const response = await fetch(api, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(formData),
      });

      if (!response.ok) {
        throw new Error(`Error: ${response.statusText}`);
      }

      const result = await response.json();
      console.log("Product Created:", result);
      alert("Product created successfully!");
    } catch (error) {
      console.error("Error creating product:", error);
      alert("Failed to create product. Please try again.");
    } finally {
      setPopupVisible(false);
    }
  };

  const handleEditProduct = async (formData) => {
    try {
      const response = await fetch(`${api}/${formData.code}`, {
        method: "PUT",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(formData),
      });

      if (!response.ok) {
        throw new Error(`Error: ${response.statusText}`);
      }

      const result = await response.json();
      console.log("Product Updated:", result);
      alert("Product updated successfully!");
    } catch (error) {
      console.error("Error updating product:", error);
      alert("Failed to update product. Please try again.");
    } finally {
      setPopupVisible(false);
    }
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
          {t.back}
        </Link>
      </nav>
      <nav aria-label="Breadcrumb" className="hidden sm:flex">
        <ol role="list" className="flex items-center space-x-4">
          <li>
            <div className="flex">
              <Link
                to="/product"
                className="text-sm font-medium text-gray-500 hover:text-gray-700"
              >
                {t.mainpage}
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
                {t.title}
              </span>
            </div>
          </li>
        </ol>
      </nav>

      <div className="mt-2 md:flex md:items-center md:justify-between">
        <div className="min-w-0 flex-1">
          <h2 className="text-2xl/7 font-bold text-gray-900 sm:truncate sm:text-3xl sm:tracking-tight">
            {t.title}
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
                placeholder={t.keyword}
                value={searchTerm}
                onChange={(e) => setSearchTerm(e.target.value)}
                className="block w-full rounded-none rounded-l-md border-0 py-1.5 pl-10 text-gray-900 ring-1 ring-inset ring-gray-300 placeholder:text-gray-400 focus:ring-2 focus:ring-inset focus:ring-blue-400 sm:text-sm/6"
              />
            </div>
            <button
              onClick={handleResetSearch}
              className="relative -ml-px inline-flex items-center gap-x-1.5 rounded-r-md px-3 py-2 text-sm font-semibold text-gray-900 ring-1 ring-inset ring-gray-300 hover:bg-gray-50"
            >
              {t.reset}
            </button>
            <button
              className="ml-4 rounded-md bg-blue-600 px-3 py-2 text-sm font-semibold text-white shadow-sm hover:bg-blue-500 focus-visible:outline focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-blue-600"
              onClick={() => {
                setFormMode("add");
                setFormVisible(true);
              }}
            >
              {t.create}
            </button>
          </div>
        </div>
      </div>

      {isLoading ? (
        <p className="text-center">{t.loading}</p>
      ) : errorMessage ? (
        <p className="text-center text-red-500">{errorMessage}</p>
      ) : products.length === 0 ? (
        <div className="mt-2 ring-1 ring-gray-300 sm:mx-0 sm:rounded-sm">
          <table className="min-w-full divide-y  divide-gray-300">
            <thead>
              <tr className="divide-x divide-gray-200">
                <th className="py-1.5 pl-4 pr-4 text-left text-sm font-semibold text-gray-900">
                  {t.code}
                </th>
                <th className="px-4 py-1 text-left text-sm font-semibold text-gray-900">
                  {t.name1}
                </th>
                <th className="px-4 py-1 text-center text-sm font-semibold text-gray-900">
                  {t.name2}
                </th>
                <th className="px-4 py-1 text-right text-sm font-semibold text-gray-900">
                  {t.price}
                </th>
                <th className="px-4 py-1 text-center text-sm font-semibold text-gray-900">
                  {t.active}
                </th>
                <th className="px-4 py-1 text-center text-sm font-semibold text-gray-900">
                  {t.createdate}
                </th>
                <th className="px-4 py-1 text-center text-sm font-semibold text-gray-900">
                  {t.createby}
                </th>
                <th className="px-4 py-1 text-center text-sm font-semibold text-gray-900">
                  {t.delete}
                </th>
                <th className="px-4 py-1 text-center text-sm font-semibold text-gray-900">
                  {t.update}
                </th>
              </tr>
            </thead>
            <tbody className="divide-y divide-gray-200 bg-white">
              <tr className="divide-x divide-gray-200">
                <td
                  className="py-1.5 pl-4 pr-4 text-center text-sm"
                  colSpan={9}
                >
                  <span className="text-center">{t.nodata}</span>
                </td>
              </tr>
            </tbody>
          </table>
        </div>
      ) : (
        <div className="mt-2 ring-1 ring-gray-300 sm:mx-0 sm:rounded-sm">
          <table className="min-w-full divide-y  divide-gray-300">
            <thead>
              <tr className="divide-x divide-gray-200">
                <th className="py-1.5 pl-4 pr-4 text-left text-sm font-semibold text-gray-900">
                  {t.code}
                </th>
                <th className="px-4 py-1 text-left text-sm font-semibold text-gray-900">
                  {t.name1}
                </th>
                <th className="px-4 py-1 text-center text-sm font-semibold text-gray-900">
                  {t.name2}
                </th>
                <th className="px-4 py-1 text-right text-sm font-semibold text-gray-900">
                  {t.price}
                </th>
                <th className="px-4 py-1 text-center text-sm font-semibold text-gray-900">
                  {t.active}
                </th>
                <th className="px-4 py-1 text-center text-sm font-semibold text-gray-900">
                  {t.createdate}
                </th>
                <th className="px-4 py-1 text-center text-sm font-semibold text-gray-900">
                  {t.createby}
                </th>
                <th className="px-4 py-1 text-center text-sm font-semibold text-gray-900">
                  {t.delete}
                </th>
                <th className="px-4 py-1 text-center text-sm font-semibold text-gray-900">
                  {t.update}
                </th>
              </tr>
            </thead>
            <tbody className="divide-y divide-gray-200 bg-white">
              {products.map((product) => (
                <tr key={product.id} className="divide-x divide-gray-200">
                  <td className="whitespace-nowrap p-1 text-sm font-medium text-gray-900 ">
                    {product.code}
                  </td>
                  <td className="whitespace-nowrap p-1 text-sm text-gray-600">
                    {product.name1}
                  </td>
                  <td className="whitespace-nowrap p-1 text-sm text-gray-600">
                    {product.name2}
                  </td>
                  <td className="whitespace-nowrap p-1 text-sm text-right text-gray-600">
                    {formatNumber(product.stdprice)}
                  </td>
                  <td className="whitespace-nowrap p-1 text-sm text-center text-gray-600">
                    {product.active === "Y" ? "Active" : "Inactive"}
                  </td>
                  <td className="whitespace-nowrap p-1 text-sm text-center text-gray-600">
                    {formatDateTime(product.createatutc)}
                  </td>
                  <td className="whitespace-nowrap p-1 text-sm text-center text-gray-600">
                    {product.createby}
                  </td>
                  <td className="whitespace-nowrap p-1 text-sm text-center text-gray-600">
                    <button
                      className="text-red-500 font-semibold"
                      onClick={() =>
                        confirmDelete(
                          product.id,
                          product.code + " : " + product.name1
                        )
                      }
                    >
                      <span>{t.delete}</span>
                    </button>
                  </td>
                  <td className="whitespace-nowrap p-1 text-sm text-center text-gray-600">
                    <button
                      className="text-blue-500 font-semibold"
                      onClick={() => {
                        setFormMode("edit");
                        setFormData({
                          id: product.id,
                          name1: product.name1,
                          name2: product.name2,
                          code: product.code,
                          productgroupid: product.productgroupid,
                          prodtype: product.prodtype,
                          vattype: product.vattype,
                          stdprice: product.stdprice,
                        });
                        setFormVisible(true);
                      }}
                    >
                      <span>{t.update}</span>
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
          {t.firstpage}
        </button>
        <button
          className="px-4 py-2 text-sm font-semibold bg-gray-200 rounded-lg hover:bg-gray-300"
          disabled={page === 1 || isLoading}
          onClick={() => handlePageChange(page - 1)}
        >
          {t.prevpage}
        </button>
        <span className="mx-4">
          {t.page} {page} {t.of} {totalPages}
        </span>
        <button
          className="mr-2 px-4 py-2 text-sm font-semibold bg-gray-200 rounded-lg hover:bg-gray-300"
          disabled={page === totalPages || isLoading}
          onClick={() => handlePageChange(page + 1)}
        >
          {t.nextpage}
        </button>
        <button
          className="px-4 py-2 text-sm font-semibold bg-gray-200 rounded-lg hover:bg-gray-300"
          disabled={page === totalPages || isLoading}
          onClick={() => handlePageChange(totalPages)}
        >
          {t.lastpage}
        </button>
        <span className="mx-4"> {t.number} {totalRecords} {t.record}</span>
      </div>

      {popupVisible && (
        <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center">
          <div className="bg-white p-6 rounded-lg shadow-lg">
            <h2 className="text-2xl font-bold mb-4">{t.confirmtitle}</h2>
            <p>{t.confirm_msg} {deleteName} ?</p>
            <div className="flex justify-end space-x-4 mt-4">
              <button
                className="bg-gray-300 px-4 py-2 rounded-lg hover:bg-gray-400"
                onClick={() => setPopupVisible(false)}
              >
                {t.cancel}
              </button>
              <button
                className="bg-red-500 text-white px-4 py-2 rounded-lg hover:bg-red-600"
                onClick={handleDelete}
              >
                {t.delete}
              </button>
            </div>
          </div>
        </div>
      )}

      {formVisible && (
        <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center">
          <div className="bg-white p-6 rounded-lg shadow-lg w-full max-w-lg">
            <h2 className="text-2xl font-bold mb-4">
              {formMode === "add" ? t.popupformtitle_addnew : t.popupformtitle_update}
            </h2>
            {formMode === "add" ? (
              <ProductForm
                initialData={null}
                onSubmit={handleCreateProduct}
                onCancel={handleClosForm}
              />
            ) : (
              <ProductForm
                initialData={formData}
                onSubmit={handleEditProduct}
                onCancel={handleClosForm}
              />
            )}
          </div>
        </div>
      )}
    </div>
  );
};

export default Product;
