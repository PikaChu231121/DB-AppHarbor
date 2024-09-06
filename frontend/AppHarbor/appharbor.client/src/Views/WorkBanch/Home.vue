<template>
    <Loading :loading="isLoading" />
    <div class="Home">
        <div class="header">
            <div class="title">{{ user_nickname }}的应用库</div>
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
            <div class="info-box" v-for="app in filteredApplications" :key="app.id" @click="appDetail(app.id)">
                <img :src="getAppImgUrl(app.image)" class="app-image" />
                <p class="app-name">{{ app.name }}</p>
                <!-- Use @click.stop to prevent the event from bubbling up to the parent div -->
                <button class="purchase-button" @click.stop="downloadApp(app.package, app.id)">下载</button>
            </div>
        </div>


        <!-- App detail modal -->
        <!--<div class="app-detail-modal" v-if="selectedApp">
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
    </div>-->
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
                axios.post(`${this.$Url}/api/user/userInfo`, { token: token })
                    .then(response => {
                        const data = response.data;
                        this.user_id = data.id;
                        this.user_nickname = data.nickname;
                        this.avatar_url = data.avatar ? `${this.$Url}${data.avatar}` : '../../public/default.png';
                        this.fetchApplications(token); // Fetch applications after fetching user data
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
            fetchApplications() {
                var token = Cookies.get('token');
                let formData = new FormData();
                formData.append('token', token);
                axios.post(`${this.$Url}/api/Order/fetchOwnApps`, formData)
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
            downloadApp(packageUrl, appId) {
                this.isLoading = true;

                // 发送请求到后端更新下载次数
                axios.post(`${this.$Url}/api/application/incrementDownloadCount`, { appId: appId })
                    .then(response => {
                        console.log('Download count incremented successfully:', response.data);
                    })
                    .catch(error => {
                        console.error('Error incrementing download count:', error);
                    })
                    .finally(() => {
                        // 处理下载逻辑
                        setTimeout(() => {
                            if (packageUrl) {
                                window.open(`${this.$Url}${packageUrl}`, '_blank');
                            } else {
                                console.error('Package URL is missing');
                            }
                            this.isLoading = false; // Hide loading animation
                        }, 2000); // Delay for 2 seconds
                    });
            },
            openAppDetail(app) {
                this.selectedApp = app;
            },
            closeAppDetail() {
                this.selectedApp = null;
            },
            getAppImgUrl(imgPath) {
                if (imgPath) {
                    return `${this.$Url}${imgPath}`;
                }
                return '../../public/default.png'; // 默认图片路径
            },
            appDetail(id) {
                this.$router.push(`/app/${id}`);
            }
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

    .auto-wrapper {
        display: flex;
        flex-wrap: wrap;
        justify-content: center;
        width: 100%;
        height: calc(100% - 150px);
        overflow-y: auto;
        padding: 10px;
        //transition: transform 0.3s ease, box-shadow 0.3s ease;
    }



    .info-box {
        display: flex;
        flex-direction: column;
        cursor: pointer;
        align-items: center;
        background: #fff9f9;
        border: 3px solid #ffd7d2;
        padding: 20px;
        margin: 15px;
        width: 220px;
        height: 240px;
        max-height: 280px;
        border-radius: 10px;
        box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
        transition: transform 0.3s ease, box-shadow 0.3s ease, background-color 1.0s;
    }

        .info-box:hover {
            background-color: #ffcece;
            transform: scale(1.05);
            box-shadow: 0 6px 12px rgba(0, 0, 0, 0.2);
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
        transition: background-color 0.3s ease, transform 0.3s ease, box-shadow 0.3s ease;
        margin-bottom: 15px;
        font-family: 'Microsoft YaHei', sans-serif; /* 设置字体为微软雅黑 */
        font-weight: bold; /* 加粗字体 */
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
        font-family: 'Microsoft YaHei', sans-serif; /* 设置字体为微软雅黑 */
        font-weight: bold; /* 加粗字体 */
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
        text-align: center; 
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
    }

        .app-detail-content p:first-of-type {
            font-size: 20px; /* Larger font size for app name */
            font-weight: bold; /* Bold font for app name */
            color: #333; /* Darker color for app name */
        }

    .category-dropdown {
        position: relative; /* 使用相对定位 */
        margin-top: 20px; /* 距离上一个组件的上边距 */
        margin-left: auto; /* 自动左边距以推到最右侧 */
        background-color: #fbb1a2; /* 背景颜色 */
        padding: 10px 15px; /* 内边距 */
        border-radius: 8px; /* 边框圆角 */
        box-shadow: 0 4px 8px rgba(0, 0, 0, 0.2); /* 阴影效果 */
        z-index: 100; /* 确保在其他元素之上 */
        display: flex;
        align-items: center;
        transition: transform 0.3s ease, box-shadow 0.3s ease;
    }

        .category-dropdown:hover {
            transform: translateY(-5px); /* 悬停时向上移动 */
            box-shadow: 0 8px 16px rgba(0, 0, 0, 0.3); /* 增强阴影效果 */
        }

        .category-dropdown label {
            margin-right: 10px;
            font-size: 14px; /* 较小的字体 */
            color: #fff; /* 标签文字颜色 */
            font-weight: bold;
        }

        .category-dropdown select {
            padding: 8px 10px;
            border: none; /* 去除边框 */
            border-radius: 5px;
            background-color: #fff; /* 设置背景颜色 */
            font-size: 14px; /* 较小的字体 */
            color: #333; /* 字体颜色 */
            cursor: pointer;
            box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1); /* 轻微阴影 */
            transition: background-color 0.3s ease, box-shadow 0.3s ease;
        }

            .category-dropdown select:hover {
                background-color: #f7e2dc; /* 悬停时的背景颜色 */
                box-shadow: 0 4px 8px rgba(0, 0, 0, 0.2); /* 增加阴影 */
            }

            .category-dropdown select:focus {
                outline: none; /* 去除聚焦时的默认边框 */
                box-shadow: 0 0 5px 2px rgba(251, 177, 162, 0.5); /* 聚焦时的阴影 */
            }


    .no-applications {
        font-size: 24px; 
        color: #d54f8c;
        text-align: center;
        margin-top: 20px;
        position: absolute; /* 绝对定位以居中 */
        top: 50%; /* 垂直居中 */
        left: 50%; /* 水平居中 */
        transform: translate(-50%, -50%); /* 调整位置以确保完全居中 */
    }

</style>

