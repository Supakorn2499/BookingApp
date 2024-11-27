import axios from "axios";

// สร้าง instance ของ axios พร้อมตั้งค่า base URL
const api = axios.create({
  baseURL: "https://localhost:7005",
  timeout: 5000, // เวลา timeout (ms)
  headers: {
    "Content-Type": "application/json",
  },
});

api.interceptors.request.use((config) => {
  const token = localStorage.getItem("token");
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

api.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response && error.response.status === 401) {
      localStorage.removeItem("token");
      window.location.href = "/login";
    }
    return Promise.reject(error);
  }
);

export default api;
