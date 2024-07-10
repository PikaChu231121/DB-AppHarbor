<!--侧边栏组件实现-->
<template>
    <div>
        <Loading :loading="isLoading" />
        <nav class="navigation-menu">
            <div class="menu-container">
                <header class="logo-container">
                    <img src="@/assets/A.png" alt="Company Logo" class="logo" />
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
    </div>
</template>

<script>
    import Loading from "../Tools/Loading.vue"
    export default {
        name: 'SideNavigation',
        components: {
            Loading,
        },
        data() {
            return {
                selectedItem: null,
                userProfileActive: false,
                isLoading: false,
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
                if (this.menuItems[index].label === 'Purse') {
                    this.isLoading = true; // 开始加载动画
                    setTimeout(() => {
                        this.isLoading = false; // 几秒后停止加载动画
                        this.selectedItem = index; // 选择Purse菜单项
                        this.$emit('update-content', this.menuItems[index].label);
                    }, 12000); // 设置加载动画持续时间为3秒
                } else {
                    this.selectedItem = index;
                    this.userProfileActive = false; // Deselect user profile when a menu item is selected
                    this.$emit('update-content', this.menuItems[index].label);
                }
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
        top: 30px; /* 紧跟在Header下面 */
        left: 10px;
        height: calc(100% - 60px - 10px); /* 减去Header的高度 */
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
        margin-bottom: 40px;
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
        transition: background-color 0.5s ease;
        transition: transform 0.6s ease, color 0.6s ease;
    }

        .menu-item.active {
            background-color: #FFEDEB;
        }

        .menu-item:hover {
            background-color: #F4DDDA;
            transform: scale(1.2); /* 鼠标悬停时的缩放效果 */
        }

    .menu-icon {
        width: 16px;
        height: 16px;
        color: #77797B;
        transition: color 0.6s ease; /* 颜色过渡效果 */
    }

    .user-profile {
        display: flex;
        justify-content: center;
        margin-top: 2px;
        padding: 8px;
        border-radius: 12px;
        cursor: pointer;
        transition: background-color 0.5s ease;
        transition: transform 0.6s ease, color 0.6s ease;
    }

        .user-profile.active {
            background-color: rgba(239, 194, 187, 0.18);
        }

        .user-profile:hover {
            background-color: #FCF4F3; /* 设置悬停时的背景颜色 */
            border-radius: 25px;
            transform: scale(1.2); /* 鼠标悬停时的缩放效果 */
        }

    .profile-icon {
        margin-top: 2px;
        width: 15px;
        height: 15px;
    }



</style>