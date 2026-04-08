<template>
  <div>
    <Toast />
    <!-- Header -->
    <div class="flex items-center space-x-3 mb-6">
      <button
        @click="$router.push('/admin/products')"
        class="p-2 rounded-lg text-gray-400 hover:bg-gray-100 hover:text-gray-700 transition-colors"
      >
        <ArrowLeftIcon class="h-5 w-5" stroke-width="2" />
      </button>
      <div>
        <h3 class="text-xl font-bold text-[#1F2937]">
          {{ isEdit ? "Sửa sản phẩm" : "Thêm sản phẩm mới" }}
        </h3>
        <p class="text-sm text-gray-500 mt-0.5">
          {{
            isEdit
              ? `Mã: ${route.params.id}`
              : "Sản phẩm phải có ít nhất 1 biến thể."
          }}
        </p>
      </div>
    </div>

    <div v-if="loadingData" class="flex justify-center py-16 text-gray-400">
      <Loader2Icon class="h-7 w-7 animate-spin" />
    </div>

    <template v-else>
      <div class="grid grid-cols-1 lg:grid-cols-3 gap-5">
        <!-- Main Form -->
        <div class="lg:col-span-2 space-y-5">
          <!-- Thông tin chính -->
          <div class="bg-white rounded-xl border border-gray-200 p-5">
            <h4
              class="text-sm font-bold text-gray-800 uppercase tracking-wide mb-4"
            >
              Thông tin chính
            </h4>
            <div class="space-y-4">
              <div class="grid grid-cols-1 sm:grid-cols-2 gap-4">
                <div>
                  <label
                    class="block text-xs font-bold text-gray-500 uppercase mb-1.5"
                    >Mã SP *</label
                  >
                  <input
                    v-model="form.maSp"
                    type="text"
                    :disabled="isEdit"
                    placeholder="VD: SP001"
                    class="w-full px-3.5 py-2.5 border border-gray-200 rounded-lg text-sm focus:outline-none focus:ring-2 focus:ring-blue-500/20 focus:border-blue-400 disabled:bg-gray-50 disabled:text-gray-500"
                  />
                </div>
                <div>
                  <label
                    class="block text-xs font-bold text-gray-500 uppercase mb-1.5"
                    >Danh mục</label
                  >
                  <select
                    v-model="form.maDanhMuc"
                    class="w-full px-3.5 py-2.5 border border-gray-200 rounded-lg text-sm focus:outline-none focus:ring-2 focus:ring-blue-500/20 focus:border-blue-400"
                  >
                    <option :value="null">-- Chọn danh mục --</option>
                    <option
                      v-for="dm in categories"
                      :key="dm.maDanhMuc"
                      :value="dm.maDanhMuc"
                    >
                      {{ dm.tenDanhMuc }}
                    </option>
                  </select>
                </div>
              </div>
              <div>
                <label
                  class="block text-xs font-bold text-gray-500 uppercase mb-1.5"
                  >Tên sản phẩm *</label
                >
                <input
                  v-model="form.tenSp"
                  type="text"
                  placeholder="Nhập tên sản phẩm"
                  class="w-full px-3.5 py-2.5 border border-gray-200 rounded-lg text-sm focus:outline-none focus:ring-2 focus:ring-blue-500/20 focus:border-blue-400"
                />
              </div>
              <div class="grid grid-cols-1 sm:grid-cols-2 gap-4">
                <div>
                  <label
                    class="block text-xs font-bold text-gray-500 uppercase mb-1.5"
                    >Giá gốc *</label
                  >
                  <input
                    v-model.number="form.giaGoc"
                    type="number"
                    min="0"
                    placeholder="0"
                    class="w-full px-3.5 py-2.5 border border-gray-200 rounded-lg text-sm focus:outline-none focus:ring-2 focus:ring-blue-500/20 focus:border-blue-400"
                  />
                </div>
                <div>
                  <label
                    class="block text-xs font-bold text-gray-500 uppercase mb-1.5"
                    >Chất liệu</label
                  >
                  <input
                    v-model="form.chatLieu"
                    type="text"
                    placeholder="VD: Cotton, Polyester"
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
                  rows="3"
                  placeholder="Mô tả sản phẩm..."
                  class="w-full px-3.5 py-2.5 border border-gray-200 rounded-lg text-sm focus:outline-none focus:ring-2 focus:ring-blue-500/20 focus:border-blue-400 resize-none"
                ></textarea>
              </div>
            </div>
          </div>

          <!-- Biến thể hiện tại (chỉ khi edit) -->
          <div
            v-if="isEdit && existingVariants.length > 0"
            class="bg-white rounded-xl border border-gray-200 p-5"
          >
            <h4
              class="text-sm font-bold text-gray-800 uppercase tracking-wide mb-4"
            >
              Biến thể hiện tại ({{ existingVariants.length }})
            </h4>
            <div class="overflow-x-auto border border-gray-200 rounded-lg">
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
                    v-for="v in existingVariants"
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
                </tbody>
              </table>
            </div>
          </div>

          <!-- Thêm biến thể mới -->
          <div class="bg-white rounded-xl border border-gray-200 p-5">
            <div class="flex items-center justify-between mb-4">
              <h4
                class="text-sm font-bold text-gray-800 uppercase tracking-wide"
              >
                {{ isEdit ? "Thêm biến thể mới" : "Biến thể sản phẩm *" }}
              </h4>
              <button
                @click="addVariant"
                class="text-xs font-semibold text-blue-600 hover:text-blue-700 transition"
              >
                + Thêm biến thể
              </button>
            </div>
            <div v-if="form.dsBienThe.length > 0" class="space-y-3">
              <div
                v-for="(v, i) in form.dsBienThe"
                :key="i"
                class="p-3 border border-gray-100 rounded-lg bg-gray-50/50"
              >
                <div class="grid grid-cols-2 sm:grid-cols-4 gap-3">
                  <div>
                    <label
                      class="block text-[10px] font-bold text-gray-400 uppercase mb-1"
                      >Size *</label
                    >
                    <select
                      v-model="v.maKichThuoc"
                      class="w-full px-2.5 py-2 border border-gray-200 rounded-lg text-xs focus:outline-none focus:ring-2 focus:ring-blue-500/20"
                    >
                      <option
                        v-for="s in sizes"
                        :key="s.maSize"
                        :value="s.maSize"
                      >
                        {{ s.tenSize }}
                      </option>
                    </select>
                  </div>
                  <div>
                    <label
                      class="block text-[10px] font-bold text-gray-400 uppercase mb-1"
                      >Màu *</label
                    >
                    <select
                      v-model="v.maMauSac"
                      class="w-full px-2.5 py-2 border border-gray-200 rounded-lg text-xs focus:outline-none focus:ring-2 focus:ring-blue-500/20"
                    >
                      <option
                        v-for="m in colors"
                        :key="m.maMau"
                        :value="m.maMau"
                      >
                        {{ m.tenMau }}
                      </option>
                    </select>
                  </div>
                  <div>
                    <label
                      class="block text-[10px] font-bold text-gray-400 uppercase mb-1"
                      >Số lượng *</label
                    >
                    <input
                      v-model.number="v.soLuongTon"
                      type="number"
                      min="0"
                      class="w-full px-2.5 py-2 border border-gray-200 rounded-lg text-xs focus:outline-none focus:ring-2 focus:ring-blue-500/20"
                    />
                  </div>
                  <div class="flex items-end space-x-2">
                    <div class="flex-1">
                      <label
                        class="block text-[10px] font-bold text-gray-400 uppercase mb-1"
                        >Giá bán</label
                      >
                      <input
                        v-model.number="v.giaBan"
                        type="number"
                        min="0"
                        class="w-full px-2.5 py-2 border border-gray-200 rounded-lg text-xs focus:outline-none focus:ring-2 focus:ring-blue-500/20"
                      />
                    </div>
                    <button
                      @click="form.dsBienThe.splice(i, 1)"
                      class="p-2 rounded-lg text-red-400 hover:bg-red-50 transition-colors mb-0.5"
                    >
                      <Trash2Icon class="h-3.5 w-3.5" stroke-width="2" />
                    </button>
                  </div>
                </div>
              </div>
            </div>
            <div v-else class="py-6 text-center text-gray-400 text-sm">
              {{
                isEdit
                  ? "Thêm biến thể mới nếu cần."
                  : 'Phải có ít nhất 1 biến thể. Bấm "Thêm biến thể".'
              }}
            </div>
          </div>
        </div>

        <!-- Sidebar -->
        <div class="space-y-5">
          <!-- Ảnh -->
          <div class="bg-white rounded-xl border border-gray-200 p-5">
            <h4
              class="text-sm font-bold text-gray-800 uppercase tracking-wide mb-4"
            >
              Hình ảnh
            </h4>
            <!-- Ảnh hiện tại (edit mode) -->
            <div v-if="isEdit && currentImages.length > 0" class="mb-3">
              <p class="text-[10px] font-bold text-gray-400 uppercase mb-1.5">
                Ảnh hiện tại
              </p>
              <div class="grid grid-cols-3 gap-2">
                <div
                  v-for="(img, i) in currentImages"
                  :key="i"
                  class="rounded-lg overflow-hidden aspect-square bg-gray-100"
                >
                  <img :src="img" class="w-full h-full object-cover" />
                </div>
              </div>
            </div>
            <input
              ref="fileInput"
              type="file"
              multiple
              accept="image/*"
              class="hidden"
              @change="handleFileChange"
            />
            <button
              @click="($refs.fileInput as HTMLInputElement).click()"
              class="w-full py-5 border-2 border-dashed border-gray-200 rounded-lg text-sm text-gray-400 hover:border-blue-400 hover:text-blue-500 transition-colors"
            >
              <UploadCloudIcon
                class="h-6 w-6 mx-auto mb-1"
                stroke-width="1.5"
              />
              {{ isEdit ? "Thêm ảnh mới" : "Chọn ảnh sản phẩm" }}
            </button>
            <div
              v-if="previewImages.length > 0"
              class="grid grid-cols-3 gap-2 mt-3"
            >
              <div
                v-for="(img, i) in previewImages"
                :key="i"
                class="relative group rounded-lg overflow-hidden aspect-square bg-gray-100"
              >
                <img :src="img" class="w-full h-full object-cover" />
                <button
                  @click="removeImage(i)"
                  class="absolute top-1 right-1 p-1 rounded-full bg-red-500 text-white opacity-0 group-hover:opacity-100 transition-opacity"
                >
                  <XIcon class="h-3 w-3" stroke-width="3" />
                </button>
              </div>
            </div>
          </div>

          <!-- Submit -->
          <div class="bg-white rounded-xl border border-gray-200 p-5">
            <button
              @click="handleSubmit"
              :disabled="saving"
              class="w-full py-3 rounded-lg bg-blue-600 text-white text-sm font-bold hover:bg-blue-700 shadow-sm transition disabled:opacity-50"
            >
              {{
                saving
                  ? "Đang lưu..."
                  : isEdit
                    ? "Cập nhật sản phẩm"
                    : "Tạo sản phẩm"
              }}
            </button>
            <button
              @click="$router.push('/admin/products')"
              class="w-full py-2.5 mt-2 rounded-lg text-sm font-medium text-gray-500 hover:bg-gray-100 transition"
            >
              Hủy bỏ
            </button>
          </div>
        </div>
      </div>
    </template>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from "vue";
