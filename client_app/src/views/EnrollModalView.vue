<template>
  <div class="auth-bg">
    <div class="auth-card card">
      <h1 class="auth-title">Запись на секцию</h1>
      <p class="section-subtitle">{{ sectionName }}</p>

      <form @submit.prevent="submitEnrollment" class="auth-form">
        <input v-model="fullName" type="text" class="form-input" placeholder="ФИО" required />
        <input v-model="group" type="text" class="form-input" placeholder="Группа" required />
        <input v-model="phone" type="tel" class="form-input" placeholder="Номер телефона" required />

        <label class="consent-label">
          <input v-model="consent" type="checkbox" required />
          <span>Согласие на обработку персональных данных</span>
        </label>

        <button type="submit" class="btn-primary full-width">Записаться</button>
      </form>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import { api } from '../api/client';

const route = useRoute();
const router = useRouter();
const sectionName = ref('Футбол');
const fullName = ref('');
const group = ref('');
const phone = ref('');
const consent = ref(false);

onMounted(async () => {
  const user = JSON.parse(localStorage.getItem('susu_user') || 'null');
  if (user) {
    fullName.value = user.fullName;
    group.value = user.groupNumber || '';
  }
  const catalog = await api.getFullCatalog();
  const found = catalog.find(s => s.id === Number(route.params.id));
  if (found) sectionName.value = found.name;
});

async function submitEnrollment() {
  const user = JSON.parse(localStorage.getItem('susu_user') || 'null');
  await api.createEnrollment({
    userId: user?.id || '00000000-0000-0000-0000-000000000001',
    sectionId: Number(route.params.id),
    status: 'active'
  });
  router.push('/cabinet/student');
}
</script>

<style scoped>
.auth-bg {
  min-height: calc(100vh - 128px);
  display: flex;
  align-items: center;
  justify-content: center;
  background: radial-gradient(circle at 20% 20%, #e9eef7 0%, #f7f9fc 100%);
  padding: 24px;
}
.auth-card { width: 100%; max-width: 420px; padding: 36px 32px; text-align: center; border-radius: 18px; }
.auth-title { color: var(--susu-dark); font-size: 24px; font-weight: 700; }
.section-subtitle { color: var(--susu-navy); font-weight: 600; margin: 4px 0 22px; }
.auth-form { display: flex; flex-direction: column; gap: 14px; }
.form-input {
  width: 100%;
  padding: 13px 16px;
  border: 1px solid var(--border-light);
  border-radius: 8px;
  background: #F8FAFC;
  font-size: 14px;
}
.consent-label { display: flex; align-items: center; gap: 10px; text-align: left; font-size: 13px; margin: 4px 0; cursor: pointer; }
.full-width { width: 100%; }
</style>
