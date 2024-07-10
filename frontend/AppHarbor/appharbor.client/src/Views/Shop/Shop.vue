<template>
    <div class="app-search">
        <aside class="filter-section">
            <FilterSection @tags-changed="handleTagsChanged" />
        </aside>
        <main class="search-section">
            <SearchBar /> <!--@search="handleSearch"/>--> 
            <AppGrid :apps="paginatedApps" />
            <!--<AppGrid :apps="this.apps" />-->
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
                apps: [],
                Category:"All",//--------------我这先设置成All，需要调试可以改成Social或Office，你等之后加上前端的分类模块后再作具体修改
                
                currentPage: 1, // 当前页码
                totalPages: 2, // 总页数
                appsPerPage: 10, // 每页显示的应用数量
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
                axios.post('http://localhost:5118/api/application/getapplist', {
                    Category: this.Category,
                    Page: this.currentPage  
                })
                    .then(response => {
                        this.apps = response.data;
                        //console.log(2);
                        this.totalPages = Math.ceil(this.apps.length / this.appsPerPage);
                    })
                    .catch(error => {
                        console.error("Error fetching apps:", error);
                    });

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
            //console.log(1);
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