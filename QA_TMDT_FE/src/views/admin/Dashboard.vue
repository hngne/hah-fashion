<template>
  <div>
    <!-- Page Title -->
    <div class="mb-6">
      <h3 class="text-xl sm:text-2xl font-bold text-[#1F2937]">
        Xin chào, {{ userName }}!
      </h3>
      <p class="text-sm text-gray-500 mt-1">
        Tổng quan hoạt động kinh doanh HaHFashion.
      </p>
    </div>

    <!-- Loading -->
    <div
      v-if="isLoading"
      class="flex items-center justify-center h-64 text-gray-400"
    >
      <Loader2Icon class="h-8 w-8 animate-spin mr-3" stroke-width="1.5" />
      <span class="text-lg font-medium">Đang tải dữ liệu...</span>
    </div>

    <template v-else>
      <!-- ===== METRIC CARDS ===== -->
      <div class="grid grid-cols-2 lg:grid-cols-4 gap-4 mb-6">
        <MetricCard
          v-for="card in metricCards"
          :key="card.label"
          :icon="card.icon"
          :label="card.label"
          :value="card.value"
          :change="card.change"
          :changeType="card.changeType"
          :iconBg="card.iconBg"
          :iconColor="card.iconColor"
        />
      </div>

      <!-- ===== CHARTS ROW ===== -->
      <div class="grid grid-cols-1 xl:grid-cols-3 gap-5 mb-6">
        <!-- Area Chart: Doanh thu -->
        <div
          class="xl:col-span-2 bg-white rounded-xl border border-gray-200 p-4 sm:p-6"
        >
          <div
            class="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-3 mb-5"
          >
            <h4 class="text-sm font-bold text-gray-800 uppercase tracking-wide">
              Doanh thu theo thời gian
            </h4>
            <div class="flex flex-wrap items-center gap-2">
              <input
                type="date"
                v-model="dateFrom"
                @change="fetchDoanhThu"
                class="text-xs border border-gray-200 rounded-lg px-2.5 py-1.5 text-gray-600 focus:outline-none focus:ring-2 focus:ring-blue-500/20"
              />
              <span class="text-xs text-gray-400">→</span>
              <input
                type="date"
                v-model="dateTo"
                @change="fetchDoanhThu"
                class="text-xs border border-gray-200 rounded-lg px-2.5 py-1.5 text-gray-600 focus:outline-none focus:ring-2 focus:ring-blue-500/20"
              />
              <select
                v-model="chartType"
                @change="fetchDoanhThu"
                class="text-xs border border-gray-200 rounded-lg px-2.5 py-1.5 text-gray-600 focus:outline-none focus:ring-2 focus:ring-blue-500/20"
              >
                <option value="day">Ngày</option>
                <option value="month">Tháng</option>
                <option value="year">Năm</option>
              </select>
            </div>
          </div>
          <div class="h-[280px] sm:h-[300px]">
            <Line
              v-if="doanhThuData.length > 0"
              :data="areaChartData"
              :options="areaChartOptions"
            />
            <div
              v-else
              class="h-full flex items-center justify-center text-gray-400 text-sm"
            >
              Chưa có dữ liệu doanh thu trong khoảng thời gian này.
            </div>
          </div>
        </div>

        <!-- Doughnut Chart: Doanh thu theo danh mục -->
        <div class="bg-white rounded-xl border border-gray-200 p-4 sm:p-6">
          <h4
            class="text-sm font-bold text-gray-800 uppercase tracking-wide mb-5"
          >
            Top danh mục bán chạy
          </h4>
          <div class="h-[220px] flex items-center justify-center">
            <Doughnut
              v-if="topBanChay.length > 0"
              :data="doughnutChartData"
              :options="doughnutChartOptions"
            />
            <span v-else class="text-gray-400 text-sm">Chưa có dữ liệu.</span>
          </div>
          <!-- Legend -->
          <div
            v-if="topBanChay.length > 0"
            class="mt-4 grid grid-cols-2 gap-1.5"
          >
            <div
              v-for="(sp, i) in topBanChay"
              :key="sp.maSP"
              class="flex items-center space-x-2"
            >
              <span
                class="w-2.5 h-2.5 rounded-full flex-shrink-0"
                :style="{ backgroundColor: chartColors[i] }"
              ></span>
              <span class="text-[11px] text-gray-600 truncate"
                >{{ sp.tenSP }} ({{ sp.soLuongBan }})</span
              >
            </div>
          </div>
        </div>
      </div>

      <!-- ===== TOP PRODUCTS ROW ===== -->
      <div class="grid grid-cols-1 lg:grid-cols-2 gap-5 mb-6">
        <!-- Top bán chạy -->
        <div class="bg-white rounded-xl border border-gray-200 p-4 sm:p-6">
          <div class="flex items-center space-x-2 mb-5">
            <TrendingUpIcon class="h-4 w-4 text-emerald-500" stroke-width="2" />
            <h4 class="text-sm font-bold text-gray-800 uppercase tracking-wide">
              Top bán chạy
            </h4>
          </div>
          <div v-if="topBanChay.length > 0" class="space-y-3">
            <div
              v-for="(sp, idx) in topBanChay"
              :key="sp.maSP"
              class="flex items-center space-x-3 p-2.5 rounded-lg hover:bg-gray-50 transition-colors"
            >
              <div
                :class="[
                  'w-8 h-8 rounded-full flex items-center justify-center text-xs font-extrabold flex-shrink-0',
                  idx === 0
                    ? 'bg-amber-100 text-amber-700'
                    : idx === 1
                      ? 'bg-gray-100 text-gray-600'
                      : idx === 2
                        ? 'bg-orange-100 text-orange-600'
                        : 'bg-gray-50 text-gray-400',
                ]"
              >
                {{ idx + 1 }}
              </div>
              <div class="flex-1 min-w-0">
                <p class="text-[13px] font-semibold text-gray-800 truncate">
                  {{ sp.tenSP }}
                </p>
                <p class="text-[11px] text-gray-400">
                  {{ sp.soLuongBan }} đã bán · {{ formatMoney(sp.tongTienThu) }}
                </p>
              </div>
            </div>
          </div>
          <div
            v-else
            class="h-40 flex items-center justify-center text-gray-400 text-sm"
          >
            Chưa có dữ liệu.
          </div>
        </div>

        <!-- Top bán ế -->
        <div class="bg-white rounded-xl border border-gray-200 p-4 sm:p-6">
          <div class="flex items-center space-x-2 mb-5">
            <TrendingDownIcon class="h-4 w-4 text-red-400" stroke-width="2" />
            <h4 class="text-sm font-bold text-gray-800 uppercase tracking-wide">
              Top sản phẩm ế
            </h4>
          </div>
          <div v-if="topBanE.length > 0" class="space-y-3">
            <div
              v-for="(sp, idx) in topBanE"
              :key="sp.maSP"
              class="flex items-center space-x-3 p-2.5 rounded-lg hover:bg-gray-50 transition-colors"
            >
              <div
                class="w-8 h-8 rounded-full flex items-center justify-center text-xs font-extrabold flex-shrink-0 bg-red-50 text-red-400"
              >
                {{ idx + 1 }}
              </div>
              <div class="flex-1 min-w-0">
                <p class="text-[13px] font-semibold text-gray-800 truncate">
                  {{ sp.tenSP }}
                </p>
                <p class="text-[11px] text-gray-400">
                  {{ sp.soLuongBan }} đã bán · {{ formatMoney(sp.tongTienThu) }}
                </p>
              </div>
            </div>
          </div>
          <div
            v-else
            class="h-40 flex items-center justify-center text-gray-400 text-sm"
          >
            Không có sản phẩm ế.
          </div>
        </div>
      </div>

      <!-- ===== RECENT ORDERS TABLE ===== -->
      <div class="bg-white rounded-xl border border-gray-200">
        <div
          class="px-4 sm:px-6 py-4 border-b border-gray-100 flex items-center justify-between"
        >
          <h4 class="text-sm font-bold text-gray-800 uppercase tracking-wide">
            Đơn hàng gần đây
          </h4>
          <span class="text-xs text-gray-400 font-medium"
            >{{ recentOrders.length }} đơn</span
          >
        </div>
        <!-- Mobile: card view -->
        <div class="sm:hidden divide-y divide-gray-50">
          <div
            v-for="order in recentOrders"
            :key="order.maDonHang"
            class="p-4 space-y-2"
          >
            <div class="flex items-center justify-between">
              <span class="text-sm font-bold text-blue-600">{{
                order.maDonHang
              }}</span>
              <span
                :class="[
                  'text-[10px] font-bold px-2 py-0.5 rounded-full',
                  getStatusClass(order.trangThaiDonHang),
                ]"
              >
                {{ statusLabel(order.trangThaiDonHang) }}
              </span>
            </div>
            <div class="flex items-center justify-between text-sm">
              <span class="text-gray-500">{{
                formatDate(order.ngayDatHang)
              }}</span>
              <span class="font-bold text-gray-800">{{
                formatMoney(order.thanhToan)
              }}</span>
            </div>
          </div>
          <div
            v-if="recentOrders.length === 0"
            class="p-8 text-center text-gray-400 text-sm"
          >
            Chưa có đơn hàng.
          </div>
        </div>
        <!-- Desktop: table view -->
        <div class="hidden sm:block overflow-x-auto">
          <table v-if="recentOrders.length > 0" class="w-full">
            <thead>
              <tr
                class="text-left text-[11px] font-bold text-gray-400 uppercase tracking-wider"
              >
                <th class="px-6 py-3">Mã đơn</th>
                <th class="px-6 py-3">Ngày đặt</th>
                <th class="px-6 py-3">Tổng tiền</th>
                <th class="px-6 py-3">Phí ship</th>
                <th class="px-6 py-3">Thanh toán</th>
                <th class="px-6 py-3">Trạng thái</th>
              </tr>
            </thead>
            <tbody class="divide-y divide-gray-50">
              <tr
                v-for="order in recentOrders"
                :key="order.maDonHang"
                class="hover:bg-gray-50/50 transition-colors"
              >
                <td class="px-6 py-3.5 text-sm font-bold text-blue-600">
                  {{ order.maDonHang }}
                </td>
                <td class="px-6 py-3.5 text-sm text-gray-500">
                  {{ formatDate(order.ngayDatHang) }}
                </td>
                <td class="px-6 py-3.5 text-sm font-semibold text-gray-800">
                  {{ formatMoney(order.tongTienHang) }}
                </td>
                <td class="px-6 py-3.5 text-sm text-gray-500">
                  {{ formatMoney(order.phiShip || 0) }}
                </td>
                <td class="px-6 py-3.5 text-sm font-bold text-gray-800">
                  {{ formatMoney(order.thanhToan) }}
                </td>
                <td class="px-6 py-3.5">
                  <span
                    :class="[
                      'text-[11px] font-bold px-2.5 py-1 rounded-full',
                      getStatusClass(order.trangThaiDonHang),
                    ]"
                  >
                    {{ statusLabel(order.trangThaiDonHang) }}
                  </span>
                </td>
              </tr>
            </tbody>
          </table>
          <div v-else class="p-10 text-center text-gray-400 text-sm">
            Chưa có đơn hàng nào.
          </div>
        </div>
      </div>
    </template>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from "vue";
