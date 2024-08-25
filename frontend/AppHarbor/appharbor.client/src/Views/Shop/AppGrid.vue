<template>
    <div class="app-grid">
        <div v-for="app in apps" :key="app.id" class="app-item" @click="goToDetail(app.id)">
            <img :src="getAppImgUrl(app.image)" :alt="app.name" class="app-image" />
            <div class="app-details">
                <h3 class="app-name">{{ app.name }}</h3>
                <p class="app-price" v-html="formattedPrice(app.price,app.discount)"></p>
                <p class="app-category"> {{ app.category }}</p>
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
            },
            formattedPrice(price, discount) {
                console.log("Received price:", price);
                console.log("Received discount:", discount);
                if (price === 0) {
                    return `<span>免费!</span>`;
                }
                console.log(price, discount);
                const originalPrice = price.toFixed(2).split('.');
                const originalIntegerPart = originalPrice[0];
                const originalDecimalPart = originalPrice[1];

                const effectiveDiscount = discount || 1; // 处理可能为 null 的情况
                const discountedPrice = (price * effectiveDiscount).toFixed(2).split('.');
                const discountedIntegerPart = discountedPrice[0];
                const discountedDecimalPart = discountedPrice[1];

                let result = `
                    <span>
                        ￥<span class="integer-part">${discountedIntegerPart}</span>.<span class="decimal-part">${discountedDecimalPart}</span>
                    </span>
                `;

                if (effectiveDiscount < 1.0) {
                    result += `
                        <span style="text-decoration: line-through; font-size: 0.6em; color:gray;">
                            原价：<span class="integer-part">${originalIntegerPart}</span>.<span class="decimal-part">${originalDecimalPart}</span>
                        </span>
                    `;
                }

                return result;
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
        padding: 10px;
        justify-content: center;
        align-content: center;
        border: 2px solid whitesmoke;
        background-color:whitesmoke;
        border-radius: 10px;
    }

    .app-item {
        display: flex;
        flex-direction: column;
        align-items: center;
        justify-content: space-between;
        background-color: #fff;
        border-radius: 10px;
        padding: 10px;
        border: 1px solid #e2e2e2;
        transition: background-color 0.3s ease;
        width: 180px;
        height: 220px;
        box-shadow: 0 2px 2px rgba(0, 0, 0, 0.03);
    }

        .app-item:hover {
            background-color:whitesmoke;
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
        font-size: 20px;
        font-weight: 600;
        margin-bottom: 5px;
        color: #333;
        //font-family: 'Baloo 2', cursive; /* Cute font */
        
    }

    .app-price {
        font-size: 18px;
        color: #e91e63;
        margin-bottom: 5px;
        //font-family: 'Poppins', sans-serif; /* Friendly font */
    }

    .app-category {
        font-size: 16px;
        width: 70px;
        color: lightseagreen;
        position: center;
        //font-family: 'Pacifico', cursive; /* 后备字体 */
        background-color :whitesmoke;
        border-radius: 10px;
    }

    /* 空白占位符样式 */
    .empty-slot {
        visibility: hidden;
    }
</style>
