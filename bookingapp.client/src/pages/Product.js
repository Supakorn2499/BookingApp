import { useState, useEffect } from "react";
import api from "./apiConfig.js";
import { BarsArrowUpIcon } from "@heroicons/react/20/solid";
const Product = () => {
  const [products, setProducts] = useState([]);
  const [layout, setLayout] = useState("table");
  const [searchTerm, setSearchTerm] = useState("");
  const [page, setPage] = useState(1);
  const [totalPages, setTotalPages] = useState(0);
  const [isLoading, setIsLoading] = useState(false);
  const [popupVisible, setPopupVisible] = useState(false);
  const [deleteId, setDeleteId] = useState(null);
  const [errorMessage, setErrorMessage] = useState("");

  // ฟอร์มจัดการสินค้า
  const [formVisible, setFormVisible] = useState(false);
  const [formMode, setFormMode] = useState("add"); // "add" หรือ "edit"
  const [formData, setFormData] = useState({
    name: "",
    code: "",
    sellingPrice: 0,
  });
  const [editId, setEditId] = useState(null);

  const ITEMS_PER_PAGE = 20;

  // Fetch products
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
    } catch (error) {
      setErrorMessage("ไม่สามารถโหลดข้อมูลสินค้าได้");
      console.error("Error fetching products:", error);
    } finally {
      setIsLoading(false);
    }
  };

  const handleSearch = () => {
    setPage(1);
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
      await api.delete(`/products/deleteProduct/${deleteId}`);
      setProducts((prev) => prev.filter((product) => product._id !== deleteId));
      //alert("ลบสินค้าสำเร็จ");
    } catch (error) {
      console.error("Error deleting product:", error);
      //alert("ลบสินค้าล้มเหลว");
    } finally {
      setPopupVisible(false);
      setDeleteId(null);
    }
  };

  const confirmDelete = (id) => {
    setDeleteId(id);
    setPopupVisible(true);
  };

  const handlePageChange = (newPage) => {
    if (newPage >= 1 && newPage <= totalPages) setPage(newPage);
  };

  const handleFormSubmit = async (e) => {
    e.preventDefault();
    try {
      if (formMode === "add") {
        // เพิ่มสินค้าใหม่
        const { data } = await api.post("/products/createProduct", formData);
        setProducts((prev) => [...prev, data.product]);
        //alert("เพิ่มสินค้าสำเร็จ");
      } else {
        // แก้ไขสินค้า
        const { data } = await api.put(
          `/products/updateProduct/${editId}`,
          formData
        );
        setProducts((prev) =>
          prev.map((product) =>
            product._id === editId ? data.product : product
          )
        );
        //alert("แก้ไขสินค้าสำเร็จ");
      }
      setFormVisible(false);
      setFormData({ name: "", code: "", sellingPrice: 0 });
    } catch (error) {
      console.error("Error saving product:", error);
      //alert("บันทึกข้อมูลสินค้าไม่สำเร็จ");
    }
  };

  return (
    <div className="px-4 sm:px-6 lg:px-8">
      <div className="sm:flex sm:items-centerm mb-2">
        <div className="sm:flex-auto">
          <h1 className="text-base font-semibold text-gray-900">
            ข้อมูลสินค้าทั้งหมด
          </h1>
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
                placeholder="รหัสสินค้า/ชื่อสินค้า"
                value={searchTerm}
                onChange={(e) => setSearchTerm(e.target.value)}
                className="block w-full rounded-none rounded-l-md border-0 py-1.5 pl-10 text-gray-900 ring-1 ring-inset ring-gray-300 placeholder:text-gray-400 focus:ring-2 focus:ring-inset focus:ring-blue-400 sm:text-sm/6"
              />
            </div>
            <button
              onClick={handleResetSearch}
              className="relative -ml-px inline-flex items-center gap-x-1.5 rounded-r-md px-3 py-2 text-sm font-semibold text-gray-900 ring-1 ring-inset ring-gray-300 hover:bg-gray-50"
            >
              รีเซ็ต
            </button>
            <button
              className="ml-4 rounded-md bg-blue-600 px-3 py-2 text-sm font-semibold text-white shadow-sm hover:bg-blue-500 focus-visible:outline focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-blue-600"
              onClick={() => {
                setFormMode("add");
                setFormData({ name: "", code: "", sellingPrice: 0 });
                setFormVisible(true);
              }}
            >
              เพิ่มสินค้าใหม่
            </button>
          </div>
        </div>
      </div>

      {isLoading ? (
        <p className="text-center">กำลังโหลดข้อมูล...</p>
      ) : errorMessage ? (
        <p className="text-center text-red-500">{errorMessage}</p>
      ) : products.length === 0 ? (
        <p className="text-center">ไม่มีข้อมูลสินค้า</p>
      ) : layout === "card" ? (
        <div className="grid grid-cols-4 gap-4">
          {products.map((product) => (
            <div key={product._id} className="border rounded-lg p-4 shadow">
              <img
                src={product.image || "https://via.placeholder.com/150"}
                alt={product.name}
                className="w-full h-40 object-cover mb-2 rounded"
              />
              <h2 className="text-lg font-semibold">{product.name}</h2>
              <p>ราคา: {product.sellingPrice} บาท</p>
              <div className="flex justify-between mt-4">
                <button
                  className="flex items-center space-x-2 text-blue-500"
                  onClick={() => {
                    setFormMode("edit");
                    setEditId(product._id);
                    setFormData({
                      name: product.name,
                      code: product.code,
                      sellingPrice: product.sellingPrice,
                      costPrice: product.costPrice,
                    });
                    setFormVisible(true);
                  }}
                >
                  <span>แก้ไข</span>
                </button>
                <button
                  className="flex items-center space-x-2 text-red-500"
                  onClick={() => confirmDelete(product._id)}
                >
                  <span>ลบ</span>
                </button>
              </div>
            </div>
          ))}
        </div>
      ) : (
        <div className="mt-2 ring-1 ring-gray-300 sm:mx-0 sm:rounded-sm">
          <table className="min-w-full divide-y  divide-gray-300">
            <thead>
              <tr className="divide-x divide-gray-200">
                <th className="py-1.5 pl-4 pr-4 text-left text-sm font-semibold text-gray-900">
                  รหัส
                </th>
                <th className="px-4 py-1 text-left text-sm font-semibold text-gray-900">
                  ชื่อสินค้า
                </th>
                <th className="px-4 py-1 text-center text-sm font-semibold text-gray-900">
                  ต้นทุน
                </th>
                <th className="px-4 py-1 text-center text-sm font-semibold text-gray-900">
                  ราคา
                </th>
                <th className="px-4 py-1 text-center text-sm font-semibold text-gray-900">
                  การจัดการ
                </th>
              </tr>
            </thead>
            <tbody className="divide-y divide-gray-200 bg-white">
              {products.map((product) => (
                <tr key={product._id} className="divide-x divide-gray-200">
                  <td className="whitespace-nowrap py-1 pl-4 pr-4 text-sm font-medium text-gray-900 ">
                    {product.code}
                  </td>
                  <td className="whitespace-nowrap p-1 text-sm text-gray-600">
                    {product.name1}
                  </td>
                  <td className="whitespace-nowrap p-1 text-sm text-gray-600">
                    {product.costPrice}
                  </td>
                  <td className="whitespace-nowrap p-1 text-sm text-gray-600">
                    {product.sellingPrice}
                  </td>
                  <td className="whitespace-nowrap p-1 text-sm text-gray-600"></td>
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
          หน้าแรก
        </button>
        <button
          className="px-4 py-2 text-sm font-semibold bg-gray-200 rounded-lg hover:bg-gray-300"
          disabled={page === 1 || isLoading}
          onClick={() => handlePageChange(page - 1)}
        >
          ก่อนหน้า
        </button>
        <span className="mx-4">
          หน้า {page} จาก {totalPages}
        </span>
        <button
          className="mr-2 px-4 py-2 text-sm font-semibold bg-gray-200 rounded-lg hover:bg-gray-300"
          disabled={page === totalPages || isLoading}
          onClick={() => handlePageChange(page + 1)}
        >
          ถัดไป
        </button>
        <button
          className="px-4 py-2 text-sm font-semibold bg-gray-200 rounded-lg hover:bg-gray-300"
          disabled={page === totalPages || isLoading}
          onClick={() => handlePageChange(totalPages)}
        >
          หน้าสุดท้าย
        </button>
      </div>

      {popupVisible && (
        <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center">
          <div className="bg-white p-6 rounded-lg shadow-lg">
            <h2 className="text-2xl font-bold mb-4">ยืนยันการลบ</h2>
            <p>คุณต้องการลบสินค้านี้หรือไม่?</p>
            <div className="flex justify-end space-x-4 mt-4">
              <button
                className="bg-gray-300 px-4 py-2 rounded-lg hover:bg-gray-400"
                onClick={() => setPopupVisible(false)}
              >
                ยกเลิก
              </button>
              <button
                className="bg-red-500 text-white px-4 py-2 rounded-lg hover:bg-red-600"
                onClick={handleDelete}
              >
                ลบ
              </button>
            </div>
          </div>
        </div>
      )}

      {formVisible && (
        <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center">
          <div className="bg-white p-6 rounded-lg shadow-lg w-full max-w-lg">
            <h2 className="text-2xl font-bold mb-4">
              {formMode === "add" ? "เพิ่มสินค้าใหม่" : "แก้ไขสินค้า"}
            </h2>
            <form onSubmit={handleFormSubmit}>
              <div className="mb-4">
                <label className="block mb-2 font-semibold">ชื่อสินค้า</label>
                <input
                  type="text"
                  value={formData.name}
                  onChange={(e) =>
                    setFormData({ ...formData, name: e.target.value })
                  }
                  className="w-full px-4 py-2 border rounded-lg"
                  required
                />
              </div>
              <div className="mb-4">
                <label className="block mb-2 font-semibold">รหัสสินค้า</label>
                <input
                  type="text"
                  value={formData.code}
                  onChange={(e) =>
                    setFormData({ ...formData, code: e.target.value })
                  }
                  className="w-full px-4 py-2 border rounded-lg"
                  required
                />
              </div>
              <div className="mb-4">
                <label className="block mb-2 font-semibold">ราคาต้นทุน</label>
                <input
                  type="number"
                  value={formData.costPrice}
                  onChange={(e) =>
                    setFormData({
                      ...formData,
                      costPrice: parseFloat(e.target.value),
                    })
                  }
                  className="w-full px-4 py-2 border rounded-lg"
                  required
                />
              </div>
              <div className="mb-4">
                <label className="block mb-2 font-semibold">ราคา</label>
                <input
                  type="number"
                  value={formData.sellingPrice}
                  onChange={(e) =>
                    setFormData({
                      ...formData,
                      sellingPrice: parseFloat(e.target.value),
                    })
                  }
                  className="w-full px-4 py-2 border rounded-lg"
                  required
                />
              </div>
              <div className="flex justify-end space-x-4">
                <button
                  type="button"
                  className="bg-gray-300 px-4 py-2 rounded-lg hover:bg-gray-400"
                  onClick={() => setFormVisible(false)}
                >
                  ยกเลิก
                </button>
                <button
                  type="submit"
                  className="bg-blue-500 text-white px-4 py-2 rounded-lg hover:bg-blue-600"
                >
                  {formMode === "add" ? "เพิ่ม" : "บันทึก"}
                </button>
              </div>
            </form>
          </div>
        </div>
      )}
    </div>
  );
};

export default Product;
