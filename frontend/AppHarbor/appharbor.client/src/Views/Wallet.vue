<template>
    <!link href="https://fonts.googleapis.com/css2?family=Roboto+Condensed:wght@400;700&display=swap" rel="stylesheet">
    <div class="Wallet">
        <div v-if="notificationMessage" class="notification-toast">
            <p>{{ notificationMessage }}</p>
        </div>
        <div class="header">
            <div class="title">{{ user_nickname }}的钱包</div>
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

        <div class="wallet-body">
            <div class="auto-wrapper">
                <p class="leftmoney">钱包余额</p>
                <p class="user-credit" v-html="formattedCredit"></p>
                <div class="button-row">
                    <input type="number" v-model="rechargeAmount" placeholder="输入充值金额" class="input-recharge" />
                    <button type="button" class="chargebutton" @click="recharge">充值</button>
                </div>
            </div>

            <div class="order-list">
                <p class="text">交易记录</p>
                <div class="transaction-table">
                    <table>
                        <thead>
                            <tr>
                                <th>日期</th>
                                <th>描述</th>
                                <th>金额</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr v-for="(transaction, index) in transactions" :key="index"
                                :class="{ 'recharge': transaction.type === 'recharge', 'purchase': transaction.type === 'purchase' }">
                                <td>{{ transaction.time.replace('T', ' ') }}</td>
                                <td v-if="transaction.type === 'recharge'">充值</td>
                                <td v-else-if="transaction.type === 'purchase'">
                                    为用户 {{ transaction.receiverNickName }} 购买了 {{ transaction.applicationName }}
                                </td>
                                <td>{{ transaction.amount }}</td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>



    </div>
</template>

