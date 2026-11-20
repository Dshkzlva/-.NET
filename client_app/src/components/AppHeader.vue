<template>
  <header class="susu-header" :class="{ 'header-centered': minimal }">
    <div class="container header-inner">
      <router-link to="/" class="logo-block">
        <div class="emblem">🏛️</div>
        <div class="logo-text">
          <span class="logo-title">ЮУрГУ</span>
          <span class="logo-sub">Южно-Уральский государственный университет</span>
        </div>
      </router-link>

      <nav v-if="!minimal" class="nav-links">
        <router-link to="/">Секции</router-link>
        <a href="#" @click.prevent="goToCabinet">Личный кабинет</a>
        <router-link to="/about">О проекте</router-link>
      </nav>

      <div v-if="!minimal" class="user-action">
        <button class="profile-btn" @click="goToCabinet" :title="userName">👤</button>
      </div>
    </div>
  </header>
</template>

<script setup>
import { computed } from 'vue';
import { useRouter } from 'vue-router';

defineProps({ minimal: { type: Boolean, default: false } });

const router = useRouter();
const user = computed(() => JSON.parse(localStorage.getItem('susu_user') || 'null'));
const userName = computed(() => user.value ? user.value.fullName : 'Войти в систему');

function goToCabinet() {
  if (!user.value) router.push('/login');
  else if (user.value.role === 'teacher') router.push('/cabinet/teacher');
  else router.push('/cabinet/student');
}
</script>

<style scoped>
.susu-header {
  background-color: var(--susu-navy);
  color: #fff;
  height: 68px;
  display: flex;
  align-items: center;
}
.header-inner {
  width: 100%;
  display: flex;
  align-items: center;
  justify-content: space-between;
}
.header-centered .header-inner {
  justify-content: center;
}
.logo-block {
  display: flex;
  align-items: center;
  gap: 12px;
  text-decoration: none;
  color: #fff;
}
.emblem { font-size: 28px; }
.logo-text { display: flex; flex-direction: column; }
.logo-title { font-weight: 800; font-size: 20px; line-height: 1.1; }
.logo-sub { font-size: 11px; opacity: 0.85; }
.nav-links { display: flex; gap: 32px; }
.nav-links a {
  color: #fff;
  text-decoration: none;
  font-size: 15px;
  font-weight: 500;
  opacity: 0.9;
}
.nav-links a:hover, .nav-links a.router-link-active {
  opacity: 1;
  text-decoration: underline;
  text-underline-offset: 6px;
}
.profile-btn {
  width: 36px;
  height: 36px;
  border-radius: 50%;
  border: none;
  background: #fff;
  cursor: pointer;
  font-size: 16px;
}
</style>
