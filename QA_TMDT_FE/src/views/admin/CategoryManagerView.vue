<template>
  <div>
    <Toast />

    <div class="mb-6 flex flex-col gap-4 lg:flex-row lg:items-end lg:justify-between">
      <div>
        <h3 class="text-xl font-bold text-slate-900">Quản lý danh mục</h3>
        <p class="mt-1 text-sm text-slate-500">
          Kiểm tra quan hệ cha-con, chặn vòng lặp và tối ưu hiển thị mobile.
        </p>
      </div>

      <button
        type="button"
        @click="openModal()"
        class="inline-flex items-center justify-center gap-2 rounded-full bg-slate-950 px-5 py-3 text-sm font-semibold text-white transition hover:bg-slate-800"
      >
        <PlusIcon class="h-4 w-4" />
        Thêm danh mục
      </button>
    </div>

    <section class="rounded-3xl border border-[var(--ui-border)] bg-white p-4 shadow-[var(--shadow-soft)]">
      <div class="relative">
        <SearchIcon class="pointer-events-none absolute left-4 top-1/2 h-4 w-4 -translate-y-1/2 text-slate-400" />
        <input
          v-model="searchQuery"
          type="text"
          placeholder="Tìm theo tên hoặc mã danh mục..."
          class="w-full rounded-2xl border border-[var(--ui-border)] bg-white py-3 pl-11 pr-4 text-sm text-slate-900 outline-none transition focus:border-[var(--accent)] focus:ring-4 focus:ring-[var(--accent-soft)]"
        />
      </div>
    </section>

    <div v-if="loading" class="flex justify-center py-16 text-slate-400">
      <Loader2Icon class="h-8 w-8 animate-spin" />
    </div>

    <section
      v-else-if="filteredCategories.length"
      class="mt-6 overflow-hidden rounded-3xl border border-[var(--ui-border)] bg-white shadow-[var(--shadow-soft)]"
    >
      <div class="hidden overflow-x-auto lg:block">
        <table class="min-w-full">
          <thead class="bg-slate-50 text-left text-xs font-semibold uppercase tracking-[0.16em] text-slate-400">
            <tr>
              <th class="px-5 py-4">Mã</th>
              <th class="px-5 py-4">Tên danh mục</th>
              <th class="px-5 py-4">Mô tả</th>
              <th class="px-5 py-4">Danh mục cha</th>
              <th class="px-5 py-4 text-right">Thao tác</th>
            </tr>
          </thead>
          <tbody class="divide-y divide-slate-100">
            <tr v-for="row in filteredCategories" :key="row.item.maDanhMuc" class="transition hover:bg-slate-50/70">
              <td class="px-5 py-4 text-sm font-medium text-slate-400">#{{ row.item.maDanhMuc }}</td>
              <td class="px-5 py-4">
                <p class="text-sm font-semibold text-slate-900">
                  <span class="mr-2 text-slate-300">{{ "—".repeat(row.level) }}</span>
                  {{ row.item.tenDanhMuc }}
                </p>
              </td>
              <td class="px-5 py-4 text-sm text-slate-600">{{ row.item.moTa || "-" }}</td>
              <td class="px-5 py-4 text-sm text-slate-600">{{ parentName(row.item.maDanhMucCha) }}</td>
              <td class="px-5 py-4">
                <div class="flex justify-end gap-2">
                  <button
                    type="button"
                    class="inline-flex h-10 w-10 items-center justify-center rounded-2xl border border-[var(--ui-border)] bg-white text-slate-500 transition hover:border-[var(--accent)] hover:text-[var(--accent)]"
                    @click="openModal(row.item)"
                  >
                    <PencilIcon class="h-4 w-4" />
                  </button>
                  <button
                    type="button"
                    class="inline-flex h-10 w-10 items-center justify-center rounded-2xl border border-rose-200 bg-white text-rose-500 transition hover:bg-rose-50"
                    @click="deleteTarget = row.item"
                  >
                    <Trash2Icon class="h-4 w-4" />
                  </button>
                </div>
              </td>
            </tr>
          </tbody>
        </table>
      </div>

      <div class="divide-y divide-slate-100 lg:hidden">
        <div v-for="row in filteredCategories" :key="row.item.maDanhMuc" class="space-y-3 p-4">
          <div class="flex items-start justify-between gap-3">
            <div class="min-w-0">
              <p class="text-xs font-semibold uppercase tracking-[0.16em] text-slate-400">
                #{{ row.item.maDanhMuc }}
              </p>
              <p class="mt-1 text-sm font-semibold text-slate-900">
                <span class="mr-2 text-slate-300">{{ "—".repeat(row.level) }}</span>
                {{ row.item.tenDanhMuc }}
              </p>
              <p class="mt-2 text-sm text-slate-500">{{ row.item.moTa || "Không có mô tả." }}</p>
            </div>
            <div class="flex gap-2">
              <button
                type="button"
                class="inline-flex h-10 w-10 items-center justify-center rounded-2xl border border-[var(--ui-border)] bg-white text-slate-500 transition hover:border-[var(--accent)] hover:text-[var(--accent)]"
                @click="openModal(row.item)"
              >
                <PencilIcon class="h-4 w-4" />
              </button>
              <button
                type="button"
                class="inline-flex h-10 w-10 items-center justify-center rounded-2xl border border-rose-200 bg-white text-rose-500 transition hover:bg-rose-50"
                @click="deleteTarget = row.item"
              >
                <Trash2Icon class="h-4 w-4" />
              </button>
            </div>
          </div>

          <div class="rounded-2xl bg-slate-50 px-3 py-2 text-sm text-slate-600">
            Danh mục cha: {{ parentName(row.item.maDanhMucCha) }}
          </div>
        </div>
      </div>
    </section>

    <div
      v-else
      class="mt-6 rounded-3xl border border-dashed border-[var(--ui-border)] bg-white px-4 py-14 text-center text-sm text-slate-500"
    >
      Không tìm thấy danh mục phù hợp.
    </div>

    <teleport to="body">
      <transition name="modal-fade">
        <div
          v-if="showModal"
          class="fixed inset-0 z-50 flex items-center justify-center bg-slate-950/55 px-4 py-8"
          @click.self="showModal = false"
        >
          <div class="w-full max-w-lg rounded-3xl bg-white p-6 shadow-2xl">
            <div class="mb-5">
              <h4 class="text-lg font-semibold text-slate-900">
                {{ editingId ? "Cập nhật danh mục" : "Thêm danh mục" }}
              </h4>
              <p class="mt-1 text-sm text-slate-500">
                He thong se chan chon danh muc con lam danh muc cha.
              </p>
            </div>

            <div class="space-y-4">
              <div>
                <label class="mb-1.5 block text-sm font-semibold text-slate-700">Ten danh muc</label>
                <input
                  v-model="form.tenDanhMuc"
                  type="text"
                  placeholder="VD: Ao, Quan, Giay..."
                  :class="inputClass('tenDanhMuc')"
                />
                <p v-if="fieldError('tenDanhMuc')" class="mt-1 text-xs font-medium text-rose-600">
                  {{ fieldError("tenDanhMuc") }}
                </p>
              </div>

              <div>
                <label class="mb-1.5 block text-sm font-semibold text-slate-700">Mô tả</label>
                <textarea
                  v-model="form.moTa"
                  rows="3"
                  placeholder="Mô tả ngắn cho danh mục"
                  class="w-full rounded-2xl border border-[var(--ui-border)] bg-white px-4 py-3 text-sm text-slate-900 outline-none transition focus:border-[var(--accent)] focus:ring-4 focus:ring-[var(--accent-soft)]"
                />
              </div>

              <div>
                <label class="mb-1.5 block text-sm font-semibold text-slate-700">Danh mục cha</label>
                <select v-model="form.maDanhMucCha" :class="inputClass('maDanhMucCha')">
                  <option :value="null">Không có danh mục cha</option>
                  <option
                    v-for="option in parentOptions"
                    :key="option.maDanhMuc"
                    :value="option.maDanhMuc"
                  >
                    {{ categoryPath(option) }}
                  </option>
                </select>
                <p v-if="fieldError('maDanhMucCha')" class="mt-1 text-xs font-medium text-rose-600">
                  {{ fieldError("maDanhMucCha") }}
                </p>
              </div>
            </div>

            <div class="mt-6 flex gap-3">
              <button
                type="button"
                class="inline-flex flex-1 items-center justify-center rounded-2xl border border-[var(--ui-border)] bg-white px-4 py-3 text-sm font-semibold text-slate-700 transition hover:bg-slate-50"
                @click="showModal = false"
              >
                Huy
              </button>
              <button
                type="button"
                :disabled="saving"
                class="inline-flex flex-1 items-center justify-center gap-2 rounded-2xl bg-slate-950 px-4 py-3 text-sm font-semibold text-white transition hover:bg-slate-800 disabled:cursor-not-allowed disabled:opacity-60"
                @click="handleSave"
              >
                <Loader2Icon v-if="saving" class="h-4 w-4 animate-spin" />
                {{ editingId ? "Cập nhật" : "Thêm mới" }}
              </button>
            </div>
          </div>
        </div>
      </transition>
    </teleport>

    <teleport to="body">
      <transition name="modal-fade">
        <div
          v-if="deleteTarget"
          class="fixed inset-0 z-50 flex items-center justify-center bg-slate-950/55 px-4"
          @click.self="deleteTarget = null"
        >
          <div class="w-full max-w-sm rounded-3xl bg-white p-6 shadow-2xl">
            <h4 class="text-lg font-semibold text-slate-900">Xoa danh muc</h4>
            <p class="mt-2 text-sm text-slate-500">
              Ban chac chan muon xoa <strong class="text-slate-900">{{ deleteTarget.tenDanhMuc }}</strong>?
            </p>
            <div class="mt-6 flex gap-3">
              <button
                type="button"
                class="inline-flex flex-1 items-center justify-center rounded-2xl border border-[var(--ui-border)] bg-white px-4 py-3 text-sm font-semibold text-slate-700 transition hover:bg-slate-50"
                @click="deleteTarget = null"
              >
                Huy
              </button>
              <button
                type="button"
                :disabled="saving"
                class="inline-flex flex-1 items-center justify-center gap-2 rounded-2xl bg-rose-600 px-4 py-3 text-sm font-semibold text-white transition hover:bg-rose-500 disabled:cursor-not-allowed disabled:opacity-60"
                @click="handleDelete"
              >
                <Loader2Icon v-if="saving" class="h-4 w-4 animate-spin" />
                Xoa
              </button>
            </div>
          </div>
        </div>
      </transition>
    </teleport>
  </div>
