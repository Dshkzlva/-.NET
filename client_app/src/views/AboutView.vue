<template>
  <div class="container about-page">
    <div class="about-banner">
      <h1>О портале спортивных секций ЮУрГУ</h1>
      <p>Единая платформа для записи на спортивные дисциплины и отслеживания расписания студентов.</p>
    </div>

    <div class="about-grid">
      <div>
        <h2>О системе</h2>
        <p class="system-desc">
          Веб-портал разработан для упрощения и автоматизации процесса записи студентов ЮУрГУ на секции
          элективной физической культуры. Платформа позволяет выбирать интересующий вид спорта, ознакомиться с
          преподавательским составом, расписанием занятий и местом их проведения в режиме реального времени.
        </p>

        <div class="features-grid">
          <div class="card feature-card">
            <h4>Удобная запись</h4>
            <p>Быстрый выбор секции в пару кликов без бумажных заявлений.</p>
          </div>
          <div class="card feature-card">
            <h4>Актуальное расписание</h4>
            <p>Синхронизация дней и времени тренировок в личном кабинете.</p>
          </div>
          <div class="card feature-card">
            <h4>Чаты секций</h4>
            <p>Прямые ссылки на ВКонтакте-чаты с тренером и вашей группой.</p>
          </div>
          <div class="card feature-card">
            <h4>Спортивные базы</h4>
            <p>Полный список площадок, бассейнов и манежей университета.</p>
          </div>
        </div>
      </div>

      <div>
        <div class="card contact-card">
          <div class="address">г. Челябинск, ул. Коммуны, 15</div>
          <h3>Спортивный клуб ЮУрГУ</h3>
          <div class="phone">+7 (351) 267-92-65</div>
          <a href="mailto:sport@susu.ru" class="email">sport@susu.ru</a>
          <div class="hours">Пн – Пт: 09:00 – 17:00</div>
        </div>

        <div class="help-card">
          <h4>Возникли вопросы?</h4>
          <p>При возникновении проблем с записью обратитесь в директорат своего института или в спортклуб ЮУрГУ.</p>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
.about-page { padding: 36px 24px 64px; }
.about-banner {
  background: linear-gradient(135deg, #1E448A 0%, #1D63DC 100%);
  color: #fff;
  padding: 44px 48px;
  border-radius: 18px;
  margin-bottom: 36px;
}
.about-banner h1 { font-size: 32px; margin-bottom: 10px; }
.about-banner p { opacity: 0.9; font-size: 16px; }
.about-grid { display: grid; grid-template-columns: 1.7fr 1fr; gap: 32px; }
h2 { color: var(--susu-dark); font-size: 24px; margin-bottom: 14px; }
.system-desc { color: #334155; margin-bottom: 24px; font-size: 15px; }
.features-grid { display: grid; grid-template-columns: 1fr 1fr; gap: 16px; }
.feature-card { padding: 20px; }
.feature-card h4 { color: var(--susu-navy); margin-bottom: 6px; }
.feature-card p { font-size: 13px; color: var(--text-muted); }
.contact-card { padding: 28px; margin-bottom: 16px; }
.address { font-size: 13px; color: var(--text-muted); margin-bottom: 8px; }
.contact-card h3 { color: var(--susu-dark); margin-bottom: 12px; }
.phone { font-weight: 600; margin-bottom: 8px; }
.email { display: inline-block; color: var(--susu-blue); text-decoration: none; margin-bottom: 12px; }
.hours { font-size: 13px; color: var(--text-muted); }
.help-card { background: var(--susu-dark); color: #fff; padding: 24px 28px; border-radius: 14px; }
.help-card h4 { margin-bottom: 8px; }
.help-card p { font-size: 13px; opacity: 0.85; }
@media (max-width: 820px) { .about-grid, .features-grid { grid-template-columns: 1fr; } }
</style>
