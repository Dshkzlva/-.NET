<template>
  <div class="cabinet-layout">
    <aside class="sidebar">
      <div class="profile-card">
        <div class="avatar-circle">👨‍🎓</div>
        <div>
          <div class="user-name">{{ user.fullName }}</div>
          <div class="user-group">{{ user.groupNumber || 'КЭ-305' }}</div>
        </div>
      </div>

      <nav class="side-nav">
        <router-link to="/">🏠 Главная</router-link>
        <a href="#" class="active">💪 Мои секции</a>
        <a href="#">📅 Расписание</a>
        <a href="#">🔔 Уведомления</a>
        <a href="#">⚙️ Настройки</a>
      </nav>

      <button class="logout-btn" @click="logout">🚪 Выйти</button>
    </aside>

    <main class="cabinet-main">
      <section class="block">
        <h2>Мои секции</h2>
        <div v-if="!enrollments.length" class="card empty-card">
          Вы пока не записаны ни в одну секцию. <router-link to="/">Выбрать секцию</router-link>
        </div>

        <div v-for="item in enrollments" :key="item.id" class="card my-section-row">
          <div class="sec-info">
            <span class="sport-icon">⚽</span>
            <div>
              <div class="sec-title">{{ item.sectionName }}</div>
              <div class="sec-cat">{{ item.categoryName }}</div>
            </div>
          </div>

          <div class="time-pill">{{ item.scheduleText }}</div>

          <button class="btn-primary" @click="removeEnrollment(item.id)">Отменить запись</button>
        </div>
      </section>

      <section class="block">
        <h2>Ближайшие занятия</h2>
        <div v-for="item in enrollments" :key="'next-' + item.id" class="card next-lesson-card">
          <div class="date-badge">
            <strong>26</strong>
            <span>сентября</span>
          </div>
          <div>
            <div class="sec-title">{{ item.sectionName }}</div>
            <div class="lesson-time">17:00-18:30</div>
            <div class="lesson-place">{{ item.facility }}</div>
          </div>
        </div>
      </section>
    </main>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue';
import { useRouter } from 'vue-router';
import { api } from '../api/client';

const router = useRouter();
const user = ref({ fullName: 'Иванов Иван', groupNumber: 'КЭ-305' });
const enrollments = ref([]);

onMounted(async () => {
  const saved = JSON.parse(localStorage.getItem('susu_user') || 'null');
  if (saved) user.value = saved;
  enrollments.value = await api.getEnrollments(user.value.id);
});

async function removeEnrollment(id) {
  await api.cancelEnrollment(id);
  enrollments.value = enrollments.value.filter(e => e.id !== id);
}

function logout() {
  localStorage.removeItem('susu_user');
  router.push('/login');
}
</script>

<style scoped>
.cabinet-layout { display: flex; min-height: calc(100vh - 128px); }
.sidebar { width: 260px; background: #E9EDF4; padding: 24px 16px; display: flex; flex-direction: column; }
.profile-card { background: #DCE2EC; padding: 14px; border-radius: 12px; display: flex; align-items: center; gap: 12px; margin-bottom: 24px; }
.avatar-circle { width: 42px; height: 42px; border-radius: 50%; background: #fff; display: flex; align-items: center; justify-content: center; font-size: 20px; }
.user-name { font-weight: 700; font-size: 15px; }
.user-group { font-size: 13px; color: var(--text-muted); }
.side-nav { display: flex; flex-direction: column; gap: 8px; }
.side-nav a { padding: 10px 14px; border-radius: 8px; text-decoration: none; color: var(--text-main); font-weight: 500; font-size: 14px; }
.side-nav a.active { color: var(--susu-blue); font-weight: 700; }
.logout-btn { margin-top: auto; background: none; border: none; text-align: left; padding: 12px 14px; font-size: 14px; font-weight: 600; cursor: pointer; }
.cabinet-main { flex: 1; padding: 36px 48px; }
.block { margin-bottom: 36px; }
.block h2 { color: var(--susu-blue); font-size: 24px; margin-bottom: 18px; }
.my-section-row { padding: 20px 24px; display: flex; align-items: center; justify-content: space-between; gap: 20px; margin-bottom: 14px; }
.sec-info { display: flex; align-items: center; gap: 14px; }
.sport-icon { font-size: 28px; }
.sec-title { font-weight: 700; font-size: 17px; }
.sec-cat { font-size: 13px; color: var(--text-muted); }
.time-pill { background: #F4F6FA; padding: 14px 24px; border-radius: 10px; font-weight: 600; font-size: 14px; }
.next-lesson-card { padding: 20px 24px; display: flex; align-items: center; gap: 20px; margin-bottom: 14px; }
.date-badge { background: #F1F5F9; border-radius: 12px; padding: 12px 18px; text-align: center; min-width: 90px; }
.date-badge strong { display: block; font-size: 24px; color: var(--susu-dark); }
.date-badge span { font-size: 12px; color: var(--text-muted); }
.lesson-time { font-weight: 600; font-size: 14px; margin: 2px 0; }
.lesson-place { font-size: 13px; color: var(--text-muted); }
.empty-card { padding: 24px; }
</style>
