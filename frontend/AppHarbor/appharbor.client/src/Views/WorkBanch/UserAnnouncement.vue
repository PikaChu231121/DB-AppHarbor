<template>
    <div class="announcement-container">
        <div class="header">
            <!-- Title -->
            <h1 class="main-title">
                <svg xmlns="http://www.w3.org/2000/svg" width="1em" height="1em" viewBox="0 0 24 24"><g fill="none" fill-rule="evenodd"><path d="M24 0v24H0V0zM12.593 23.258l-.011.002l-.071.035l-.02.004l-.014-.004l-.071-.035q-.016-.005-.024.005l-.004.01l-.017.428l.005.02l.01.013l.104.074l.015.004l.012-.004l.104-.074l.012-.016l.004-.017l-.017-.427q-.004-.016-.017-.018m.265-.113l-.013.002l-.185.093l-.01.01l-.003.011l.018.43l.005.012l.008.007l.201.093q.019.005.029-.008l.004-.014l-.034-.614q-.005-.019-.02-.022m-.715.002a.02.02 0 0 0-.027.006l-.006.014l-.034.614q.001.018.017.024l.015-.002l.201-.093l.01-.008l.004-.011l.017-.43l-.003-.012l-.01-.01z" /><path fill="currentColor" d="M19 4.741V8a3 3 0 1 1 0 6v3c0 1.648-1.881 2.589-3.2 1.6l-2.06-1.546A8.66 8.66 0 0 0 10 15.446v2.844a2.71 2.71 0 0 1-5.316.744l-1.57-5.496a4.7 4.7 0 0 1 3.326-7.73l3.018-.168a9.34 9.34 0 0 0 4.19-1.259l2.344-1.368C17.326 2.236 19 3.197 19 4.741M5.634 15.078l.973 3.407A.71.71 0 0 0 8 18.29v-3.01l-1.56-.087a5 5 0 0 1-.806-.115M17 4.741L14.655 6.11A11.3 11.3 0 0 1 10 7.604v5.819c1.787.246 3.488.943 4.94 2.031L17 17zM8 7.724l-1.45.08a2.7 2.7 0 0 0-.17 5.377l.17.015l1.45.08zM19 10v2a1 1 0 0 0 .117-1.993z" /></g></svg>
            通知公告</h1>

            <!-- Search Box -->
            <div class="search-box">
                <input type="text"
                       v-model="inputQuery"
                       @keydown.enter="handleSearch"
                       class="search-input"
                       placeholder="搜索公告标题...按回车搜索" />
            </div>
        </div>

        <!-- No Results Message -->
        <div v-if="noResults" class="no-results-message">
            没有搜索到公告
        </div>

        <!-- Announcement List -->
        <ul class="announcement-list" v-if="!noResults">
            <li v-for="(announce, index) in paginatedAnnounces" :key="announce.id" class="announcement-item">
                <h2 class="announcement-title">第{{ announce.id }}号公告：{{ announce.title }}</h2>
                <p class="announcement-details">
                    发布时间: <span class="detail-value">{{ announce.publishTime.replace('T', '-') }}</span>
                </p>
                <button class="view-detail-button" @click="openDetail(announce)">查看详细</button>
            </li>
        </ul>

        <!-- Detail Popup -->
        <div v-if="selectedAnnouncement" class="popup-overlay" @click="closeDetail">
            <div class="popup-content" @click.stop>
                <h2 class="popup-title">{{ selectedAnnouncement.title }}</h2>
                <p class="popup-details">
                    发布公告管理员: <span class="detail-value">{{ selectedAnnouncement.adminNickname }}</span>
                </p>
                <p class="popup-details">
                    发布时间: <span class="detail-value">{{ selectedAnnouncement.publishTime.replace('T', '-') }}</span>
                </p>
                <p class="popup-details">
                    公告内容:
                </p>
                <p class="popup-content-text">
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;{{ selectedAnnouncement.content }}
                </p>
                <button class="popup-close-button" @click="closeDetail">关闭</button>
            </div>
        </div>

        <!-- Pagination Controls -->
        <div class="pagination-controls" v-if="!noResults">
            <button @click="changePage(currentPage - 1)" :disabled="currentPage === 1">上一页</button>
            <span>第 {{ currentPage }} 页 / 共 {{ totalPages }} 页</span>
            <input type="number"
                   v-model.number="pageInput"
                   @input="updatePageInput"
                   min="1"
                   :max="totalPages"
                   placeholder="跳转到页" />
            <button @click="jumpToPage">跳转</button>
            <button @click="changePage(currentPage + 1)" :disabled="currentPage === totalPages">下一页</button>
        </div>
    </div>
</template>

