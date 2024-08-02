<template>
    <div class="app-grid">
        <div v-for="app in apps" :key="app.id" class="app-item" @click="goToDetail(app.id)">
            <img :src="app.image" :alt="app.name" class="app-image" />
            <div class="app-details">
                <h3 class="app-name">{{ app.name }}</h3>
                <p class="app-price">Price: {{ app.price }}</p>
                <p class="app-category">Category: {{ app.category }}</p>
            </div>
        </div>
    </div>
</template>

<script>
    export default {
        name: 'AppGrid',
        props: {
            apps: {
                type: Array,
                required: true,
                default: () => []
            }
        },
        methods: {
            goToDetail(appId) {
                this.$router.push(`/app/${appId}`);
            }
        }
    }
</script>

<style scoped>
    @import url('https://fonts.googleapis.com/css2?family=Poppins:wght@400;600&display=swap');
    @import url('https://fonts.googleapis.com/css2?family=Baloo+2:wght@400;600&display=swap');

    .app-grid {
        display: flex;
        flex-wrap: wrap;
        gap: 20px;
        padding: 20px;
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
        transition: transform 0.3s, box-shadow 0.3s;
    }

        .app-item:hover {
            transform: translateY(-5px);
            box-shadow: 0 8px 16px rgba(0, 0, 0, 0.15);
        }

    .app-image {
        width: 100px;
        height: 100px;
        object-fit: cover;
        border-radius: 8px;
        transition: transform 0.3s;
    }

        .app-image:hover {
            transform: scale(1.05);
        }

    .app-details {
        text-align: center;
    }

    .app-name {
        font-size: 16px;
        font-weight: 600;
        margin-bottom: 5px;
        color: #333;
        font-family: 'Baloo 2', cursive; /* Cute font */
    }

    .app-price {
        font-size: 14px;
        color: #e91e63;
        margin-bottom: 5px;
        font-family: 'Poppins', sans-serif; /* Friendly font */
    }

    .app-category {
        font-size: 14px;
        color: #666;
        font-family: 'Poppins', sans-serif; /* Friendly font */
    }
</style>
