<template>
    <div class="card">
        <alert-box :msg="alert"></alert-box>
        <confirm-box :msg="confirm"></confirm-box>
        <NotificationModal :visible="showNotification"
                           :title="notificationTitle"
                           :message="notificationMessage"
                           @close="showNotification = false" />
        <div class="button-container">
            <button class="back-button" @click="goBack">Back to Shop</button>
        </div>
        <!-- 图片信息 -->
        <div class="image-placeholder">
            <img :src="getAppImgUrl(app.image)" :alt="app.name" class="app-image" />
        </div>
        <!-- 应用详情 -->
        <div class="app-details">
            <h2 class="text-heading">{{ app.name }}</h2>
            <div class="tag">{{ app.category }}</div>
            <div class="price">￥{{ app.price }}</div>

            <div class="button-container">
                <button class="button" @click="goToPurchase(app.id)">购买</button>
                <button class="button" @click="toggleFavourite">{{ isFavourited ? '取消收藏' : '收藏' }}</button>
            </div>

            <div class="faq">
                <div class="faq-header" @click="toggleFAQ">
                    <h3 class="faq-title">介绍</h3>
                    <span class="arrow">
                        <img v-if="isFAQOpen" src="@/assets/direction-up.svg" alt="Arrow Up">
                        <img v-else src="@/assets/direction-down.svg" alt="Arrow Down">
                    </span>
                </div>
                <div class="faq-content" :class="{ hidden: !isFAQOpen }">
                    <p>{{ app.description }}</p>
                </div>
            </div>

            <!-- 举报按钮 -->
            <div class="report-button-container">
                <button class="button report-button" @click="openReportModal">举报</button>
            </div>
        </div>
    </div>
    <!-- 评论区域 -->
    <div class="comments-container">
        <h3>用户评论</h3>
        <div v-for="comment in comments" :key="comment.id" class="comment-item">
            <img :src="getAvatarUrl(comment.avatar)" alt="Avatar" class="avatar">
            <div class="info">
                <span class="nickname">{{ comment.nickname }}</span>
                <div class="score">
                    <span v-for="star in 5" :key="star" class="star" :class="{ filled: star <= comment.score }">&#9733;</span>
                </div>
                <p class="content">{{ comment.content }}</p>
                <span class="publishTime">{{ comment.publishTime }}</span>
            </div>
        </div>
        <div class="comment-editor">
            <textarea v-model="newComment.content" placeholder="输入评论内容"></textarea>
            <div class="score">
                <span v-for="star in 5" :key="star" class="star" :class="{ filled: star <= newComment.score }" @click="setScore(star)">&#9733;</span>
            </div>
            <button class="button" @click="submitComment">发布评论</button>
        </div>
    </div>

    <!-- 举报弹窗 -->
    <div v-if="showReportModal" class="report-modal">
        <div class="modal-content">
            <h3 style="font-size:30px;font-weight:bold">举报应用&nbsp;{{ app.name }}</h3>
            <p  style="font-size:15px;font-family:'Times New Roman', Times, serif">你确定要举报{{ app.name }}吗?请在下方填写举报内容</p>
            <textarea v-model="reportContent" placeholder="请输入举报内容"></textarea>
            <button class="rbutton" @click="submitReport">提交举报</button>
            <button class="rbutton" @click="closeReportModal">取消</button>
        </div>
    </div>
</template>

