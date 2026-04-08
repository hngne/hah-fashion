<template>
  <div>
    <Toast />
    <div
      class="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-3 mb-6"
    >
      <div>
        <h3 class="text-xl font-bold text-[#1F2937]">Quản lý Khuyến mãi</h3>
        <p class="text-sm text-gray-500 mt-0.5">
          Quản lý chương trình khuyến mãi và sản phẩm áp dụng.
        </p>
      </div>
      <button
        @click="openModal()"
        class="inline-flex items-center px-4 py-2.5 rounded-lg bg-blue-600 text-white text-sm font-semibold hover:bg-blue-700 shadow-sm transition"
      >
        <PlusIcon class="h-4 w-4 mr-1.5" stroke-width="2.5" /> Thêm KM
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
          placeholder="Tìm theo mã hoặc tên khuyến mãi..."
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
              <th class="px-5 py-3">Mã KM</th>
              <th class="px-5 py-3">Tên</th>
              <th class="px-5 py-3">Thời gian</th>
              <th class="px-5 py-3 w-20">TT</th>
              <th class="px-5 py-3 w-20">Chi tiết</th>
              <th class="px-5 py-3 w-28 text-right">Thao tác</th>
            </tr>
          </thead>
          <tbody class="divide-y divide-gray-50">
            <tr
              v-for="km in filteredList"
              :key="km.maKhuyenMai"
              class="hover:bg-gray-50/50 transition-colors"
            >
              <td
                class="px-5 py-3 text-sm font-mono font-semibold text-blue-600"
              >
                {{ km.maKhuyenMai }}
              </td>
              <td class="px-5 py-3 text-sm font-medium text-gray-800">
                {{ km.tenKhuyenMai }}
              </td>
              <td class="px-5 py-3 text-xs text-gray-500">
                {{ fmtDate(km.ngayBatDau) }} → {{ fmtDate(km.ngayKetThuc) }}
              </td>
              <td class="px-5 py-3">
                <span
                  :class="
                    km.trangThai === 'Active' ||
                    km.trangThai === 'Đang hoạt động'
                      ? 'bg-emerald-50 text-emerald-700'
                      : 'bg-gray-100 text-gray-500'
                  "
                  class="text-[10px] font-bold px-2 py-0.5 rounded-full"
                  >{{ km.trangThai }}</span
                >
              </td>
              <td class="px-5 py-3">
                <button
                  @click="viewChiTiet(km)"
                  class="text-xs font-semibold text-blue-600 hover:text-blue-700 transition"
                >
                  Xem SP
                </button>
              </td>
              <td class="px-5 py-3 text-right">
                <button
                  @click="openModal(km)"
                  class="p-1.5 rounded-lg text-gray-400 hover:text-blue-600 hover:bg-blue-50 transition-colors mr-1"
                >
                  <PencilIcon class="h-4 w-4" stroke-width="2" />
                </button>
                <button
                  @click="confirmDelete(km)"
                  class="p-1.5 rounded-lg text-gray-400 hover:text-red-500 hover:bg-red-50 transition-colors"
                >
                  <Trash2Icon class="h-4 w-4" stroke-width="2" />
                </button>
              </td>
            </tr>
            <tr v-if="filteredList.length === 0">
              <td
                colspan="6"
                class="px-5 py-10 text-center text-gray-400 text-sm"
              >
                Không tìm thấy khuyến mãi nào.
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>

    <!-- Add/Edit KM Modal -->
    <teleport to="body"
      ><transition name="modal">
        <div
          v-if="showModal"
          class="fixed inset-0 z-50 flex items-center justify-center p-4 bg-black/40"
          @click.self="showModal = false"
        >
          <div class="bg-white rounded-xl shadow-2xl w-full max-w-lg p-6">
            <h4 class="text-lg font-bold text-gray-800 mb-5">
              {{ editing ? "Sửa khuyến mãi" : "Thêm khuyến mãi" }}
            </h4>
            <div class="space-y-4">
              <div class="grid grid-cols-2 gap-4">
                <div>
                  <label
                    class="block text-xs font-bold text-gray-500 uppercase mb-1.5"
                    >Mã KM *</label
                  >
                  <input
                    v-model="form.maKhuyenMai"
                    type="text"
                    :disabled="editing"
                    placeholder="VD: KM001"
                    class="w-full px-3.5 py-2.5 border border-gray-200 rounded-lg text-sm font-mono focus:outline-none focus:ring-2 focus:ring-blue-500/20 disabled:bg-gray-50"
                  />
                </div>
                <div>
                  <label
                    class="block text-xs font-bold text-gray-500 uppercase mb-1.5"
                    >Tên KM *</label
                  >
                  <input
                    v-model="form.tenKhuyenMai"
                    type="text"
                    placeholder="VD: Sale hè 2026"
                    class="w-full px-3.5 py-2.5 border border-gray-200 rounded-lg text-sm focus:outline-none focus:ring-2 focus:ring-blue-500/20"
                  />
                </div>
              </div>
              <div class="grid grid-cols-2 gap-4">
                <div>
                  <label
                    class="block text-xs font-bold text-gray-500 uppercase mb-1.5"
                    >Bắt đầu *</label
                  >
                  <input
                    v-model="form.ngayBatDau"
                    type="datetime-local"
                    class="w-full px-3.5 py-2.5 border border-gray-200 rounded-lg text-sm focus:outline-none focus:ring-2 focus:ring-blue-500/20"
                  />
                </div>
                <div>
                  <label
                    class="block text-xs font-bold text-gray-500 uppercase mb-1.5"
                    >Kết thúc *</label
                  >
                  <input
                    v-model="form.ngayKetThuc"
                    type="datetime-local"
                    class="w-full px-3.5 py-2.5 border border-gray-200 rounded-lg text-sm focus:outline-none focus:ring-2 focus:ring-blue-500/20"
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
                  placeholder="Mô tả..."
                  class="w-full px-3.5 py-2.5 border border-gray-200 rounded-lg text-sm focus:outline-none focus:ring-2 focus:ring-blue-500/20 resize-none"
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

    <!-- Chi tiết KM Modal: sản phẩm áp dụng -->
    <teleport to="body"
      ><transition name="modal">
        <div
          v-if="showChiTiet"
          class="fixed inset-0 z-50 flex items-center justify-center p-4 bg-black/40 overflow-y-auto"
          @click.self="showChiTiet = false"
        >
          <div
            class="bg-white rounded-xl shadow-2xl w-full max-w-xl my-8 max-h-[90vh] overflow-y-auto"
          >
            <div
              class="sticky top-0 bg-white px-6 py-4 border-b border-gray-100 flex items-center justify-between z-10"
            >
              <h4 class="text-lg font-bold text-gray-800">
                SP áp dụng: {{ chiTietKM?.maKhuyenMai }}
              </h4>
              <button
                @click="showChiTiet = false"
                class="p-1.5 rounded-lg text-gray-400 hover:bg-gray-100"
              >
                <XIcon class="h-5 w-5" />
              </button>
            </div>
            <div class="p-6">
              <div
                v-if="chiTietLoading"
                class="flex justify-center py-8 text-gray-400"
              >
                <Loader2Icon class="h-6 w-6 animate-spin" />
              </div>
              <template v-else>
                <div
                  class="overflow-x-auto border border-gray-200 rounded-lg mb-4"
                >
                  <table class="w-full text-sm">
                    <thead>
                      <tr
                        class="text-[10px] font-bold text-gray-400 uppercase bg-gray-50"
                      >
                        <th class="px-4 py-2 text-left">Mã SP</th>
                        <th class="px-4 py-2 text-left">Tên SP</th>
                        <th class="px-4 py-2 text-right">% Giảm</th>
                        <th class="px-4 py-2 w-16 text-right">Xóa</th>
                      </tr>
                    </thead>
                    <tbody class="divide-y divide-gray-50">
                      <tr
                        v-for="ct in chiTietList"
                        :key="ct.maSp"
                        class="hover:bg-gray-50/50"
                      >
                        <td class="px-4 py-2 font-mono text-xs text-blue-600">
                          {{ ct.maSp }}
                        </td>
                        <td class="px-4 py-2 font-medium">{{ ct.tenSP }}</td>
                        <td class="px-4 py-2 text-right font-bold text-red-500">
                          -{{ ct.phanTramGiam }}%
                        </td>
                        <td class="px-4 py-2 text-right">
                          <button
                            @click="deleteChiTiet(ct)"
                            class="p-1 rounded text-gray-400 hover:text-red-500"
                          >
                            <Trash2Icon class="h-3.5 w-3.5" stroke-width="2" />
                          </button>
                        </td>
                      </tr>
                      <tr v-if="chiTietList.length === 0">
                        <td
                          colspan="4"
                          class="px-4 py-4 text-center text-gray-400 text-sm"
                        >
                          Chưa có SP áp dụng.
                        </td>
                      </tr>
                    </tbody>
                  </table>
                </div>
                <!-- Add SP to KM -->
                <div class="p-3 bg-gray-50 rounded-lg">
                  <p class="text-xs font-bold text-gray-500 uppercase mb-2">
                    Thêm SP áp dụng
                  </p>
                  <div class="flex items-end space-x-2">
                    <div class="flex-1">
                      <input
                        v-model="addCtForm.maSp"
                        type="text"
                        placeholder="Mã SP"
                        class="w-full px-3 py-2 border border-gray-200 rounded-lg text-xs focus:outline-none focus:ring-2 focus:ring-blue-500/20"
                      />
                    </div>
                    <div class="w-24">
                      <input
                        v-model.number="addCtForm.phanTramGiam"
                        type="number"
                        min="1"
                        max="100"
                        placeholder="%"
                        class="w-full px-3 py-2 border border-gray-200 rounded-lg text-xs focus:outline-none focus:ring-2 focus:ring-blue-500/20"
                      />
                    </div>
                    <button
                      @click="addChiTiet"
                      :disabled="savingCt"
                      class="px-3 py-2 text-xs font-semibold text-white bg-blue-600 hover:bg-blue-700 rounded-lg transition disabled:opacity-50"
                    >
                      {{ savingCt ? "..." : "Thêm" }}
                    </button>
                  </div>
                </div>
              </template>
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
            <h4 class="text-lg font-bold text-gray-800 mb-1">Xóa khuyến mãi</h4>
            <p class="text-sm text-gray-500">
              Bạn có chắc muốn xóa
              <strong class="text-gray-700">{{
                deleteTarget?.maKhuyenMai
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
                {{ saving ? "Xóa..." : "Xóa" }}
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
  X as XIcon,
} from "lucide-vue-next";
import { khuyenMaiService } from "../../services/khuyenmai.service";
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
  maKhuyenMai: "",
  tenKhuyenMai: "",
  ngayBatDau: "",
  ngayKetThuc: "",
  moTa: "",
});

