<template>
    <div class="profile-settings">
        <h1>个人信息</h1>
        <div class="user-info">
            <div class="form-group">
                <div class="avatar-edit">
                    <img :src="user.avatar" alt="用户头像" class="avatar" />
                    <div class="edit-icon">
                        <img src="../../public/editing.png" @click="triggerFileInput" />
                    </div>
                    <input type="file" ref="fileInput" @change="onFileChange" class="file-input" />
                </div>
            </div>
            <div class="form-group">
                <label>ID:</label>
                <p>{{ user.id }}</p>
            </div>
            <div class="form-group">
                <label>昵称:</label>
                <div class="nickname-edit">
                    <input type="text"
                           v-model="user.nickname"
                           @input="enableSaveButton"
                           class="nickname-input" />
                </div>
            </div>
            <div class="form-group">
                <label>注册时间:</label>
                <p>{{ user.registerTime }}</p>
            </div>
            <button :disabled="!isSaveEnabled" @click="save">保存修改</button>
        </div>
    </div>
</template>

<script>
    import axios from 'axios';
    import global from "../global.js"
    import Cookies from 'js-cookie';

    export default {
        name: 'ProfileSettings',
        data() {
            return {
                user: {
                    id: '',
                    avatar: '',
                    nickname: '',
                    registerTime: ''
                },
                isSaveEnabled: false
            }
        },
        mounted() {
            // 读取 localStorage 中的 id
            const storedId = localStorage.getItem('globalId');
            this.isEditing = false;
            if (global.id == '') {
                this.user.id = storedId;
                global.id = storedId; // 更新 global.js 中的 id
            } else {
                this.user.id = global.id;
                localStorage.setItem('globalId', global.id); // 将 global.id 保存到 localStorage
            }
            this.fetchUserInfo();
        },
        methods: {
            fetchUserInfo() {
                var token = Cookies.get('token');
                axios.post('http://localhost:5118/api/user/userInfo', { token: token })
                    .then(response => {
                        this.user = response.data;
                    })
                    .catch(error => {
                        console.error('Error fetching user data:', error);
                    });
            },
            enableSaveButton() {
                this.isSaveEnabled = true;
            },
            triggerFileInput() {
                this.$refs.fileInput.click();
            },
            onFileChange(event) {
                const file = event.target.files[0];
                if (file) {
                    const reader = new FileReader();
                    reader.onload = (e) => {
                        this.user.avatar = e.target.result;
                        this.enableSaveButton();
                    };
                    reader.readAsDataURL(file);
                }
            },
            save() {
                var token = Cookies.get('token');
                axios.post('http://localhost:5118/api/user/updateUserInfo', {
                    token: token,
                    avatar: this.user.avatar,
                    nickname: this.user.nickname
                })
                    .then(response => {
                        console.log('User info updated successfully');
                        this.isSaveEnabled = false;
                    })
                    .catch(error => {
                        console.error('Error updating user info:', error);
                    });
            }
        }
    };
</script>

<style scoped>
    .profile-settings {
        max-width: 600px;
        margin: 0 auto;
        padding: 40px;
        background-color: #fff;
        border-radius: 8px;
        box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
    }

    h1 {
        margin-bottom: 20px;
        font-size: 32px;
        color: #333;
        text-align: center;
    }

    .user-info {
        display: flex;
        flex-direction: column;
        gap: 20px;
    }

    .form-group {
        display: flex;
        flex-direction: column;
    }

    label {
        margin-bottom: 5px;
        font-weight: bold;
        font-size: 20px;
        color: #333;
    }

    input[type="text"], p {
        padding: 10px;
        border: 1px solid #ccc;
        border-radius: 4px;
        font-size: 16px;
    }

    .nickname-edit {
        display: flex;
        gap: 10px;
    }

    .nickname-input:disabled {
        background-color: #f9f9f9;
    }

    .avatar-edit {
        display: flex;
        align-items: center;
        gap: 10px;
    }

    .avatar {
        width: 80px;
        height: 80px;
        border-radius: 50%;
        object-fit: cover;
    }

    .file-input {
        display: none;
    }

    button {
        padding: 10px 20px;
        background-color: #007bff;
        color: #fff;
        border: none;
        border-radius: 4px;
        cursor: pointer;
    }

        button:disabled {
            background-color: #ccc;
            cursor: not-allowed;
        }

        button:hover:enabled {
            background-color: #0056b3;
        }

    .edit-icon img {
        width: 24px; /* 调整图标大小 */
        height: 24px; /* 调整图标大小 */
    }
</style>
