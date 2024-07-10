<template>
    <div class="root-group-1">
        <div class="header">
            <img src="../../public/avatar/default.png" class="avatar-header" />
        </div>
        <div class="avatar">
            <img src="../../public/avatar/default.png" class="avatar-circle" />
            <div class="user-info">
                <p class="user-nickname">{{ user_nickname }}</p>
                <p class="user-id">{{ user_id }}</p>
            </div>
        </div>

        <div class="auto-wrapper">
            <div class="info-box">
                <p class="text">钱包余额</p>
                <div class="button-row">
                    <button type="button" class="button" @click="goToLogin">充值</button>
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
                                <td>{{ transaction.date }}</td>
                                <td>{{ transaction.description }}</td>
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
export default {
    data() {
        return {
            user_nickname: '4534',
            user_id: '5357535',
            transactions: [
                { date: '2024-07-01', description: '充值', amount: '+100.00' },
                { date: '2024-07-02', description: '消费', amount: '-50.00' },
                { date: '2024-07-03', description: '消费', amount: '-20.00' },{ date: '2024-07-02', description: '消费', amount: '-50.00' },

            ]
        };
    },
    methods: {
        goToLogin() {
            // 充值按钮的点击事件处理逻辑
        },
        prevPage() {
            // 上一页按钮的点击事件处理逻辑
        },
        nextPage() {
            // 下一页按钮的点击事件处理逻辑
        }
    }
}
</script>

<style lang="scss" scoped>
.root-group-1 {
    position: relative;
    display: flex;
    flex-direction: column;
    align-items: center;
    width: 100%;
}

.header {
    height: 14%;
    width: 100%;
    overflow: hidden;
    box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
    position: relative;
    border-radius: 10px;
}

.avatar-header {
    width: 100%;
    height: 200px;
    object-fit: cover;
    filter: blur(50px);
}

.avatar {
    display: flex;
    flex-direction: row;
    position: absolute;
    left: 5%;
    top: 8%;
    height: 100px;
}

.avatar-circle {
    position: relative;
    background-color: #ffffff;
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
    margin-left: 15%;
}

.user-nickname {
    font-size: 60px;
    height: 70%;
    align-items: center;
    display: flex;
    flex-direction: row;
    text-shadow: 0 3px 15px rgb(255, 255, 255);
}
.user-id {
    // font-size: 60px;
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
    height: auto;
    margin: 2.4% 0 0;
}

.info-box {
    display: flex;
    flex-direction: column;
    align-items: center;
    background: #e9f2fb;
    padding: 2% 1% 2% 2%;
    width: 49%;
    height: 500px;
    border-radius: 10px;
    box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
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

.button {
    width: 40%;
    padding: 0.5em;
    border: none;
    border-radius: 4px;
    background-color: #42b983;
    color: white;
    font-size: 1em;
    cursor: pointer;
}

.button:hover {
    background-color: #3ca676;
    box-shadow: 0 4px 8px rgba(0, 0, 0, 0.3);
}
</style>
