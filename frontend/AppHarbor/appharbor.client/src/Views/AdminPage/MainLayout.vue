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
                         :class="{ active: selectedStatus === '未封禁用户' }"
                         @click="searchunbanuser">活跃用户</div>
                </div>
            </div>

            <div @click="toggleSection('comment')" class="menu-item section-header">
                评论
            </div>
            <div v-show="sections.comment" class="section-content">
<!--                <div class="menu">
                    <div class="menu-item"
                         :class="{ active: selectedStatus === '待审核应用' }"
                         @click="selectseleasing">已审核评论</div>
                    <div class="menu-item"
                         :class="{ active: selectedStatus === '已审核应用' }"
                         @click="selectseleased">未审核评论</div>
                </div>-->

            </div>
        </div>

        <div class="main-content">
            <div v-if="loading" class="loading">加载中...</div>
            <div v-if="error" class="error">加载失败: {{ error.message }}</div>
            <div v-if="!loading && !error" class="status-display">
                <h2>{{ selectedStatus }}</h2>
            </div>
            <div class="app-list">
                <div v-for="item in items" :key="item.id" class="app-item">
                    <div class="app-header">
                        <h3>{{ item.name }}</h3>
                        <p>版本: {{ item.version }}</p>
                        <p>类型: {{ item.category }}</p>
                        <p>ID: {{ item.id }}</p>
                        <p>价格: {{ item.price === 0 ? '免费' : item.price }}</p> <!-- 更新后的价格显示 -->
                        <p>描述: {{ item.description }}</p>
                    </div>
                    <div class="app-actions">
                        <button @click="handleShelve(item)" class="action-button">上架</button>
                        <button @click="handleView(item)" class="action-button">查看</button>
                    </div>
                </div>
            </div>
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
                selectedStatus: '请在右侧选择你要查看的应用状态', // 默认状态
                sections: {
                    appManagement: false,
                    userManagement: false,
                    comment: false,
                },
            };
        },
        methods: {
            toggleSection(section) {
                this.sections[section]=!this.sections[section];
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
                this.fetchUserData('http://localhost:5118/api/banuser/searchbanuser');
            },
            searchunbanuser() {
                this.selectedStatus = '未封禁用户';
                this.fetchUserData('http://localhost:5118/api/user/searchunbanuser');
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
            fetchUserData(url, data = null) {
                this.loading = true;
                this.error = null;
                axios.post(url, data)
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
            handleShelve(item) {
                console.log('上架:', item);
            },
            handleView(item) {
                console.log('查看:', item);
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
</style>
