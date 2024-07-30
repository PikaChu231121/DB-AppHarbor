<template>
    <div class="transaction-records">
        <h1>交易记录</h1>
        <div class="search-bar">
            <input v-model="searchQuery" placeholder="搜索交易记录..." />
            <input type="date" v-model="startDate" />
            <input type="date" v-model="endDate" />
            <button @click="initiateSearch">搜索</button>
        </div>
        <table>
            <thead>
                <tr>
                    <th>ID</th>
                    <th>时间</th>
                    <th>金额</th>
                    <th>应用名称</th>
                    <th>买家名称</th>
                    <th>接收者名称</th>
                    <th>应用ID</th>
                    <th>买家ID</th>
                    <th>接收者ID</th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="transaction in transactions" :key="transaction.id">
                    <td>{{ transaction.id }}</td>
                    <td>{{ transaction.time }}</td>
                    <td>{{ transaction.amount }}</td>
                    <td>{{ transaction.applicationName }}</td>
                    <td>{{ transaction.buyerName }}</td>
                    <td>{{ transaction.receiverName }}</td>
                    <td>{{ transaction.applicationId }}</td>
                    <td>{{ transaction.buyerId }}</td>
                    <td>{{ transaction.receiverId }}</td>
                </tr>
            </tbody>
        </table>
        <div class="pagination">
            <button @click="prevPage" :disabled="currentPage === 1">上一页</button>
            <span>第 {{ currentPage }} 页</span>
            <button @click="nextPage" :disabled="currentPage === totalPages">下一页</button>
        </div>
    </div>
</template>

<script>
import Cookies from 'js-cookie';
import axios from 'axios';

export default {
    data() {
        return {
            transactions: [],
            currentPage: 1,
            totalPages: 1,
            searchQuery: '',
            startDate: '',
            endDate: ''
        };
    },
    methods: {
        fetchTransactions(page = 1) {
            const token = Cookies.get('token');
            let formData = new FormData();
            formData.append('token', token);
            formData.append('search', this.searchQuery);
            formData.append('startDate', this.startDate);
            formData.append('endDate', this.endDate);
            formData.append('page', page);

            axios.post('http://localhost:5118/api/merchant/getTransactions', formData)
                .then(response => {
                    this.transactions = response.data.transactions.$values;
                    this.totalPages = response.data.totalPages;
                    this.currentPage = page;
                })
                .catch(error => {
                    console.error('Error fetching transactions:', error);
                });
        },
        initiateSearch() {
            this.currentPage = 1;
            this.fetchTransactions();
        },
        fetchPage(page) {
            if (page > 0 && page <= this.totalPages) {
                this.fetchTransactions(page);
            }
        },
        prevPage() {
            if (this.currentPage > 1) {
                this.fetchTransactions(this.currentPage - 1);
            }
        },
        nextPage() {
            if (this.currentPage < this.totalPages) {
                this.fetchTransactions(this.currentPage + 1);
            }
        }
    },
    mounted() {
        this.fetchTransactions();
    }
};
</script>

<style>
.transaction-records {
    padding: 20px;
    background-color: #e8f5e9;
    border-radius: 8px;
    box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
}

h1 {
    color: #388e3c;
}

.search-bar {
    margin-bottom: 20px;
}

input {
    padding: 8px;
    border: 1px solid #ccc;
    border-radius: 4px;
    width: 100%;
    max-width: 300px;
}

table {
    width: 100%;
    border-collapse: collapse;
    margin-top: 20px;
}

th,
td {
    border: 1px solid #ddd;
    padding: 8px;
    text-align: left;
}

th {
    background-color: #a5d6a7;
    color: #2e7d32;
}

.pagination {
    margin-top: 20px;
    display: flex;
    justify-content: space-between;
    align-items: center;
}

button {
    padding: 8px 16px;
    border: none;
    background-color: #66bb6a;
    color: white;
    border-radius: 4px;
    cursor: pointer;
}

button:disabled {
    background-color: #c8e6c9;
    cursor: not-allowed;
}
</style>