<template>
    <div class="header">
        <div class="user-icon">👤</div>
        <input type="text" class="search-bar" placeholder="搜索..." />
    </div>
</template>

<script>
    export default {
        name: "Header",
    };
</script>

<style scoped>
    .header {
        display: flex;
        justify-content: space-between;
        padding: 20px;
        background-color: #6a1b9a;
        color: white;
    }

    .search-bar {
        width: 200px;
        padding: 5px;
    }
</style>
