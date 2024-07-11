<!--主界面父组件，包含Header、SideNavigation等子组件-->
<template>
    <div class="main-layout">
        <Header />
        <div class="layout-body">
            <SideNavigation @update-content="updateContent" />
            <div class="content">
                <!--主页面内容-->
                <component :is="currentView" />
            </div>
        </div>
    </div>
</template>

<script>
    import Header from './Header.vue'; // 标题栏
    import SideNavigation from './SideNavigation.vue'; // 侧边栏
    //import HomeView from './HomeView.vue'; // Example component
    import Shop from '../Shop/Shop.vue'; // Example component
    import Favourites from '../Favourites.vue'; // Example component
    import Wallet from '../Wallet.vue'; // Example component
    //import FriendsView from './FriendsView.vue'; // Example component
    import PersonalInformation from '../PersonalInformation.vue'; // Example component

    export default {
        components: {
            Header,
            SideNavigation,
            //HomeView,
            Shop,
            Favourites,
            Wallet,
            //FriendsView,
            PersonalInformation
        },
        data() {
            return {
                currentView: 'Shop' // Default view
            };
        },
        methods: {
            updateContent(view) {
                switch (view) {
                    case 'Home':
                        this.currentView = 'HomeView';
                        break;
                    case 'Shop':
                        this.currentView = 'Shop';
                        break;
                    case 'Collection':
                        this.currentView = 'Favourites';
                        break;
                    case 'Purse':
                       this.currentView = 'Wallet';
                       break;
                    case 'Friends':
                        /*this.currentView = 'FriendsView';*/
                        break;
                    case 'UserProfile':
                        this.currentView = 'PersonalInformation';
                        break;
                    default:
                        this.currentView = 'Shop';
                }
            }
        }
    };
</script>

<style scoped>
    .main-layout {
        position: fixed;
        top: 0;
        left: 0;
        display: flex;
        flex-direction: column;
        width: 100%;
        height: 100%;

        background-color: #efc2bb;
        background-size: cover;
    }

    .layout-body {
        display: flex;
        /*justify-content: space-between;*/
        justify-content: flex-start;
        width: 100%;
        height: 100%;
        gap: 20px;
        margin-top: 60px; /* 给Header留出空间 */

        padding-left: 60px; /* 确保内容不被侧边栏遮挡 */
        overflow: hidden; /* 隐藏溢出的内容 */
    }

    .content {
        margin-left: 20px; /* 调整与侧边栏的间距 */
        padding: 10px;
        /*width: calc(100% - 60px);*/
        margin-top: 5px; /* 确保不被 header 遮挡 */
        margin-bottom: 40px;
        margin-right: 20px;
        background-color: white; /* content部分背景为白色 */
        flex-grow: 1; /* 使content部分占据页面的大部分 */
        border-radius: 12px; /* Optional: 给content部分添加圆角 */
        /*box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);*/ /* Optional: 添加阴影效果 */
        overflow: auto; /* 当内容超出时，添加滚动条 */
    }


</style>

