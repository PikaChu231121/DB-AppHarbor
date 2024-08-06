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

            <div v-if="section==='userManagement' && !userstate" class="app-list">
                <div v-for="user in users" :key="user.id" class="app-item">
                    <div class="user-header">
                        <h3>用户ID：{{ user.userId }}</h3>
                        <p>封禁操作执行ID：{{ user.adminId }}</p>
                        <p>封禁时间：{{ user.time }}</p>
                        <p>封禁原因：{{ user.reason }}</p>
                    </div>
                    <div class="app-actions">
                        <!-- 只保留一个解除封禁按钮 -->
                        <button @click="handleUnban(user)" class="action-button">解除封禁</button>
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
                        <button @click="handleBan(user)" class="action-button">封禁</button>
                    </div>
                </div>
            </div>

            <!-- 封禁成功弹窗 -->
            <div v-if="showBanSuccessPopup" class="popup-overlay" @click="closeBanSuccessPopup">
                <div class="popup-content success-popup" @click.stop>
                    <h3>封禁成功</h3>
                    <p>该用户已成功封禁！</p>
                    <button @click="closeBanSuccessPopup" class="popup-close-button">关闭</button>
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


    <!-- 确认弹窗 -->
    <div v-if="showConfirmPopup" class="popup-overlay" @click="cancelShelve">
        <div class="popup-content confirm-popup" @click.stop>
            <h3>确认上架</h3>
            <p>您确定要上架 {{ appToShelve ? appToShelve.name : '' }} 应用吗？</p>
            <div class="confirm-buttons">
                <button @click="confirmShelve" class="popup-confirm-button">确定</button>
                <button @click="cancelShelve" class="popup-cancel-button">取消</button>
            </div>
        </div>
    </div>

    <div v-if="showSuccessPopup" class="popup-overlay" @click="closeSuccessPopup">
        <div class="popup-content success-popup" @click.stop>
            <h3>审核成功</h3>
            <p>应用 {{ appToShelve ? appToShelve.name : '' }} 已成功上架！</p>
            <button @click="closeSuccessPopup" class="popup-close-button">关闭</button>
        </div>
    </div>

    <!-- 封禁确认弹窗 -->
    <div v-if="showBanConfirmPopup" class="popup-overlay" @click="cancelBan">
        <div class="popup-content ban-confirm-popup" @click.stop>
            <h3>确认封禁</h3>
            <p>请填写封禁理由：</p>
            <textarea v-model="banReason" rows="4" placeholder="请输入封禁理由"></textarea>
            <div class="confirm-buttons">
                <button @click="confirmBan" class="popup-confirm-button">确定封禁</button>
                <button @click="cancelBan" class="popup-cancel-button">取消</button>
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
                userstate: 0,
                section: null,
                selectedStatus: '请在右侧选择你要查看的应用状态',
                sections: {
                    appManagement: false,
                    userManagement: false,
                    comment: false,
                },
                showPopup: false,
                showConfirmPopup: false,
                showSuccessPopup: false,
                showBanConfirmPopup: false,
                showBanSuccessPopup: false,
                selectedApp: null,
                appToShelve: null,
                selectedUser: null,
                banReason: '',
                userToBan: null,
            };
        },
        methods: {
            handleBan(user) {
                this.userToBan = user;
                this.showBanConfirmPopup = true;
            },
            confirmBan() {
                const token = Cookies.get('token');
                if (!token) {
                    alert('未提供 token');
                    return;
                }

                const formData = new FormData();
                formData.append('mytoken', token);
                formData.append('user_id', this.userToBan.id);
                formData.append('reason', this.banReason);

                axios.post('http://localhost:5118/api/BanUser/banuser', formData)
                    .then(response => {
                        this.showBanConfirmPopup = false;
                        this.banReason = '';
                        this.userToBan = null;
                        this.showBanSuccessPopup = true;
                        this.searchbanuser();
                    })
                    .catch(error => {
                        console.error('封禁失败:', error);
                        alert('封禁失败，请重试');
                    });
            },
            cancelBan() {
                this.showBanConfirmPopup = false;
                this.banReason = '';
                this.userToBan = null;
            },
            closeBanSuccessPopup() {
                this.showBanSuccessPopup = false;
            },
            closeUserPopup() {
                this.showUserPopup = false;
                this.selectedUser = null;
            },
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
                this.appToShelve = item; // 保存要上架的应用信息
                this.showConfirmPopup = true; // 显示确认弹窗
            },

            confirmShelve() {
                const token = Cookies.get('token');
                if (!token) {
                    alert('未提供 token');
                    return;
                }

                const formData = new FormData();
                formData.append('Id', this.appToShelve.id);
                formData.append('token', token);
                axios.post('http://localhost:5118/api/application/confirmrelease', formData)
                    .then(response => {
                        this.showConfirmPopup = false; // 隐藏确认弹窗
                        this.showSuccessPopup = true; // 显示成功弹窗
                        this.fetchData('http://localhost:5118/api/application/selectseleasing');
                    })
                    .catch(error => {
                        console.error('审核失败:', error);
                        alert('审核失败，请重试');
                    })
                    .finally(() => {
                        this.appToShelve = null; // 清除应用信息
                    });
            },

            cancelShelve() {
                this.showConfirmPopup = false; // 取消上架操作，隐藏确认弹窗
                this.appToShelve = null; // 清除应用信息
            },

            showDetails(item) {
                this.selectedApp = item;
                this.showPopup = true;
            },
            closePopup() {
                this.showPopup = false;
                this.selectedApp = null;
            },
            closeSuccessPopup() {
                this.showSuccessPopup = false;
                this.appToShelve = null;
            }
        }
    };
