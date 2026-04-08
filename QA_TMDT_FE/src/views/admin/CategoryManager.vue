<template>
  <div>
    <Toast />
    <!-- Header -->
    <div
      class="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-3 mb-6"
    >
      <div>
        <h3 class="text-xl font-bold text-[#1F2937]">Quản lý Danh mục</h3>
        <p class="text-sm text-gray-500 mt-0.5">
          Thêm, sửa, xóa danh mục sản phẩm.
        </p>
      </div>
      <button
        @click="openModal()"
        class="inline-flex items-center px-4 py-2.5 rounded-lg bg-blue-600 text-white text-sm font-semibold hover:bg-blue-700 shadow-sm transition"
      >
        <PlusIcon class="h-4 w-4 mr-1.5" stroke-width="2.5" /> Thêm danh mục
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
          @input="debouncedSearch"
          type="text"
          placeholder="Tìm theo tên hoặc mã danh mục..."
          class="w-full pl-9 pr-4 py-2.5 border border-gray-200 rounded-lg text-sm focus:outline-none focus:ring-2 focus:ring-blue-500/20 focus:border-blue-400"
        />
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
      <div class="overflow-x-auto">
        <table class="w-full">
          <thead>
            <tr
              class="text-left text-[11px] font-bold text-gray-400 uppercase tracking-wider border-b border-gray-100"
            >
              <th class="px-5 py-3 w-20">Mã</th>
              <th class="px-5 py-3">Tên danh mục</th>
              <th class="px-5 py-3">Mô tả</th>
              <th class="px-5 py-3 w-28">DM Cha</th>
              <th class="px-5 py-3 w-32 text-right">Thao tác</th>
            </tr>
          </thead>
          <tbody class="divide-y divide-gray-50">
            <tr
              v-for="dm in filteredList"
              :key="dm.maDanhMuc"
              class="hover:bg-gray-50/50 transition-colors"
            >
              <td class="px-5 py-3 text-sm text-gray-400 font-mono">
                #{{ dm.maDanhMuc }}
              </td>
              <td class="px-5 py-3 text-sm font-semibold text-gray-800">
                {{ dm.tenDanhMuc }}
              </td>
              <td class="px-5 py-3 text-sm text-gray-500 max-w-xs truncate">
                {{ dm.moTa || "—" }}
              </td>
              <td class="px-5 py-3 text-sm text-gray-500">
                {{ getParentName(dm.maDanhMucCha) }}
              </td>
              <td class="px-5 py-3 text-right">
                <button
                  @click="openModal(dm)"
                  class="p-1.5 rounded-lg text-gray-400 hover:text-blue-600 hover:bg-blue-50 transition-colors mr-1"
                  title="Sửa"
                >
                  <PencilIcon class="h-4 w-4" stroke-width="2" />
                </button>
                <button
                  @click="confirmDelete(dm)"
                  class="p-1.5 rounded-lg text-gray-400 hover:text-red-500 hover:bg-red-50 transition-colors"
                  title="Xóa"
                >
                  <Trash2Icon class="h-4 w-4" stroke-width="2" />
                </button>
              </td>
            </tr>
            <tr v-if="filteredList.length === 0">
              <td
                colspan="5"
                class="px-5 py-10 text-center text-gray-400 text-sm"
              >
                Không tìm thấy danh mục nào.
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
          <div class="bg-white rounded-xl shadow-2xl w-full max-w-md p-6">
            <h4 class="text-lg font-bold text-gray-800 mb-5">
              {{ editing ? "Sửa danh mục" : "Thêm danh mục" }}
            </h4>
            <div class="space-y-4">
              <div>
                <label
                  class="block text-xs font-bold text-gray-500 uppercase mb-1.5"
                  >Tên danh mục *</label
                >
                <input
                  v-model="form.tenDanhMuc"
                  type="text"
                  placeholder="VD: Áo, Quần, Váy..."
                  class="w-full px-3.5 py-2.5 border border-gray-200 rounded-lg text-sm focus:outline-none focus:ring-2 focus:ring-blue-500/20 focus:border-blue-400"
                />
              </div>
              <div>
                <label
                  class="block text-xs font-bold text-gray-500 uppercase mb-1.5"
                  >Mô tả</label
                >
                <textarea
                  v-model="form.moTa"
                  rows="2"
                  placeholder="Mô tả ngắn..."
                  class="w-full px-3.5 py-2.5 border border-gray-200 rounded-lg text-sm focus:outline-none focus:ring-2 focus:ring-blue-500/20 focus:border-blue-400 resize-none"
                ></textarea>
              </div>
              <div>
                <label
                  class="block text-xs font-bold text-gray-500 uppercase mb-1.5"
                  >Danh mục cha</label
                >
                <select
                  v-model="form.maDanhMucCha"
                  class="w-full px-3.5 py-2.5 border border-gray-200 rounded-lg text-sm focus:outline-none focus:ring-2 focus:ring-blue-500/20 focus:border-blue-400"
                >
                  <option :value="null">-- Không có (gốc) --</option>
                  <option
                    v-for="p in parentOptions"
                    :key="p.maDanhMuc"
                    :value="p.maDanhMuc"
                  >
                    {{ p.tenDanhMuc }}
                  </option>
                </select>
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
            <h4 class="text-lg font-bold text-gray-800 mb-1">Xác nhận xóa</h4>
            <p class="text-sm text-gray-500">
              Bạn có chắc muốn xóa danh mục
              <strong class="text-gray-700">{{
                deleteTarget?.tenDanhMuc
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
import { danhMucService } from "../../services/danhmuc.service";
import { useToast } from "../../composables/useToast";
import Toast from "../../components/admin/Toast.vue";

const { success, error } = useToast();
const list = ref<any[]>([]);
const loading = ref(true);
const saving = ref(false);
const showModal = ref(false);
const showDelete = ref(false);
const editing = ref(false);
const editId = ref<number | null>(null);
const deleteTarget = ref<any>(null);
const searchQuery = ref("");
let searchTimer: any = null;

const form = ref({
  tenDanhMuc: "",
  moTa: "",
  maDanhMucCha: null as number | null,
});

// Filter locally by both name and ID
const filteredList = computed(() => {
  if (!searchQuery.value.trim()) return list.value;
  const q = searchQuery.value.trim().toLowerCase();
  return list.value.filter(
    (dm) =>
      dm.tenDanhMuc.toLowerCase().includes(q) ||
      String(dm.maDanhMuc).includes(q),
  );
});

// Parent options (exclude self when editing)
const parentOptions = computed(() => {
  if (!editing.value) return list.value;
  return list.value.filter((dm) => dm.maDanhMuc !== editId.value);
});

const getParentName = (maCha: number | null) => {
  if (!maCha) return "—";
  const p = list.value.find((dm) => dm.maDanhMuc === maCha);
  return p?.tenDanhMuc || `#${maCha}`;
};

const debouncedSearch = () => {
  clearTimeout(searchTimer);
  searchTimer = setTimeout(() => {}, 300);
};

const fetchAll = async () => {
  loading.value = true;
  try {
    const res: any = await danhMucService.getAll();
    if (res.success) list.value = res.data || [];
  } catch {
    error("Lỗi tải danh mục");
  } finally {
    loading.value = false;
  }
};

const openModal = (item?: any) => {
  if (item) {
    editing.value = true;
    editId.value = item.maDanhMuc;
    form.value = {
      tenDanhMuc: item.tenDanhMuc,
      moTa: item.moTa || "",
      maDanhMucCha: item.maDanhMucCha || null,
    };
  } else {
    editing.value = false;
    editId.value = null;
    form.value = { tenDanhMuc: "", moTa: "", maDanhMucCha: null };
  }
  showModal.value = true;
};

const handleSave = async () => {
  if (!form.value.tenDanhMuc.trim()) {
    error("Tên danh mục không được để trống");
    return;
  }
  saving.value = true;
  try {
    const payload = {
      tenDanhMuc: form.value.tenDanhMuc,
      moTa: form.value.moTa || undefined,
      maDanhMucCha: form.value.maDanhMucCha || undefined,
    };
    const res: any =
      editing.value && editId.value !== null
        ? await danhMucService.update(editId.value, payload)
        : await danhMucService.create(payload);
    if (res.success) {
      success(
        editing.value
          ? "Cập nhật danh mục thành công"
          : "Thêm danh mục thành công",
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

const confirmDelete = (dm: any) => {
  deleteTarget.value = dm;
  showDelete.value = true;
};

const handleDelete = async () => {
  if (!deleteTarget.value) return;
  saving.value = true;
  try {
    const res: any = await danhMucService.delete(deleteTarget.value.maDanhMuc);
    if (res.success !== false) {
      success("Xóa danh mục thành công");
      showDelete.value = false;
      fetchAll();
    } else error(res.message);
  } catch (e: any) {
    error(e.response?.data?.message || e.response?.data || "Không thể xóa");
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
