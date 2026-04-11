<template>
  <div>
    <Toast />

    <div class="flex items-center gap-3 mb-6">
      <button
        type="button"
        @click="router.push('/admin/products')"
        class="inline-flex h-10 w-10 items-center justify-center rounded-xl border border-[var(--ui-border)] bg-white text-slate-500 transition hover:border-[var(--ui-border-strong)] hover:text-slate-900"
      >
        <ArrowLeftIcon class="h-4 w-4" />
      </button>
      <div>
        <h3 class="text-xl font-bold text-slate-900">
          {{ isEdit ? "Cập nhật sản phẩm" : "Tạo sản phẩm mới" }}
        </h3>
        <p class="text-sm text-slate-500">
          {{ isEdit ? form.maSp : "Lưu thông tin cơ bản, biến thể và hình ảnh theo từng bước rõ ràng." }}
        </p>
      </div>
    </div>

    <div v-if="loadingData" class="flex justify-center py-16 text-slate-400">
      <Loader2Icon class="h-8 w-8 animate-spin" />
    </div>

    <div v-else class="grid grid-cols-1 gap-6 xl:grid-cols-[minmax(0,1fr)_360px]">
      <section class="space-y-6">
        <article class="rounded-3xl border border-[var(--ui-border)] bg-white p-5 shadow-[var(--shadow-soft)]">
          <div class="mb-4 flex items-center justify-between gap-3">
            <div>
              <h4 class="text-sm font-semibold uppercase tracking-[0.16em] text-slate-500">
                Thông tin cơ bản
              </h4>
              <p class="mt-1 text-sm text-slate-500">
                Các trường bắt buộc và dữ liệu hiển thị trên client.
              </p>
            </div>
            <span class="rounded-full bg-slate-100 px-3 py-1 text-xs font-semibold text-slate-600">
              {{ isEdit ? "Edit" : "Create" }}
            </span>
          </div>

          <div class="grid grid-cols-1 gap-4 md:grid-cols-2">
            <div>
              <label class="mb-1.5 block text-sm font-semibold text-slate-700">Mã SP</label>
              <input
                v-model="form.maSp"
                :disabled="isEdit"
                type="text"
                placeholder="VD: SP001"
                :class="inputClass('maSp')"
              />
              <p v-if="fieldError('maSp')" class="mt-1 text-xs font-medium text-rose-600">
                {{ fieldError("maSp") }}
              </p>
            </div>

            <div>
              <label class="mb-1.5 block text-sm font-semibold text-slate-700">Danh mục</label>
              <select v-model="form.maDanhMuc" :class="inputClass('maDanhMuc')">
                <option :value="null">Chọn danh mục</option>
                <option v-for="category in categories" :key="category.maDanhMuc" :value="category.maDanhMuc">
                  {{ categoryLabel(category) }}
                </option>
              </select>
              <p
                v-if="fieldError('maDanhMuc')"
                class="mt-1 text-xs font-medium text-rose-600"
              >
                {{ fieldError("maDanhMuc") }}
              </p>
            </div>

            <div class="md:col-span-2">
              <label class="mb-1.5 block text-sm font-semibold text-slate-700">Tên sản phẩm</label>
              <input
                v-model="form.tenSp"
                type="text"
                placeholder="Nhập tên sản phẩm"
                :class="inputClass('tenSp')"
              />
              <p v-if="fieldError('tenSp')" class="mt-1 text-xs font-medium text-rose-600">
                {{ fieldError("tenSp") }}
              </p>
            </div>

            <div>
              <label class="mb-1.5 block text-sm font-semibold text-slate-700">Giá gốc</label>
              <input
                v-model.number="form.giaGoc"
                min="0"
                type="number"
                placeholder="0"
                :class="inputClass('giaGoc')"
              />
              <p v-if="fieldError('giaGoc')" class="mt-1 text-xs font-medium text-rose-600">
                {{ fieldError("giaGoc") }}
              </p>
            </div>

            <div>
              <label class="mb-1.5 block text-sm font-semibold text-slate-700">Chất liệu</label>
              <input
                v-model="form.chatLieu"
                type="text"
                placeholder="VD: Cotton"
                :class="inputClass()"
              />
            </div>

            <div class="md:col-span-2">
              <label class="mb-1.5 block text-sm font-semibold text-slate-700">Mô tả</label>
              <textarea
                v-model="form.moTa"
                rows="4"
                placeholder="Mô tả ngắn gọn cho sản phẩm"
                :class="textareaClass()"
              />
            </div>
          </div>
        </article>

        <article
          v-if="isEdit && form.existingVariants.length"
          class="rounded-3xl border border-[var(--ui-border)] bg-white p-5 shadow-[var(--shadow-soft)]"
        >
          <div class="mb-4 flex items-center justify-between gap-3">
            <div>
              <h4 class="text-sm font-semibold uppercase tracking-[0.16em] text-slate-500">
                Bien the hien co
              </h4>
              <p class="mt-1 text-sm text-slate-500">
                Chi sua gia ban va ton kho. Mau/size da luu se duoc giu nguyen.
              </p>
            </div>
            <span class="rounded-full bg-slate-100 px-3 py-1 text-xs font-semibold text-slate-600">
              {{ form.existingVariants.length }} bien the
            </span>
          </div>

          <div class="space-y-3">
            <div
              v-for="(variant, index) in form.existingVariants"
              :key="variant.maChiTietSP"
              class="rounded-2xl border border-[var(--ui-border)] bg-slate-50/80 p-4"
            >
              <div class="grid grid-cols-1 gap-3 md:grid-cols-[1.2fr_1.2fr_1fr_1fr]">
                <div>
                  <label class="mb-1 block text-xs font-semibold uppercase tracking-[0.16em] text-slate-400">
                    Size
                  </label>
                  <div class="rounded-2xl border border-[var(--ui-border)] bg-white px-3 py-2.5 text-sm font-medium text-slate-700">
                    {{ variant.tenSize }}
                  </div>
                </div>

                <div>
                  <label class="mb-1 block text-xs font-semibold uppercase tracking-[0.16em] text-slate-400">
                    Mau
                  </label>
                  <div class="rounded-2xl border border-[var(--ui-border)] bg-white px-3 py-2.5 text-sm font-medium text-slate-700">
                    {{ variant.tenMau }}
                  </div>
                </div>

                <div>
                  <label class="mb-1 block text-xs font-semibold uppercase tracking-[0.16em] text-slate-400">
                    Ton kho
                  </label>
                  <input
                    v-model.number="variant.soLuongTon"
                    min="0"
                    type="number"
                    :class="compactInputClass(`existingVariants.${index}.soLuongTon`)"
                  />
                  <p
                    v-if="fieldError(`existingVariants.${index}.soLuongTon`)"
                    class="mt-1 text-[11px] font-medium text-rose-600"
                  >
                    {{ fieldError(`existingVariants.${index}.soLuongTon`) }}
                  </p>
                </div>

                <div>
                  <label class="mb-1 block text-xs font-semibold uppercase tracking-[0.16em] text-slate-400">
                    Gia ban
                  </label>
                  <input
                    v-model.number="variant.giaBan"
                    min="0"
                    type="number"
                    :class="compactInputClass(`existingVariants.${index}.giaBan`)"
                  />
                  <p
                    v-if="fieldError(`existingVariants.${index}.giaBan`)"
                    class="mt-1 text-[11px] font-medium text-rose-600"
                  >
                    {{ fieldError(`existingVariants.${index}.giaBan`) }}
                  </p>
                </div>
              </div>
            </div>
          </div>
        </article>

        <article class="rounded-3xl border border-[var(--ui-border)] bg-white p-5 shadow-[var(--shadow-soft)]">
          <div class="mb-4 flex items-center justify-between gap-3">
            <div>
              <h4 class="text-sm font-semibold uppercase tracking-[0.16em] text-slate-500">
                {{ isEdit ? "Thêm biến thể mới" : "Biến thể sản phẩm" }}
              </h4>
              <p class="mt-1 text-sm text-slate-500">
                Kiem tra trung mau/size ngay tren form, khong cho gui du lieu trung lap.
              </p>
            </div>
            <button
              type="button"
              @click="addVariant"
              class="inline-flex items-center gap-2 rounded-full border border-slate-200 bg-slate-50 px-4 py-2 text-sm font-semibold text-slate-700 transition hover:border-slate-300 hover:bg-slate-100"
            >
              <PlusIcon class="h-4 w-4" />
              Thêm biến thể
            </button>
          </div>

          <p v-if="fieldError('newVariants')" class="mb-3 text-sm font-medium text-rose-600">
            {{ fieldError("newVariants") }}
          </p>

          <div v-if="form.newVariants.length" class="space-y-3">
            <div
              v-for="(variant, index) in form.newVariants"
              :key="`${variant.maKichThuoc}-${variant.maMauSac}-${index}`"
              class="rounded-2xl border border-[var(--ui-border)] bg-slate-50/80 p-4"
            >
              <div class="grid grid-cols-1 gap-3 md:grid-cols-[1.1fr_1.1fr_0.9fr_0.9fr_auto]">
                <div>
                  <label class="mb-1 block text-xs font-semibold uppercase tracking-[0.16em] text-slate-400">
                    Size
                  </label>
                  <select
                    v-model="variant.maKichThuoc"
                    :class="compactInputClass(`newVariants.${index}.maKichThuoc`)"
                  >
                    <option v-for="size in sizes" :key="size.maSize" :value="size.maSize">
                      {{ size.tenSize }}
                    </option>
                  </select>
                  <p
                    v-if="fieldError(`newVariants.${index}.maKichThuoc`)"
                    class="mt-1 text-[11px] font-medium text-rose-600"
                  >
                    {{ fieldError(`newVariants.${index}.maKichThuoc`) }}
                  </p>
                </div>

                <div>
                  <label class="mb-1 block text-xs font-semibold uppercase tracking-[0.16em] text-slate-400">
                    Mau
                  </label>
                  <select
                    v-model="variant.maMauSac"
                    :class="compactInputClass(`newVariants.${index}.maMauSac`)"
                  >
                    <option v-for="color in colors" :key="color.maMau" :value="color.maMau">
                      {{ color.tenMau }}
                    </option>
                  </select>
                  <p
                    v-if="fieldError(`newVariants.${index}.maMauSac`)"
                    class="mt-1 text-[11px] font-medium text-rose-600"
                  >
                    {{ fieldError(`newVariants.${index}.maMauSac`) }}
                  </p>
                </div>

                <div>
                  <label class="mb-1 block text-xs font-semibold uppercase tracking-[0.16em] text-slate-400">
                    Ton kho
                  </label>
                  <input
                    v-model.number="variant.soLuongTon"
                    min="0"
                    type="number"
                    :class="compactInputClass(`newVariants.${index}.soLuongTon`)"
                  />
                  <p
                    v-if="fieldError(`newVariants.${index}.soLuongTon`)"
                    class="mt-1 text-[11px] font-medium text-rose-600"
                  >
                    {{ fieldError(`newVariants.${index}.soLuongTon`) }}
                  </p>
                </div>

                <div>
                  <label class="mb-1 block text-xs font-semibold uppercase tracking-[0.16em] text-slate-400">
                    Gia ban
                  </label>
                  <input
                    v-model.number="variant.giaBan"
                    min="0"
                    type="number"
                    :class="compactInputClass(`newVariants.${index}.giaBan`)"
                  />
                  <p
                    v-if="fieldError(`newVariants.${index}.giaBan`)"
                    class="mt-1 text-[11px] font-medium text-rose-600"
                  >
                    {{ fieldError(`newVariants.${index}.giaBan`) }}
                  </p>
                </div>

                <div class="flex items-end justify-end">
                  <button
                    type="button"
                    @click="removeVariant(index)"
                    class="inline-flex h-11 w-11 items-center justify-center rounded-2xl border border-rose-200 bg-white text-rose-500 transition hover:bg-rose-50"
                  >
                    <Trash2Icon class="h-4 w-4" />
                  </button>
                </div>
              </div>

              <p
                v-if="fieldError(`newVariants.${index}.duplicate`)"
                class="mt-2 text-xs font-medium text-rose-600"
              >
                {{ fieldError(`newVariants.${index}.duplicate`) }}
              </p>
            </div>
          </div>

          <div
            v-else
            class="rounded-2xl border border-dashed border-[var(--ui-border)] bg-slate-50 px-4 py-8 text-center text-sm text-slate-500"
          >
            {{ isEdit ? "Chua co bien the moi duoc them." : "San pham moi bat buoc phai co it nhat 1 bien the." }}
          </div>
        </article>
      </section>

      <aside class="space-y-6">
        <article class="rounded-3xl border border-[var(--ui-border)] bg-white p-5 shadow-[var(--shadow-soft)]">
          <div class="mb-4 flex items-center justify-between gap-3">
            <div>
              <h4 class="text-sm font-semibold uppercase tracking-[0.16em] text-slate-500">
                Hinh anh
              </h4>
              <p class="mt-1 text-sm text-slate-500">
                {{ isEdit ? "Xóa ảnh cũ, thêm ảnh mới qua AnhController." : "Upload ảnh cùng lúc tạo sản phẩm." }}
              </p>
            </div>
            <button
              type="button"
              @click="fileInput?.click()"
              class="inline-flex items-center gap-2 rounded-full border border-slate-200 bg-slate-50 px-4 py-2 text-sm font-semibold text-slate-700 transition hover:border-slate-300 hover:bg-slate-100"
            >
              <UploadCloudIcon class="h-4 w-4" />
              Chon anh
            </button>
          </div>

          <input
            ref="fileInput"
            accept="image/*"
            class="hidden"
            multiple
            type="file"
            @change="handleFileChange"
          />

          <div v-if="existingImages.length" class="mb-5">
            <p class="mb-2 text-xs font-semibold uppercase tracking-[0.16em] text-slate-400">
              Anh hien co
            </p>
            <div class="grid grid-cols-2 gap-3 sm:grid-cols-3">
              <div
                v-for="image in existingImages"
                :key="image.maAnh"
                class="group relative overflow-hidden rounded-2xl border border-[var(--ui-border)] bg-slate-100"
              >
                <img :src="image.duongDanAnh" class="aspect-square h-full w-full object-cover" />
                <button
                  type="button"
                  @click="queueDeleteImage(image.maAnh)"
                  class="absolute right-2 top-2 inline-flex h-8 w-8 items-center justify-center rounded-full bg-white/90 text-rose-500 opacity-0 shadow transition group-hover:opacity-100"
                >
                  <XIcon class="h-4 w-4" />
                </button>
              </div>
            </div>
          </div>

          <div>
            <p class="mb-2 text-xs font-semibold uppercase tracking-[0.16em] text-slate-400">
              Anh se upload
            </p>
            <div v-if="previewImages.length" class="grid grid-cols-2 gap-3 sm:grid-cols-3">
              <div
                v-for="(image, index) in previewImages"
                :key="image"
                class="group relative overflow-hidden rounded-2xl border border-[var(--ui-border)] bg-slate-100"
              >
                <img :src="image" class="aspect-square h-full w-full object-cover" />
                <button
                  type="button"
                  @click="removeSelectedImage(index)"
                  class="absolute right-2 top-2 inline-flex h-8 w-8 items-center justify-center rounded-full bg-white/90 text-rose-500 opacity-0 shadow transition group-hover:opacity-100"
                >
                  <XIcon class="h-4 w-4" />
                </button>
              </div>
            </div>
            <div
              v-else
              class="rounded-2xl border border-dashed border-[var(--ui-border)] bg-slate-50 px-4 py-8 text-center text-sm text-slate-500"
            >
              Chua co anh moi nao duoc chon.
            </div>
          </div>
        </article>

        <article class="rounded-3xl border border-[var(--ui-border)] bg-white p-5 shadow-[var(--shadow-soft)]">
          <div class="space-y-3">
            <div class="flex items-center justify-between text-sm text-slate-500">
              <span>Gia goc</span>
              <span class="font-semibold text-slate-900">{{ formatMoney(form.giaGoc) }}</span>
            </div>
            <div class="flex items-center justify-between text-sm text-slate-500">
              <span>Bien the cu</span>
              <span class="font-semibold text-slate-900">{{ form.existingVariants.length }}</span>
            </div>
            <div class="flex items-center justify-between text-sm text-slate-500">
              <span>Bien the moi</span>
              <span class="font-semibold text-slate-900">{{ form.newVariants.length }}</span>
            </div>
            <div class="flex items-center justify-between text-sm text-slate-500">
              <span>Anh moi</span>
              <span class="font-semibold text-slate-900">{{ selectedFiles.length }}</span>
            </div>
            <div v-if="removedImageIds.length" class="flex items-center justify-between text-sm text-rose-600">
              <span>Anh se xoa</span>
              <span class="font-semibold">{{ removedImageIds.length }}</span>
            </div>
          </div>

          <div class="mt-5 space-y-2">
            <button
              type="button"
              :disabled="saving"
              @click="handleSubmit"
              class="inline-flex w-full items-center justify-center gap-2 rounded-2xl bg-slate-950 px-4 py-3 text-sm font-semibold text-white transition hover:bg-slate-800 disabled:cursor-not-allowed disabled:opacity-60"
            >
              <Loader2Icon v-if="saving" class="h-4 w-4 animate-spin" />
              <SaveIcon v-else class="h-4 w-4" />
              {{ saving ? "Đang lưu..." : isEdit ? "Cập nhật sản phẩm" : "Tạo sản phẩm" }}
            </button>

            <button
              type="button"
              @click="router.push('/admin/products')"
              class="inline-flex w-full items-center justify-center rounded-2xl border border-[var(--ui-border)] bg-white px-4 py-3 text-sm font-semibold text-slate-700 transition hover:bg-slate-50"
            >
              Quay lai danh sach
            </button>
          </div>
        </article>
      </aside>
    </div>
  </div>
