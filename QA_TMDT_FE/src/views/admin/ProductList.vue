<template>
  <div>
    <Toast />
    <!-- Header -->
    <div
      class="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-3 mb-6"
    >
      <div>
        <h3 class="text-xl font-bold text-[#1F2937]">Quản lý Sản phẩm</h3>
        <p class="text-sm text-gray-500 mt-0.5">
          Danh sách tất cả sản phẩm trong hệ thống.
        </p>
      </div>
      <router-link
        to="/admin/products/new"
        class="inline-flex items-center px-4 py-2.5 rounded-lg bg-blue-600 text-white text-sm font-semibold hover:bg-blue-700 shadow-sm transition"
      >
        <PlusIcon class="h-4 w-4 mr-1.5" stroke-width="2.5" /> Thêm sản phẩm
      </router-link>
    </div>

    <!-- Search -->
    <div class="bg-white rounded-xl border border-gray-200 p-4 mb-5">
      <div class="flex flex-col sm:flex-row gap-3">
        <div
          class="flex items-center space-x-1 bg-gray-100 rounded-lg p-0.5 flex-shrink-0"
        >
          <button
            @click="searchType = 'name'"
            :class="[
              'px-3 py-1.5 text-xs font-semibold rounded-md transition',
              searchType === 'name'
                ? 'bg-white text-blue-600 shadow-sm'
                : 'text-gray-500',
            ]"
          >
            Tên SP
          </button>
          <button
            @click="searchType = 'code'"
            :class="[
              'px-3 py-1.5 text-xs font-semibold rounded-md transition',
              searchType === 'code'
                ? 'bg-white text-blue-600 shadow-sm'
                : 'text-gray-500',
            ]"
          >
            Mã SP
          </button>
        </div>
        <div class="flex-1 relative">
          <SearchIcon
            class="absolute left-3 top-1/2 -translate-y-1/2 h-4 w-4 text-gray-400"
            stroke-width="2"
          />
          <input
            v-model="searchQuery"
            @input="debouncedSearch"
            type="text"
            :placeholder="
              searchType === 'name'
                ? 'Tìm theo tên sản phẩm...'
                : 'Nhập mã sản phẩm (VD: SP001)...'
            "
            class="w-full pl-9 pr-4 py-2.5 border border-gray-200 rounded-lg text-sm focus:outline-none focus:ring-2 focus:ring-blue-500/20 focus:border-blue-400"
          />
        </div>
      </div>
    </div>

    <div v-if="loading" class="flex justify-center py-16 text-gray-400">
      <Loader2Icon class="h-7 w-7 animate-spin" />
    </div>

    <div
      v-else
      class="bg-white rounded-xl border border-gray-200 overflow-hidden"
    >
      <!-- Desktop -->
      <div class="hidden md:block overflow-x-auto">
        <table class="w-full">
          <thead>
            <tr
              class="text-left text-[11px] font-bold text-gray-400 uppercase tracking-wider border-b border-gray-100"
            >
              <th class="px-5 py-3 w-16">Ảnh</th>
              <th class="px-5 py-3">Tên sản phẩm</th>
              <th class="px-5 py-3">Danh mục</th>
              <th class="px-5 py-3">Giá gốc</th>
              <th class="px-5 py-3 w-24">Biến thể</th>
              <th class="px-5 py-3 w-40 text-right">Thao tác</th>
            </tr>
          </thead>
          <tbody class="divide-y divide-gray-50">
            <tr
              v-for="sp in items"
              :key="sp.maSp"
              class="hover:bg-gray-50/50 transition-colors cursor-pointer"
              @click="viewDetail(sp.maSp)"
            >
              <td class="px-5 py-3">
                <div
                  class="w-10 h-10 rounded-lg bg-gray-100 overflow-hidden flex-shrink-0"
                >
                  <img
                    v-if="sp.anhDaiDien"
                    :src="sp.anhDaiDien"
                    class="w-full h-full object-cover"
                  />
                  <PackageIcon
                    v-else
                    class="w-5 h-5 m-2.5 text-gray-300"
                    stroke-width="1.5"
                  />
                </div>
              </td>
              <td class="px-5 py-3">
                <p class="text-sm font-semibold text-gray-800 line-clamp-1">
                  {{ sp.tenSp }}
                </p>
                <p class="text-[11px] text-gray-400 mt-0.5">{{ sp.maSp }}</p>
              </td>
              <td class="px-5 py-3 text-sm text-gray-600">
                {{ sp.tenDanhMuc || "—" }}
              </td>
              <td class="px-5 py-3 text-sm font-medium text-gray-800">
                {{ formatMoney(sp.giaGoc) }}
              </td>
              <td class="px-5 py-3">
                <span
                  class="text-xs font-bold px-2 py-0.5 rounded-full bg-blue-50 text-blue-600"
                  >{{ sp.dsBienThe?.length || 0 }}</span
                >
              </td>
              <td class="px-5 py-3 text-right" @click.stop>
                <button
                  @click="viewDetail(sp.maSp)"
                  class="p-1.5 rounded-lg text-gray-400 hover:text-blue-600 hover:bg-blue-50 transition-colors mr-1"
                  title="Xem chi tiết"
                >
                  <EyeIcon class="h-4 w-4" stroke-width="2" />
                </button>
                <router-link
                  :to="`/admin/products/${sp.maSp}/edit`"
                  class="p-1.5 rounded-lg text-gray-400 hover:text-blue-600 hover:bg-blue-50 transition-colors inline-block mr-1"
                  title="Sửa"
                >
                  <PencilIcon class="h-4 w-4" stroke-width="2" />
                </router-link>
                <button
                  @click="confirmDelete(sp)"
                  class="p-1.5 rounded-lg text-gray-400 hover:text-red-500 hover:bg-red-50 transition-colors"
                  title="Xóa"
                >
                  <Trash2Icon class="h-4 w-4" stroke-width="2" />
                </button>
              </td>
            </tr>
            <tr v-if="items.length === 0">
              <td
                colspan="6"
                class="px-5 py-10 text-center text-gray-400 text-sm"
              >
                Không tìm thấy sản phẩm nào.
              </td>
            </tr>
          </tbody>
        </table>
      </div>

      <!-- Mobile -->
      <div class="md:hidden divide-y divide-gray-50">
        <div
          v-for="sp in items"
          :key="sp.maSp"
          class="p-4 flex items-start space-x-3"
          @click="viewDetail(sp.maSp)"
        >
          <div
            class="w-14 h-14 rounded-lg bg-gray-100 overflow-hidden flex-shrink-0"
          >
            <img
              v-if="sp.anhDaiDien"
              :src="sp.anhDaiDien"
              class="w-full h-full object-cover"
            />
            <PackageIcon
              v-else
              class="w-6 h-6 m-4 text-gray-300"
              stroke-width="1.5"
            />
          </div>
          <div class="flex-1 min-w-0">
            <p class="text-sm font-semibold text-gray-800 line-clamp-1">
              {{ sp.tenSp }}
            </p>
            <p class="text-xs text-gray-400 mt-0.5">
              {{ sp.tenDanhMuc }} · {{ sp.dsBienThe?.length || 0 }} biến thể
            </p>
            <p class="text-sm font-bold text-gray-700 mt-1">
              {{ formatMoney(sp.giaGoc) }}
            </p>
          </div>
          <div class="flex flex-col space-y-1" @click.stop>
            <router-link
              :to="`/admin/products/${sp.maSp}/edit`"
              class="p-1.5 rounded-lg text-gray-400 hover:text-blue-600 hover:bg-blue-50"
            >
              <PencilIcon class="h-4 w-4" stroke-width="2" />
            </router-link>
            <button
              @click="confirmDelete(sp)"
              class="p-1.5 rounded-lg text-gray-400 hover:text-red-500 hover:bg-red-50"
            >
              <Trash2Icon class="h-4 w-4" stroke-width="2" />
            </button>
          </div>
        </div>
        <div
          v-if="items.length === 0"
          class="p-8 text-center text-gray-400 text-sm"
        >
          Không có sản phẩm.
        </div>
      </div>

      <!-- Pagination -->
      <div
        v-if="totalPage > 1"
        class="px-5 py-3 border-t border-gray-100 flex flex-col sm:flex-row items-center justify-between gap-2"
      >
        <span class="text-xs text-gray-400"
          >Trang {{ page }} / {{ totalPage }} · {{ totalItem }} sản phẩm</span
        >
        <div class="flex items-center space-x-1">
          <button
            @click="goPage(page - 1)"
            :disabled="page <= 1"
            class="px-3 py-1.5 text-xs font-medium rounded-lg border border-gray-200 hover:bg-gray-50 disabled:opacity-40 transition"
          >
            Trước
          </button>
          <button
            v-for="p in visiblePages"
            :key="p"
            @click="goPage(p)"
            :class="[
              'px-3 py-1.5 text-xs font-medium rounded-lg transition',
              p === page
                ? 'bg-blue-600 text-white'
                : 'border border-gray-200 hover:bg-gray-50',
            ]"
          >
            {{ p }}
          </button>
          <button
            @click="goPage(page + 1)"
            :disabled="page >= totalPage"
            class="px-3 py-1.5 text-xs font-medium rounded-lg border border-gray-200 hover:bg-gray-50 disabled:opacity-40 transition"
          >
            Sau
          </button>
        </div>
      </div>
    </div>

    <!-- ===== PRODUCT DETAIL MODAL ===== -->
    <teleport to="body">
      <transition name="modal">
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
            <template v-else-if="detailData">
              <!-- Header -->
              <div
                class="sticky top-0 bg-white px-6 py-4 border-b border-gray-100 flex items-center justify-between z-10"
              >
                <h4 class="text-lg font-bold text-gray-800">
                  Chi tiết sản phẩm
                </h4>
                <button
                  @click="showDetail = false"
                  class="p-1.5 rounded-lg text-gray-400 hover:bg-gray-100"
                >
                  <XIcon class="h-5 w-5" />
                </button>
              </div>
              <!-- Body -->
              <div class="p-6 space-y-5">
                <!-- Ảnh -->
                <div
                  v-if="detailData.duongDanAnhSPs?.length"
                  class="flex gap-2 overflow-x-auto pb-2"
                >
                  <img
                    v-for="(img, i) in detailData.duongDanAnhSPs"
                    :key="i"
                    :src="img"
                    class="w-20 h-20 rounded-lg object-cover border border-gray-200 flex-shrink-0"
                  />
                </div>
                <!-- Info -->
                <div class="grid grid-cols-2 gap-x-6 gap-y-3 text-sm">
                  <div>
                    <span class="text-gray-400 text-xs uppercase font-bold"
                      >Mã SP</span
                    >
                    <p class="font-semibold text-gray-800 mt-0.5">
                      {{ detailData.maSp }}
                    </p>
                  </div>
                  <div>
                    <span class="text-gray-400 text-xs uppercase font-bold"
                      >Danh mục</span
                    >
                    <p class="font-semibold text-gray-800 mt-0.5">
                      {{ detailData.tenDanhMuc || "—" }}
                    </p>
                  </div>
                  <div>
                    <span class="text-gray-400 text-xs uppercase font-bold"
                      >Tên SP</span
                    >
                    <p class="font-semibold text-gray-800 mt-0.5">
                      {{ detailData.tenSp }}
                    </p>
                  </div>
                  <div>
                    <span class="text-gray-400 text-xs uppercase font-bold"
                      >Giá gốc</span
                    >
                    <p class="font-semibold text-gray-800 mt-0.5">
                      {{ formatMoney(detailData.giaGoc) }}
                    </p>
                  </div>
                  <div>
                    <span class="text-gray-400 text-xs uppercase font-bold"
                      >Giá KM</span
                    >
                    <p class="font-semibold text-emerald-600 mt-0.5">
                      {{ formatMoney(detailData.giaKm) }}
                    </p>
                  </div>
                  <div>
                    <span class="text-gray-400 text-xs uppercase font-bold"
                      >Chất liệu</span
                    >
                    <p class="font-semibold text-gray-800 mt-0.5">
                      {{ detailData.chatLieu || "—" }}
                    </p>
                  </div>
                </div>
                <div v-if="detailData.moTa">
                  <span class="text-gray-400 text-xs uppercase font-bold"
                    >Mô tả</span
                  >
                  <p class="text-sm text-gray-600 mt-1">
                    {{ detailData.moTa }}
                  </p>
                </div>
                <!-- Variants Table -->
                <div>
                  <h5 class="text-xs font-bold text-gray-400 uppercase mb-2">
                    Biến thể ({{ detailData.dsBienThe?.length || 0 }})
                  </h5>
                  <div
                    class="overflow-x-auto border border-gray-200 rounded-lg"
                  >
                    <table class="w-full text-sm">
                      <thead>
                        <tr
                          class="text-[10px] font-bold text-gray-400 uppercase bg-gray-50"
                        >
                          <th class="px-4 py-2 text-left">Mã CTSP</th>
                          <th class="px-4 py-2 text-left">Size</th>
                          <th class="px-4 py-2 text-left">Màu</th>
                          <th class="px-4 py-2 text-right">Tồn kho</th>
                          <th class="px-4 py-2 text-right">Giá bán</th>
                        </tr>
                      </thead>
                      <tbody class="divide-y divide-gray-50">
                        <tr
                          v-for="v in detailData.dsBienThe"
                          :key="v.maChiTietSP"
                          class="hover:bg-gray-50/50"
                        >
                          <td class="px-4 py-2 font-mono text-xs text-blue-600">
                            {{ v.maChiTietSP }}
                          </td>
                          <td class="px-4 py-2 font-medium">{{ v.tenSize }}</td>
                          <td class="px-4 py-2 font-medium">{{ v.tenMau }}</td>
                          <td class="px-4 py-2 text-right">
                            {{ v.soLuongTon ?? 0 }}
                          </td>
                          <td class="px-4 py-2 text-right font-semibold">
                            {{ formatMoney(v.giaBan || 0) }}
                          </td>
                        </tr>
                        <tr v-if="!detailData.dsBienThe?.length">
                          <td
                            colspan="5"
                            class="px-4 py-4 text-center text-gray-400"
                          >
                            Không có biến thể.
                          </td>
                        </tr>
                      </tbody>
                    </table>
                  </div>
                </div>
              </div>
              <!-- Footer -->
              <div
                class="px-6 py-4 border-t border-gray-100 flex justify-end space-x-2"
              >
                <button
                  @click="showDetail = false"
                  class="px-4 py-2 text-sm font-medium text-gray-600 hover:bg-gray-100 rounded-lg transition"
                >
                  Đóng
                </button>
                <router-link
                  :to="`/admin/products/${detailData.maSp}/edit`"
                  @click="showDetail = false"
                  class="px-4 py-2 text-sm font-semibold text-white bg-blue-600 hover:bg-blue-700 rounded-lg shadow-sm transition"
                  >Sửa sản phẩm</router-link
                >
              </div>
            </template>
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
            <h4 class="text-lg font-bold text-gray-800 mb-1">Xóa sản phẩm</h4>
            <p class="text-sm text-gray-500">
              Bạn có chắc muốn xóa
              <strong class="text-gray-700">{{ deleteTarget?.tenSp }}</strong
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
                :disabled="deleting"
                class="px-5 py-2 text-sm font-semibold text-white bg-red-500 hover:bg-red-600 rounded-lg shadow-sm transition disabled:opacity-50"
              >
                {{ deleting ? "Đang xóa..." : "Xóa" }}
              </button>
            </div>
          </div>
        </div>
      </transition>
    </teleport>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from "vue";
