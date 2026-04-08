<template>
  <div>
    <Toast />
    <div
      class="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-3 mb-6"
    >
      <div>
        <h3 class="text-xl font-bold text-[#1F2937]">Quản lý Voucher</h3>
        <p class="text-sm text-gray-500 mt-0.5">
          Thêm, sửa, xóa mã giảm giá (voucher).
        </p>
      </div>
      <button
        @click="openModal()"
        class="inline-flex items-center px-4 py-2.5 rounded-lg bg-blue-600 text-white text-sm font-semibold hover:bg-blue-700 shadow-sm transition"
      >
        <PlusIcon class="h-4 w-4 mr-1.5" stroke-width="2.5" /> Thêm voucher
      </button>
    </div>

    <!-- Search -->
    <div class="bg-white rounded-xl border border-gray-200 p-4 mb-5">
      <div class="relative">
        <SearchIcon
          class="absolute left-3 top-1/2 -translate-y-1/2 h-4 w-4 text-gray-400"
          stroke-width="2"
        />
        <input
          v-model="searchQuery"
          type="text"
          placeholder="Tìm theo mã hoặc tên voucher..."
          class="w-full pl-9 pr-4 py-2.5 border border-gray-200 rounded-lg text-sm focus:outline-none focus:ring-2 focus:ring-blue-500/20 focus:border-blue-400"
        />
      </div>
    </div>

    <div v-if="loading" class="flex justify-center py-16 text-gray-400">
      <Loader2Icon class="h-7 w-7 animate-spin" />
    </div>

    <div
      v-else
      class="bg-white rounded-xl border border-gray-200 overflow-hidden"
    >
      <div class="overflow-x-auto">
        <table class="w-full">
          <thead>
            <tr
              class="text-left text-[11px] font-bold text-gray-400 uppercase tracking-wider border-b border-gray-100"
            >
              <th class="px-5 py-3">Mã voucher</th>
              <th class="px-5 py-3">Tên</th>
              <th class="px-5 py-3">Giảm giá</th>
              <th class="px-5 py-3">Điều kiện</th>
              <th class="px-5 py-3">Thời gian</th>
              <th class="px-5 py-3 w-20">TT</th>
              <th class="px-5 py-3 w-28 text-right">Thao tác</th>
            </tr>
          </thead>
          <tbody class="divide-y divide-gray-50">
            <tr
              v-for="v in filteredList"
              :key="v.maVoucher"
              class="hover:bg-gray-50/50 transition-colors"
            >
              <td
                class="px-5 py-3 text-sm font-mono font-semibold text-blue-600"
              >
                {{ v.maVoucher }}
              </td>
              <td class="px-5 py-3 text-sm font-medium text-gray-800">
                {{ v.tenVoucher || "—" }}
              </td>
              <td class="px-5 py-3 text-sm font-bold text-emerald-600">
                {{ formatMoney(v.giamGia) }}
              </td>
              <td class="px-5 py-3 text-sm text-gray-500">
                {{
                  v.dieuKienApDung ? `≥ ${formatMoney(v.dieuKienApDung)}` : "—"
                }}
              </td>
              <td class="px-5 py-3 text-xs text-gray-500">
                {{ fmtDate(v.ngayBatDau) }} → {{ fmtDate(v.ngayKetThuc) }}
              </td>
              <td class="px-5 py-3">
                <span
                  :class="
                    v.trangThai === 'Active' || v.trangThai === 'Đang hoạt động'
                      ? 'bg-emerald-50 text-emerald-700'
                      : 'bg-gray-100 text-gray-500'
                  "
                  class="text-[10px] font-bold px-2 py-0.5 rounded-full"
                  >{{ v.trangThai }}</span
                >
              </td>
              <td class="px-5 py-3 text-right">
                <button
                  @click="openModal(v)"
                  class="p-1.5 rounded-lg text-gray-400 hover:text-blue-600 hover:bg-blue-50 transition-colors mr-1"
                >
                  <PencilIcon class="h-4 w-4" stroke-width="2" />
                </button>
                <button
                  @click="confirmDelete(v)"
                  class="p-1.5 rounded-lg text-gray-400 hover:text-red-500 hover:bg-red-50 transition-colors"
                >
                  <Trash2Icon class="h-4 w-4" stroke-width="2" />
                </button>
              </td>
            </tr>
            <tr v-if="filteredList.length === 0">
              <td
                colspan="7"
                class="px-5 py-10 text-center text-gray-400 text-sm"
              >
                Không tìm thấy voucher nào.
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>

    <!-- Add/Edit Modal -->
    <teleport to="body"
      ><transition name="modal">
        <div
          v-if="showModal"
          class="fixed inset-0 z-50 flex items-center justify-center p-4 bg-black/40"
          @click.self="showModal = false"
        >
          <div
            class="bg-white rounded-xl shadow-2xl w-full max-w-lg p-6 max-h-[90vh] overflow-y-auto"
          >
            <h4 class="text-lg font-bold text-gray-800 mb-5">
              {{ editing ? "Sửa voucher" : "Thêm voucher" }}
            </h4>
            <div class="space-y-4">
              <div>
                <label
                  class="block text-xs font-bold text-gray-500 uppercase mb-1.5"
                  >Mã voucher *</label
                >
                <input
                  v-model="form.maVoucher"
                  type="text"
                  :disabled="editing"
                  placeholder="VD: GIAM20K"
                  class="w-full px-3.5 py-2.5 border border-gray-200 rounded-lg text-sm font-mono focus:outline-none focus:ring-2 focus:ring-blue-500/20 focus:border-blue-400 disabled:bg-gray-50"
                />
              </div>
              <div>
                <label
                  class="block text-xs font-bold text-gray-500 uppercase mb-1.5"
                  >Tên voucher</label
                >
                <input
                  v-model="form.tenVoucher"
                  type="text"
                  placeholder="VD: Giảm 20k cho đơn 200k"
                  class="w-full px-3.5 py-2.5 border border-gray-200 rounded-lg text-sm focus:outline-none focus:ring-2 focus:ring-blue-500/20 focus:border-blue-400"
                />
              </div>
              <div class="grid grid-cols-2 gap-4">
                <div>
                  <label
                    class="block text-xs font-bold text-gray-500 uppercase mb-1.5"
                    >Giảm giá (₫) *</label
                  >
                  <input
                    v-model.number="form.giamGia"
                    type="number"
                    min="0"
                    class="w-full px-3.5 py-2.5 border border-gray-200 rounded-lg text-sm focus:outline-none focus:ring-2 focus:ring-blue-500/20 focus:border-blue-400"
                  />
                </div>
                <div>
                  <label
                    class="block text-xs font-bold text-gray-500 uppercase mb-1.5"
                    >Điều kiện (₫)</label
                  >
                  <input
                    v-model.number="form.dieuKienApDung"
                    type="number"
                    min="0"
                    placeholder="Đơn tối thiểu"
                    class="w-full px-3.5 py-2.5 border border-gray-200 rounded-lg text-sm focus:outline-none focus:ring-2 focus:ring-blue-500/20 focus:border-blue-400"
                  />
                </div>
              </div>
              <div class="grid grid-cols-2 gap-4">
                <div>
                  <label
                    class="block text-xs font-bold text-gray-500 uppercase mb-1.5"
                    >Ngày bắt đầu *</label
                  >
                  <input
                    v-model="form.ngayBatDau"
                    type="datetime-local"
                    class="w-full px-3.5 py-2.5 border border-gray-200 rounded-lg text-sm focus:outline-none focus:ring-2 focus:ring-blue-500/20 focus:border-blue-400"
                  />
                </div>
                <div>
                  <label
                    class="block text-xs font-bold text-gray-500 uppercase mb-1.5"
                    >Ngày kết thúc *</label
                  >
                  <input
                    v-model="form.ngayKetThuc"
                    type="datetime-local"
                    class="w-full px-3.5 py-2.5 border border-gray-200 rounded-lg text-sm focus:outline-none focus:ring-2 focus:ring-blue-500/20 focus:border-blue-400"
                  />
                </div>
              </div>
              <div>
                <label
                  class="block text-xs font-bold text-gray-500 uppercase mb-1.5"
                  >Mô tả</label
                >
                <textarea
                  v-model="form.moTa"
                  rows="2"
                  placeholder="Mô tả voucher..."
                  class="w-full px-3.5 py-2.5 border border-gray-200 rounded-lg text-sm focus:outline-none focus:ring-2 focus:ring-blue-500/20 focus:border-blue-400 resize-none"
                ></textarea>
              </div>
            </div>
            <div class="flex justify-end space-x-2 mt-6">
              <button
                @click="showModal = false"
                class="px-4 py-2 text-sm font-medium text-gray-600 hover:bg-gray-100 rounded-lg transition"
              >
                Hủy
              </button>
              <button
                @click="handleSave"
                :disabled="saving"
                class="px-5 py-2 text-sm font-semibold text-white bg-blue-600 hover:bg-blue-700 rounded-lg shadow-sm transition disabled:opacity-50"
              >
                {{ saving ? "Đang lưu..." : editing ? "Cập nhật" : "Thêm mới" }}
              </button>
            </div>
          </div>
        </div>
      </transition></teleport
    >

    <!-- Delete Confirm -->
    <teleport to="body"
      ><transition name="modal">
        <div
          v-if="showDelete"
          class="fixed inset-0 z-50 flex items-center justify-center p-4 bg-black/40"
          @click.self="showDelete = false"
        >
          <div
            class="bg-white rounded-xl shadow-2xl w-full max-w-sm p-6 text-center"
          >
            <Trash2Icon
              class="h-10 w-10 text-red-400 mx-auto mb-3"
              stroke-width="1.5"
            />
            <h4 class="text-lg font-bold text-gray-800 mb-1">Xóa voucher</h4>
            <p class="text-sm text-gray-500">
              Bạn có chắc muốn xóa
              <strong class="text-gray-700">{{
                deleteTarget?.maVoucher
              }}</strong
              >?
            </p>
            <div class="flex justify-center space-x-2 mt-5">
              <button
                @click="showDelete = false"
                class="px-4 py-2 text-sm font-medium text-gray-600 hover:bg-gray-100 rounded-lg transition"
              >
                Hủy
              </button>
              <button
                @click="handleDelete"
                :disabled="saving"
                class="px-5 py-2 text-sm font-semibold text-white bg-red-500 hover:bg-red-600 rounded-lg shadow-sm transition disabled:opacity-50"
              >
                {{ saving ? "Đang xóa..." : "Xóa" }}
              </button>
            </div>
          </div>
        </div>
      </transition></teleport
    >
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from "vue";
import {
  Plus as PlusIcon,
  Pencil as PencilIcon,
  Trash2 as Trash2Icon,
  Search as SearchIcon,
  Loader2 as Loader2Icon,
} from "lucide-vue-next";
import { voucherService } from "../../services/voucher.service";
import { useToast } from "../../composables/useToast";
import Toast from "../../components/admin/Toast.vue";

