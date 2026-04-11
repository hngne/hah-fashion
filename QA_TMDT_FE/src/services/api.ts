import axios from "axios";

const api = axios.create({
  baseURL: import.meta.env.VITE_API_BASE_URL || "https://localhost:7196/api",

  timeout: 15000,
  headers: {
    "Content-Type": "application/json",
    "ngrok-skip-browser-warning": "true",
  },
});

// Request Interceptor: Tự động đính kèm Token
api.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem("token");
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => {
    return Promise.reject(error);
  },
);

// Response Interceptor: Xử lý lỗi 401 Unauthorized toàn cục
api.interceptors.response.use(
  (response) => {
    // API Của bạn trả về Wrapper { success, message, response }
    return response.data;
  },
  (error) => {
    if (error.response && error.response.status === 401) {
      // Token hết hạn hoặc không hợp lệ -> Xóa bộ nhớ và điều hướng
      localStorage.removeItem("token");
      localStorage.removeItem("user");
      const currentPath = window.location.pathname;
      if (currentPath.startsWith("/admin")) {
        window.location.assign("/admin/login");
      } else {
        const redirect = encodeURIComponent(
          `${window.location.pathname}${window.location.search}`,
        );
        window.location.assign(`/login?redirect=${redirect}`);
      }
    }
    return Promise.reject(error);
  },
);

export default api;
