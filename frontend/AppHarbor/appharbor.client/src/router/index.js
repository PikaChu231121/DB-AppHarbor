import { createRouter, createWebHistory } from 'vue-router'

const routes = [
    {
        path:"/",
        component:()=>import("@/Views/PersonalInformation.vue")
    },
    {
        path: "/RegisterAccount",
        component: () => import("@/Views/RegisterAccount.vue")
    },
    {
        path: "/ChangePassword",
        component: () => import("@/Views/ChangePassword.vue")
    }
]

const router = createRouter(
    {
        history: createWebHistory(),
        routes
    }
)

export default router