<script>
    import axios from 'axios';

    export default {
        data() {
            return {
                announces: [],
                selectedAnnouncement: null,
                currentPage: 1,
                pageSize: 5, // 每页显示的公告数量
                pageInput: '', // 跳转页输入框的值
                inputQuery: '', // 用户输入的搜索词
                searchQuery: '' // 实际用于搜索的查询词
            };
        },
        computed: {
            noResults() {
                return this.searchQuery && this.filteredAnnounces.length === 0;
            },
            totalPages() {
                return Math.ceil(this.filteredAnnounces.length / this.pageSize);
            },
            filteredAnnounces() {
                if (this.searchQuery) {
                    return this.announces.filter(announce =>
                        announce.title.toLowerCase().includes(this.searchQuery.toLowerCase())
                    );
                }
                return this.announces;
            },
            paginatedAnnounces() {
                const start = (this.currentPage - 1) * this.pageSize;
                const end = start + this.pageSize;
                return this.filteredAnnounces.slice(start, end);
            },
            formattedCreateTime() {
                return this.paginatedAnnounces.map(announce => {
                    let dateTime = announce.publishTime;
                    // 替换"T"为空格
                    dateTime = dateTime.replace('T', ' ');
                    // 转换为 Date 对象
                    let date = new Date(dateTime);
                    // 增加8小时
                    date.setHours(date.getHours() + 8);
                    // 格式化为 YYYY-MM-DD HH:mm:ss
                    let year = date.getFullYear();
                    let month = ('0' + (date.getMonth() + 1)).slice(-2);
                    let day = ('0' + date.getDate()).slice(-2);
                    let hours = ('0' + date.getHours()).slice(-2);
                    let minutes = ('0' + date.getMinutes()).slice(-2);
                    let seconds = ('0' + date.getSeconds()).slice(-2);
                    return `${year}-${month}-${day} ${hours}:${minutes}:${seconds}`;
                });
            }  
        },
        mounted() {
            this.searchAnnouncement(); // Initialize with default announcements
        },
        methods: {
            searchAnnouncement() {
                axios.post(`${this.$Url}/api/announcement/getannouncementlist`)
                    .then(response => {
                        this.announces = response.data.$values;
                        this.pageInput = ''; // Reset input field on data fetch
                        this.adjustCurrentPage(); // Adjust currentPage if necessary
                    })
                    .catch(error => {
                        console.error('查看公告失败:', error);
                    });
            },
            handleSearch() {
                this.searchQuery = this.inputQuery; // Set search query to the input value
                this.currentPage = 1; // Reset to the first page on search
                this.searchAnnouncement(); // Fetch announcements after searching
            },
            openDetail(announcement) {
                this.selectedAnnouncement = announcement;
            },
            closeDetail() {
                this.selectedAnnouncement = null;
            },
            changePage(page) {
                if (page < 1 || page > this.totalPages) return;
                this.currentPage = page;
                this.pageInput = ''; // Clear page input on page change
            },
            jumpToPage() {
                const page = Math.max(1, Math.min(this.pageInput || 1, this.totalPages));
                if (page !== this.currentPage) {
                    this.currentPage = page;
                    this.pageInput = ''; // Clear page input after jump
                }
            },
            updatePageInput() {
                // Ensures pageInput is within valid range
                if (this.pageInput < 1) {
                    this.pageInput = 1;
                } else if (this.pageInput > this.totalPages) {
                    this.pageInput = this.totalPages;
                }
            },
            adjustCurrentPage() {
                // Adjust currentPage if necessary based on filtered results
                this.$nextTick(() => {
                    if (this.totalPages < this.currentPage) {
                        this.currentPage = this.totalPages || 1;
                    }
                });
            }
        }
    };
</script>