<script>
    import axios from 'axios';
    import Cookies from 'js-cookie';
    import NotificationModal from './NotificationModal.vue';
    import AlertBox from '../AlertBox.vue';
    import ConfirmBox from '../ConfirmBox.vue';

    export default {
        components: { 
            NotificationModal,
            AlertBox,
            ConfirmBox
        },
        data() {
            return {
                app: null,
                isFAQOpen: true,
                comments: [],
                newComment: {
                    content: '',
                    score: 0
                },
                isFavourited: false,
                alert: '',
                confirm:'',
                showReportModal: false, // 是否显示举报弹窗
                reportContent: '', // 举报内容
                showNotification: false,
                notificationTitle: '',
                notificationMessage: ''
            };
        },
        created() {
            const appId = this.$route.params.id;
            this.fetchAppDetails(appId);
            this.checkIfFavourite(appId);
            this.fetchAllComments(appId);
            this.fetchUserInfo();
        },
        methods: {
            toggleFAQ() {
                this.isFAQOpen = !this.isFAQOpen;
            },
            fetchAppDetails(appId) {
                axios.post('http://localhost:5118/api/application/getappdetail', { Id: appId })
                    .then(response => {
                        this.app = response.data;
                    })
                    .catch(error => {
                        console.error("Error fetching apps:", error);
                    });
            },
            getAppImgUrl(imgPath) {
                if (imgPath) {
                    return `http://localhost:5118${imgPath}`;
                }
                return '../../public/default.png'; // 默认图片路径
            },
            goBack() {
                this.$router.push('/WorkBanchPage');
            },
            goToPurchase(appId) {
                this.$router.push(`/Purchase/${appId}`);
            },
            addFavourite() {
                const token = Cookies.get('token');
                axios.post('http://localhost:5118/api/favourite/addFavourite', {
                    token: token,
                    id: this.app.id
                })
                    .then(response => {
                        const parsedData = response.data;
                        if (parsedData.success) {
                            this.confirmNotification('收藏成功！');
                            this.isFavourited = true;
                        } else {
                            this.alertNotification('收藏失败，请稍后重试！');
                        }
                    })
                    .catch(error => {
                        console.error('Error adding favourite:', error);
                    });
            },
            removeFavourite() {
                const token = Cookies.get('token');
                axios.post('http://localhost:5118/api/favourite/deleteFavourite', {
                    token: token,
                    id: this.app.id
                })
                    .then(response => {
                        const parsedData = response.data;
                        if (parsedData.success) {
                            this.confirmNotification('取消收藏成功！');
                            this.isFavourited = false;
                        } else {
                            this.alertNotification('取消收藏失败，请稍后重试！');
                        }
                    })
                    .catch(error => {
                        console.error('Error removing favourite:', error);
                    });
            },
            checkIfFavourite(appId) {
                const token = Cookies.get('token');
                axios.post('http://localhost:5118/api/favourite/checkIfFavourite', {
                    token: token,
                    appId: appId
                })
                    .then(response => {
                        const parsedData = response.data;
                        this.isFavourited = parsedData.isFavourited;
                    })
                    .catch(error => {
                        console.error('Error checking if favourite:', error);
                    });
            },
            toggleFavourite() {
                if (this.isFavourited) {
                    this.removeFavourite();
                } else {
                    this.addFavourite();
                }
            },
            alertNotification(message) {
                this.alert = message;
            },
            confirmNotification(message) {
                this.confirm = message;
            },
            installapp() {
                console.log('downloading: ' + this.app.id);
                axios.post('http://localhost:5118/api/application/installapp', { Id: this.app.id })
                    .then(response => {
                        window.location.href = `http://localhost:5118${response.data}`;
                        console.log('downloaded: ' + this.app.id);
                    })
                    .catch(error => {
                        console.error("Error install:", error);
                    });
            },
            fetchAllComments(appId) {
                axios.post('http://localhost:5118/api/comment/getappcomment', { ApplicationId: appId })
                    .then(response => {
                        this.comments = response.data.$values;
                        console.log("12 length of comments is " + this.comments.length);
                        console.log(this.comments[0].id);
                        console.log(this.comments[1].id);
                    })
                    .catch(error => {
                        console.error('Error fetching app comments:', error);
                        console.log("21");
                    });
            },
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
            setScore(score) {
                this.newComment.score = score;
            },
            submitComment() {
                const token = Cookies.get('token');
                axios.post('http://localhost:5118/api/comment/postappcomment', {
                    token: token,
                    content: this.newComment.content,
                    rating: this.newComment.score,
                    applicationId: this.app.id
                })
                .then(response => {
                    const parsedData = response.data;
                    if (parsedData && parsedData.success) {
                        alert('评论成功！');
                        const appId = this.$route.params.id;
                        this.fetchAllComments(appId);
                        // 清空评论表单
                        this.newComment.content = '';
                        this.newComment.score = 0;
                        /*this.isFavourited = true;*/
                    } else {
                        alert('评论失败：' + parsedData.msg);
                    }
                })
                .catch(error => {
                    console.error('Error adding comment:', error);
                    alert('评论失败：' + error.message);
                });
            },
            getAvatarUrl(avatarPath) {
                if (avatarPath) {
                    return `http://localhost:5118${avatarPath}`;
                }
                return '../../public/default.png'; // 默认头像路径
            },
            openReportModal() {
                console.log('Report modal opened'); // 这行用于调试
                this.showReportModal = true;
            },
            closeReportModal() {
                this.showReportModal = false;
                this.reportContent = ''; // 清空举报内容
            },
            submitReport() {
                const token = Cookies.get('token');

                // 获取当前时间并加上 8 小时
                const now = new Date();
                const reportTime = new Date(now.getTime() + 8 * 60 * 60 * 1000).toISOString(); // 加 8 小时并转换为 ISO 8601 格式

                axios.post('http://localhost:5118/api/report/publishreport', {
                    token: token,
                    content: this.reportContent,
                    reportTime: reportTime, // 传递调整后的时间
                    applicationId: this.app.id
                })
                    .then(response => {
                        this.reportContent= '';
                        this.notificationTitle = '成功';
                        this.notificationMessage = `成功举报 ${this.app.name}`;
                        this.showNotification = true;
                        this.showReportModal = false;
                    })
                    .catch(error => {
                        this.reportContent = '';
                        this.notificationTitle = '失败';
                        this.notificationMessage = '提交报告时发生错误。';
                        this.showNotification = true;
                        console.error('Error submitting report:', error);
                    });
            }
        }
    }
