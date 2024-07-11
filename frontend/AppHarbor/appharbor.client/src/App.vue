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
    /* 全局样式可以在这里定义 */
</style>
