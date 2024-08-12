<template>
    <div class="announcement-form">
        <h2>公告</h2>

        <form @submit.prevent="submitAnnouncement">
            <div class="form-group">
                <label for="title">标题</label>
                <input type="text"
                       id="title"
                       v-model="announcementTitle"
                       placeholder="请输入公告标题"
                       required />
            </div>

            <div class="form-group">
                <label for="content">内容</label>
                <textarea id="content"
                          v-model="announcementContent"
                          placeholder="请输入公告内容"
                          required></textarea>
            </div>

            <button type="submit">发布</button>
        </form>
    </div>
</template>

<script>
    import axios from 'axios';
    import Cookies from 'js-cookie';

    export default {
        data() {
            return {
                announcementTitle: '',
                announcementContent: '',
            };
        },
        methods: {
            submitAnnouncement() {
                let formData = new FormData();
                var token = Cookies.get('token');
                formData.append('token', token);  // 提取从第二个字符开始的子字符串
                formData.append('title', this.announcementTitle);
                formData.append('content', this.announcementContent);
                axios.post('http://localhost:5118/api/announcement/publishannouncement', formData)
                    .then(response => {
                        /*发布成功的提示*/
                    })
                    .catch(error => {
                        /*发布失败的提示*/
                    });
            },
        },
    };
</script>

<style scoped>
    .announcement-form {
        max-width: 600px;
        margin: 0 auto;
    }

    .form-group {
        margin-bottom: 1rem;
    }

    label {
        display: block;
        margin-bottom: 0.5rem;
    }

    input,
    textarea {
        width: 100%;
        padding: 0.5rem;
        font-size: 1rem;
        margin-bottom: 1rem;
    }

    button {
        padding: 0.5rem 1rem;
        font-size: 1rem;
        background-color: #007bff;
        color: white;
        border: none;
        cursor: pointer;
    }

        button:hover {
            background-color: #0056b3;
        }

    .announcement-display {
        margin-top: 2rem;
        padding: 1rem;
        background-color: #f8f9fa;
        border-radius: 0.25rem;
    }
</style>
