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
                isFavourited: false // 是否已经收藏，默认未收藏
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
            }
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
        margin: auto;
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
</style>
