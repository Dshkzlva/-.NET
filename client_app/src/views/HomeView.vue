<template>
  <div>
    <section class="hero-banner">
      <div class="hero-overlay">
        <div class="container hero-content">
          <h1>Спортивные секции<br />ЮУрГУ</h1>
          <p>Выбирай направление, записывайся онлайн и<br />занимайся спортом вместо физры</p>
          <a href="#sections-catalog" class="btn-primary">Перейти к секциям</a>
        </div>
      </div>
    </section>

    <section id="sections-catalog" class="container catalog-section">
      <div class="catalog-header">
        <h2>Популярные секции</h2>
        <div class="filters">
          <input v-model="searchQuery" type="text" placeholder="Поиск по виду спорта или тренеру..." class="filter-input" />
          <select v-model="selectedCategory" class="filter-select">
            <option value="">Все направления</option>
            <option v-for="cat in categories" :key="cat" :value="cat">{{ cat }}</option>
          </select>
        </div>
      </div>

      <div class="sections-grid">
        <div v-for="sec in filteredSections" :key="sec.id" class="section-card card">
          <div class="card-img-wrap" @click="$router.push(`/section/${sec.id}`)">
            <img :src="sec.image" :alt="sec.name" />
          </div>
          <div class="card-body">
            <h3 @click="$router.push(`/section/${sec.id}`)">{{ sec.name }}</h3>
            <p class="category-label">{{ sec.categoryName }}</p>
            <router-link :to="`/enroll/${sec.id}`" class="btn-primary full-btn">Записаться</router-link>
          </div>
        </div>
      </div>
    </section>
  </div>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue';
import { api } from '../api/client';

const sections = ref([]);
const searchQuery = ref('');
const selectedCategory = ref('');

onMounted(async () => {
  sections.value = await api.getFullCatalog();
});

const categories = computed(() => [...new Set(sections.value.map(s => s.categoryName))]);

const filteredSections = computed(() =>
  sections.value.filter(s => {
    const matchesSearch =
      s.name.toLowerCase().includes(searchQuery.value.toLowerCase()) ||
      s.coachFullName.toLowerCase().includes(searchQuery.value.toLowerCase());
    const matchesCat = !selectedCategory.value || s.categoryName === selectedCategory.value;
    return matchesSearch && matchesCat;
  })
);
</script>

<style scoped>
.hero-banner {
  background: url('https://unsplash.com') center/cover no-repeat;
  min-height: 360px;
  display: flex;
}
.hero-overlay {
  width: 100%;
  background: linear-gradient(90deg, rgba(17, 45, 94, 0.88) 0%, rgba(30, 68, 138, 0.45) 100%);
  display: flex;
  align-items: center;
  padding: 48px 0;
}
.hero-content { color: #fff; width: 100%; }
.hero-content h1 { font-size: 42px; font-weight: 800; line-height: 1.15; margin-bottom: 16px; }
.hero-content p { font-size: 16px; opacity: 0.92; margin-bottom: 24px; }
.catalog-section { padding: 40px 24px 64px; }
.catalog-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  flex-wrap: wrap;
  gap: 16px;
  margin-bottom: 28px;
}
.catalog-header h2 { color: var(--susu-dark); font-size: 26px; font-weight: 800; }
.filters { display: flex; gap: 12px; }
.filter-input, .filter-select {
  padding: 10px 14px;
  border: 1px solid var(--border-light);
  border-radius: 8px;
  background: #fff;
  font-size: 14px;
}
.sections-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(270px, 1fr));
  gap: 24px;
}
.section-card { overflow: hidden; padding: 14px; display: flex; flex-direction: column; }
.card-img-wrap { height: 140px; border-radius: 10px; overflow: hidden; cursor: pointer; margin-bottom: 14px; }
.card-img-wrap img { width: 100%; height: 100%; object-fit: cover; }
.card-body h3 { color: var(--susu-dark); font-size: 18px; font-weight: 700; cursor: pointer; }
.category-label { color: var(--text-muted); font-size: 13px; margin: 4px 0 16px; }
.full-btn { width: 100%; }
</style>
