import { defineStore } from "pinia";
import { ref } from "vue";

export const useAuthStore = defineStore("auth", () => {
  const token = ref<string | null>(localStorage.getItem("token"));
  const user = ref<any | null>(
    JSON.parse(localStorage.getItem("user") || "null"),
  );

  const setAuth = (newToken: string, userData: any) => {
    token.value = newToken;
    user.value = userData;
    localStorage.setItem("token", newToken);
    localStorage.setItem("user", JSON.stringify(userData));
  };

  const logout = () => {
    token.value = null;
    user.value = null;
    localStorage.removeItem("token");
    localStorage.removeItem("user");
  };

  const isAuthenticated = () => !!token.value;

  const isAdmin = () => user.value?.vaiTro === "admin";

  return { token, user, setAuth, logout, isAuthenticated, isAdmin };
});