</template>

<script setup lang="ts">
import { computed, onBeforeUnmount, onMounted, ref } from "vue";
import { useRoute, useRouter } from "vue-router";
import {
  ArrowLeft as ArrowLeftIcon,
  Loader2 as Loader2Icon,
  Plus as PlusIcon,
  Save as SaveIcon,
  Trash2 as Trash2Icon,
  UploadCloud as UploadCloudIcon,
  X as XIcon,
} from "lucide-vue-next";
import Toast from "../../components/admin/Toast.vue";
import { useToast } from "../../composables/useToast";
import { anhService } from "../../services/anh.service";
import { danhMucService } from "../../services/danhmuc.service";
import { mauSizeService } from "../../services/mausize.service";
import { extractErrorMessage, hasFieldErrors } from "../../services/service-helpers";
import { sanPhamService } from "../../services/sanpham.service";
import type {
  CategoryItem,
  ColorItem,
  ExistingProductVariantDraft,
  FieldErrorMap,
  ImageItem,
  ProductDetail,
  ProductFormValues,
  ProductVariantDraft,
  SizeItem,
} from "../../types";
import { validateProductForm } from "../../utils/validators";

const route = useRoute();
const router = useRouter();
const { error, success, warning } = useToast();

const isEdit = computed(() => Boolean(route.params.id));
const loadingData = ref(false);
const saving = ref(false);
const fileInput = ref<HTMLInputElement | null>(null);

