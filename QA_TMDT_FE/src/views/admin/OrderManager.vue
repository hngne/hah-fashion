<template>
  <div>
    <Toast />
    <!-- Header -->
    <div
      class="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-3 mb-6"
    >
      <div>
        <h3 class="text-xl font-bold text-[#1F2937]">Quản lý Đơn hàng</h3>
        <p class="text-sm text-gray-500 mt-0.5">
          Xem, cập nhật trạng thái và quản lý đơn hàng.
        </p>
      </div>
    </div>

    <!-- Filters -->
    <div class="bg-white rounded-xl border border-gray-200 p-4 mb-5">
      <div class="flex flex-col sm:flex-row gap-3">
        <div class="flex-1 relative">
          <SearchIcon
            class="absolute left-3 top-1/2 -translate-y-1/2 h-4 w-4 text-gray-400"
            stroke-width="2"
          />
          <input
            v-model="searchQuery"
            type="text"
            placeholder="Tìm theo mã đơn hàng..."
            class="w-full pl-9 pr-4 py-2.5 border border-gray-200 rounded-lg text-sm focus:outline-none focus:ring-2 focus:ring-blue-500/20 focus:border-blue-400"
          />
        </div>
        <div
          class="flex items-center space-x-1 bg-gray-100 rounded-lg p-0.5 flex-shrink-0 overflow-x-auto"
        >
          <button
            v-for="f in statusFilters"
            :key="f.value"
            @click="filterStatus = f.value"
            :class="[
              'px-3 py-1.5 text-xs font-semibold rounded-md transition whitespace-nowrap',
              filterStatus === f.value
                ? 'bg-white text-blue-600 shadow-sm'
                : 'text-gray-500',
            ]"
          >
            {{ f.label }}
          </button>
        </div>
      </div>
    </div>

    <div v-if="loading" class="flex justify-center py-16 text-gray-400">
      <Loader2Icon class="h-7 w-7 animate-spin" />
    </div>

    <!-- Table -->
    <div
      v-else
      class="bg-white rounded-xl border border-gray-200 overflow-hidden"
    >
      <!-- Desktop -->
      <div class="hidden lg:block overflow-x-auto">
        <table class="w-full">
          <thead>
            <tr
              class="text-left text-[11px] font-bold text-gray-400 uppercase tracking-wider border-b border-gray-100"
            >
              <th class="px-5 py-3">Mã đơn</th>
              <th class="px-5 py-3">Ngày đặt</th>
              <th class="px-5 py-3">Trạng thái</th>
              <th class="px-5 py-3">Tổng tiền</th>
              <th class="px-5 py-3">Thanh toán</th>
              <th class="px-5 py-3 w-40 text-right">Thao tác</th>
            </tr>
          </thead>
          <tbody class="divide-y divide-gray-50">
            <tr
              v-for="dh in filteredList"
              :key="dh.maDonHang"
              class="hover:bg-gray-50/50 transition-colors cursor-pointer"
              @click="viewDetail(dh.maDonHang)"
            >
              <td
                class="px-5 py-3 text-sm font-mono font-semibold text-blue-600"
              >
                {{ dh.maDonHang }}
              </td>
              <td class="px-5 py-3 text-sm text-gray-600">
                {{ formatDate(dh.ngayDatHang) }}
              </td>
              <td class="px-5 py-3">
                <span
                  :class="statusBadgeClass(dh.trangThaiDonHang)"
                  class="text-xs font-bold px-2.5 py-1 rounded-full"
                  >{{ statusLabel(dh.trangThaiDonHang) }}</span
                >
              </td>
              <td class="px-5 py-3 text-sm font-medium text-gray-800">
                {{ formatMoney(dh.tongTienHang) }}
              </td>
              <td class="px-5 py-3 text-sm font-semibold text-emerald-600">
                {{ formatMoney(dh.thanhToan) }}
              </td>
              <td class="px-5 py-3 text-right" @click.stop>
                <button
                  @click="viewDetail(dh.maDonHang)"
                  class="p-1.5 rounded-lg text-gray-400 hover:text-blue-600 hover:bg-blue-50 transition-colors mr-1"
                  title="Chi tiết"
                >
                  <EyeIcon class="h-4 w-4" stroke-width="2" />
                </button>
                <button
                  v-if="canTransition(dh.trangThaiDonHang)"
                  @click="openStatusModal(dh)"
                  class="p-1.5 rounded-lg text-gray-400 hover:text-orange-500 hover:bg-orange-50 transition-colors"
                  title="Đổi trạng thái"
                >
                  <RefreshCwIcon class="h-4 w-4" stroke-width="2" />
                </button>
              </td>
            </tr>
            <tr v-if="filteredList.length === 0">
              <td
                colspan="6"
                class="px-5 py-10 text-center text-gray-400 text-sm"
              >
                Không tìm thấy đơn hàng nào.
              </td>
            </tr>
          </tbody>
        </table>
      </div>

      <!-- Mobile -->
      <div class="lg:hidden divide-y divide-gray-50">
        <div
          v-for="dh in filteredList"
          :key="dh.maDonHang"
          class="p-4 space-y-2 cursor-pointer"
          @click="viewDetail(dh.maDonHang)"
        >
          <div class="flex items-center justify-between">
            <span class="text-sm font-mono font-semibold text-blue-600">{{
              dh.maDonHang
            }}</span>
            <span
              :class="statusBadgeClass(dh.trangThaiDonHang)"
              class="text-[10px] font-bold px-2 py-0.5 rounded-full"
              >{{ statusLabel(dh.trangThaiDonHang) }}</span
            >
          </div>
          <div class="flex items-center justify-between text-xs text-gray-500">
            <span>{{ formatDate(dh.ngayDatHang) }}</span>
            <span class="font-bold text-gray-800">{{
              formatMoney(dh.thanhToan)
            }}</span>
          </div>
        </div>
        <div
          v-if="filteredList.length === 0"
          class="p-8 text-center text-gray-400 text-sm"
        >
          Không có đơn hàng.
        </div>
      </div>
    </div>

    <!-- ===== DETAIL MODAL ===== -->
    <teleport to="body"
      ><transition name="modal">
        <div
          v-if="showDetail"
          class="fixed inset-0 z-50 flex items-center justify-center p-4 bg-black/40 overflow-y-auto"
          @click.self="showDetail = false"
        >
          <div
            class="bg-white rounded-xl shadow-2xl w-full max-w-2xl my-8 max-h-[90vh] overflow-y-auto"
          >
            <div
              v-if="detailLoading"
              class="flex justify-center py-16 text-gray-400"
            >
              <Loader2Icon class="h-7 w-7 animate-spin" />
            </div>
            <template v-else-if="detail">
              <!-- Header -->
              <div
                class="sticky top-0 bg-white px-6 py-4 border-b border-gray-100 flex items-center justify-between z-10"
              >
                <div>
                  <h4 class="text-lg font-bold text-gray-800">
                    Đơn hàng {{ detail.maDonHang }}
                  </h4>
                  <span
                    :class="statusBadgeClass(detail.trangThaiDonHang)"
                    class="text-xs font-bold px-2.5 py-1 rounded-full mt-1 inline-block"
                    >{{ statusLabel(detail.trangThaiDonHang) }}</span
                  >
                </div>
                <button
                  @click="showDetail = false"
                  class="p-1.5 rounded-lg text-gray-400 hover:bg-gray-100"
                >
                  <XIcon class="h-5 w-5" />
                </button>
              </div>
              <!-- Body -->
              <div class="p-6 space-y-5">
                <div class="grid grid-cols-2 gap-x-6 gap-y-3 text-sm">
                  <div>
                    <span class="text-gray-400 text-xs uppercase font-bold"
                      >Ngày đặt</span
                    >
                    <p class="font-semibold text-gray-800 mt-0.5">
                      {{ formatDate(detail.ngayDatHang) }}
                    </p>
                  </div>
                  <div>
                    <span class="text-gray-400 text-xs uppercase font-bold"
                      >Ngày giao</span
                    >
                    <p class="font-semibold text-gray-800 mt-0.5">
                      {{ formatDate(detail.ngayGiaoHang) }}
                    </p>
                  </div>
                  <div>
                    <span class="text-gray-400 text-xs uppercase font-bold"
                      >Thanh toán</span
                    >
                    <p class="font-semibold text-gray-800 mt-0.5">
                      {{ detail.phuongThucThanhToan }}
                    </p>
                  </div>
                  <div>
                    <span class="text-gray-400 text-xs uppercase font-bold"
                      >Vận chuyển</span
                    >
                    <p class="font-semibold text-gray-800 mt-0.5">
                      {{ detail.phuongThucVanChuyen }}
                    </p>
                  </div>
                  <div class="col-span-2">
                    <span class="text-gray-400 text-xs uppercase font-bold"
                      >Địa chỉ</span
                    >
                    <p class="font-semibold text-gray-800 mt-0.5">
                      {{ detail.diaChiGiaoHang }}
                    </p>
                  </div>
                </div>

                <!-- Money summary -->
                <div class="bg-gray-50 rounded-lg p-4 space-y-2 text-sm">
                  <div class="flex justify-between">
                    <span class="text-gray-500">Tổng tiền hàng</span
                    ><span class="font-medium">{{
                      formatMoney(detail.tongTienHang)
                    }}</span>
                  </div>
                  <div class="flex justify-between">
                    <span class="text-gray-500">Phí ship</span
                    ><span class="font-medium">{{
                      formatMoney(detail.phiShip || 0)
                    }}</span>
                  </div>
                  <div
                    v-if="detail.giamGiaVoucher"
                    class="flex justify-between"
                  >
                    <span class="text-gray-500">Giảm giá voucher</span
                    ><span class="font-medium text-red-500"
                      >-{{ formatMoney(detail.giamGiaVoucher) }}</span
                    >
                  </div>
                  <div
                    class="flex justify-between border-t border-gray-200 pt-2"
                  >
                    <span class="font-bold text-gray-800">Thành tiền</span
                    ><span class="font-bold text-emerald-600 text-base">{{
                      formatMoney(detail.thanhToan)
                    }}</span>
                  </div>
                </div>

                <!-- Items table -->
                <div>
                  <h5 class="text-xs font-bold text-gray-400 uppercase mb-2">
                    Sản phẩm ({{ detail.chiTietDonHang?.length || 0 }})
                  </h5>
                  <div
                    class="overflow-x-auto border border-gray-200 rounded-lg"
                  >
                    <table class="w-full text-sm">
                      <thead>
                        <tr
                          class="text-[10px] font-bold text-gray-400 uppercase bg-gray-50"
                        >
                          <th class="px-4 py-2 text-left">Sản phẩm</th>
                          <th class="px-4 py-2 text-left">Size</th>
                          <th class="px-4 py-2 text-left">Màu</th>
                          <th class="px-4 py-2 text-right">SL</th>
                          <th class="px-4 py-2 text-right">Đơn giá</th>
                          <th class="px-4 py-2 text-right">Thành tiền</th>
                        </tr>
                      </thead>
                      <tbody class="divide-y divide-gray-50">
                        <tr
                          v-for="ct in detail.chiTietDonHang"
                          :key="ct.maCTSP"
                          class="hover:bg-gray-50/50"
                        >
                          <td class="px-4 py-2">
                            <div class="flex items-center space-x-3">
                              <img
                                v-if="ct.hinhAnhUrl"
                                :src="ct.hinhAnhUrl"
                                alt="Product"
                                class="w-10 h-10 rounded-md object-cover border border-gray-100"
                              />
                              <div
                                v-else
                                class="w-10 h-10 rounded-md bg-gray-100 flex items-center justify-center text-gray-400"
                              >
                                <PackageIcon class="w-5 h-5" />
                              </div>
                              <span class="font-medium">{{ ct.tenSP }}</span>
                            </div>
                          </td>
                          <td class="px-4 py-2">{{ ct.tenSize }}</td>
                          <td class="px-4 py-2">{{ ct.tenMau }}</td>
                          <td class="px-4 py-2 text-right">{{ ct.soLuong }}</td>
                          <td class="px-4 py-2 text-right">
                            {{ formatMoney(ct.donGia) }}
                          </td>
                          <td class="px-4 py-2 text-right font-semibold">
                            {{ formatMoney(ct.soLuong * ct.donGia) }}
                          </td>
                        </tr>
                      </tbody>
                    </table>
                  </div>
                </div>
              </div>
              <!-- Footer: status change -->
              <div
                class="px-6 py-4 border-t border-gray-100 flex items-center justify-between"
              >
                <div
                  v-if="canTransition(detail.trangThaiDonHang)"
                  class="flex items-center space-x-2"
                >
                  <span class="text-xs text-gray-400 font-semibold"
                    >Chuyển trạng thái:</span
                  >
                  <button
                    v-for="next in nextStatuses(detail.trangThaiDonHang)"
                    :key="next"
                    @click="changeStatus(detail.maDonHang, next)"
                    :disabled="changingStatus"
                    :class="[
                      'px-3 py-1.5 text-xs font-semibold rounded-lg transition disabled:opacity-50',
                      next === 'DaHuy'
                        ? 'bg-red-50 text-red-600 hover:bg-red-100'
                        : 'bg-blue-50 text-blue-600 hover:bg-blue-100',
                    ]"
                  >
                    {{ statusLabel(next) }}
                  </button>
                </div>
                <div v-else class="text-xs text-gray-400">
                  Đơn hàng đã kết thúc.
                </div>
                <button
                  @click="showDetail = false"
                  class="px-4 py-2 text-sm font-medium text-gray-600 hover:bg-gray-100 rounded-lg transition"
                >
                  Đóng
                </button>
              </div>
            </template>
          </div>
        </div>
      </transition></teleport
    >

    <!-- ===== STATUS CHANGE MODAL ===== -->
    <teleport to="body"
      ><transition name="modal">
        <div
          v-if="showStatusModal"
          class="fixed inset-0 z-50 flex items-center justify-center p-4 bg-black/40"
          @click.self="showStatusModal = false"
        >
          <div class="bg-white rounded-xl shadow-2xl w-full max-w-sm p-6">
            <h4 class="text-lg font-bold text-gray-800 mb-4">Đổi trạng thái</h4>
            <p class="text-sm text-gray-500 mb-3">
              Đơn: <strong>{{ statusTarget?.maDonHang }}</strong>
            </p>
            <p class="text-sm text-gray-500 mb-4">
              Hiện tại:
              <span
                :class="statusBadgeClass(statusTarget?.trangThaiDonHang)"
                class="text-xs font-bold px-2 py-0.5 rounded-full"
                >{{ statusLabel(statusTarget?.trangThaiDonHang || "") }}</span
              >
            </p>
            <div class="space-y-2">
              <button
                v-for="next in nextStatuses(
                  statusTarget?.trangThaiDonHang || '',
                )"
                :key="next"
                @click="changeStatus(statusTarget!.maDonHang, next)"
                :disabled="changingStatus"
                :class="[
                  'w-full py-2.5 text-sm font-semibold rounded-lg transition disabled:opacity-50',
                  next === 'DaHuy'
                    ? 'bg-red-500 text-white hover:bg-red-600'
                    : 'bg-blue-600 text-white hover:bg-blue-700',
                ]"
              >
                {{
                  changingStatus ? "Đang xử lý..." : `→ ${statusLabel(next)}`
                }}
              </button>
            </div>
            <button
              @click="showStatusModal = false"
              class="w-full py-2 mt-3 text-sm font-medium text-gray-500 hover:bg-gray-100 rounded-lg transition"
            >
              Hủy
            </button>
          </div>
        </div>
      </transition></teleport
    >
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from "vue";
import {
  Search as SearchIcon,
  Eye as EyeIcon,
  RefreshCw as RefreshCwIcon,
  Loader2 as Loader2Icon,
  X as XIcon,
  Package as PackageIcon,
} from "lucide-vue-next";
import { donHangService } from "../../services/donhang.service";
import { useToast } from "../../composables/useToast";
import Toast from "../../components/admin/Toast.vue";