import { useRoute, useRouter } from "vue-router";
import {
  ArrowLeft as ArrowLeftIcon,
  Trash2 as Trash2Icon,
  UploadCloud as UploadCloudIcon,
  X as XIcon,
  Loader2 as Loader2Icon,
} from "lucide-vue-next";
import { sanPhamService } from "../../services/sanpham.service";
import { mauSizeService } from "../../services/mausize.service";
import { useToast } from "../../composables/useToast";
import Toast from "../../components/admin/Toast.vue";
import api from "../../services/api";

const route = useRoute();
const router = useRouter();
const { success, error } = useToast();

const isEdit = computed(() => !!route.params.id);
const loadingData = ref(false);
const saving = ref(false);
const sizes = ref<any[]>([]);
const colors = ref<any[]>([]);
const categories = ref<any[]>([]);
const selectedFiles = ref<File[]>([]);
const previewImages = ref<string[]>([]);
const currentImages = ref<string[]>([]);
const existingVariants = ref<any[]>([]);

const form = ref({
  maSp: "",
  tenSp: "",
  maDanhMuc: null as number | null,
  giaGoc: 0,
  chatLieu: "",
  moTa: "",
  dsBienThe: [] as {
    maKichThuoc: number;
    maMauSac: number;
    soLuongTon: number;
    giaBan: number;
  }[],
});

