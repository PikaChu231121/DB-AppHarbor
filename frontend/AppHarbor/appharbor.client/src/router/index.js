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
    //{
    //    path: "/Home",
    //    component: () => import("@/Views/Home.vue")
    //},
    {
        path: "/Shop",
        component: () => import("@/Views/Shop/Shop.vue")
    },
    {
        path: "/Favourites",
        component: () => import("@/Views/Favourites.vue")
    },
    {
        path: "/Wallet",
        component: () => import("@/Views/Wallet.vue")
    },
    //{
    //    path: "/Friends",
    //    component: () => import("@/Views/Friends.vue")
    //},
    {
        path: "/PersonalInformation",
        component: () => import("@/Views/PersonalInformation.vue")
    },
    {
        path: '/app/:id',
        name: 'AppDetail',
        component: () => import("@/Views/Shop/AppDetail.vue")
        //path: "/AppDetail",
        //component: () => import("@/Views/Shop/AppDetail.vue")
    }
    //{
    //    path: "/FliterSection",
    //    component: () => import("@/Views/Shop/FliterSection.vue")
    //}
]

const router = createRouter(
    {
        history: createWebHistory(),
        routes
    }
)

export default router