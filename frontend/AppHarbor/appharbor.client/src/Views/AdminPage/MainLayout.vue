<template>
    <BanAlert v-if="alertMessage" style="z-index: 1;" :message="alertMessage" @close="alertMessage = ''" />
    <div class="main-layout">
        <div class="sidebar">
            <div @click="toggleSection('appManagement')" class="menu-item section-header">
                应用管理
            </div>
            <div v-show="sections.appManagement" class="section-content">
                <div class="menu">
                    <div class="menu-item"
                         :class="{ active: selectedStatus === '待审核应用' }"
                         @click="selectseleasing();changeSection('appManagement')">待审核</div>
                    <div class="menu-item"
                         :class="{ active: selectedStatus === '已审核应用' }"
                         @click="selectseleased();changeSection('appManagement')">已审核</div>
                </div>
            </div>

            <div @click="toggleSection('userManagement')" class="menu-item section-header">
                用户管理
            </div>
            <div v-show="sections.userManagement" class="section-content">
                <div class="menu">
                    <div class="menu-item"
                         :class="{ active: selectedStatus === '封禁用户' }"
                         @click="searchbanuser();changeSection('userManagement')">封禁用户</div>
                    <div class="menu-item"
                         :class="{ active: selectedStatus === '活跃用户' }"
                         @click="searchunbanuser();changeSection('userManagement')">活跃用户</div>
                </div>
            </div>

            <div @click="toggleSection('merchant')" class="menu-item section-header">
                商家管理
            </div>
            <div v-show="sections.merchant" class="section-content">
                <!-- 商家相关内容 -->
            </div>

            <div @click="toggleSection('comment')" class="menu-item section-header">
                评论管理
            </div>
            <div v-show="sections.comment" class="section-content">
                <!-- 评论相关内容 -->
            </div>

            <div @click="toggleSection('anouncement')" class="menu-item section-header">
                通知管理
            </div>
            <div v-show="sections.anouncement" class="section-content">
                <!-- 通知相关内容 -->
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
                        <p>封禁操作执行管理员ID：{{ user.adminId }}</p>
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
                    <h3>成功封禁</h3>
                    <p>管理员操作成功！</p>
                    <button @click="closeBanSuccessPopup" class="popup-close-button">关闭</button>
                </div>
            </div>

            <!-- 封禁解除成功弹窗 -->
            <div v-if="showUnBanSuccessPopup" class="popup-overlay" @click="closeBanSuccessPopup">
                <div class="popup-content success-popup" @click.stop>
                    <h3>成功解除封禁</h3>
                    <p>管理员操作成功！</p>
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

    <!-- 封禁解除确认弹窗 -->
    <div v-if="showUnbanConfirmPopup" class="popup-overlay" @click="cancelUnban">
        <div class="popup-content unban-confirm-popup" @click.stop>
            <h3>确认解除封禁</h3>
            <p>您确定要解除用户 {{ userToUnban ? userToUnban.userId : '' }} 的封禁吗？</p>
            <div class="confirm-buttons">
                <button @click="confirmUnban" class="popup-confirm-button">确定</button>
                <button @click="cancelUnban" class="popup-cancel-button">取消</button>
            </div>
        </div>
    </div>

</template>

<script>
    import axios from 'axios';
    import Cookies from 'js-cookie';
    import BanAlert from './BanAlert.vue';

    export default {
        name: "MainLayout",
        components: {
            BanAlert,
        },
        data() {
            return {
                alertMessage: '',

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

                userToUnban: null,
                showUnbanConfirmPopup: false,
            };
        },
        methods: {
            handleUnban(user) {
                this.userToUnban = user;
                this.showUnbanConfirmPopup = true;
            },
            confirmUnban() {
                const token = Cookies.get('token');
                if (!token) {
                    alert('未提供 token');
                    return;
                }

                const formData = new FormData();
                formData.append('mytoken', token);
                formData.append('user_id', this.userToUnban.userId); // 确保传递正确的用户 ID

                axios.post('http://localhost:5118/api/BanUser/unbanuser', formData)
                    .then(response => {
                        this.showUnbanConfirmPopup = false;
                        this.userToUnban = null;
                        this.showBanSuccessPopup = true;
                        this.searchbanuser();
                    })
                    .catch(error => {
                        console.error('解除封禁失败:', error);
                        alert('解除封禁失败，请重试');
                    });
            },
            cancelUnban() {
                this.showUnbanConfirmPopup = false;
                this.userToUnban = null;
            },
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
                        //this.alertMessage = '请输入封禁理由';
                        alert('请输入封禁理由');
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
            changeSection(section) {
                this.section = section;
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
    @import './MainLayout.css';

</style>
