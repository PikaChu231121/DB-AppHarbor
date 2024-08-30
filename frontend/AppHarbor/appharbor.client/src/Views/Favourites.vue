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
                <button @click="toggleBulkDelete">{{ isBulkDeleting ? '取消批量删除': '批量删除' }}</button>
                <button @click="bulkDelete" :disabled="!isBulkDeleting || selectedFavourites.length === 0">取消收藏选中应用</button>
            </div>
            <div v-if="favourites.length" class="favourite-grid">
                <div v-for="(favourite,index) in favourites" :key="favourite.id" class="favourite-item">
                    <h3>
                        <router-link :to="{ name: 'AppDetail', params: { id: favourite.applicationId } }">
                            {{ favourite.applicationName }}
                        </router-link>
                    </h3>
                    <p>收藏时间: {{ formatDate(favourite.createTime)  }}</p>
                    <p>分类: {{ favourite.applicationCategory  }}</p>
                    <p>id: {{ favourite.applicationId }}</p>
                    <div class="action-buttons">
                        <button @click="deleteFavourite(favourite.applicationId)" :disabled="isBulkDeleting">取消收藏</button>
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
        methods: {
            formatDate(dateTime) {
                const date = new Date(dateTime);
                date.setHours(date.getHours() + 8);
                return date.toLocaleString('zh-CN', {
                    year: 'numeric',
                    month: '2-digit',
                    day: '2-digit',
                    hour: '2-digit',
                    minute: '2-digit',
                    second: '2-digit',
                    hour12: false
                });
            },
            async fetchFavourites() {
                try {
                    const token = Cookies.get('token');
                    const formData = new FormData();
                    formData.append('token', token);
                    formData.append('categoryFilter', this.selectedCategory);
                    const response = await axios.post('http://localhost:5118/api/favourite/getfavourites', formData);
                    const parsedData = JSON.parse(response.data);

                    if (parsedData && parsedData.Favourites) {
                        this.favourites = parsedData.Favourites;
                        this.message = '';
                        if (this.favourites.length === 0) {
                            this.message = this.selectedCategory === 'all'
                                ? '您还没有收藏的应用哦，去商店逛逛吧'
                                : '当前分类下暂无收藏应用哦，去商店逛逛吧';
                        }
                    } else {
                        this.favourites = [];
                        this.message = '您还没有收藏的应用哦，去商店逛逛吧';
                    }
                } catch (error) {
                    this.message = '加载收藏夹失败，请稍后重试！';
                    console.error('Error fetching favourites:', error);
                }
            },
            async deleteFavourite(id) {
                try {
                    const token = Cookies.get('token');
                    const response = await axios.post('http://localhost:5118/api/favourite/deleteFavourite', {
                        token: token,
                        id: id
                    });
                    const parsedData = response.data;
                    if (parsedData.success) {
                        this.favourites = this.favourites.filter(fav => fav.id !== id);
                        this.confirmNotification('取消收藏应用收藏成功！');
                        this.fetchFavourites(); // 重新拉取收藏夹内容
                    } else {
                        this.alertNotification('取消收藏应用收藏失败，请稍后重试！');
                    }
                } catch (error) {
                    this.alertNotification('取消收藏应用收藏失败，请稍后重试！');
                    console.error('Error deleting favourite:', error);
                }
            },
            async bulkDelete() {
                try {
                    const token = Cookies.get('token');
                    const response = await axios.post('http://localhost:5118/api/favourite/bulkDelete', {
                        token: token,
                        ids: this.selectedFavourites
                    });
                    const parsedData = JSON.parse(response.data);
                    if (parsedData.success) {
                        this.favourites = this.favourites.filter(fav => !this.selectedFavourites.includes(fav.id));
                        this.selectedFavourites = []; // 清空选中项
                        this.isBulkDeleting = false; // 重置批量删除状态
                        this.confirmNotification('批量取消收藏应用成功');
                        this.fetchFavourites(); // 重新拉取收藏夹内容
                    } else {
                        this.alertNotification('批量取消收藏应用收藏失败，请稍后重试！');
                    }
                } catch (error) {
                    this.alertNotification('批量取消收藏应用收藏失败，请稍后重试！');
                    console.error('Error bulk deleting favourites:', error);
                }
            },
            alertNotification(message) {
                this.notify('alert', message);
            },
            confirmNotification(message) {
                this.notify('confirm', message);
            },
            notify(type, message) {
                this[type] = '';
                this.$nextTick(() => {
                    this[type] = message;
                });
            },
            filterByCategory() {
                console.log("Selected category:", this.selectedCategory); // 确认选中的种类是否正确
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
