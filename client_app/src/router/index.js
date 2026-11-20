import { createRouter, createWebHistory } from 'vue-router';
import LoginView from '../views/LoginView.vue';
import HomeView from '../views/HomeView.vue';
import SectionDetailView from '../views/SectionDetailView.vue';
import EnrollModalView from '../views/EnrollModalView.vue';
import StudentCabinetView from '../views/StudentCabinetView.vue';
import TeacherCabinetView from '../views/TeacherCabinetView.vue';
import AboutView from '../views/AboutView.vue';

const routes = [
  { path: '/login', name: 'login', component: LoginView },
  { path: '/', name: 'home', component: HomeView },
  { path: '/section/:id', name: 'section-detail', component: SectionDetailView },
  { path: '/enroll/:id', name: 'enroll', component: EnrollModalView },
  { path: '/cabinet/student', name: 'student-cabinet', component: StudentCabinetView },
  { path: '/cabinet/teacher', name: 'teacher-cabinet', component: TeacherCabinetView },
  { path: '/about', name: 'about', component: AboutView }
];

export default createRouter({
  history: createWebHistory(),
  routes
});
