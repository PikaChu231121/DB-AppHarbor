<template>
    <Loading :loading="isLoading" />

    <div class="Home">
        <div class="header">
            <div class="title">
                <svg xmlns="http://www.w3.org/2000/svg" width="1em" height="1em" viewBox="0 0 32 32"><path fill="currentColor" d="M16 4.906L3.625 10.063l-.625.28V27h26V10.344l-.625-.281zm0 2.188l11 4.593V25h-2V14H7v11H5V11.687zM9 16h14v9H9z" /></svg>
                {{ user_nickname }}的应用库
            </div>
            <div class="user-section">
                <div class="avatar-wrapper">
                    <img :src="avatar_url" class="avatar-circle" />
                </div>
                <div class="user-info">
                    <p class="user-nickname">{{ user_nickname }}</p>
                    <p class="user-id">用户ID：{{ user_id }}</p>
                </div>
            </div>
        </div>

        <!-- Add dropdown filter -->
        <div class="category-dropdown">
            <label for="category-select">选择应用种类:</label>
            <select id="category-select" v-model="selectedCategory" @change="filterApplications">
                <option v-for="category in categories" :key="category" :value="category">
                    {{ category }}
                </option>
            </select>
        </div>

        <!-- Display no applications message if filteredApplications is empty -->
        <div class="no-applications" v-if="filteredApplications.length === 0">
            <p>该分类下无应用</p>
        </div>

        <div class="auto-wrapper">
            <div class="info-box" v-for="app in filteredApplications" :key="app.id">
                <img :src="getAppImgUrl(app.Image)" class="app-image" />
                <p class="app-name">{{ app.name }}</p>
                <button class="purchase-button" @click="downloadApp(app.package)">下载</button>
                <button class="view-button" @click="openAppDetail(app)">查看应用</button>
            </div>
        </div>

        <!-- App detail modal -->
        <div class="app-detail-modal" v-if="selectedApp">
            <div class="app-detail-content">
                <button class="close-button" @click="closeAppDetail">×</button>
                <p style="color: #d54f8c;">
                    <svg xmlns="http://www.w3.org/2000/svg" width="1em" height="1em" viewBox="0 0 256 256"><path fill="currentColor" d="M244 160a12 12 0 0 1-12 12h-31.33l17.66 29.89a12 12 0 1 1-20.66 12.21L140.9 118a12 12 0 0 1 20.66-12.2l24.92 42.2H232a12 12 0 0 1 12 12m-110.85-12H97.39l64.94-109.89a12 12 0 1 0-20.66-12.21L128 49l-13.67-23.1a12 12 0 1 0-20.66 12.21l20.39 34.51L69.52 148H24a12 12 0 0 0 0 24h109.15a12 12 0 0 0 0-24m-74.32 41.67a12 12 0 0 0-16.43 4.22l-4.73 8a12 12 0 1 0 20.66 12.21l4.73-8a12 12 0 0 0-4.23-16.43" /></svg>
                    应用名称：{{ selectedApp.name }}
                </p>
                <p>版本：{{ selectedApp.version }}</p>
                <p>发行商：{{selectedApp.merchantName}}</p>
                <p>应用分类：{{ selectedApp.category }}</p>
                <p>介绍：{{ selectedApp.description }}</p>
            </div>
        </div>
    </div>
</template>

