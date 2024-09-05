<template>
    <div class="announcement-form">
        <h2>公告发布</h2>

        <form @submit.prevent="showConfirmationPopup = true">
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

            <div class="button-container">
                <button type="submit">发布</button>
            </div>
        </form>

        <ConfirmationPopup :show="showConfirmationPopup"
                           @confirm="submitAnnouncement"
                           @cancel="showConfirmationPopup = false" />

        <NotificationPopup :show="showSuccessPopup"
                           message="管理员发布公告成功"
                           @update:show="showSuccessPopup = $event" />

        <NotificationPopup :show="showErrorPopup"
                           message="公告发布失败，请重试"
                           @update:show="showErrorPopup = $event" />
    </div>
</template>

<script>
    import axios from 'axios';
    import Cookies from 'js-cookie';
    import NotificationPopup from './NotificationPopup.vue';
    import ConfirmationPopup from './ConfirmationPopup.vue';

    export default {
        components: {
            NotificationPopup,
            ConfirmationPopup
        },
        data() {
            return {
                announcementTitle: '',
                announcementContent: '',
                showSuccessPopup: false,
                showErrorPopup: false,
                showConfirmationPopup: false
            };
        },
        methods: {
            submitAnnouncement() {
                let formData = new FormData();
                var token = Cookies.get('token');
                formData.append('token', token);
                formData.append('title', this.announcementTitle);
                formData.append('content', this.announcementContent);

                axios.post(`${this.$Url}/api/announcement/publishannouncement`, formData)
                    .then(response => {
                        this.showSuccessPopup = true;
                        this.announcementTitle = '';
                        this.announcementContent = '';
                        this.showConfirmationPopup = false;
                    })
                    .catch(error => {
                        this.showErrorPopup = true;
                        console.error('发布失败:', error);
                        this.showConfirmationPopup = false;
                    });
            }
        }
    };
</script>

<style scoped>
    .announcement-form {
        max-width: 800px;
        margin: 0 auto;
        padding: 30px;
        border-radius: 12px;
        background: #fafafa;
        box-shadow: 0 6px 12px rgba(0, 0, 0, 0.1);
        font-family: 'Poppins', sans-serif;
        animation: fadeIn 0.5s ease-in-out;
    }

        .announcement-form h2 {
            margin-bottom: 1.5rem;
            color: #333;
            font-size: 1.75rem;
            text-align: center;
            font-weight: 600;
        }

    .form-group {
        margin-bottom: 2rem;
    }

    label {
        display: block;
        margin-bottom: 0.75rem;
        color: #555;
        font-weight: 600; /* Increased font weight for emphasis */
        font-size: 1.25rem; /* Larger font size for better readability */
        letter-spacing: 0.5px; /* Slightly increased letter spacing */
        text-shadow: 0 1px 3px rgba(0, 0, 0, 0.1); /* Subtle text shadow for depth */
    }

    input,
    textarea {
        width: 100%;
        padding: 1rem;
        font-size: 1.125rem;
        border: 1px solid #ddd;
        border-radius: 5px;
        box-shadow: inset 0 1px 3px rgba(0, 0, 0, 0.1);
        transition: border-color 0.3s ease, box-shadow 0.3s ease;
    }

        input:focus,
        textarea:focus {
            border-color: #007bff;
            box-shadow: 0 0 8px rgba(0, 123, 255, 0.25);
            outline: none;
        }

    .button-container {
        display: flex;
        justify-content: center;
    }

    button {
        padding: 1rem 2rem;
        font-size: 1.125rem;
        background-color: #007bff;
        color: white;
        border: none;
        border-radius: 5px;
        cursor: pointer;
        transition: background-color 0.3s ease, transform 0.3s ease;
        box-shadow: 0 6px 12px rgba(0, 0, 0, 0.2);
    }

        button:hover {
            background-color: #0056b3;
            transform: translateY(-2px);
        }

        button:active {
            background-color: #004494;
            transform: translateY(0);
        }

    @keyframes fadeIn {
        from {
            opacity: 0;
            transform: translateY(-10px);
        }

        to {
            opacity: 1;
            transform: translateY(0);
        }
    }
</style>