import {
  Plus as PlusIcon,
  Pencil as PencilIcon,
  Trash2 as Trash2Icon,
  Search as SearchIcon,
  Package as PackageIcon,
  Loader2 as Loader2Icon,
  Eye as EyeIcon,
  X as XIcon,
} from "lucide-vue-next";
import { sanPhamService } from "../../services/sanpham.service";
import { useToast } from "../../composables/useToast";
import Toast from "../../components/admin/Toast.vue";

const { success, error } = useToast();
const items = ref<any[]>([]);
const loading = ref(true);
const page = ref(1);
const pageSize = 10;
const totalItem = ref(0);
const totalPage = ref(0);
const searchQuery = ref("");
const searchType = ref<"name" | "code">("name");
const showDelete = ref(false);
const deleting = ref(false);
const deleteTarget = ref<any>(null);

// Detail modal
const showDetail = ref(false);
const detailLoading = ref(false);
const detailData = ref<any>(null);

let searchTimer: any = null;

const visiblePages = computed(() => {
  const pages: number[] = [];
  const start = Math.max(1, page.value - 2);
  const end = Math.min(totalPage.value, start + 4);
  for (let i = start; i <= end; i++) pages.push(i);
  return pages;
});

const formatMoney = (val: number) =>
  val ? val.toLocaleString("vi-VN") + " ₫" : "0 ₫";

