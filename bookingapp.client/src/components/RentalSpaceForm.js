import React, { useEffect, useState } from "react";
import AsyncSelect from "react-select/async";
import api from "../pages/apiConfig";
import { useOutletContext } from "react-router-dom";

const RentalSpaceForm = ({ initialData, onSubmit, onCancel }) => {
  const [formData, setFormData] = useState({
    id: 0,
    room_name: null,
    room_size: 1,
    monthly_price: 0,
    daily_price: 0,
    hourly_price: 0,
    zone_id: 1,
    building_id: 1,
    floor_id: 1,
    rental_type_id: 1,
    active: "Y",
    deleted:0,
  });
  const { language } = useOutletContext();
  // แปลภาษา
  const translations = {
    th: {
      code: "รหัส",
      room_name: "ชื่อห้อง",
      name2: "",
      status: "สถานะ",
      building: "อาคาร",
      floor: "ชั้น",
      rentaltype: "ประเภท",
      zone: "โซน",
      room_size: "ขนาด/ตรม.",
      monthly_price: "ราคาต่อเดือน",
      daily_price: "ราคาต่อวัน",
      hourly_price: "ราคาต่อชั่วโมง",
      status: "สถานะ",
      cancel: "ยกเลิก",
      save: "บันทึก",
      active: "เปิดใช้งาน",
      inactive: "ปิดใช้งาน",
    },
    en: {
      code: "Code",
      room_name: "Room",
      name2: "Room",
      status: "Status",
      building: "Building",
      floor: "Floor",
      rentaltype: "Type",
      zone: "Zone",
      room_size: "Size/Square Meter",
      monthly_price: "Price/Month",
      daily_price: "Price/Day",
      hourly_price: "Price/Hour",
      status: "Status",
      cancel: "Cancel",
      save: "Save",
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

          const zone_id = initialData.zone_id
            ? await fetchOptionId("zone/GetById?id=", initialData.zone_id)
            : null;

          const building_id = initialData.building_id
            ? await fetchOptionId(
                "building/GetById?id=",
                initialData.building_id
              )
            : null;

          const floor_id = initialData.floor_id
            ? await fetchOptionId("floor/GetById?id=", initialData.floor_id)
            : null;

          const rental_type_id = initialData.rental_type_id
            ? await fetchOptionId(
                "RentalType/GetById?id=",
                initialData.rental_type_id
              )
            : null;
          setFormData({
            id: initialData.id,
            room_name: initialData.room_name,
            room_size: initialData.room_size,
            monthly_price: initialData.monthly_price,
            daily_price: initialData.daily_price,
            hourly_price: initialData.hourly_price,
            zone_id: zone_id,
            building_id: building_id,
            floor_id: floor_id,
            rental_type_id: rental_type_id,
            active: selectedStatus,
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
      zone_id: formData.zone_id?.value || null,
      building_id: formData.building_id?.value || null,
      floor_id: formData.floor_id?.value || null,
      rental_type_id: formData.rental_type_id?.value || null,
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
          {translation.room_name}
        </label>
        <input
          type="text"
          name="room_name"
          value={formData.room_name}
          onChange={handleChange}
          className="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm"
        />
      </div>
      <div>
        <label className="block p-1 text-sm font-medium text-gray-700">
          {translation.room_size}
        </label>
        <input
          type="number"
          name="room_size"
          value={formData.room_size}
          onChange={handleChange}
          className="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm"
        />
      </div>
      {/* Dropdown สำหรับ type */}
      <div>
        <label className="block p-1 text-sm font-medium text-gray-700">
          {translation.rentaltype}
        </label>
        <AsyncSelect
          placeholder={translation.select}
          cacheOptions
          loadOptions={(inputValue) => loadOptions("rentaltype", inputValue)}
          defaultOptions
          value={formData.rental_type_id}
          onChange={(selectedOption) =>
            handleSelectChange("rental_type_id", selectedOption)
          }
        />
      </div>
      {/* Dropdown สำหรับ zone */}
      <div>
        <label className="block p-1 text-sm font-medium text-gray-700">
          {translation.zone}
        </label>
        <AsyncSelect
          placeholder={translation.select}
          cacheOptions
          loadOptions={(inputValue) => loadOptions("zone", inputValue)}
          defaultOptions
          value={formData.zone_id}
          onChange={(selectedOption) =>
            handleSelectChange("zone_id", selectedOption)
          }
        />
      </div>
      {/* Dropdown สำหรับ building */}
      <div>
        <label className="block p-1 text-sm font-medium text-gray-700">
          {translation.building}
        </label>
        <AsyncSelect
          placeholder={translation.select}
          cacheOptions
          loadOptions={(inputValue) => loadOptions("building", inputValue)}
          defaultOptions
          value={formData.building_id}
          onChange={(selectedOption) =>
            handleSelectChange("building_id", selectedOption)
          }
        />
      </div>
      {/* Dropdown สำหรับ bank floor */}
      <div>
        <label className="block p-1 text-sm font-medium text-gray-700">
          {translation.floor}
        </label>
        <AsyncSelect
          placeholder={translation.select}
          cacheOptions
          loadOptions={(inputValue) => loadOptions("floor", inputValue)}
          defaultOptions
          value={formData.floor_id}
          onChange={(selectedOption) =>
            handleSelectChange("floor_id", selectedOption)
          }
        />
      </div>

      <div>
        <label className="block p-1 text-sm font-medium text-gray-700">
          {translation.hourly_price}
        </label>
        <input
          type="number"
          name="hourly_price"
          value={formData.hourly_price}
          onChange={handleChange}
          className="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm"
        />
      </div>
      <div>
        <label className="block p-1 text-sm font-medium text-gray-700">
          {translation.daily_price}
        </label>
        <input
          type="number"
          name="daily_price"
          value={formData.daily_price}
          onChange={handleChange}
          className="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm"
        />
      </div>
      <div>
        <label className="block p-1 text-sm font-medium text-gray-700">
          {translation.monthly_price}
        </label>
        <input
          type="number"
          name="monthly_price"
          value={formData.monthly_price}
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

export default RentalSpaceForm;
