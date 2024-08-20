<template>
    <div class="favourite-list">
        <alert-box :msg="alert"></alert-box>
        <confirm-box :msg="confirm"></confirm-box>
        <h1>我的收藏夹</h1>

        <!-- 应用种类筛选选择框 -->
        <label for="categoryFilter">选择应用种类：</label>
        <select id="categoryFilter" @change="filterByCategory" v-model="selectedCategory">
            <option value="all">全部</option>
            <option value="娱乐">娱乐</option>
            <option value="社交">社交</option>
            <option value="购物">购物</option>
            <option value="健康养生">健康养生</option>
            <option value="办公">办公</option>
            <option value="教育">教育</option>
        </select>

        <div class="user-info">
            <div class="delete-button-group">
                <button @click="toggleBulkDelete">{{ isBulkDeleting ? '取消批量删除' : '批量删除' }}</button>
                <button @click="bulkDelete" :disabled="!isBulkDeleting || selectedFavourites.length === 0">删除选中应用</button>
            </div>
            <div v-if="favourites.length" class="favourite-grid">
                <div v-for="(favourite,index) in favourites" :key="favourite.id" class="favourite-item">
                    <h3>
                        <router-link :to="{ name: 'AppDetail', params: { id: favourite.applicationId } }">
                            {{ favourite.applicationName }}
                        </router-link>
                    </h3>
                    <p>收藏时间: {{formattedCreateTime[index] }}</p>
                    <p>分类: {{ favourite.applicationCategory  }}</p>
                    <p>id: {{ favourite.applicationId }}</p>
                    <div class="action-buttons">
                        <button @click="deleteFavourite(favourite.applicationId)" :disabled="isBulkDeleting">删除</button>
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
    import ConfirmBox from './ConfirmBox.vue';

    export default {
        name: 'FavouriteList',
        components: {
            AlertBox,
            ConfirmBox
        },
        data() {
            return {
                favourites: [],
                message: '加载中...',
                isBulkDeleting: false,
                selectedFavourites: [],
                selectedCategory: 'all',
                alert: '',
                confirm:''
            };
        },
        created() {
            this.fetchFavourites();
        },
        computed: {
            formattedCreateTime() {
                return this.favourites.map(favourite => {
                    let dateTime = favourite.createTime;
                    // 替换"T"为空格
                    dateTime = dateTime.replace('T', ' ');
                    // 转换为 Date 对象
                    let date = new Date(dateTime);
                    // 增加8小时
                    date.setHours(date.getHours() + 8);
                    // 格式化为 YYYY-MM-DD HH:mm:ss
                    let year = date.getFullYear();
                    let month = ('0' + (date.getMonth() + 1)).slice(-2);
                    let day = ('0' + date.getDate()).slice(-2);
                    let hours = ('0' + date.getHours()).slice(-2);
                    let minutes = ('0' + date.getMinutes()).slice(-2);
                    let seconds = ('0' + date.getSeconds()).slice(-2);
                    return `${year}-${month}-${day} ${hours}:${minutes}:${seconds}`;
                });
            }  
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
                        this.message = '加载收藏夹失败，请稍后重试！';
                        console.error('Error fetching favourites:', error);
                    });
            },
            deleteFavourite(id) {
                var token = Cookies.get('token');
                console.log(id);
                axios.post('http://localhost:5118/api/favourite/deleteFavourite', {
                    token: token,
                    id: id
                })
                    .then(response => {
                        const parsedData = response.data;
                        if (parsedData.success) {
                            this.favourites = this.favourites.filter(fav => fav.id !== id);
                            console.log("Delete successful:", parsedData);
                            this.confirmNotification('删除应用收藏成功！');
                            this.fetchFavourites(); // 重新拉取收藏夹内容
                        } else {
                            this.alertNotification('删除应用收藏失败，请稍后重试！');
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
                            this.selectedFavourites = []; // 清空选中项
                            this.isBulkDeleting = false; // 重置批量删除状态
                            console.log("Bulk delete successful:", parsedData);
                            this.confirmNotification('批量删除应用成功');
                            this.fetchFavourites(); // 重新拉取收藏夹内容
                        } else {
                            this.alertNotification('批量删除应用收藏失败，请稍后重试！');
                            console.error('Bulk delete failed:', parsedData);
                        }
                    })
                    .catch(error => {
                        console.error('Error bulk deleting favourites:', error);
                    });
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
        transition: transform 0.3s ease, box-shadow 0.3s ease; /* 过渡效果 */
    }

    .favourite-item:hover {
        transform: translateY(-5px); /* 向上浮动 */
        box-shadow: 0 15px 20px rgba(0, 0, 0, 0.2); /* 更大的阴影效果 */
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
        margin-left: 30px;
    }

    .bulk-delete-checkbox {
        width: 20px;
        height: 20px;
        margin-top:10px;
        margin-left: 60px;
    }
</style>
