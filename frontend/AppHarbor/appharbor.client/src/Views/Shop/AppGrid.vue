<template>
    <div class="app-grid">
        <div v-for="app in apps" :key="app.id" class="app-item">
            <img :src="app.image" :alt="app.name" class="app-image" />
            <div class="app-details">
                <h3 class="app-name">{{ app.name }}</h3>
                <p class="app-price">{{ app.price }}</p>
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

    .app-item image {
        width: 100px;
        height: 100px;
        object-fit: cover;
        border-radius: 10px;
    }

    .app-details {
        flex: 1; /* »√œÍ«È≤ø∑÷ÃÓ≥‰ £”‡ø’º‰ */
    }

    .app-name {
        font-size: 16px;
        font-weight: bold;
        margin-bottom: 5px;
    }

    .app-price {
        font-size: 14px;
        color: #666;
        margin-bottom: 5px;
    }

    .app-category {
        font-size: 14px;
        color: #888;
    }
</style>
