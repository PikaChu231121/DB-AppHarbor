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
        <!-- Friends Popup -->
        <div v-if="showFriendsPopup" class="friends-popup">
            <button class="popup-item1" @click="handlePopupClick('Your friends')">
                <img src="https://cdn.builder.io/api/v1/image/assets/TEMP/f2f0cc5253685e466269ae8336d8d72a3d274305a41c2aa06f39552802b5c83d?apiKey=b4c87aa6fd1245589700a3931ad0dfbf&" alt="Your friends" class="popup-icon" />
                <span>  &nbsp;&nbsp;  Your friends</span>
            </button>
            <button class="popup-item2" @click="handlePopupClick('Add friends')">
                <img src="https://cdn.builder.io/api/v1/image/assets/TEMP/a1c7d9975252d8bba0b8cf7508b2e7ed21aaafbf43caa40771cf38fccdbd7a4e?apiKey=b4c87aa6fd1245589700a3931ad0dfbf&" alt="Add friends" class="popup-icon" />
                <span>  &nbsp;&nbsp;  Add friends</span>
            </button>
            <button class="popup-item3" @click="handlePopupClick('Buy me')">
                <img src="https://cdn.builder.io/api/v1/image/assets/TEMP/322a906bee0409691439aed3778cc4aa69ac9130e4eac018d076c6e27b660e92?apiKey=b4c87aa6fd1245589700a3931ad0dfbf&" alt="Buy me" class="popup-icon" />
                <span>   &nbsp;&nbsp;  Buy me</span>
            </button>
        </div>
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
                showFriendsPopup: false,
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
                if (this.menuItems[index].label === 'Purse' || this.menuItems[index].label === 'Home' || this.menuItems[index].label === 'Shop' || this.menuItems[index].label === 'Collection') {
                    this.isLoading = true; // 开始加载动画
                    setTimeout(() => {
                        this.isLoading = false; // 几秒后停止加载动画
                        this.selectedItem = index; // 选择Purse菜单项
                        this.$emit('update-content', this.menuItems[index].label);
                    }, 2000); // 设置加载动画持续时间为2秒
                } else if (this.menuItems[index].label === 'Friends') {
                    this.toggleFriendsPopup();
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
            },
            toggleFriendsPopup() {
                this.showFriendsPopup = !this.showFriendsPopup;
            },
            handlePopupClick(action) {
                // Handle each popup button click action here
                console.log(action);
                this.showFriendsPopup = false; // Hide the popup after a button is clicked
            }
        }
    };
</script>

<style scoped>
    @import url('https://fonts.googleapis.com/css2?family=Fredoka+One&family=Poppins:wght@400&display=swap');
    @import url('https://fonts.googleapis.com/css2?family=Baloo+2&family=Quicksand:wght@400;700&family=Fredoka+One&display=swap');

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

    /* Friends Popup */
    .friends-popup {
        position: absolute;
        top: 300px; /* 根据需要调整位置 */
        left: 90px;
        background-color: #fbeaea;
        border-radius: 12px;
        padding: 16px;
        width:220px;
        box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
        z-index: 1000;
    }

    .popup-item1 {
        display: flex;
        align-items: center;
        margin-bottom: 8px;
        padding: 8px 16px;
        font-size: medium;
        border: none;
        border-radius: 12px;
        font-family: 'Baloo 2', cursive;
        font-weight: bolder;
        color: #F8887D;
        background-color: transparent;
        cursor: pointer;
        transition: background-color 0.3s, transform 0.3s, color 0.3s;
    }
    .popup-item2 {
        display: flex;
        align-items: center;
        font-size: medium;
        margin-bottom: 8px;
        padding: 8px 16px;
        border: none;
        border-radius: 12px;
        font-family: 'Baloo 2', cursive;
        font-weight: bolder;
        color: #27CCA6;
        background-color: transparent;
        cursor: pointer;
        transition: background-color 0.3s, transform 0.3s, color 0.3s;
    }
    .popup-item3 {
        display: flex;
        align-items: center;
        margin-bottom: 8px;
        padding: 8px 16px;
        border: none;
        border-radius: 12px;
        font-size: medium;
        font-family: 'Baloo 2', cursive;
        font-weight: bolder;
        color: #B245C0;
        background-color: transparent;
        cursor: pointer;
        transition: background-color 0.3s, transform 0.3s, color 0.3s;
    }

        .popup-item:last-child {
            margin-bottom: 0;
        }

        .popup-item:hover {
            background-color: #ffe5e5;
            transform: scale(1.05);
        }

        .popup-item:active {
            background-color: #ffcccc;
            transform: scale(0.95);
        }

    .popup-icon {
        width: 20px;
        height: 20px;
        margin-right: 8px;
    }
    .popup-item1, .popup-item2, .popup-item3 {
        display: flex;
        align-items: center;
        margin-bottom: 8px;
        padding: 8px 16px;
        font-size: medium;
        border: none;
        border-radius: 12px;
        font-family: 'Baloo 2', cursive;
        font-weight: bolder;
        cursor: pointer;
        transition: background-color 0.3s, transform 0.3s, color 0.3s;
    }

    .popup-item1 {
        color: #F8887D;
    }

    .popup-item2 {
        color: #27CCA6;
    }

    .popup-item3 {
        color: #B245C0;
    }

        .popup-item1:hover, .popup-item2:hover, .popup-item3:hover {
            background-color: #ffe5e5;
            transform: scale(1.05);
        }

        .popup-item1:active, .popup-item2:active, .popup-item3:active {
            background-color: #ffcccc;
            transform: scale(0.95);
        }

    .popup-icon {
        width: 20px;
        height: 20px;
        margin-right: 8px;
    }
</style>