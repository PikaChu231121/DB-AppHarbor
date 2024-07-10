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
    //import ShopView from './ShopView.vue'; // Example component
    import Favourites from '../Favourites.vue'; // Example component
    import Wallet from '../Wallet.vue'; // Example component
    //import FriendsView from './FriendsView.vue'; // Example component
    import PersonalInformation from '../PersonalInformation.vue'; // Example component

    export default {
        components: {
            Header,
            SideNavigation,
            //HomeView,
            //ShopView,
            Favourites,
            Wallet,
            //FriendsView,
            PersonalInformation
        },
        data() {
            return {
                currentView: 'HomeView' // Default view
            };
        },
        methods: {
            updateContent(view) {
                switch (view) {
                    case 'Home':
                        this.currentView = 'HomeView';
                        break;
                    //case 'Shop':
                    //    this.currentView = 'ShopView';
                    //    break;
                    case 'Collection':
                        this.currentView = 'Favourites';
                        break;
                    case 'Purse':
                       this.currentView = 'Wallet';
                       break;
                    //case 'Friends':
                    //    this.currentView = 'FriendsView';
                    //    break;
                    case 'UserProfile':
                        this.currentView = 'PersonalInformation';
                        break;
                    default:
                        this.currentView = 'HomeView';
                }
            }
        }
    };
</script>

<style scoped>
    .main-layout {
        display: flex;
        flex-direction: column;
        width: 100%;
    }

    .layout-body {
        display: flex;
        justify-content: space-between;
        width: 100%;
        gap: 20px;
        margin-top: 80px; /* 给Header留出空间 */
    }

    .content {
        margin-left: 60px; /* 调整与侧边栏的间距 */
        padding: 20px;
        width: calc(100% - 60px);
        margin-top: 60px; /* 确保不被 header 遮挡 */
    }

    @media (max-width: 991px) {
        .layout-body {
            max-width: 100%;
            flex-wrap: wrap;
        }

        .content {
            margin-left: 0;
        }
    }
</style>

