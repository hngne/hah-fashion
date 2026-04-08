<template>
  <div>
    <Toast />
    <div
      class="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-3 mb-6"
    >
      <div>
        <h3 class="text-xl font-bold text-[#1F2937]">Quản lý Người dùng</h3>
        <p class="text-sm text-gray-500 mt-0.5">
          Danh sách tài khoản khách hàng và quản trị viên.
        </p>
      </div>
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
          placeholder="Tìm theo tên đăng nhập, họ tên hoặc email..."
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
      <!-- Desktop Table -->
      <div class="hidden md:block overflow-x-auto">
        <table class="w-full">
          <thead>
            <tr
              class="text-left text-[11px] font-bold text-gray-400 uppercase tracking-wider border-b border-gray-100"
            >
              <th class="px-5 py-3">Tài khoản</th>
              <th class="px-5 py-3">Thông tin</th>
              <th class="px-5 py-3">Phân quyền</th>
              <th class="px-5 py-3 w-40 text-right">Thao tác</th>
            </tr>
          </thead>
          <tbody class="divide-y divide-gray-50">
            <tr
              v-for="tk in filteredList"
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
                  {{ tk.hoTen || "—" }}
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
              <td class="px-5 py-3 text-right">
                <button
                  v-if="tk.vaiTro !== 'admin'"
                  @click="changeRole(tk, 'admin')"
                  class="p-1.5 rounded-lg text-gray-400 hover:text-purple-600 hover:bg-purple-50 transition-colors mr-1"
                  title="Cấp quyền Admin"
                >
                  <ShieldAlertIcon class="h-4 w-4" stroke-width="2" />
                </button>
                <button
                  v-if="tk.vaiTro === 'admin'"
                  @click="changeRole(tk, 'customer')"
                  class="p-1.5 rounded-lg text-gray-400 hover:text-blue-600 hover:bg-blue-50 transition-colors mr-1"
                  title="Hạ quyền Khách hàng"
                >
                  <ShieldCloseIcon class="h-4 w-4" stroke-width="2" />
                </button>
                <button
                  @click="confirmAction(tk)"
                  class="p-1.5 rounded-lg text-gray-400 hover:text-red-500 hover:bg-red-50 transition-colors"
                  title="Khóa/Xóa tài khoản"
                >
                  <LockIcon class="h-4 w-4" stroke-width="2" />
                </button>
              </td>
            </tr>
            <tr v-if="filteredList.length === 0">
              <td
                colspan="4"
                class="px-5 py-10 text-center text-gray-400 text-sm"
              >
                Không tìm thấy tài khoản nào.
              </td>
            </tr>
          </tbody>
        </table>
      </div>

      <!-- Mobile View -->
      <div class="md:hidden divide-y divide-gray-50">
        <div
          v-for="tk in filteredList"
          :key="tk.maTaiKhoan"
          class="p-4 flex flex-col gap-2"
        >
          <div class="flex justify-between items-start">
            <div>
              <p class="text-sm font-semibold text-gray-800">
                {{ tk.tenDangNhap }}
              </p>
              <p class="text-xs text-gray-400">
                {{ tk.hoTen || "Chưa cập nhật tên" }}
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
              <span v-if="tk.email" class="block">{{ tk.email }}</span>
              <span v-if="tk.soDienThoai" class="block">{{
                tk.soDienThoai
              }}</span>
            </div>
            <div class="flex space-x-1">
              <button
                v-if="tk.vaiTro !== 'admin'"
                @click="changeRole(tk, 'admin')"
                class="p-1.5 rounded bg-gray-50 text-gray-500 hover:text-purple-600"
              >
                <ShieldAlertIcon class="h-4 w-4" />
              </button>
              <button
                v-if="tk.vaiTro === 'admin'"
                @click="changeRole(tk, 'customer')"
                class="p-1.5 rounded bg-gray-50 text-gray-500 hover:text-blue-600"
              >
                <ShieldCloseIcon class="h-4 w-4" />
              </button>
              <button
                @click="confirmAction(tk)"
                class="p-1.5 rounded bg-red-50 text-red-500 hover:bg-red-100"
              >
                <LockIcon class="h-4 w-4" />
              </button>
            </div>
          </div>
        </div>
      </div>
    </div>

    <!-- Confirm Modal -->
    <teleport to="body"
      ><transition name="modal">
        <div
          v-if="showConfirm"
          class="fixed inset-0 z-50 flex items-center justify-center p-4 bg-black/40"
          @click.self="showConfirm = false"
        >
          <div
            class="bg-white rounded-xl shadow-2xl w-full max-w-sm p-6 text-center"
          >
            <LockIcon
              class="h-10 w-10 text-red-400 mx-auto mb-3"
              stroke-width="1.5"
            />
            <h4 class="text-lg font-bold text-gray-800 mb-1">Khóa tài khoản</h4>
            <p class="text-sm text-gray-500">
              Hành động này sẽ khóa/xóa tài khoản
              <strong class="text-gray-700">{{
                actionTarget?.tenDangNhap
              }}</strong
              >.<br />Bạn có chắc chắn?
            </p>
            <div class="flex justify-center space-x-2 mt-5">
              <button
                @click="showConfirm = false"
                class="px-4 py-2 text-sm font-medium text-gray-600 hover:bg-gray-100 rounded-lg transition"
              >
                Hủy
              </button>
              <button
                @click="handleAction"
                :disabled="processing"
                class="px-5 py-2 text-sm font-semibold text-white bg-red-500 hover:bg-red-600 rounded-lg shadow-sm transition disabled:opacity-50"
              >
                {{ processing ? "Đang xử lý..." : "Xác nhận khóa" }}
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
  Search as SearchIcon,
  Loader2 as Loader2Icon,
  Mail as MailIcon,
  Phone as PhoneIcon,
  Lock as LockIcon,
  ShieldAlert as ShieldAlertIcon,
  ShieldClose as ShieldCloseIcon,
} from "lucide-vue-next";
import { userService } from "../../services/user.service";
import { useToast } from "../../composables/useToast";
import Toast from "../../components/admin/Toast.vue";

