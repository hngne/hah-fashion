import axios from "axios";

const api = axios.create({
  baseURL: import.meta.env.VITE_API_BASE_URL || "https://localhost:7196/api",

  timeout: 30000,
  headers: {
    "Content-Type": "application/json",
    "ngrok-skip-browser-warning": "true",
  },
});

// Request Interceptor: Tá»± Ä‘á»™ng Ä‘Ã­nh kÃ¨m Token
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

// Response Interceptor: Xá»­ lÃ½ lá»—i 401 Unauthorized toÃ n cá»¥c
api.interceptors.response.use(
  (response) => {
    // API Cá»§a báº¡n tráº£ vá» Wrapper { success, message, response }
    return response.data;
  },
  (error) => {
    if (error.response && error.response.status === 401) {
      // Token háº¿t háº¡n hoáº·c khÃ´ng há»£p lá»‡ -> XÃ³a bá»™ nhá»› vÃ  Ä‘iá»u hÆ°á»›ng
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

