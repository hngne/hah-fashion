<template>
  <div class="space-y-4">
    <section class="rounded-3xl border border-[var(--ui-border)] bg-white p-4 shadow-[var(--shadow-soft)]">
      <h3 class="mb-3 text-sm font-semibold uppercase tracking-[0.16em] text-slate-500">Danh mục</h3>
      <div class="space-y-2">
        <button
          type="button"
          :class="buttonClass(!activeCategoryId)"
          @click="$emit('select-category', null)"
        >
          Tất cả sản phẩm
        </button>
        <div v-for="category in categories" :key="category.maDanhMuc" class="space-y-2">
          <button
            type="button"
            :class="buttonClass(activeCategoryId === category.maDanhMuc)"
            @click="$emit('select-category', category.maDanhMuc)"
          >
            {{ category.tenDanhMuc }}
          </button>
          <div v-if="getChildren(category.maDanhMuc).length" class="space-y-2 pl-4">
            <button
              v-for="child in getChildren(category.maDanhMuc)"
              :key="child.maDanhMuc"
              type="button"
              :class="buttonClass(activeCategoryId === child.maDanhMuc)"
              @click="$emit('select-category', child.maDanhMuc)"
            >
              {{ child.tenDanhMuc }}
            </button>
          </div>
        </div>
      </div>
    </section>

    <section class="rounded-3xl border border-[var(--ui-border)] bg-white p-4 shadow-[var(--shadow-soft)]">
      <h3 class="mb-3 text-sm font-semibold uppercase tracking-[0.16em] text-slate-500">Khoảng giá</h3>
      <div class="space-y-2">
        <button
          v-for="range in priceRanges"
          :key="range.key || 'all'"
          type="button"
          :class="buttonClass(activePriceKey === range.key)"
          @click="$emit('toggle-price', range.key)"
        >
          {{ range.label }}
        </button>
      </div>
    </section>
  </div>
</template>

<script setup lang="ts">
import type { CategoryItem } from "../../types";

defineProps<{
  categories: CategoryItem[];
  activeCategoryId: number | null;
  activePriceKey: string;
  getChildren: (parentId: number) => CategoryItem[];
}>();

defineEmits<{
  (event: "select-category", value: number | null): void;
  (event: "toggle-price", value: string): void;
}>();

const priceRanges = [
  { key: "", label: "Tat ca muc gia" },
  { key: "under-200", label: "Duoi 200.000đ" },
  { key: "200-500", label: "200.000đ - 500.000đ" },
  { key: "500-1000", label: "500.000đ - 1.000.000đ" },
  { key: "over-1000", label: "Tren 1.000.000đ" },
];

function buttonClass(active: boolean) {
  return [
    "w-full rounded-2xl px-4 py-3 text-left text-sm font-medium transition",
    active ? "bg-slate-950 text-white" : "bg-slate-50 text-slate-600 hover:bg-slate-100",
  ].join(" ");
}
</script>
