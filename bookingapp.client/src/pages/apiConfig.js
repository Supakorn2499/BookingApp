import axios from "axios";

// สร้าง instance ของ axios พร้อมตั้งค่า base URL
const api = axios.create({
  baseURL: "https://localhost:7005",
  timeout: 5000, // เวลา timeout (ms)
  headers: {
    "Content-Type": "application/json",
  },
});

export default api;
