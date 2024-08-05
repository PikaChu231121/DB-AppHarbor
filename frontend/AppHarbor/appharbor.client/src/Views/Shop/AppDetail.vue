<template>
    <div class="card">
        <div class="button-container">
            <button class="back-button" @click="goBack">Back to Shop</button>
        </div>
        <!--图片信息-->
        <div class="image-placeholder">
            <img :src="app.image" :alt="app.name" class="app-image" />
        </div>
        <!--应用详情-->
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
        </div>
    </div>
    <!--评论区域-->
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
</template>


<script>
    import axios from 'axios';
    import Cookies from 'js-cookie';
    export default {

        data() {
            return {
                app: null,
                isFAQOpen: true,

                comments: [],
                newComment: {
                    content: '',
                    score: 0
                },

                isFavourited:false // 是否已经收藏，默认未收藏
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

            /*------------------和显示应用详情有关的方法-------------------*/
            toggleFAQ() {
                this.isFAQOpen = !this.isFAQOpen;
            },
            fetchAppDetails(appId) {
                axios.post('http://localhost:5118/api/application/getappdetail', {
                    Id: appId
                })
                    .then(response => {
                        this.app = response.data;
                    })
                    .catch(error => {
                        console.error("Error fetching apps:", error);
                    });
            },
            goBack() {
                this.$router.push('/WorkBanchPage');
            },
            goToPurchase(appId) {
                this.$router.push(`/Purchase/${appId}`);
            },

            /*------------------和收藏有关的方法-------------------*/
            addFavourite() {
                const token = Cookies.get('token');
                axios.post('http://localhost:5118/api/favourite/addFavourite', {
                    token: token,
                    id: this.app.id
                })
                    .then(response => {
                        const parsedData = response.data;
                        if (parsedData.success) {
                            alert('收藏成功');
                            this.isFavourited = true;
                        } else {
                            alert('收藏失败：' + parsedData.msg);
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
                            alert('取消收藏成功');
                            this.isFavourited = false;
                        } else {
                            alert('取消收藏失败：' + parsedData.msg);
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
            installapp() {
                console.log('downloading: ' + this.app.id);
                axios.post('http://localhost:5118/api/application/installapp', {
                    Id: this.app.id
                })
                    .then(response => {
                        window.location.href = `http://localhost:5118${response.data}`;
                        console.log('downloaded: ' + this.app.id);
                    })
                    .catch(error => {
                        console.error("Error install:", error);
                    });
            },

            /*------------------和评论有关的方法-------------------*/
            fetchAllComments(appId) {
                // 在这里获取该应用的全部评论
                axios.post('http://localhost:5118/api/comment/getappcomment', {
                    ApplicationId: appId
                })
                .then(response => {
                    this.comments = response.data.$values;
                    console.log("12 length of comments is "+this.comments.length);
                    console.log(this.comments[0].id);
                    console.log(this.comments[1].id);
                })
                .catch(error => {
                    console.error('Error fetching app comments:', error);
                    console.log("21");
                });
            },
            fetchUserInfo() {
                // 获取用户个人信息，便于发布评论
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
                // 设置评分
                this.newComment.score = score;
            },
            submitComment() {
                const token = Cookies.get('token');
                /*
                const newComment = {
                    user: {
                        avatar: this.user.avatar,
                        nickname: this.user.nickname,
                    },
                    content: this.newComment.content, // 评论内容
                    score: this.newComment.score, // 评分(1-5)
                    publishTime: new Date().toLocaleString() // 发布时间
                };
                this.comments.push(newComment);
                */
                // Here you should add the logic to send the new comment to the server
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
                        // 将新评论添加到评论列表中
                        this.comments.push({
                            id: parsedData.commentId, // 服务器返回的新评论ID
                            content: this.newComment.content,
                            score: this.newComment.score,
                            avatar: this.user.avatar,
                            nickname: this.user.nickname,
                            publishTime: new Date().toLocaleString() // 注意这是个假的时间
                        });
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
        }
    };
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
        background-color: #ffffff;
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

</style>
