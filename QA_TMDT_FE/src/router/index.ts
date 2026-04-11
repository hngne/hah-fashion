import { createRouter, createWebHistory } from "vue-router";
import { useAuthStore } from "../stores/auth";

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  scrollBehavior(_to, _from, savedPosition) {
    if (savedPosition) return savedPosition;
    return { top: 0 };
  },
  routes: [
    // ===== ADMIN =====
    {
      path: "/admin/login",
      name: "admin-login",
      component: () => import("../views/admin/Login.vue"),
    },
    {
      path: "/admin",
      name: "admin-layout",
      component: () => import("../views/admin/AdminLayout.vue"),
      meta: { requiresAdmin: true },
      children: [
        {
          path: "",
          name: "admin-dashboard",
          component: () => import("../views/admin/Dashboard.vue"),
        },
        {
          path: "products",
          name: "admin-products",
          component: () => import("../views/admin/ProductListView.vue"),
        },
        {
          path: "products/new",
          name: "admin-product-new",
          component: () => import("../views/admin/ProductFormView.vue"),
        },
        {
          path: "products/:id/edit",
          name: "admin-product-edit",
          component: () => import("../views/admin/ProductFormView.vue"),
          props: true,
        },
        {
          path: "variant-attributes",
          name: "admin-variant-attributes",
          component: () => import("../views/admin/VariantAttributes.vue"),
        },
        {
          path: "categories",
          name: "admin-categories",
          component: () => import("../views/admin/CategoryManagerView.vue"),
        },
        {
          path: "orders",
          name: "admin-orders",
          component: () => import("../views/admin/OrderManager.vue"),
        },
        {
          path: "users",
          name: "admin-users",
          component: () => import("../views/admin/UserManager.vue"),
        },
        {
          path: "vouchers",
          name: "admin-vouchers",
          component: () => import("../views/admin/VoucherManager.vue"),
        },
        {
          path: "promotions",
          name: "admin-promotions",
          component: () => import("../views/admin/PromotionManager.vue"),
        },
        {
          path: "settings",
          name: "admin-settings",
          component: () => import("../views/admin/Settings.vue"),
        },
      ],
    },

    // ===== CLIENT =====
    {
      path: "/",
      name: "client-layout",
      component: () => import("../views/client/ClientLayout.vue"),
      children: [
        {
          path: "",
          name: "home",
          component: () => import("../views/client/Home.vue"),
        },
        {
          path: "product/:maSP",
          name: "product-detail",
          component: () => import("../views/client/ProductDetailView.vue"),
          props: true,
        },
        {
          path: "search",
          name: "product-search",
          component: () => import("../views/client/ProductSearchView.vue"),
        },
        {
          path: "category/:maDM",
          name: "category-products",
          component: () => import("../views/client/ProductSearchView.vue"),
          props: true,
        },
        {
          path: "sale",
          redirect: {
            name: "product-search",
            query: { sort: "price-desc" },
          },
        },
        {
          path: "login",
          name: "client-login",
          component: () => import("../views/client/ClientAuthView.vue"),
        },
        {
          path: "cart",
          name: "cart",
          component: () => import("../views/client/Cart.vue"),
        },
        {
          path: "checkout",
          name: "checkout",
          component: () => import("../views/client/CheckoutView.vue"),
        },
        {
          path: "order-success/:maDH",
          name: "order-success",
          component: () => import("../views/client/OrderSuccess.vue"),
          props: true,
        },
        {
          path: "orders",
          name: "order-history",
          component: () => import("../views/client/OrderHistoryView.vue"),
        },
        {
          path: ":pathMatch(.*)*",
          name: "client-not-found",
          component: () => import("../views/client/NotFoundView.vue"),
        },
      ],
    },
  ],
});

// ========== ROUTE GUARD ==========
router.beforeEach((to, _from, next) => {
  if (to.matched.some((record) => record.meta.requiresAdmin)) {
    const authStore = useAuthStore();
    if (!authStore.isAuthenticated() || !authStore.isAdmin()) {
      next({ name: "admin-login" });
    } else {
      next();
    }
  } else {
    next();
  }
});

export default router;