const categories = ref<CategoryItem[]>([]);
const sizes = ref<SizeItem[]>([]);
const colors = ref<ColorItem[]>([]);
const existingImages = ref<ImageItem[]>([]);
const removedImageIds = ref<number[]>([]);
const selectedFiles = ref<File[]>([]);
const previewImages = ref<string[]>([]);
const errors = ref<FieldErrorMap>({});

const form = ref<ProductFormValues>({
  maSp: "",
  tenSp: "",
  maDanhMuc: null,
  giaGoc: 0,
  chatLieu: "",
  moTa: "",
  newVariants: [],
  existingVariants: [],
});

const parentById = computed(() => {
  const map = new Map<number, CategoryItem>();
  for (const category of categories.value) {
    map.set(category.maDanhMuc, category);
  }
  return map;
});

const inputClass = (field?: string) =>
  [
    "w-full rounded-2xl border bg-white px-4 py-3 text-sm text-slate-900 transition outline-none",
    field && fieldError(field)
      ? "border-rose-300 focus:border-rose-400 focus:ring-4 focus:ring-rose-100"
      : "border-[var(--ui-border)] focus:border-[var(--accent)] focus:ring-4 focus:ring-[var(--accent-soft)]",
  ].join(" ");

const compactInputClass = (field?: string) =>
  [
    "w-full rounded-2xl border bg-white px-3 py-2.5 text-sm text-slate-900 transition outline-none",
    field && fieldError(field)
      ? "border-rose-300 focus:border-rose-400 focus:ring-4 focus:ring-rose-100"
      : "border-[var(--ui-border)] focus:border-[var(--accent)] focus:ring-4 focus:ring-[var(--accent-soft)]",
  ].join(" ");

