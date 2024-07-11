<template>
    <div class="favourite-list">
        <alert-box :message="notification"></alert-box>
        <h1>个人收藏夹</h1>
        <div class="user-info">
            <div class="delete-button-group">
                <button @click="toggleBulkDelete">{{ isBulkDeleting ? '退出批量删除模式' : '批量删除' }}</button>
                <button @click="bulkDelete" :disabled="!isBulkDeleting || selectedFavourites.length === 0">删除选中应用</button>
            </div>
            <div v-if="favourites.length" class="favourite-grid">
                <div v-for="favourite in favourites" :key="favourite.id" class="favourite-item">
                    <h3>ApplicationId: {{ favourite.applicationId }}</h3>
                    <p>CreateTime: {{ favourite.createTime }}</p>
                    <p>Visibility: {{ favourite.visibility }}</p>
                    <div class="action-buttons">
                        <button @click="deleteFavourite(favourite.id)" :disabled="isBulkDeleting">删除</button>
                        <input type="checkbox" v-if="isBulkDeleting" v-model="selectedFavourites" :value="favourite.id" class="bulk-delete-checkbox">
                    </div>
                </div>
            </div>
            <div v-else>
                <p>{{ message }}</p>
            </div>
        </div>
    </div>
</template>


<script>
    import axios from 'axios';
    import Cookies from 'js-cookie';
    import AlertBox from './AlertBox.vue';

    export default {
        name: 'FavouriteList',
        components: {
            AlertBox
        },
        data() {
            return {
                favourites: [],
                message: '加载中...',
                notification: '', // 通知用户删除情况
                isBulkDeleting: false,
                selectedFavourites: []
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
            },
            toggleBulkDelete() {
                this.isBulkDeleting = !this.isBulkDeleting;
                if (!this.isBulkDeleting) {
                    this.selectedFavourites = []; // 清空已选中的应用
                }
            },
            bulkDelete() {
                var token = Cookies.get('token');
                axios.post('http://localhost:5118/api/favourite/bulkDelete', {
                    token: token,
                    ids: this.selectedFavourites
                })
                    .then(response => {
                        const parsedData = JSON.parse(response.data);
                        if (parsedData.success) {
                            this.favourites = this.favourites.filter(fav => !this.selectedFavourites.includes(fav.id));
                            console.log("Bulk delete successful:", parsedData);
                            this.showNotification('批量删除应用成功');
                            this.fetchFavourites(); // 重新拉取收藏夹内容
                        } else {
                            console.error('Bulk delete failed:', parsedData);
                        }
                    })
                    .catch(error => {
                        console.error('Error bulk deleting favourites:', error);
                    });
            }
        }
    };
</script>

<style scoped>
    .favourite-list {
        max-width: 700px;
        min-height: 780px;
        margin: 0 auto;
        padding: 40px;
        background-color: #fff;
        border-radius: 8px;
        box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
        text-align: center;
    }

    h1 {
        margin-bottom: 20px;
        font-size: 32px;
        color: #333;
        text-align: center;
        border: 3px solid #F3C7BA;
        border-radius: 8px;
    }

    .user-info {
        display: flex;
        flex-direction: column;
        gap: 20px;
    }

    .favourite-grid {
        display: grid;
        grid-template-columns: repeat(2, 1fr); /* 固定两列布局 */
        gap: 20px;
    }

    .favourite-item {
        padding: 20px;
        border: 1px solid #F3C7BA;
        border-radius: 8px;
        box-shadow: 0 10px 10px rgba(0, 0, 0, 0.1);
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
        padding: 10px 20px;
        background-color: #fbeaea;
        font-size: 15px;
        color: #F8887D;
        border: 3px solid #FADAD6;
        border-radius: 10px;
        cursor: pointer;
    }

        button:hover:enabled {
            background-color: #ffe5e5;
            transform: scale(1.05);
            color: #F8887D;
        }

        button:disabled {
            cursor: not-allowed;
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

    .delete-button-group {
        display: flex;
        justify-content: space-between;
        margin-bottom: 20px;
    }

    .action-buttons {
        display: flex;
        align-items: center;
        gap: 10px; 
    }
    .bulk-delete-checkbox {
        width: 20px; 
        height: 20px; 
    }
</style>