// Chi tiết KM
const showChiTiet = ref(false);
const chiTietKM = ref<any>(null);
const chiTietList = ref<any[]>([]);
const chiTietLoading = ref(false);
const savingCt = ref(false);
const addCtForm = ref({ maSp: "", phanTramGiam: 10 });

const filteredList = computed(() => {
  if (!searchQuery.value.trim()) return list.value;
  const q = searchQuery.value.trim().toLowerCase();
  return list.value.filter(
    (k) =>
      k.maKhuyenMai.toLowerCase().includes(q) ||
      k.tenKhuyenMai.toLowerCase().includes(q),
  );
});

const fmtDate = (d: string) =>
  d ? new Date(d).toLocaleDateString("vi-VN") : "—";
const toLocal = (d: string) =>
  d ? new Date(d).toISOString().slice(0, 16) : "";

const fetchAll = async () => {
  loading.value = true;
  try {
    const r: any = await khuyenMaiService.getAll();
    if (r.success) list.value = r.data || [];
  } catch {
    error("Lỗi tải KM");
  } finally {
    loading.value = false;
  }
};

const openModal = (item?: any) => {
  if (item) {
    editing.value = true;
    form.value = {
      maKhuyenMai: item.maKhuyenMai,
      tenKhuyenMai: item.tenKhuyenMai,
      ngayBatDau: toLocal(item.ngayBatDau),
      ngayKetThuc: toLocal(item.ngayKetThuc),
      moTa: item.moTa || "",
    };
  } else {
    editing.value = false;
    form.value = {
      maKhuyenMai: "",
      tenKhuyenMai: "",
      ngayBatDau: "",
      ngayKetThuc: "",
      moTa: "",
    };
  }
  showModal.value = true;
};

