<template>
    <div class="favourite-list">
        <h1>个人收藏夹</h1>
        <ul v-if="favourites.length">
            <li v-for="favourite in favourites" :key="favourite.appId">
                <h3>ApplicationId: {{ favourite.appId }}</h3>
                <p>Name: {{ favourite.appName }}</p>
                <p>Visibility: {{ favourite.appVisibility }}</p>
                <p>UserId: {{ favourite.userId }}</p>
            </li>
        </ul>
        <div v-else>
            <p>{{ message }}</p>
        </div>
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
                message: '加载中...'
            }
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
                            this.message = '当前用户无收藏夹！';
                            console.error('Error: Expected Favourites but got:', response.data);
                        }
                    })
                    .catch(error => {
                        this.message = '当前用户无收藏夹！';
                        console.error('Error fetching favourites:', error);
                    });
            }
        }
    }
</script>

<style scoped>
    .favourite-list {
        max-width: 600px;
        margin: 0 auto;
        padding: 40px;
        background-color: #fff;
        border-radius: 8px;
        box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
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
</style>
