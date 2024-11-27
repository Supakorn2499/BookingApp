import React, { useEffect, useState } from "react";
import AsyncSelect from "react-select/async";
import api from "../pages/apiConfig";

const ProductForm = ({ initialData, onSubmit, onCancel }) => {
  const [formData, setFormData] = useState({
    code: "",
    name1: "",
    name2: "",
    prodtype: null,
    productgroupid: null,
    vattype: null,
    remark1: "",
  });

  useEffect(() => {
    if (initialData) {
      setFormData({
        code: initialData.code || "",
        name1: initialData.name1 || "",
        name2: initialData.name2 || "",
        prodtype: initialData.prodtype
          ? { value: initialData.prodtype, label: initialData.prodtype }
          : null,
        productgroupid: initialData.productgroupid
          ? {
              value: initialData.productgroupid,
              label: initialData.productgroupid,
            }
          : null,
        vattype: initialData.vattype
          ? { value: initialData.vattype, label: initialData.vattype }
          : null,
        remark1: initialData.remark1 || "",
      });
    }
  }, [initialData]);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData((prev) => ({ ...prev, [name]: value }));
  };

  const handleSelectChange = (name, selectedOption) => {
    setFormData((prev) => ({ ...prev, [name]: selectedOption }));
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    const outputData = {
      ...formData,
      prodtype: formData.prodtype?.value || null,
      productgroupid: formData.productgroupid?.value || null,
      vattype: formData.vattype?.value || null,
    };
    onSubmit(outputData);
  };

  const handleCancel = (e) => {
    onCancel(false);
  };
  const isEditing = Boolean(initialData);

  // ฟังก์ชันสำหรับดึงข้อมูลแบบ Async
  const loadOptions = async (endpoint, inputValue) => {
    try {
      const response = await api.get(`/${endpoint}/Search`, {
        params: {
          pageNumber: 1,
          pageSize: 20,
          keyword: inputValue,
        },
      });
      return response.data.data.map((item) => ({
        value: item.code,
        label: item.name1,
      }));
    } catch (error) {
      console.error(`Error fetching ${endpoint} options:`, error);
      return [];
    }
  };

  return (
    <form onSubmit={handleSubmit}>
      <div>
        <label className="block p-1 text-sm font-medium text-gray-700">
          รหัสสินค้า
        </label>
        <input
          type="text"
          name="code"
          value={formData.code}
          onChange={handleChange}
          className="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm"
        />
      </div>

      <div>
        <label className="block p-1 text-sm font-medium text-gray-700">
          ชื่อสินค้าไทย
        </label>
        <input
          type="text"
          name="name1"
          value={formData.name1}
          onChange={handleChange}
          className="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm"
        />
      </div>

      <div>
        <label className="block p-1 text-sm font-medium text-gray-700">
          ชื่อสินค้าอังกฤษ
        </label>
        <input
          type="text"
          name="name2"
          value={formData.name2}
          onChange={handleChange}
          className="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm"
        />
      </div>
      <div>
        <label className="block p-1 text-sm font-medium text-gray-700">
          ราคา
        </label>
        <input
          type="number"
          name="name2"
          value={formData.stdprice}
          onChange={handleChange}
          className="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm"
        />
      </div>
      {/* Dropdown สำหรับ ProdType */}
      <div>
        <label className="block p-1 text-sm font-medium text-gray-700">
          ประเภทสินค้า
        </label>
        <AsyncSelect
          placeholder="เลือก"
          cacheOptions
          loadOptions={(inputValue) => loadOptions("ProdType", inputValue)}
          defaultOptions
          value={formData.prodtype}
          onChange={(selectedOption) =>
            handleSelectChange("prodtype", selectedOption)
          }
        />
      </div>

      {/* Dropdown สำหรับ ProductGroup */}
      <div>
        <label className="block p-1 text-sm font-medium text-gray-700">
          กลุ่มสินค้า
        </label>
        <AsyncSelect
          placeholder="เลือก"
          cacheOptions
          loadOptions={(inputValue) => loadOptions("ProdGroup", inputValue)}
          defaultOptions
          value={formData.productgroupid}
          onChange={(selectedOption) =>
            handleSelectChange("productgroupid", selectedOption)
          }
        />
      </div>

      {/* Dropdown สำหรับ VatType */}
      <div>
        <label className="block p-1 text-sm font-medium text-gray-700">
          ประเภทภาษี
        </label>
        <AsyncSelect
          placeholder="เลือก"
          cacheOptions
          loadOptions={(inputValue) => loadOptions("Vattype", inputValue)}
          defaultOptions
          value={formData.vattype}
          onChange={(selectedOption) =>
            handleSelectChange("vattype", selectedOption)
          }
        />
      </div>

      <div>
        <label className="block p-1 text-sm font-medium text-gray-700">
          หมายเหตุ
        </label>
        <textarea
          name="remark1"
          value={formData.remark1}
          onChange={handleChange}
          className="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm"
        ></textarea>
      </div>

      <div className="flex justify-end mt-4">
        <button
          type="button"
          className="bg-gray-300 px-4 py-2 rounded-lg hover:bg-gray-400  mr-4"
          onClick={() => handleCancel(false)}
        >
          ยกเลิก
        </button>
        <button
          type="submit"
          className="bg-blue-500 text-white px-4 py-2 rounded-lg hover:bg-blue-600"
        >
          บันทึก
        </button>
      </div>
    </form>
  );
};

export default ProductForm;
