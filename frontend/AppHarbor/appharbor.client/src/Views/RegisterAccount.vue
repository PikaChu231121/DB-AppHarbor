<template>
    <div>
        <Loading :loading="isLoading" />
        <LoginAlert v-if="alertMessage" style="z-index: 1;" :message="alertMessage" @close="alertMessage = ''" />
        <div class="backwrapper">
            <div class="container" style="margin-left:350px;margin-top:190px">
                <div class="content">
                    <div class="login-section">
                        <div class="login-form" style="margin-top:0px">
                            <div class="header">
                                <h2 class="login_text" style="font-family: 'Heiti SC'; font-size: 40px; font-weight: bolder; margin-top: 20px;">注册账号</h2>
                                <div class="user-type">
                                    <label for="userType" style="font-family: 'Hanyi Wenhei 85W', sans-serif; font-size: 20px;">选择用户类型</label>
                                    <select id="userType" v-model="userType" style="font-size: 15px;">
                                        <option value="普通用户">普通用户</option>
                                        <option value="商家">商家</option>
                                        <option value="管理员">管理员</option>
                                    </select>
                                </div>
                            </div>
                            <form @submit.prevent="register">
                                <div class="form-group" style="margin-top:10px">
                                    <label for="nickname">昵称</label>
                                    <input type="text" id="nickname" v-model="nickname" required />
                                </div>
                                <div class="form-group">
                                    <label for="password">密码</label>
                                    <input type="password" id="password" v-model="password" required />
                                </div>
                                <div class="button-row">
                                    <button type="button" class="register-button" @click="goToRegister">确认注册</button>
                                </div>
                            </form>
                        </div>
                    </div>
                    <div class="video-section">
                        <video autoplay loop muted playsinline>
                            <source src="@/../public/video/login_gif.mp4" type="video/mp4">
                            Your browser does not support the video tag.
                        </video>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>


<script>
    import axios from 'axios';
    import Cookies from 'js-cookie';
    import global from "../global.js";
    import LoginAlert from './LoginAlert.vue';
    import Loading from './Tools/Loading.vue';

    export default {
        name: 'RegisterAccount',

        components: {
            LoginAlert,
            Loading,
        },

        data() {
            return {
                nickname: '',
                password: '',
                userType: '普通用户', // 默认用户类型
                alertMessage: '',
                isLoading: false
            };
        },

        methods: {
            register() {
                this.isLoading = true; // Show loading animation at the start of registration

                let url = '';
                if (this.userType === '普通用户') {
                    url = 'http://localhost:5118/api/User/register';
                } else if (this.userType === '商家') {
                    url = 'http://localhost:5118/api/Merchant/register';
                } else if (this.userType === '管理员') {
                    url = 'http://localhost:5118/api/Admin/register';
                }

                axios.post(url, {
                    nickname: this.nickname,
                    password: this.password,
                })
                    .then(response => {
                        console.log("successfully registered!");
                        console.log(response.data.id);
                        this.alertMessage = `注册成功！您的用户名是 ${response.data.id} `;
                        // Display loading animation for 2 seconds and then redirect
                        setTimeout(() => {
                            this.isLoading = false; // Hide loading animation
                        }, 2000); // Delay for 2 seconds
                    })
                    .catch(error => {
                        setTimeout(() => {
                            this.isLoading = false; // Hide loading animation
                        }, 2000); // Delay for 2 seconds

                        this.alertMessage = `注册失败`;
                        console.log(this.error);
                        this.isLoading = false; // Hide loading animation if there's an error
                    });
            },
            goToRegister() {
                this.register();
            }
        }
    };
</script>


