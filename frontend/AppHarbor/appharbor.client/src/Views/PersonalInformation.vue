<template>
    <div class="profile-settings">
        <h1>用户信息</h1>
        <div class="user-info" v-if="!isEditing">
            <p><strong>ID:</strong> {{ user.id }}</p>
            <p><strong>昵称:</strong> {{ user.nickname }}</p>
            <p><strong>注册时间:</strong> {{ user.registerTime }}</p>
            <p><strong>用户状态:</strong> {{ user.status }}</p>
            <button @click="toggleEdit">编辑</button>
        </div>
        <form v-else @submit.prevent="saveChanges">
            <div class="form-group">
                <label for="nickname">昵称</label>
                <input type="text" id="nickname" v-model="user.nickname">
            </div>
            <div class="form-group">
                <label for="registerTime">注册时间</label>
                <input type="text" id="registerTime" v-model="user.registerTime" disabled>
            </div>
            <div class="form-group">
                <label for="status">用户状态</label>
                <input type="text" id="status" v-model="user.status">
            </div>
            <button type="submit">保存更改</button>
            <button type="button" @click="toggleEdit">取消</button>
        </form>
    </div>
</template>

<script>
    export default {
        name: 'ProfileSettings',
        data() {
            return {
                isEditing: false,
                user: {
                    id: '12345',
                    nickname: '丁真',
                    registerTime: '2024-01-01',
                    status: '活跃'
                }
            };
        },
        methods: {
            toggleEdit() {
                this.isEditing = !this.isEditing;
            },
            saveChanges() {
                // 保存更改逻辑在这里
                console.log('更改已保存:', this.user);
                this.toggleEdit();
            }
        }
    };
</script>

<style scoped>
    .profile-settings {
        max-width: 800px;
        margin: 0 auto;
        padding: 40px;
        background-color: #fff;
        border-radius: 8px;
        box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
        height: 80vh;
        display: flex;
        flex-direction: column;
        justify-content: center;
    }

    h1 {
        margin-bottom: 20px;
        font-size: 32px;
        color: #333;
        text-align: center;
    }

    .user-info p {
        margin: 20px 0;
        font-size: 18px;
    }

    .form-group {
        margin-bottom: 20px;
    }

    label {
        display: block;
        margin-bottom: 10px;
        font-weight: bold;
        color: #333;
    }

    input[type="text"] {
        width: 100%;
        padding: 10px;
        border: 1px solid #ccc;
        border-radius: 4px;
        font-size: 16px;
    }

    button {
        padding: 10px 20px;
        background-color: #007bff;
        color: #fff;
        border: none;
        border-radius: 4px;
        cursor: pointer;
        margin-right: 10px;
    }

        button:hover {
            background-color: #0056b3;
        }

        button[type="button"] {
            background-color: #6c757d;
        }

            button[type="button"]:hover {
                background-color: #5a6268;
            }
</style>