const { success, error } = useToast();
const list = ref<any[]>([]);
const loading = ref(true);
const searchQuery = ref("");
const filterStatus = ref("all");

// Detail
const showDetail = ref(false);
const detailLoading = ref(false);
const detail = ref<any>(null);

// Status change
const showStatusModal = ref(false);
const statusTarget = ref<any>(null);
const changingStatus = ref(false);

const statusFilters = [
  { value: "all", label: "Tất cả" },
  { value: "ChoXacNhan", label: "Chờ xác nhận" },
  { value: "DangVanChuyen", label: "Đang giao" },
  { value: "GiaoHangThanhCong", label: "Thành công" },
  { value: "DaHuy", label: "Đã hủy" },
];

const statusMap: Record<string, string> = {
  ChoXacNhan: "Chờ xác nhận",
  DangVanChuyen: "Đang vận chuyển",
  GiaoHangThanhCong: "Giao thành công",
  DaHuy: "Đã hủy",
};
const statusLabel = (s: string) => statusMap[s] || s;

const statusBadgeClass = (s: string) => ({
  "bg-yellow-50 text-yellow-700": s === "ChoXacNhan",
  "bg-blue-50 text-blue-700": s === "DangVanChuyen",
  "bg-emerald-50 text-emerald-700": s === "GiaoHangThanhCong",
  "bg-red-50 text-red-600": s === "DaHuy",
});

