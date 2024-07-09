import './assets/main.css'

import { createApp } from 'vue'

import App from './App.vue'

import router from './router'




createApp(App).use(router).mount('#app')

// 添加全局变量
App.config.globalProperties.$globalVar = '这是一个全局变量'