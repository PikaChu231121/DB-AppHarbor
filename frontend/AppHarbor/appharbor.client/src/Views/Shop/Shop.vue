<template>
    <div class="app-search">
        <aside class="filter-section">
            <FilterSection @tags-changed="handleTagsChanged" />
        </aside>
        <main class="search-section">
            <SearchBar /> <!--@search="handleSearch"/>--> 
            <AppGrid :apps="paginatedApps" />
            <Pagination :total-pages="totalPages" v-model:current-page="currentPage" @page-changed="handlePageChange" />
        </main>
    </div>
</template>

<script>
    import FilterSection from './FliterSection.vue';
    import SearchBar from './SearchBar.vue';
    import AppGrid from './AppGrid.vue';
    import Pagination from './Pagination.vue';
    import axios from 'axios';

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
                apps: [
                    { id: 1, image: 'https://via.placeholder.com/100', name: 'App 1', price: '$1.99', category: 'Category A' },
                    { id: 2, image: 'https://via.placeholder.com/100', name: 'App 2', price: '$2.99', category: 'Category A' },
                    { id: 3, image: 'https://via.placeholder.com/100', name: 'App 3', price: '$3.99', category: 'Category A' },
                    { id: 4, image: 'https://via.placeholder.com/100', name: 'App 4', price: '$4.99', category: 'Category A' },
                    { id: 5, image: 'https://via.placeholder.com/100', name: 'App 5', price: '$5.99', category: 'Category A' },
                    { id: 6, image: 'https://via.placeholder.com/100', name: 'App 6', price: '$6.99', category: 'Category B' },
                    { id: 7, image: 'https://via.placeholder.com/100', name: 'App 7', price: '$7.99', category: 'Category B' },
                    { id: 8, image: 'https://via.placeholder.com/100', name: 'App 8', price: '$8.99', category: 'Category B' },
                    { id: 9, image: 'https://via.placeholder.com/100', name: 'App 9', price: '$9.99', category: 'Category B' },
                    { id: 10, image: 'https://via.placeholder.com/100', name: 'App 10', price: '$10.99', category: 'Category B' },
                    { id: 11, image: 'https://via.placeholder.com/100', name: 'App 11', price: '$11.99', category: 'Category B' },
                    { id: 12, image: 'https://via.placeholder.com/100', name: 'App 12', price: '$12.99', category: 'Category B' }
                ], // 存储所有应用信息
                currentPage: 1, // 当前页码
                totalPages: 2, // 总页数
                appsPerPage: 10 // 每页显示的应用数量
                selectedTags: [] // 选中的标签
            }
        },
        computed: {
            paginatedApps() {
                // 计算当前页需要展示的应用
                const start = (this.currentPage - 1) * this.appsPerPage;
                const end = start + this.appsPerPage;
                return this.apps.slice(start, end);
            }
        },
        methods: {
            fetchApps() {
                // 从远端数据库获取应用信息
                const payload = {
                    Category: this.selectedTags.length > 0 ? this.selectedTags.join(',') : "All",
                    Page: this.currentPage
                };

                axios.post('https://localhost:5118/application/getapplist', payload)
                    .then(response => {
                        this.apps = response.data;
                        this.totalPages = Math.ceil(response.data.total / this.appsPerPage); // 假设response.data包含total字段
                    })
                    .catch(error => {
                        console.error("Error fetching apps:", error);
                    });
            },
            handleTagsChanged(tags) {
                this.selectedTags = tags;
                this.currentPage = 1;
                this.fetchApps();
            },
            handleSearch(searchTerm) {
                //// 根据搜索词过滤应用
                //// 这里假设后端支持搜索查询，返回搜索结果
                //axios.get(`https://api.example.com/apps?search=${searchTerm}`)
                //    .then(response => {
                //        this.apps = response.data.apps;
                //        this.totalPages = Math.ceil(this.apps.length / this.appsPerPage);
                //        this.currentPage = 1; // 重置到第一页
                //    })
                //    .catch(error => {
                //        console.error("Error searching apps:", error);
                //    });

                // 根据搜索词过滤应用（这里是前端模拟过滤）
                const filteredApps = this.apps.filter(app => app.name.toLowerCase().includes(searchTerm.toLowerCase()));
                this.apps = filteredApps;
                this.totalPages = Math.ceil(this.apps.length / this.appsPerPage);
                this.currentPage = 1; // 重置到第一页
            },
            handlePageChange(newPage) {
                this.currentPage = newPage;
                this.fetchApps();
            }
        },
        created() {
            // 初始获取应用信息
            this.fetchApps();
        }
    }
</script>

<style scoped>
    .app-search {
        border-radius: 20px;
        background-color: var(--sds-color-background-default-default);
        display: flex;
        justify-content: center;
        /*padding: 20px;*/
        padding-top:10px;
        padding-bottom:10px;

        background-color:aqua; /*测试颜色*/
        height:100%;
        width:100%;
    }

    .filter-section {
        width: 30%;
        height: 100%;
        margin-right: 20px;
        border-radius: 10px;

        margin-right: auto; /* 将搜索区域向右对齐 */
        margin-left: 10px; /* 增加右侧距离 */
    }

    .search-section {
        width: 70%;
        height: 100%;
        border-radius: 10px;

        display: flex;
        flex-direction: column; /* 垂直布局 */
        justify-content: space-between; /* 将内容分布在顶部和底部 */

        margin-left: auto; /* 将搜索区域向右对齐 */
        margin-right: 20px; /* 增加右侧距离 */
        background-color: aquamarine; /*测试颜色*/
    }

    @media (max-width: 991px) {
        .app-search {
            flex-direction: column;
            padding: 20px;
        }

        .filter-section {
            margin-bottom: 40px;
        }

        .search-section {
            width: 100%;
            margin-right: 0;
        }

    }
</style>