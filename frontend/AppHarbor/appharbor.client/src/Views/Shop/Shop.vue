<template>
    <div class="app-search">
        <aside class="filter-section">
            <FilterSection @tags-changed="handleTagsChange" @price-range-updated="handlePriceChange" />
        </aside>
        <main class="search-section">
            <div class="title-container">
                <h1 class="title">应用商店</h1>
            </div>
            <SearchBar @search="handleSearch" />
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
                selectedTags: [], // 选中的标签
                Category: "All", // 检索属性
                priceRange: [0, 1000],
                searchQuery: "",
                currentPage: 1, // 当前页码，初始为1
                totalPages: 1, // 总页数，初始为1
                appsPerPage: 10 // 每页显示的应用数量
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
                        this.paginatedApps();
                    })
                    .catch(error => {
                        console.error("Error fetching apps:", error);
                    });
            },
            searchApps() {
                console.log(this.searchQuery);
                console.log(this.Category);
                console.log(this.priceRange[0]);
                console.log(this.priceRange[1]);

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
            TagsIsEmpty() {
                return this.selectedTags.length === 0;
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
            handleTagsChange(newTags) {

                /*测试：先取selectedTags的第一个作为筛选*/
                this.selectedTags = newTags;
                if (this.TagsIsEmpty()) {
                    this.Category = 'All';
                } else {
                    this.Category = this.selectedTags[0];
                }
                console.log("changed");
                this.searchApps();
                console.log("TEST");
            },

            // 处理价格变化
            handlePriceChange(newRange) {
                this.priceRange = newRange;
                console.log('priceRange has changed: ' + this.priceRange[0] + ' to ' + this.priceRange[1]);
                this.searchApps();
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
        background-color: #faebd7; /* Background color for the entire search area */
        display: flex;
        justify-content: center;
        padding: 20px; /* Adjust padding for better spacing */
        height: 100%;
        width: 100%;
    }

    .filter-section {
        width: 25%; /* Adjust width for better spacing */
        margin-right: 20px;
        border-radius: 10px;
    }

    .search-section {
        width: 75%; /* Adjust width for better spacing */
        display: flex;
        flex-direction: column;
        justify-content: space-between;
        background-color: #faebd7; /* Match background color */
    }

    .title-container {
        text-align: center; /* Center align title */
        margin-bottom: 20px; /* Space between title and search bar */
    }

    .title {
        font-size: 2rem; /* Adjust font size as needed */
        color: black; /* Cute color for the title */
        font-family: 'Comic Sans MS', cursive, sans-serif; /* Cute font style */
    }

    @media (max-width: 991px) {
        .app-search {
            flex-direction: column;
            padding: 20px;
        }

        .filter-section {
            margin-bottom: 40px;
            width: 100%;
        }

        .search-section {
            width: 100%;
            margin-right: 0;
        }
    }
</style>