<script>
    import axios from 'axios';
    import Cookies from 'js-cookie';
    import Loading from '../Tools/Loading.vue';

    export default {
        components: {
            Loading,
        },
        data() {
            return {
                user_nickname: '',
                user_id: '',
                avatar_url: '',
                applications: [],
                filteredApplications: [], // Add this for filtered applications
                selectedApp: null, // Add this for selected app detail
                isLoading: false,
                selectedCategory: '全部', // Default to show all categories
                categories: ['全部', '娱乐', '社交', '购物', '健康养生', '办公', '学习']
            };
        },
        methods: {
            fetchUser() {
                var token = Cookies.get('token');
                axios.post('http://localhost:5118/api/user/userInfo', { token: token })
                    .then(response => {
                        const data = response.data;
                        this.user_id = data.id;
                        this.user_nickname = data.nickname;
                        this.avatar_url = data.avatar ? `http://localhost:5118${data.avatar}` : '../../public/default.png';
                        this.fetchApplications(token); // Fetch applications after fetching user data
                    })
                    .catch(error => {
                        console.error('Error fetching user data:', error);
                    });
            },
            fetchApplications() {
                var token = Cookies.get('token');
                let formData = new FormData();
                formData.append('token', token);
                axios.post('http://localhost:5118/api/Order/fetchOwnApps', formData)
                    .then(response => {
                        this.applications = response.data.$values;
                        this.filterApplications(); // Filter applications based on the selected category
                    })
                    .catch(error => {
                        console.error('Error fetching applications:', error);
                    });
            },
            filterApplications() {
                if (this.selectedCategory === '全部') {
                    this.filteredApplications = this.applications;
                } else {
                    this.filteredApplications = this.applications.filter(app => app.category === this.selectedCategory);
                }
            },
            downloadApp(packageUrl) {
                this.isLoading = true;

                setTimeout(() => {
                    if (packageUrl) {
                        window.open(packageUrl, '_blank');
                    } else {
                        console.error('Package URL is missing');
                    }
                    this.isLoading = false; // Hide loading animation

                }, 2000); // Delay for 2 seconds
            },
            openAppDetail(app) {
                this.selectedApp = app;
            },
            closeAppDetail() {
                this.selectedApp = null;
            },
            getAppImgUrl(imgPath) {
                if (imgPath) {
                    return `http://localhost:5118${imgPath}`;
                }
                return '../../public/default.png'; // 默认图片路径
            },
        },
        mounted() {
            this.fetchUser();
        }
    };
</script>

