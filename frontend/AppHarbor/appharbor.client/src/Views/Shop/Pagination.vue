<template>
    <nav class="pagination-container" aria-label="Pagination">
        <div class="pagination-controls">
            <!-- Previous Page Button -->
            <button class="pagination-button previous" @click="goToPreviousPage" :disabled="isFirstPage">
                <img src="https://cdn.builder.io/api/v1/image/assets/TEMP/eb00729ac35dd30cdcce73386ced7ed9edb9836a5c738bba749960c794ad5761?apiKey=b4c87aa6fd1245589700a3931ad0dfbf&" alt="" class="pagination-icon" />
                <span>Previous</span>
            </button>
            <!-- Page Numbers -->
            <ul class="pagination-list">
                <li v-for="page in visiblePages" :key="page">
                    <button class="pagination-number"
                            :class="{ 'active': page === currentPage }"
                            @click="goToPage(page)">
                        {{ page }}
                    </button>
                </li>
                <!-- Ellipsis to indicate more pages -->
                <li v-if="showEllipsis">
                    <span class="pagination-ellipsis">...</span>
                </li>
            </ul>
            <!-- Next Page Button -->
            <button class="pagination-button next" @click="goToNextPage" :disabled="isLastPage">
                <span>Next</span>
                <img src="https://cdn.builder.io/api/v1/image/assets/TEMP/b85c02f0b5539596af9cb766334ec851876c21e920ea6e52714e82da9823a8fb?apiKey=b4c87aa6fd1245589700a3931ad0dfbf&" alt="" class="pagination-icon" />
            </button>
        </div>
    </nav>
</template>

<script>
    export default {
        name: 'PaginationComponent',
        props: {
            currentPage: {
                type: Number,
                required: true
            },
            totalPages: {
                type: Number,
                required: true
            }
        },
        computed: {
            isFirstPage() {
                return this.currentPage === 1;
            },
            isLastPage() {
                return this.currentPage === this.totalPages;
            },
            visiblePages() {
                let pages = [];
                let start = Math.max(1, this.currentPage - Math.floor(this.visiblePageCount / 2));
                let end = Math.min(this.totalPages, start + this.visiblePageCount - 1);

                if (end - start + 1 < this.visiblePageCount) {
                    start = Math.max(1, end - this.visiblePageCount + 1);
                }

                for (let i = start; i <= end; i++) {
                    pages.push(i);
                }

                return pages;
            },
            showEllipsis() {
                return this.totalPages > this.visiblePageCount;
            }
        },
        data() {
            return {
                visiblePageCount: 5
            }
        },
        methods: {
            goToPreviousPage() {
                if (!this.isFirstPage) {
                    this.$emit('page-changed', this.currentPage - 1);
                }
            },
            goToNextPage() {
                if (!this.isLastPage) {
                    this.$emit('page-changed', this.currentPage + 1);
                }
            },
            goToPage(page) {
                this.$emit('page-changed', page);
            }
        }
    }
</script>

<style scoped>
    @import url('https://fonts.googleapis.com/css2?family=Poppins:wght@400;600&display=swap');

    .pagination-container {
        border-radius: 15px;
        background-color: #ffffff; /* White background */
        box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1); /* Soft shadow */
        display: flex;
        max-width: 600px;
        font-size: 14px;
        color: #333;
        justify-content: center;
        padding: 10px;
        margin-left: 100px; /* Center horizontally */
        margin-top: 0px; /* Space above */
        font-family: 'Poppins', sans-serif; /* Cute font */
    }

    .pagination-controls {
        display: flex;
        align-items: center;
        gap: 10px;
    }

    .pagination-button {
        display: flex;
        align-items: center;
        justify-content: center;
        gap: 6px;
        border-radius: 50px; /* Rounded buttons */
        color: #555;
        padding: 8px 12px;
        background-color: #f4f4f4; /* Light gray background */
        border: 1px solid #ddd;
        cursor: pointer;
        transition: background-color 0.3s ease, color 0.3s ease;
    }

        .pagination-button:disabled {
            opacity: 0.5;
            cursor: not-allowed;
        }

        .pagination-button:hover:not(:disabled) {
            background-color: #e0e0e0; /* Slightly darker gray on hover */
        }

    .pagination-icon {
        width: 18px;
        height: 18px;
    }

    .pagination-list {
        display: flex;
        list-style-type: none;
        padding: 0;
        margin: 0;
        gap: 8px;
    }

    .pagination-number {
        border-radius: 50%;
        background: none;
        border: 1px solid #ddd;
        cursor: pointer;
        width: 32px;
        height: 32px;
        display: flex;
        align-items: center;
        justify-content: center;
        font-size: 14px;
        transition: background-color 0.3s ease, color 0.3s ease;
    }

        .pagination-number.active {
            background-color: #fbb1a2; /* Highlight color */
            color: #fff;
            border-color: #fbb1a2;
        }

        .pagination-number:hover:not(.active) {
            background-color: #f4f4f4; /* Light gray on hover */
        }

    .pagination-ellipsis {
        font-size: 18px;
        color: #777;
        font-weight: bold;
        padding: 0 8px;
    }

    @media (max-width: 991px) {
        .pagination-container {
            flex-direction: column;
        }

        .pagination-controls {
            flex-wrap: wrap;
        }
    }
</style>
