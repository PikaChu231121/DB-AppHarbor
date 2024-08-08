<template>
    <div class="sidebar">
        <div class="sidebar-header">
            <div class="icon">
                <img :src="getAvatarUrl(merchant.avatar)" class="avatar" />
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
                <li @click="selectMenuItem('trash')">
                    <span>Test</span>
                </li>
                <li @click="selectMenuItem('records')">
                    <span>交易记录</span>
                </li>
                <li @click="selectMenuItem('wallet')">
                    <span>钱包管理</span>
                </li>
            </ul>
        </div>
    </div>
</template>

<script>
import axios from 'axios';
import Cookies from 'js-cookie';
export default {
    name: 'SideBar',
    data() {
        return {
            merchant: {
                id: '',
                nickname: '',
                avatar: '',
            }
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
    }
}  
</script>

<style scoped>
.sidebar {
    width: 240px;
    background-color: #ffffff;
    height: 95vh;
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
</style>