const { success, error } = useToast();
const list = ref<any[]>([]);
const loading = ref(true);
const saving = ref(false);
const showModal = ref(false);
const showDelete = ref(false);
const editing = ref(false);
const deleteTarget = ref<any>(null);
const searchQuery = ref("");

const form = ref({
  maVoucher: "",
  tenVoucher: "",
  giamGia: 0,
  ngayBatDau: "",
  ngayKetThuc: "",
  dieuKienApDung: 0,
  moTa: "",
});

const filteredList = computed(() => {
  if (!searchQuery.value.trim()) return list.value;
  const q = searchQuery.value.trim().toLowerCase();
  return list.value.filter(
    (v) =>
      v.maVoucher.toLowerCase().includes(q) ||
      (v.tenVoucher || "").toLowerCase().includes(q),
  );
});

const formatMoney = (val: number) =>
  val ? val.toLocaleString("vi-VN") + " ₫" : "0 ₫";
const fmtDate = (d: string) =>
  d ? new Date(d).toLocaleDateString("vi-VN") : "—";
const toLocal = (d: string) =>
  d ? new Date(d).toISOString().slice(0, 16) : "";

const fetchAll = async () => {
  loading.value = true;
  try {
    const r: any = await voucherService.getAll();
    if (r.success) list.value = r.data || [];
  } catch {
    error("Lỗi tải voucher");
  } finally {
    loading.value = false;
  }
};