const textareaClass = () =>
  "w-full rounded-2xl border border-[var(--ui-border)] bg-white px-4 py-3 text-sm text-slate-900 transition outline-none focus:border-[var(--accent)] focus:ring-4 focus:ring-[var(--accent-soft)]";

function fieldError(key: string) {
  return errors.value[key];
}

function formatMoney(value: number) {
  return `${(value || 0).toLocaleString("vi-VN")} đ`;
}

function categoryLabel(category: CategoryItem) {
  const parts = [category.tenDanhMuc];
  let currentParentId = category.maDanhMucCha;

  while (currentParentId) {
    const parent = parentById.value.get(currentParentId);
    if (!parent) break;
    parts.unshift(parent.tenDanhMuc);
    currentParentId = parent.maDanhMucCha ?? null;
  }

  return parts.join(" / ");
}

function variantKey(variant: Pick<ProductVariantDraft, "maKichThuoc" | "maMauSac">) {
  return `${variant.maKichThuoc}-${variant.maMauSac}`;
}

function existingVariantKeys() {
  return form.value.existingVariants.map((variant) =>
    variantKey({
      maKichThuoc: variant.maKichThuoc,
      maMauSac: variant.maMauSac,
    }),
  );
}

function nextAvailableVariant(): ProductVariantDraft | null {
  const taken = new Set([
    ...existingVariantKeys(),
    ...form.value.newVariants.map((variant) => variantKey(variant)),
  ]);

  for (const size of sizes.value) {
    for (const color of colors.value) {
      const key = `${size.maSize}-${color.maMau}`;
      if (!taken.has(key)) {
        return {
          maKichThuoc: size.maSize,
          maMauSac: color.maMau,
          soLuongTon: 0,
          giaBan: form.value.giaGoc || 0,
        };
      }
    }
  }

  return null;
}

