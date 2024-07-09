<template>
    <el-aside width="200px">
        <el-menu default-active="1"
                 class="el-menu-vertical-demo"
                 background-color="#545c64"
                 text-color="#fff"
                 active-text-color="#ffd04b">
            <el-menu-item index="1">导航一</el-menu-item>
            <el-menu-item index="2">导航二</el-menu-item>
            <el-menu-item index="3">导航三</el-menu-item>
        </el-menu>
    </el-aside>
</template>

<script>
    export default {
        name: 'SideBar'
    };
</script>

<style scoped>
    .el-menu-vertical-demo {
        line-height: 60px;
    }
</style>

