<!--主界面父组件，包含Header、SideNavigation等子组件-->
<template>
    <div class="main-layout">
        <Header />
        <div class="layout-body">
            <SideNavigation />
            <div class="content">
                <!-- 页面内容 -->
            </div>
        </div>
    </div>
</template>

<script>
    import Header from './Header.vue'; // 标题栏
    import SideNavigation from './SideNavigation.vue'; // 侧边栏

    export default {
        components: {
            Header,
            SideNavigation,
        },
    };
</script>

<style scoped>
    .main-layout {
        display: flex;
        flex-direction: column;
        width: 100%;
    }

    .layout-body {
        display: flex;
        justify-content: space-between;
        width: 100%;
        gap: 20px;
        margin-top: 80px; /* 给Header留出空间 */
    }

    .content {
        flex: 1;
        padding: 20px;
    }

    @media (max-width: 991px) {
        .layout-body {
            max-width: 100%;
            flex-wrap: wrap;
        }
    }
</style>

