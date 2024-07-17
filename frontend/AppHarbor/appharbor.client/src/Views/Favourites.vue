<template>
    <div class="favourite-list">
        <alert-box :message="notification"></alert-box>
        <h1>个人收藏夹</h1>

        <!-- 应用种类筛选选择框 -->
        <label for="categoryFilter">选择应用种类：</label>
        <select id="categoryFilter" @change="filterByCategory" v-model="selectedCategory">
            <option value="all">全部</option>
            <option value="Office">办公</option>
            <option value="Social">社交</option>
            <option value="Program">Program</option>
        </select>

        <div class="user-info">
            <div class="delete-button-group">
                <button @click="toggleBulkDelete">{{ isBulkDeleting ? '取消批量删除' : '批量删除' }}</button>
                <button @click="bulkDelete" :disabled="!isBulkDeleting || selectedFavourites.length === 0">删除选中应用</button>
            </div>
            <div v-if="favourites.length" class="favourite-grid">
                <div v-for="favourite in favourites" :key="favourite.id" class="favourite-item">
                    <h3>
                        <router-link :to="{ name: 'AppDetail', params: { id: favourite.applicationId } }">
                            应用名称: {{ favourite.applicationName }}
                        </router-link>
                    </h3>
                    <p>收藏时间: {{ favourite.createTime }}</p>
                    <p>可见性: {{ favourite.visibility }}</p>
                    <p>分类: {{ favourite.applicationCategory  }}</p>
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
                notification: '',
                isBulkDeleting: false,
                selectedFavourites: [],
                selectedCategory: 'all'
            };
        },
        created() {
            this.fetchFavourites();
        },
        methods: {
            fetchFavourites() {
                const token = Cookies.get('token');
                const formData = new FormData();
                formData.append('token', token);
                formData.append('categoryFilter', this.selectedCategory);
                axios.post('http://localhost:5118/api/favourite/getfavourites', formData)
                    .then(response => {
                        console.log("API response data:", response.data); // 获得 API 响应的数据

                        const parsedData = JSON.parse(response.data);

                        if (parsedData && parsedData.Favourites) {
                            this.favourites = parsedData.Favourites;
                            this.message = ''; // 清空消息
                            console.log("Favourites array:", this.favourites);

                            if (this.favourites.length === 0) {
                                this.message = this.selectedCategory === 'all'
                                    ? '您还没有收藏的应用哦，去商店逛逛吧'
                                    : '当前分类下暂无收藏应用哦，去商店逛逛吧';
                            }
                        } else {
                            this.favourites = [];
                            this.message = '您还没有收藏的应用哦，去商店逛逛吧';
                            console.error('Error: Expected Favourites but got:', response.data);
                        }
                    })
                    .catch(error => {
                        this.message = '加载收藏夹失败，请稍后再试';
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
            },
            showNotification(message) {
                this.notification = message;
                setTimeout(() => {
                    this.notification = '';
                }, 2000);
            },
            filterByCategory() {
                console.log("Selected category:", this.selectedCategory); // 调试信息，确认选中的种类是否正确
                this.fetchFavourites();
            },
            toggleBulkDelete() {
                this.isBulkDeleting = !this.isBulkDeleting;
                if (!this.isBulkDeleting) {
                    this.selectedFavourites = []; // 清空已选中的应用
                }
            }
        }
    };
</script>

<style scoped>
    .favourite-list {
        max-width: 800px;
        min-height: 600px;
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
        align-items: center;
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
        transition: all 0.3s ease; /* 过渡效果 */
    }

        button:hover:enabled {
            background-color: #ffe5e5;
            transform: scale(1.05);
            color: #F8887D;
        }

        button:disabled {
            cursor: not-allowed;
            opacity: 0.7; /* 减少透明度表示不可用状态 */
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
        justify-content: space-around;
        margin-bottom: 20px;
        width: 100%;
        max-width: 600px;
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


<!--商店添加收藏按钮-->
<!--        <button @click="toggleFavourite(app.id)">
            {{ isFavourite(app.id) ? '取消收藏' : '收藏' }}
        </button>-->
<!--export default {
    props: {
        app: {
            type: Object,
            required: true
        }
    },
    methods: {
        toggleFavourite(id) {
            const token = Cookies.get('token');
            const action = this.isFavourite(id) ? 'deleteFavourite' : 'addFavourite';
            axios.post(`http://localhost:5118/api/favourite/${action}`, {
                token: token,
                applicationId: id
            })
            .then(response => {
                const parsedData = JSON.parse(response.data);
                if (parsedData.success) {
                    this.$emit('updateFavourites'); // 触发父组件更新收藏夹内容
                } else {
                    console.error('Toggle favourite failed:', parsedData);
                }
            })
            .catch(error => {
                console.error('Error toggling favourite:', error);
            });
        },
        isFavourite(id) {
            // 检查当前应用是否已经在收藏夹中
            return this.favourites.some(fav => fav.applicationId === id);
        }
    }
};-->
<!--主界面监听点击事件-->
<!--<template>
    <div class="store">
        <store-app v-for="app in apps" :key="app.id" :app="app" @updateFavourites="fetchFavourites" />
    </div>
</template>

<script>
import StoreApp from './StoreApp.vue';
import axios from 'axios';
import Cookies from 'js-cookie';

export default {
    components: {
        StoreApp
    },
    data() {
        return {
            apps: [],
            favourites: []
        };
    },
    created() {
        this.fetchApps();
        this.fetchFavourites();
    },
    methods: {
        fetchApps() {
            // 拉取商店中的应用列表
            axios.get('http://localhost:5118/api/store/apps')
                .then(response => {
                    this.apps = response.data;
                })
                .catch(error => {
                    console.error('Error fetching apps:', error);
                });
        },
        fetchFavourites() {
            // 拉取用户的收藏夹内容
            const token = Cookies.get('token');
            axios.post('http://localhost:5118/api/favourite/getfavourites', {
                token: token
            })
            .then(response => {
                const parsedData = JSON.parse(response.data);
                if (parsedData && parsedData.Favourites) {
                    this.favourites = parsedData.Favourites;
                    console.log("Favourites array:", this.favourites);
                } else {
                    console.error('Error: Expected Favourites but got:', response.data);
                }
            })
            .catch(error => {
                console.error('Error fetching favourites:', error);
            });
        }
    }
};
</script>
-->
