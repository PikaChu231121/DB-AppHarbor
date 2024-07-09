<!--<template>
    <nav class="side-navigation">
        <ul class="navigation-list">
            <li v-for="(item, index) in navigationItems" :key="index" class="navigation-item">
                <a :href="item.link" :class="['navigation-link', { 'active': item.active }]">
                    <img :src="item.icon" :alt="item.label + ' icon'" class="navigation-icon" />
                    <span class="navigation-label">{{ item.label }}</span>
                </a>
            </li>
        </ul>
    </nav>
</template>

<script>
    export default {
        name: 'SideNavigation',
        data() {
            return {
                navigationItems: [
                    { label: 'Home', icon: 'https://cdn.builder.io/api/v1/image/assets/TEMP/967c6560da4b213132ab0417a82a3f9c6c5a1ba2fa8ef36db8bad11882b6858a?apiKey=b4c87aa6fd1245589700a3931ad0dfbf&', link: '#', active: false },
                    { label: 'Dashboard', icon: 'https://cdn.builder.io/api/v1/image/assets/TEMP/5b774bca1879174ba445330d7506a61e25868f4939b355a432ec5e59311232f0?apiKey=b4c87aa6fd1245589700a3931ad0dfbf&', link: '#', active: false },
                    { label: 'Project', icon: 'https://cdn.builder.io/api/v1/image/assets/TEMP/522aff549119ae7230a5f059337b10220a2aae586a4eb85391330d09465bac13?apiKey=b4c87aa6fd1245589700a3931ad0dfbf&', link: '#', active: false },
                    { label: 'Tasks', icon: 'https://cdn.builder.io/api/v1/image/assets/TEMP/044a74e6c11214946080a89ee6b721b93df04db7b25e5573f6f3acc787656265?apiKey=b4c87aa6fd1245589700a3931ad0dfbf&', link: '#', active: false },
                    { label: 'Reporting', icon: 'https://cdn.builder.io/api/v1/image/assets/TEMP/522aff549119ae7230a5f059337b10220a2aae586a4eb85391330d09465bac13?apiKey=b4c87aa6fd1245589700a3931ad0dfbf&', link: '#', active: true },
                    { label: 'Users', icon: '', link: '#', active: false },
                    { label: 'Support', icon: 'https://cdn.builder.io/api/v1/image/assets/TEMP/911911c604361729536fae4c773145f331c7211ba809afb37e63d37aacd9747b?apiKey=b4c87aa6fd1245589700a3931ad0dfbf&', link: '#', active: false },
                    { label: 'Settings', icon: 'https://cdn.builder.io/api/v1/image/assets/TEMP/5af3553eb5ab41e3322f8d3e282426f70b5897f72f171f361c2ecc22075c8e74?apiKey=b4c87aa6fd1245589700a3931ad0dfbf&', link: '#', active: false },
                ],
            };
        },
    };
</script>