<style scoped>
    .Home {
        display: flex;
        flex-direction: column;
        align-items: center;
        width: 100%;
        height: 100%;
        font-family: 'Arial', sans-serif;
    }

    .header {
        display: flex;
        justify-content: space-between;
        align-items: center;
        width: 100%;
        padding: 20px;
        background-color: #ffd7d2;
        box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
        border-radius: 10px;
    }

    .title {
        font-size: 24px;
        color: #000;
        font-weight: bold;
        font-family: 'Comic Sans MS', cursive, sans-serif;
    }

    .user-section {
        display: flex;
        align-items: center;
    }

    .avatar-wrapper {
        width: 50px;
        height: 50px;
        border-radius: 50%;
        overflow: hidden;
        margin-right: 10px;
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

    .user-nickname {
        font-size: 16px;
        font-weight: bold;
        font-family: 'Comic Sans MS', cursive, sans-serif;
    }

    .user-id {
        font-size: 14px;
        color: #888;
        font-family: 'Comic Sans MS', cursive, sans-serif;
    }

    .auto-wrapper {
        display: flex;
        flex-wrap: wrap;
        justify-content: center;
        width: 100%;
        height: calc(100% - 150px);
        overflow-y: auto;
        padding: 10px;
    }

    .info-box {
        display: flex;
        flex-direction: column;
        align-items: center;
        background: #fff9f9;
        border: 3px solid #ffd7d2;
        padding: 20px;
        margin: 15px;
        width: 220px;
        height: 280px;
        max-height: 280px;
        border-radius: 10px;
        box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
        transition: transform 0.3s ease, box-shadow 0.3s ease;
    }

    .app-image {
        width: 100px;
        height: 100px;
        object-fit: cover;
        border-radius: 10px;
    }

    .app-name {
        font-size: 18px;
        font-weight: bold;
        margin: 8px 0;
        text-align: center;
        overflow: hidden;
        white-space: nowrap;
        text-overflow: ellipsis;
        font-family: 'Comic Sans MS', cursive, sans-serif;
    }

    .purchase-button {
        background-color: #fbb1a2;
        color: white;
        border: none;
        border-radius: 20px;
        padding: 10px 20px;
        border-radius: 5px;
        cursor: pointer;
        box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
        font-family: 'Comic Sans MS', cursive, sans-serif;
        transition: background-color 0.3s ease, transform 0.3s ease, box-shadow 0.3s ease;
        margin-bottom: 15px;
    }

        .purchase-button:hover {
            background-color: #f97c6c;
            transform: scale(1.05);
            box-shadow: 0 6px 12px rgba(0, 0, 0, 0.2);
        }

    .view-button {
        background-color: #e0a9b6;
        color: white;
        border: none;
        border-radius: 20px;
        box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
        padding: 10px 20px;
        border-radius: 5px;
        cursor: pointer;
        transition: background-color 0.3s ease, transform 0.3s ease, box-shadow 0.3s ease;
        font-family: 'Comic Sans MS', cursive, sans-serif;
    }

        .view-button:hover {
            background-color: #d88b9f;
            transform: scale(1.05);
            box-shadow: 0 6px 12px rgba(0, 0, 0, 0.2);
        }

    .app-detail-modal {
        position: fixed;
        top: 0;
        left: 0;
        width: 100%;
        height: 100%;
        background: rgba(0, 0, 0, 0.5);
        display: flex;
        justify-content: center;
        align-items: center;
        z-index: 1000;
    }

    .app-detail-content {
        background: #fff;
        border-radius: 10px;
        padding: 20px;
        width: 80%;
        max-width: 600px;
        box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
        position: relative;
        text-align: center; /* Center-align text in the modal */
    }

    .close-button {
        position: absolute;
        top: 10px;
        right: 10px;
        background: #fbb1a2;
        border: none;
        color: white;
        font-size: 20px;
        width: 30px;
        height: 30px;
        border-radius: 50%;
        display: flex;
        align-items: center;
        justify-content: center;
        cursor: pointer;
        transition: background-color 0.3s ease, transform 0.3s ease, box-shadow 0.3s ease;
        box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
    }

        .close-button:hover {
            background-color: #f97c6c;
            transform: scale(1.1);
            box-shadow: 0 6px 12px rgba(0, 0, 0, 0.2);
        }

        .close-button:active {
            transform: scale(0.9);
            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
        }

    .app-detail-content p {
        margin: 10px 0;
        font-size: 16px;
        font-family: 'Comic Sans MS', cursive, sans-serif;
    }

        .app-detail-content p:first-of-type {
            font-size: 20px; /* Larger font size for app name */
            font-weight: bold; /* Bold font for app name */
            color: #333; /* Darker color for app name */
        }

    .category-dropdown {
        margin: 20px 0;
        display: flex;
        justify-content: center;
        align-items: center;
    }

        .category-dropdown label {
            margin-right: 10px;
            font-size: 16px;
            color: #333;
            font-family: 'Comic Sans MS', cursive, sans-serif;
        }

        .category-dropdown select {
            padding: 10px;
            border: 1px solid #ddd;
            border-radius: 5px;
            background: #fff;
            font-size: 16px;
            color: #333;
            transition: border-color 0.3s ease;
        }

            .category-dropdown select:focus {
                border-color: #fbb1a2;
                outline: none;
            }

    .no-applications {
        font-size: 24px; /* 增大字体 */
        color: #d54f8c; /* 使用深粉色，或者选择一个可爱的颜色 */
        text-align: center;
        margin-top: 20px;
        font-family: 'Comic Sans MS', cursive, sans-serif; /* 使用可爱的字体 */
        position: absolute; /* 绝对定位以居中 */
        top: 50%; /* 垂直居中 */
        left: 50%; /* 水平居中 */
        transform: translate(-50%, -50%); /* 调整位置以确保完全居中 */
    }

</style>

