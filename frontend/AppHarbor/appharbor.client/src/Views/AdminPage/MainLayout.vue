
<template>
    <div class="main-layout">
        <div class="sidebar">
            <div @click="toggleSection('appManagement')" class="menu-item section-header">
                应用管理
            </div>
            <div v-show="sections.appManagement" class="section-content">
                <div class="menu">
                    <div class="menu-item"
                         :class="{ active: selectedStatus === '待审核应用' }"
                         @click="selectseleasing">待审核</div>
                    <div class="menu-item"
                         :class="{ active: selectedStatus === '已审核应用' }"
                         @click="selectseleased">已审核</div>
                </div>
            </div>

            <div @click="toggleSection('userManagement')" class="menu-item section-header">
                用户管理
            </div>
            <div v-show="sections.userManagement" class="section-content">
                <div class="menu">
                    <div class="menu-item"
                         :class="{ active: selectedStatus === '封禁用户' }"
                         @click="searchbanuser">封禁用户</div>
                    <div class="menu-item"
                         :class="{ active: selectedStatus === '活跃用户' }"
                         @click="searchunbanuser">活跃用户</div>
                </div>
            </div>


            <div @click="toggleSection('comment')" class="menu-item section-header">
                评论
            </div>
            <div v-show="sections.comment" class="section-content">
                <!-- 评论相关内容 -->
            </div>
        </div>

        <div class="main-content">
            <div v-if="loading" class="loading">加载中...</div>
            <div v-if="error" class="error">加载失败: {{ error.message }}</div>
            <div v-if="!loading && !error" class="status-display">
                <h2>{{ selectedStatus }}</h2>
            </div>
            <div v-if="section==='appManagement'" class="app-list">
                <div v-for="item in items" :key="item.id" class="app-item">
                    <div class="app-header">
                        <h3>{{ item.name }}</h3>
                        <span class="app-actions">
                            <button v-if="selectedStatus === '待审核应用'" @click="handleShelve(item)" class="action-button">上架应用</button>
                            <button @click="showDetails(item)" class="action-button">查看应用</button>
                        </span>
                    </div>
                </div>
            </div>

            <div v-if="section==='userManagement'&&!userstate" class="app-list">
                <div v-for="user in users" :key="user.id" class="app-item">
                    <div class="user-header">
                        <h3>用户ID：{{ user.userId }}</h3>
                        <p>封禁操作执行ID：{{ user.adminId }}</p>
                        <p>封禁时间：{{ user.time }}</p>
                        <p>封建原因：{{ user.reason }}</p>

                    </div>
                    <div class="app-actions">
                        <button @click="handleShelve(item)" class="action-button">上架</button>
                        <button @click="handleView(item)" class="action-button">查看</button>
                    </div>
                </div>
            </div>
            <div v-if="section==='userManagement'&&userstate" class="app-list">
                <div v-for="user in users" :key="user.id" class="app-item">
                    <div class="user-header">
                        <h3>用户ID：{{ user.id }}</h3>
                        <p>用户昵称：{{ user.nickname }}</p>
                        <p>账号注册时间：{{ user.registerTime }}</p>
                    </div>
                    <div class="app-actions">
                        <button @click="handleShelve(item)" class="action-button">封禁</button>
                        <button @click="handleView(item)" class="action-button">查看</button>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- 弹窗 -->
    <div v-if="showPopup" class="popup-overlay" @click="closePopup">
        <div class="popup-content" @click.stop>
            <h3>&nbsp;&nbsp;&nbsp;{{ selectedApp.name }}</h3>
            <p>&nbsp;&nbsp;应用版本: &nbsp;&nbsp;&nbsp;{{ selectedApp.version }}</p>
            <p>&nbsp;&nbsp;应用类型:&nbsp;&nbsp;&nbsp; {{ selectedApp.category }}</p>
            <p>&nbsp;&nbsp;应用ID: &nbsp;&nbsp;&nbsp;{{ selectedApp.id }}</p>
            <p>&nbsp;&nbsp;应用价格:&nbsp;&nbsp;&nbsp; {{ selectedApp.price === 0 ? '免费' : selectedApp.price }}</p>
            <p>&nbsp;&nbsp;应用描述: &nbsp;&nbsp;&nbsp;{{ selectedApp.description }}</p>
            <button @click="closePopup" class="popup-close-button">关闭</button>
        </div>
    </div>