function addVariant() {
  if (!sizes.value.length || !colors.value.length) {
    warning("Can co size va mau truoc khi them bien the.");
    return;
  }

  const variant = nextAvailableVariant();
  if (!variant) {
    warning("Tat ca to hop size/mau da ton tai.");
    return;
  }

  form.value.newVariants.push(variant);
}

function removeVariant(index: number) {
  form.value.newVariants.splice(index, 1);
}

function handleFileChange(event: Event) {
  const files = (event.target as HTMLInputElement).files;
  if (!files) return;

  for (const file of Array.from(files)) {
    selectedFiles.value.push(file);
    previewImages.value.push(URL.createObjectURL(file));
  }

  (event.target as HTMLInputElement).value = "";
}

function removeSelectedImage(index: number) {
  const preview = previewImages.value[index];
  if (preview) {
    URL.revokeObjectURL(preview);
  }
  previewImages.value.splice(index, 1);
  selectedFiles.value.splice(index, 1);
}

function queueDeleteImage(maAnh: number) {
  removedImageIds.value.push(maAnh);
  existingImages.value = existingImages.value.filter((image) => image.maAnh !== maAnh);
}

async function loadOptions() {
  const [categoryResponse, sizeResponse, colorResponse] = await Promise.all([
    danhMucService.getAll(),
    mauSizeService.getAllSize(),
    mauSizeService.getAllMau(),
  ]);

  categories.value = categoryResponse.data ?? [];
  sizes.value = sizeResponse.data ?? [];
  colors.value = colorResponse.data ?? [];
}