const formatMoney = (val: number) =>
  val ? val.toLocaleString("vi-VN") + " ₫" : "0 ₫";

const addVariant = () => {
  const newSize = sizes.value[0]?.maSize || 0;
  const newColor = colors.value[0]?.maMau || 0;

  // Check duplicate in new variants
  const dupInNew = form.value.dsBienThe.some(
    (v) => v.maKichThuoc === newSize && v.maMauSac === newColor,
  );
  // Check duplicate in existing variants
  const sizeLabel =
    sizes.value.find((s) => s.maSize === newSize)?.tenSize || "";
  const colorLabel =
    colors.value.find((c) => c.maMau === newColor)?.tenMau || "";
  const dupInExisting = existingVariants.value.some(
    (v) => v.tenSize === sizeLabel && v.tenMau === colorLabel,
  );

  if (dupInNew || dupInExisting) {
    error(
      `Biến thể ${sizeLabel} - ${colorLabel} đã tồn tại! Hãy chọn Size/Màu khác.`,
    );
    return;
  }

  form.value.dsBienThe.push({
    maKichThuoc: newSize,
    maMauSac: newColor,
    soLuongTon: 0,
    giaBan: form.value.giaGoc || 0,
  });
};

const handleFileChange = (e: Event) => {
  const files = (e.target as HTMLInputElement).files;
  if (!files) return;
  for (const f of Array.from(files)) {
    selectedFiles.value.push(f);
    previewImages.value.push(URL.createObjectURL(f));
  }
};

