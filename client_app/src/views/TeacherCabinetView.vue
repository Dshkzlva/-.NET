<template>
  <div class="cabinet-layout">
    <aside class="sidebar">
      <div class="profile-card">
        <div class="avatar-circle">👨‍🏫</div>
        <div>
          <div class="user-name">{{ teacher.fullName }}</div>
          <div class="user-group">{{ teacher.position || 'Доцент кафедры физкультурного воспитания' }}</div>
        </div>
      </div>

      <nav class="side-nav">
        <router-link to="/">🏠 Главная</router-link>
        <a href="#">💪 Мои секции</a>
        <a href="#" class="active">📅 Планирование занятий</a>
        <a href="#">🔔 Уведомления</a>
        <a href="#">⚙️ Настройки</a>
      </nav>

      <button class="logout-btn" @click="logout">🚪 Выйти</button>
    </aside>

    <main class="cabinet-main">
      <h1 class="page-title">Планирование занятий</h1>

      <div class="mode-buttons">
        <button class="mode-btn dark" @click="applyDayType('holiday')">🏖️ Выходные</button>
        <button class="mode-btn blue" @click="applyDayType('work')">💼 Рабочие дни</button>
        <button class="mode-btn light" @click="applyDayType('empty')">✕ Очистить</button>
      </div>

      <div class="planner-grid">
        <div class="card calendar-card">
          <h3>Сентябрь 2026</h3>
          <div class="weekdays">
            <span>ПН</span><span>ВТ</span><span>СР</span><span>ЧТ</span><span>ПТ</span>
            <span class="muted">СБ</span><span class="muted">ВС</span>
          </div>

          <div class="days-grid">
            <div
              v-for="day in calendarDays"
              :key="day.num"
              class="day-cell"
              :class="[day.type, { selected: selectedDay?.num === day.num }]"
              @click="selectedDay = day"
            >
              <span class="day-num">{{ day.num }}</span>
              <span v-if="day.hours" class="day-hours">{{ day.hours }}</span>
            </div>
          </div>
        </div>

        <div class="card details-card" v-if="selectedDay">
          <h3>Занятия на {{ selectedDay.num }} сентября</h3>

          <div v-if="selectedDay.type === 'work'" class="lesson-box">
            <div class="lesson-row">
              <span class="blue-text">Время: «08:00 – 09:30»</span>
              <span class="blue-text">Запись: «18/20 чел.»</span>
            </div>
            <div class="lesson-row bottom">
              <strong>Название: «Футбол (Секция)»</strong>
              <span class="muted">Зал: «Зал №1»</span>
            </div>
          </div>
          <div v-else class="lesson-box muted">
            На выбранную дату занятий не запланировано.
          </div>

          <button
            v-if="selectedDay.type === 'work'"
            class="btn-primary full-width"
            @click="attendanceMarked = true"
          >
            {{ attendanceMarked ? '✓ Посещаемость отмечена' : 'Отметить посещаемость' }}
          </button>
        </div>
      </div>
    </main>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue';
import { useRouter } from 'vue-router';

const router = useRouter();
const teacher = ref({
  fullName: 'Соколов П.П.',
  position: 'Доцент кафедры физкультурного воспитания'
});
const attendanceMarked = ref(false);

const calendarDays = ref([
  { num: 20, type: 'empty', hours: '' },
  { num: 21, type: 'empty', hours: '' },
  { num: 22, type: 'work', hours: '8:00 - 16:00' },
  { num: 23, type: 'work', hours: '8:00 - 16:00' },
  { num: 24, type: 'work', hours: '8:00 - 16:00' },
  { num: 25, type: 'holiday', hours: '' },
  { num: 26, type: 'holiday', hours: '' },
  { num: 27, type: 'work', hours: '8:00 - 16:00' },
  { num: 28, type: 'work', hours: '8:00 - 16:00' },
  { num: 29, type: 'empty', hours: '' },
  { num: 30, type: 'work', hours: '8:00 - 16:00' },
  { num: 1, type: 'next-month', hours: '' },
  { num: 2, type: 'next-month', hours: '' },
  { num: 3, type: 'next-month', hours: '' }
]);

const selectedDay = ref(calendarDays.value);

onMounted(() => {
  const saved = JSON.parse(localStorage.getItem('susu_user') || 'null');
  if (saved && saved.role === 'teacher') teacher.value = saved;
});

function applyDayType(type) {
  if (!selectedDay.value) return;
  selectedDay.value.type = type;
  selectedDay.value.hours = type === 'work' ? '8:00 - 16:00' : '';
  attendanceMarked.value = false;
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
.user-group { font-size: 11px; color: var(--text-muted); }
.side-nav { display: flex; flex-direction: column; gap: 8px; }
.side-nav a { padding: 10px 14px; border-radius: 8px; text-decoration: none; color: var(--text-main); font-weight: 500; font-size: 14px; }
.side-nav a.active { color: var(--susu-blue); font-weight: 700; }
.logout-btn { margin-top: auto; background: none; border: none; text-align: left; padding: 12px 14px; font-size: 14px; font-weight: 600; cursor: pointer; }
.cabinet-main { flex: 1; padding: 36px 48px; }
.page-title { color: var(--susu-blue); font-size: 28px; margin-bottom: 18px; }
.mode-buttons { display: flex; gap: 12px; margin-bottom: 24px; }
.mode-btn { padding: 10px 18px; border-radius: 8px; border: none; font-weight: 600; font-size: 13px; cursor: pointer; }
.mode-btn.dark { background: var(--susu-dark); color: #fff; }
.mode-btn.blue { background: var(--susu-blue); color: #fff; }
.mode-btn.light { background: #fff; color: var(--text-main); border: 1px solid var(--border-light); }
.planner-grid { display: grid; grid-template-columns: 1.5fr 1fr; gap: 28px; align-items: start; }
.calendar-card, .details-card { padding: 28px; }
.calendar-card h3 { color: var(--susu-navy); font-size: 20px; margin-bottom: 18px; }
.weekdays { display: grid; grid-template-columns: repeat(7, 1fr); text-align: center; font-weight: 700; color: var(--susu-navy); font-size: 13px; margin-bottom: 10px; }
.days-grid { display: grid; grid-template-columns: repeat(7, 1fr); gap: 10px; }
.day-cell {
  height: 76px;
  border-radius: 12px;
  padding: 8px 4px;
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: space-between;
  cursor: pointer;
  border: 1px solid #CBD5E1;
  background: #fff;
}
.day-cell.selected { outline: 3px solid #93C5FD; }
.day-cell.work { background: var(--susu-blue); color: #fff; border-color: var(--susu-blue); }
.day-cell.holiday { background: var(--susu-dark); color: #fff; border-color: var(--susu-dark); }
.day-cell.next-month { background: #F1F5F9; border-color: transparent; }
.day-num { font-weight: 700; font-size: 16px; }
.day-hours { font-size: 9px; opacity: 0.95; }
.lesson-box { background: #F4F6FA; border-radius: 12px; padding: 18px; margin: 18px 0 22px; }
.lesson-row { display: flex; justify-content: space-between; font-size: 13px; }
.lesson-row.bottom { margin-top: 12px; align-items: center; }
.blue-text { color: var(--susu-blue); font-weight: 600; }
.muted { color: var(--text-muted); }
.full-width { width: 100%; }
</style>
