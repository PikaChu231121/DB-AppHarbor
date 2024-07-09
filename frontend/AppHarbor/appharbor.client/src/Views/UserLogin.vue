<template>
    <div class="backwrapper">
        <div class="container" style="margin-left:350px;margin-top:190px">
            <div class="content">
                <div class="login-section">
                    <div class="login-form">
                        <h2 class="login_text" style="font-family: 'Heiti SC'; font-size: 45px ; font-weight:bolder;margin-top:5px">登 录</h2>
                        <form @submit.prevent="login">
                            <div class="form-group">
                                <label for="username" style="font-family: 'Hanyi Wenhei 85W', sans-serif; font-size: 25px;  ">用户名</label>
                                <input type="text" style="height:40px;font-size:20px"id="username" v-model="username" required />
                            </div>
                            <div class="form-group">
                                <label for="password" style="font-family: 'Hanyi Wenhei 85W', sans-serif; font-size: 25px;">密码</label>
                                <input type="password" style="height:40px"id="password" v-model="password" required /><br><br>
                            </div>
                            <div class="button-row"style="margin-top:9px">
                                <button type="button" class="login-button" style="font-size:20px;margin-left: 0px; width: 270px; font-family: 'Hanyi Wenhei 85W', sans-serif; " @click="goToLogin">登录</button>
                            </div>
                            <div class="button-row" style="font-family: 'Hanyi Wenhei 85W', sans-serif; ">
                                <button type="button" class="secondary-button" style="margin-top:6px"@click="goToRegister">注册账号</button>
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
</template>

<script>
    import axios from 'axios';
    import Cookies from 'js-cookie';

    export default {
        name: 'UserLogin',
        data() {
            return {
                username: '',
                password: '',
            };
        },
        methods: {
            login() {
                axios.post('http://localhost:5118/user/login', {
                    id: this.username,
                    password: this.password
                })
                    .then(response => {
                        Cookies.set("token", response.data);
                        console.log("successfully logged in");
                    })
                    .catch(error => {
                        this.user = null;
                        this.error = 'Login failed: ' + error.response.data;
                        console.log(this.error);
                    });
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
        margin-left:-270px;
        height: 100vh;
        width: 100vw;
        background-image: url('/public/login_back.svg');
        background-size: cover; /* Ensures the image covers the entire screen */
        background-position: center; /* Centers the image */
        background-repeat: no-repeat; /* Prevents the image from repeating */
    }
    .container {
        margin-top: 20px;
        display: flex;
        height: 60vh;
        width: 60vw;
        
        justify-content: flex-start; /* 确保容器内容靠左 */
        background-color: white; /* 背景色可以根据需要调整 */
    }

    .content {
        
        display: flex;
        height: 100%;
        width: 100%;
        max-width: 1200px; /* 可以根据需要调整最大宽度 */
        margin-left: 0; /* 确保内容从左侧开始 */
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
    }
    .login-form {
        height: 628px;
        background: white;
        padding: 5em;
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
        margin-left:0px;
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
        font-family:'Hanyi Wenhei 85W', sans-serif;
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
</style>
