<template>
    <div id="app">
        <Header />
        <MainLayout />
    </div>
</template>

<script>
import Header from "@/Views/AdminPage/Header.vue";
import MainLayout from "@/Views/AdminPage/MainLayout.vue";

export default {
  name: "App",
  components: {
    Header,
    MainLayout,
  },
};
</script>

<style>
    #app {
        display: flex;
        flex-direction: column;
        height: 100vh;
    }

    .content {
        display: flex;
        flex-grow: 1;
    }
</style>