</template>


<script>
    import axios from 'axios';
    import Cookies from 'js-cookie';

    export default {
        name: "MainLayout",
        data() {
            return {
                items: [],
                users: [],
                loading: false,
                error: null,
                userstate: 0,
                section: null,
                selectedStatus: '请在右侧选择你要查看的应用状态', // 默认状态
                sections: {
                    appManagement: false,
                    userManagement: false,
                    comment: false,
                },
                showPopup: false,
                selectedApp: null,
            };
        },
        methods: {
            toggleSection(section) {
                this.section = section;
                this.sections[section] = !this.sections[section];
            },
            selectseleasing() {
                this.selectedStatus = '待审核应用';
                this.fetchData('http://localhost:5118/api/application/selectseleasing');
            },
            selectseleased() {
                this.selectedStatus = '已审核应用';
                const token = Cookies.get('token');
                const formData = new FormData();
                formData.append('token', token);
                this.fetchData('http://localhost:5118/api/application/selectseleased', formData);
            },
            searchbanuser() {
                this.selectedStatus = '封禁用户';
                this.userstate = 0;
                axios.post('http://localhost:5118/api/banuser/searchbanuser')
                    .then(response => {
                        this.users = response.data;
                    })
                    .catch(error => {
                        this.error = error;
                    })
                    .finally(() => {
                        this.loading = false;
                    });
            },
            searchunbanuser() {
                this.selectedStatus = '活跃用户';
                this.userstate = 1;
                axios.post('http://localhost:5118/api/user/searchunbanuser')
                    .then(response => {
                        this.users = response.data.$values;
                    })
                    .catch(error => {
                        this.error = error;
                    })
                    .finally(() => {
                        this.loading = false;
                    });
            },
            fetchData(url, data = null) {
                this.loading = true;
                this.error = null;
                axios.post(url, data)
                    .then(response => {
                        this.items = response.data.$values;
                    })
                    .catch(error => {
                        this.error = error;
                    })
                    .finally(() => {
                        this.loading = false;
                    });
            },
            handleShelve(item) {
                const token = Cookies.get('token');
                if (!token) {
                    alert('未提供 token');
                    return;
                }

                const formData = new FormData();
                formData.append('Id', item.id);
                formData.append('token', token);

                axios.post('http://localhost:5118/api/application/confirmrelease', formData)
                    .then(response => {
                        alert('应用已通过审核');
                        // 更新列表或执行其他操作
                        this.fetchData('http://localhost:5118/api/application/selectseleasing');
                    })
                    .catch(error => {
                        console.error('审核失败:', error);
                        alert('审核失败，请重试');
                    });
            },
            showDetails(item) {
                this.selectedApp = item;
                this.showPopup = true;
            },
            closePopup() {
                this.showPopup = false;
                this.selectedApp = null;
            }
        }
    };
</script>

