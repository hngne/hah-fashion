<template>
  <div>
    <HeroCarousel />
    <FeaturedCategories />
    <NewArrivals />
    <PromoBanners />
    <BestSellers />
    <Lookbook />
    <TrustNewsletter />
  </div>
</template>

<script setup lang="ts">
import HeroCarousel from "../../components/client/HeroCarousel.vue";
import FeaturedCategories from "../../components/client/FeaturedCategories.vue";
import NewArrivals from "../../components/client/NewArrivals.vue";
import PromoBanners from "../../components/client/PromoBanners.vue";
import BestSellers from "../../components/client/BestSellers.vue";
import Lookbook from "../../components/client/Lookbook.vue";
import TrustNewsletter from "../../components/client/TrustNewsletter.vue";
</script>