<style scoped>
    .login-container {
        display: flex;
        justify-content: center;
        align-items: center;
        height: 100vh;
        width: 100vh;
        position: absolute;
        top: 50%;
        left: 50%;
        transform: translate(-50%, -50%);
    }

    .login-form {
        background: white;
        padding: 2em;
        border-radius: 12px;
        box-shadow: 0 4px 15px rgba(0, 0, 0, 0.1);
        height: 50vh;
        width: 500px;
        display: flex;
        flex-direction: column;
        justify-content: center;
        align-items: center;
    }

    .login-text {
        text-align: center;
        margin-bottom: 1em;
        color: #fbb1a2;
        font-size: 1.5em;
        font-weight: bold;
    }

    .header {
        display: flex;
        justify-content: space-between;
        width: 100%;
    }

    .user-type {
        display: flex;
        flex-direction: column;
        align-items: flex-end;
    }

    .form-group {
        margin-bottom: 0.75em; /* Adjust this value to bring the fields closer */
        width: 100%;
    }

    label {
        display: block;
        margin-bottom: 0.5em;
        color: #333;
    }

    input, select {
        width: 100%;
        padding: 0.5em;
        border: 1px solid #ccc;
        border-radius: 5px;
        font-size: 1em;
    }

    .button-row {
        width: 100%;
        display: flex;
        justify-content: center;
        margin-top: 1em;
    }

    .register-button {
        width: 100%;
        padding: 0.75em;
        border: none;
        border-radius: 8px;
        background-color: #f3c7ba;
        color: white;
        font-size: 1.2em;
        cursor: pointer;
        transition: background-color 0.3s, transform 0.3s;
    }

        .register-button:hover {
            background-color: #fbb1a2;
        }

        .register-button:active {
            transform: scale(0.95);
        }

    html, body {
        margin: 0;
        padding: 0;
        height: 100%;
        width: 100%;
        overflow: hidden;
    }

    * {
        box-sizing: border-box;
    }

    .backwrapper {
        position: fixed;
        top: 0;
        left: 0;
        height: 100vh;
        width: 103vw;
        background-image: url('/public/login_back.svg');
        background-size: cover;
        background-position: center;
        background-repeat: no-repeat;
    }

    .container {
        margin-top: 20px;
        display: flex;
        height: 60vh;
        width: 60vw;
        justify-content: flex-start;
        background-color: white;
    }

    .content {
        display: flex;
        height: 100%;
        width: 100%;
        max-width: 1200px;
        margin-left: 0;
        position: relative; /* Ensure relative positioning for z-index */
    }

    .login-section {
        flex: 1;
        display: flex;
        justify-content: center;
        align-items: center;
    }

    .video-section {
        flex: 1;
        display: flex;
        justify-content: center;
        align-items: center;
        position: relative; /* Ensure relative positioning for z-index */
    }

    .login-form {
        height: 530px;
        background: white;
        padding: 2em;
        border-radius: 2px;
        display: flex;
        flex-direction: column;
        justify-content: center;
        align-items: center;
        font-size: large;
    }

    .login_text {
        text-align: center;
        margin: auto;
    }

    .form-group {
        margin-bottom: 2em; /* Adjust this value to bring the fields closer */
    }

    label {
        display: block;
        margin-bottom: 0.5em;
    }

    input {
        width: 100%;
        padding: 0.5em;
        border: 1px solid #ccc;
        border-radius: 5px;
    }

    .button-row {
        width: 100%;
        display: flex;
        justify-content: space-between;
        margin-bottom: 2em;
        margin-left: 0px;
    }

    .login-button {
        width: 150px;
        padding: 0.5em;
        border: none;
        border-radius: 4px;
        background-color: #F3C7BA;
        color: white;
        font-size: 0.9em;
        justify-content: space-between;
        text-align: center;
        cursor: pointer;
        font-family: 'Hanyi Wenhei 85W', sans-serif;
    }

    .secondary-button {
        width: calc(50% - 5px);
        padding: 0.5em;
        border: none;
        border-radius: 4px;
        background-color: #F3C7BA;
        color: white;
        font-size: 0.9em;
        justify-content: space-between;
        text-align: center;
        cursor: pointer;
        font-family: 'Hanyi Wenhei', sans-serif;
    }

        .login-button:hover, .secondary-button:hover {
            background-color: #fbb1a2;
        }

    video {
        width: 100%;
        height: 100%;
        object-fit: cover;
    }

    /* Ensure the modal is above other content */
    .LoginAlert {
        position: absolute;
        top: 50%;
        left: 50%;
        transform: translate(-50%, -50%);
        z-index: 0; /* Adjust as necessary */
    }
</style>