import { useAuthStore } from "../../stores/auth";
import {
  DollarSign,
  ShoppingBag,
  Users as UsersIcon,
  Package,
  Loader2 as Loader2Icon,
  TrendingUp as TrendingUpIcon,
  TrendingDown as TrendingDownIcon,
} from "lucide-vue-next";
import { Line, Doughnut } from "vue-chartjs";
import {
  Chart as ChartJS,
  CategoryScale,
  LinearScale,
  PointElement,
  LineElement,
  ArcElement,
  Tooltip,
  Legend,
  Filler,
} from "chart.js";
import MetricCard from "../../components/admin/MetricCard.vue";
import { thongKeService } from "../../services/thongke.service";
import { donHangService } from "../../services/donhang.service";
import { taiKhoanService } from "../../services/taikhoan.service";
import { sanPhamService } from "../../services/sanpham.service";
import { getTotalItems } from "../../services/service-helpers";

ChartJS.register(
  CategoryScale,
  LinearScale,
  PointElement,
  LineElement,
  ArcElement,
  Tooltip,
  Legend,
  Filler,
);

const authStore = useAuthStore();
const isLoading = ref(true);

const chartColors = [
  "#2563eb",
  "#f59e0b",
  "#10b981",
  "#8b5cf6",
  "#ef4444",
  "#06b6d4",
  "#ec4899",
];

