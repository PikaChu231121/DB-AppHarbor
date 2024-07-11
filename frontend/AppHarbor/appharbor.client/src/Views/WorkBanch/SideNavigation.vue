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
                        <img :src="selectedItem === index ? item.activeIcon : item.icon" :alt="item.label + ' Icon'" class="menu-icon" />
                    </li>
                </ul>
                <footer class="user-profile" :class="{ active: userProfileActive }" @click="selectUserProfile">
                    <img src="https://cdn.builder.io/api/v1/image/assets/TEMP/f2f0cc5253685e466269ae8336d8d72a3d274305a41c2aa06f39552802b5c83d?apiKey=b4c87aa6fd1245589700a3931ad0dfbf&" alt="User Profile" class="profile-icon" />
                </footer>
            </div>
        </nav>
        <!-- Friends Popup -->
        <transition name="popup">
            <div v-if="showFriendsPopup" class="friends-popup" ref="friendsPopup">
                <button class="popup-item1" @click="handlePopupClick('Your friends')">
                    <img src="../../assets/yourfriends.svg" alt="Your friends" class="popup-icon" />
                    <span>&nbsp;&nbsp;Your friends</span>
                </button>
                <button class="popup-item2" @click="handlePopupClick('Add friends')">
                    <img src="../../assets/addfriends.svg" alt="Add friends" class="popup-icon" />
                    <span>&nbsp;&nbsp;Add friends</span>
                </button>
                <button class="popup-item3" @click="handlePopupClick('Buy me')">
                    <img src="../../assets/buyme.svg" alt="Buy me" class="popup-icon" />
                    <span>&nbsp;&nbsp;Buy me</span>
                </button>
            </div>
        </transition>
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
                pastSelectedItem: null,
                selectedItem: null,
                userProfileActive: false,
                isLoading: false,
                showFriendsPopup: false,
                menuItems: [
                    { label: 'Home', icon: '../src/assets/home.svg', activeIcon: '../src/assets/homeActive.svg' },
                    { label: 'Shop', icon: '../src/assets/shop.svg', activeIcon: '../src/assets/shopActive.svg' },
                    { label: 'Collection', icon: '../src/assets/collection.svg', activeIcon: '../src/assets/collectionActive.svg' },
                    { label: 'Purse', icon: '../src/assets/purse.svg', activeIcon: '../src/assets/purseActive.svg' },
                    { label: 'Friends', icon: '../src/assets/friends.svg', activeIcon: '../src/assets/friendsActive.svg' }
                ],
            };
        },
        methods: {
            selectItem(index) {
                if (['Purse', 'Home', 'Shop', 'Collection'].includes(this.menuItems[index].label)) {
                    this.isLoading = true;
                    setTimeout(() => {
                        this.isLoading = false;
                        //this.pastSelectedItem = this.selectedItem;
                        this.selectedItem = index;
                        this.showFriendsPopup = false;
                        this.$emit('update-content', this.menuItems[index].label);
                    }, 2000);
                } else if (this.menuItems[index].label === 'Friends') {
                    if (['Purse', 'Home', 'Shop', 'Collection'].includes(this.menuItems[this.selectedItem].label)) {
                        this.pastSelectedItem = this.selectedItem;
                    }
                    this.selectedItem = index;
                    /*this.$emit('update-content', this.menuItems[index].label);*/
                    this.toggleFriendsPopup();
                } else {
                    this.selectedItem = index;
                    this.userProfileActive = false;
                    this.$emit('update-content', this.menuItems[index].label);
                }
            },
            selectUserProfile() {
                this.selectedItem = null;
                this.userProfileActive = true;
                this.$emit('update-content', 'UserProfile');
            },
            toggleFriendsPopup() {
                this.showFriendsPopup = !this.showFriendsPopup;
                if (!this.showFriendsPopup) {
                    this.selectedItem = null;
                    this.userProfileActive = false;
                    if (['Purse', 'Home', 'Shop', 'Collection'].includes(this.menuItems[this.pastSelectedItem].label)) {
                        this.selectedItem = this.pastSelectedItem;
                    }
                }
            },
            handlePopupClick(action) {
                console.log(action);
                this.showFriendsPopup = false;
                this.$emit('update-content', action);
            },
            handleClickOutside(event) {
                if (this.showFriendsPopup && !this.$refs.friendsPopup.contains(event.target) && !this.$el.contains(event.target)) {
                    this.showFriendsPopup = false;
                    this.selectedItem = null;
                    if (['Purse', 'Home', 'Shop', 'Collection'].includes(this.menuItems[this.pastSelectedItem].label)) {
                        this.selectedItem = this.pastSelectedItem;
                    }
                }
            }
        },
        mounted() {
            document.addEventListener('click', this.handleClickOutside);
        },
        beforeDestroy() {
            document.removeEventListener('click', this.handleClickOutside);
        }
    };
</script>

<style scoped>
    @import url('https://fonts.googleapis.com/css2?family=Fredoka+One&family=Poppins:wght@400&display=swap');
    @import url('https://fonts.googleapis.com/css2?family=Baloo+2&family=Quicksand:wght@400;700&family=Fredoka+One&display=swap');

    .navigation-menu {
        position: fixed;
        top: 30px;
        left: 10px;
        height: calc(100% - 60px - 10px);
        width: 60px;
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
            transform: scale(1.2);
        }

    .menu-icon {
        width: 16px;
        height: 16px;
        transition: color 0.6s ease;
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
            background-color: #FCF4F3;
            border-radius: 25px;
            transform: scale(1.2);
        }

    .profile-icon {
        margin-top: 2px;
        width: 15px;
        height: 15px;
    }

    .friends-popup {
        position: absolute;
        top: 300px;
        left: 90px;
        background-color: #fbeaea;
        border-radius: 12px;
        padding: 16px;
        width: 200px;
        box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
        z-index: 1000;
        opacity: 1;
        transform: translateY(0);
    }

        .friends-popup button {
            border: 3px solid #FADAD6;
            width: 170px;
            border-radius: 10px;
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
