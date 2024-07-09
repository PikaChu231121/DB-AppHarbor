<template>
    <div class="user-info-container">
        <div class="user-card">
            <img :src="user.avatar" alt="User Avatar" class="user-avatar">
            <div class="user-details">
                <h2>{{ user.name }}</h2>
                <p>ID: {{ user.id }}</p>
                <p>注册时间: {{ user.registerTime }}</p>
                <p>状态: {{ user.status }}</p>
            </div>
        </div>
    </div>
</template>

<script>
    export default {
        name: 'UserInfo',
        data() {
            return {
                user: {
                    id: '12345',
                    name: 'John Doe',
                    avatar: 'path/to/avatar.jpg',
                    registerTime: '2024-01-01',
                    status: 'Active'
                }
            };
        }
    };
</script>

<style scoped>
    .user-info-container {
        display: flex;
        justify-content: center;
        align-items: center;
        height: 100vh;
        background-color: #f0f8ff;
    }

    .user-card {
        background-color: #ffffff;
        border-radius: 10px;
        box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
        padding: 20px;
        width: 300px;
        text-align: center;
    }

    .user-avatar {
        border-radius: 50%;
        width: 100px;
        height: 100px;
        margin-bottom: 20px;
    }

    .user-details {
        color: #4a4a4a;
    }

    h2 {
        color: #ff69b4;
        margin-bottom: 10px;
    }

    p {
        margin: 5px 0;
    }
</style>