// ===== DATA =====
const totalRevenue = ref(0);
const totalOrders = ref(0);
const totalUsers = ref(0);
const totalProducts = ref(0);
const doanhThuData = ref<any[]>([]);
const topBanChay = ref<any[]>([]);
const topBanE = ref<any[]>([]);
const recentOrders = ref<any[]>([]);
const chartType = ref("month");

// Date pickers
const now = new Date();
const dateFrom = ref(
  new Date(now.getFullYear(), 0, 1).toISOString().slice(0, 10),
);
const dateTo = ref(now.toISOString().slice(0, 10));

const userName = computed(
  () => authStore.user?.hoTen?.split(" ").pop() || "Admin",
);

// ===== METRIC CARDS =====
const metricCards = computed(() => [
  {
    icon: DollarSign,
    label: "Tổng doanh thu",
    value: formatMoney(totalRevenue.value),
    change: "Thực tế",
    changeType: "up" as const,
    iconBg: "bg-blue-50",
    iconColor: "text-blue-600",
  },
  {
    icon: ShoppingBag,
    label: "Tổng đơn hàng",
    value: totalOrders.value.toLocaleString("vi-VN"),
    change: "Tất cả",
    changeType: "up" as const,
    iconBg: "bg-emerald-50",
    iconColor: "text-emerald-600",
  },
  {
    icon: UsersIcon,
    label: "Người dùng",
    value: totalUsers.value.toLocaleString("vi-VN"),
    change: "Tài khoản",
    changeType: "up" as const,
    iconBg: "bg-violet-50",
    iconColor: "text-violet-600",
  },
  {
    icon: Package,
    label: "Sản phẩm",
    value: totalProducts.value.toLocaleString("vi-VN"),
    change: "Đang bán",
    changeType: "up" as const,
    iconBg: "bg-amber-50",
    iconColor: "text-amber-600",
  },
]);

