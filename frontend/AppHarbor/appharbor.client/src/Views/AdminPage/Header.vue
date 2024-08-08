<template>
    <div class="header">
        <div class="user-info">
            <img :src="adminAvatar" alt="Avatar" class="user-avatar" />
            <div class="user-details">
                <div class="user-id">管理员ID: {{ adminId }}</div>
                <div class="user-nickname">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;您好，{{ adminNickname }}管理员~</div>
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
    .header {
        display: flex;
        justify-content: space-between;
        align-items: center; /* 垂直对齐 */
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
        width: 50px; /* 增加头像宽度 */
        height: 50px; /* 增加头像高度 */
        border-radius: 50%; /* 圆形头像 */
        margin-right: 15px; /* 头像和文字间距 */
        border: 2px solid #fff; /* 增加边框 */
    }

    .user-details {
        display: flex;
        align-items: center; /* 垂直对齐 */
    }

    .user-id {
        font-size: 20px; /* 增加ID字体大小 */
        font-weight: bold; /* 加粗字体 */
        margin-right: 15px; /* 增加ID和昵称间距 */
    }

    .user-nickname {
        font-size: 20px; /* 增加昵称字体大小 */
        font-weight: 600; /* 使昵称字体稍微加粗 */
        color: #fff; /* 白色字体 */
    }


    .search-container {
        display: flex;
        align-items: center; /* 垂直对齐 */
        position: relative; /* 相对定位以便图标绝对定位 */
    }

    .search-bar {
        width: 300px; /* 增加宽度 */
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
        left: 10px; /* 调整位置 */
        font-size: 18px; /* 调整图标大小 */
        color: #6a1b9a;
    }
</style>
