import React, { useEffect, useState } from "react";
import AsyncSelect from "react-select/async";
import api from "../pages/apiConfig";
import { useOutletContext } from "react-router-dom";

const PaytypeForm = ({ initialData, onSubmit, onCancel }) => {
  const [formData, setFormData] = useState({
    id: 0,
    companyid: 1,
    name1: "",
    name2: "",
    code: "",
    active: "Y",
    isdeposit: null,
    iswithdraw: null,
    group: null,
    inout: "",
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
      remark: "หมายเหตุ",
      active: "เปิดใช้งาน",
      inactive: "ปิดใช้งาน",
      deposit: "ฝากเงิน",
      withdraw: "ถอนเงิน",
      group: "กลุ่ม",
      inout: "เข้า/ออก",
      status: "สถานะ",
      yes: "ใช่",
      no: "ไม่ใช่",
      in: "เข้า",
      out: "ออก",
    },
    en: {
      code: "Code",
      name1: "Name (Thail)",
      name2: "Name (English)",
      cancel: "Cancel",
      save: "Save",
      select: "Select...",
      active: "Active",
      inactive: "InActive",
      deposit: "Deposit",
      withdraw: "Withdraw",
      group: "Group",
      inout: "In/Out",
      status: "Status",
      yes: "Yes",
      no: "No",
      in: "IN",
      out: "OUT",
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

  const INOUT = [
    {
      id: 0,
      code: "-",
      name: "-",
    },
    {
      id: 1,
      code: "I",
      name: translation.in,
    },
    {
      id: 2,
      code: "O",
      name: translation.out,
    },
  ];

  // State สำหรับสถานะปัจจุบัน
  const [selectedStatus, setSelectedStatus] = useState(Status[0].code);
  const [selectedINOUT, setSelectedINOUT] = useState(INOUT[0].code);

  useEffect(() => {
    const fetchDefaultValues = async () => {
      if (initialData) {
        try {
          // อัปเดต state ด้วยข้อมูลที่ดึงมา
          if (initialData && initialData.active) {
            setSelectedStatus(initialData.active);
          }
          if (initialData && initialData.inout) {
            setSelectedINOUT(initialData.inout);
          }
          setFormData({
            id: initialData.id,
            code: initialData.code,
            companyid: 1,
            name1: initialData.name1,
            name2: initialData.name2,
            active: selectedStatus,
            isdeposit: initialData.isdeposit,
            iswithdraw: initialData.iswithdraw,
            group: initialData.group,
            inout: selectedINOUT,
          });
        } catch (error) {
          console.error("Error fetching default values:", error);
        }
      }
    };

    fetchDefaultValues();
  }, [initialData, selectedStatus, selectedINOUT]);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData((prev) => ({ ...prev, [name]: value }));
  };

  const handleActiveChange = (e) => {
    const newStatus = e.target.value;
    setFormData((prev) => ({ ...prev, active: newStatus }));
  };

  const handleINOUTChange = (e) => {
    const newINOUT = e.target.value;
    setFormData((prev) => ({ ...prev, inout: newINOUT }));
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    const outputData = {
      ...formData,
    };
    onSubmit(outputData);
  };

  // ปุ่มยกเลิก
  const handleCancel = (e) => {
    onCancel(false);
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

      {/* Dropdown สำหรับ INOUT */}
      <div>
        <label className="block p-1 text-sm font-medium text-gray-700">
          {translation.inout}
        </label>
        <select
          id="inout"
          name="inout"
          value={formData.inout}
          onChange={handleINOUTChange}
          className="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm"
        >
          {INOUT.map((item) => (
            <option key={item.id} value={item.code}>
              {item.name}
            </option>
          ))}
        </select>
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

export default PaytypeForm;