// ===== CHART CONFIGS =====
const areaChartData = computed(() => ({
  labels: doanhThuData.value.map((d: any) => d.thoiGian),
  datasets: [
    {
      label: "Doanh thu (₫)",
      data: doanhThuData.value.map((d: any) => d.doanhThu),
      borderColor: "#2563eb",
      backgroundColor: "rgba(37, 99, 235, 0.08)",
      fill: true,
      tension: 0.4,
      pointRadius: 4,
      pointBackgroundColor: "#2563eb",
      pointBorderColor: "#fff",
      pointBorderWidth: 2,
      pointHoverRadius: 6,
      borderWidth: 2.5,
    },
  ],
}));

const areaChartOptions = {
  responsive: true,
  maintainAspectRatio: false,
  plugins: {
    legend: { display: false },
    tooltip: {
      backgroundColor: "#1e293b",
      titleFont: { size: 12, weight: "bold" as const },
      bodyFont: { size: 11 },
      padding: 10,
      cornerRadius: 8,
      callbacks: {
        label: (ctx: any) => formatMoney(ctx.raw),
      },
    },
  },
  scales: {
    x: {
      grid: { display: false },
      ticks: { font: { size: 10 }, color: "#9ca3af", maxRotation: 45 },
    },
    y: {
      grid: { color: "#f3f4f6" },
      ticks: {
        font: { size: 10 },
        color: "#9ca3af",
        callback: (val: any) => {
          if (val >= 1_000_000) return (val / 1_000_000).toFixed(0) + "M";
          if (val >= 1_000) return (val / 1_000).toFixed(0) + "K";
          return val;
        },
      },
    },
  },
};