</script>


<style scoped>
    @import url('https://fonts.googleapis.com/css2?family=Pacifico&display=swap');

    .card {
        display: flex;
        border: 1px solid #e5e5e5;
        border-radius: 12px; /* Updated border radius */
        overflow: hidden;
        max-width: 800px;
        background-color: #fff;
        margin: auto;
        background-color: #faebd7; /* Updated background color */
    }

    .button-container {
        display: flex;
        flex-direction: column;
        padding: 10px;
    }

    .back-button {
        background: linear-gradient(135deg, #fbb1a2, #e89a8d); /* Gradient background */
        color: #fff;
        padding: 12px 24px;
        border: none;
        border-radius: 12px; /* Rounded corners */
        cursor: pointer;
        font-size: 18px; /* Slightly larger font size */
        font-weight: bold;
        transition: background-color 0.3s, transform 0.3s, box-shadow 0.3s;
        box-shadow: 0 6px 12px rgba(0, 0, 0, 0.3); /* Enhanced shadow */
    }

        .back-button:hover {
            background: linear-gradient(135deg, #e89a8d, #e0897c); /* Darker gradient on hover */
            transform: translateY(-3px); /* Slight lift effect */
        }

        .back-button:active {
            background: linear-gradient(135deg, #e0897c, #d5786f); /* Even darker gradient on click */
            transform: translateY(0);
        }

        .back-button:focus {
            outline: none;
            box-shadow: 0 0 0 4px rgba(250, 235, 215, 0.5);
        }

    .image-placeholder {
        width: 50%;
        height: 100%;
        display: flex;
        justify-content: center;
        align-items: center;
        background-color: #faebd7;
        margin-top: auto;
        margin-bottom: auto;
    }

    .app-image {
        width: auto;
        height: auto;
        max-width: 95%;
        max-height: 95%;
        object-fit: cover;
        display: block;
    }

    .app-details {
        width: 50%;
        padding: 20px;
        display: flex;
        flex-direction: column;
        justify-content: space-between;
    }

    .text-heading {
        font-size: 24px;
        font-family: 'Pacifico', cursive; /* Cute font */
        color: #333; /* Updated text color */
        margin-bottom: 10px;
    }

    .tag {
        background-color: #e0f7ea;
        color: #4caf50;
        padding: 4px 8px;
        margin-bottom: 10px;
        font-size: 16px;
        border-radius: 8px;
        width: fit-content;
        font-family: 'Pacifico', cursive; /* Cute font */
    }

    .price {
        font-size: 28px;
        font-weight: bold;
        color: #ff5722; /* Updated color */
        margin-bottom: 10px;
        font-family: 'Pacifico', cursive; /* Cute font */
    }

    .description {
        margin-bottom: 20px;
        font-family: 'Pacifico', cursive; /* Cute font */
    }

    .button, .purchase-button {
        display: inline-block;
        background-color: #fbb1a2;
        color: #fff;
        padding: 12px 24px;
        border: none;
        border-radius: 8px;
        cursor: pointer;
        font-size: 16px;
        font-weight: bold;
        transition: background-color 0.3s, transform 0.3s, box-shadow 0.3s;
        box-shadow: 0 4px 8px rgba(0, 0, 0, 0.2);
        font-family: 'Pacifico', cursive; /* Cute font */
    }

        .button:hover, .purchase-button:hover {
            background-color: #e89a8d;
            transform: translateY(-2px);
        }

        .button:active, .purchase-button:active {
            background-color: #e0897c;
            transform: translateY(0);
        }

        .button:focus, .purchase-button:focus {
            outline: none;
            box-shadow: 0 0 0 4px rgba(250, 235, 215, 0.5);
        }

    .faq {
        border-top: 1px solid #e5e5e5;
        padding-top: 10px;
    }

    .faq-header {
        display: flex;
        justify-content: space-between;
        align-items: center;
        cursor: pointer;
    }

    .faq-title {
        font-size: 18px;
        font-family: 'Pacifico', cursive; /* Cute font */
        margin: 0;
    }

    .arrow img {
        width: 38px;
        height: 38px;
    }

    .faq-content {
        height: 100px;
        overflow: hidden;
        transition: visibility 0.3s, opacity 0.3s;
    }

        .faq-content.hidden {
            visibility: hidden;
            opacity: 0;
        }

    .comments-container {
        max-width: 800px;
        margin: 20px auto;
        background: #f9f9f9;
        padding: 20px;
        border-radius: 8px;
        border: 1px solid #e5e5e5;
    }

    .comment-item {
        display: flex;
        margin-bottom: 20px;
    }

    .avatar {
        width: 50px;
        height: 50px;
        border-radius: 50%;
        margin-right: 10px;
    }

    .info {
        flex: 1;
    }

    .nickname {
        font-weight: bold;
    }

    .score {
        display: flex;
    }

    .star {
        font-size: 20px;
        color: #ccc;
    }

        .star.filled {
            color: #f5a623;
        }

    .content {
        margin-top: 5px;
    }

    .app-details {
        width: 50%;
        padding: 20px;
        display: flex;
        flex-direction: column;
        justify-content: space-between;
    }

    .button-container {
        display: flex;
        gap: 15px; /* 增加按钮间距 */
        margin-top: 10px; /* 调整与其他元素的间距 */
    }

    .publishTime {
        color: #888;
        font-size: 12px;
    }

    .comment-editor {
        margin-top: 20px;
    }

        .comment-editor textarea {
            width: 100%;
            padding: 10px;
            margin-bottom: 10px;
            border: 1px solid #e5e5e5;
            border-radius: 4px;
        }

        .comment-editor .score {
            margin-bottom: 10px;
        }

    /* 举报弹窗样式 */
    .report-modal {
        position: fixed; /* 固定在页面 */
        top: 0;
        left: 0;
        width: 100%;
        height: 100%;
        background: rgba(0, 0, 0, 0.5); /* 半透明背景 */
        display: flex;
        align-items: center;
        justify-content: center;
        z-index: 1000; /* 确保弹窗位于最上层 */
    }

    .modal-content {
        background-color: #fff;
        border-radius: 10px;
        padding: 20px;
        width: 80%;
        max-width: 500px;
        box-shadow: 0 4px 8px rgba(0, 0, 0, 0.2);
        position: relative;
    }

    .report-modal h3 {
        margin-top: 0;
    }

    textarea {
        width: 100%;
        height: 100px;
        border-radius: 5px;
        border: 1px solid #ddd;
        padding: 10px;
        margin-bottom: 10px;
        margin-top:10px;
    }

    .rbutton {
        margin-left: 90px;
        background-color: #fbb1a2;
        border: none;
        border-radius: 5px;
        color: white;
        padding: 10px 20px;
        cursor: pointer;
        font-weight:bolder;
        font-size:15px;
        font-family: 'Pacifico', cursive; /* Cute font */
        transition: background-color 0.3s, transform 0.2s;
    }

        .rbutton:hover {
            background-color: #e99a8f;
            transform: scale(1.05);
        }

    .report-button {
        background-color: #fbb1a2;
        border-color: #fbb1a2;
    }

</style>
