<template>
    <div class="transaction-records">
        <h1>交易记录</h1>
        <div class="search-bar">
            <input v-model="searchQuery" placeholder="全局搜索..." class="global-search" />
            <button @click="toggleAdvancedSearch" class="advanced-search-toggle">
                {{ showAdvancedSearch ? '隐藏高级检索' : '高级检索' }}
            </button>
            <div v-if="showAdvancedSearch" class="advanced-search">
                <input v-model="applicationId" placeholder="应用ID搜索..." />
                <input v-model="buyerId" placeholder="买家ID搜索..." />
                <input v-model="receiverId" placeholder="接收者ID搜索..." />
                <input v-model="applicationName" placeholder="应用名称搜索..." />
                <input v-model="buyerName" placeholder="买家昵称搜索..." />
                <input v-model="receiverName" placeholder="接收者昵称搜索..." />
                <input type="date" v-model="startDate" placeholder="开始日期..." />
                <input type="date" v-model="endDate" placeholder="结束日期..." />
            </div>
            <button @click="initiateSearch" class="search-button">搜索</button>
        </div>
        <div class="table-container">
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
                        <td>
                            <div class="date">{{ formatDate(transaction.time) }}</div>
                            <div class="time">{{ formatTime(transaction.time) }}</div>
                        </td>
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
        </div>
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
            applicationId: '',
            buyerId: '',
            receiverId: '',
            applicationName: '',
            buyerName: '',
            receiverName: '',
            startDate: '',
            endDate: '',
            showAdvancedSearch: false
        };
    },
    methods: {
        fetchTransactions(page = 1) {
            const token = Cookies.get('token');
            let formData = new FormData();
            formData.append('token', token);
            formData.append('search', this.searchQuery);
            formData.append('applicationId', this.applicationId);
            formData.append('buyerId', this.buyerId);
            formData.append('receiverId', this.receiverId);
            formData.append('applicationName', this.applicationName);
            formData.append('buyerName', this.buyerName);
            formData.append('receiverName', this.receiverName);
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
        },
        toggleAdvancedSearch() {
            this.showAdvancedSearch = !this.showAdvancedSearch;
        },
        formatDate(dateTime) {
            const date = new Date(dateTime);
            return date.toISOString().split('T')[0]; // 返回 YYYY-MM-DD  
        },
        formatTime(dateTime) {
            const date = new Date(dateTime);
            return date.toTimeString().split(' ')[0]; // 返回 HH:MM:SS  
        }
    },
    mounted() {
        this.fetchTransactions();
    }
};  
</script>

<style scoped>  
.transaction-records {  
  display: flex;  
  flex-direction: column;  
  background-color: #e8f5e9;  
  padding: 10px; /* 减少内边距 */  
  border-radius: 10px;  
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);  
  max-width: 1000px;  
  margin: 10px auto; /* 减少外边距 */  
  font-family: 'Baloo 2', cursive, Arial, sans-serif;  
  font-size: 16px; /* 调整字体大小 */  
  color: #333;  
  min-height: 80vh; /* 设置最小高度 */  
  justify-content: space-between; /* 保持分页按钮在底部 */  
}  

.search-bar {  
  display: flex;  
  flex-wrap: wrap;  
  gap: 5px; /* 减少间距 */  
  margin-bottom: 10px;  
}  

.global-search,  
.search-button,  
.advanced-search-toggle {  
  flex: 1 1 150px; /* 减少宽度 */  
  padding: 8px; /* 减少内边距 */  
  border: 1px solid #ccc;  
  border-radius: 5px;  
  background-color: #fff;  
  transition: border-color 0.3s, box-shadow 0.3s;  
}  

.advanced-search-toggle {  
  cursor: pointer;  
  background-color: #26a69a;  
  color: #fff;  
}  

.advanced-search-toggle:hover,  
.search-button:hover {  
  background-color: #00796b;  
}  

.advanced-search {  
  width: 100%;  
  display: flex;  
  flex-wrap: wrap;  
  gap: 5px; /* 减少间距 */  
  margin-top: 5px; /* 减少顶部间距 */  
}  

.advanced-search input {  
  flex: 1 1 150px; /* 减少宽度 */  
}  

.table-container {  
  flex: 1;  
  width: 100%;  
  overflow-x: auto;  
}  

table {  
  width: 100%;  
  border-collapse: collapse;  
  table-layout: fixed;  
  max-height: 60vh; /* 设置最大高度 */  
  overflow-y: auto; /* 当内容超过最大高度时启用滚动 */  
}  

th, td {  
  padding: 6px; /* 减少内填充 */  
  text-align: left;  
  word-wrap: break-word;  
}  

th {  
  background-color: #00796b;  
  color: #fff;  
}  

th, td {  
  border: 1px solid #ddd;  
}  

td .date {  
  display: block;  
}  

td .time {  
  display: block;  
  color: #666; /* 时间的颜色稍微淡一些 */  
  font-size: 12px; /* 时间的字体稍微小一些 */  
}  

.pagination {  
  display: flex;  
  justify-content: center;  
  gap: 10px; /* 减少间距 */  
  padding: 10px 0; /* 增加内边距确保分页按钮在底部 */  
}  

.pagination button {  
  padding: 5px 10px; /* 减少内边距 */  
  background-color: #26a69a;  
  color: #fff;  
  border: none;  
  border-radius: 5px;  
  cursor: pointer;  
  transition: background-color 0.3s, transform 0.3s, color 0.3s;  
  font-family: 'Baloo 2', cursive;  
  font-size: 14px; /* 调整字体大小 */  
}  

.pagination button:disabled {  
  cursor: not-allowed;  
  background-color: #ccc;  
}  

.pagination button:hover:enabled {  
  background-color: #00796b;  
  transform: scale(1.05);  
}  

.pagination span {  
  display: flex;  
  align-items: center;  
}  

@media (max-width: 768px) {  
  .advanced-search-toggle, .search-button, .global-search {  
    width: 100%;  
  }  

  .pagination {  
    flex-wrap: wrap;  
  }  
}  
</style>