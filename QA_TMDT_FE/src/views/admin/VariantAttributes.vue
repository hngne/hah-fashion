<template>
  <div>
    <Toast />
    <!-- Header -->
    <div
      class="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-3 mb-6"
    >
      <div>
        <h3 class="text-xl font-bold text-[#1F2937]">Quản lý biến thể</h3>
        <p class="text-sm text-gray-500 mt-0.5">
          Quản lý Màu sắc và Kích thước cho biến thể sản phẩm.
        </p>
      </div>
    </div>

    <!-- Tabs -->
    <div class="flex space-x-1 bg-gray-100 rounded-lg p-1 mb-6 max-w-xs">
      <button
        @click="activeTab = 'color'"
        :class="[
          'flex-1 py-2 px-4 text-sm font-semibold rounded-md transition',
          activeTab === 'color'
            ? 'bg-white text-blue-600 shadow-sm'
            : 'text-gray-500 hover:text-gray-700',
        ]"
      >
        <PaletteIcon class="h-4 w-4 inline mr-1.5" stroke-width="2" />Màu sắc
      </button>
      <button
        @click="activeTab = 'size'"
        :class="[
          'flex-1 py-2 px-4 text-sm font-semibold rounded-md transition',
          activeTab === 'size'
            ? 'bg-white text-blue-600 shadow-sm'
            : 'text-gray-500 hover:text-gray-700',
        ]"
      >
        <RulerIcon class="h-4 w-4 inline mr-1.5" stroke-width="2" />Kích thước
      </button>
    </div>

    <!-- ===== MÀU SẮC TAB ===== -->
    <template v-if="activeTab === 'color'">
      <div class="flex justify-end mb-4">
        <button
          @click="openColorModal()"
          class="inline-flex items-center px-4 py-2.5 rounded-lg bg-blue-600 text-white text-sm font-semibold hover:bg-blue-700 shadow-sm transition"
        >
          <PlusIcon class="h-4 w-4 mr-1.5" stroke-width="2.5" /> Thêm màu
        </button>
      </div>
      <div v-if="loadingColors" class="flex justify-center py-12 text-gray-400">
        <Loader2Icon class="h-6 w-6 animate-spin" />
      </div>
      <div
        v-else
        class="bg-white rounded-xl border border-gray-200 overflow-hidden"
      >
        <table class="w-full">
          <thead>
            <tr
              class="text-left text-[11px] font-bold text-gray-400 uppercase tracking-wider border-b border-gray-100"
            >
              <th class="px-5 py-3 w-12">#</th>
              <th class="px-5 py-3">Tên màu</th>
              <th class="px-5 py-3 w-28">Mã HEX</th>
              <th class="px-5 py-3 w-16">Xem</th>
              <th class="px-5 py-3 w-28 text-right">Thao tác</th>
            </tr>
          </thead>
          <tbody class="divide-y divide-gray-50">
            <tr
              v-for="(m, i) in colorList"
              :key="m.maMau"
              class="hover:bg-gray-50/50 transition-colors"
            >
              <td class="px-5 py-3 text-sm text-gray-400">{{ i + 1 }}</td>
              <td class="px-5 py-3 text-sm font-semibold text-gray-800">
                {{ m.tenMau }}
              </td>
              <td class="px-5 py-3 text-sm text-gray-500 font-mono">
                {{ m.maHex || "—" }}
              </td>
              <td class="px-5 py-3">
                <div
                  class="w-6 h-6 rounded border border-gray-200"
                  :style="{ backgroundColor: m.maHex || '#ccc' }"
                ></div>
              </td>
              <td class="px-5 py-3 text-right">
                <button
                  @click="openColorModal(m)"
                  class="p-1.5 rounded-lg text-gray-400 hover:text-blue-600 hover:bg-blue-50 transition-colors mr-1"
                >
                  <PencilIcon class="h-4 w-4" stroke-width="2" />
                </button>
                <button
                  @click="confirmDeleteItem('color', m)"
                  class="p-1.5 rounded-lg text-gray-400 hover:text-red-500 hover:bg-red-50 transition-colors"
                >
                  <Trash2Icon class="h-4 w-4" stroke-width="2" />
                </button>
              </td>
            </tr>
            <tr v-if="colorList.length === 0">
              <td
                colspan="5"
                class="px-5 py-8 text-center text-gray-400 text-sm"
              >
                Chưa có màu sắc nào.
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </template>

    <!-- ===== KÍCH THƯỚC TAB ===== -->
    <template v-if="activeTab === 'size'">
      <div class="flex justify-end mb-4">
        <button
          @click="openSizeModal()"
          class="inline-flex items-center px-4 py-2.5 rounded-lg bg-blue-600 text-white text-sm font-semibold hover:bg-blue-700 shadow-sm transition"
        >
          <PlusIcon class="h-4 w-4 mr-1.5" stroke-width="2.5" /> Thêm size
        </button>
      </div>
      <div v-if="loadingSizes" class="flex justify-center py-12 text-gray-400">
        <Loader2Icon class="h-6 w-6 animate-spin" />
      </div>
      <div
        v-else
        class="grid grid-cols-2 sm:grid-cols-3 md:grid-cols-4 lg:grid-cols-6 gap-3"
      >
        <div
          v-for="s in sizeList"
          :key="s.maSize"
          class="bg-white rounded-xl border border-gray-200 p-4 flex flex-col items-center group hover:border-blue-300 hover:shadow-sm transition-all"
        >
          <div
            class="w-12 h-12 rounded-full bg-blue-50 flex items-center justify-center mb-2"
          >
            <span class="text-lg font-extrabold text-blue-600">{{
              s.tenSize
            }}</span>
          </div>
          <div
            class="flex items-center space-x-1 opacity-0 group-hover:opacity-100 transition-opacity"
          >
            <button
              @click="openSizeModal(s)"
              class="p-1.5 rounded-lg text-gray-400 hover:text-blue-600 hover:bg-blue-50 transition-colors"
            >
              <PencilIcon class="h-3.5 w-3.5" stroke-width="2" />
            </button>
            <button
              @click="confirmDeleteItem('size', s)"
              class="p-1.5 rounded-lg text-gray-400 hover:text-red-500 hover:bg-red-50 transition-colors"
            >
              <Trash2Icon class="h-3.5 w-3.5" stroke-width="2" />
            </button>
          </div>
        </div>
        <div
          v-if="sizeList.length === 0"
          class="col-span-full py-8 text-center text-gray-400 text-sm"
        >
          Chưa có kích thước nào.
        </div>
      </div>
    </template>

    <!-- ===== COLOR MODAL ===== -->
    <teleport to="body"
      ><transition name="modal">
        <div
          v-if="showColorModal"
          class="fixed inset-0 z-50 flex items-center justify-center p-4 bg-black/40"
          @click.self="showColorModal = false"
        >
          <div class="bg-white rounded-xl shadow-2xl w-full max-w-md p-6">
            <h4 class="text-lg font-bold text-gray-800 mb-5">
              {{ editingColor ? "Sửa màu sắc" : "Thêm màu sắc" }}
            </h4>
            <div class="space-y-4">
              <div>
                <label
                  class="block text-xs font-bold text-gray-500 uppercase mb-1.5"
                  >Tên màu *</label
                >
                <input
                  v-model="colorForm.tenMau"
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
                    v-model="colorForm.maHex"
                    type="text"
                    placeholder="#FF0000"
                    class="flex-1 px-3.5 py-2.5 border border-gray-200 rounded-lg text-sm font-mono focus:outline-none focus:ring-2 focus:ring-blue-500/20 focus:border-blue-400"
                  />
                  <input
                    v-model="colorForm.maHex"
                    type="color"
                    class="w-10 h-10 rounded-lg border border-gray-200 cursor-pointer"
                  />
                </div>
              </div>
            </div>
            <div class="flex justify-end space-x-2 mt-6">
              <button
                @click="showColorModal = false"
                class="px-4 py-2 text-sm font-medium text-gray-600 hover:bg-gray-100 rounded-lg transition"
              >
                Hủy
              </button>
              <button
                @click="saveColor"
                :disabled="saving"
                class="px-5 py-2 text-sm font-semibold text-white bg-blue-600 hover:bg-blue-700 rounded-lg shadow-sm transition disabled:opacity-50"
              >
                {{ saving ? "Lưu..." : editingColor ? "Cập nhật" : "Thêm mới" }}
              </button>
            </div>
          </div>
        </div>
      </transition></teleport
    >

    <!-- ===== SIZE MODAL ===== -->
    <teleport to="body"
      ><transition name="modal">
        <div
          v-if="showSizeModal"
          class="fixed inset-0 z-50 flex items-center justify-center p-4 bg-black/40"
          @click.self="showSizeModal = false"
        >
          <div class="bg-white rounded-xl shadow-2xl w-full max-w-sm p-6">
            <h4 class="text-lg font-bold text-gray-800 mb-5">
              {{ editingSize ? "Sửa kích thước" : "Thêm kích thước" }}
            </h4>
            <div>
              <label
                class="block text-xs font-bold text-gray-500 uppercase mb-1.5"
                >Tên size *</label
              >
              <input
                v-model="sizeForm.tenSize"
                type="text"
                placeholder="VD: S, M, L, XL, 38..."
                class="w-full px-3.5 py-2.5 border border-gray-200 rounded-lg text-sm focus:outline-none focus:ring-2 focus:ring-blue-500/20 focus:border-blue-400"
              />
            </div>
            <div class="flex justify-end space-x-2 mt-6">
              <button
                @click="showSizeModal = false"
                class="px-4 py-2 text-sm font-medium text-gray-600 hover:bg-gray-100 rounded-lg transition"
              >
                Hủy
              </button>
              <button
                @click="saveSize"
                :disabled="saving"
                class="px-5 py-2 text-sm font-semibold text-white bg-blue-600 hover:bg-blue-700 rounded-lg shadow-sm transition disabled:opacity-50"
              >
                {{ saving ? "Lưu..." : editingSize ? "Cập nhật" : "Thêm mới" }}
              </button>
            </div>
          </div>
        </div>
      </transition></teleport
    >

    <!-- ===== DELETE CONFIRM ===== -->
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
              Bạn có chắc muốn xóa
              <strong>{{
                deleteTarget?.tenMau || deleteTarget?.tenSize
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
import { ref, onMounted } from "vue";
import {
  Plus as PlusIcon,
  Pencil as PencilIcon,
  Trash2 as Trash2Icon,
  Loader2 as Loader2Icon,
  Palette as PaletteIcon,
  Ruler as RulerIcon,
} from "lucide-vue-next";
import { mauSizeService } from "../../services/mausize.service";
import { useToast } from "../../composables/useToast";
import Toast from "../../components/admin/Toast.vue";

const { success, error } = useToast();
const activeTab = ref<"color" | "size">("color");

// Color state
const colorList = ref<any[]>([]);
const loadingColors = ref(true);
const showColorModal = ref(false);
const editingColor = ref(false);
const editColorId = ref<number | null>(null);
const colorForm = ref({ tenMau: "", maHex: "#000000" });

// Size state
const sizeList = ref<any[]>([]);
const loadingSizes = ref(true);
const showSizeModal = ref(false);
const editingSize = ref(false);
const editSizeId = ref<number | null>(null);
const sizeForm = ref({ tenSize: "" });

// Shared
const saving = ref(false);
const showDelete = ref(false);
const deleteTarget = ref<any>(null);
const deleteType = ref<"color" | "size">("color");

// ===== COLOR =====
const fetchColors = async () => {
  loadingColors.value = true;
  try {
    const r: any = await mauSizeService.getAllMau();
    if (r.success) colorList.value = r.data || [];
  } catch {
    error("Lỗi tải màu sắc");
  } finally {
    loadingColors.value = false;
  }
};

const openColorModal = (item?: any) => {
  if (item) {
    editingColor.value = true;
    editColorId.value = item.maMau;
    colorForm.value = { tenMau: item.tenMau, maHex: item.maHex || "#000000" };
  } else {
    editingColor.value = false;
    editColorId.value = null;
    colorForm.value = { tenMau: "", maHex: "#000000" };
  }
  showColorModal.value = true;
};

const saveColor = async () => {
  if (!colorForm.value.tenMau.trim()) {
    error("Tên màu không được để trống");
    return;
  }
  saving.value = true;
  try {
    const res: any =
      editingColor.value && editColorId.value !== null
        ? await mauSizeService.updateMau(editColorId.value, colorForm.value)
        : await mauSizeService.createMau(colorForm.value);
    if (res.success) {
      success(
        editingColor.value ? "Cập nhật màu thành công" : "Thêm màu thành công",
      );
      showColorModal.value = false;
      fetchColors();
    } else error(res.message);
  } catch (e: any) {
    error(e.response?.data?.message || "Lỗi");
  } finally {
    saving.value = false;
  }
};

// ===== SIZE =====
const fetchSizes = async () => {
  loadingSizes.value = true;
  try {
    const r: any = await mauSizeService.getAllSize();
    if (r.success) sizeList.value = r.data || [];
  } catch {
    error("Lỗi tải kích thước");
  } finally {
    loadingSizes.value = false;
  }
};

const openSizeModal = (item?: any) => {
  if (item) {
    editingSize.value = true;
    editSizeId.value = item.maSize;
    sizeForm.value = { tenSize: item.tenSize };
  } else {
    editingSize.value = false;
    editSizeId.value = null;
    sizeForm.value = { tenSize: "" };
  }
  showSizeModal.value = true;
};

const saveSize = async () => {
  if (!sizeForm.value.tenSize.trim()) {
    error("Tên size không được để trống");
    return;
  }
  saving.value = true;
  try {
    const res: any =
      editingSize.value && editSizeId.value !== null
        ? await mauSizeService.updateSize(editSizeId.value, sizeForm.value)
        : await mauSizeService.createSize(sizeForm.value);
    if (res.success) {
      success(
        editingSize.value ? "Cập nhật size thành công" : "Thêm size thành công",
      );
      showSizeModal.value = false;
      fetchSizes();
    } else error(res.message);
  } catch (e: any) {
    error(e.response?.data?.message || "Lỗi");
  } finally {
    saving.value = false;
  }
};

// ===== DELETE =====
const confirmDeleteItem = (type: "color" | "size", item: any) => {
  deleteType.value = type;
  deleteTarget.value = item;
  showDelete.value = true;
};

const handleDelete = async () => {
  if (!deleteTarget.value) return;
  saving.value = true;
  try {
    const res: any =
      deleteType.value === "color"
        ? await mauSizeService.deleteMau(deleteTarget.value.maMau)
        : await mauSizeService.deleteSize(deleteTarget.value.maSize);
    if (res.success) {
      success("Xóa thành công");
      showDelete.value = false;
      deleteType.value === "color" ? fetchColors() : fetchSizes();
    } else error(res.message);
  } catch (e: any) {
    error(e.response?.data?.message || "Không thể xóa");
  } finally {
    saving.value = false;
  }
};

onMounted(() => {
  fetchColors();
  fetchSizes();
});
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
