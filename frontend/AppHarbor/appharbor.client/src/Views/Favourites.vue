<template>
    <div v-if="alert || confirm" class="notification-toast">
        <p>{{ alert || confirm }}</p>
    </div>
    <div class="favourite-list">
        <div class="header">
            <div class="title">{{ user_nickname }}的收藏夹</div>
            <div class="user-section">
                <div class="avatar-wrapper">
                    <img :src="avatar_url" class="avatar-circle" />
                </div>
                <div class="user-info">
                    <p class="user-nick">{{ user_nickname }}</p>
                    <p class="user-id">ID : {{ user_id }}</p>
                </div>
            </div>
        </div>

        <div class="filter-container">
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
        </div>

        <div class="bulk-actions">
            <button class="bulk-delete-toggle" @click="toggleBulkDelete">
                {{ isBulkDeleting ? '取消批量操作' : '批量操作' }}
            </button>
            <button v-if="isBulkDeleting" class="bulk-delete-confirm" @click="bulkDelete" :disabled="!selectedFavourites.length">
                批量取消收藏
            </button>
        </div>

        <div v-if="favourites.length" class="auto-wrapper">
            <div v-for="(favourite, index) in favourites"
                 :key="favourite.id"
                 class="info-box"
                 @click="!isBulkDeleting && goToDetail(favourite.applicationId)">
                <img :src="getAppImgUrl(favourite.appimage)" class="app-image" />
                <p class="app-name">{{ favourite.applicationName }}</p>
                <p class="app-category">{{ favourite.applicationCategory }}</p>
                <p class="app-info">收藏时间: {{ formatDate(favourite.createTime) }}</p>
                <div class="action-buttons">
                    <button class="favourite-button" @click.stop="deleteFavourite(favourite.applicationId)" :disabled="isBulkDeleting">取消收藏</button>
                    <input type="checkbox"
                           v-if="isBulkDeleting"
                           v-model="selectedFavourites"
                           :value="favourite.id"
                           class="bulk-delete-checkbox">
                </div>
            </div>
        </div>

        <div v-else class="no-applications">
            <p>{{ message }}</p>
        </div>

        <!-- Alert and Confirm Boxes -->
        <AlertBox v-if="alert" :message="alert" @close="alert = ''" />
        <ConfirmBox v-if="confirm" :message="confirm" @close="confirm = ''" />
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
                confirm: '',
                user_nickname: '',
                user_id: '',
                avatar_url: '',
            };
        },
        created() {
            this.fetchUser();
            this.fetchFavourites();
            this.resetNotifications();
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
            fetchUser() {
                const token = Cookies.get('token');
                axios.post(`${this.$Url}/api/user/userInfo`, { token })
                    .then(response => {
                        const data = response.data;
                        this.user_id = data.id;
                        this.user_nickname = data.nickname;
                        this.avatar_url = data.avatar ? `${this.$Url}${data.avatar}` : '../../public/default.png';
                    })
                    .catch(error => {
                        console.error('Error fetching user data:', error);
                        Cookies.remove('token');
                        this.$router.push('/').then(() => {
                            // 刷新登录页面
                            window.location.reload();
                        });
                        alert("token异常，请重新登陆！");
                    });
            },
            async fetchFavourites() {
                try {
                    const token = Cookies.get('token');
                    const formData = new FormData();
                    formData.append('token', token);
                    formData.append('categoryFilter', this.selectedCategory);
                    const response = await axios.post(`${this.$Url}/api/favourite/getfavourites`, formData);
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
                    const response = await axios.post(`${this.$Url}/api/favourite/deleteFavourite`, {
                        token: token,
                        id: id
                    });
                    const parsedData = response.data;
                    if (parsedData.success) {
                        this.favourites = this.favourites.filter(fav => fav.id !== id);
                        this.confirmNotification('取消收藏应用成功！');
                        this.fetchFavourites(); // 重新拉取收藏夹内容
                    } else {
                        this.alertNotification('取消收藏应用失败，请稍后重试！');
                    }
                } catch (error) {
                    this.alertNotification('取消收藏应用失败，请稍后重试！');
                    console.error('Error deleting favourite:', error);
                }
            },
            async bulkDelete() {
                try {
                    const token = Cookies.get('token');
                    const response = await axios.post(`${this.$Url}/api/favourite/bulkDelete`, {
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
                        this.alertNotification('批量取消收藏应用失败，请稍后重试！');
                    }
                } catch (error) {
                    this.alertNotification('批量取消收藏应用失败，请稍后重试！');
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
                    this.selectedFavourites = [];
                    this.confirmNotification('批量操作已取消'); // 取消批量操作时提示
                } // 清空已选中的应用
            },
            getAppImgUrl(imgPath) {
                if (imgPath) {
                    return `${this.$Url}${imgPath}`;
                }
                return '../../public/default.png'; // 默认图片路径
            },
            goToDetail(appId) {
                if (!this.isBulkDeleting) {
                    this.$router.push(`/app/${appId}`);
                }
            },
            resetNotifications() {
                this.alert = '';
                this.confirm = '';
            },
        }
    };
</script>

