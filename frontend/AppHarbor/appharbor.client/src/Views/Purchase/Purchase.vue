<template>
    <div class="background">
        <alert-box :msg="alert"></alert-box>
        <confirm-box :msg="confirm"></confirm-box>
        <div class="order-container">
            <!-- 左侧部分 -->
            <div class="left-container">
                <div class="section_2">
                    <span class="title">应用购买</span>
                    <div class="recipient-section">
                        <span class="label">接收者:</span>
                        <div class="dropdown">
                            <div class="receiver-item">
                                <img :src="getAvatarUrl(receiver.avatar)" class="avatar" alt="Receiver Avatar">
                                <div class="receiver-details">
                                    <p class="receiver-name">{{ receiver.nickname }}</p>
                                    <p class="receiver-id">{{ receiver.id }}</p>
                                </div>
                            </div>
                            <img :class="{'open': showDropdown}"
                                 class="dropdown-toggle"
                                 @click="toggleDropdown"
                                 src="@/assets/direction-down.svg" />
                        </div>
                        <div class="dropdown-menu" v-show="showDropdown">
                            <div v-for="friend in friends" :key="friend.id" class="friendsmenu-item" @click="changeReceiver(friend)">
                                <p>{{friend.nickname}}</p>
                            </div>
                        </div>
                    </div>
                    <span class="label">买家(您):</span>
                    <div class="user-item">
                        <img :src="getAvatarUrl(user.avatar)" class="avatar" alt="User Avatar">
                        <div class="user-details">
                            <p class="user-name">{{ user.nickname }}</p>
                            <p class="user-id">{{ user.id }}</p>
                        </div>
                    </div>
                </div>
                <div class="money-left">
                    <img loading="lazy" src="https://cdn.builder.io/api/v1/image/assets/TEMP/ebfe3a97bf3fbf1744b64e98bf537a292b984f51f5a76525c09758e46a083f17?apiKey=b4c87aa6fd1245589700a3931ad0dfbf&" class="wallet-img" />
                    <div class="money">
                        <div class="text-money">￥{{ user_credit }}</div>
                        <div class="text-left">余额</div>
                    </div>
                </div>
            </div>

            <!-- 中间的应用详情部分 -->
            <div class="app-container">
                <button class="circle-button" @click="backToDetails(app.id)">返回</button> <!-- 新增的圆形按钮 -->
                <div class="app-image-wrapper">
                    <img loading="lazy" :src="getAppImgUrl(app.image)" :alt="app.name" class="app-image" />
                </div>
                <div class="app-details">
                    <h2 class="app-name">{{ app.name }}</h2>
                    <div class="app-price" v-html="formattedPrice"></div>
                    <div class="app-description">{{ app.description }}</div>
                </div>
            </div>

            <!-- 右侧部分 -->
            <div class="right-container">
                <div class="tips-container">
                    <div class="text-tips">AppHarbor</div>
                    <div class="tips-wrapper">
                        <div class="tips-item">
                            <p class="tips-text">
                            快为你的小伙伴购买心仪的应用吧！获取应用后即可前往"我的库存"中查看并下载。
                            </p>
                        </div>
                    </div>
                </div>
                <div class="purchase-container">
                    <div class="text-total">价格</div>
                    <div class="purchase-price" v-html="formattedPrice"></div>
                    <button class="purchase-button" @click="handlePurchase">确认购买</button>
                </div>
            </div>
        </div>
        <!-- 弹窗 -->
        <div v-if="showModal" class="modal-overlay" @click="closeModal">
            <div class="modal-content" @click.stop>
                <p>{{ purchaseMessage }}</p>
                <button @click="closeModal">关闭</button>
            </div>
        </div>
    </div>
</template>

