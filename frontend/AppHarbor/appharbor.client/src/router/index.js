import { createRouter, createWebHistory } from 'vue-router'

const routes = [
    {
        path:"/",
        component:()=>import("@/Views/UserLogin.vue")
    },
    {
        path: "/RegisterAccount",
        component: () => import("@/Views/RegisterAccount.vue")
    },
    {
        path: "/ChangePassword",
        component: () => import("@/Views/ChangePassword.vue")
    },
    {
        path: "/PersonalInformation",
        component: () => import("@/Views/PersonalInformation.vue")
    }
]

const router = createRouter(
    {
        history: createWebHistory(),
        routes
    }
)

export default router