<script setup>

</script>

<template>
    <router-view v-slot="{ Component }" class="light">
        <keep-alive>
            <component :is="Component" />
        </keep-alive>
    </router-view>
    <!--<router-view/>-->
</template>

<script>

</script>

<style>
    body {
        font-family: 'PingFang SC', 'Microsoft YaHei', 'Source Han Sans', 'Noto Sans CJK SC', 'Hiragino Sans GB', sans-serif !important;
        font-weight: bold ; 
    }
    
</style>