const handleSave = async () => {
  if (!form.value.maKhuyenMai.trim() || !form.value.tenKhuyenMai.trim()) {
    error("Mã và Tên KM không được để trống");
    return;
  }
  if (!form.value.ngayBatDau || !form.value.ngayKetThuc) {
    error("Phải chọn thời gian");
    return;
  }
  saving.value = true;
  try {
    const payload = { ...form.value, moTa: form.value.moTa || undefined };
    const res: any = editing.value
      ? await khuyenMaiService.update(payload)
      : await khuyenMaiService.create(payload);
    if (res.success) {
      success(editing.value ? "Cập nhật KM thành công" : "Thêm KM thành công");
      showModal.value = false;
      fetchAll();
    } else error(res.message);
  } catch (e: any) {
    error(e.response?.data?.message || "Lỗi");
  } finally {
    saving.value = false;
  }
};

const confirmDelete = (km: any) => {
  deleteTarget.value = km;
  showDelete.value = true;
};
const handleDelete = async () => {
  if (!deleteTarget.value) return;
  saving.value = true;
  try {
    const res: any = await khuyenMaiService.delete(
      deleteTarget.value.maKhuyenMai,
    );
    if (res.success !== false) {
      success("Xóa KM thành công");
      showDelete.value = false;
      fetchAll();
    } else error(res.message);
  } catch (e: any) {
    error(e.response?.data?.message || e.response?.data || "Lỗi xóa");
  } finally {
    saving.value = false;
  }
};