</template>

<script setup lang="ts">
import { computed, onMounted, ref } from "vue";
import {
  Loader2 as Loader2Icon,
  Pencil as PencilIcon,
  Plus as PlusIcon,
  Search as SearchIcon,
  Trash2 as Trash2Icon,
} from "lucide-vue-next";
import Toast from "../../components/admin/Toast.vue";
import { useToast } from "../../composables/useToast";
import { danhMucService } from "../../services/danhmuc.service";
import { extractErrorMessage, hasFieldErrors } from "../../services/service-helpers";
import type { CategoryFormValues, CategoryItem, FieldErrorMap } from "../../types";
import { getCategoryDescendantIds, validateCategoryForm } from "../../utils/validators";

type FlattenedCategory = { item: CategoryItem; level: number };

const { error, success } = useToast();

const categories = ref<CategoryItem[]>([]);
const searchQuery = ref("");
const loading = ref(true);
const saving = ref(false);
const showModal = ref(false);
const editingId = ref<number | null>(null);
const deleteTarget = ref<CategoryItem | null>(null);
const errors = ref<FieldErrorMap>({});

const form = ref<CategoryFormValues>({
  tenDanhMuc: "",
  moTa: "",
  maDanhMucCha: null,
});

const categoryMap = computed(() => {
  const map = new Map<number, CategoryItem>();
  for (const category of categories.value) {
    map.set(category.maDanhMuc, category);
  }
  return map;
});

