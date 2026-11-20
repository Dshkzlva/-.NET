<template>
  <footer class="susu-footer">
    <div class="container footer-inner">
      <div>© 2026 Южно-Уральский государственный университет</div>
      <div class="footer-links">
        <router-link to="/about">Контакты</router-link>
        <router-link to="/about">Правила</router-link>
        <router-link to="/about">Правовая информация</router-link>
      </div>
    </div>
  </footer>
</template>

<style scoped>
.susu-footer {
  background-color: var(--susu-dark);
  color: rgba(255, 255, 255, 0.85);
  padding: 20px 0;
  font-size: 13px;
  margin-top: auto;
}
.footer-inner {
  display: flex;
  justify-content: space-between;
  align-items: center;
}
.footer-links { display: flex; gap: 24px; }
.footer-links a { color: rgba(255, 255, 255, 0.85); text-decoration: none; }
.footer-links a:hover { color: #fff; }
</style>
