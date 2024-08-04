<template>
    <div class="header">
        <div class="user-icon">👤</div>
        <div class="search-container">
            <input type="text" class="search-bar" placeholder="搜索..." />
            <div class="search-icon">🔍</div>
        </div>
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
        align-items: center; /* 垂直对齐 */
        padding: 15px; /* 调整内边距 */
        background-color: #6a1b9a;
        color: white;
        flex-shrink: 0; /* 防止头部收缩 */
    }

    .search-container {
        display: flex;
        align-items: center; /* 垂直对齐 */
        position: relative; /* 相对定位以便图标绝对定位 */
    }

    .search-bar {
        width: 300px; /* 增加宽度 */
        padding: 10px;
        padding-left: 40px; /* 为图标留出空间 */
        border: none;
        border-radius: 4px;
        box-shadow: 0 0 4px rgba(0, 0, 0, 0.1);
        font-size: 16px;
        background-color: #fff;
    }

    .search-icon {
        position: absolute;
        left: 10px; /* 调整位置 */
        font-size: 18px; /* 调整图标大小 */
        color: #6a1b9a;
    }
</style>
