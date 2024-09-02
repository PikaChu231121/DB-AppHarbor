<template>
    <div class="search-bar">
        <form class="search-form" @submit.prevent="handleSearch">
            <label for="searchInput" class="visually-hidden">搜索</label>
            <input id="searchInput" type="text" class="search-input" placeholder="搜索..." v-model="searchQuery" />
            <button type="submit" class="search-button">
                <img src="https://cdn.builder.io/api/v1/image/assets/TEMP/ceee35d297d4c0fe5453107e73e7fbb494e6164c747eb1afdb410a1be3052fd4?apiKey=b4c87aa6fd1245589700a3931ad0dfbf&"
                    alt="Search" class="search-icon" />
            </button>
        </form>
        <div class="filter-tags">
            <span v-for="(filter, index) in filters" :key="index"
                :class="['filter-tag', { active: activeFilter === filter }]" @click="setActiveFilter(filter)">
                {{ filter }}
            </span>
        </div>
    </div>
</template>

<script>
export default {
    name: 'SearchBar',
    data() {
        return {
            searchQuery: '',
            filters: ['按价格排序', '按热度排序'],
            activeFilter: '按热度排序' // Ĭ�ϼ���Ĺ�����
        }
    },
    methods: {
        handleSearch() {
            // Implement search functionality
            console.log('Searching for:', this.searchQuery);
            this.$emit('search', this.searchQuery);
        },
        setActiveFilter(filter) {
            this.activeFilter = filter;
            console.log('Filter set to:', filter);
            // ������Ը���ѡ��Ĺ�����������Ӧ���������˹���
            this.$emit('sort-method-changed', filter);
        }
    }
}
</script>

<style scoped>
@import url('https://fonts.googleapis.com/css2?family=Poppins:wght@400;600&display=swap');

    .search-bar {
        display: flex;
        align-items: center;
        justify-content: flex-start;
        /* Align items closer together */
        margin-top: 10px;
        height: auto;
        /* Allow height to adjust based on content */
        width: 100%;
        border-radius: 25px;
        padding: 10px;
        gap: 10px;
        /* Add this to control the space between the search bar and filter tags */
        transition: transform 0.3s;
    }
        .search-bar:hover {
            transform: scale(1.01);
        }

    .search-form {
        display: flex;
        align-items: center;
        border: 0.5px solid #e4e4e4;
        border-radius: 25px;
        padding: 8px 12px;
        background-color: #ffffff;
        width: 80%;
        box-shadow: 2px 3px 3px rgba(0, 0, 0, 0.05);
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
        font-weight: bold !important; /* 设置粗体 */
    }

.search-input {
    flex: 1;
    border: none;
    color: #333;
    background: transparent;
    padding: 8px;
    border-radius: 20px;
    transition: background-color 0.3s ease;
}

.search-input:focus {
    outline: none;
    //background-color: #f0f0f5;
}

.search-button {
    background: none;
    border: none;
    cursor: pointer;
    padding: 0;
}

.search-icon {
    width: 24px;
    height: 24px;
    transition: transform 0.3s ease;
}

.search-button:hover .search-icon {
    transform: scale(1.1);
}

.filter-tags {
    display: flex;
    gap: 12px;
    align-items: center;
}

    .filter-tag {
        font-weight: bold !important; /* 设置粗体 */
        color: #555;
        background-color: #e0e0e0;
        border-radius: 20px;
        padding: 6px 12px;
        cursor: pointer;
        transition: background-color 0.3s ease, color 0.3s ease;
    }

.filter-tag.active {
    background-color: #007bff;
    color: #fff;
    font-weight: 600;
}

.filter-tag:hover {
    background-color: #dcdcdc;
}

@media (max-width: 991px) {
    .search-bar {
        flex-direction: column;
        align-items: flex-start;
    }

    .search-form {
        width: 100%;
    }

    .filter-tags {
        flex-wrap: wrap;
        margin-top: 10px;
    }
}
</style>