const removeImage = (i: number) => {
  selectedFiles.value.splice(i, 1);
  previewImages.value.splice(i, 1);
};

const handleSubmit = async () => {
  if (!form.value.maSp.trim() || !form.value.tenSp.trim()) {
    error("Mã SP và Tên SP không được để trống");
    return;
  }
  if (!isEdit.value && form.value.dsBienThe.length === 0) {
    error("Sản phẩm mới phải có ít nhất 1 biến thể");
    return;
  }

  // Check for duplicate variants in the new list
  for (let i = 0; i < form.value.dsBienThe.length; i++) {
    for (let j = i + 1; j < form.value.dsBienThe.length; j++) {
      if (
        form.value.dsBienThe[i].maKichThuoc ===
          form.value.dsBienThe[j].maKichThuoc &&
        form.value.dsBienThe[i].maMauSac === form.value.dsBienThe[j].maMauSac
      ) {
        const s = sizes.value.find(
          (x) => x.maSize === form.value.dsBienThe[i].maKichThuoc,
        )?.tenSize;
        const c = colors.value.find(
          (x) => x.maMau === form.value.dsBienThe[i].maMauSac,
        )?.tenMau;
        error(`Có biến thể trùng lặp: ${s} - ${c}. Vui lòng kiểm tra lại.`);
        return;
      }
    }
  }

  // Check new variants against existing
  for (const nv of form.value.dsBienThe) {
    const sizeLabel =
      sizes.value.find((s) => s.maSize === nv.maKichThuoc)?.tenSize || "";
    const colorLabel =
      colors.value.find((c) => c.maMau === nv.maMauSac)?.tenMau || "";
    if (
      existingVariants.value.some(
        (ev) => ev.tenSize === sizeLabel && ev.tenMau === colorLabel,
      )
    ) {
      error(`Biến thể ${sizeLabel} - ${colorLabel} đã tồn tại trong sản phẩm!`);
      return;
    }
  }

  saving.value = true;
  try {
    const fd = new FormData();
    fd.append("MaSp", form.value.maSp);
    fd.append("TenSp", form.value.tenSp);
    if (form.value.maDanhMuc)
      fd.append("MaDanhMuc", String(form.value.maDanhMuc));
    fd.append("GiaGoc", String(form.value.giaGoc));
    if (form.value.chatLieu) fd.append("ChatLieu", form.value.chatLieu);
    if (form.value.moTa) fd.append("MoTa", form.value.moTa);
    fd.append("DsBienThe", JSON.stringify(form.value.dsBienThe));
    for (const f of selectedFiles.value) fd.append("AnhSps", f);

    const res: any = isEdit.value
      ? await sanPhamService.update(fd)
      : await sanPhamService.create(fd);

    if (res.success) {
      success(
        isEdit.value
          ? "Cập nhật sản phẩm thành công"
          : "Thêm sản phẩm thành công",
      );
      router.push("/admin/products");
    } else {
      error(res.message || "Lỗi");
    }
  } catch (e: any) {
    error(e.response?.data?.message || "Lỗi thao tác");
  } finally {
    saving.value = false;
  }
};

const loadOptions = async () => {
  try {
    const [sRes, cRes, dmRes]: any[] = await Promise.all([
      mauSizeService.getAllSize(),
      mauSizeService.getAllMau(),
      api.get("/DanhMuc"),
    ]);
    if (sRes.success) sizes.value = sRes.data || [];
    if (cRes.success) colors.value = cRes.data || [];
    if (dmRes.success) categories.value = dmRes.data || [];
  } catch (e) {
    console.warn("Error loading options:", e);
  }
};

const loadProduct = async () => {
  if (!isEdit.value) return;
  loadingData.value = true;
  try {
    const res: any = await sanPhamService.getFullInfo(
      route.params.id as string,
    );
    if (res.success && res.data) {
      const sp = res.data;
      form.value.maSp = sp.maSp;
      form.value.tenSp = sp.tenSp;
      form.value.giaGoc = sp.giaGoc;
      form.value.chatLieu = sp.chatLieu || "";
      form.value.moTa = sp.moTa || "";
      currentImages.value = sp.duongDanAnhSPs || [];
      existingVariants.value = sp.dsBienThe || [];

      // Map tenDanhMuc back to maDanhMuc
      if (sp.tenDanhMuc) {
        const matched = categories.value.find(
          (dm: any) => dm.tenDanhMuc === sp.tenDanhMuc,
        );
        if (matched) form.value.maDanhMuc = matched.maDanhMuc;
      }
    }
  } catch (e) {
    error("Lỗi tải sản phẩm");
  } finally {
    loadingData.value = false;
  }
};

onMounted(async () => {
  await loadOptions();
  await loadProduct();
});
</script>
