<template>
    <div>
        <div class="login-container">
            <div class="login-form">
                <h2 class="login_text">注册账号</h2>
                <form @submit.prevent="login">
                    <div class="form-group">
                        <label for="username">用户名</label>
                        <input type="text" id="username" v-model="username" required />
                    </div>
                    <div class="form-group">
                        <label for="password">密码</label>
                        <input type="password" id="bpassword" v-model="password" required />
                    </div>
                    <div class="form-group">
                        <label for="name">姓名</label>
                        <input type="text" id="name" v-model="name" required />
                    </div>
                    <div class="button-row">
                        <button type="button" class="login-button" @click="goToRegister">确认注册</button>
                    </div>
                </form>
            </div>
        </div>
    </div>
</template>


<script>
    import axios from 'axios';

    export default {
        name: 'ChangePassword',
        data() {
            return {
                username: '',
                password: '',
                realname: '',
            };
        },
        methods: {
            Register() {
                axios.post('http://localhost:5118/user/Register', {
                    id: this.username,
                    password: this.password,
                })
                    .then(response => {
                        console.log("successfully Register!");
                    })
                    .catch(error => {
                        this.user = null;
                        this.error = 'Login failed: ' + error.response.data + '用户名已存在';
                        console.log(this.error);
                    });
            },
            goToRegister() {
                // 点击登录按钮，提交表单,跳转主页
                this.Register();

            },
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
        background-color: #ffffff;
        position: absolute;
        top: 50%;
        left: 50%;
        transform: translate(-50%, -50%);
    }

    .login-form {
        background: white;
        padding: 2em;
        border-radius: 8px;
        box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
        height: 50vh;
        width: 500px;
        display: flex;
        flex-direction: column;
        justify-content: center;
        align-items: center;
    }

    .login_text {
        text-align: center;
        margin-bottom: 1em;
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
    }

    .login-button {
        width: 100%;
        padding: 0.5em;
        border: none;
        border-radius: 4px;
        background-color: #42b983;
        color: white;
        font-size: 1em;
        cursor: pointer;
    }

    .secondary-button {
        width: calc(50% - 5px);
        padding: 0.5em;
        border: none;
        border-radius: 4px;
        background-color: #42b983;
        color: white;
        font-size: 1em;
        cursor: pointer;
    }

        .login-button:hover,
        .secondary-button:hover {
            background-color: #349a72;
        }
</style>
