<template>
    <div class="app-search">
        <aside class="filter-section">
            <FilterSection @tags-changed="handleTagsChange" @price-range-updated = "handlePriceChange" />
        </aside>
        <main class="search-section">
            <!--<SearchBar />--> 
            <SearchBar @search="handleSearch"/>
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
                /*searchedApps: [],*/
                appsShown: [], // 当前页显示应用
                selectedTags: [], // 选中的标签
                Category: "All", // 检索属性
                priceRange: [0,100],
                searchQuery: "",
                
                currentPage: 1, // 当前页码，初始为1
                totalPages: 1, // 总页数，初始为1
                appsPerPage: 10 // 每页显示的应用数量
            }
        },
        methods: {
            fetchApps() {
                console.log(this.searchQuery);
                console.log(this.Category);
                console.log(this.priceRange[0]);
                console.log(this.priceRange[1]);
                //// 从远端数据库获取应用信息
                //axios.post('http://localhost:5118/api/application/getapplist', {
                //    Category: this.Category
                //    /*Page: this.currentPage */  
                //})
                //    .then(response => {
                //        this.apps = response.data.$values;
                //        //console.log(2);
                //        /*this.searchedApps=this.apps;*/
                //        this.totalPages = Math.ceil(this.apps.length / this.appsPerPage);
                //        this.paginatedApps();
                //    })
                //    .catch(error => {
                //        console.error("Error fetching apps:", error);
                //    });
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
                console.log(this.apps.length);
            },
            paginatedApps() {
                // 计算当前页需要展示的应用
                const start = (this.currentPage - 1) * this.appsPerPage;
                const end = start + this.appsPerPage < this.apps.length ? start + this.appsPerPage : this.apps.length;
                //console.log(Array.isArray(this.apps));
                console.log('appsShown changed');
                this.appsShown = this.apps.slice(start, end); // slice不取最后一个元素
            },
            AppsIsEmpty() {
                // 判断应用列表是否为空
                return this.apps.length === 0;
            },
            TagsIsEmpty() {
                // 判断标签数组是否为空
                return this.selectedTags.length === 0;
            },
            handleSearch(searchTerm) {

                this.searchQuery = searchTerm;
                //console.log(this.searchQuery);
                //console.log(this.Category);
                //console.log(this.priceRange[0]);
                //console.log(this.priceRange[1]);
                //console.log(this.apps.length);
                // 根据搜索词过滤应用
                // 这里假设后端支持搜索查询，返回搜索结果
                //axios.post('http://localhost:5118/api/application/searchapplist', {
                //    Category: this.Category,

                //    Price_min: this.priceRange[0],

                //    Price_max: this.priceRange[1],

                //    Content: this.searchQuery

                //})
                //    .then(response => {
                //        this.apps = response.data.$values;
                //        this.totalPages = Math.ceil(this.apps.length / this.appsPerPage);
                //        this.currentPage = 1; // 重置到第一页
                //        this.paginatedApps();
                //    })
                //    .catch(error => {
                //        console.error("Error searching apps:", error);
                //    });
                //if (searchTerm.trim() === "") {
                //    // 搜索词为空时，重新获取所有应用
                //    this.fetchApps();
                //} else {
                //    // 根据搜索词过滤应用
                //    const searchResults = this.apps.filter(app => app.name.toLowerCase().includes(searchTerm.toLowerCase()));
                //    this.searchedApps = searchResults;
                //    this.totalPages = Math.ceil(this.searchedApps.length / this.appsPerPage);
                //    this.currentPage = 1; // 重置到第一页
                //    this.paginatedApps();
                //}
                this.fetchApps();
            },
            handlePageChange(newPage) {
                // 处理当前页号的变化
                this.currentPage = newPage;
                /*this.fetchApps();*/
                this.paginatedApps();
            },
            handleTagsChange(newTags) {
                // 处理当前标签的变化
                this.selectedTags = newTags;
                /*测试：先取selectedTags的第一个作为筛选*/
                if (this.TagsIsEmpty()) {
                    /*this.selectedTags.push('All');*/
                    this.Category = 'All';
                } else {
                    this.Category = this.selectedTags[0];
                }

                this.fetchApps();
            },
            handlePriceChange(newRange) {
                this.priceRange = newRange;
                console.log('priceRange has changed');
                //console.log(this.priceRange[0]);
                //console.log(this.priceRange[1]);
                this.fetchApps();
            }
        },
        created() {
            // 初始获取应用信息
            //console.log(1);
            this.fetchApps();
            /*this.paginatedApps();*/
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