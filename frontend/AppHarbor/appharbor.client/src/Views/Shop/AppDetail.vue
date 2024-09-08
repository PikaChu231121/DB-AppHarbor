<template>
    <div class="card">
        <alert-box :msg="alert"></alert-box>
        <confirm-box :msg="confirm"></confirm-box>
        <NotificationModal :visible="showNotification" :title="notificationTitle" :message="notificationMessage"
                           @close="showNotification = false" />
        <div class="appheader">
            <img loading="lazy" src="@/../public/logo-text.png" class="logo-text-image" alt="AppHabour Logo Text" />
            <div class="button-container">
                <button class="back-button" @click="goBack">返回</button>
            </div>
            <div class="header-title">应用详情 : {{ app.name }}</div>
        </div>


        <div class="app-details" :style="{ backgroundImage: `url(${getAppImgUrl(app.image)})` }">
            <div class="acrylic"></div>
            <div class="image-placeholder">
                <img :src="getAppImgUrl(app.image)" :alt="app.name" class="app-image" />
            </div>

            <div class="appd">
                <h2 class="app-name">{{ app.name }}</h2>
                <div class="tag">{{ app.category }}</div>
                <div class="price" v-html="formattedPrice"></div>
                <button class="button" @click="goToPurchase(app.id)">购买</button>
            </div>
            <div class="favandrep">
                <button class="favourite-button" @click="toggleFavourite" :class="{ 'not-favourited': !isFavourited }">
                    <span class="icon">{{ isFavourited ? '😍' : '☆' }}</span>
                </button>
                <div class="report-button-container">
                    <button class="report-button" @click="openReportModal">
                        <span class="icon">😵</span>
                    </button>
                </div>
            </div>
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

        <div v-if="showReportModal" class="report-modal">
            <div class="modal-content">
                <h3 style="font-size:30px;font-weight:bold">举报应用&nbsp;{{ app.name }}</h3>
                <p style="font-size:15px;font-family:'Times New Roman', Times, serif">你确定要举报{{ app.name }}吗?请在下方填写举报内容</p>
                <textarea v-model="reportContent" placeholder="请输入举报内容"></textarea>
                <button class="rbutton" @click="submitReport">提交举报</button>
                <button class="rbutton" @click="closeReportModal">取消</button>
            </div>
        </div>

        <div v-if="showModal" class="modal-overlay" @click="closeModal">
            <div class="modal-content" @click.stop>
                <p>{{ commentMessage }}</p>
                <button @click="closeModal">关闭</button>
            </div>
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
    beforeRouteEnter(to, from, next) {
        next(vm => {
            // 路由进入此页时，清空旧的 app 数据，重新获取
            vm.app = '';
            const appId = to.params.id;
            vm.fetchAppDetails(appId);
            vm.checkIfFavourite(appId);
            vm.fetchAllComments(appId);
            vm.fetchUserInfo();
        });
    },
        data() {
            return {
                app: '',
                user: '',
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
                axios.post(`${this.$Url}/api/application/getappdetail`, { Id: appId })
                    .then(response => {
                        this.app = response.data;
                    })
                    .catch(error => {
                        console.error("Error fetching apps:", error);
                    });
            },
            getAppImgUrl(imgPath) {
                if (imgPath) {
                    return `${this.$Url}${imgPath}`;
                }
                return '../../public/473a0adf42244483aad60880b50e129a.png'; // 默认图片路径
            },
            goBack() {
                this.$router.push('/WorkBanchPage');
            },
            goToPurchase(appId) {
                this.$router.push(`/Purchase/${appId}`);
            },
            addFavourite() {
                const token = Cookies.get('token');
                axios.post(`${this.$Url}/api/favourite/addFavourite`, {
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
                axios.post(`${this.$Url}/api/favourite/deleteFavourite`, {
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
                axios.post(`${this.$Url}/api/favourite/checkIfFavourite`, {
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
                axios.post(`${this.$Url}/api/application/installapp`, { Id: this.app.id })
                    .then(response => {
                        window.location.href = `${this.$Url}${response.data}`;
                        console.log('downloaded: ' + this.app.id);
                    })
                    .catch(error => {
                        console.error("Error install:", error);
                    });
            },
            fetchAllComments(appId) {
                axios.post(`${this.$Url}/api/comment/getappcomment`, { ApplicationId: appId })
                    .then(response => {
                        this.comments = response.data.$values;
                    })
                    .catch(error => {
                        console.error('Error fetching app comments:', error);
                    });
            },
            fetchUserInfo() {
                var token = Cookies.get('token');
                axios.post(`${this.$Url}/api/user/userInfo`, { token: token })
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
                axios.post(`${this.$Url}/api/comment/postappcomment`, {
                    token: token,
                    content: this.newComment.content,
                    rating: this.newComment.score,
                    applicationId: this.app.id
                })
                    .then(response => {
                        const parsedData = response.data;
                        if (parsedData && parsedData.success) {
                            // 评论弹窗
                            this.commentMessage = '评论成功！';
                            this.showModal = true;
                            // 更新评论显示
                            const appId = this.$route.params.id;
                            this.fetchAllComments(appId);
                            // 清空评论表单
                            this.newComment.content = '';
                            this.newComment.score = 0;
                            /*this.isFavourited = true;*/
                        } else {
                            this.commentMessage = '评论失败：' + parsedData.msg;
                            this.showModal = true;
                        }
                    })
                    .catch(error => {
                        const parsedData = error.response.data;
                        console.error('Error adding comment:', error);
                        this.commentMessage = '评论失败：' + parsedData.msg;
                        this.showModal = true;
                    });
            },
            deleteComment(commentId) {
                axios.post(`${this.$Url}/api/comment/deleteappcomment`, {
                    commentId: commentId
                })
                    .then(response => {
                        const parsedData = response.data;
                        if (parsedData && parsedData.success) {
                            this.commentMessage = '删除评论成功！';
                            this.showModal = true;
                            this.fetchAllComments(this.app.id); // 重新获取评论列表以更新页面
                        } else {
                            this.commentMessage = '删除评论失败：' + parsedData.msg;
                            this.showModal = true;
                        }
                    })
                    .catch(error => {
                        console.error('Error deleting comment:', error);
                        this.commentMessage = '删除评论失败：' + error.message;
                        this.showModal = true;
                    });
            },
            closeModal() {
                this.showModal = false;
            },
            getAvatarUrl(avatarPath) {
                if (avatarPath) {
                    return `${this.$Url}${avatarPath}`;
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

                axios.post(`${this.$Url}/api/report/publishreport`, {
                    token: token,
                    content: this.reportContent,
                    reportTime: reportTime, // 传递调整后的时间
                    applicationId: this.app.id
                })
                    .then(() => {
                        this.reportContent = '';
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
            },
            formatDate(dateString) {
                const date = new Date(dateString);
                return date.toLocaleString('zh-CN', {
                    year: 'numeric',
                    month: '2-digit',
                    day: '2-digit',
                    hour: '2-digit',
                    minute: '2-digit',
                    second: '2-digit',
                    hour12: false
                }).replace(/\//g, '-'); // 将斜杠替换为短横线
            },
        },
        computed: {
            formattedPrice() {
                if (!this.app)return;
                if (this.app.price === 0) {
                    return `<span>Free! 免费</span>`;
                }

                const originalPrice = this.app.price.toFixed(2).split('.');
                const originalIntegerPart = originalPrice[0];
                const originalDecimalPart = originalPrice[1];

                const discountedPrice = (this.app.price * this.app.discount).toFixed(2).split('.');
                const discountedIntegerPart = discountedPrice[0];
                const discountedDecimalPart = discountedPrice[1];

                let result = `
                    <span>
                        ￥ <span class="integer-part">${discountedIntegerPart}</span>.<span class="decimal-part">${discountedDecimalPart}</span>
                    </span>
                `;

                if (this.app.discount < 1.0) {
                    result += `
                    <span style="text-decoration: line-through; font-size: 0.6em; color:gray;">
                        原价：<span class="integer-part">${originalIntegerPart}</span>.<span class="decimal-part">${originalDecimalPart}</span>
                    </span>
                `;
                }

                return result;
            }

        }
    }
</script>


<style scoped>

    .card {
        width: 100%; 
        //height: 100vh;
        display: flex; 
        flex-direction: column; 
        box-sizing: border-box; 
        background-color:whitesmoke;
    }

    .appheader {
        display: flex;
        height: 65px;
        position: fixed;
        top: 0;
        left: 0;
        width: 100%;
        display: flex;
        justify-content: space-between;
        padding: 12px;
        gap: 20px;
        background-color: #efc2bb; /* Optional: Add a background color */
        z-index: 1000;
        border-bottom: 4px solid #ffe7d3;
    }

    .header-title {
        margin-left: auto;
        margin-right: 20px;
        font-weight: bold;
        font-size: 25px;
        color: white;
    }

    
    .logo-text-image {
        height: 40px; /* Adjust height as needed */
    }


    .button-container {
        position: absolute;
        left: 50%;
        top: 50%;
        transform: translate(-50%, -50%);
        display: flex; 
        justify-content: center;
        align-items: center;
    }

    .back-button {
        background-color: #fbb1a2; /* 背景颜色 */
        padding: 10px 20px; /* 内边距 */
        border-radius: 8px; /* 边框圆角 */
        //box-shadow: 0 4px 8px rgba(0, 0, 0, 0.2); /* 阴影效果 */
        z-index: 100; /* 确保在其他元素之上 */
        transition: background-color 0.3s ease, box-shadow 0.3s ease;
        font-weight: bold !important;
        font-family:'Microsoft YaHei';
        font-size: 20px;
        color:white;
        border: none;
    }

        .back-button:hover {
            background-color :darksalmon;
            //background: linear-gradient(135deg, #e89a8d, #e0897c); /* Darker gradient on hover */
            //transform: translateY(-3px); /* Slight lift effect */
        }

        .back-button:active {
            background: linear-gradient(135deg, #e0897c, #d5786f); /* Even darker gradient on click */
            transform: translateY(0);
        }

        .back-button:focus {
            outline: none;
            box-shadow: 0 0 0 4px rgba(250, 235, 215, 0.5);
        }

    .app-details {
        margin-top: 30px;
        padding: 150px;
        box-sizing: border-box;
        display: flex;
        position: relative;
        //overflow: hidden;
        background-size: cover;
        background-position: center;
        background-repeat: no-repeat;
    }

        .app-details .acrylic {
            position: absolute;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            backdrop-filter: blur(10px) brightness(0.8);
            //background-color: whitesmoke;
            background-color: #ffffffef;
            z-index: 1;
        }


    .image-placeholder {
        width: 250px; 
        height: 250px; 
        //background-color: #eaeaea; 
        border-radius: 60px; /* 添加圆角 */
        overflow: hidden; 
        z-index:2;
    }


    .app-image {
        width: 100%;
        height: 100%;
        object-fit: cover; /* 确保图片不会变形 */
    }


    .appd {
        margin-left: 50px;
        margin-right: 100px;
        width: auto;
        display: flex;
        flex-direction: column;
        justify-content: space-between;
        flex: 1;
        position: relative;
        z-index: 2;
    }


    .app-name {
        font-size: 45px;
        font-weight: bold;
        //margin-bottom: 10px; /* 添加一些下边距 */
    }

    /* 分类和价格样式 */
    .tag  {
        font-size: 18px;
        margin-top: 10px;
    }
    .price {
        font-size: 15px;
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

    .price {
        font-size: 28px;
        font-weight: bold;
        color: #ff5722; /* Updated color */
        margin-bottom: 10px;
        font-weight: bold;
    }

    .favandrep {
        right: 50px;
        display: flex;
        flex-direction: column;
        justify-content: space-between;
        flex: 1;
        position: relative;
        z-index: 2;
    }

    /* 调整购买按钮样式 */
    .button {
        max-width: 360px;
        padding: 10px 10px;
        font-size: 25px;
        font-family: 'Microsoft YaHei';
        border-radius: 8px;
        background-color: #fbb1a2;
        color: white;
        transition: background-color 0.3s ease;
    }

    .favourite-button {
        position: relative;
        width: 60px;
        height: 60px;
        background-color: white;
        border-radius: 20px;
        border: 0;
        box-shadow: 2px 3px 3px rgba(0, 0, 0, 0.05);
        transition: transform 0.2s ease, box-shadow 0.2s ease;
        cursor: pointer;
    }

        .favourite-button:hover {
            transform: translateY(-3px);
            box-shadow: 2px 5px 5px rgba(0, 0, 0, 0.1);
        }

        .favourite-button .icon {
            font-size: 30px;
            color: #ffd700; /* 收藏时为黄色 */
        }

        .favourite-button.not-favourited .icon {
            color: #ccc; /* 未收藏时为灰色 */
        }

        .favourite-button::after {
            content: '收藏';
            position: absolute;
            top: -35px;
            left: 50%;
            transform: translateX(-50%);
            background-color: #333;
            color: white;
            padding: 5px 10px;
            border-radius: 5px;
            font-size: 14px;
            white-space: nowrap;
            opacity: 0;
            transition: opacity 0.2s ease;
            pointer-events: none;
        }

        .favourite-button:hover::after {
            opacity: 1;
        }

    /* 举报按钮样式 */
    .report-button-container .report-button {
        position: relative;
        width: 60px;
        height: 60px;
        font-size: 30px;
        background-color: white;
        border-radius: 20px;
        border: 0;
        box-shadow: 2px 3px 3px rgba(0, 0, 0, 0.05);
        transition: transform 0.2s ease, box-shadow 0.2s ease;
        cursor: pointer;
    }

        .report-button-container .report-button:hover {
            transform: translateY(-3px);
            box-shadow: 2px 5px 5px rgba(0, 0, 0, 0.1);
        }

        /* 举报按钮的图标 */
        .report-button-container .report-button .icon {
            font-size: 30px;
            color: inherit;
        }

        /* 举报按钮的气泡文字 */
        .report-button-container .report-button::after {
            content: '举报';
            position: absolute;
            top: -35px;
            left: 50%;
            transform: translateX(-50%);
            background-color: #333;
            color: white;
            padding: 5px 10px;
            border-radius: 5px;
            font-size: 14px;
            white-space: nowrap;
            opacity: 0;
            transition: opacity 0.2s ease;
            pointer-events: none;
        }

        .report-button-container .report-button:hover::after {
            opacity: 1;
        }


    .description {
        margin-bottom: 20px;
        font-weight: bold;
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
        font-weight: bold;
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
        //border-top: 1px solid #e5e5e5;
        margin-top: 30px;
        margin-left: 150px;
        margin-right: 150px;
        background-color: white;
        border: 1px solid #ccc;
        border-radius: 10px;
        padding: 30px;
    }

    .faq-header {
        display: flex;
        justify-content: space-between;
        align-items: center;
        cursor: pointer;
    }

    .faq-title {
        font-size: 18px;
        font-weight: bold;
        margin: 0;
    }

    .arrow img {
        width: 38px;
        height: 38px;
    }

    .faq-content {
        height: 100px;
        overflow: auto;
        transition: visibility 0.3s, opacity 0.3s;
    }

        .faq-content.hidden {
            visibility: hidden;
            opacity: 0;
        }

    .comments-container {
        margin-top: 30px;
        margin-left: 150px;
        margin-right: 150px;
        margin-bottom: 50px;
        background-color: white;
        border: 1px solid #ccc;
        border-radius: 10px;
        padding: 30px;
    }

    .comment-item {
        border-bottom: 1px solid #e0e0e0; /* 下边框作为分隔线 */
        padding: 10px 0; /* 为内容留出内边距 */
        margin-bottom: 10px; /* 每条评论之间留出一些空隙 */
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

    .buy-button {
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
        margin-top: 10px;
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

    /* 弹窗遮罩 */
    .modal-overlay {
        position: fixed;
        top: 0;
        left: 0;
        width: 100%;
        height: 100%;
        background-color: rgba(0, 0, 0, 0.6); /* 半透明暗色 */
        display: flex;
        align-items: center;
        justify-content: center;
        z-index: 1000;
    }

    /* 弹窗内容 */
    .modal-content {
        background-color: #fff;
        padding: 30px; /* 增加内边距 */
        border-radius: 10px;
        box-shadow: 0 4px 15px rgba(0, 0, 0, 0.3);
        text-align: center;
        z-index: 1001;
        width: 400px; /* 设置弹窗的宽度 */
        max-width: 90%; /* 确保在小屏幕上弹窗不会太大 */
    }

        .modal-content button {
            margin-top: 20px;
            padding: 10px 20px;
            background-color: #efc2bb;
            border: none;
            border-radius: 5px;
            cursor: pointer;
        }

            .modal-content button:hover {
                background-color: #e89a8d;
                transform: translateY(-2px);
            }
</style>
