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
            return this.totalPages > this.visiblePageCount; // ≈–∂œ «∑Òœ‘ æ °¬‘∫≈
        }
    },
    data() {
        return {
            //currentPage: 1,
            //totalPages: 10,
            visiblePageCount: 3
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
        },
    }
}
</script>

<style scoped>
    .pagination-container {
        border-radius: 30px;
        background-color: #fff;
        display: flex;
        max-width: 517px;
        font-size: 16px;
        color: var(--sds-color-text-default-default);
        font-weight: 400;
        line-height: 100%;
        justify-content: center;
        padding: 0 10px;

        margin-left: auto; 
        margin-right: auto; /*æ”÷–∂‘∆Î*/
        margin-top: auto;
        background-color: aqua; /*≤‚ ‘—’…´*/
    }

    .pagination-controls {
        display: flex;
        align-items: center;
        gap: 8px;
    }

    .pagination-button {
        display: flex;
        align-items: center;
        justify-content: center;
        gap: 8px;
        border-radius: var(--sds-size-radius-200);
        color: var(--sds-color-text-default-secondary);
        padding: 8px 12px;
        background: none;
        border: none;
        cursor: pointer;
        font-family: Inter, sans-serif;
    }

        .pagination-button:disabled {
            opacity: 0.5;
            cursor: not-allowed;
        }

    .pagination-icon {
        width: 16px;
        height: 16px;
    }

    .pagination-list {
        display: flex;
        list-style-type: none;
        padding: 0;
        margin: 0;
        gap: 8px;
    }

    .pagination-number {
        font-family: Inter, sans-serif;
        justify-content: center;
        align-items: center;
        border-radius: var(--sds-size-radius-200);
        background: none;
        border: none;
        cursor: pointer;
        width: 32px;
        height: 32px;
        padding: 8px 12px;
    }

        .pagination-number.active {
            background-color: var(--sds-color-background-brand-default);
            color: var(--sds-color-text-brand-on-brand);
        }

    .pagination-ellipsis {
        font-family: Inter, sans-serif;
        justify-content: center;
        border-radius: 8px;
        color: var(--black-100, #000);
        font-weight: 700;
        line-height: 140%;
        padding: 8px 16px;
    }

    @media (max-width: 991px) {
        .pagination-container {
            white-space: initial;
        }

        .pagination-controls {
            flex-wrap: wrap;
        }

        .pagination-button,
        .pagination-number,
        .pagination-ellipsis {
            white-space: initial;
        }
    }
</style>