const flattenedCategories = computed<FlattenedCategory[]>(() => {
  const children = new Map<number | null, CategoryItem[]>();

  for (const category of categories.value) {
    const key = category.maDanhMucCha ?? null;
    const bucket = children.get(key) || [];
    bucket.push(category);
    children.set(key, bucket);
  }

  const result: FlattenedCategory[] = [];
  const walk = (parentId: number | null, level: number) => {
    const branch = children.get(parentId) || [];
    branch.sort((a, b) => a.tenDanhMuc.localeCompare(b.tenDanhMuc));
    for (const category of branch) {
      result.push({ item: category, level });
      walk(category.maDanhMuc, level + 1);
    }
  };

  walk(null, 0);
  return result;
});

const filteredCategories = computed(() => {
  const keyword = searchQuery.value.trim().toLowerCase();
  if (!keyword) return flattenedCategories.value;

  return flattenedCategories.value.filter(({ item }) => {
    return (
      item.tenDanhMuc.toLowerCase().includes(keyword) ||
      String(item.maDanhMuc).includes(keyword)
    );
  });
});

const parentOptions = computed(() => {
  if (!editingId.value) return categories.value;
  const blocked = new Set([
    editingId.value,
    ...getCategoryDescendantIds(categories.value, editingId.value),
  ]);

  return categories.value.filter((category) => !blocked.has(category.maDanhMuc));
});

