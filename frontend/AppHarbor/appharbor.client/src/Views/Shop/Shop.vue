<template>
    <div class="app-search">
        <aside class="filter-section">
            <FilterSection @tag-changed="handleTagChange" @price-range-updated="handlePriceChange" />
        </aside>
        <main class="search-section">
            <div class="title-container">
                <!img loading="lazy" src="@/../public/shop.svg" class="shop-text-image" />
                <span class="title">应用商店</span>
            </div>
            <SearchBar 
                       @search="handleSearch"
                       @sort-method-changed="handleSortMethodChange"
            />
            <AppGrid :apps="appsShown" />
            <Pagination :total-pages="totalPages" v-model:current-page="currentPage" @page-changed="handlePageChange" />
        </main>
    </div>
</template>

<script>
    import axios from 'axios';
    import FilterSection from './FliterSection.vue';
    import SearchBar from './SearchBar.vue';
    import AppGrid from './AppGrid.vue';
    import Pagination from './Pagination.vue';

    export default {
        name: 'Shop',
        components: {
            FilterSection,
            SearchBar,
            AppGrid,
            Pagination
        },
        data() {
            return {
                apps: [], // 后端返回的应用列表
                appsShown: [], // 当前页显示应用
                selectedTag: '全部', // 选中的标签
                Category: "All", // 检索属性
                priceRange: [0, 1000],
                searchQuery: "",
                currentPage: 1, // 当前页码，初始为1
                totalPages: 1, // 总页数，初始为1
                appsPerPage: 10, // 每页显示的应用数量
                sortMethod: '按热度排序', // 添加排序方式
            }
        },
        methods: {
            fetchApps() {
                console.log('fetchApps has been execute!');
                axios.post(`${this.$Url}/api/application/getapplist`, {
                    Category: this.Category
                })
                    .then(response => {
                        this.apps = response.data.$values;
                        this.totalPages = Math.ceil(this.apps.length / this.appsPerPage);
                        this.currentPage = 1; // 重置到第一页
                        this.sortApps();
                        this.paginatedApps();
                    })
                    .catch(error => {
                        console.error("Error fetching apps:", error);
                    });
            },
            searchApps() {
                axios.post(`${this.$Url}/api/application/searchapplist`, {
                    Category: this.Category,
                    Price_min: this.priceRange[0],
                    Price_max: this.priceRange[1],
                    Content: this.searchQuery
                })
                    .then(response => {
                        this.apps = response.data.$values;
                        this.totalPages = Math.ceil(this.apps.length / this.appsPerPage);
                        this.currentPage = 1; // 重置到第一页
                        this.sortApps();
                        this.paginatedApps();
                    })
                    .catch(error => {
                        console.error("Error searching apps:", error);
                    });
                console.log('num of apps searched: ' + this.apps.length);
            },

            // 计算当前页需要展示的应用
            paginatedApps() {
                const start = (this.currentPage - 1) * this.appsPerPage;
                const end = start + this.appsPerPage < this.apps.length ? start + this.appsPerPage : this.apps.length;
                console.log('slice apps from ' + start + ' to ' + end + ' in apps');
                this.appsShown = this.apps.slice(start, end);
            },

            // 判断应用列表是否为空
            AppsIsEmpty() {
                return this.apps.length === 0;
            },

            // 判断标签数组是否为空
            //TagsIsEmpty() {
            //    return this.selectedTags.length === 0;
            //},
            sortApps() {
                if (this.sortMethod === '按价格排序') {
                    this.apps.sort((a, b) => b.price - a.price);
                } else if (this.sortMethod === '按热度排序') {
                    this.apps.sort((a, b) => b.downloadCount - a.downloadCount);
                }
            },
            // 处理搜索操作
            handleSearch(searchTerm) {
                this.searchQuery = searchTerm;
                this.searchApps();
            },

            // 处理当前页号的变化
            handlePageChange(newPage) {
                this.currentPage = newPage;
                this.paginatedApps();
            },

            // 处理当前标签的变化
            handleTagChange(newTag) {
                /*测试：先取selectedTags的第一个作为筛选*/
                this.selectedTag = newTag;
                this.Category = (newTag ===  '全部' ? 'All' : this.selectedTag);
                this.searchApps();
            },

            // 处理价格变化
            handlePriceChange(newRange) {
                this.priceRange = newRange;
                this.searchApps();
            },
            handleSortMethodChange(newSortMethod) {
                this.sortMethod = newSortMethod;
                this.sortApps();
                this.paginatedApps(); // 重新计算显示的应用
            }
        },
        created() {
            this.fetchApps();
        }
    }
</script>

<style scoped>
    .app-search {
        border-radius: 20px;
        background-color: white; /* Background color for the entire search area */
        display: flex;
        background-image: url("@/../public/logo-text.png");
        background-repeat: no-repeat;
        background-size: cover;
        background-position: center;
    }

    .FilterSection {
        width: 22vh;
        height: 85vh;
        border-radius: 12px;
        //background-color: #FCF8F8;
        padding: 20px;
        box-sizing: border-box;
        //transition: background-color 0.3s ease;
        //border: 2px solid #d3d3d3;
    }

    .search-section {
        width: 120%; /* Adjust width for better spacing */
        display: flex;
        flex-direction: column;
        //justify-content: space-between;
        //background-color: black; /* Match background color */
        
    }


    .title-container {
        display: flex;
        align-items: center; /* 使图片和文字垂直居中 */
        text-align: end;
        border-bottom: 4px solid darksalmon; /* 添加一个粗的下划线 */
        padding-bottom: 10px; /* 添加一点内边距，使下划线与内容有适当的间距 */
    }

    .title-container img {
        width: auto; /* 根据需要调整图片宽度 */
        height: 50px; /* 将图片高度设置为和文字相近，1em等于当前字体大小 */
        margin-left: 20px;
        margin-right: 20px;
        color:antiquewhite;
    }

    .title {
        margin-left: 30px;
        font-family: 'PingFang SC', 'Microsoft YaHei', 'Source Han Sans', 'Noto Sans CJK SC', 'Hiragino Sans GB', sans-serif !important;
        font-size: 60px; /* 合适的字体大小 */
        color: darksalmon; /* 舒适的深灰色 */
        font-weight: bold !important; /* 设置粗体 */
    }


</style>
