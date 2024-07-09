<script setup>

</script>

<template>
    <div id="app">
        <UserLogin />
    </div>
</template>

<script>
    import UserLogin from './Views/UserLogin.vue';

    export default {
        name: 'App',
        components: {
            UserLogin
        }
    };
</script>

<style>
    /* 全局样式可以在这里定义 */
</style>
