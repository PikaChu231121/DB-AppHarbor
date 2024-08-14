<template>
    <div class="app-grid">
        <div v-for="app in apps" :key="app.id" class="app-item" @click="goToDetail(app.id)">
            <img :src="getAppImgUrl(app.image)" :alt="app.name" class="app-image" />
            <div class="app-details">
                <h3 class="app-name">{{ app.name }}</h3>
                <p class="app-price">Price: {{ app.price }}</p>
                <p class="app-category">Category: {{ app.category }}</p>
            </div>
        </div>
        <!-- 使用空白的占位符，确保页面上的应用保持两行五列布局 -->
        <div v-for="n in emptySlots" :key="`empty-${n}`" class="app-item empty-slot"></div>
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
            },
            getAppImgUrl(imgPath) {
                if (imgPath) {
                    return `http://localhost:5118${imgPath}`;
                }
                return '../../public/default.png'; // 默认图片路径
            }
        }
    }
</script>

<style scoped>
    @import url('https://fonts.googleapis.com/css2?family=Poppins:wght@400;600&display=swap');
    @import url('https://fonts.googleapis.com/css2?family=Baloo+2:wght@400;600&display=swap');

    .app-grid {
        display: grid;
        grid-template-columns: repeat(5, 1fr);
        grid-template-rows: repeat(2, 1fr);
        gap: 20px;
        padding: 20px;
        justify-content: center;
        align-content: center;
    }

    .app-item {
        display: flex;
        flex-direction: column;
        align-items: center;
        justify-content: space-between;
        background-color: #fff;
        border-radius: 10px;
        padding: 10px;
        box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
        transition: transform 0.3s, box-shadow 0.3s;
        width: 180px;
        height: 220px;
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

    /* 空白占位符样式 */
    .empty-slot {
        visibility: hidden;
    }
</style>
