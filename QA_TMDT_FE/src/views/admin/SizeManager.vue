<template>
  <div>
    <Toast />
    <!-- Header -->
    <div
      class="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-3 mb-6"
    >
      <div>
        <h3 class="text-xl font-bold text-[#1F2937]">Quản lý Kích thước</h3>
        <p class="text-sm text-gray-500 mt-0.5">
          Thêm, sửa, xóa các kích thước (size) cho biến thể sản phẩm.
        </p>
      </div>
      <button
        @click="openModal()"
        class="inline-flex items-center px-4 py-2.5 rounded-lg bg-blue-600 text-white text-sm font-semibold hover:bg-blue-700 shadow-sm transition"
      >
        <PlusIcon class="h-4 w-4 mr-1.5" stroke-width="2.5" /> Thêm size
      </button>
    </div>

    <!-- Loading -->
    <div v-if="loading" class="flex justify-center py-16 text-gray-400">
      <Loader2Icon class="h-7 w-7 animate-spin" />
    </div>

    <!-- Grid Cards for sizes -->
    <div
      v-else
      class="grid grid-cols-2 sm:grid-cols-3 md:grid-cols-4 lg:grid-cols-6 gap-3"
    >
      <div
        v-for="(s, idx) in list"
        :key="s.maSize"
        class="bg-white rounded-xl border border-gray-200 p-4 flex flex-col items-center group hover:border-blue-300 hover:shadow-sm transition-all"
      >
        <div
          class="w-12 h-12 rounded-full bg-blue-50 flex items-center justify-center mb-2.5"
        >
          <span class="text-lg font-extrabold text-blue-600">{{
            s.tenSize
          }}</span>
        </div>
        <p class="text-xs text-gray-400 mb-3">#{{ s.maSize }}</p>
        <div
          class="flex items-center space-x-1 opacity-0 group-hover:opacity-100 transition-opacity"
        >
          <button
            @click="openModal(s)"
            class="p-1.5 rounded-lg text-gray-400 hover:text-blue-600 hover:bg-blue-50 transition-colors"
          >
            <PencilIcon class="h-3.5 w-3.5" stroke-width="2" />
          </button>
          <button
            @click="confirmDelete(s)"
            class="p-1.5 rounded-lg text-gray-400 hover:text-red-500 hover:bg-red-50 transition-colors"
          >
            <Trash2Icon class="h-3.5 w-3.5" stroke-width="2" />
          </button>
        </div>
      </div>
      <!-- Empty state -->
      <div
        v-if="list.length === 0"
        class="col-span-full py-10 text-center text-gray-400 text-sm"
      >
        Chưa có kích thước nào.
      </div>
    </div>

    <!-- Modal Add/Edit -->
    <teleport to="body">
      <transition name="modal">
        <div
          v-if="showModal"
          class="fixed inset-0 z-50 flex items-center justify-center p-4 bg-black/40"
          @click.self="showModal = false"
        >
          <div class="bg-white rounded-xl shadow-2xl w-full max-w-sm p-6">
            <h4 class="text-lg font-bold text-gray-800 mb-5">
              {{ editing ? "Sửa kích thước" : "Thêm kích thước" }}
            </h4>
            <div>
              <label
                class="block text-xs font-bold text-gray-500 uppercase mb-1.5"
                >Tên size *</label
              >
              <input
                v-model="form.tenSize"
                type="text"
                placeholder="VD: S, M, L, XL, 38, 39..."
                class="w-full px-3.5 py-2.5 border border-gray-200 rounded-lg text-sm focus:outline-none focus:ring-2 focus:ring-blue-500/20 focus:border-blue-400"
              />
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
      </transition>
    </teleport>

    <!-- Delete Confirm -->
    <teleport to="body">
      <transition name="modal">
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
              Bạn có chắc muốn xóa size
              <strong>{{ deleteTarget?.tenSize }}</strong
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
      </transition>
    </teleport>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from "vue";
import {
  Plus as PlusIcon,
  Pencil as PencilIcon,
  Trash2 as Trash2Icon,
  Loader2 as Loader2Icon,
} from "lucide-vue-next";
import { mauSizeService } from "../../services/mausize.service";
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
const form = ref({ tenSize: "" });

const fetchAll = async () => {
  loading.value = true;
  try {
    const res: any = await mauSizeService.getAllSize();
    if (res.success) list.value = res.data || [];
  } catch (e) {
    error("Lỗi tải danh sách size");
  } finally {
    loading.value = false;
  }
};

const openModal = (item?: any) => {
  if (item) {
    editing.value = true;
    editId.value = item.maSize;
    form.value = { tenSize: item.tenSize };
  } else {
    editing.value = false;
    editId.value = null;
    form.value = { tenSize: "" };
  }
  showModal.value = true;
};

const handleSave = async () => {
  if (!form.value.tenSize.trim()) {
    error("Tên size không được để trống");
    return;
  }
  saving.value = true;
  try {
    if (editing.value && editId.value !== null) {
      const res: any = await mauSizeService.updateSize(
        editId.value,
        form.value,
      );
      if (res.success) {
        success("Cập nhật size thành công");
        showModal.value = false;
        fetchAll();
      } else error(res.message);
    } else {
      const res: any = await mauSizeService.createSize(form.value);
      if (res.success) {
        success("Thêm size thành công");
        showModal.value = false;
        fetchAll();
      } else error(res.message);
    }
  } catch (e: any) {
    error(e.response?.data?.message || "Lỗi thao tác");
  } finally {
    saving.value = false;
  }
};

const confirmDelete = (item: any) => {
  deleteTarget.value = item;
  showDelete.value = true;
};

const handleDelete = async () => {
  if (!deleteTarget.value) return;
  saving.value = true;
  try {
    const res: any = await mauSizeService.deleteSize(deleteTarget.value.maSize);
    if (res.success) {
      success("Xóa size thành công");
      showDelete.value = false;
      fetchAll();
    } else error(res.message);
  } catch (e: any) {
    error(e.response?.data?.message || "Không thể xóa");
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
