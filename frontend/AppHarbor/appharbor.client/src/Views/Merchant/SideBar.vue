<template>
    <div class="sidebar">
        <div class="sidebar-header">
            <div class="icon">
                <img :src="getAvatarUrl(merchant.avatar)" class="avatar" 
                @click = "togglePopup"/>
            </div>
            <div class="merchant-info">
                <span class="nickname">{{ merchant.nickname }}</span>
            </div>
        </div>
        <div class="sidebar-menu">
            <ul>
                <li @click="selectMenuItem('outbox')">
                    <span>应用管理</span>
                </li>
                <li @click="selectMenuItem('releaseApp')">
                    <span>发布应用</span>
                </li>
                <li @click="selectMenuItem('records')">
                    <span>交易记录</span>
                </li>
                <li @click="selectMenuItem('wallet')">
                    <span>钱包管理</span>
                </li>
            </ul>
        </div>
        <transition name="popup">
            <div v-if="showMerchant" class="merchant-profile-popup" ref="popup">
                <PersonalInformation />
            </div>
        </transition>
    </div>
</template>

<script>
import axios from 'axios';
import Cookies from 'js-cookie';
import PersonalInformation from './PersonalInformation.vue';
export default {
    name: 'SideBar',
    components: {
        PersonalInformation
    },
    data() {
        return {
            merchant: {
                id: '',
                nickname: '',
                avatar: '',
            },
            showMerchant: false
        }
    },
    methods: {
        selectMenuItem(menuItem) {
            this.$emit('menuItemSelected', menuItem);
        },
        getAvatarUrl(avatarPath) {
            if (avatarPath) {
                return 'http://localhost:5118' + avatarPath;
            } else {
                return '../../public/default.png';
            }
        },
        togglePopup(event) {
            this.showMerchant = !this.showMerchant;
            event.stopPropagation();
        },
        handleClickOutside(event) {
            if (this.showMerchant && !this.$refs.popup.contains(event.target)
                && !this.$el.contains(event.target)) {
                this.showMerchant = false;
            }
        }
    },
    created() {
        const token = Cookies.get('token');
        axios.post('http://localhost:5118/api/merchant/MerchantInfo', { token })
            .then(response => {
                this.merchant.id = response.data.id;
                this.merchant.nickname = response.data.nickName;
                this.merchant.avatar = response.data.avatar;
            })
            .catch(error => {
                console.error(error);
            });
        document.addEventListener('click', this.handleClickOutside);
    },
    beforeUnmount() {
        document.removeEventListener('click', this.handleClickOutside);
    }
}  
</script>

<style scoped>
.sidebar {
    width: 240px;
    background-color: #ffffff;
    height: 90vh;
    padding: 16px;
    box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
    display: flex;
    flex-direction: column;
}

.sidebar-header {
    display: flex;
    align-items: center;
    margin-bottom: 32px;
}

.icon {
    margin-right: 16px;
}

.avatar {
    width: 48px;
    /* 增大头像大小 */
    height: 48px;
    /* 增大头像大小 */
    border-radius: 50%;
    object-fit: cover;
    border: 2px solid #007bff;
    /* 为头像添加边框 */
}

    .avatar:hover {
        transform: scale(1.1); /* 鼠标悬浮时放大头像 */
        box-shadow: 0 4px 8px rgba(0, 0, 0, 0.2); /* 鼠标悬浮时增加阴影 */
    }

    .avatar:active {
        transform: scale(0.95); /* 点击头像时缩小头像 */
    }

.merchant-info {
    display: flex;
    flex-direction: column;
    /* 垂直排列 */
}

.nickname {
    font-size: 20px;
    /* 增大字体 */
    font-weight: bold;
    /* 加粗 */
    color: #333;
    /* 深色字体 */
}

.sidebar-menu ul {
    list-style: none;
    padding: 0;
    margin: 0;
    flex-grow: 1;
}

.sidebar-menu ul li {
    padding: 12px;
    cursor: pointer;
    color: #333;
    transition: background-color 0.3s ease, color 0.3s ease;
    font-size: 16px;
    border-radius: 8px;
    background-color: #fafafa;
    margin-bottom: 10px;
    display: flex;
    align-items: center;
    box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1);
}

span {
    font-weight: bold;
    font-family: 'Poppins', sans-serif;
}

.sidebar-menu ul li:hover {
    background-color: #e6f7ff;
    color: #007bff;
}

.merchant-profile-popup {
        position: absolute;
        top: 100px; /* 根据需要调整位置*/
        left: 260px;
        background-color: #f0f9ff;
        border-radius: 12px;
        padding: 16px;
        width: 500px;
        box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
        z-index: 1000;
        opacity: 1;
        transform: translateY(0);
    }

    .popup-enter-active, .popup-leave-active {
        transition: opacity 0.3s ease, transform 0.3s ease;
    }

    .popup-enter-from, .popup-leave-to {
        opacity: 0;
        transform: translateX(-10px) translateY(-10px);
    }
</style>