const doughnutChartData = computed(() => ({
  labels: topBanChay.value.map((sp: any) => sp.tenSP),
  datasets: [
    {
      data: topBanChay.value.map((sp: any) => sp.soLuongBan),
      backgroundColor: chartColors.slice(0, topBanChay.value.length),
      borderWidth: 3,
      borderColor: "#ffffff",
      hoverOffset: 6,
    },
  ],
}));

const doughnutChartOptions = {
  responsive: true,
  maintainAspectRatio: false,
  cutout: "65%",
  plugins: {
    legend: { display: false },
    tooltip: {
      backgroundColor: "#1e293b",
      cornerRadius: 8,
      bodyFont: { size: 11 },
    },
  },
};

// ===== HELPERS =====
const formatMoney = (val: number) => {
  if (!val) return "0 ₫";
  return val.toLocaleString("vi-VN") + " ₫";
};

const formatDate = (dateStr: string) => {
  if (!dateStr) return "";
  return new Date(dateStr).toLocaleDateString("vi-VN", {
    day: "2-digit",
    month: "2-digit",
    year: "numeric",
  });
};

const statusMap: Record<string, string> = {
  ChoXacNhan: "Chờ xác nhận",
  DangVanChuyen: "Đang vận chuyển",
  GiaoHangThanhCong: "Giao thành công",
  DaHuy: "Đã hủy",
};
const statusLabel = (s: string) => statusMap[s] || s;

const getStatusClass = (status: string) => {
  if (status === "GiaoHangThanhCong") return "bg-emerald-50 text-emerald-700";
  if (status === "DangVanChuyen") return "bg-blue-50 text-blue-700";
  if (status === "ChoXacNhan") return "bg-yellow-50 text-yellow-700";
  if (status === "DaHuy") return "bg-red-50 text-red-700";
  return "bg-gray-50 text-gray-700";
};

// ===== FETCH =====
const fetchDoanhThu = async () => {
  try {
    const res: any = await thongKeService.getDoanhThu(
      dateFrom.value,
      dateTo.value,
      chartType.value,
    );
    if (res.success) {
      doanhThuData.value = res.data || [];
      totalRevenue.value = doanhThuData.value.reduce(
        (s: number, d: any) => s + (d.doanhThu || 0),
        0,
      );
    }
  } catch (e) {
    console.warn("Lỗi doanh thu:", e);
  }
};

const fetchAll = async () => {
  isLoading.value = true;
  try {
    const [ordersRes, usersRes, productsRes, topChayRes, topERes]: any[] =
      await Promise.allSettled([
        donHangService.getAllAdmin(),
        taiKhoanService.getAllAdmin(),
        sanPhamService.getAll(1, 1),
        thongKeService.getTopBanChay(5),
        thongKeService.getTopE(5),
      ]);

    if (ordersRes.status === "fulfilled" && ordersRes.value?.success) {
      const all = ordersRes.value.data || [];
      totalOrders.value = all.length;
      recentOrders.value = [...all]
        .sort(
          (a: any, b: any) =>
            new Date(b.ngayDatHang).getTime() -
            new Date(a.ngayDatHang).getTime(),
        )
        .slice(0, 8);
    }
    if (usersRes.status === "fulfilled" && usersRes.value?.success)
      totalUsers.value = (usersRes.value.data || []).length;
    if (productsRes.status === "fulfilled" && productsRes.value?.success) {
      const d = productsRes.value.data;
      totalProducts.value = getTotalItems(d);
    }
    if (topChayRes.status === "fulfilled" && topChayRes.value?.success)
      topBanChay.value = topChayRes.value.data || [];
    if (topERes.status === "fulfilled" && topERes.value?.success)
      topBanE.value = topERes.value.data || [];

    await fetchDoanhThu();
  } catch (e) {
    console.error("Lỗi dashboard:", e);
  } finally {
    isLoading.value = false;
  }
};

onMounted(fetchAll);
</script>
