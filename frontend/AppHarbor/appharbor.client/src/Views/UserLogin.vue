<template>
    <div>
        <Loading :loading="isLoading" />
        <LoginAlert v-if="alertMessage" style="z-index: 1;" :message="alertMessage" @close="alertMessage = ''" />
        <div class="backwrapper">
            <div class="container" style="margin-left:350px;margin-top:190px">
                <div class="content">
                    <div class="login-section">
                        <div class="login-form">
                            <h2 class="login_text" style="font-family: 'Heiti SC'; font-size: 40px; font-weight: bolder; margin-top: 5px;">登 录</h2>
                            <form @submit.prevent="login">
                                <div class="form-group">
                                    <label for="id" style="font-family: 'Hanyi Wenhei 85W', sans-serif; font-size: 20px;">用户ID</label>
                                    <input type="text" style="height: 40px; font-size: 15px;" id="id" v-model="id" required />
                                </div>
                                <div class="form-group">
                                    <label for="password" style="font-family: 'Hanyi Wenhei 85W', sans-serif; font-size: 20px;">密码</label>
                                    <input type="password" style="height: 40px;" id="password" v-model="password" required /><br><br><br>
                                </div>
                                <div class="button-row" style="margin-top: 9px;">
                                    <button type="button" class="login-button" style="font-size: 18px; margin-left: 0px; margin-top: -30px; width: 270px; font-family: 'Hanyi Wenhei 85W', sans-serif;" @click="goToLogin">登录</button>
                                </div>
                                <div class="button-row" style="font-family: 'Hanyi Wenhei 85W', sans-serif; margin-bottom: 20px;">
                                    <button type="button" class="secondary-button" @click="goToRegister">注册账号</button>
                                    <button type="button" class="secondary-button" @click="goToForgotPassword">修改密码</button>
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
        name: 'UserLogin',
        components: {
            LoginAlert,
            Loading,
        },
        data() {
            return {
                id: '',
                password: '',
                alertMessage: '',
                isLoading: false
            };
        },
        methods: {
            login() {
                if (this.id[0] === 'a') {
                    let formData = new FormData();
                    formData.append('id', this.id.substring(1));  // 提取从第二个字符开始的子字符串
                    console.log(this.id.substring(1));
                    formData.append('password', this.password);
                    axios.post('http://localhost:5118/api/admin/adminlogin', formData)
                        .then(response => {
                            Cookies.set("token", response.data);
                            this.isLoading = true; // Show loading animation on login attempt
                            this.alertMessage = `您好! 尊敬的 ${this.id} 管理员, 欢迎来到 AppHarbor!`;
                            global.id = this.id;
                            setTimeout(() => {
                                this.$router.push('/');//#################################跳转到需要的地方

                            }, 3000);

                        })
                        .catch(error => {
                            this.isLoading = false;
                            this.alertMessage = '登录失败: ' + error.response.data;
                        });
                }
                else if (this.id[0] === 'm') {
                    //#################################添加商家的登入和跳转

                }
                else {
                    let formData = new FormData();
                    formData.append('id', this.id);
                    formData.append('password', this.password);
                    axios.post('http://localhost:5118/api/user/login', formData)
                        .then(response => {
                            Cookies.set("token", response.data);
                            this.isLoading = true; // Show loading animation on login attempt
                            this.alertMessage = `您好! 尊敬的 ${this.id} 用户, 欢迎来到 AppHarbor!`;
                            global.id = this.id;
                            setTimeout(() => {
                                this.$router.push('/WorkBanchPage');

                            }, 3000); // Redirect after 2 seconds

                        })
                        .catch(error => {
                            this.isLoading = false; // Hide loading animation on login failure
                            this.alertMessage = '登录失败: ' + error.response.data;
                        });
                }
            },
            goToLogin() {
                this.login();
            },
            goToRegister() {
                this.$router.push('/RegisterAccount');
            },
            goToForgotPassword() {
                this.$router.push('/ChangePassword');
            }
        }
    };
</script>

<style scoped>
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
        height: 480px;
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
        margin-bottom: 1em;
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
        margin-top: 1em;
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