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
    <div class="comments-container">
        <div v-for="comment in comments" :key="comment.id" class="comment-item">
            <img :src="comment.user.avatar" alt="Avatar" class="avatar">
            <div class="info">
                <span class="nickname">{{ comment.user.nickname }}</span>
                <p class="content">{{ comment.content }}</p>
            </div>
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
                //Appname: "Keep",
                //Classification: "健身",
                //Price: "70",
                //DetailIntro: "「Keep」是一款健身App，超过2亿运动爱好者的选择！无论是想减肥塑形或增肌，还是寻找健身跑步瑜伽计步等训练计划，你可以随时随地选择课程进行训练！",
                isFAQOpen: true,

                comments: [
                    {
                        id: 1,
                        user: { avatar: 'https://randomuser.me/api/portraits/men/2.jpg', nickname: 'Kobe Bryant' },
                        content: 'Man ! What can I say ? Mamba out!',
                    },
                    {
                        id: 2,
                        user: { avatar: 'https://randomuser.me/api/portraits/women/2.jpg', nickname: 'Mamba' },
                        content: '沙克也干了',
                    },
                ],
                isFavourited:false // 是否已经收藏，默认未收藏
            };
        },
        created() {
            const appId = this.$route.params.id;
            this.fetchAppDetails(appId);
            this.checkIfFavourite(appId);
        },
        methods: {
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
                    } else {
                        alert('收藏失败：' + parsedData.msg);
                    }
                })
                .catch(error=>{
                    console.error('Error adding favourite:',error);
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
                
            }
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
        margin-left:auto;
        margin-right:auto;
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

    .nickname {
        font-weight: bold;
    }

    .content {
        margin-top: 5px;
    }

</style>
