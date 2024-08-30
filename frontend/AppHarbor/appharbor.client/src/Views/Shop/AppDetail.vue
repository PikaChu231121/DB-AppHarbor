<template>
    <div class="card">
        <alert-box :msg="alert"></alert-box>
        <confirm-box :msg="confirm"></confirm-box>
        <NotificationModal :visible="showNotification" :title="notificationTitle" :message="notificationMessage"
                           @close="showNotification = false" />
        <div class="button-container">
            <button class="back-button" @click="goBack">返回商店</button>
        </div>
        <!-- 图片信息 -->
        <div class="image-placeholder">
            <div class="image-frame">
                <img :src="getAppImgUrl(app.image)" :alt="app.name" class="app-image" />
            </div>
        </div>
        <!-- 应用详情 -->
        <div class="app-details">
            <h2 class="text-heading">{{ app.name }}</h2>
            <div class="tag">{{ app.category }}</div>
            <div class="price" v-html="formattedPrice"></div>

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
                    <span v-for="star in 5" :key="star" class="star"
                          :class="{ filled: star <= comment.score }">&#9733;</span>
                </div>
                <p class="content">{{ comment.content }}</p>
                <span class="publishTime">发布于 {{ formatDate(comment.publishTime) }}</span>
            </div>
            <div class="delete-button-container">
                <button v-if="comment.userId === user.id"
                        @click="deleteComment(comment.id)"
                        class="button delete-button">
                    删除
                </button>
            </div>
        </div>
        <div class="comment-editor">
            <textarea v-model="newComment.content" placeholder="输入评论内容"></textarea>
            <div class="score">
                <span v-for="star in 5" :key="star" class="star" :class="{ filled: star <= newComment.score }"
                      @click="setScore(star)">&#9733;</span>
            </div>
            <button class="button" @click="submitComment">发布评论</button>
        </div>
    </div>

    <!-- 举报弹窗 -->
    <div v-if="showReportModal" class="report-modal">
        <div class="modal-content">
            <h3 style="font-size:30px;font-weight:bold">举报应用&nbsp;{{ app.name }}</h3>
            <p style="font-size:15px;font-family:'Times New Roman', Times, serif">你确定要举报{{ app.name }}吗?请在下方填写举报内容</p>
            <textarea v-model="reportContent" placeholder="请输入举报内容"></textarea>
            <button class="rbutton" @click="submitReport">提交举报</button>
            <button class="rbutton" @click="closeReportModal">取消</button>
        </div>
    </div>

    <!-- 评论弹窗 -->
    <div v-if="showModal" class="modal-overlay" @click="closeModal">
        <div class="modal-content" @click.stop>
            <p>{{ commentMessage }}</p>
            <button @click="closeModal">关闭</button>
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
                user: null,
                isFAQOpen: true,
                comments: [],
                newComment: {
                    content: '',
                    score: 0
                },
                isFavourited: false,
                alert: '',
                confirm: '',
                showReportModal: false, // 是否显示举报弹窗
                reportContent: '', // 举报内容
                showNotification: false,
                notificationTitle: '',
                notificationMessage: '',
                showModal: false, // 是否显示评论弹窗
                commentMessage: '',
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
                this.alert = '';
                this.$nextTick(() => {
                    this.alert = message;
                });
            },
            confirmNotification(message) {
                this.confirm = '';
                this.$nextTick(() => {
                    this.confirm = message;
                });
            },
            installapp() {
                console.log('downloading: ' + this.app.id);
                axios.post('http://localhost:5118/api/application/installApp', { Id: this.app.id })
                    .then(response => {
                        console.log('downloaded: ' + response.data);
                        if (response.data !== '') {
                            alert('购买成功！');
                        }
                    })
                    .catch(error => {
                        console.error('Error downloading app:', error);
                    });
            },
            submitComment() {
                const token = Cookies.get('token');
                axios.post('http://localhost:5118/api/comment/submitComment', {
                    token: token,
                    appId: this.app.id,
                    content: this.newComment.content,
                    score: this.newComment.score
                })
                    .then(response => {
                        const parsedData = response.data;
                        if (parsedData.success) {
                            this.comments.push(parsedData.comment);
                            this.commentMessage = '评论发布成功！';
                            this.newComment.content = '';
                            this.newComment.score = 0;
                        } else {
                            this.commentMessage = '评论发布失败，请稍后重试！';
                        }
                        this.showModal = true;
                    })
                    .catch(error => {
                        console.error('Error submitting comment:', error);
                        this.commentMessage = '评论发布失败，请稍后重试！';
                        this.showModal = true;
                    });
            },
            deleteComment(commentId) {
                const token = Cookies.get('token');
                axios.post('http://localhost:5118/api/comment/deleteComment', {
                    token: token,
                    commentId: commentId
                })
                    .then(response => {
                        const parsedData = response.data;
                        if (parsedData.success) {
                            this.comments = this.comments.filter(comment => comment.id !== commentId);
                            this.commentMessage = '评论删除成功！';
                        } else {
                            this.commentMessage = '评论删除失败，请稍后重试！';
                        }
                        this.showModal = true;
                    })
                    .catch(error => {
                        console.error('Error deleting comment:', error);
                        this.commentMessage = '评论删除失败，请稍后重试！';
                        this.showModal = true;
                    });
            },
            fetchAllComments(appId) {
                axios.post('http://localhost:5118/api/comment/getAllComments', { appId: appId })
                    .then(response => {
                        this.comments = response.data.comments;
                    })
                    .catch(error => {
                        console.error("Error fetching comments:", error);
                    });
            },
            setScore(score) {
                this.newComment.score = score;
            },
            closeModal() {
                this.showModal = false;
            },
            openReportModal() {
                this.showReportModal = true;
            },
            closeReportModal() {
                this.showReportModal = false;
            },
            submitReport() {
                const token = Cookies.get('token');
                axios.post('http://localhost:5118/api/report/submitReport', {
                    token: token,
                    appId: this.app.id,
                    content: this.reportContent
                })
                    .then(response => {
                        const parsedData = response.data;
                        if (parsedData.success) {
                            this.notificationTitle = '举报成功';
                            this.notificationMessage = '您的举报已提交，我们将尽快处理。';
                        } else {
                            this.notificationTitle = '举报失败';
                            this.notificationMessage = '举报提交失败，请稍后重试。';
                        }
                        this.showReportModal = false;
                        this.showNotification = true;
                    })
                    .catch(error => {
                        console.error('Error submitting report:', error);
                        this.notificationTitle = '举报失败';
                        this.notificationMessage = '举报提交失败，请稍后重试。';
                        this.showReportModal = false;
                        this.showNotification = true;
                    });
            },
            getAvatarUrl(avatarPath) {
                if (avatarPath) {
                    return `http://localhost:5118${avatarPath}`;
                }
                return '../../public/default-avatar.png'; // 默认头像图片路径
            },
            formatDate(dateString) {
                const date = new Date(dateString);
                return `${date.getFullYear()}-${date.getMonth() + 1}-${date.getDate()}`;
            },
            fetchUserInfo() {
                const token = Cookies.get('token');
                axios.post('http://localhost:5118/api/user/getUserInfo', { token: token })
                    .then(response => {
                        this.user = response.data.user;
                    })
                    .catch(error => {
                        console.error("Error fetching user info:", error);
                    });
            },
        },
        computed: {
            formattedPrice() {
                if (this.app.discountPrice > 0 && this.app.discountPrice < this.app.price) {
                    return `<span class="price-original">${this.app.price} 元</span> <span class="price-discount">${this.app.discountPrice} 元</span>`;
                }
                return `<span class="price-current">${this.app.price} 元</span>`;
            },
        },
    };
