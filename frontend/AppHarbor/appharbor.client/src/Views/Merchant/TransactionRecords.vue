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
            <th @click="changeSort('time')" class="sortable">时间
              <span class="sort-icons">
                <span v-if="sortBy === 'time' && sortOrder === 'asc'" class="arrow-up">▲</span>
                <span v-if="sortBy === 'time' && sortOrder === 'desc'" class="arrow-down">▼</span>
              </span>
            </th>
            <th @click="changeSort('amount')" class="sortable">金额
              <span class="sort-icons">
                <span v-if="sortBy === 'amount' && sortOrder === 'asc'" class="arrow-up">▲</span>
                <span v-if="sortBy === 'amount' && sortOrder === 'desc'" class="arrow-down">▼</span>
              </span>
            </th>
            <th>应用ID</th>
            <th>应用名称</th>
            <th>买家ID</th>
            <th>买家昵称</th>
            <th>接收者ID</th>
            <th>接收者昵称</th>
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
            <td>{{ transaction.applicationId }}</td>
            <td>{{ transaction.applicationName }}</td>
            <td>{{ transaction.buyerId }}</td>
            <td>{{ transaction.buyerName }}</td>
            <td>{{ transaction.receiverId }}</td>
            <td>{{ transaction.receiverName }}</td>
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
// 保持现有的 JavaScript 代码  
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
      showAdvancedSearch: false,
      sortBy: 'time', // 默认按时间排序  
      sortOrder: 'desc' // 默认降序  
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
      formData.append('sortBy', this.sortBy);
      formData.append('sortOrder', this.sortOrder);

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
      return date.toISOString().split('T')[0];
    },
    formatTime(dateTime) {
      const date = new Date(dateTime);
      return date.toTimeString().split(' ')[0];
    },
    changeSort(column) {
      if (this.sortBy === column) {
        this.sortOrder = this.sortOrder === 'asc' ? 'desc' : 'asc'; // 切换排序方式  
      } else {
        this.sortBy = column; // 更新排序列  
        this.sortOrder = 'asc'; // 默认升序  
      }
      this.initiateSearch(); // 重新搜索  
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
  background-color: #f0f9ff;
  /* 更改背景色为浅蓝色 */
  padding: 20px;
  border-radius: 10px;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
  max-width: 1000px;
  margin: 0 auto;
  font-family: 'Baloo 2', cursive, Arial, sans-serif;
  font-size: 16px;
  color: #333;
  min-height: 85vh;
  max-height: 85vh;
  justify-content: space-between;
}

h1 {
  color: #1976d2;
}

.search-bar {
  display: flex;
  flex-wrap: wrap;
  gap: 5px;
  margin-bottom: 10px;
}

.global-search,
.search-button,
.advanced-search-toggle {
  flex: 1 1 150px;
  padding: 8px;
  border: 1px solid #ccc;
  border-radius: 5px;
  background-color: #fff;
  transition: border-color 0.3s, box-shadow 0.3s;
}

.search-button,
.advanced-search-toggle {
  cursor: pointer;
  background-color: #1e88e5;
  /* 更改按钮背景色为蓝色 */
  color: #fff;
}

.advanced-search-toggle:hover,
.search-button:hover {
  background-color: #1565c0;
  /* 更改悬停时背景色为深蓝色 */
}

.advanced-search {
  width: 100%;
  display: flex;
  flex-wrap: wrap;
  gap: 5px;
  margin-top: 5px;
}

.advanced-search input {
  flex: 1 1 150px;
}

.table-container {
  flex: 1;
  width: 100%;
  overflow-x: auto;
  max-height: 65vh;
  overflow-y: scroll;
}

table {
  width: 100%;
  border-collapse: collapse;
  table-layout: fixed;
  word-wrap: break-word;
  max-height: 100%;
  background-color: #fff;
}

th,
td {
  padding: 8px;
  text-align: left;
}

th {
  background-color: #42a5f5;
  /* 更改表头背景色为蓝色 */
  color: #fff;
  position: sticky;
  top: 0;
}

th,
td {
  border: 1px solid #ddd;
  border-left: none;
  border-right: none;
  text-align: center;
}

td .date {
  display: block;
}

td .time {
  display: block;
  color: #666;
  font-size: 12px;
}

.sortable {
  cursor: pointer;
}

.sort-icons {
  margin-left: 5px;
}

.arrow-up,
.arrow-down {
  font-size: 12px;
}

.pagination {
  display: flex;
  justify-content: center;
  gap: 10px;
  padding: 10px 0;
}

.pagination button {
  padding: 5px 10px;
  background-color: #1e88e5;
  /* 更改按钮背景色为蓝色 */
  color: #fff;
  border: none;
  border-radius: 5px;
  cursor: pointer;
  transition: background-color 0.3s, transform 0.3s, color 0.3s;
  font-family: 'Baloo 2', cursive;
  font-size: 14px;
}

.pagination button:disabled {
  cursor: not-allowed;
  background-color: #ccc;
}

.pagination button:hover:enabled {
  background-color: #1565c0;
  /* 更改悬停时背景色为深蓝色 */
  transform: scale(1.05);
}

.pagination span {
  display: flex;
  align-items: center;
}

@media (max-width: 768px) {

  .advanced-search-toggle,
  .search-button,
  .global-search {
    width: 100%;
  }

  .pagination {
    flex-wrap: wrap;
  }
}
</style>