import { createRouter, createWebHistory } from 'vue-router'

const routes = [
    {
        path:"/",
        component: () => import("@/Views/UserLogin.vue")
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
        path: "/WorkBanchPage",
        component: () => import("@/Views/WorkBanch/MainLayout.vue")
    },
    {
        path: "/PersonalInformation",
        component: () => import("@/Views/PersonalInformation.vue")
    },
    {
        path: "/AppDetail",
        component: () => import("@/Views/Shop/AppDetail.vue")
    },
    {
        path: "/FliterSection",
        component: () => import("@/Views/Shop/FliterSection.vue")
    }
]

const router = createRouter(
    {
        history: createWebHistory(),
        routes
    }
)

export default router