<style scoped>
    .Wallet {
        display: flex;
        flex-direction: column;
        width: 100%;
        height: 85%;
        /* background-color: aquamarine; */
    }

    .header {
        display: flex;
        justify-content: space-between;
        align-items: center;
        width: 100%;
        height: 100px;
        padding: 10px;
        border-bottom: 5px solid darksalmon;
        /* background-color: #fbeaea; */
        /* position: fixed; */
    }

    .title {
        font-size: 50px;
        color: #f97c6c;
        font-weight: bold;
        margin-left:20px;
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
        margin-right: 20px;
    }

    .user-nick {
        font-size: 22px;
        font-weight: bold;
    }

    .user-id {
        font-size: 20px;
        color: #888;
        font-weight: bold;
    }

    .wallet-body {
        /* display: flex; */
        /* background-color: gold; */
        height: 100%;
        border-bottom: 5px solid darksalmon;
    }

    .auto-wrapper {
        /* position:fixed; */
        width: 100%;
        height: 20%;
        background-color: #FADAD6;
        display: flex;
        flex-direction: row; /* 组件横向排列 */
        align-items: center; /* 组件上下居中 */
        justify-content: space-between; /* 控制不同部分的对齐 */
        border-bottom: 5px solid #fec65a;
        /* border-radius: 20px; */
    }

    .leftmoney {
        font-size: 30px;
        font-weight: bold;
        color: palevioletred;
        margin-left: 40px;
        /* background-color:aquamarine; */
    }


    .user-credit {
        font-size: 60px;
        font-family: 'Roboto Condensed', sans-serif;
        font-weight: bold;
        color: #D2691E;
        /* background-color: #e9c3c3; */
        padding: 20px;
        /* border: 2px solid pink; */
        border-radius: 20px;
        /* margin-left: 30px; 使其在父容器中居中 */
        /* transform: translateX(-50%); 左对齐的同时保持居中 */
    }

    .button-row {
        display: flex;
        justify-content: flex-end; /* 输入框和按钮右对齐 */
        align-items: center;
        gap: 10px; /* 在输入框和按钮之间添加间距 */
        margin-right: 20px; /* 按钮组距离右边界 */
    }

    .input-recharge {
        padding: 8px;
        font-size: 30px;
        border: 4px solid #FADAD6;
        border-radius: 20px;
    }

    .chargebutton {
        font-weight: bold;
        font-size: 25px;
        color: white;
        background-color: plum;
        border: none;
        border-radius: 20px;
        padding: 10px;
    }

    .order-list {
        height: 75%;
        width:100%;
        overflow: auto;
    }

    .text {
        font-size: 30px;
        font-weight: bold;
        color: palevioletred;
        margin-left: 40px;
        margin-top:20px;
    }


    .transaction-table {
        width: 95%;
        /* overflow-y: auto; */
        /* max-height: 40%;  */
        border-radius: 20px;
        margin:20px;
    }

    table {
        width: 95%;
        border-collapse: collapse;
        font-size: 16px;
        font-weight:bold !important;
        font-family:'Microsoft YaHei' !important;
        background-color: #fbeaea;
        border: 3px solid #fadad6;
        border-radius: 20px;
    }

    thead th {
        background-color: #fadad6;
        color: #f8887d;
        font-weight: bold;
        border-bottom: 3px solid #fadad6;
        font-weight: bold !important;
        font-family: 'Microsoft YaHei' !important;
        text-align: left;
    }

    th,
    td {
        padding: 10px;
        border-bottom: 1px solid #fadad6;
        font-weight: bold !important;
        font-family: 'Microsoft YaHei' !important;
    }

    tr:hover {
        background-color: #ffe5e5;
        transform: scale(1.01);
        transition: background-color 0.3s, transform 0.3s;
    }

    .recharge {
        background-color: #ffe5e5;
    }

    .purchase {
        background-color: #fbeaea;
    }

    .recharge td {
        color: #d9534f;
    }

    .purchase td {
        color: #5a5a5a;
    }

    .notification-toast {
        position: fixed;
        bottom: 80px;
        left: 50%;
        transform: translateX(-50%);
        background-color: #17a2b8; /* 青色背景 */
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



<script>
    import axios from 'axios';
    import Cookies from 'js-cookie';
    export default {
        data() {
            return {
                user_nickname: '',
                user_id: '',
                transactions: [],
                avatar_url: '',
                credit: -1,
                rechargeAmount: 0, // 充值金额
                notificationMessage: '', // 通知消息
            };
        },
        methods: {
            showNotification(message) {
                this.notificationMessage = message;
                setTimeout(() => {
                    this.notificationMessage = '';
                }, 4000); // 气泡提示显示4秒钟
            },
            fetchUserAndTransactions() {
                var token = Cookies.get('token');
                axios.post(`${this.$Url}/api/user/userInfo`, { token: token })
                    .then(response => {
                        this.user = response.data;
                        console.info(response.data);
                        this.user_id = response.data.id;
                        this.user_nickname = response.data.nickname;
                        this.avatar_url = response.data.avatar ? `${this.$Url}${response.data.avatar}` : '../../public/default.png'; //avatar 判空
                        this.credit = response.data.credit;
                        this.fetchTransactions();
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

            fetchTransactions() {
                console.info(this.user_id);
                axios.post(`${this.$Url}/api/user/getTransaction`, { id: this.user_id })
                    .then(response => {
                        this.transactions = response.data.$values;
                        console.info(this.transactions);
                    })
                    .catch(error => {
                        console.error('Error fetching transactions:', error);
                    });
            },
            recharge() {
                if (this.rechargeAmount <= 0) {
                    this.showNotification('请输入有效的充值金额');
                    return;
                }

                if (this.rechargeAmount + this.credit > 1e6) {
                    this.showNotification('充值失败，账户金额不能超过 1000000 元');
                    return;
                }

                if (!/^\d+(\.\d{1,2})?$/.test(this.rechargeAmount)) {
                    this.showNotification('请输入最多两位小数的有效金额');
                    return;
                }

                axios.post(`${this.$Url}/api/user/recharge`, { id: this.user_id, amount: this.rechargeAmount })
                    .then(response => {
                        this.fetchUserAndTransactions();
                        this.showNotification('充值成功');
                    })
                    .catch(error => {
                        console.error('Error recharging:', error);
                        this.showNotification('充值失败，请联系管理员');
                    });
            },
        },
        mounted() {
            this.fetchUserAndTransactions(); // 页面加载时从cookies获取用户ID，再获取交易信息
        },

        computed: {
            formattedCredit() {
                if (this.credit < 0) {
                    return "正在加载";
                }
                const creditStr = this.credit.toFixed(2).split('.');
                const integerPart = creditStr[0];
                const decimalPart = creditStr[1];
                return `<span>￥ </span><span class="integer-part">${integerPart}</span>.<span class="decimal-part">${decimalPart}</span>`;
            }
        }
    }
</script>

