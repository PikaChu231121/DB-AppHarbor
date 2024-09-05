<template>
    <div class="header" @click="handleClickOutside">
        <div class="user-info">
            <img :src="adminAvatar" alt="Avatar" class="user-avatar" @click="togglePopup" />
            <div class="user-details">
                <svg xmlns="http://www.w3.org/2000/svg" width="2em" height="2em" viewBox="0 0 24 24"><path fill="currentColor" d="M12 14v2a6 6 0 0 0-6 6H4a8 8 0 0 1 8-8m0-1c-3.315 0-6-2.685-6-6s2.685-6 6-6s6 2.685 6 6s-2.685 6-6 6m0-2c2.21 0 4-1.79 4-4s-1.79-4-4-4s-4 1.79-4 4s1.79 4 4 4m9 6h1v5h-8v-5h1v-1a3 3 0 1 1 6 0zm-2 0v-1a1 1 0 1 0-2 0v1z" /></svg>
                &nbsp;&nbsp;&nbsp;<div class="user-id">管理员ID: {{ adminId }}</div>
                <div class="user-nickname">您好，{{ adminNickname }}管理员~</div>
            </div>
        </div>
        <transition name="popup">
            <div v-if="showAdmin" class="admin-profile-popup" ref="popup">
                <PersonalInformation />
            </div>
        </transition>
    </div>
</template>

<script>
    import axios from 'axios';
    import Cookies from 'js-cookie';
    import PersonalInformation from "./PersonalInformation.vue";

    export default {
        name: "Header",
        components: {
            PersonalInformation
        },
        data() {
            return {
                adminId: '',
                adminNickname: '',
                adminAvatar: '',
                showAdmin: false
            };
        },
        created() {
            this.fetchAdminInfo();
            document.addEventListener('click', this.handleClickOutside); // Add event listener for clicks
        },
        beforeDestroy() {
            document.removeEventListener('click', this.handleClickOutside); // Clean up the event listener
        },
        methods: {
            fetchAdminInfo() {
                const token = Cookies.get('token');
                let formData = new FormData();
                formData.append('token', token);
                axios.post(`${this.$Url}/api/admin/adminInfo`, formData)
                    .then(response => {
                        const data = response.data;
                        this.adminId = data.id;
                        this.adminNickname = data.nickname;
                        this.adminAvatar = data.avatar ? `${this.$Url}${data.avatar}` : '../../public/default.png'; // avatar 判空
                    })
                    .catch(error => {
                        console.error('Error fetching admin data:', error);
                    });
            },
            togglePopup(event) {
                this.showAdmin = !this.showAdmin;
                event.stopPropagation();
            },
            handleClickOutside(event) {
                if (this.showAdmin && !this.$refs.popup.contains(event.target) && !this.$el.contains(event.target)) {
                    this.showAdmin = false;
                }
            }
        }
    };
</script>

<style scoped>
    @import url('https://fonts.googleapis.com/css2?family=Poppins:wght@400;600&display=swap'); /* 引入Poppins字体 */

    .header {
        display: flex;
        justify-content: space-between;
        align-items: center;
        padding: 15px; /* 调整内边距 */
        background-color: #6a1b9a;
        color: white;
        flex-shrink: 0; /* 防止头部收缩 */
    }

    .user-info {
        display: flex;
        align-items: center;
    }

    .user-avatar {
        width: 50px;
        height: 50px;
        border-radius: 50%;
        margin-left: 10px;
        margin-right: 20px; /* 头像和文字间距 */
        border: 2px solid #fff; /* 增加边框 */
        transition: transform 0.3s ease, box-shadow 0.3s ease; /* 添加过渡效果 */
        cursor: pointer; /* 设置鼠标样式为点击手型 */
    }

        .user-avatar:hover {
            transform: scale(1.1); /* 鼠标悬浮时放大头像 */
            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.2); /* 鼠标悬浮时增加阴影 */
        }

        .user-avatar:active {
            transform: scale(0.95); /* 点击头像时缩小头像 */
        }

    .user-details {
        display: flex;
        align-items: center; /* 垂直对齐 */
    }

    .user-id {
        font-size: 20px;
        font-weight: 600;
        margin-right: 20px;
        font-family: 'Poppins', sans-serif;
        letter-spacing: 0.5px;
        text-shadow: 1px 1px 2px rgba(0, 0, 0, 0.2);
    }

    .user-nickname {
        font-size: 20px;
        font-weight: 600;
        color: #fff;
        font-family: 'Poppins', sans-serif;
        letter-spacing: 0.5px;
        text-shadow: 1px 1px 2px rgba(0, 0, 0, 0.2);
        padding-left: 300px;
    }

    .search-container {
        display: flex;
        align-items: center;
        position: relative;
    }

    .search-bar {
        width: 300px;
        padding: 10px;
        padding-left: 40px; /* 为图标留出空间 */
        border: none;
        border-radius: 4px;
        box-shadow: 0 0 4px rgba(0, 0, 0, 0.1);
        font-size: 16px;
        background-color: #fff;
    }

    .search-icon {
        position: absolute;
        left: 10px;
        font-size: 18px;
        color: #6a1b9a;
    }

    .admin-profile-popup {
        position: absolute;
        top: 50px; /* 根据需要调整位置 */
        left: 210px;
        background-color: rgba(101, 85, 143);
        border-radius: 12px;
        padding: 16px;
        width: 500px;
        box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
        z-index: 1000;
        opacity: 1;
        transform: translateY(0);
    }
</style>