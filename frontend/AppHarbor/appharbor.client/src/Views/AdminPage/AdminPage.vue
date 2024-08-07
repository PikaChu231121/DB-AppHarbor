<template>
    <img src="@/assets/admin.png">
    <div class="admin-page">
        <Header />
        <MainLayout />
    </div>
</template>

<script>
    import Header from "@/Views/AdminPage/Header.vue";
    import MainLayout from "@/Views/AdminPage/MainLayout.vue";

    export default {
        name: "AdminPage",
        components: {
            Header,
            MainLayout,
        },
    };
</script>

<style scoped>
    .admin-page {
        display: flex;
        flex-direction: column;
        height: 85vh; /* 调整后的高度 */
        width: 100%; /* 视口宽度的全部 */
        margin: 0;
        padding: 0;
        /* 使用相对路径 */
        background-image: url('../../assets/admin.png');
        /* 使用绝对路径 */
        background-image: url('~@/assets/admin.png'); /* Webpack 别名 */
        /* 如果直接引用可以工作，尝试相对于根目录的路径 */
        background-image: url('/src/assets/admin.png');
    }

    header {
        /* 如有需要，调整头部的高度 */
        flex-shrink: 0; /* 防止头部收缩 */
    }

    main {
        flex-grow: 1; /* 允许 MainLayout 填充剩余空间 */
        overflow: hidden; /* 防止溢出问题 */
    }
</style>
