import './assets/main.css'

import { createApp } from 'vue'

import App from './App.vue'

import router from './router'

const app = createApp(App);

// 添加全局变量
app.config.globalProperties.$globalVar = '0';

app.use(router).mount('#app');