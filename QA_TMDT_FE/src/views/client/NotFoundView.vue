<template>
  <div class="min-h-[70vh] bg-[var(--bg-page)] px-4 py-20">
    <div class="mx-auto max-w-2xl rounded-[36px] border border-[var(--ui-border)] bg-white px-6 py-14 text-center shadow-[var(--shadow-soft)] sm:px-10">
      <p class="text-xs font-semibold uppercase tracking-[0.2em] text-slate-400">404</p>
      <h1 class="mt-3 text-3xl font-black tracking-tight text-slate-950 sm:text-4xl">
        Trang bạn tìm không tồn tại
      </h1>
      <p class="mt-3 text-sm leading-7 text-slate-500">
        Có thể đường dẫn đã thay đổi, bị xóa hoặc bạn đã truy cập sai liên kết.
      </p>
      <div class="mt-8 flex flex-col justify-center gap-3 sm:flex-row">
        <RouterLink
          to="/"
          class="inline-flex items-center justify-center rounded-full bg-slate-950 px-5 py-3 text-sm font-semibold text-white transition hover:bg-slate-800"
        >
          Về trang chủ
        </RouterLink>
        <RouterLink
          to="/search"
          class="inline-flex items-center justify-center rounded-full border border-[var(--ui-border)] bg-white px-5 py-3 text-sm font-semibold text-slate-700 transition hover:bg-slate-50"
        >
          Xem danh sách sản phẩm
        </RouterLink>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { RouterLink } from "vue-router";
</script>
