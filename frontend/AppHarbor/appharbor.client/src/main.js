import './assets/main.css'

import './assets/light.css'

import { createApp } from 'vue'

import App from './App.vue'

import router from './router'

const app = createApp(App);


// 定义全局常量
app.config.globalProperties.$Url = 'http://localhost:5118';
//app.config.globalProperties.$Url = 'http://103.40.13.87:31113';


app.use(router).mount('#app');