function mapExistingVariants(detail: ProductDetail): ExistingProductVariantDraft[] {
  return (detail.dsBienThe || []).map((variant) => ({
    maChiTietSP: variant.maChiTietSP,
    maKichThuoc: variant.maKichThuoc,
    maMauSac: variant.maMauSac,
    tenSize: variant.tenSize,
    tenMau: variant.tenMau,
    soLuongTon: variant.soLuongTon ?? 0,
    giaBan: variant.giaBan ?? detail.giaGoc,
  }));
}

async function loadProduct() {
  if (!isEdit.value) return;

  const productId = route.params.id as string;
  const [detailResponse, imageResponse] = await Promise.all([
    sanPhamService.getFullInfo(productId),
    anhService.getByProduct(productId),
  ]);

  const detail = detailResponse.data;
  form.value = {
    maSp: detail.maSp,
    tenSp: detail.tenSp,
    maDanhMuc: detail.maDanhMuc ?? null,
    giaGoc: detail.giaGoc,
    chatLieu: detail.chatLieu || "",
    moTa: detail.moTa || "",
    newVariants: [],
    existingVariants: mapExistingVariants(detail),
  };

  existingImages.value = imageResponse.data ?? [];
  removedImageIds.value = [];
}

function buildBaseFormData() {
  const formData = new FormData();
  formData.append("MaSp", form.value.maSp.trim());
  formData.append("TenSp", form.value.tenSp.trim());
  formData.append("MaDanhMuc", String(form.value.maDanhMuc));
  formData.append("GiaGoc", String(form.value.giaGoc));
  formData.append("DsBienThe", "[]");

  if (form.value.chatLieu.trim()) formData.append("ChatLieu", form.value.chatLieu.trim());
  if (form.value.moTa.trim()) formData.append("MoTa", form.value.moTa.trim());

  return formData;
}

