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
            };
        },
        methods: {
            fetchUser() {
                var token = Cookies.get('token');
                axios.post('http://localhost:5118/api/user/userInfo', { token: token })
                    .then(response => {
                        this.user = response.data;
                        console.info(response.data);
                        this.user_id = response.data.id;
                        this.user_nickname = response.data.nickname;
                        this.avatar_url = response.data.avatar ? `http://localhost:5118${response.data.avatar}` : '../../public/default.png'; //avatar 判空

                      
                    })
                    .catch(error => {
                        console.error('Error fetching user data:', error);
                    });
            },

           
        },
        mounted() {
            this.fetchUser(); // 页面加载时从cookies获取用户ID
        },


    }
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
        filter: blur(50px)opacity(80%);
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

    
</style>
