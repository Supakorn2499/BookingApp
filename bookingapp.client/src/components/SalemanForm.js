import React, { useEffect, useState } from "react";
import AsyncSelect from "react-select/async";
import api from "../pages/apiConfig";
import { useOutletContext } from "react-router-dom";

const SalemanForm = ({ initialData, onSubmit, onCancel }) => {
  const [formData, setFormData] = useState({
    id: 0,
    prefix_th: null,
    prefix_en: null,
    name1: null,
    name2: null,
    code: null,
    position: null,
    mobile: null,
    email: null,
    commission: null,
    active: "Y",
    sale_team_id: 1,
  });
  const { language } = useOutletContext();
  // แปลภาษา
  const translations = {
    th: {
      code: "รหัส",
      name1: "ชื่อไทย",
      name2: "ชื่ออังกฤษ",
      cancel: "ยกเลิก",
      save: "บันทึก",
      select: "เลือก...",
      mobile: "เบอร์โทร",
      email: "อีเมล์",
      position: "ตำแหน่ง",
      commission: "คอมมิชชั่น",
      prefix_th: "คำนำหน้า (TH)",
      prefix_en: "คำนำหน้า (EN)",
      remark: "หมายเหตุ",
      active: "เปิดใช้งาน",
      inactive: "ปิดใช้งาน",
      saleteam: "ทีมขาย",
      status: "สถานะ",
    },
    en: {
      code: "Code",
      name1: "Name (Thail)",
      name2: "Name (English)",
      cancel: "Cancel",
      save: "Save",
      select: "Select...",
      mobile: "Mobile",
      email: "Email",
      position: "Position",
      commission: "Commission",
      prefix_th: "Prefix (TH)",
      prefix_en: "Prefix (EN)",
      active: "Active",
      inactive: "Inactive",
      saleteam: "SaleTeam",
      status: "Status",
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

  useEffect(() => {
    const fetchDefaultValues = async () => {
      if (initialData) {
        try {
          // อัปเดต state ด้วยข้อมูลที่ดึงมา
          const fetchOptionId = async (endpoint, id) => {
            const response = await api.get(`/${endpoint}${id}`);

            return {
              value: response.data.id,
              label: response.data.name1,
            };
          };

          if (initialData && initialData.active) {
            setSelectedStatus(initialData.active);
          }

          const sale_team_id = initialData.sale_team_id
            ? await fetchOptionId(
                "SaleTeam/GetById?id=",
                initialData.sale_team_id
              )
            : null;

          setFormData({
            id: initialData.id,
            code: initialData.code,
            companyid: 1,
            name1: initialData.name1,
            name2: initialData.name2,
            active: selectedStatus,
            position: initialData.position,
            mobile: initialData.mobile,
            email: initialData.email,
            commission: initialData.commission,
            sale_team_id: sale_team_id,
            prefix_th: initialData.prefix_th,
            prefix_en: initialData.prefix_en,
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
      sale_team_id: formData.sale_team_id?.value || null,
    };
    onSubmit(outputData);
  };

  // ปุ่มยกเลิก
  const handleCancel = (e) => {
    onCancel(false);
  };
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

  const isEditing = Boolean(initialData);

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
          {translation.prefix_th}
        </label>
        <input
          type="text"
          name="prefix_th"
          value={formData.prefix_th}
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
          {translation.prefix_en}
        </label>
        <input
          type="text"
          name="prefix_en"
          value={formData.prefix_en}
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
      {/* Dropdown สำหรับ SaleTeam */}
      <div>
        <label className="block p-1 text-sm font-medium text-gray-700">
          {translation.saleteam}
        </label>
        <AsyncSelect
          placeholder={translation.select}
          cacheOptions
          loadOptions={(inputValue) => loadOptions("SaleTeam", inputValue)}
          defaultOptions
          value={formData.sale_team_id}
          onChange={(selectedOption) =>
            handleSelectChange("sale_team_id", selectedOption)
          }
        />
      </div>
      <div>
        <label className="block p-1 text-sm font-medium text-gray-700">
          {translation.position}
        </label>
        <input
          type="text"
          name="position"
          value={formData.position}
          onChange={handleChange}
          className="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm"
        />
      </div>
      <div>
        <label className="block p-1 text-sm font-medium text-gray-700">
          {translation.mobile}
        </label>
        <input
          type="text"
          name="mobile"
          value={formData.mobile}
          onChange={handleChange}
          className="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm"
        />
      </div>
      <div>
        <label className="block p-1 text-sm font-medium text-gray-700">
          {translation.email}
        </label>
        <input
          type="text"
          name="email"
          value={formData.email}
          onChange={handleChange}
          className="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm"
        />
      </div>
      <div>
        <label className="block p-1 text-sm font-medium text-gray-700">
          {translation.commission}
        </label>
        <input
          type="number"
          name="commission"
          value={formData.commission}
          onChange={handleChange}
          className="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm"
        />
      </div>
      {/* Dropdown สำหรับ Status */}
      <div>
        <label className="block p-1 text-sm font-medium text-gray-700">
          {translation.status}
        </label>
        <select
          id="active"
          name="active"
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

export default SalemanForm;
