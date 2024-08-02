<template>
    <div class="search-bar">
        <form class="search-form" @submit.prevent="handleSearch">
            <label for="searchInput" class="visually-hidden">Search</label>
            <input id="searchInput"
                   type="text"
                   class="search-input"
                   placeholder="Search"
                   v-model="searchQuery" />
            <button type="submit" class="search-button">
                <img src="https://cdn.builder.io/api/v1/image/assets/TEMP/ceee35d297d4c0fe5453107e73e7fbb494e6164c747eb1afdb410a1be3052fd4?apiKey=b4c87aa6fd1245589700a3931ad0dfbf&" alt="Search" class="search-icon" />
            </button>
        </form>
        <div class="filter-tags">
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
    @import url('https://fonts.googleapis.com/css2?family=Poppins:wght@400;600&display=swap');

    .search-bar {
        display: flex;
        align-items: center;
        justify-content: space-between; /* Adjusted for better spacing */
        margin-top: 10px;
        height: 50px;
        width: 100%;
        border-radius: 25px; /* Rounded corners */
        background-color: #f4f4f9; /* Light, soft background color */
        padding: 10px 20px;
        box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
    }

    .search-form {
        display: flex;
        align-items: center;
        border: 1px solid #dcdcdc;
        border-radius: 25px;
        padding: 8px 12px;
        background-color: #ffffff;
        width: 70%;
        box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
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
        font: 400 16px/1.5 'Poppins', sans-serif;
        color: #333;
        background: transparent;
        padding: 8px;
        border-radius: 20px;
        transition: background-color 0.3s ease;
    }

        .search-input:focus {
            outline: none;
            background-color: #f0f0f5;
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
        font: 400 14px/1.5 'Poppins', sans-serif;
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
