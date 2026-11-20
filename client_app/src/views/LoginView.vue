<template>
  <div class="auth-bg">
    <div class="auth-card card">
      <h1 class="auth-title">Спортивные секции ЮУрГУ</h1>

      <div class="role-switch">
        <button :class="{ active: role === 'student' }" @click="role = 'student'">Студент</button>
        <button :class="{ active: role === 'teacher' }" @click="role = 'teacher'">Преподаватель</button>
      </div>

      <form @submit.prevent="handleLogin" class="auth-form">
        <input v-model="email" type="text" class="form-input" placeholder="логин (@susu.ru)" required />
        <input v-model="password" type="password" class="form-input" placeholder="пароль" required />
        <button type="submit" class="btn-primary full-width">Вход</button>
      </form>

      <a href="#" class="forgot-link" @click.prevent="alert('Обратитесь в спортклуб ЮУрГУ')">Забыли пароль?</a>

      <div class="auth-meta">
        <span class="lang">Русский (ru)</span>
        <span class="divider">|</span>
        <span>Уведомление о файлах cookie</span>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref } from 'vue';
import { useRouter } from 'vue-router';
import { api } from '../api/client';

const router = useRouter();
const email = ref('');
const password = ref('');
const role = ref('student');

async function handleLogin() {
  const userData = await api.login(email.value, role.value);
  localStorage.setItem('susu_user', JSON.stringify(userData));
  router.push(userData.role === 'teacher' ? '/cabinet/teacher' : '/cabinet/student');
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
.auth-card {
  width: 100%;
  max-width: 420px;
  padding: 40px 32px 24px;
  text-align: center;
  border-radius: 18px;
}
.auth-title { color: var(--susu-dark); font-size: 24px; font-weight: 700; margin-bottom: 20px; }
.role-switch {
  display: grid;
  grid-template-columns: 1fr 1fr;
  background: #F1F5F9;
  padding: 4px;
  border-radius: 8px;
  margin-bottom: 18px;
}
.role-switch button {
  border: none;
  background: transparent;
  padding: 8px;
  border-radius: 6px;
  font-weight: 600;
  color: var(--text-muted);
  cursor: pointer;
}
.role-switch button.active { background: #fff; color: var(--susu-navy); }
.auth-form { display: flex; flex-direction: column; gap: 14px; }
.form-input {
  width: 100%;
  padding: 13px 16px;
  border: 1px solid var(--border-light);
  border-radius: 8px;
  background: #F8FAFC;
  font-size: 14px;
}
.full-width { width: 100%; margin-top: 4px; }
.forgot-link { display: inline-block; margin: 16px 0 20px; color: var(--susu-blue); font-size: 14px; text-decoration: none; }
.auth-meta {
  border-top: 1px solid var(--border-light);
  padding-top: 14px;
  display: flex;
  justify-content: center;
  gap: 16px;
  font-size: 12px;
  color: var(--text-muted);
}
.lang { color: var(--susu-navy); font-weight: 600; }
</style>
