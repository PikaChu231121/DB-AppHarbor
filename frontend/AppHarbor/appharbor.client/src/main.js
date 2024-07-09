import './assets/main.css'

import { createApp } from 'vue'

import App from './App.vue'

import router from './router'

const app = createApp(App);

// ���ȫ�ֱ���
app.config.globalProperties.$globalVar = '0';

app.use(router).mount('#app');