const canTransition = (s: string) =>
  s === "ChoXacNhan" || s === "DangVanChuyen";

const nextStatuses = (s: string): string[] => {
  if (s === "ChoXacNhan") return ["DangVanChuyen", "DaHuy"];
  if (s === "DangVanChuyen") return ["GiaoHangThanhCong", "DaHuy"];
  return [];
};

const filteredList = computed(() => {
  let result = list.value;
  if (filterStatus.value !== "all")
    result = result.filter((d) => d.trangThaiDonHang === filterStatus.value);
  if (searchQuery.value.trim()) {
    const q = searchQuery.value.trim().toLowerCase();
    result = result.filter((d) => d.maDonHang.toLowerCase().includes(q));
  }
  return result;
});

const formatMoney = (val: number) =>
  val ? val.toLocaleString("vi-VN") + " ₫" : "0 ₫";
const formatDate = (d: string) =>
  d
    ? new Date(d).toLocaleDateString("vi-VN", {
        day: "2-digit",
        month: "2-digit",
        year: "numeric",
        hour: "2-digit",
        minute: "2-digit",
      })
    : "—";

const fetchOrders = async () => {
  loading.value = true;
  try {
    const res: any = await donHangService.getAll();
    if (res.success) list.value = res.data || [];
  } catch {
    error("Lỗi tải danh sách đơn hàng");
  } finally {
    loading.value = false;
  }
};