</script>


<style scoped>
    @import url('https://fonts.googleapis.com/css2?family=Poppins:wght@300;400;600&display=swap');

    .ban-confirm-popup {
        max-width: 400px;
        padding: 20px;
    }

        .ban-confirm-popup textarea {
            width: 100%;
            padding: 10px;
            margin-bottom: 20px;
            border-radius: 8px;
            border: 1px solid #ddd;
            box-sizing: border-box;
            font-family: 'Poppins', sans-serif;
        }

    .confirm-buttons {
        display: flex;
        justify-content: flex-end;
        gap: 10px;
    }

    .popup-confirm-button, .popup-cancel-button {
        background: linear-gradient(135deg, #6a1b9a, #9c27b0);
        color: #fff;
        border: none;
        border-radius: 12px;
        padding: 12px 24px;
        cursor: pointer;
        font-weight: 600;
        font-size: 16px;
        transition: background 0.3s ease, transform 0.3s ease, box-shadow 0.3s ease;
        font-family: 'Poppins', sans-serif;
        box-shadow: 0 4px 8px rgba(0, 0, 0, 0.2);
    }

        .popup-confirm-button:hover {
            background: linear-gradient(135deg, #9c27b0, #6a1b9a);
            transform: scale(1.05);
        }

    .popup-cancel-button {
        background: linear-gradient(135deg, #f44336, #e53935);
    }

        .popup-cancel-button:hover {
            background: linear-gradient(135deg, #e53935, #f44336);
            transform: scale(1.05);
        }

    .user-popup {
        max-width: 400px;
        padding: 20px;
        background-color: #fff;
        color: #333;
    }

    .user-avatar {
        width: 100px;
        height: 100px;
        border-radius: 50%;
        display: block;
        margin: 0 auto 10px;
    }

    .user-popup h3, .user-popup p {
        margin: 10px 0;
        font-family: 'Poppins', sans-serif;
    }

    .user-popup .popup-close-button {
        background-color: #6a1b9a;
        color: #fff;
        border: none;
        border-radius: 12px;
        padding: 10px 20px;
        cursor: pointer;
        font-weight: 600;
        font-size: 16px;
        transition: background-color 0.3s ease, transform 0.2s ease;
        margin-top: 15px;
    }

        .user-popup .popup-close-button:hover {
            background-color: #4a0072;
        }

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
        justify-content: flex-end;
        gap: 12px;
    }

    /* 按钮样式更新 */
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
        box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
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

    .confirm-popup {
        max-width: 400px;
        padding: 20px;
    }

    .confirm-buttons {
        display: flex;
        justify-content: flex-end;
        gap: 10px;
    }

    .popup-confirm-button, .popup-cancel-button {
        background: linear-gradient(135deg, #6a1b9a, #9c27b0); /* 渐变背景 */
        color: #fff;
        border: none;
        border-radius: 12px; /* 更圆润的边框 */
        padding: 12px 24px;
        cursor: pointer;
        font-weight: 600;
        font-size: 16px; /* 增加字体大小 */
        transition: background 0.3s ease, transform 0.3s ease, box-shadow 0.3s ease;
        font-family: 'Poppins', sans-serif;
        box-shadow: 0 4px 8px rgba(0, 0, 0, 0.2); /* 添加阴影 */
    }

    .popup-confirm-button {
        background: linear-gradient(135deg, #6a1b9a, #9c27b0);
    }

    .popup-cancel-button {
        background: linear-gradient(135deg, #f44336, #e53935); /* 红色渐变背景 */
    }

        .popup-confirm-button:hover, .popup-cancel-button:hover {
            background: linear-gradient(135deg, #9c27b0, #6a1b9a); /* 悬停时反转渐变 */
            transform: scale(1.05); /* 缩放动画 */
        }

        .popup-confirm-button:active, .popup-cancel-button:active {
            transform: scale(0.98); /* 按下时缩小效果 */
        }

    .success-popup {
        max-width: 400px;
        padding: 20px;
        background-color: #d4edda;
        color: #155724;
    }

        .success-popup h3 {
            color: #155724;
        }

        .success-popup p {
            color: #155724;
        }

    .popup-close-button {
        background-color: #6a1b9a;
        color: #fff;
    }

        .popup-close-button:hover {
            background-color: #4a0072;
        }

    .user-header {
        margin-bottom: 15px;
        font-family: 'Poppins', sans-serif;
        color: #333;
    }

        .user-header h3 {
            margin: 0;
            font-size: 20px;
            color: #6a1b9a; /* 紫色标题 */
        }

        .user-header p {
            margin: 5px 0;
            font-size: 16px;
            color: #555; /* 暗灰色字体 */
        }
</style>