import api from "./api";

export const thongKeService = {
  // Doanh thu theo khoảng thời gian (type: "day" | "month" | "year")
  getDoanhThu(from: string, to: string, type: string) {
    return api.get("/ThongKe/doanh-thu", {
      params: { from, to, type },
    });
  },

  // Top sản phẩm bán chạy
  getTopBanChay(top: number = 5) {
    return api.get("/ThongKe/top-ban-chay", { params: { top } });
  },

  // Top sản phẩm ế
  getTopE(top: number = 5) {
    return api.get("/ThongKe/top-e", { params: { top } });
  },
};