<style scoped>
    @import url('https://fonts.googleapis.com/css2?family=Poppins:wght@300;400;600&display=swap');

    .main-layout {
        display: flex;
        height: 100%;
        width: 100%;
    }

    .sidebar {
        width: 250px;
        background-color: #f4f4f4;
        padding: 20px;
        border-right: 2px solid #e0e0e0;
        box-sizing: border-box;
        overflow-y: auto;
    }

    .section-header {
        padding: 12px;
        cursor: pointer;
        font-weight: 600;
        color: #6a1b9a;
        transition: background-color 0.3s ease, color 0.3s ease;
        font-size: 18px;
        font-family: 'Poppins', sans-serif;
        border-radius: 8px;
        background-color: #fff;
        margin-bottom: 10px;
    }

        .section-header:hover {
            background-color: #e1bee7;
            color: #4a0072;
        }

    .menu-item.active {
        background-color: #6a1b9a;
        color: #fff;
    }

    .section-content {
        padding-left: 10px;
        padding-top: 5px;
    }

    .menu-item {
        padding: 10px;
        border-radius: 8px;
        cursor: pointer;
        transition: background-color 0.3s ease, color 0.3s ease;
        font-family: 'Poppins', sans-serif;
    }

        .menu-item:hover {
            background-color: #f3e5f5;
        }

    .main-content {
        padding: 20px;
        flex-grow: 1;
        background-color: #fff;
        box-sizing: border-box;
        overflow-y: auto;
    }

    .status-display {
        margin-bottom: 20px;
    }

        .status-display h2 {
            margin: 0;
            font-size: 28px;
            color: #6a1b9a;
            font-family: 'Poppins', sans-serif;
            font-weight: 600;
        }

    .app-list {
        max-height: calc(100vh - 80px);
    }

    .app-item {
        border: 1px solid #ddd;
        border-radius: 12px;
        padding: 20px;
        margin-bottom: 15px;
        box-shadow: 0 6px 12px rgba(0, 0, 0, 0.1);
        transition: box-shadow 0.3s ease, transform 0.3s ease;
        background-color: #f9f5ff;
        font-family: 'Poppins', sans-serif;
    }

        .app-item:hover {
            box-shadow: 0 8px 16px rgba(0, 0, 0, 0.15);
            transform: scale(1.03);
        }

    .app-header {
        display: flex;
        justify-content: space-between;
        align-items: center;
        margin-bottom: 15px;
        font-size: 18px;
    }

        .app-header h3 {
            margin: 0;
            font-size: 22px;
            color: #6a1b9a;
            font-family: 'Poppins', sans-serif;
            font-weight: 600;
        }

        .app-header p {
            margin: 5px 0;
            font-size: 16px;
            font-family: 'Poppins', sans-serif;
        }

    .app-actions {
        display: flex;
        gap: 12px;
    }

    .action-button {
        background-color: #6a1b9a;
        color: #fff;
        border: none;
        border-radius: 8px;
        padding: 10px 20px;
        cursor: pointer;
        font-weight: 600;
        font-size: 14px;
        transition: background-color 0.3s ease, transform 0.2s ease;
        font-family: 'Poppins', sans-serif;
    }

        .action-button:hover {
            background-color: #4a0072;
            transform: scale(1.05);
        }

    .loading, .error {
        color: #d32f2f;
        font-weight: 600;
        font-size: 16px;
        font-family: 'Poppins', sans-serif;
    }

    /* 弹窗样式 */
    .popup-overlay {
        position: fixed;
        top: 0;
        left: 0;
        width: 100%;
        height: 100%;
        background: rgba(0, 0, 0, 0.3); /* Slight dark overlay */
        display: flex;
        align-items: center;
        justify-content: center;
        z-index: 1000;
        animation: fadeIn 0.3s ease-in;
    }

    .popup-content {
        background-color: #fff;
        padding: 20px;
        border-radius: 12px;
        box-shadow: 0 8px 16px rgba(0, 0, 0, 0.2);
        max-width: 500px;
        width: 100%;
        position: relative;
        animation: scaleIn 0.3s ease-out;
    }

        .popup-content h3 {
            margin-top: 0;
            font-size: 26px;
            color: #6a1b9a; /* Purple color */
            font-family: 'Poppins', sans-serif;
            font-weight: 600;
        }

        .popup-content p {
            margin: 10px 0;
            font-size: 18px;
            color: #333;
            font-family: 'Poppins', sans-serif;
        }

    .popup-close-button {
        background-color: #6a1b9a; /* Purple color */
        color: #fff;
        border: none;
        border-radius: 12px;
        padding: 10px 20px;
        cursor: pointer;
        font-weight: 600;
        font-size: 16px;
        transition: background-color 0.3s ease, transform 0.2s ease;
        position: absolute;
        bottom: 20px;
        right: 20px;
        box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
    }

        .popup-close-button:hover {
            background-color: #4a0072; /* Darker purple */
            transform: scale(1.1);
        }

    /* Animations */
    @keyframes fadeIn {
        from {
            opacity: 0;
        }

        to {
            opacity: 1;
        }
    }

    @keyframes scaleIn {
        from {
            transform: scale(0.8);
        }

        to {
            transform: scale(1);
        }
    }
</style>