</script>

<style scoped>
    body {
        margin: 0;
        padding: 0;
        font-family: 'Arial', sans-serif;
        background-color: #f0f0f0;
        color: #333;
    }

    .card {
        max-width: 1200px;
        margin: 20px auto;
        padding: 20px;
        background-color: #ffffff;
        box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
        border-radius: 15px;
        display: flex;
        flex-direction: row;
        justify-content: space-between;
        align-items: center;
    }

    .image-placeholder {
        flex: 1;
        text-align: center;
    }

    .image-frame {
        display: inline-block;
        padding: 10px;
        border-radius: 15px;
        background: linear-gradient(145deg, #e0e0e0, #ffffff);
        box-shadow: 5px 5px 10px #c6c6c6, -5px -5px 10px #ffffff;
    }

    .app-image {
        width: 100%;
        height: auto;
        max-width: 300px;
        border-radius: 10px;
        object-fit: contain;
    }

    .app-details {
        flex: 2;
        text-align: left;
        padding-left: 20px;
    }

    .text-heading {
        font-size: 24px;
        font-weight: bold;
        color: #333; /* Updated text color */
        margin-bottom: 10px;
        font-size: 2em;
        font-weight: bold;
        color: #2c3e50;
    }

    .price {
        font-size: 1.5em;
        color: #ff5722;
        margin-bottom: 20px;
    }

    .tag {
        background-color: #e0f7ea;
        color: #4caf50;
        padding: 4px 8px;
        margin-bottom: 10px;
        font-size: 16px;
        border-radius: 8px;
        width: fit-content;
        font-weight: bold;
    }

    .price-discount {
        color: #ff5722;
        margin-left: 10px;
    }

    .price-current {
        font-weight: bold;
        color: #ff5722; /* Updated color */
        margin-bottom: 10px;
        font-weight: bold;
    }

    .description {
        margin-bottom: 20px;
        font-weight: bold;
    }

    .button {
        padding: 12px 25px;
        border: none;
        border-radius: 25px;
        background-color: #2196f3;
        color: white;
        font-size: 1.1em;
        cursor: pointer;
        font-size: 16px;
        font-weight: bold;
        transition: background-color 0.3s, transform 0.3s, box-shadow 0.3s;
        box-shadow: 0 4px 8px rgba(0, 0, 0, 0.2);
        font-weight: bold;
    }

        .button:hover {
            background-color: #1976d2;
        }

    .back-button {
        padding: 10px 20px;
        font-size: 1em;
        color: #fff;
        background-color: #757575;
        border-radius: 25px;
        border: none;
        cursor: pointer;
    }

        .back-button:hover {
            background-color: #616161;
        }

    .faq {
        text-align: left;
        margin-top: 20px;
    }

    .faq-header {
        cursor: pointer;
        display: flex;
        justify-content: space-between;
        align-items: center;
    }

    .faq-title {
        font-size: 18px;
        font-weight: bold;
        margin: 0;
        font-size: 1.5em;
        font-weight: bold;
    }

    .faq-content {
        margin-top: 10px;
    }

        .faq-content.hidden {
            display: none;
        }

    .report-button-container {
        text-align: center;
        margin-top: 20px;
    }

    .report-button {
        background-color: #e53935;
        color: white;
        padding: 12px 25px;
        border-radius: 25px;
        cursor: pointer;
        border: none;
        transition: background-color 0.3s;
    }

        .report-button:hover {
            background-color: #c62828;
        }

    .comments-container {
        margin-top: 40px;
    }

    .comment-item {
        display: flex;
        margin-bottom: 20px;
        padding: 15px;
        background-color: #f5f5f5;
        border-radius: 10px;
    }

    .avatar {
        width: 50px;
        height: 50px;
        border-radius: 50%;
        margin-right: 15px;
    }

    .info {
        flex-grow: 1;
    }

    .nickname {
        font-weight: bold;
        margin-bottom: 5px;
        color: #333;
    }

    .score {
        margin-bottom: 5px;
    }

    .star {
        font-size: 1.2em;
        color: #ffb400;
    }

        .star.filled {
            color: #ffb400;
        }

    .content {
        margin-bottom: 10px;
        color: #555;
    }

    .publishTime {
        font-size: 0.8em;
        color: #757575;
    }

    .delete-button-container {
        margin-left: 15px;
    }

    .delete-button {
        background-color: #d32f2f;
        color: white;
        padding: 5px 10px;
        border-radius: 5px;
        border: none;
        cursor: pointer;
    }

        .delete-button:hover {
            background-color: #c62828;
        }

    .comment-editor {
        margin-top: 20px;
    }

        .comment-editor textarea {
            width: 100%;
            padding: 15px;
            border: 1px solid #ccc;
            border-radius: 10px;
            margin-bottom: 10px;
        }

    .modal-overlay {
        position: fixed;
        top: 0;
        left: 0;
        right: 0;
        bottom: 0;
        background-color: rgba(0, 0, 0, 0.5);
        display: flex;
        justify-content: center;
        align-items: center;
    }

    .modal-content {
        background-color: #ffffff;
        padding: 20px;
        border-radius: 15px;
        max-width: 500px;
        width: 100%;
        height: 100px;
        border-radius: 5px;
        border: 1px solid #ddd;
        padding: 10px;
        margin-bottom: 10px;
        margin-top:10px;
    }

    .rbutton {
        margin-left: 60px;
        background-color: #fbb1a2;
        border: none;
        border-radius: 5px;
        color: white;
        padding: 10px 20px;
        cursor: pointer;
        font-weight: bolder;
        font-size: 15px;
        font-weight: bold;
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

    .delete-button {
        background-color: #fbb1a2;
        color: white;
        border: none;
        padding: 5px 10px;
        border-radius: 4px;
        cursor: pointer;
        font-size: 15px;
        margin-left: 10px;
        margin-right: auto;
    }

    .report-modal {
        position: fixed;
        top: 0;
        left: 0;
        right: 0;
        bottom: 0;
        background-color: rgba(0, 0, 0, 0.5);
        display: flex;
        justify-content: center;
        align-items: center;
    }

    .modal-content {
        background-color: white;
        padding: 20px;
        border-radius: 15px;
        max-width: 400px;
        width: 100%;
        text-align: center;
    }

        .modal-content textarea {
            width: 100%;
            height: 100px;
            padding: 15px;
            border: 1px solid #ccc;
            border-radius: 10px;
            margin-bottom: 20px;
        }

        .modal-content button {
            padding: 12px 25px;
            border: none;
            border-radius: 25px;
            cursor: pointer;
            font-size: 1.1em;
            background-color: #2196f3;
            color: white;
            margin-right: 10px;
        }

            .modal-content button:hover {
                background-color: #1976d2;
            }

        .modal-content .rbutton {
            background-color: #e53935;
            color: white;
            padding: 12px 25px;
            border-radius: 25px;
            cursor: pointer;
            border: none;
            transition: background-color 0.3s;
        }

            .modal-content .rbutton:hover {
                background-color: #c62828;
            }
</style>