// ===== Chi tiết KM (SP áp dụng) =====
const viewChiTiet = async (km: any) => {
  chiTietKM.value = km;
  showChiTiet.value = true;
  chiTietLoading.value = true;
  addCtForm.value = { maSp: "", phanTramGiam: 10 };
  try {
    const r: any = await khuyenMaiService.getChiTietByMaKM(km.maKhuyenMai);
    if (r.success) chiTietList.value = r.data || [];
  } catch {
    error("Lỗi tải chi tiết KM");
  } finally {
    chiTietLoading.value = false;
  }
};

const addChiTiet = async () => {
  if (!addCtForm.value.maSp.trim() || !addCtForm.value.phanTramGiam) {
    error("Nhập mã SP và % giảm");
    return;
  }
  savingCt.value = true;
  try {
    const res: any = await khuyenMaiService.addChiTiet({
      maKhuyenMai: chiTietKM.value.maKhuyenMai,
      maSp: addCtForm.value.maSp,
      phanTramGiam: addCtForm.value.phanTramGiam,
    });
    if (res.success) {
      success("Thêm SP vào KM thành công");
      addCtForm.value = { maSp: "", phanTramGiam: 10 };
      viewChiTiet(chiTietKM.value);
    } else error(res.message);
  } catch (e: any) {
    error(e.response?.data?.message || "Lỗi thêm");
  } finally {
    savingCt.value = false;
  }
};

const deleteChiTiet = async (ct: any) => {
  try {
    const res: any = await khuyenMaiService.deleteChiTiet(
      chiTietKM.value.maKhuyenMai,
      ct.maSp,
    );
    if (res.success !== false) {
      success("Đã xóa SP khỏi KM");
      viewChiTiet(chiTietKM.value);
    } else error(res.message);
  } catch (e: any) {
    error(e.response?.data?.message || e.response?.data || "Lỗi xóa");
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
