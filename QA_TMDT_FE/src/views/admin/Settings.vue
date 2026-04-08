<template>
  <div class="flex items-center justify-center py-20">
    <div class="text-center">
      <SettingsIcon
        class="h-16 w-16 text-gray-300 mx-auto mb-4"
        stroke-width="1.5"
      />
      <h3 class="text-xl font-bold text-gray-700">Cài đặt hệ thống</h3>
      <p class="text-gray-500 mt-2 max-w-sm mx-auto">
        Đây là trang dành cho các cài đặt cấu hình chung của website (ví dụ:
        thông tin liên hệ, logo, cấu hình phí ship mặc định, SEO meta tags...).
        <br /><br />
        Trang này hiện đang được giữ chỗ (placeholder) và sẽ được triển khai khi
        có API tương ứng từ backend.
      </p>
    </div>
  </div>
</template>

<script setup lang="ts">
import { Settings as SettingsIcon } from "lucide-vue-next";
</script>
