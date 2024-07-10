<template>
    <div class="app-grid">
        <!--每个应用对应的组件，包含应用信息-->
        <div class="app-item" v-for="app in apps" :key="app.id">
            <!--应用图片、名称、价格等信息-->
            <img :src="app.image" alt="App Image" />
            <h3>{{ app.name }}</h3> 
            <p>{{ app.price }}</p>
        </div>
    </div>
</template>

<script>
    export default {
        name: 'AppGrid',
        props: {
            apps: {
                type: Array,
                required: true
            }
        }
    }
</script>

<style scoped>
    .app-grid {
        display: flex;
        flex-wrap: wrap;
        gap: 20px;
    }

    .app-item {
        flex: 1 0 calc(20% - 20px);
        display: flex;
        flex-direction: column;
        align-items: center;
        background-color: #fff;
        border-radius: 10px;
        padding: 10px;
        box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
    }

    .app-item img {
        width: 100px;
        height: 100px;
        object-fit: cover;
        border-radius: 10px;
    }

    .app-item h3 {
        margin: 10px 0 5px;
        font-size: 1.2em;
    }

    .app-item p {
        color: #888;
    }
</style>
