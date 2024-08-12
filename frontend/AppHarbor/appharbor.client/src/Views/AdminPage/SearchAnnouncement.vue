<template>
    <div class="announcement-container">
        <ul class="announcement-list">
            <li v-for="announce in announces" :key="announce.id" class="announcement-item">
                <h2 class="announcement-title">第{{ announce.id }}号公告：{{ announce.title }}</h2>
                <p class="announcement-details">
                    发布时间: <span class="detail-value">{{ announce.publishTime }}</span>
                </p>
                <button class="view-detail-button" @click="openDetail(announce)">查看详细</button>
            </li>
        </ul>

        <!-- Detail Popup -->
        <div v-if="selectedAnnouncement" class="popup-overlay" @click="closeDetail">
            <div class="popup-content" @click.stop>
                <h2 class="popup-title">{{ selectedAnnouncement.title }}</h2>
                <p class="popup-details">
                    发布公告管理员: <span class="detail-value">{{ selectedAnnouncement.adminNickname }}</span>
                </p>
                <p class="popup-details">
                    发布时间: <span class="detail-value">{{ selectedAnnouncement.publishTime }}</span>
                </p>
                <p class="popup-details">
                    公告内容:
                </p>
                <p class="popup-content-text">
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;{{ selectedAnnouncement.content }}
                </p>
                <button class="popup-close-button" @click="closeDetail">关闭</button>
            </div>
        </div>

    </div>
</template>

<script>
    import axios from 'axios';

    export default {
        data() {
            return {
                announces: [],
                selectedAnnouncement: null,
            };
        },
        mounted() {
            this.searchAnnouncement();
        },
        methods: {
            searchAnnouncement() {
                axios.post('http://localhost:5118/api/announcement/getannouncementlist')
                    .then(response => {
                        this.announces = response.data.$values;
                        console.log(this.announces);
                    })
                    .catch(error => {
                        console.error('查看公告失败:', error);
                    });
            },
            openDetail(announcement) {
                this.selectedAnnouncement = announcement;
            },
            closeDetail() {
                this.selectedAnnouncement = null;
            },
        },
    };
</script>

<style scoped>
    .announcement-container {
        padding: 20px;
        background-color: #f5f5f5;
        border-radius: 8px;
        max-width: 900px;
        margin: 0 auto;
    }

    .announcement-list {
        list-style-type: none;
        padding: 0;
    }

    .announcement-item {
        margin-bottom: 20px;
        border-bottom: 1px solid #ddd;
        padding: 15px;
        background-color: #fff;
        border-radius: 8px;
        box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
        transition: transform 0.3s ease, box-shadow 0.3s ease;
        position: relative; /* For positioning the button */
    }

        .announcement-item:hover {
            transform: translateY(-5px);
            box-shadow: 0 6px 12px rgba(0, 0, 0, 0.2);
        }

    .announcement-title {
        margin: 0;
        font-size: 22px;
        color: #6a1b9a;
        font-family: 'Comic Sans MS', cursive, sans-serif;
    }

    .announcement-details {
        font-size: 14px;
        color: #555;
    }

    .detail-value {
        font-weight: bold;
        color: #000;
    }

    .view-detail-button {
        position: absolute;
        right: 15px;
        top: 15px;
        background-color: #6a1b9a;
        color: #fff;
        border: none;
        border-radius: 8px;
        padding: 10px 16px;
        cursor: pointer;
        transition: background-color 0.3s ease, transform 0.3s ease, box-shadow 0.3s ease;
        font-size: 14px;
        font-weight: bold;
    }

        .view-detail-button:hover {
            background-color: #4a0072;
            transform: scale(1.05); /* 放大效果 */
            box-shadow: 0 4px 10px rgba(0, 0, 0, 0.2); /* 悬停时阴影效果 */
        }

    .popup-overlay {
        position: fixed;
        top: 0;
        left: 0;
        width: 100%;
        height: 100%;
        background: rgba(0, 0, 0, 0.5);
        display: flex;
        justify-content: center;
        align-items: center;
        z-index: 1000;
    }

    .popup-content {
        background: #fff;
        padding: 20px;
        border-radius: 12px;
        max-width: 600px;
        width: 90%;
        box-shadow: 0 6px 12px rgba(0, 0, 0, 0.2);
        position: relative;
        display: flex;
        flex-direction: column;
        gap: 15px;
        animation: popupIn 0.3s ease-out; /* 弹窗显示动画 */
    }

    .popup-title {
        margin: 0;
        font-size: 24px;
        color: #6a1b9a;
        font-family: 'Comic Sans MS', cursive, sans-serif;
        font-weight: bold; /* 加粗标题 */
    }

    .popup-content-text {
        font-size: 16px;
        color: #333;
        line-height: 1.5;
        margin-top: auto; /* Pushes the content to the bottom */
        white-space: pre-line; /* 保持内容换行 */
    }

    .popup-details {
        font-size: 14px;
        color: #555;
    }

    .detail-value {
        font-weight: bold; /* 详情值加粗 */
        color: #000;
    }

    .popup-close-button {
        position: absolute;
        top: 10px;
        right: 10px;
        background-color: #6a1b9a;
        color: #fff;
        border: none;
        border-radius: 50%;
        padding: 10px;
        cursor: pointer;
        transition: background-color 0.3s ease, transform 0.3s ease;
        font-size: 14px;
        font-weight: bold;
    }

        .popup-close-button:hover {
            background-color: #4a0072;
            transform: scale(1.1); /* 放大效果 */
        }

    @keyframes popupIn {
        from {
            opacity: 0;
            transform: translateY(-20px);
        }

        to {
            opacity: 1;
            transform: translateY(0);
        }
    }
</style>
