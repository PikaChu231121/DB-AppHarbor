<template>
    <div class="Wallet">
        <div class="auto-wrapper">
            <div class="info-box">
                <p class="text">钱包余额</p>
                <p class="merchant-credit" v-html="formattedCredit"></p>
                <div class="button-row">
                    <input type="number" v-model="withdrawAmount" placeholder="输入提现金额" class="input-withdraw" />
                    <button type="button" class="button" @click="withdraw">提现</button>
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
            credit: -1,
            withdrawAmount: 0 // 提现金额
        };
    },
    methods: {
        fetchCredit() {
            var token = Cookies.get('token');
            axios.post('http://localhost:5118/api/merchant/getCredit', { token: token })
                .then(response => {
                    this.credit = response.data.credit;
                })
                .catch(error => {
                    console.error('Error fetching merchant data:', error);
                });
        },

        withdraw() {
            if (this.withdrawAmount <= 0) {
                alert('请输入有效的提现金额');
                return;
            }
            if (!/^\d+(\.\d{1,2})?$/.test(this.withdrawAmount)) {
                alert('请输入最多两位小数的有效金额');
                return;
            }
            var token = Cookies.get('token');
            axios.post('http://localhost:5118/api/merchant/withdrawCredit', { token: token, amount: this.withdrawAmount })
                .then(response => {
                    // console.log(response.data);
                    this.credit = response.data.newCredit;
                    alert('提现成功');
                })
                .catch(error => {
                    console.error('Error withdrawing:', error);
                    alert('提现失败，请联系管理员');
                });
        },
    },
    mounted() {
        this.fetchCredit(); // 页面加载时从cookies获取用户ID，再获取交易信息
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
.merchant-credit {
    font-size: 30px;
    /* 基本字体大小 */
}

::v-deep .integer-part {
    font-size: 50px;
}

::v-deep .decimal-part {
    font-size: 20px;
}

.input-withdraw {
    padding: 10px;
    font-size: 16px;
    margin-right: 10px;
    border: 2px solid #FADAD6;
    border-radius: 5px;
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
    overflow-y: scroll;
    margin-bottom: 1rem;
}

/* 表格基本样式 */
table {
    width: 100%;
    border-collapse: collapse;
    margin: 20px 0;
    font-size: 18px;
    text-align: left;
    background-color: #fbeaea;
    border: 3px solid #fadad6;
    border-radius: 10px;
    overflow: hidden;
    box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
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
