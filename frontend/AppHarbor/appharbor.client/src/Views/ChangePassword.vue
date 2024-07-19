<template>
    <div>
        <div class="backwrapper">
            <div class="container" style="margin-left:350px;margin-top:190px">
                <div class="content">
                    <div class="login-section">
                        <div class="login-form">
                            <h2 class="login_text" style="font-family: 'Heiti SC'; font-size: 40px; font-weight: bolder; margin-top: 20px;">修改密码</h2>
                            <form @submit.prevent="changePassword">
                                <div class="form-group">
                                    <label for="username">用户ID</label>
                                    <input type="text" id="username" v-model="username" required />
                                </div>
                                <div class="form-group">
                                    <label for="bpassword">原密码</label>
                                    <input type="password" id="bpassword" v-model="bpassword" required />
                                </div>
                                <div class="form-group">
                                    <label for="apassword">新密码</label>
                                    <input type="password" id="apassword" v-model="apassword" required />
                                </div>
                                <div class="button-row">
                                    <button type="button" class="register-button" @click="goToChange">确认修改</button>
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

    export default {
        name: 'ChangePassword',
        data() {
            return {
                username: '',
                bpassword: '',
                apassword: '',
            };
        },
        methods: {
            changePassword() {
                axios.post('http://localhost:5118/api/User/changepassword', {
                    Id: this.username,
                    OldPassword: this.bpassword,
                    NewPassword: this.apassword,
                })
                    .then(response => {
                        console.log("成功修改！");
                        alert('修改成功！');
                    })
                    .catch(error => {
                        alert('修改失败！');
                        console.log(this.bpassword);
                        console.log(error.response.data);
                    });
            },
            goToChange() {
                this.changePassword();
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
        position: absolute;
        top: 50%;
        left: 50%;
        transform: translate(-50%, -50%);
    }

    .login-form {
        background: white;
        padding: 2em;
        padding-bottom: 1em; /* Adjust this value to move the content up */
        box-shadow: 0 4px 15px rgba(0, 0, 0, 0.1);
        height: 60vh;
        width: 500px;
        display: flex;
        flex-direction: column;
        justify-content: center;
        align-items: center;
        margin-top: 0px; /* Adjust this value to move the content up */
    }

    .login-text {
        text-align: center;
        margin-bottom: 1em;
        color: #fbb1a2;
        font-size: 1.5em;
        font-weight: bold;
    }

    .form-group {
        margin-bottom: 0.75em; /* Adjust this value to bring the fields closer */
        width: 100%;
    }

    label {
        display: block;
        font-size: 20px;
        margin-bottom: 0.5em;
        color: #333;
    }

    input {
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

    .login_text {
        text-align: center;
        margin: auto;
    }

    .form-group {
        margin-bottom: 1em; /* Adjust this value to bring the fields closer */
    }

    label {
        margin-left: -70px;
        display: block;
        margin-bottom: 0.5em;
    }

    input {
        margin-left:-70px;
        width: 200%;
        height:40px;
        padding: 0.5em;
        border: 1px solid #ccc;
        border-radius: 5px;
    }

    .button-row {
        width: 100%;
        display: flex;
        justify-content: space-between;
        margin-bottom: 3em;
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
