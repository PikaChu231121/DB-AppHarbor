<template>
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

        <div v-if="favourites.length" class="auto-wrapper">
            <div v-for="(favourite, index) in favourites" :key="favourite.id" class="info-box" @click="goToDetail(favourite.applicationId)">
                <img :src="getAppImgUrl(favourite.appAvatar)" class="app-image" />
                <p class="app-name">{{ favourite.applicationName }}</p>
                <p class="app-category">{{ favourite.applicationCategory }}</p>
                <p class="app-info">收藏时间: {{ formatDate(favourite.createTime) }}</p>
                <div class="action-buttons">
                    <button class="favourite-button" @click.stop="deleteFavourite(favourite.applicationId)" :disabled="isBulkDeleting">取消收藏</button>
                    <input type="checkbox" v-if="isBulkDeleting" v-model="selectedFavourites" :value="favourite.id" class="bulk-delete-checkbox">
                </div>
            </div>
        </div>

        <div v-else class="no-applications">
            <p>{{ message }}</p>
        </div>
    </div>
</template>

<script>
    import axios from 'axios';
    import Cookies from 'js-cookie';

    export default {
        data() {
            return {
                favourites: [],
                message: '加载中...',
                isBulkDeleting: false,
                selectedFavourites: [],
                selectedCategory: 'all',
                user_nickname: '',
                user_id: '',
                avatar_url: '',
            };
        },
        created() {
            this.fetchUser();
            this.fetchFavourites();
        },
        methods: {
            fetchUser() {
                const token = Cookies.get('token');
                axios.post('http://localhost:5118/api/user/userInfo', { token })
                    .then(response => {
                        const data = response.data;
                        this.user_id = data.id;
                        this.user_nickname = data.nickname;
                        this.avatar_url = data.avatar ? `http://localhost:5118${data.avatar}` : '../../public/default.png';
                    })
                    .catch(error => {
                        console.error('Error fetching user data:', error);
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
            deleteFavourite(id) {
                // Implementation remains unchanged
            },
            bulkDelete() {
                // Implementation remains unchanged
            },
            filterByCategory() {
                this.fetchFavourites();
            },
            getAppImgUrl(imgPath) {
                if (imgPath) {
                    return `http://localhost:5118${imgPath}`;
                }
                return '../../public/default.png'; // 默认图片路径
            },
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
            goToDetail(appId) {
                this.$router.push(`/app/${appId}`);
            }
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
        height :90px;
        padding: 20px;
        border-bottom: 5px solid darksalmon; 
    }

    .title {
        font-size: 50px;
        color: #f97c6c;
        font-weight: bold;
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
        width: 100%;
        padding: 10px;
        text-align: left;
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
        padding: 2px 10px; /* 增加左右内边距，确保文本不贴边 */
        display: inline-block; /* 让标签宽度自动适应内容 */
        text-align: center; /* 居中对齐文本 */
        margin: 0 auto; /* 确保在父容器内居中 */
        font-weight: bold !important; /* 设置粗体 */
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
        margin-top: 10px;
    }
</style>
