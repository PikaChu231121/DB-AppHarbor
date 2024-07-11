<template>
    <div class="search-bar">
        <form class="search-form" @submit.prevent="handleSearch">
            <label for="searchInput" class="visually-hidden">Search</label> <!--输入框-->
            <input id="searchInput"
                    type="text"
                    class="search-input"
                    placeholder="Search"
                    v-model="searchQuery" />
            <button type="submit" class="search-button">
                <!--搜索按钮-->
                <img src="https://cdn.builder.io/api/v1/image/assets/TEMP/ceee35d297d4c0fe5453107e73e7fbb494e6164c747eb1afdb410a1be3052fd4?apiKey=b4c87aa6fd1245589700a3931ad0dfbf&" alt="Search" class="search-icon" />
            </button>
        </form>
        <div class="filter-tags">
            <!--搜索框后的过滤器(按价格升序、降序，按排行排序等)-->
            <span class="filter-tag active">Price descending</span>
            <span class="filter-tag">Rating</span>
        </div>
    </div>
</template>

<script>
    export default {
        name: 'SearchBar',
        data() {
            return {
                searchQuery: ''
            }
        },
        methods: {
            handleSearch() {
                // Implement search functionality
                console.log('Searching for:', this.searchQuery);
                this.$emit('search', this.searchQuery);
            }
        }
    }
</script>

<style scoped>
    .search-bar {
        display: flex;
        align-items: center;
        justify-content: space-evenly; /* 确保子元素之间的空间分配 */

        margin-top: 10px;
        height: 50px;
        width: 100%;
        border-radius: 20px;
        background-color: antiquewhite; /*背景色用于调试位置*/
    }

    .search-form {
        display: flex;
        align-items: center;
        border: 1px solid rgba(217, 217, 217, 1);
        border-radius: 20px;
        padding: 12px 16px;
        background-color: var(--sds-color-background-default-default);

        height:100%;
        width:65%;
    }

    .visually-hidden {
        position: absolute;
        width: 1px;
        height: 1px;
        padding: 0;
        margin: -1px;
        overflow: hidden;
        clip: rect(0, 0, 0, 0);
        white-space: nowrap;
        border: 0;
    }

    .search-input {
        flex: 1;
        border: none;
        font: 400 16px/140% Inter, sans-serif;
        color: var(--sds-color-text-default-tertiary);
        background: transparent;
    }

        .search-input:focus {
            outline: none;
        }

    .search-button {
        background: none;
        border: none;
        cursor: pointer;
        padding: 0;
    }

    .search-icon {
        width: 20px;
        height: 20px;
    }

    .filter-tags {
        display: flex;
        gap: 8px;
        /*margin-top: 12px;*/
    }

    .filter-tag {
        font: 400 16px/140% Inter, sans-serif;
        color: var(--sds-color-text-brand-tertiary);
        background-color:gray;
        border-radius: 20px;
        padding: 8px;
        cursor: pointer;
    }

        .filter-tag.active {
            font-weight: 600;
        }

    @media (max-width: 991px) {
        .search-bar {
            margin-bottom: 40px;
        }

        .filter-tags {
            flex-wrap: wrap;
        }
    }
</style>