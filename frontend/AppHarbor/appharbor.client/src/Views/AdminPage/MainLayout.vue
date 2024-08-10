<template>
    <BanAlert v-if="alertMessage" style="z-index: 1;" :message="alertMessage" @close="alertMessage = ''" />
    <div class="main-layout">
        <div class="sidebar">
            <div @click="toggleSection('appManagement')" class="menu-item section-header">
                <svg xmlns="http://www.w3.org/2000/svg" width="1em" height="1em" viewBox="0 0 24 24"><path fill="none" stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="1.5" d="M12 22C6.477 22 2 17.523 2 12S6.477 2 12 2s10 4.477 10 10s-4.477 10-10 10M10.5 5.5l7 11m-4-11l-7 11m7-2.5h-7m11 0H16" /></svg>
                &nbsp;&nbsp;&nbsp;应用管理
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
                <svg xmlns="http://www.w3.org/2000/svg" width="1em" height="1em" viewBox="0 0 24 24"><path fill="currentColor" d="M12.3 12.22A4.92 4.92 0 0 0 14 8.5a5 5 0 0 0-10 0a4.92 4.92 0 0 0 1.7 3.72A8 8 0 0 0 1 19.5a1 1 0 0 0 2 0a6 6 0 0 1 12 0a1 1 0 0 0 2 0a8 8 0 0 0-4.7-7.28M9 11.5a3 3 0 1 1 3-3a3 3 0 0 1-3 3m9.74.32A5 5 0 0 0 15 3.5a1 1 0 0 0 0 2a3 3 0 0 1 3 3a3 3 0 0 1-1.5 2.59a1 1 0 0 0-.5.84a1 1 0 0 0 .45.86l.39.26l.13.07a7 7 0 0 1 4 6.38a1 1 0 0 0 2 0a9 9 0 0 0-4.23-7.68" /></svg>
                &nbsp;&nbsp;&nbsp;用户管理
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
                <svg xmlns="http://www.w3.org/2000/svg" width="1em" height="1em" viewBox="0 0 24 24"><path fill="currentColor" d="M22 7.82a1.25 1.25 0 0 0 0-.19l-2-5A1 1 0 0 0 19 2H5a1 1 0 0 0-.93.63l-2 5a1.25 1.25 0 0 0 0 .19A.58.58 0 0 0 2 8a4 4 0 0 0 2 3.4V21a1 1 0 0 0 1 1h14a1 1 0 0 0 1-1v-9.56A4 4 0 0 0 22 8a.58.58 0 0 0 0-.18M13 20h-2v-4h2Zm5 0h-3v-5a1 1 0 0 0-1-1h-4a1 1 0 0 0-1 1v5H6v-8a4 4 0 0 0 3-1.38a4 4 0 0 0 6 0A4 4 0 0 0 18 12Zm0-10a2 2 0 0 1-2-2a1 1 0 0 0-2 0a2 2 0 0 1-4 0a1 1 0 0 0-2 0a2 2 0 0 1-4 .15L5.68 4h12.64L20 8.15A2 2 0 0 1 18 10" /></svg>
                &nbsp;&nbsp;&nbsp;商家管理
            </div>
            <div v-show="sections.merchant" class="section-content">
                <div class="menu">
                    <div class="menu-item"
                         :class="{ active: selectedStatus === '封禁商家' }"
                         @click="searchbanmerchant();changeSection('MerManagement')">封禁商家</div>
                    <div class="menu-item"
                         :class="{ active: selectedStatus === '活跃商家' }"
                         @click="searchunbanMerchant();changeSection('MerManagement')">活跃商家</div>
                </div>
            </div>

            <div @click="toggleSection('comment')" class="menu-item section-header">
                <svg xmlns="http://www.w3.org/2000/svg" width="1em" height="1em" viewBox="0 0 24 24"><g fill="none"><path d="M24 0v24H0V0zM12.593 23.258l-.011.002l-.071.035l-.02.004l-.014-.004l-.071-.035q-.016-.005-.024.005l-.004.01l-.017.428l.005.02l.01.013l.104.074l.015.004l.012-.004l.104-.074l.012-.016l.004-.017l-.017-.427q-.004-.016-.017-.018m.265-.113l-.013.002l-.185.093l-.01.01l-.003.011l.018.43l.005.012l.008.007l.201.093q.019.005.029-.008l.004-.014l-.034-.614q-.005-.019-.02-.022m-.715.002a.02.02 0 0 0-.027.006l-.006.014l-.034.614q.001.018.017.024l.015-.002l.201-.093l.01-.008l.004-.011l.017-.43l-.003-.012l-.01-.01z" /><path fill="currentColor" d="M16 4a3 3 0 0 1 2.995 2.824L19 7v2a3 3 0 0 1 2.995 2.824L22 12v4a3 3 0 0 1-2.824 2.995L19 19v.966c0 1.02-1.143 1.594-1.954 1.033l-.096-.072L14.638 19H11a3 3 0 0 1-1.998-.762l-.14-.134L7 19.5c-.791.593-1.906.075-1.994-.879L5 18.5V17a3 3 0 0 1-2.995-2.824L2 14V7a3 3 0 0 1 2.824-2.995L5 4zm3 7h-8a1 1 0 0 0-1 1v4a1 1 0 0 0 1 1h3.638a2 2 0 0 1 1.28.464l1.088.906A1.5 1.5 0 0 1 18.5 17h.5a1 1 0 0 0 1-1v-4a1 1 0 0 0-1-1m-3-5H5a1 1 0 0 0-1 1v7a1 1 0 0 0 1 1h.5A1.5 1.5 0 0 1 7 16.5v.5l1.01-.757A3 3 0 0 1 8 16v-4a3 3 0 0 1 3-3h6V7a1 1 0 0 0-1-1" /></g></svg>
                &nbsp;&nbsp;&nbsp;评论管理
            </div>
            <div v-show="sections.comment" class="section-content">
                <!-- 评论相关内容 -->
            </div>

            <div @click="toggleSection('anouncement')" class="menu-item section-header">
                <svg xmlns="http://www.w3.org/2000/svg" width="1em" height="1em" viewBox="0 0 20 20"><path fill="currentColor" d="M6 8v7h8V8a4 4 0 1 0-8 0m2.03-5.67a2 2 0 1 1 3.95 0A6 6 0 0 1 16 8v6l3 2v1H1v-1l3-2V8a6 6 0 0 1 4.03-5.67M12 18a2 2 0 1 1-4 0z" /></svg>
                &nbsp;&nbsp;&nbsp;通知管理
            </div>
            <div v-show="sections.anouncement" class="section-content">
                <!-- 通知相关内容 -->
            </div>
        </div>

        <div class="main-content">
            <!--应用-->
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

            <!--用户-->
            <div v-if="section==='userManagement' && !userstate" class="app-list">
                <div v-for="user in users" :key="user.id" class="app-item">
                    <div class="user-header">
                        <h3>用户昵称：{{ user.nickname }}</h3>
                        <p>用户ID：{{ user.userId }}</p>
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
                        <h3>用户昵称：{{ user.nickname }}</h3>
                        <p>用户ID：{{ user.id }}</p>
                        <p>用户昵称：{{ user.nickname }}</p>
                        <p>账号注册时间：{{ user.registerTime }}</p>
                    </div>
                    <div class="app-actions">
                        <button @click="handleBan(user)" class="action-button">封禁</button>
                    </div>
                </div>
            </div>

            <!--商家-->
            <div v-if="section==='MerManagement' && !merstate" class="app-list">
                <div v-for="mer in mers" :key="mer.id" class="app-item">
                    <div class="user-header">
                        <h3>商家昵称：{{ mer.merchantNickname }}</h3>
                        <p>商家ID：{{ mer.userId}}</p>
                        <p>封禁操作执行管理员ID：{{ mer.adminId }}</p>
                        <p>封禁时间：{{ mer.time }}</p>
                        <p>封禁原因：{{ mer.reason }}</p>
                    </div>
                    <div class="app-actions">
                        <!-- 只保留一个解除封禁按钮 -->
                        <button @click="handleMerUnban(mer)" class="action-button">解除封禁</button>
                    </div>
                </div>
            </div>
            <div v-if="section==='MerManagement'&&merstate" class="app-list">
                <div v-for="mer in mers" :key="mer.id" class="app-item">
                    <div class="user-header">
                        <h3>商家昵称：{{ mer.nickname }}</h3>
                        <p>商家ID：{{ mer.id }}</p>
                        <p>商家昵称：{{ mer.nickname }}</p>
                        <p>商家注册时间：{{ mer.registerTime }}</p>
                    </div>
                    <div class="app-actions">
                        <button @click="handleMerBan(mer)" class="action-button">封禁</button>
                    </div>
                </div>
            </div>

            <!-- 封禁成功弹窗 -->
            <div v-if="showBanSuccessPopup" class="popup-overlay" @click="closeBanSuccessPopup">
                <div class="popup-content success-popup" @click.stop>
                    <h3>成功执行</h3>
                    <p>管理员权限操作成功！</p>
                    <button @click="closeBanSuccessPopup" class="popup-close-button">关闭</button>
                </div>
            </div>

            <!-- 封禁解除成功弹窗 -->
            <div v-if="showUnBanSuccessPopup" class="popup-overlay" @click="closeBanSuccessPopup">
                <div class="popup-content success-popup" @click.stop>
                    <h3>成功执行</h3>
                    <p>管理员权限操作成功！</p>
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
            <h3>管理员审核成功</h3>
            <p>该应用已成功上架！</p>
            <button @click="closeSuccessPopup" class="popup-close-button">关闭</button>
        </div>
    </div>

    <!-- 封禁确认弹窗 -->
    <div v-if="showBanConfirmPopup" class="popup-overlay" @click="cancelBan">
        <div class="popup-content ban-confirm-popup" @click.stop>
            <h3>确认封禁&nbsp;{{ userToBan ? userToBan.nickname : '' }}</h3>
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
            <h3>确认解除封禁&nbsp;{{ userToUnban ? userToUnban.nickname : '' }}</h3>
            <p>您确定要解除用户 {{ userToUnban ? userToUnban.nickname : '' }} 的封禁吗？</p>
            <div class="confirm-buttons">
                <button @click="confirmUnban" class="popup-confirm-button">确定</button>
                <button @click="cancelUnban" class="popup-cancel-button">取消</button>
            </div>
        </div>
    </div>

    <!-- 封禁商家确认弹窗 -->
    <div v-if="showMerBanConfirmPopup" class="popup-overlay" @click="cancelMerBan">
        <div class="popup-content ban-confirm-popup" @click.stop>
            <h3>确认封禁商家&nbsp;{{ merToBan ? merToBan.nickname : '' }}</h3>
            <p>请填写封禁理由：</p>
            <textarea v-model="merBanReason" rows="4" placeholder="请输入封禁理由"></textarea>
            <div class="confirm-buttons">
                <button @click="confirmMerBan" class="popup-confirm-button">确定封禁</button>
                <button @click="cancelMerBan" class="popup-cancel-button">取消</button>
            </div>
        </div>
    </div>

    <!-- 封禁解除确认弹窗 -->
    <div v-if="showMerUnbanConfirmPopup" class="popup-overlay" @click="cancelMerUnban">
        <div class="popup-content unban-confirm-popup" @click.stop>
            <h3>确认解除封禁&nbsp;{{ merToUnban ? merToUnban.merchantNickname : '' }}</h3>
            <p>您确定要解除商家 {{ merToUnban ? merToUnban.merchantNickname : '' }} 的封禁吗？</p>
            <div class="confirm-buttons">
                <button @click="confirmMerUnban" class="popup-confirm-button">确定</button>
                <button @click="cancelMerUnban" class="popup-cancel-button">取消</button>
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
                mers: [],
                loading: false,
                error: null,
                userstate: 0,
                merstate: 0,
                section: null,
                selectedStatus: '请在右侧选择你要查看的应用状态',
                sections: {
                    appManagement: false,
                    userManagement: false,
                    comment: false,
                    merchant: false
                },
                showPopup: false,
                showConfirmPopup: false,
                showSuccessPopup: false,
                showBanConfirmPopup: false,
                showBanSuccessPopup: false,
                showUnbanConfirmPopup: false,
                showMerBanConfirmPopup: false,
                selectedApp: null,
                appToShelve: null,
                selectedUser: null,
                merToBan: null,
                merBanReason: '',

                userToUnban: null,
                showUnbanConfirmPopup: false,
                showMerUnbanConfirmPopup: false,
                merToUnban: null,
            };
        },
        methods: {
            handleMerUnban(merchant) {
                this.merToUnban = merchant;
                this.showMerUnbanConfirmPopup = true;
            },
            confirmMerUnban() {
                const token = Cookies.get('token');
                if (!token) {
                    alert('未提供 token');
                    return;
                }

                const formData = new FormData();
                formData.append('mytoken', token);
                formData.append('merchant_id', this.merToUnban.userId);

                axios.post('http://localhost:5118/api/BanMerchant/unbanmerchant', formData)
                    .then(response => {
                        this.showMerUnbanConfirmPopup = false;
                        this.merToUnban = null;
                        this.showBanSuccessPopup = true;
                        this.searchbanmerchant();
                    })
                    .catch(error => {
                        console.error('解除封禁商家失败:', error);
                        alert('解除封禁商家失败，请重试');
                    });
            },
            cancelMerUnban() {
                this.showMerUnbanConfirmPopup = false;
                this.merToUnban = null;
            },
            handleMerBan(merchant) {
                this.merToBan = merchant;
                this.showMerBanConfirmPopup = true;
            },
            confirmMerBan() {
                const token = Cookies.get('token');
                if (!token) {
                    alert('未提供 token');
                    return;
                }

                const formData = new FormData();
                formData.append('mytoken', token);
                formData.append('merchant_id', this.merToBan.id);
                formData.append('reason', this.merBanReason);

                axios.post('http://localhost:5118/api/BanMerchant/banmerchant', formData)
                    .then(response => {
                        this.showMerBanConfirmPopup = false;
                        this.merBanReason = '';
                        this.merToBan = null;
                        this.showBanSuccessPopup = true;
                        this.searchbanmerchant();
                    })
                    .catch(error => {
                        console.error('封禁商家失败:', error);
                        alert('封禁商家失败，请重试');
                    });
            },
            cancelMerBan() {
                this.showMerBanConfirmPopup = false;
                this.merBanReason = '';
                this.merToBan = null;
            },
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
            searchbanmerchant() {
                this.selectedStatus = '封禁商家';
                this.merstate = 0;
                axios.post('http://localhost:5118/api/banmerchant/searchbanmerchant')
                    .then(response => {
                        this.mers = response.data;
                    })
                    .catch(error => {
                        this.error = error;
                    })
                    .finally(() => {
                        this.loading = false;
                    });
            },
            searchunbanMerchant() {
                this.selectedStatus = '活跃商家';
                this.merstate = 1;
                axios.post('http://localhost:5118/api/merchant/searchunbanmerchant')
                    .then(response => {
                        this.mers = response.data.$values;
                        console.log(this.mers);
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