<style scoped>
    .announcement-container {
        padding: 20px;
        background-color: #f5f5f5;
        border-radius: 8px;
        width: 100%;
        margin: 0 auto;
        height: 100%;
    }

    .header {
        display: flex;
        align-items: center;
        justify-content: space-between;
        margin-bottom: 20px;
    }

    .main-title {
        font-size: 36px;
        color: black;
        //font-family: 'Comic Sans MS', cursive, sans-serif;
        font-weight: bold;
        margin: 0;
    }

    .search-box {
        width: 300px;
    }

    .search-input {
        width: 100%;
        padding: 10px;
        border: 1px solid #ddd;
        border-radius: 4px;
        font-size: 18px;
        //font-family: 'Comic Sans MS', cursive, sans-serif;
        font-weight: bold;
    }

    .announcement-list {
        list-style-type: none;
        padding: 0;
    }

    .announcement-item {
        margin-bottom: 20px;
        border-bottom: 1px solid #ddd;
        padding: 15px;
        background-color: #fff;
        border-radius: 8px;
        box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
        transition: transform 0.3s ease, box-shadow 0.3s ease;
        position: relative;
    }

        .announcement-item:hover {
            transform: translateY(-5px);
            box-shadow: 0 6px 12px rgba(0, 0, 0, 0.2);
        }

    .announcement-title {
        margin: 0;
        font-size: 22px;
        color: #ffa8a4;
        //font-family: 'Comic Sans MS', cursive, sans-serif;
        font-weight: bold;
    }

    .announcement-details {
        font-size: 14px;
        color: #555;
    }

    .detail-value {
        font-weight: bold;
        color: #000;
    }

    .view-detail-button {
        position: absolute;
        right: 15px;
        top: 15px;
        background-color: #ffa8a4;
        color: #fff;
        border: none;
        border-radius: 8px;
        padding: 10px 16px;
        cursor: pointer;
        transition: background-color 0.3s ease, transform 0.3s ease, box-shadow 0.3s ease;
        font-size: 14px;
        font-weight: bold;
    }

        .view-detail-button:hover {
            background-color: #f78f8c;
            transform: scale(1.05);
            box-shadow: 0 4px 10px rgba(0, 0, 0, 0.2);
        }

    .popup-overlay {
        position: fixed;
        top: 0;
        left: 0;
        width: 100%;
        height: 100%;
        background: rgba(0, 0, 0, 0.5);
        display: flex;
        align-items: center;
        justify-content: center;
        animation: fadeIn 0.3s ease;
    }

    .popup-content {
        position: relative; /* Ensure the close button is positioned relative to this container */
        background: #fff;
        padding: 20px;
        border-radius: 12px;
        width: 80%;
        max-width: 600px;
        box-shadow: 0 4px 12px rgba(0, 0, 0, 0.2);
        animation: scaleUp 0.3s ease;
    }

    .popup-title {
        margin-top: 0;
        font-size: 24px;
        //font-family: 'Comic Sans MS', cursive, sans-serif;
        font-weight: bold;
        color: #ffa8a4;
        text-shadow: 1px 1px 2px rgba(0, 0, 0, 0.2);
    }

    .popup-details {
        margin: 10px 0;
        font-size: 16px;
        color: #555;
    }

    .popup-content-text {
        white-space: pre-line;
        font-size: 16px;
        color: #333;
    }

    .popup-close-button {
        position: absolute; /* Position the button in the absolute space of the popup-content */
        top: 10px;
        right: 10px;
        background: linear-gradient(to right, #f78f8c, #e06666);
        color: #fff;
        border: none;
        border-radius: 8px;
        padding: 12px 20px;
        cursor: pointer;
        transition: background-color 0.3s ease, transform 0.3s ease, box-shadow 0.3s ease;
        font-size: 16px;
        font-weight: bold;
    }

        .popup-close-button:hover {
            background: linear-gradient(to right, #e06666, #f78f8c);
            transform: scale(1.05);
            box-shadow: 0 8px 15px rgba(0, 0, 0, 0.3);
        }

    @keyframes fadeIn {
        from {
            opacity: 0;
        }

        to {
            opacity: 1;
        }
    }

    @keyframes scaleUp {
        from {
            transform: scale(0.9);
            opacity: 0;
        }

        to {
            transform: scale(1);
            opacity: 1;
        }
    }

    .pagination-controls {
        display: flex;
        justify-content: center;
        //font-family: 'Comic Sans MS', cursive, sans-serif;
        font-weight: bold;
        align-items: center;
        margin-top: 20px;
    }

        .pagination-controls button {
            margin: 0 5px;
            padding: 5px 10px;
            font-size: 18px;
            border: 1px solid #ddd;
            //font-family: 'Comic Sans MS', cursive, sans-serif;
            font-weight: bold;
            border-radius: 4px;
            background-color: #ffa8a4;
            color: #fff;
            cursor: pointer;
            transition: background-color 0.3s ease;
        }

            .pagination-controls button:disabled {
                background-color: #ccc;
                cursor: not-allowed;
            }

            .pagination-controls button:hover:not(:disabled) {
                background-color: #f78f8c;
            }

        .pagination-controls input {
            width: 60px;
            margin: 0 10px;
            //font-family: 'Comic Sans MS', cursive, sans-serif;
            font-weight: bold;
            padding: 5px;
            border: 1px solid #ddd;
            border-radius: 4px;
            font-size: 14px;
        }

    .no-results-message {
        text-align: center;
        font-size: 18px;
        color: #888;
        margin: 20px 0;
        font-weight:bold;
        //font-family: 'Comic Sans MS', cursive, sans-serif;
    }
</style>
