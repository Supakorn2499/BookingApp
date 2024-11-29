import React, { startTransition, useEffect, useState } from "react";
import AsyncSelect from "react-select/async";
import api from "../pages/apiConfig";
import { useOutletContext } from "react-router-dom";

const ProductForm = ({ initialData, onSubmit, onCancel }) => {
  const [formData, setFormData] = useState({
    id: 0,
    companyid: 1,
    productgroupid: 1,
    name1: "",
    name2: "",
    code: "",
    prodtype: 1,
    vattype: 1,
    saleprice: 0,
    unitname: "",
    remark: "",
    active: "",
  });
  const { language } = useOutletContext();
  // แปลภาษา
  const translations = {
    th: {
      code: "รหัส",
      name1: "ชื่อไทย",
      name2: "ชื่ออังกฤษ",
      price: "ราคา",
      status: "สถานะ",
      unitname: "หน่วย",
      prodtype: "ประเภท",
      prodgroup: "กลุ่ม",
      vattype: "ประเภทภาษี",
      cancel: "ยกเลิก",
      save: "บันทึก",
      select: "เลือก...",
      remark: "หมายเหตุ",
      active: "เปิดใช้งาน",
      inactive: "ปิดใช้งาน",
    },
    en: {
      code: "Code",
      name1: "Name (Thail)",
      name2: "Name (English)",
      price: "Price",
      status: "Status",
      unitname: "UnitName",
      prodtype: "Product Type",
      prodgroup: "Product Group",
      vattype: "VAT",
      cancel: "Cancel",
      save: "Save",
      select: "Select...",
      remark: "Remark",
      active: "Active",
      inactive: "InActive",
    },
  };

  const currentLanguage = translations[language] ? language : "th";
  const translation = translations[currentLanguage];
  const Status = [
    {
      id: 1,
      code: "Y",
      name: translation.active,
    },
    {
      id: 2,
      code: "N",
      name: translation.inactive,
    },
  ];

  // State สำหรับสถานะปัจจุบัน
  const [selectedStatus, setSelectedStatus] = useState(Status[0].code);

  // อัปเดต selectedStatus เมื่อ initialData มีค่า active ใหม่
  useEffect(() => {
    if (initialData && initialData.active) {
      setSelectedStatus(initialData.active);
    }
  }, [initialData]);

  useEffect(() => {
    const fetchDefaultValues = async () => {
      if (initialData) {
        try {
          // ดึงข้อมูลชื่อจาก API เพื่อเติมใน dropdown
          const fetchOptionId = async (endpoint, id) => {
            const response = await api.get(`/${endpoint}${id}`);

            return {
              value: response.data.id,
              label: response.data.name1,
            };
          };

          // console.log("prodtype", initialData.prodtype);
          const prodtype = initialData.prodtype
            ? await fetchOptionId("ProdType/GetById?id=", initialData.prodtype)
            : null;

          // console.log("productgroupid", initialData.productgroupid);
          const productgroupid = initialData.productgroupid
            ? await fetchOptionId(
                "ProdGroup/GetById?id=",
                initialData.productgroupid
              )
            : null;
          // console.log("vattype", initialData.vattype);
          const vattype = initialData.vattype
            ? await fetchOptionId("Vattype/GetById?id=", initialData.vattype)
            : null;

          // อัปเดต state ด้วยข้อมูลที่ดึงมา

          setFormData({
            id: initialData.id,
            companyid: 1,
            productgroupid: productgroupid,
            name1: initialData.name1,
            name2: initialData.name2,
            code: initialData.code,
            prodtype: prodtype,
            vattype: vattype,
            active: selectedStatus,
            saleprice: initialData.saleprice,
            unitname: initialData.unitname,
            remark: initialData.remark,
          });
        } catch (error) {
          console.error("Error fetching default values:", error);
        }
      }
    };

    fetchDefaultValues();
  }, [initialData, selectedStatus]);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData((prev) => ({ ...prev, [name]: value }));
  };

  const handleSelectChange = (name, selectedOption) => {
    console.log(name, selectedOption);
    setFormData((prev) => ({ ...prev, [name]: selectedOption }));
  };

  const handleActiveChange = (e) => {
    const newStatus = e.target.value;
    setFormData((prev) => ({ ...prev, active: newStatus }));
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
        value: item.id,
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
          {translation.code}
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
          {translation.name1}
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
          {translation.name2}
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
          {translation.price}
        </label>
        <input
          type="number"
          name="saleprice"
          value={formData.saleprice}
          onChange={handleChange}
          className="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm"
        />
      </div>
      <div>
        <label className="block p-1 text-sm font-medium text-gray-700">
          {translation.unitname}
        </label>
        <input
          type="text"
          name="unitname"
          value={formData.unitname}
          onChange={handleChange}
          className="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm"
        />
      </div>
      {/* Dropdown สำหรับ ProdType */}
      <div>
        <label className="block p-1 text-sm font-medium text-gray-700">
          {translation.prodtype}
        </label>
        <AsyncSelect
          placeholder={translation.select}
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
          {translation.prodgroup}
        </label>
        <AsyncSelect
          placeholder={translation.select}
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
          {translation.vattype}
        </label>
        <AsyncSelect
          placeholder={translation.select}
          cacheOptions
          loadOptions={(inputValue) => loadOptions("Vattype", inputValue)}
          defaultOptions
          value={formData.vattype}
          onChange={(selectedOption) =>
            handleSelectChange("vattype", selectedOption)
          }
        />
      </div>
      {/* Dropdown สำหรับ Status */}
      <div>
        <label className="block p-1 text-sm font-medium text-gray-700">
          {translation.status}
        </label>
        <select
          id="status"
          name="status"
          value={formData.active}
          onChange={handleActiveChange}
          className="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm"
        >
          {Status.map((item) => (
            <option key={item.id} value={item.code}>
              {item.name}
            </option>
          ))}
        </select>
      </div>
      <div>
        <label className="block p-1 text-sm font-medium text-gray-700">
          {translation.remark}
        </label>
        <textarea
          name="remark"
          value={formData.remark}
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
          {translation.cancel}
        </button>
        <button
          type="submit"
          className="bg-blue-500 text-white px-4 py-2 rounded-lg hover:bg-blue-600"
        >
          {translation.save}
        </button>
      </div>
    </form>
  );
};

export default ProductForm;
