<template>
    <div class="app-search">
        <aside class="filter-section">
            <FilterSection @tag-changed="handleTagChange" @price-range-updated="handlePriceChange" />
        </aside>
        <main class="search-section">
            <div class="title-container">
                <h1 class="title">
                    <svg xmlns="http://www.w3.org/2000/svg" width="1em" height="1em" viewBox="0 0 24 24"><path fill="currentColor" d="M11.4 9.6v4.2H7.2V9.6zm0 9.6V15H7.2v4.2zm5.4-9.6v4.2h-4.2V9.6zm0 9.6V15h-4.2v4.2zM7.2 5.4V2.7c0-1.16.94-2.1 2.1-2.1h5.4c1.16 0 2.1.94 2.1 2.1v2.7h6.3a.9.9 0 0 1 .9.9v13.8a3.3 3.3 0 0 1-3.3 3.3H3.3A3.3 3.3 0 0 1 0 20.1V6.3a.9.9 0 0 1 .9-.9zM9 2.7v2.7h6V2.7a.3.3 0 0 0-.3-.3H9.3a.3.3 0 0 0-.3.3M1.8 20.1a1.5 1.5 0 0 0 1.5 1.5h17.4a1.5 1.5 0 0 0 1.5-1.5V7.2H1.8z" /></svg>
                应用商店</h1>
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
                sortMethod: 'Rating', // 添加排序方式
            }
        },
        methods: {
            fetchApps() {
                console.log('fetchApps has been execute!');
                axios.post('http://localhost:5118/api/application/getapplist', {
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
                axios.post('http://localhost:5118/api/application/searchapplist', {
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
                if (this.sortMethod === '价格') {
                    this.apps.sort((a, b) => b.price - a.price);
                } else if (this.sortMethod === '评分') {
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
        background-color: whitesmoke; /* Background color for the entire search area */
        display: flex;
        //height: 80%;
        //justify-content: center;
        //padding: 10px; /* Adjust padding for better spacing */
        //height: 95%;
        //width: 110%;
    }

    .FilterSection {
        width: 22vh; 
        height: 85vh;
        border-radius: 12px;
        background-color: white;
        padding: 20px;
        box-sizing: border-box;
        transition: background-color 0.3s ease;
        border: 2px solid #d3d3d3;
    }

    .search-section {
        width: 120%; /* Adjust width for better spacing */
        display: flex;
        flex-direction: column;
        //justify-content: space-between;
        //background-color: black; /* Match background color */
    }

    .title-container {
        text-align: center; /* Center align title */
    }

    .title {
        font-size: 2rem; /* Adjust font size as needed */
        color: black; /* Cute color for the title */
        //font-family: 'Comic Sans MS', cursive, sans-serif; /* Cute font style */
    }

</style>
