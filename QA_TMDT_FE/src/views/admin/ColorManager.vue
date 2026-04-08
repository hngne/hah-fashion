<template>
  <div>
    <Toast />
    <!-- Header -->
    <div
      class="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-3 mb-6"
    >
      <div>
        <h3 class="text-xl font-bold text-[#1F2937]">Quản lý Màu sắc</h3>
        <p class="text-sm text-gray-500 mt-0.5">
          Thêm, sửa, xóa các màu sắc cho biến thể sản phẩm.
        </p>
      </div>
      <button
        @click="openModal()"
        class="inline-flex items-center px-4 py-2.5 rounded-lg bg-blue-600 text-white text-sm font-semibold hover:bg-blue-700 shadow-sm transition"
      >
        <PlusIcon class="h-4 w-4 mr-1.5" stroke-width="2.5" /> Thêm màu
      </button>
    </div>

    <!-- Loading -->
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
              <th class="px-5 py-3 w-16">#</th>
              <th class="px-5 py-3">Tên màu</th>
              <th class="px-5 py-3 w-32">Mã HEX</th>
              <th class="px-5 py-3 w-20">Xem</th>
              <th class="px-5 py-3 w-32 text-right">Thao tác</th>
            </tr>
          </thead>
          <tbody class="divide-y divide-gray-50">
            <tr
              v-for="(mau, idx) in list"
              :key="mau.maMau"
              class="hover:bg-gray-50/50 transition-colors"
            >
              <td class="px-5 py-3 text-sm text-gray-400">{{ idx + 1 }}</td>
              <td class="px-5 py-3 text-sm font-semibold text-gray-800">
                {{ mau.tenMau }}
              </td>
              <td class="px-5 py-3 text-sm text-gray-500 font-mono">
                {{ mau.maHex || "—" }}
              </td>
              <td class="px-5 py-3">
                <div
                  class="w-6 h-6 rounded border border-gray-200"
                  :style="{ backgroundColor: mau.maHex || '#ccc' }"
                ></div>
              </td>
              <td class="px-5 py-3 text-right">
                <button
                  @click="openModal(mau)"
                  class="p-1.5 rounded-lg text-gray-400 hover:text-blue-600 hover:bg-blue-50 transition-colors mr-1"
                >
                  <PencilIcon class="h-4 w-4" stroke-width="2" />
                </button>
                <button
                  @click="confirmDelete(mau)"
                  class="p-1.5 rounded-lg text-gray-400 hover:text-red-500 hover:bg-red-50 transition-colors"
                >
                  <Trash2Icon class="h-4 w-4" stroke-width="2" />
                </button>
              </td>
            </tr>
            <tr v-if="list.length === 0">
              <td
                colspan="5"
                class="px-5 py-10 text-center text-gray-400 text-sm"
              >
                Chưa có màu sắc nào.
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>

    <!-- Modal -->
    <teleport to="body">
      <transition name="modal">
        <div
          v-if="showModal"
          class="fixed inset-0 z-50 flex items-center justify-center p-4 bg-black/40"
          @click.self="showModal = false"
        >
          <div class="bg-white rounded-xl shadow-2xl w-full max-w-md p-6">
            <h4 class="text-lg font-bold text-gray-800 mb-5">
              {{ editing ? "Sửa màu sắc" : "Thêm màu sắc" }}
            </h4>
            <div class="space-y-4">
              <div>
                <label
                  class="block text-xs font-bold text-gray-500 uppercase mb-1.5"
                  >Tên màu *</label
                >
                <input
                  v-model="form.tenMau"
                  type="text"
                  placeholder="VD: Đỏ, Xanh Navy..."
                  class="w-full px-3.5 py-2.5 border border-gray-200 rounded-lg text-sm focus:outline-none focus:ring-2 focus:ring-blue-500/20 focus:border-blue-400"
                />
              </div>
              <div>
                <label
                  class="block text-xs font-bold text-gray-500 uppercase mb-1.5"
                  >Mã HEX</label
                >
                <div class="flex items-center space-x-3">
                  <input
                    v-model="form.maHex"
                    type="text"
                    placeholder="#FF0000"
                    class="flex-1 px-3.5 py-2.5 border border-gray-200 rounded-lg text-sm font-mono focus:outline-none focus:ring-2 focus:ring-blue-500/20 focus:border-blue-400"
                  />
                  <input
                    v-model="form.maHex"
                    type="color"
                    class="w-10 h-10 rounded-lg border border-gray-200 cursor-pointer"
                  />
                </div>
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
      </transition>
    </teleport>

    <!-- Delete Confirm Modal -->
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
              Bạn có chắc muốn xóa màu
              <strong>{{ deleteTarget?.tenMau }}</strong
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
const form = ref({ tenMau: "", maHex: "#000000" });

const fetchAll = async () => {
  loading.value = true;
  try {
    const res: any = await mauSizeService.getAllMau();
    if (res.success) list.value = res.data || [];
  } catch (e) {
    error("Lỗi tải danh sách màu");
  } finally {
    loading.value = false;
  }
};

const openModal = (item?: any) => {
  if (item) {
    editing.value = true;
    editId.value = item.maMau;
    form.value = { tenMau: item.tenMau, maHex: item.maHex || "#000000" };
  } else {
    editing.value = false;
    editId.value = null;
    form.value = { tenMau: "", maHex: "#000000" };
  }
  showModal.value = true;
};

const handleSave = async () => {
  if (!form.value.tenMau.trim()) {
    error("Tên màu không được để trống");
    return;
  }
  saving.value = true;
  try {
    if (editing.value && editId.value !== null) {
      const res: any = await mauSizeService.updateMau(editId.value, form.value);
      if (res.success) {
        success("Cập nhật màu thành công");
        showModal.value = false;
        fetchAll();
      } else error(res.message);
    } else {
      const res: any = await mauSizeService.createMau(form.value);
      if (res.success) {
        success("Thêm màu thành công");
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
    const res: any = await mauSizeService.deleteMau(deleteTarget.value.maMau);
    if (res.success) {
      success("Xóa màu thành công");
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
.modal-enter-from > div,
.modal-leave-to > div {
  transform: scale(0.95);
}
</style>