const { success, error } = useToast();
const list = ref<any[]>([]);
const loading = ref(true);
const processing = ref(false);
const showConfirm = ref(false);
const actionTarget = ref<any>(null);
const searchQuery = ref("");

const filteredList = computed(() => {
  if (!searchQuery.value.trim()) return list.value;
  const q = searchQuery.value.trim().toLowerCase();
  return list.value.filter(
    (tk) =>
      tk.tenDangNhap?.toLowerCase().includes(q) ||
      tk.hoTen?.toLowerCase().includes(q) ||
      tk.email?.toLowerCase().includes(q),
  );
});

const fetchAll = async () => {
  loading.value = true;
  try {
    const res: any = await userService.getAll();
    if (res.success) list.value = res.data || [];
  } catch {
    error("Lỗi tải danh sách người dùng");
  } finally {
    loading.value = false;
  }
};

const changeRole = async (tk: any, newRole: string) => {
  const conf = confirm(
    `Bạn có chắc muốn cấp vai trò ${newRole} cho tài khoản ${tk.tenDangNhap}?`,
  );
  if (!conf) return;
  processing.value = true;
  try {
    const res: any = await userService.updateRole(tk.maTaiKhoan, newRole);
    if (res.success) {
      success("Cập nhật quyền thành công");
      fetchAll();
    } else error(res.message);
  } catch (e: any) {
    error(e.response?.data?.message || "Lỗi cập nhật quyền");
  } finally {
    processing.value = false;
  }
};

const confirmAction = (tk: any) => {
  actionTarget.value = tk;
  showConfirm.value = true;
};

const handleAction = async () => {
  if (!actionTarget.value) return;
  processing.value = true;
  try {
    const res: any = await userService.deleteOrLock(
      actionTarget.value.maTaiKhoan,
    );
    if (res.success !== false) {
      success("Khóa/Xóa tài khoản thành công");
      showConfirm.value = false;
      fetchAll();
    } else error(res.message);
  } catch (e: any) {
    error(e.response?.data?.message || e.response?.data || "Lỗi thao tác");
  } finally {
    processing.value = false;
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