async function createProduct() {
  const formData = buildBaseFormData();
  formData.set("DsBienThe", JSON.stringify(form.value.newVariants));

  for (const file of selectedFiles.value) {
    formData.append("AnhSps", file);
  }

  await sanPhamService.create(formData);
}

async function updateBaseProduct() {
  const formData = buildBaseFormData();
  await sanPhamService.update(formData);
}

async function updateExistingVariants() {
  for (const variant of form.value.existingVariants) {
    await sanPhamService.updateVariant({
      maChiTietSp: variant.maChiTietSP,
      soLuongTon: variant.soLuongTon,
      giaBan: variant.giaBan,
    });
  }
}

async function createNewVariants() {
  for (const variant of form.value.newVariants) {
    await sanPhamService.createVariant({
      maSP: form.value.maSp.trim(),
      maKichThuoc: variant.maKichThuoc,
      maMauSac: variant.maMauSac,
      soLuongTon: variant.soLuongTon,
      giaBan: variant.giaBan,
    });
  }
}

async function syncImages() {
  if (removedImageIds.value.length) {
    for (const maAnh of removedImageIds.value) {
      await anhService.deleteImage(maAnh);
    }
  }

  if (selectedFiles.value.length) {
    await anhService.addImages(form.value.maSp.trim(), selectedFiles.value);
  }
}

function validateBeforeSubmit() {
  errors.value = validateProductForm(form.value, {
    requireInitialVariants: !isEdit.value,
    existingVariantKeys: existingVariantKeys(),
  });

  return !hasFieldErrors(errors.value);
}

async function handleSubmit() {
  if (!validateBeforeSubmit()) {
    error("Vui long kiem tra lai du lieu truoc khi luu.");
    return;
  }

  saving.value = true;
  try {
    if (isEdit.value) {
      await updateBaseProduct();
      await updateExistingVariants();
      await createNewVariants();
      await syncImages();
      success("Cập nhật sản phẩm thành công.");
    } else {
      await createProduct();
      success("Tạo sản phẩm thành công.");
    }

    router.push("/admin/products");
  } catch (caughtError) {
    error(extractErrorMessage(caughtError, "Không thể lưu sản phẩm."));
  } finally {
    saving.value = false;
  }
}

onMounted(async () => {
  loadingData.value = true;
  try {
    await loadOptions();
    await loadProduct();
  } catch (caughtError) {
    error(extractErrorMessage(caughtError, "Không thể tải dữ liệu sản phẩm."));
  } finally {
    loadingData.value = false;
  }
});

onBeforeUnmount(() => {
  for (const preview of previewImages.value) {
    URL.revokeObjectURL(preview);
  }
});
</script>
