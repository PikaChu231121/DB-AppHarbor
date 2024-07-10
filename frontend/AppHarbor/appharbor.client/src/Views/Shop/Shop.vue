<template>
    <div class="app-search">
        <!--<aside class="filter-section">
            <FilterSection />
        </aside>-->
        <main class="search-section">
            <SearchBar />
            <!--<AppGrid />
            <Pagination />-->
        </main>
    </div>
</template>

<script>
    //import FilterSection from './FilterSection.vue';
    //import SearchBar from './SearchBar.vue';
    //import AppGrid from './AppGrid.vue';
    //import Pagination from './Pagination.vue';

    export default {
        name: 'Shop',
        components: {
/*            FilterSection,*/
            SearchBar,
/*            ProductGrid*/
        }   
    }
</script>

<style scoped>
    .app-search {
        border-radius: 20px;
        background-color: var(--sds-color-background-default-default);
        display: flex;
        justify-content: center;
        padding: 32px;
    }

/*    .filter-section {
        width: 27%;
        margin-right: 20px;
    }*/

    .search-section {
        width: 73%;
    }

    @media (max-width: 991px) {
        .app-search {
            flex-direction: column;
            padding: 20px;
        }

/*        .filter-section {
            margin-bottom: 40px;
        }*/

        .search-section {
            width: 100%;
            margin-right: 0;
        }

    }
</style>