<style scoped>
    .side-navigation {
        position: fixed;
        left: 0;
        top: 0;
        height: 100%;
        max-width: 223px;
        padding-bottom: 80px;

    }

    .navigation-list {
        list-style-type: none;
        padding: 12px 0 0;
        margin: 0;
    }

    .navigation-item {
        margin-top: 8px;
    }

    .navigation-link {
        display: flex;
        align-items: center;
        padding: 8px 12px;
        border-radius: 3px;
        text-decoration: none;
        color: var(--Light-usage-text---color-text-0, #1c1f23);
        font: 600 14px/143% Inter, sans-serif;
        letter-spacing: -0.14px;
    }

        .navigation-link.active {
            background-color: var(--Light-usage-primary-light---color-primary-light-default, #eaf6ff);
        }

    .navigation-icon {
        width: 20px;
        height: 20px;
        margin-right: 8px;
    }

    .navigation-label {
        white-space: nowrap;
    }

    /* Users icon styles */
    .navigation-item:nth-child(6) .navigation-icon {
        background-color: var(--Icon-Blue, #4cc3fa);
        border-radius: 50%;
        width: 18px;
        height: 18px;
        display: flex;
        align-items: center;
        justify-content: center;
    }

        .navigation-item:nth-child(6) .navigation-icon::after {
            content: '';
            width: 8px;
            height: 8px;
            background-color: #fff;
            border-radius: 50%;
        }
</style>-->

<!--侧边栏组件实现-->
<template>
    <nav class="navigation-menu">
        <div class="menu-container">
            <header class="logo-container">
                <img src="https://cdn.builder.io/api/v1/image/assets/TEMP/2a60b1ed82f1172eb03fb24500d6a2c8738bf2a3af9420ddd9c80e22ecc80ef9?apiKey=b4c87aa6fd1245589700a3931ad0dfbf&" alt="Company Logo" class="logo" />
            </header>
            <ul class="menu-items">
                <li v-for="(item, index) in menuItems" :key="index" :class="['menu-item', { active: selectedItem === index }]" @click="selectItem(index)">
                    <img :src="item.icon" :alt="item.label + ' Icon'" class="menu-icon" />
                </li>
            </ul>
            <footer class="user-profile" :class="{ active: userProfileActive }" @click="selectUserProfile">
                <img src="https://cdn.builder.io/api/v1/image/assets/TEMP/f2f0cc5253685e466269ae8336d8d72a3d274305a41c2aa06f39552802b5c83d?apiKey=b4c87aa6fd1245589700a3931ad0dfbf&" alt="User Profile" class="profile-icon" />
            </footer>
        </div>
    </nav>
</template>

<script>
    export default {
        name: 'SideNavigation',
        data() {
            return {
                selectedItem: null,
                userProfileActive: false,
                menuItems: [
                    { label: 'Home', icon: 'https://cdn.builder.io/api/v1/image/assets/TEMP/f7f8683160f0eeac08fc6d9ea071a796fe62241473dfca92bf303b19c7ff8a61?apiKey=b4c87aa6fd1245589700a3931ad0dfbf&' },
                    { label: 'Shop', icon: 'https://cdn.builder.io/api/v1/image/assets/TEMP/a1c7d9975252d8bba0b8cf7508b2e7ed21aaafbf43caa40771cf38fccdbd7a4e?apiKey=b4c87aa6fd1245589700a3931ad0dfbf&' },
                    { label: 'Collection', icon: 'https://cdn.builder.io/api/v1/image/assets/TEMP/a44e9dec8d1bc8dbd92ad473ebbf785ae74327807e67a3c2774f3732d0a61a24?apiKey=b4c87aa6fd1245589700a3931ad0dfbf&' },
                    { label: 'Purse', icon: 'https://cdn.builder.io/api/v1/image/assets/TEMP/322a906bee0409691439aed3778cc4aa69ac9130e4eac018d076c6e27b660e92?apiKey=b4c87aa6fd1245589700a3931ad0dfbf&' },
                    { label: 'Friends', icon: 'https://cdn.builder.io/api/v1/image/assets/TEMP/9391b38f177ede077284ea2a50a643d8c9b9889dbed04a2013303fef2cd678f1?apiKey=b4c87aa6fd1245589700a3931ad0dfbf&' }
                ],
            };
        },
        methods: {
            selectItem(index) {
                this.selectedItem = index;
                this.userProfileActive = false; // Deselect user profile when a menu item is selected   
                this.$emit('update-content', this.menuItems[index].label);
            },
            selectUserProfile() {
                this.selectedItem = null; // Deselect any menu item
                this.userProfileActive = true;
                this.$emit('update-content', 'UserProfile');
            }
        }
    };
</script>

<style scoped>
    .navigation-menu {
        position: fixed;
        top: 60px; /* 紧跟在Header下面 */
        left: 0;
        height: calc(100% - 60px); /* 减去Header的高度 */
        width: 60px; /* 调整宽度 */
        background-color: #efc2bb;
        display: flex;
        align-items: center;
        justify-content: center;
    }

    .menu-container {
        width: 80%;
        height: 90%;
        border-radius: 20px;
        background-color: #fff;
        display: flex;
        flex-direction: column;
        align-items: center;
        padding: 16px 8px;
    }

    .logo-container {
        display: flex;
        justify-content: center;
        margin-bottom: 20px;
    }

    .logo {
        width: 30px;
        height: 30px;
        border-radius: 12px;
    }

    .menu-items {
        list-style-type: none;
        padding: 0;
        margin: 0;
        flex: 1;
        display: flex;
        flex-direction: column;
        align-items: center;
    }

    .menu-item {
        display: flex;
        align-items: center;
        justify-content: center;
        width: 100%;
        padding: 8px;
        margin-bottom: 8px;
        border-radius: 12px;
        cursor: pointer;
    }

        .menu-item.active {
            background-color: rgba(239, 194, 187, 0.18);
        }

    .menu-icon {
        width: 16px;
        height: 16px;
    }

    .user-profile {
        display: flex;
        justify-content: center;
        margin-top: auto;
        padding: 8px;
        border-radius: 12px;
        cursor: pointer;
    }

        .user-profile.active {
            background-color: rgba(239, 194, 187, 0.18);
        }

    .profile-icon {
        width: 16px;
        height: 16px;
    }

    @media (max-width: 991px) {
        .navigation-menu {
            width: 60px;
        }

        .logo-container,
        .menu-items {
            display: none;
        }
    }
</style>