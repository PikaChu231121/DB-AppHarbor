<template>
    <div class="profile-settings">
        <h1>个人信息</h1>
        <div class="user-info">
            <div class="form-group">
                <label>头像:</label>
                <div class="avatar-edit">
                    <img :src="user.avatar" alt="用户头像" class="avatar" />
                    <button @click="triggerFileInput">修改</button>
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
                           :disabled="!isEditing"
                           class="nickname-input" />
                    <button @click="toggleEdit">{{ isEditing ? '保存' : '修改' }}</button>
                </div>
            </div>
            <div class="form-group">
                <label>注册时间:</label>
                <p>{{ user.registerTime }}</p>
            </div>
            <div class="form-group">
                <label>用户状态:</label>
                <p>{{ user.state }}</p>
            </div>
        </div>
    </div>
</template>

<script>
    import axios from 'axios';
    import global from "../global.js"

    export default {
        name: 'ProfileSettings',
        data() {
            return {
                isEditing: false,
                user: {
                    id:'',
                },
            };
        },
        mounted() {
             // 读取 localStorage 中的 id
            const storedId = localStorage.getItem('globalId');
            if (storedId) {
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
                axios.post('http://localhost:5118/user/userInfo', {id: this.user.id})
                    .then(response => {
                        this.user = response.data;
                        this.translateUserState();
                    })
                    .catch(error => {
                        console.error('Error fetching user data:', error);
                    });
            },
            toggleEdit() {
                //console.log(this.user.nickname);
                if (this.isEditing) {
                    // 这里可以添加保存更改的逻辑
                    console.log('更改已保存:', this.user.nickname);
                }
                this.isEditing = !this.isEditing;
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
                    };
                    reader.readAsDataURL(file);
                }
            },
            translateUserState() {
                console.log('yes');
               if (this.user.state == 'Normal') {
                   this.user.state = '正常';
               } else if (this.user.state == 'active') {
                   this.user.state = '活跃';
               } else if (this.user.state == 'inactive') {
                   this.user.state = '不活跃';
               }
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
        font-size:20px;
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

    button:hover {
        background-color: #0056b3;
    }
</style>