const fetchProducts = async () => {
  loading.value = true;
  try {
    const q = searchQuery.value.trim();
    if (q && searchType.value === "code") {
      // Search by product code: use getFullInfo
      const res: any = await sanPhamService.getFullInfo(q);
      if (res.success && res.data) {
        items.value = [res.data];
        totalItem.value = 1;
        totalPage.value = 1;
      } else {
        items.value = [];
        totalItem.value = 0;
        totalPage.value = 0;
      }
    } else {
      // Search by name or get all
      const res: any = q
        ? await sanPhamService.searchByName(q, page.value, pageSize)
        : await sanPhamService.getAll(page.value, pageSize);
      if (res.success && res.data) {
        items.value = res.data.item || [];
        totalItem.value = res.data.totalItem || 0;
        totalPage.value = res.data.totalPage || 0;
      }
    }
  } catch (e) {
    error("Lỗi tải danh sách sản phẩm");
  } finally {
    loading.value = false;
  }
};

const goPage = (p: number) => {
  if (p >= 1 && p <= totalPage.value) {
    page.value = p;
    fetchProducts();
  }
};
const debouncedSearch = () => {
  clearTimeout(searchTimer);
  searchTimer = setTimeout(() => {
    page.value = 1;
    fetchProducts();
  }, 400);
};

const viewDetail = async (maSp: string) => {
  showDetail.value = true;
  detailLoading.value = true;
  detailData.value = null;
  try {
    const res: any = await sanPhamService.getFullInfo(maSp);
    if (res.success) detailData.value = res.data;
    else error(res.message);
  } catch (e) {
    error("Lỗi tải chi tiết sản phẩm");
  } finally {
    detailLoading.value = false;
  }
};

const confirmDelete = (sp: any) => {
  deleteTarget.value = sp;
  showDelete.value = true;
};
const handleDelete = async () => {
  if (!deleteTarget.value) return;
  deleting.value = true;
  try {
    const res: any = await sanPhamService.delete(deleteTarget.value.maSp);
    if (res.success) {
      success("Xóa sản phẩm thành công");
      showDelete.value = false;
      fetchProducts();
    } else error(res.message);
  } catch (e: any) {
    error(e.response?.data?.message || "Lỗi xóa sản phẩm");
  } finally {
    deleting.value = false;
  }
};

onMounted(fetchProducts);
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