const openModal = (item?: any) => {
  if (item) {
    editing.value = true;
    form.value = {
      maVoucher: item.maVoucher,
      tenVoucher: item.tenVoucher || "",
      giamGia: item.giamGia,
      ngayBatDau: toLocal(item.ngayBatDau),
      ngayKetThuc: toLocal(item.ngayKetThuc),
      dieuKienApDung: item.dieuKienApDung || 0,
      moTa: item.moTa || "",
    };
  } else {
    editing.value = false;
    form.value = {
      maVoucher: "",
      tenVoucher: "",
      giamGia: 0,
      ngayBatDau: "",
      ngayKetThuc: "",
      dieuKienApDung: 0,
      moTa: "",
    };
  }
  showModal.value = true;
};

const handleSave = async () => {
  if (!form.value.maVoucher.trim()) {
    error("Mã voucher không được để trống");
    return;
  }
  if (!form.value.ngayBatDau || !form.value.ngayKetThuc) {
    error("Phải chọn thời gian");
    return;
  }
  saving.value = true;
  try {
    const payload = {
      ...form.value,
      dieuKienApDung: form.value.dieuKienApDung || undefined,
      moTa: form.value.moTa || undefined,
    };
    const res: any = editing.value
      ? await voucherService.update(payload)
      : await voucherService.create(payload);
    if (res.success) {
      success(
        editing.value
          ? "Cập nhật voucher thành công"
          : "Thêm voucher thành công",
      );
      showModal.value = false;
      fetchAll();
    } else error(res.message);
  } catch (e: any) {
    error(e.response?.data?.message || "Lỗi thao tác");
  } finally {
    saving.value = false;
  }
};

const confirmDelete = (v: any) => {
  deleteTarget.value = v;
  showDelete.value = true;
};
const handleDelete = async () => {
  if (!deleteTarget.value) return;
  saving.value = true;
  try {
    const res: any = await voucherService.delete(deleteTarget.value.maVoucher);
    if (res.success !== false) {
      success("Xóa voucher thành công");
      showDelete.value = false;
      fetchAll();
    } else error(res.message);
  } catch (e: any) {
    error(e.response?.data?.message || e.response?.data || "Lỗi xóa");
  } finally {
    saving.value = false;
  }
};

onMounted(fetchAll);
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
