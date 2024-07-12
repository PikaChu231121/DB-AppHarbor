<template>
    <div class="Wallet">
        <div class="header">
            <img :src="avatar_url"  class="avatar-header"  />
            <div class="avatar">
                <img :src="avatar_url"  class="avatar-circle" />
                <div class="user-info">
                    <p class="user-nickname">用户昵称：{{ user_nickname }}</p>
                    <p class="user-id">用户ID：{{ user_id }}</p>
                </div>
            </div>
        </div>
        <div class="auto-wrapper">
            <div class="info-box">
                <p class="text">钱包余额</p>
                <p class="user-credit" v-html="formattedCredit"></p>
                <div class="button-row">
                    <button type="button" class="button">充值</button>
                </div>
            </div>
            <div class="info-box">
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
                            <tr v-for="(transaction, index) in transactions" :key="index">
                                <td>{{ transaction.time }}</td>
                                <td>为用户 {{ transaction.receiverNickName }} 购买了 {{ transaction.applicationName }}</td>
                                <td>{{ transaction.amount }}</td>
                            </tr>
                        </tbody>
                    </table>
                </div>

                <div class="button-row">
                    <button type="button" class="button" @click="prevPage">上一页</button>
                    <button type="button" class="button" @click="nextPage">下一页</button>
                </div>
            </div>
        </div>
    </div>
</template>

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
            credit: -1
        };
    },
    methods: {
        fetchUserAndTransaction() {
                var token = Cookies.get('token');
                axios.post('http://localhost:5118/api/user/userInfo', { token: token })
                    .then(response => {
                        this.user = response.data;
                        console.info(response.data);
                        this.user_id = response.data.id;
                        this.user_nickname = response.data.nickname;
                        this.avatar_url = response.data.avatar ? `http://localhost:5118${response.data.avatar}` : '../../public/default.png'; //avatar 判空
                        this.credit = response.data.credit;

                        // 确保在user_id被设置之后调用fetchTransactions
                        this.fetchTransactions();
                        // TODO: 现在需要两次通信，第一次用cookies从服务器取了userinfo，等到这个info再问服务器要交易记录
                        // 理想状态是通过cookies直接能得到所有的信息
                    })
                    .catch(error => {
                        console.error('Error fetching user data:', error);
                    });
        },
            
        fetchTransactions() {
            console.info(this.user_id);
            axios.post('http://localhost:5118/api/user/getTransaction', { id: this.user_id })
                .then(response => {
                    this.transactions = response.data.$values;
                    console.info(this.transactions);
                })
                .catch(error => {
                    console.error('Error fetching transactions:', error);
                });
        },
        goToPay() {
            // 充值按钮的点击事件处理逻辑
        },
        prevPage() {
            // 上一页按钮的点击事件处理逻辑
        },
        nextPage() {
            // 下一页按钮的点击事件处理逻辑
        }
    },
    mounted() {
        this.fetchUserAndTransaction(); // 页面加载时从cookies获取用户ID，再获取交易信息
    },

    computed: {
    formattedCredit() {
      // 将credit转换为字符串并拆分为整数部分和小数部分
      let creditStr = this.credit.toFixed(2).split('.');
      let integerPart = creditStr[0];
      let decimalPart = creditStr[1];
      // 返回带有HTML标记的字符串
      return `<span>￥ </span><span class="integer-part">${integerPart}</span>.<span class="decimal-part">${decimalPart}</span>`;
    }
  }
}
</script>

<style scoped>
.Wallet {
    position: relative;
    display: flex;
    flex-direction: column;
    align-items: center;
    width: 100%;
    height: 100%;
}

.header {
    min-height: 150px;
    width: 100%;
    overflow: hidden;
    box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
    position: relative;
    border-radius: 10px;
    border: 3px solid #ffd7d2;
}

.avatar-header {
    width: 100%;
    height: 200px;
    object-fit: cover;
    filter: blur(50px)opacity(80%);
}

.avatar {
    display: flex;
    flex-direction: row;
    position: absolute;
    left: 5%;
    top: 15%;
    height: 100px;
    width: 500px;
}

.avatar-circle {
    position: relative;
    background-color: #ffffff;
    object-fit:cover;
    height: 100%;
    aspect-ratio: 1 / 1;
    border-radius: 50%;
    box-shadow: 0 4px 20px rgba(0, 0, 0, 0.447);
    z-index: 1;
}

.user-info {
    display: flex;
    flex-direction: column;
    justify-content: space-between;
    margin-bottom: 4%;
    margin-left: 5%;
}

.user-nickname {
    font-size: 40px;
    height: 70%;
    align-items: center;
    display: flex;
    flex-direction: row;
    text-shadow: 0 3px 15px rgb(255, 255, 255);
}

.user-id {
    width: 100%;
    margin-left: 5px;
    height: 70%;
    align-items: center;
    display: flex;
    flex-direction: row;
    text-shadow: 0 3px 15px rgb(255, 255, 255);
}

.auto-wrapper {
    display: flex;
    flex-direction: row;
    align-items: center;
    justify-content: space-between;
    width: 100%;
    height: 100%;
    margin: 10px 10px 3px;
}

.info-box {
    display: flex;
    flex-direction: column;
    align-items: center;
    background: #fff9f9;
    border: 3px solid #ffd7d2;
    padding: 2% 5% 2% 5%;
    width: calc(50% - 8px);
    height: 100%;
    border-radius: 10px;
    box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
}

.user-credit {
  font-size: 30px; /* 基本字体大小 */
}

::v-deep .integer-part {
  font-size: 50px;
}
::v-deep .decimal-part {
  font-size: 20px;
}

.text {
    font-family: Inika;
    font-size: 2rem;
    color: #000000;
    line-height: normal;
    text-align: left;
    vertical-align: top;
    font-weight: 400;
    white-space: normal;
    margin-bottom: 1rem;
}

.transaction-table {
    width: 100%;
    overflow-x: auto;
    margin-bottom: 1rem;
}

table {
    width: 100%;
    border-collapse: collapse;
    text-align: left;
}

th, td {
    padding: 0.5em;
    border-bottom: 1px solid #ddd;
}

th {
    background-color: #f2f2f2;
}

tr:hover {
    background-color: #f5f5f5;
}

.button-row {
    width: 100%;
    display: flex;
    justify-content: space-between;
    margin-top: 1em;
}

button {
        padding: 10px 20px;
        background-color: #fbeaea;
        font-size: 18px;
        color: #F8887D;
        border: 3px solid #FADAD6;
        border-radius: 10px;
        cursor: pointer;
        transition: background-color 0.3s, transform 0.3s, color 0.3s;
    }

        button:disabled {
            cursor: not-allowed;
        }

        button:hover:enabled {
            background-color: #ffe5e5;
            transform: scale(1.05);
            color: #F8887D;
            transition: background-color 0.3s, transform 0.3s, color 0.3s;
        }
        
</style>