<script>
    import axios from 'axios';
    import Cookies from 'js-cookie';
    import global from "@/global.js";
    import AlertBox from '../AlertBox.vue';
    import ConfirmBox from '../ConfirmBox.vue';

    export default {
        components: {
            AlertBox,
            ConfirmBox
        },
        beforeRouteEnter(to, from, next) {
            next(vm => {
                // 路由进入此页时，清空旧的 app 数据，重新获取
                vm.app = '';
                vm.friends = [];
                const appId = to.params.id;
                vm.fetchAppDetails(appId);
                // 获取个人信息部分
                vm.fetchUserInfo();
                // 获取好友信息部分
                vm.fetchFriends();
            });
        },
        data() {
            return {
                user: '',
                app: '',
                user_credit: 0,
                receiver: '',
                friends: [],
                showDropdown: false,
                alert: '',
                confirm: '',
                showModal: false,
                purchaseMessage: '',
            };
        },
        methods: {
            handlePurchase() {
                // 购买的后端
                let formData = new FormData();
                formData.append('BuyerID', this.user.id);
                formData.append('ReceiverID', this.receiver.id);
                formData.append('APPID', this.app.id);
                axios.post(`${this.$Url}/api/order/createneworder`, formData)
                    .then(response => {
                        /*this.confirmNotification('购买成功！');*/
                        this.purchaseMessage = '购买成功！';
                        this.showModal = true;
                        this.updateCredit();
                    })
                    .catch(error => {
                        const parsedData = error.response.data;
                        console.error('Error purchase app:', error);
                        /*this.alertNotification('购买失败：' + parsedData.msg);*/
                        this.purchaseMessage = '购买失败：' + parsedData.msg;
                        this.showModal = true;
                    });

                console.log('App has been puechased!');
            },
            closeModal() {
                this.showModal = false;
            },
            backToDetails(appId){
                this.$router.push(`/app/${appId}`);
            },
            updateCredit() {
                console.log('credit change!');
                var token = Cookies.get('token');
                axios.post(`${this.$Url}/api/user/userInfo`, { token: token })
                    .then(response => {
                        this.user_credit = response.data.credit;
                    })
                    .catch(error => {
                        console.error('Error fetching user data:', error);
                    });
            },
            toggleDropdown() {
                // 好有菜单选项
                this.showDropdown = !this.showDropdown;
            },
            fetchFriends() {
                // 获取好友列表信息
                let formData = new FormData();
                var token = Cookies.get('token');
                formData.append('token', token);
                axios.post(`${this.$Url}/api/relationship/findall`,formData)
                    .then(response => {
                        this.friends = response.data.data.$values;
                    })
                    .catch(error => {
                        console.error('Error fetching friends data:', error);
                    });
            },
            changeReceiver(newReceiver) {
                this.receiver = newReceiver;
                this.showDropdown = false; // 关闭下拉菜单
            },
            fetchAppDetails(appId) {
                // 从API或其他地方获取应用详细信息
                axios.post(`${this.$Url}/api/application/getappdetail`, {
                    Id: appId
                    /*Page: this.currentPage */
                })
                    .then(response => {
                        this.app = response.data;
                        //console.log(this.app.price);
                    })
                    .catch(error => {
                        console.error("Error fetching apps:", error);
                    });
            },
            fetchUserInfo() {
                // 获取用户个人信息
                var token = Cookies.get('token');
                axios.post(`${this.$Url}/api/user/userInfo`, { token: token })
                    .then(response => {
                        this.user = response.data;
                        // 默认接受者为自己
                        this.receiver = response.data;
                        this.friends.push(response.data);
                        this.user_credit = response.data.credit;
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
            receiverInit() {
                console.log("receiver init!");
                this.receiver.nickname = this.user.nickname;
                this.receiver.id = this.user.id;
                this.receiver.avatar = this.user.avatar;
            },
            getAvatarUrl(avatarPath) {
                if (avatarPath) {
                    return `${this.$Url}${avatarPath}`;
                }
                return '../../public/default.png'; // 默认头像路径
            },
            getAppImgUrl(imgPath) {
                if (imgPath) {
                    return `${this.$Url}${imgPath}`;
                }
                return '../../public/default.png'; // 默认图片路径
            },
            alertNotification(message) {
                this.alert = message;
            },
            confirmNotification(message) {
                this.confirm = message;
            },
            updateCredit() {
                console.log('credit change!');
                var token = Cookies.get('token');
                axios.post(`${this.$Url}/api/user/userInfo`, { token: token })
                    .then(response => {
                        this.user_credit = response.data.credit;
                    })
                    .catch(error => {
                        console.error('Error fetching user data:', error);
                    });
            },
            toggleDropdown() {
                // 好有菜单选项
                this.showDropdown = !this.showDropdown;
            },
            fetchFriends() {
                // 获取好友列表信息
                let formData = new FormData();
                var token = Cookies.get('token');
                formData.append('token', token);
                axios.post(`${this.$Url}/api/relationship/findall`, formData)
                    .then(response => {
                        this.friends = response.data.data.$values;
                    })
                    .catch(error => {
                        console.error('Error fetching friends data:', error);
                    });
            },
            changeReceiver(newReceiver) {
                // 更改当前friend的属性
                this.receiver = newReceiver;

                this.showDropdown = false; // 关闭下拉菜单
            },
            fetchAppDetails(appId) {
                // 从API或其他地方获取应用详细信息
                axios.post(`${this.$Url}/api/application/getappdetail`, {
                    Id: appId
                    /*Page: this.currentPage */
                })
                    .then(response => {
                        this.app = response.data;
                        //console.log(this.app.price);
                    })
                    .catch(error => {
                        console.error("Error fetching apps:", error);
                    });
            },
            fetchUserInfo() {
                // 获取用户个人信息
                var token = Cookies.get('token');
                axios.post(`${this.$Url}/api/user/userInfo`, { token: token })
                    .then(response => {
                        this.user = response.data;
                        // 默认接受者为自己
                        this.receiver = response.data;
                        this.friends.push(response.data);
                        this.user_credit = response.data.credit;
                    })
                    .catch(error => {
                        console.error('Error fetching user data:', error);
                    });
            },
            receiverInit() {
                console.log("receiver init!");
                this.receiver.nickname = this.user.nickname;
                this.receiver.id = this.user.id;
                this.receiver.avatar = this.user.avatar;
            },
            getAvatarUrl(avatarPath) {
                if (avatarPath) {
                    return `${this.$Url}${avatarPath}`;
                }
                return '../../public/default.png'; // 默认头像路径
            }
        },
        computed: {
            formattedPrice() {
                if (!this.app) return;
                if (this.app.price === 0) {
                    return `<span>Free! 免费</span>`;
                }

                const originalPrice = this.app.price.toFixed(2).split('.');
                const originalIntegerPart = originalPrice[0];
                const originalDecimalPart = originalPrice[1];

                const discountedPrice = (this.app.price * this.app.discount).toFixed(2).split('.');
                const discountedIntegerPart = discountedPrice[0];
                const discountedDecimalPart = discountedPrice[1];

                let result = `
                    <span>
                        ￥ <span class="integer-part">${discountedIntegerPart}</span>.<span class="decimal-part">${discountedDecimalPart}</span>
                    </span>
                `;

                if (this.app.discount < 1.0) {
                    result += `
                    <span style="text-decoration: line-through; font-size: 0.6em; color:gray;">
                        原价：<span class="integer-part">${originalIntegerPart}</span>.<span class="decimal-part">${originalDecimalPart}</span>
                    </span>
                `;
                }

                return result;
            }

        },

        created() {
            // 获取应用信息部分
            const appId = this.$route.params.id;
            this.fetchAppDetails(appId);

            // 获取个人信息部分
            this.fetchUserInfo();

            // 获取好友信息部分
            this.fetchFriends();
        },

    };
</script>

<style scoped>
    .background {
        position: fixed;
        top: 0;
        left: 0;
        height: 100%;
        width: 100%;
        background-color: #efc2bb;
        background-size: cover;
        background-position: center;
        background-repeat: no-repeat;
    }

    .order-container {
        background-color: white;
        border-radius: 20px;
        width: 95%;
        height: 90%;
        margin: 30px auto;
        display: flex;
        padding: 20px;
        box-shadow: 0 4px 15px rgba(0, 0, 0, 0.2);
    }

    .left-container {
        flex: 1;
        padding: 10px;
        margin-right: 20px;
        display: flex;
        flex-direction: column;
        justify-content:space-around;
    }

    .section_2 {
        background-color: #efc2bb;
        border-radius: 10px;
        padding: 1rem;
        box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
        margin-bottom: 20px;
    }

    .title {
        font-size: 1.5rem;
        font-weight: bold;
        color: #333;
        margin-bottom: 20px;
    }

    .recipient-section {
        margin-bottom: 20px;
    }

    .label {
        font-size: 1rem;
        color: #666;
        margin-bottom: 10px;
    }

    .dropdown {
        display: flex;
        align-items: center;
        position: relative;
    }

    .receiver-item, .user-item {
        display: flex;
        align-items: center;
        padding: 10px;
        /*background-color: #efc2bb;*/
        background-color: antiquewhite;
        border-radius: 5px;
        margin-bottom: 10px;
        box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
        width: 100%;
    }

    .dropdown-menu {
        max-height: 200px;
        overflow-y: auto;
        background-color: antiquewhite;
        border-radius: 5px;
        box-shadow: 0 2px 10px rgba(0, 0, 0, 0.2);
        padding: 10px;
        position: absolute;
        width: 300px;
        z-index: 1000;
    }

    .friendsmenu-item {
        padding: 10px;
        background-color: #efc2bb;
        border-radius: 5px;
        margin-bottom: 5px;
        box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1);
        cursor: pointer;
    }

        .friendsmenu-item:hover {
            background-color: #e0b2aa;
        }

    .dropdown-toggle {
        cursor: pointer;
        width: 20px;
        height: 20px;
        margin-left: 10px;
        transition: transform 0.3s;
        width: 38px;
        height: 38px;
    }

        .dropdown-toggle.open {
            transform: rotate(180deg);
        }

    .avatar {
        width: 40px;
        height: 40px;
        border-radius: 50%;
        margin-right: 10px;
    }

    .money-left {
        /*background-color: #fff5f5;*/
        background-color: #efc2bb;
        padding: 20px;
        border-radius: 10px;
        text-align: center;
        height: 200px;
        display: flex;
        flex-direction: column;
        justify-content: space-around;
    }

    .wallet-img {
        width: 40px;
        height: 40px;
        margin-left: auto;
        margin-right: auto;
    }

    .money {
        margin-top: 10px;
    }

    .text-money {
        font-size: 1.8rem;
        font-weight: bold;
        color: #333;
    }

    .text-left {
        font-size: 1.3rem;
        font-weight: 500;
        color: #333;
    }

    .app-container {
        flex: 3;
        max-width: 50%;
        /*max-height: 90%;*/
        background-color: #faebd7;
        border-radius: 10px;
        box-shadow: 0 4px 15px rgba(0, 0, 0, 0.1);
        display: flex;
        flex-direction: column;
        align-items: center;
        padding: 20px;
        margin-right: 20px;
        margin-top: auto;
        margin-bottom: auto;
        position: relative; /* 使容器成为定位上下文 */
    }

    .circle-button {
        position: absolute;
        top: 10px;
        left: 10px;
        width: 60px;
        height: 30px;
        border-radius: 5px;
        border: none;
        background-color: #fbb1a2;
        color: white;
        font-size: 15px;
        display: flex;
        align-items: center;
        justify-content: center;
        box-shadow: 0 2px 6px rgba(0, 0, 0, 0.2);
        cursor: pointer;
        transition: background-color 0.3s ease;
    }

        .circle-button:hover {
            background-color: #e64a19; /* 悬停时的背景颜色 */
        }

    .app-image-wrapper {
        width: 100%;
        height: 300px; /* 固定图片高度 */
        display: flex;
        justify-content: center;
        align-items: center;
        margin-bottom: 20px;
    }

    .app-image {
        max-width: 100%;
        max-height: 100%;
        object-fit: contain; /* 确保图片在固定区域内自适应 */
    }

    .app-details {
        text-align: center;
        width: 100%;
    }

    .app-name {
        font-size: 1.75rem;
        font-weight: bold;
        margin-bottom: 10px;
    }

    .app-price {
        font-size: 1.5rem;
        /*color: #e74c3c;*/
        margin-bottom: 10px;
        color: #ff5722; /* Updated color */
        font-family: 'Pacifico', cursive; /* Cute font */
    }

    .app-description {
        font-size: 1rem;
        font-weight: 600;
        color: #777;
    }

    .right-container {
        flex: 1;
        display: flex;
        flex-direction: column;
        align-items: center;
        justify-content:space-around;
    }

    .tips-container {
        width: 100%;
        height: 200px;
        background-color: #ffebf2;
        border-radius: 20px;
        box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
        margin-bottom: 20px;
        padding: 15px;
        display: flex;
        flex-direction: column;
        overflow-y: auto;
        font-family: 'Comic Sans MS', cursive, sans-serif;
        border: 2px /*dashed*/ #ff6f91;
    }

    .text-tips {
        font-size: 18px;
        color: #ff4081;
        font-weight: bold;
        text-align: center;
        margin-bottom: 10px;
    }

    .tips-wrapper {
        flex-grow: 1; /* 让tips-wrapper填充剩余空间 */
        display: flex;
        flex-direction: column;
        justify-content: center; /* 垂直居中 */
        gap: 10px;
        overflow-y: auto;
    }

    .tips-item {
        background-color: #ffe4e9;
        border-radius: 10px;
        padding: 10px;
        box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
        height: 100px;
    }

    .tips-text {
        font-size: 16px;
        color: #ff4081;
        text-align: center;
    }


    .purchase-container {
        width: 100%;
        background-color: #fff5f5;
        padding: 20px;
        border-radius: 10px;
        box-shadow: 0 4px 15px rgba(0, 0, 0, 0.1);
        text-align: center;
        height: 200px;
        display: flex;
        flex-direction: column;
        justify-content: space-around;
    }

    .text-total {
        font-size: 1.5rem;
        font-weight: 700;
        color: #333;
    }

    .purchase-price {
        font-size: 1.3rem;
        color: #ff5722; /* Updated color */
        font-family: 'Pacifico', cursive; /* Cute font */
    }

    .purchase-button {
        display: inline-block;
        background-color: #fbb1a2;
        color: #fff;
        padding: 12px 24px;
        border: none;
        border-radius: 8px;
        cursor: pointer;
        font-size: 16px;
        font-weight: bold;
        transition: background-color 0.3s, transform 0.3s, box-shadow 0.3s;
        box-shadow: 0 4px 8px rgba(0, 0, 0, 0.2);
        font-family: 'Pacifico', cursive; /* Cute font */
        width: 80%;
        margin-left:auto;
        margin-right:auto;
    }

        .purchase-button:hover {
            background-color: #e89a8d;
            transform: translateY(-2px);
        }

    /* 弹窗遮罩 */
    .modal-overlay {
        position: fixed;
        top: 0;
        left: 0;
        width: 100%;
        height: 100%;
        background-color: rgba(0, 0, 0, 0.6); /* 半透明暗色 */
        display: flex;
        align-items: center;
        justify-content: center;
        z-index: 1000;
    }

    /* 弹窗内容 */
    .modal-content {
        background-color: #fff;
        padding: 30px; /* 增加内边距 */
        border-radius: 10px;
        box-shadow: 0 4px 15px rgba(0, 0, 0, 0.3);
        text-align: center;
        z-index: 1001;
        width: 400px; /* 设置弹窗的宽度 */
        max-width: 90%; /* 确保在小屏幕上弹窗不会太大 */
    }

        .modal-content button {
            margin-top: 20px;
            padding: 10px 20px;
            background-color: #efc2bb;
            border: none;
            border-radius: 5px;
            cursor: pointer;
        }

            .modal-content button:hover {
                background-color: #e89a8d;
                transform: translateY(-2px);
            }


</style>
