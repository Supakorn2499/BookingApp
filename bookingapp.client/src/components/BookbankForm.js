import React, { useEffect, useState } from "react";
import AsyncSelect from "react-select/async";
import api from "../pages/apiConfig";
import { useOutletContext } from "react-router-dom";

const BookBankForm = ({ initialData, onSubmit, onCancel }) => {
  const [formData, setFormData] = useState({
    id: 0,
    companyid: 1,
    name1: "",
    name2: "",
    code: "",
    active: "Y",
    bankid: 1,
    bankbranchid: 1,
    bookno: "",
    balanceamount: 0,
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
      bank: "ธนาคาร",
      bankbranch: "สาขาธนาคาร",
      bookno: "เลขที่บัญชี",
      balanceamount: "จำนวนเงิน",
      status: "สถานะ",
    },
    en: {
      code: "Code",
      name1: "Name (Thail)",
      name2: "Name (English)",
      cancel: "Cancel",
      save: "Save",
      select: "Select...",
      active: "Active",
      inactive: "Inactive",
      bank: "Bank",
      bankbranch: "Bank Branch",
      bookno: "Account No",
      balanceamount: "Amount",
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

          const bankid = initialData.bankid
            ? await fetchOptionId("bank/GetById?id=", initialData.bankid)
            : null;

          const bankbranchid = initialData.bankbranchid
            ? await fetchOptionId(
                "BankBranch/GetById?id=",
                initialData.bankbranchid
              )
            : null;

          setFormData({
            id: initialData.id,
            code: initialData.code,
            companyid: 1,
            name1: initialData.name1,
            name2: initialData.name2,
            active: selectedStatus,
            bankid: bankid,
            bankbranchid: bankbranchid,
            bookno: initialData.bookno,
            balanceamount: initialData.balanceamount,
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
      bankid: formData.bankid?.value || null,
      bankbranchid: formData.bankbranchid?.value || null,
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
      {/* Dropdown สำหรับ bank */}
      <div>
        <label className="block p-1 text-sm font-medium text-gray-700">
          {translation.bank}
        </label>
        <AsyncSelect
          placeholder={translation.select}
          cacheOptions
          loadOptions={(inputValue) => loadOptions("bank", inputValue)}
          defaultOptions
          value={formData.bankid}
          onChange={(selectedOption) =>
            handleSelectChange("bankid", selectedOption)
          }
        />
      </div>
      {/* Dropdown สำหรับ bank branch */}
      <div>
        <label className="block p-1 text-sm font-medium text-gray-700">
          {translation.bankbranch}
        </label>
        <AsyncSelect
          placeholder={translation.select}
          cacheOptions
          loadOptions={(inputValue) => loadOptions("bankbranch", inputValue)}
          defaultOptions
          value={formData.bankbranchid}
          onChange={(selectedOption) =>
            handleSelectChange("bankbranchid", selectedOption)
          }
        />
      </div>
      <div>
        <label className="block p-1 text-sm font-medium text-gray-700">
          {translation.bookno}
        </label>
        <input
          type="text"
          name="bookno"
          value={formData.bookno}
          onChange={handleChange}
          className="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm"
        />
      </div>
      <div>
        <label className="block p-1 text-sm font-medium text-gray-700">
          {translation.balanceamount}
        </label>
        <input
          type="number"
          name="balanceamount"
          value={formData.balanceamount}
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

export default BookBankForm;
