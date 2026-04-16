<template>
  <div>
    <Toast />
    <div
      class="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-3 mb-6"
    >
      <div>
        <h3 class="text-xl font-bold text-[#1F2937]">Quản lý Người dùng</h3>
        <p class="text-sm text-gray-500 mt-0.5">
          Tìm kiếm tài khoản bằng API và lọc theo role.
        </p>
      </div>
    </div>

    <div class="bg-white rounded-xl border border-gray-200 p-4 mb-5">
      <div class="grid gap-3 md:grid-cols-3">
        <div class="relative md:col-span-2">
          <SearchIcon
            class="absolute left-3 top-1/2 -translate-y-1/2 h-4 w-4 text-gray-400"
            stroke-width="2"
          />
          <input
            v-model="searchQuery"
            type="text"
            placeholder="Tìm theo mã tài khoản, tên đăng nhập, họ tên hoặc email..."
            class="w-full pl-9 pr-4 py-2.5 border border-gray-200 rounded-lg text-sm focus:outline-none focus:ring-2 focus:ring-blue-500/20 focus:border-blue-400"
          />
        </div>
        <div>
          <select
            v-model="roleFilter"
            class="w-full px-3 py-2.5 border border-gray-200 rounded-lg text-sm focus:outline-none focus:ring-2 focus:ring-blue-500/20 focus:border-blue-400"
          >
            <option value="all">Tất cả role</option>
            <option value="user">User</option>
            <option value="admin">Admin</option>
          </select>
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
      <div class="hidden md:block overflow-x-auto">
        <table class="w-full">
          <thead>
            <tr
              class="text-left text-[11px] font-bold text-gray-400 uppercase tracking-wider border-b border-gray-100"
            >
              <th class="px-5 py-3">Tài khoản</th>
              <th class="px-5 py-3">Thông tin</th>
              <th class="px-5 py-3">Phân quyền</th>
            </tr>
          </thead>
          <tbody class="divide-y divide-gray-50">
            <tr
              v-for="tk in list"
              :key="tk.maTaiKhoan"
              class="hover:bg-gray-50/50 transition-colors"
            >
              <td class="px-5 py-3">
                <p class="text-sm font-semibold text-gray-800">
                  {{ tk.tenDangNhap }}
                </p>
                <p class="text-xs text-gray-400 font-mono mt-0.5">
                  {{ tk.maTaiKhoan }}
                </p>
              </td>
              <td class="px-5 py-3">
                <p class="text-sm font-medium text-gray-700">
                  {{ tk.hoTen || "-" }}
                </p>
                <div
                  class="flex flex-col text-xs text-gray-500 mt-1 space-y-0.5"
                >
                  <span v-if="tk.email" class="flex items-center"
                    ><MailIcon class="w-3 h-3 mr-1" />{{ tk.email }}</span
                  >
                  <span v-if="tk.soDienThoai" class="flex items-center"
                    ><PhoneIcon class="w-3 h-3 mr-1" />{{
                      tk.soDienThoai
                    }}</span
                  >
                </div>
              </td>
              <td class="px-5 py-3">
                <span
                  :class="
                    tk.vaiTro === 'admin'
                      ? 'bg-purple-50 text-purple-700 border-purple-200'
                      : 'bg-blue-50 text-blue-700 border-blue-200'
                  "
                  class="text-[10px] font-bold px-2 py-0.5 rounded-full border"
                >
                  {{ tk.vaiTro === "admin" ? "Quản trị viên" : "Khách hàng" }}
                </span>
              </td>
            </tr>
            <tr v-if="list.length === 0">
              <td
                colspan="3"
                class="px-5 py-10 text-center text-gray-400 text-sm"
              >
                Không tìm thấy tài khoản nào.
              </td>
            </tr>
          </tbody>
        </table>
      </div>

      <div class="md:hidden divide-y divide-gray-50">
        <div
          v-for="tk in list"
          :key="tk.maTaiKhoan"
          class="p-4 flex flex-col gap-2"
        >
          <div class="flex justify-between items-start gap-3">
            <div>
              <p class="text-sm font-semibold text-gray-800">
                {{ tk.tenDangNhap }}
              </p>
              <p class="text-xs text-gray-400 font-mono mt-1">
                {{ tk.maTaiKhoan }}
              </p>
            </div>
            <span
              :class="
                tk.vaiTro === 'admin'
                  ? 'bg-purple-50 text-purple-700'
                  : 'bg-blue-50 text-blue-700'
              "
              class="text-[10px] font-bold px-2 py-0.5 rounded-full"
            >
              {{ tk.vaiTro }}
            </span>
          </div>
          <div class="flex justify-between items-end mt-2">
            <div class="text-xs text-gray-500 space-y-1">
              <span class="block">{{ tk.hoTen || '-' }}</span>
              <span v-if="tk.email" class="block">{{ tk.email }}</span>
              <span v-if="tk.soDienThoai" class="block">{{
                tk.soDienThoai
              }}</span>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onBeforeUnmount, onMounted, watch } from "vue";
import {
  Search as SearchIcon,
  Loader2 as Loader2Icon,
  Mail as MailIcon,
  Phone as PhoneIcon,
} from "lucide-vue-next";
import { userService } from "../../services/user.service";
import { useToast } from "../../composables/useToast";
import Toast from "../../components/admin/Toast.vue";
import type { UserAccount } from "../../types";

const { error } = useToast();
const list = ref<UserAccount[]>([]);
const loading = ref(true);
const searchQuery = ref("");
const roleFilter = ref("all");
let searchTimeout: ReturnType<typeof setTimeout> | null = null;

const fetchAll = async () => {
  loading.value = true;
  try {
    const res: any = await userService.getAll({
      keyword: searchQuery.value.trim() || undefined,
      vaiTro: roleFilter.value !== "all" ? roleFilter.value : undefined,
    });
    if (res.success) list.value = res.data || [];
  } catch (e: any) {
    error(e.response?.data?.message || "Lỗi tải danh sách người dùng");
  } finally {
    loading.value = false;
  }
};

watch(
  [searchQuery, roleFilter],
  () => {
    if (searchTimeout) clearTimeout(searchTimeout);
    searchTimeout = setTimeout(fetchAll, 350);
  },
  { flush: "post" },
);

onBeforeUnmount(() => {
  if (searchTimeout) clearTimeout(searchTimeout);
});

onMounted(fetchAll);
</script>