function inputClass(field?: string) {
  return [
    "w-full rounded-2xl border bg-white px-4 py-3 text-sm text-slate-900 outline-none transition",
    field && fieldError(field)
      ? "border-rose-300 focus:border-rose-400 focus:ring-4 focus:ring-rose-100"
      : "border-[var(--ui-border)] focus:border-[var(--accent)] focus:ring-4 focus:ring-[var(--accent-soft)]",
  ].join(" ");
}

function fieldError(key: string) {
  return errors.value[key];
}

function parentName(parentId?: number | null) {
  if (!parentId) return "Không có";
  return categoryMap.value.get(parentId)?.tenDanhMuc || `#${parentId}`;
}

function categoryPath(category: CategoryItem) {
  const parts = [category.tenDanhMuc];
  let currentParentId = category.maDanhMucCha;

  while (currentParentId) {
    const parent = categoryMap.value.get(currentParentId);
    if (!parent) break;
    parts.unshift(parent.tenDanhMuc);
    currentParentId = parent.maDanhMucCha ?? null;
  }

  return parts.join(" / ");
}

async function loadCategories() {
  loading.value = true;
  try {
    const response = await danhMucService.getAll();
    categories.value = response.data ?? [];
  } catch (caughtError) {
    error(extractErrorMessage(caughtError, "Không thể tải danh mục."));
  } finally {
    loading.value = false;
  }
}

function openModal(category?: CategoryItem) {
  errors.value = {};
  if (category) {
    editingId.value = category.maDanhMuc;
    form.value = {
      tenDanhMuc: category.tenDanhMuc,
      moTa: category.moTa || "",
      maDanhMucCha: category.maDanhMucCha ?? null,
    };
  } else {
    editingId.value = null;
    form.value = { tenDanhMuc: "", moTa: "", maDanhMucCha: null };
  }
  showModal.value = true;
}

async function handleSave() {
  errors.value = validateCategoryForm(form.value, categories.value, editingId.value);
  if (hasFieldErrors(errors.value)) return;

  saving.value = true;
  try {
    if (editingId.value) {
      await danhMucService.update(editingId.value, form.value);
      success("Cập nhật danh mục thành công.");
    } else {
      await danhMucService.create(form.value);
      success("Thêm danh mục thành công.");
    }

    showModal.value = false;
    await loadCategories();
  } catch (caughtError) {
    error(extractErrorMessage(caughtError, "Không thể lưu danh mục."));
  } finally {
    saving.value = false;
  }
}

async function handleDelete() {
  if (!deleteTarget.value) return;

  saving.value = true;
  try {
    await danhMucService.delete(deleteTarget.value.maDanhMuc);
    success("Da xoa danh muc.");
    deleteTarget.value = null;
    await loadCategories();
  } catch (caughtError) {
    error(extractErrorMessage(caughtError, "Không thể xóa danh mục."));
  } finally {
    saving.value = false;
  }
}

onMounted(() => {
  void loadCategories();
});
</script>

<style scoped>
.modal-fade-enter-active,
.modal-fade-leave-active {
  transition: opacity 0.2s ease;
}

.modal-fade-enter-from,
.modal-fade-leave-to {
  opacity: 0;
}
</style>
