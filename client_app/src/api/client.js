const API_BASE = 'http://localhost:5000/api';

async function safeGet(endpoint, fallback = []) {
  try {
    const res = await fetch(`${API_BASE}${endpoint}`);
    if (!res.ok) throw new Error('API Error');
    return await res.json();
  } catch {
    return fallback;
  }
}

export const api = {
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
          { dayOfWeek: 'Вторник', startTime: '17:00:00', endTime: '18:30:00', facilityName: 'Зал №1' },
          { dayOfWeek: 'Четверг', startTime: '17:00:00', endTime: '18:30:00', facilityName: 'Зал №1' }
        ],
        desc: `Секция «${sec.name}» ЮУрГУ — отличная возможность поддержать физическую форму, развить командные навыки и принять участие в университетских соревнованиях.`,
        level: 'для всех',
        spots: 25,
        duration: '90 минут',
        image: getSectionImage(sec.name)
      };
    });
  },

  async login(email, roleName) {
    const [users, roles, profiles] = await Promise.all([
      safeGet('/users'),
      safeGet('/userroles'),
      safeGet('/studentprofiles')
    ]);

    const found = users.find(u => u.email.toLowerCase() === email.toLowerCase());
    if (found) {
      const role = roles.find(r => r.id === found.roleId)?.name || roleName;
      const profile = profiles.find(p => p.userId === found.id);
      return {
        id: found.id,
        email: found.email,
        fullName: `${found.lastName} ${found.firstName}`,
        role,
        groupNumber: profile?.groupNumber || 'КЭ-305'
      };
    }

    if (roleName === 'teacher') {
      return {
        id: '00000000-0000-0000-0000-000000000002',
        email: email || 'sokolov@susu.ru',
        fullName: 'Соколов П.П.',
        position: 'Доцент кафедры физкультурного воспитания',
        role: 'teacher'
      };
    }

    return {
      id: '00000000-0000-0000-0000-000000000001',
      email: email || 'ivanov@susu.ru',
      fullName: 'Иванов Иван',
      groupNumber: 'КЭ-305',
      role: 'student'
    };
  },

  async getEnrollments(userId) {
    const [enrollments, catalog] = await Promise.all([
      safeGet('/enrollments'),
      this.getFullCatalog()
    ]);

    const userItems = enrollments.filter(e => e.userId === userId);
    if (!userItems.length) {
      return [
        {
          id: 1,
          sectionId: 1,
          sectionName: 'Футбол',
          categoryName: 'Командные игры',
          scheduleText: 'Вт, Чт, Пт: 17:00-18:30',
          facility: 'Спортивная площадка',
          status: 'active'
        }
      ];
    }

    return userItems.map(e => {
      const sec = catalog.find(s => s.id === e.sectionId);
      const firstSchedule = sec && sec.schedules && sec.schedules;
      return {
        id: e.id,
        sectionId: e.sectionId,
        sectionName: sec ? sec.name : 'Футбол',
        categoryName: sec ? sec.categoryName : 'Командные игры',
        scheduleText: sec && sec.schedules
          ? sec.schedules.map(s => `${s.dayOfWeek}: ${s.startTime.slice(0, 5)}-${s.endTime.slice(0, 5)}`).join(', ')
          : 'Вт, Чт, Пт: 17:00-18:30',
        facility: firstSchedule ? firstSchedule.facilityName : 'Спортивная площадка',
        status: e.status
      };
    });
  },

  async createEnrollment(payload) {
    try {
      const res = await fetch(`${API_BASE}/enrollments`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(payload)
      });
      return res.ok ? await res.json() : payload;
    } catch {
      return payload;
    }
  },

  async cancelEnrollment(id) {
    try {
      await fetch(`${API_BASE}/enrollments/${id}`, { method: 'DELETE' });
    } catch {}
  }
};

function getSectionImage(name) {
  const map = {
    'Футбол': 'Футбол.png',
    'Баскетбол': 'Баскетбол.png',
    'Волейбол': 'Волейбол.png',
    'Плавание': 'Плаванье.png',
    'Теннис': 'Тенис.png',
    'Борьба': 'Борьба.png',
    'Йога': 'Йога.png',
    'Шахматы': 'Шахматы.png'
  };
  const fileName = map[name] || 'Футбол.png';
  return new URL(`../assets/images/${fileName}`, import.meta.url).href;
}

function getMockSections() {
  const list = [
    { id: 1, name: 'Футбол', categoryName: 'Командные игры', coachFullName: 'Иванов Сергей Александрович' },
    { id: 2, name: 'Баскетбол', categoryName: 'Командные игры', coachFullName: 'Кузнецов Дмитрий Игоревич' },
    { id: 3, name: 'Волейбол', categoryName: 'Командные игры', coachFullName: 'Петрова Анна Игоревна' },
    { id: 4, name: 'Плавание', categoryName: 'Водный спорт', coachFullName: 'Смирнов Алексей Владимирович' },
    { id: 5, name: 'Теннис', categoryName: 'Ракеточные виды', coachFullName: 'Орлов Игорь Викторович' },
    { id: 6, name: 'Борьба', categoryName: 'Единоборства', coachFullName: 'Волков Максим Сергеевич' },
    { id: 7, name: 'Йога', categoryName: 'Общая подготовка', coachFullName: 'Соколова Елена Павловна' },
    { id: 8, name: 'Шахматы', categoryName: 'Интеллектуальный спорт', coachFullName: 'Морозов Андрей Николаевич' }
  ];
  return list.map(item => ({
    ...item,
    level: 'для всех',
    spots: 25,
    duration: '90 минут',
    desc: `Секция «${item.name}» ЮУрГУ — отличная возможность поддержать физическую форму и принять участие в университетских турнирах.`,
    image: getSectionImage(item.name),
    schedules: [
      { dayOfWeek: 'Вторник', startTime: '17:00:00', endTime: '18:30:00', facilityName: 'Зал №1' },
      { dayOfWeek: 'Четверг', startTime: '17:00:00', endTime: '18:30:00', facilityName: 'Зал №1' }
    ]
  }));
}
