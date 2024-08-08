<template>
    <div class="header">
        <div class="user-info">
            <img :src="adminAvatar" alt="Avatar" class="user-avatar" />
            <div class="user-details">
                <div class="user-id">管理员ID: {{ adminId }}</div>
                <div class="user-nickname">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;您好，{{ adminNickname }}管理员~</div>
            </div>
        </div>
        <div class="search-container">
            <input type="text" class="search-bar" placeholder="搜索..." />
            <div class="search-icon">🔍</div>
        </div>
    </div>
</template>

<script>
    import axios from 'axios';
    import Cookies from 'js-cookie';

    export default {
        name: "Header",
        data() {
            return {
                adminId: '',
                adminNickname: '',
                adminAvatar: '',
            };
        },
        created() {
            this.fetchAdminInfo();
        },
        methods: {
            fetchAdminInfo() {
                var token = Cookies.get('token');
                axios.post('http://localhost:5118/api/admin/adminInfo', { token: token })
                    .then(response => {
                        const data = response.data;
                        console.info(data);
                        this.adminId = data.id;
                        this.adminNickname = data.nickname;
                        this.adminAvatar = data.avatar ? `http://localhost:5118${data.avatar}` : '../../public/default.png'; // avatar 判空
                    })
                    .catch(error => {
                        console.error('Error fetching admin data:', error);
                    });
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
        margin-right: 20px; /* 头像和文字间距 */
        border: 2px solid #fff; /* 增加边框 */
    }

    .user-details {
        display: flex;
        align-items: center; /* 垂直对齐 */
    }

    .user-id {
        font-size: 20px; 
        font-weight: 600; 
        margin-right: 15px; 
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
</style>
