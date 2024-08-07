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
        height: 85vh;
        width: 100%;
        margin: 0;
        padding: 0;
        background-image: url('~@/assets/admin.png');
        background-size: cover;
        background-position: center;
        z-index: 9999;
    }

    header {
        flex-shrink: 0; 
    }

    main {
        flex-grow: 1; 
        overflow: hidden;
    }
</style>
