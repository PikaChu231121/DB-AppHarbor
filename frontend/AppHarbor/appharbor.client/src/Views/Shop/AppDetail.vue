<template>
    <div class="card">
        <div class="button-container">
            <button @click="goBack">Back to Shop</button>
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
            <p class="description">Text</p>
            <button class="button" @click="goToPurchase(app.id)">购买</button>
            <button class="button" @click="toggleFavourite">{{ isFavourited ? '取消收藏' : '收藏' }}</button>
            <!--<button class="button" @click="installapp">收藏</button>--> <!-- 添加收藏按钮 -->

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
            <img :src="getAvatarUrl(comment.user.avatar)" alt="Avatar" class="avatar">
            <div class="info">
                <span class="nickname">{{ comment.user.nickname }}</span>
                <div class="rating">
                    <span v-for="star in 5" :key="star" class="star" :class="{ filled: star <= comment.rating }">&#9733;</span>
                </div>
                <p class="content">{{ comment.content }}</p>
                <span class="timestamp">{{ comment.timestamp }}</span>
            </div>
        </div>
        <div class="comment-editor">
            <textarea v-model="newComment.content" placeholder="输入评论内容"></textarea>
            <div class="rating">
                <span v-for="star in 5" :key="star" class="star" :class="{ filled: star <= newComment.rating }" @click="setRating(star)">&#9733;</span>
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
                user: null,

                comments: [
                    {
                        id: 1,
                        user: { avatar: 'https://randomuser.me/api/portraits/men/2.jpg', nickname: 'Kobe Bryant' },
                        rating: 5,
                        content: 'Man ! What can I say ? Mamba out!',
                        timestamp: '2024-07-28 12:00'
                    },
                    {
                        id: 2,
                        user: { avatar: 'https://randomuser.me/api/portraits/women/2.jpg', nickname: 'Mamba' },
                        rating: 4,
                        content: '沙克也干了',
                        timestamp: '2024-07-29 14:30'
                    },
                ],
                newComment: {
                    content: '',
                    rating: 0
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
                // 从API或其他地方获取应用详细信息
                axios.post('http://localhost:5118/api/application/getappdetail', {
                    Id: appId
                    /*Page: this.currentPage */
                })
                    .then(response => {
                        this.app = response.data;
                        //console.log(this.app.price);
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
                .then(response =>{
                    const parsedData = response.data;
                    if (parsedData.success) {
                        alert('收藏成功'); // 可能需要更改样式
                        this.isFavourited = true; 
                    } else {
                        alert('收藏失败：' + parsedData.msg);
                    }
                })
                .catch(error=>{
                    console.error('Error adding favourite:',error);
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
            checkIfFavourite(appId){
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
                    //------------------TOKEN
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
                    console.log("12");
                    console.log(this.comments[0].id);
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
            setRating(rating) {
                // 设置评分
                this.newComment.rating = rating;
            },
            submitComment() {
                const token = Cookies.get('token');
                const newComment = {
                    user: {
                        avatar: this.user.avatar,
                        nickname: this.user.nickname,
                    },
                    content: this.newComment.content, // 评论内容
                    rating: this.newComment.rating, // 评分(1-5)
                    timestamp: new Date().toLocaleString() // 发布时间
                };
                this.comments.push(newComment);
                // Here you should add the logic to send the new comment to the server

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
    .card {
        display: flex;
        border: 1px solid #e5e5e5;
        border-radius: 8px;
        overflow: hidden;
        max-width: 800px;
        background-color: #fff;

        margin-left:auto;
        margin-right:auto;
        margin-top:auto;
        margin-bottom:auto;
    }

    .button-container {
        flex-direction: column;
    }

    .image-placeholder {
        width: 50%;
        height: 100%;
        background-color: #ffffff;
        display: flex;
        justify-content: center;
        position: relative;
    }

    .image {
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
        margin-bottom: 10px;
    }

    .tag {
        background-color: #e0f7ea;
        color: #4caf50;
        padding: 4px 8px;
        margin-bottom: 10px;
        font-size: 16px;
        border-radius: 8px;
        width: 48px;
    }

    .price {
        font-size: 28px;
        font-weight: bold;
        margin-bottom: 10px;
    }

    .description {
        margin-bottom: 20px;
    }

    .button {
        display: inline-block;
        background-color: #000;
        color: #fff;
        padding: 10px 20px;
        border: none;
        border-radius: 4px;
        cursor: pointer;
        margin-bottom: 20px;
    }

    .purchase-button {
        display: inline-block;
        background-color: #000;
        color: #fff;
        padding: 10px 20px;
        border: none;
        border-radius: 4px;
        cursor: pointer;
        margin-bottom: 20px;
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
        margin: 0;
    }

    .arrow img {
        width: 38px;
        height: 38px;
    }

    .faq-content {
        height: 100px; /* 固定高度 */
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

    .rating {
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

    .timestamp {
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

    .comment-editor .rating {
        margin-bottom: 10px;
    }

</style>
