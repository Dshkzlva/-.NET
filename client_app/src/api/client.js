const API_BASE = '/api';

// Получаем токен из localStorage
function getToken() {
  return localStorage.getItem('jwt_token');
}

// Универсальная функция запросов с автоматической подстановкой токена
async function apiFetch(endpoint, options = {}) {
  const token = getToken();
  const headers = {
    'Content-Type': 'application/json',
    ...(token ? { 'Authorization': `Bearer ${token}` } : {}),
    ...options.headers
  };

  const res = await fetch(`${API_BASE}${endpoint}`, { ...options, headers });
  if (!res.ok) throw new Error(`API Error: ${res.status}`);
  
  // Если ответ пустой (например, 204 No Content для DELETE), возвращаем null
  if (res.status === 204) return null;
  
  return await res.json();
}

// Безопасное получение данных (с fallback, если API недоступен)
async function safeGet(endpoint, fallback = []) {
  try {
    return await apiFetch(endpoint);
  } catch {
    return fallback;
  }
}

export const api = {
  // НОВЫЙ МЕТОД ВХОДА ЧЕРЕЗ JWT
  async login(email) {
    try {
      const res = await fetch(`${API_BASE}/auth/login`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ email })
      });

      if (!res.ok) return null; // Пользователь не найден

      const data = await res.json();
      
      // 1. Сохраняем токен в браузере
      localStorage.setItem('jwt_token', data.token);

      // 2. Получаем профиль студента, чтобы узнать группу (теперь это защищённый запрос, но у нас есть токен!)
      let groupNumber = 'Не указана';
      if (data.user.roleId === 1) {
        try {
          const profiles = await apiFetch('/studentprofiles');
          const profile = profiles.find(p => p.userId === data.user.id);
          if (profile) groupNumber = profile.groupNumber || 'Не указана';
        } catch (e) {
          console.warn('Не удалось получить профиль', e);
        }
      }

      return {
        id: data.user.id,
        email: data.user.email,
        fullName: `${data.user.lastName} ${data.user.firstName}`,
        role: data.user.roleId === 2 ? 'teacher' : 'student',
        groupNumber: groupNumber
      };
    } catch (error) {
      console.error('Login failed:', error);
      return null;
    }
  },

  async getFullCatalog() {
    const [sections, categories, coaches, schedules, facilities] = await Promise.all([
      safeGet('/sportssections'),
      safeGet('/sportcategories'),
      safeGet('/coaches'),
      safeGet('/schedules'),
      safeGet('/facilities')
    ]);

    if (!Array.isArray(sections) || sections.length === 0) {
      return getMockSections();
    }

    return sections.map(sec => {
      const cat = categories.find(c => c.id === sec.categoryId);
      const coach = coaches.find(c => c.id === sec.coachId);
      const secSchedules = schedules
        .filter(s => s.sectionId === sec.id)
        .map(s => ({
          ...s,
          facilityName: facilities.find(f => f.id === s.facilityId)?.name || 'Зал №1'
        }));

      return {
        ...sec,
        categoryName: cat ? cat.name : 'Спортивная секция',
        coachFullName: coach ? `${coach.lastName} ${coach.firstName}` : 'Тренер кафедры ФВ',
        schedules: secSchedules.length ? secSchedules : [
          { dayOfWeek: 'Вторник', startTime: '17:00:00', endTime: '18:30:00', facilityName: 'Зал №1' }
        ],
        desc: `Секция «${sec.name}» ЮУрГУ — отличная возможность поддержать физическую форму.`,
        level: 'для всех',
        spots: 25,
        duration: '90 минут',
        image: getSectionImage(sec.name)
      };
    });
  },

  async getEnrollments(userId) {
    try {
      const [enrollments, catalog] = await Promise.all([
        apiFetch('/enrollments'), // Теперь это защищённый запрос!
        this.getFullCatalog()
      ]);

      const userItems = enrollments.filter(e => e.userId === userId);
      
      return userItems.map(e => {
        const sec = catalog.find(s => s.id === e.sectionId);
        const firstSchedule = sec?.schedules?.[0];
        return {
          id: e.id,
          sectionId: e.sectionId,
          sectionName: sec ? sec.name : 'Секция',
          categoryName: sec ? sec.categoryName : 'Категория',
          scheduleText: sec?.schedules
            ? sec.schedules.map(s => `${s.dayOfWeek}: ${s.startTime.slice(0, 5)}-${s.endTime.slice(0, 5)}`).join(', ')
            : 'По расписанию',
          facility: firstSchedule ? firstSchedule.facilityName : 'Зал',
          status: e.status
        };
      });
    } catch (error) {
      console.error('Failed to get enrollments:', error);
      return [];
    }
  },

  async createEnrollment(payload) {
    try {
      return await apiFetch('/enrollments', {
        method: 'POST',
        body: JSON.stringify(payload)
      });
    } catch (error) {
      console.error('Failed to create enrollment:', error);
      return null;
    }
  },

  async cancelEnrollment(id) {
    try {
      await apiFetch(`/enrollments/${id}`, { method: 'DELETE' });
      return true;
    } catch (error) {
      console.error('Failed to cancel enrollment:', error);
      return false;
    }
  },
  
  // Метод для выхода из системы
  logout() {
    localStorage.removeItem('jwt_token');
  }
};

function getSectionImage(name) {
  const map = {
    'Футбол': 'Футбол.png', 'Баскетбол': 'Баскетбол.png', 'Волейбол': 'Волейбол.png',
    'Плавание': 'Плаванье.png', 'Теннис': 'Тенис.png', 'Борьба': 'Борьба.png',
    'Йога': 'Йога.png', 'Шахматы': 'Шахматы.png'
  };
  const fileName = map[name] || 'Футбол.png';
  return new URL(`../assets/images/${fileName}`, import.meta.url).href;
}

function getMockSections() {
  return [
    { id: 1, name: 'Футбол', categoryName: 'Командные игры', coachFullName: 'Иванов Сергей' },
    { id: 2, name: 'Баскетбол', categoryName: 'Командные игры', coachFullName: 'Кузнецов Дмитрий' }
  ].map(item => ({
    ...item, level: 'для всех', spots: 25, duration: '90 минут',
    desc: `Секция «${item.name}» ЮУрГУ.`,
    image: getSectionImage(item.name),
    schedules: [{ dayOfWeek: 'Вторник', startTime: '17:00:00', endTime: '18:30:00', facilityName: 'Зал №1' }]
  }));
}