const viewDetail = async (maDH: string) => {
  showDetail.value = true;
  detailLoading.value = true;
  detail.value = null;
  try {
    const res: any = await donHangService.getById(maDH);
    if (res.success) detail.value = res.data;
    else error(res.message);
  } catch {
    error("Lỗi tải chi tiết đơn");
  } finally {
    detailLoading.value = false;
  }
};

const openStatusModal = (dh: any) => {
  statusTarget.value = dh;
  showStatusModal.value = true;
};

const changeStatus = async (maDH: string, newStatus: string) => {
  changingStatus.value = true;
  try {
    const res: any = await donHangService.updateStatus(maDH, newStatus);
    if (res.success) {
      success(`Đã chuyển sang ${statusLabel(newStatus)}`);
      showStatusModal.value = false;
      // Refresh detail if open
      if (detail.value?.maDonHang === maDH) {
        detail.value.trangThaiDonHang = newStatus;
      }
      fetchOrders();
    } else error(res.message);
  } catch (e: any) {
    error(e.response?.data?.message || "Lỗi cập nhật trạng thái");
  } finally {
    changingStatus.value = false;
  }
};

onMounted(fetchOrders);
</script>

<style scoped>
.modal-enter-active {
  transition: all 0.2s ease-out;
}
.modal-leave-active {
  transition: all 0.15s ease-in;
}
.modal-enter-from,
.modal-leave-to {
  opacity: 0;
}
</style>
