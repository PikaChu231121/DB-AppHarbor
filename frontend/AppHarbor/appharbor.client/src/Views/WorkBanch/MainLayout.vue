<!--<template>
    <el-container>
         引入Header组件
        <Header />
        <el-container>
             引入SideBar组件
            <SideBar />
            <el-main>
                <div class="content">
                    <h2>主内容区域</h2>
                    <p>这里是主内容。</p>
                </div>
            </el-main>
        </el-container>
    </el-container>
</template>

<script>
    import Header from './Header.vue';
    import SideBar from './SideBar.vue';

    export default {
        name: 'WorkBanch',
        components: {
            Header,
            SideBar
        }
    };
</script>

<style scoped>
    .content {
        padding: 20px;
    }
</style>-->

<template>
    <div class="main-layout">
        <Header />
    </div>
</template>

<script>
    import Header from './Header.vue';

    export default {
        components: {
            Header
        }
    }
</script>

<style scoped>
    .main-layout {
        display: flex;
        justify-content: space-between;
        width: 100%;
        gap: 20px;
    }

    @media (max-width: 991px) {
        .main-layout {
            max-width: 100%;
            flex-wrap: wrap;
        }
    }
</style>
