<template>
  <div class="container detail-page" v-if="section">
    <router-link to="/" class="back-link">← Назад к секциям</router-link>

    <div class="title-row">
      <h1>{{ section.name }}</h1>
      <span class="subtitle">{{ section.categoryName }}</span>
    </div>

    <div class="detail-grid">
      <div>
        <img :src="section.image" :alt="section.name" class="banner-img" />
        <div class="card info-card">
          <h3>О секции</h3>
          <p class="desc-text">{{ section.desc }}</p>
          <ul class="specs-list">
            <li>Уровень подготовки: <strong>{{ section.level }}</strong></li>
            <li>Количество мест: <strong>{{ section.spots }}</strong></li>
            <li>Продолжительность занятия: <strong>{{ section.duration }}</strong></li>
          </ul>
          <router-link :to="`/enroll/${section.id}`" class="btn-primary">Записаться на секцию</router-link>
        </div>
      </div>

      <div>
        <div class="card side-card">
          <h4>Преподаватель</h4>
          <div class="coach-box">
            <div class="avatar">👨‍🏫</div>
            <div>
              <div class="coach-name">{{ section.coachFullName }}</div>
              <div class="coach-role">Тренер кафедры ФВ ЮУрГУ</div>
            </div>
          </div>
        </div>

        <div class="card side-card">
          <h4>Расписание</h4>
          <div v-for="(item, idx) in section.schedules" :key="idx" class="schedule-row">
            <span>{{ item.dayOfWeek }}</span>
            <strong>{{ item.startTime.slice(0, 5) }} – {{ item.endTime.slice(0, 5) }}</strong>
          </div>
        </div>

        <div class="card side-card">
          <h4>Чат группы ВКонтакте</h4>
          <a href="https://vk.com" target="_blank" class="btn-primary btn-sm">Перейти в чат</a>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue';
import { useRoute } from 'vue-router';
import { api } from '../api/client';

const route = useRoute();
const section = ref(null);

onMounted(async () => {
  const all = await api.getFullCatalog();
  section.value = all.find(s => s.id === Number(route.params.id)) || all;
});
</script>

<style scoped>
.detail-page { padding: 32px 24px 64px; }
.back-link { color: var(--susu-blue); text-decoration: none; font-size: 14px; font-weight: 500; }
.title-row { margin: 12px 0 24px; }
.title-row h1 { font-size: 32px; color: var(--susu-dark); }
.subtitle { color: var(--text-muted); font-size: 14px; }
.detail-grid { display: grid; grid-template-columns: 2fr 1fr; gap: 24px; }
.banner-img { width: 100%; height: 260px; object-fit: cover; border-radius: 14px; margin-bottom: 20px; }
.info-card, .side-card { padding: 24px; margin-bottom: 20px; }
.info-card h3, .side-card h4 { color: var(--susu-navy); margin-bottom: 12px; }
.desc-text { color: #334155; margin-bottom: 18px; }
.specs-list { list-style: none; margin-bottom: 24px; display: flex; flex-direction: column; gap: 6px; font-size: 14px; }
.coach-box { display: flex; align-items: center; gap: 12px; }
.avatar { width: 48px; height: 48px; border-radius: 50%; background: #E2E8F0; display: flex; align-items: center; justify-content: center; font-size: 24px; }
.coach-name { font-weight: 700; font-size: 15px; }
.coach-role { font-size: 13px; color: var(--text-muted); }
.schedule-row { display: flex; justify-content: space-between; padding: 8px 0; border-bottom: 1px solid #F1F5F9; font-size: 14px; }
.btn-sm { padding: 8px 16px; font-size: 13px; }
@media (max-width: 820px) { .detail-grid { grid-template-columns: 1fr; } }
</style>