<style scoped>
    .favourite-list {
        width: 100%;
        height: 100%;
        display: flex;
        flex-direction: column;
        align-items: center;
    }

    .header {
        display: flex;
        justify-content: space-between;
        align-items: center;
        width: 100%;
        height: 90px;
        padding: 20px;
        border-bottom: 5px solid darksalmon;
    }

    .title {
        font-size: 50px;
        color: #f97c6c;
        font-weight: bold
    }

    .user-section {
        display: flex;
        align-items: center;
    }

    .avatar-wrapper {
        width: 60px;
        height: 60px;
        border-radius: 50%;
        overflow: hidden;
        margin-right: 20px;
    }

    .avatar-circle {
        width: 100%;
        height: 100%;
        object-fit: cover;
    }

    .user-info {
        display: flex;
        flex-direction: column;
    }

    .user-nick {
        font-size: 20px;
        font-weight: bold;
    }

    .user-id {
        font-size: 20px;
        color: #888;
        font-weight: bold;
    }

    .filter-container {
        position: relative;
        margin-top: 20px;
        margin-left: auto;
        background-color: #fbb1a2;
        padding: 10px 15px;
        border-radius: 8px;
        box-shadow: 0 4px 8px rgba(0, 0, 0, 0.2);
        z-index: 100;
        display: flex;
        align-items: center;
        transition: transform 0.3s ease, box-shadow 0.3s ease;
    }

        .filter-container:hover {
            transform: translateY(-5px);
            box-shadow: 0 8px 16px rgba(0, 0, 0, 0.3);
        }

        .filter-container label {
            margin-right: 10px;
            font-size: 14px;
            color: #fff;
            font-weight: bold;
        }

        .filter-container select {
            padding: 8px 10px;
            border: none;
            border-radius: 5px;
            background-color: #fff;
            font-size: 14px;
            color: #333;
            cursor: pointer;
            box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
            transition: background-color 0.3s ease, box-shadow 0.3s ease;
        }

            .filter-container select:hover {
                background-color: #f7e2dc;
                box-shadow: 0 4px 8px rgba(0, 0, 0, 0.2);
            }

            .filter-container select:focus {
                outline: none;
                box-shadow: 0 0 5px 2px rgba(251, 177, 162, 0.5);
            }

    .bulk-actions {
        display: flex;
        justify-content: center;
        align-items: center;
        margin-top: 20px;
    }

    .bulk-delete-toggle {
        padding: 10px 20px;
        margin-right: 10px;
        border: none;
        background-color: #fbb1a2;
        color: #fff;
        font-size: 16px;
        border-radius: 5px;
        cursor: pointer;
        transition: background-color 0.3s ease, box-shadow 0.3s ease;
    }

        .bulk-delete-toggle:hover {
            background-color: #f97c6c;
            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.2);
        }

    .bulk-delete-confirm {
        padding: 10px 20px;
        border: none;
        background-color: #f97c6c;
        color: #fff;
        font-size: 16px;
        border-radius: 5px;
        cursor: pointer;
        transition: background-color 0.3s ease, box-shadow 0.3s ease;
    }

        .bulk-delete-confirm:disabled {
            background-color: #ddd;
            cursor: not-allowed;
            box-shadow: none;
        }

    .auto-wrapper {
        display: flex;
        flex-wrap: wrap;
        justify-content: center;
        width: 100%;
        padding: 20px;
    }

    .info-box {
        width: 250px;
        margin: 10px;
        background: #fff9f9;
        border-radius: 10px;
        box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
        padding: 15px;
        text-align: center;
        transition: transform 0.3s ease-in-out, box-shadow 0.3s ease-in-out;
        cursor: pointer;
        position: relative;
    }

        .info-box:hover {
            transform: translateY(-5px);
            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.2);
        }

    .app-image {
        width: 100%;
        height: auto;
        border-radius: 5px;
        margin-bottom: 10px;
        transition: transform 0.3s ease;
    }

    .app-name {
        font-size: 25px;
        font-weight: bold;
        color: #333;
        margin-bottom: 5px;
    }

    .app-category {
        font-size: 16px;
        color: lightseagreen;
        background-color: whitesmoke;
        border-radius: 10px;
        padding: 2px 10px;
        display: inline-block;
        text-align: center;
        margin: 0 auto;
        font-weight: bold !important;
        margin-bottom: 10px;
    }

    .app-info {
        font-size: 0.9em;
        color: #888;
    }

    .no-applications {
        padding: 20px;
        color: #888;
        font-size: 1.2em;
    }

    .action-buttons {
        margin-top: 10px;
        position: relative;
    }

    .favourite-button {
        margin: 5px;
        padding: 10px 20px;
        border: none;
        background-color: #f97c6c;
        color: #fff;
        border-radius: 5px;
        cursor: pointer;
    }

    .bulk-delete-checkbox {
        position: absolute;
        top: -5px;
        right: -5px;
        width: 24px;
        height: 24px;
        transform: scale(1.5);
        cursor: pointer;
    }


    .notification-toast {
        position: fixed;
        bottom: 20px;
        left: 50%;
        transform: translateX(-50%);
        background-color: #3db999;
        color: white;
        padding: 10px 20px;
        border-radius: 5px;
        z-index: 1000;
        opacity: 0;
        animation: fadeInOut 4s ease forwards;
    }

    @keyframes fadeInOut {
        0% {
            opacity: 0;
        }

        10% {
            opacity: 1;
        }

        90% {
            opacity: 1;
        }

        100% {
            opacity: 0;
        }
    }

</style>
