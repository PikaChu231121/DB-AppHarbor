<template>
    <div class="Home">
        <div class="header">
            <img :src="avatar_url" class="avatar-header" />
            <div class="avatar">
                <img :src="avatar_url" class="avatar-circle" />
                <div class="user-info">
                    <p class="user-nickname">欢迎来到{{ user_nickname }}的库</p>
                    <p class="user-id">用户ID：{{ user_id }}</p>
                </div>
            </div>
        </div>
        <div class="auto-wrapper">
            <div class="info-box" v-for="app in applications" :key="app.id">
                <img :src="app.Image ? `http://localhost:5118${app.Image}` : '../../public/default-app.png'" class="app-image" />
                <div class="app-info">
                    <p class="app-name">{{ app.name }}</p>
                    <p class="app-version">版本：{{ app.version }}</p>
                    <p class="app-category">类别：{{ app.category }}</p>
                    <p class="app-description">{{ app.description }}</p>
                    <button class="purchase-button">下载</button>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
    import axios from 'axios';
    import Cookies from 'js-cookie';

    export default {
        data() {
            return {
                user_nickname: '',
                user_id: '',
                avatar_url: '',
                applications: []
            };
        },
        methods: {
            fetchUser() {
                var token = Cookies.get('token');
                axios.post('http://localhost:5118/api/user/userInfo', { token: token })
                    .then(response => {
                        const data = response.data;
                        this.user_id = data.id;
                        this.user_nickname = data.nickname;
                        this.avatar_url = data.avatar ? `http://localhost:5118${data.avatar}` : '../../public/default.png';
                        this.fetchApplications(token); // Fetch applications after fetching user data
                    })
                    .catch(error => {
                        console.error('Error fetching user data:', error);
                    });
            },
            fetchApplications() {
                var token = Cookies.get('token');
                let formData = new FormData();
                formData.append('token', token);
                axios.post('http://localhost:5118/api/Order/fetchOwnApps', formData)
                    .then(response => {
                        this.applications = response.data.$values;
                    })
                    .catch(error => {
                        console.error('Error fetching applications:', error);
                    });
            }
        },
        mounted() {
            this.fetchUser();
        }
    };
</script>

<style scoped>
    .Home {
        position: relative;
        display: flex;
        flex-direction: column;
        align-items: center;
        width: 100%;
        height: 100%;
    }

    .header {
        height: 150px;
        width: 100%;
        overflow: hidden;
        box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
        position: relative;
        border-radius: 10px;
        border: 3px solid #ffd7d2;
    }

    .avatar-header {
        width: 100%;
        height: 200px;
        object-fit: cover;
        filter: blur(50px) opacity(80%);
    }

    .avatar {
        display: flex;
        flex-direction: row;
        position: absolute;
        left: 5%;
        top: 15%;
        height: 100px;
        width: 500px;
    }

    .avatar-circle {
        position: relative;
        background-color: #ffffff;
        object-fit: cover;
        height: 100%;
        aspect-ratio: 1 / 1;
        border-radius: 50%;
        box-shadow: 0 4px 20px rgba(0, 0, 0, 0.447);
        z-index: 1;
    }

    .user-info {
        display: flex;
        flex-direction: column;
        justify-content: space-between;
        margin-bottom: 4%;
        margin-left: 5%;
    }

    .user-nickname {
        font-size: 40px;
        height: 70%;
        align-items: center;
        display: flex;
        flex-direction: row;
        text-shadow: 0 3px 15px rgb(255, 255, 255);
    }

    .user-id {
        width: 100%;
        margin-left: 5px;
        height: 70%;
        align-items: center;
        display: flex;
        flex-direction: row;
        text-shadow: 0 3px 15px rgb(255, 255, 255);
    }

    .auto-wrapper {
        display: flex;
        flex-direction: row;
        align-items: center;
        justify-content: space-between;
        width: 100%;
        height: calc(100% - 150px);
        margin: 10px 10px 3px;
    }

    .info-box {
        display: flex;
        flex-direction: column;
        align-items: center;
        background: #fff9f9;
        border: 3px solid #ffd7d2;
        padding: 2% 5% 2% 5%;
        width: calc(50% - 8px);
        height: 100%;
        border-radius: 10px;
        box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
    }

    .app-image {
        width: 100%;
        height: auto;
        object-fit: cover;
        border-radius: 50%;
        box-shadow: 0 4px 20px rgba(0, 0, 0, 0.447);
    }

    .app-info {
        display: flex;
        flex-direction: column;
        align-items: center;
        margin-top: 10px;
    }

    .app-name {
        font-size: 20px;
        font-weight: bold;
        margin-bottom: 10px;
    }

    .app-version, .app-category {
        font-size: 16px;
        margin-bottom: 10px;
    }

    .app-description {
        font-size: 16px;
        margin-bottom: 10px;
    }

    .app-price {
        font-size: 18px;
        color: #fbb1a2;
        margin-bottom: 20px;
    }

    .purchase-button {
        background-color: #fbb1a2;
        color: white;
        border: none;
        padding: 10px 20px;
        border-radius: 5px;
        cursor: pointer;
    }
</style>
