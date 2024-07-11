<template>
    <div class="favourite-list">
        <h1>个人收藏夹</h1>
        <ul v-if="favourites.length">
            <li v-for="favourite in favourites" :key="favourite.id">
                <h3>ApplicationId: {{ favourite.applicationId }}</h3>
                <p>CreateTime: {{ favourite.createTime }}</p>
                <p>Visibility: {{ favourite.visibility }}</p>
                <p>UserId: {{ favourite.userId }}</p>
                <button @click="deleteFavourite(favourite.id)">删除</button>
            </li>
        </ul>
        <div v-else>
            <p>{{ message }}</p>
        </div>
        <div v-if="notification" class="notification">{{ notification }}</div>
    </div>
</template>

<script>
    import axios from 'axios';
    import Cookies from 'js-cookie';

    export default {
        name: 'FavouriteList',
        data() {
            return {
                favourites: [],
                message: '加载中...',
                notification: '' // 通知用户删除情况
            };
        },
        created() {
            this.fetchFavourites();
        },
        methods: {
            fetchFavourites() {
                var token = Cookies.get('token');
                axios.post('http://localhost:5118/api/favourite/getfavourites', {
                    token: token
                })
                    .then(response => {
                        console.log("API response data:", response.data); // 输出 API 响应的数据
                        const parsedData = JSON.parse(response.data);
                        if (parsedData && parsedData.Favourites) {
                            this.favourites = parsedData.Favourites;
                            this.message = ''; // 清空消息
                            console.log("Favourites array:", this.favourites);
                        } else {
                            this.message = '您还没有收藏的应用哦，去商店逛逛吧';
                            console.error('Error: Expected Favourites but got:', response.data);
                        }
                    })
                    .catch(error => {
                        this.message = '您还没有收藏的应用哦，去商店逛逛吧';
                        console.error('Error fetching favourites:', error);
                    });
            },
            deleteFavourite(id) {
                var token = Cookies.get('token');
                axios.post('http://localhost:5118/api/favourite/deleteFavourite', {
                    token: token,
                    id: id
                })
                    .then(response => {
                        const parsedData = JSON.parse(response.data);
                        if (parsedData.success) {
                            this.favourites = this.favourites.filter(fav => fav.id !== id);
                            console.log("Delete successful:", parsedData);
                            this.showNotification('删除应用成功');
                            this.fetchFavourites(); // 重新拉取收藏夹内容
                        } else {
                            console.error('Delete failed:', parsedData);
                        }
                    })
                    .catch(error => {
                        console.error('Error deleting favourite:', error);
                    });
            },
            showNotification(message) {
                this.notification = message;
                setTimeout(() => {
                    this.notification = '';
                }, 2000);
            }
        }
    };
</script>

<style scoped>
    .favourite-list {
        max-width: 600px;
        margin: 0 auto;
        padding: 40px;
        background-color: #fff;
        border-radius: 8px;
        box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
        text-align:center;
    }

    h1 {
        margin-bottom: 20px;
        font-size: 32px;
        color: #333;
        text-align: center;
    }

    ul {
        list-style: none;
        padding: 0;
    }

    li {
        padding: 20px;
        border-bottom: 1px solid #ccc;
    }

        li:last-child {
            border-bottom: none;
        }

    h3 {
        margin: 0;
        font-size: 24px;
        color: #007bff;
    }

    p {
        margin: 5px 0;
        font-size: 16px;
        color: #666;
    }

    button {
        margin-top: 10px;
        padding: 5px 10px;
        font-size: 14px;
        cursor: pointer;
    }
    .notification {
        margin-top: 20px;
        padding: 10px;
        background-color: #dff0d8;
        color: #3c763d;
        border: 1px solid #d6e9c6;
        border-radius: 4px;
        text-align: center;
    }
</style>
