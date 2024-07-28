<template>
    <div class="Home">
        <div class="header">
            <div class="title">库</div>
            <div class="user-section">
                <div class="avatar-wrapper">
                    <img :src="avatar_url" class="avatar-circle" />
                </div>
                <div class="user-info">
                    <p class="user-nickname">{{ user_nickname }}</p>
                    <p class="user-id">用户ID：{{ user_id }}</p>
                </div>
            </div>
        </div>
        <div class="auto-wrapper">
            <div class="info-box" v-for="app in applications" :key="app.id" @mouseover="hoverApp(app)" @mouseleave="leaveApp">
                <img :src="app.Image ? `http://localhost:5118${app.Image}` : '../../public/default-app.png'" class="app-image" />
                <p class="app-name">{{ app.name }}</p>
                <button class="purchase-button" @click="downloadApp(app.package)">下载</button>
                <div class="app-detail" v-if="hoveredApp === app">
                    <p>应用ID：{{ app.id }}</p>
                    <p>版本：{{ app.version }}</p>
                    <p>发行商：{{ app.publisher }}</p>
                    <p>介绍：{{ app.description }}</p>
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
                applications: [],
                hoveredApp: null
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
            },
            downloadApp(packageUrl) {
                if (packageUrl) {
                    window.open(packageUrl, '_blank');
                } else {
                    console.error('Package URL is missing');
                }
            },
            hoverApp(app) {
                this.hoveredApp = app;
            },
            leaveApp() {
                this.hoveredApp = null;
            }
        },
        mounted() {
            this.fetchUser();
        }
    };
</script>



<style scoped>
    .Home {
        display: flex;
        flex-direction: column;
        align-items: center;
        width: 100%;
        height: 100%;
    }

    .header {
        display: flex;
        justify-content: space-between;
        align-items: center;
        width: 100%;
        padding: 20px;
        background-color: #ffd7d2;
        box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
        border-radius: 10px;
    }

    .title {
        font-size: 24px;
        color: #000;
    }

    .user-section {
        display: flex;
        align-items: center;
    }

    .avatar-wrapper {
        width: 50px;
        height: 50px;
        border-radius: 50%;
        overflow: hidden;
        margin-right: 10px;
    }

    .avatar-circle {
        width: 100%;
        height: 100%;
        object-fit: cover;
    }

    .user-info {
        display: flex;
        flex-direction: column;
    }

    .user-nickname {
        font-size: 16px;
        font-weight: bold;
    }

    .user-id {
        font-size: 14px;
        color: #888;
    }

    .auto-wrapper {
        display: flex;
        flex-wrap: wrap;
        justify-content: center;
        width: 100%;
        height: calc(100% - 150px);
        overflow-y: auto;
        padding: 10px;
    }

    .info-box {
        display: flex;
        flex-direction: column;
        align-items: center;
        background: #fff9f9;
        border: 3px solid #ffd7d2;
        padding: 20px;
        margin: 10px;
        width: 200px;
        border-radius: 10px;
        box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
        transition: transform 0.3s ease, box-shadow 0.3s ease;
        position: relative;
    }

        .info-box:hover {
            transform: translateY(-10px);
            box-shadow: 0 8px 16px rgba(0, 0, 0, 0.2);
        }

    .app-image {
        width: 100px;
        height: 100px;
        object-fit: cover;
        border-radius: 10px;
    }

    .app-name {
        font-size: 18px;
        font-weight: bold;
        margin: 10px 0;
    }

    .purchase-button {
        background-color: #fbb1a2;
        color: white;
        border: none;
        padding: 10px 20px;
        border-radius: 5px;
        cursor: pointer;
    }

    .app-detail {
        position: absolute;
        bottom: -150px;
        left: 50%;
        transform: translateX(-50%);
        width: 100%;
        background: #fff;
        border: 1px solid #ffd7d2;
        border-radius: 10px;
        box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
        padding: 10px;
        opacity: 0;
        transition: opacity 0.3s ease;
    }

    .info-box:hover .app-detail {
        opacity: 1;
    }

    .app-detail p {
        margin: 5px 0;
        font-size: 14px;
    }
</style>
