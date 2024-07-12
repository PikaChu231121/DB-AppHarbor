<template>
    <div class="background">
        <div class="order-container">

            <!--钱包剩余显示-->
            <div class="money-left">
                <img loading="lazy"
                     src="https://cdn.builder.io/api/v1/image/assets/TEMP/ebfe3a97bf3fbf1744b64e98bf537a292b984f51f5a76525c09758e46a083f17?apiKey=b4c87aa6fd1245589700a3931ad0dfbf&"
                     class="wallet-img" />
                <div class="money">
                    <div class="text-money">￥{{ user.credit }}</div>
                    <div class="text-left">Left</div>
                </div>
            </div>

            <!--应用详情显示-->
            <div class="app-container">
                <img loading="lazy" :src="app.image" :alt="app.name" class="app-image" />
                <div class="app-details">
                    <h2 class="app-name">{{ app.name }}</h2>
                    <div class="app-price">￥{{ app.price }}</div>
                    <div class="app-description">{{ app.description }}</div>
                </div>
            </div>

            <div class="right-container">
                <!--价格列表-->
                <div class="pricelist-container">
                    <div class="text-pricelist">Price List</div>
                    <div class="price-list-wrapper">
                        <div v-for="item in pricelist" :key="item.id" class="price-item">
                            <p class="price-text">List item</p>
                        </div>
                    </div>
                </div>
                <!--购买金额和购买按钮显示-->
                <div class="purchase-container">
                    <div class="text-total">Total</div>
                    <div class="purchase-price">￥{{ app.price }}</div>
                    <button class="purchase-button" @click="handlePurchase">BUY</button>
                </div>
            </div>

            <!--为谁购买显示-->
        </div>
    </div>
</template>

<script>
    export default {
        data() {
            return {
                user: {
                    credit: 100,
                },
                app : {
                    /*id: appId,*/
                    name: 'Keep',
                    image: '@/assets/A.png',
                    price: '70.00',
                    /*category: '健身',*/
                    description: '「Keep」是一款健身App，超过2亿运动爱好者的选择！无论是想减肥塑形或增肌，还是寻找健身跑步瑜伽计步等训练计划，你可以随时随地选择课程进行训练！'
                },
                pricelist: [
                    { id: 1 },
                    { id: 2 },
                    { id: 3 },
                    { id: 4 },
                ]
            };
        },
        methods: {
            handlePurchase() {
                console.log('App has been puechased!');
            }
        }
    };
</script>

<style scoped>
    .background {
        position: fixed;
        top: 0;
        left: 0;
        height: 100%;
        width: 100%;
        background-color: #efc2bb;
        background-size: cover;
        background-position: center;
        background-repeat: no-repeat;
    }
    .order-container {
        background-color: white;
        border-radius: 20px;
        width: 95%;
        height: 90%;
        margin-left: auto;
        margin-right: auto;
        margin-top: 30px;
        margin-bottom: auto; /*居中*/

        display: flex;
    }

    .money-left {
        /*border-radius: var(--sds-size-radius-200);*/
        border-radius: 5px ;
        box-shadow: 0px 4px 4px 0px rgba(0, 0, 0, 0.25);
        border-color: rgba(217, 217, 217, 1);
        border-style: solid;
        border-width: 1px;
        background-color: var(--sds-color-background-default-default);
        /*display: flex;*/
        max-width: 240px;
        /*flex-direction: column;*/
        color: var(--sds-color-text-default-default);
        white-space: nowrap;
        text-align: center;
        padding: 24px;
        padding-left: 40px;
        padding-right: 40px;

        margin-left:auto;
        margin-right:auto;
        margin-top:auto;
        margin-bottom:40px;
    }

    .wallet-img {
        aspect-ratio: 1;
        object-fit: contain;
        object-position: center;
        width: 40px;
        align-self: center;
    }

    .money {
        border-radius: var(--sds-size-radius-200);
        display: flex;
        margin-top: 24px;
        flex-direction: column;
    }

    .text-money {
        justify-content: center;
        letter-spacing: -0.48px;
        padding: 0 50px;
        font: 600 24px/120% Inter, sans-serif;
    }

    .text-left {
        justify-content: center;
        align-items: center;
        margin-top: 4px;
        padding: 0 60px;
        font: 400 16px/140% Inter, sans-serif;
    }

    .app-container {
        border-radius: 5px;
        box-shadow: 0px 4px 4px 0px rgba(0, 0, 0, 0.25);
        border-color: rgba(217, 217, 217, 1);
        border-style: solid;
        border-width: 1px;
        background-color: var(--sds-color-background-default-default);
        display: flex;
        max-width: 331px;
        flex-direction: column;
        font-size: 16px;
        color: var(--sds-color-text-default-default);
        font-weight: 400;
        line-height: 140%;
        padding: 16px;

        margin-left:auto;
        margin-right:auto;
        margin-top:auto;
        margin-bottom:auto;
    }

    .app-img {
        aspect-ratio: 1.2;
        object-fit:contain;
        object-position: center;
        width: 100%;
    }

    .app-details {
        display: flex;
        margin-top: 16px;
        flex-direction: column;
    }

    .app-name {
        font-family: Inter, sans-serif;
        white-space: nowrap;
        justify-content: center;
    }

    .app-price {
        font-family: Inter, sans-serif;
        margin-top: 8px;
        font-weight: 600;
        white-space: nowrap;
        justify-content: center;
    }

    .app-description {
        margin-top: 8px;
        color: var(--sds-color-text-default-secondary);
        justify-content: center;
        font: 14px Inter, sans-serif;
    }

    .right-container {
        height: 80%;
        margin-left: auto;
        margin-right: auto;
        /*justify-content: space-around;*/
        margin-top: 40px;
        margin-bottom: 40px;
        /*display:flex;*/
        display:grid;
    }

    .purchase-container {
        border-radius: 5px;
        box-shadow: 0px 4px 4px 0px rgba(0, 0, 0, 0.25);
        border-color: rgba(217, 217, 217, 1);
        border-style: solid;
        border-width: 1px;
        background-color: var(--sds-color-background-default-default);
        display: flex;
        max-width: 240px;
        flex-direction: column;
        white-space: nowrap;
        padding: 24px;
        padding-left: 80px;
        padding-right: 80px;

        margin-right: auto;
        margin-left: auto;
        margin-top:auto;
        margin-bottom:10px;
    }

    .text-total {
        margin-left: auto;
        margin-right: auto;
        font-family: Inter, sans-serif;
        justify-content: center;

        font-size: 20px;
        font-weight: 600;
    }

    .purchase-price {
        text-align: center;
        font-family: Inter, sans-serif;

        font-size: 18px;
        font-weight: 700;
    }

    .purchase-button {
        justify-content: center;
        align-items: center;
        border-radius: var(--sds-size-radius-200);
        box-shadow: 0px 4px 4px 0px rgba(0, 0, 0, 0.25);
        border-color: rgba(235, 235, 235, 1);
        border-style: solid;
        border-width: 1px;
        background-color: #ffb5b5;
        margin-top: 24px;
        color: #fff;
        padding: 12px;
        font: 600 20px/100% Inter, sans-serif;
        cursor: pointer;
    }

    .pricelist-container {
        /* 设置容器大小 */
        width: 100%; /* 或者具体宽度 */
        height: 200px; /* 容器高度，自定义适合的高度 */
        border: 1px solid #ccc; /* 边框样式，可选 */
        overflow: hidden; /* 隐藏溢出内容 */
        /*margin-right:40px;*/
        margin-right: auto;
        margin-left: auto;
    }

    .price-list-wrapper {
        /* 设置列表包装器 */
        height: 100%; /* 包装器高度与容器高度一致 */
        overflow-y: auto; /* 垂直滚动条 */
    }

    .price-item {
        /* 价格项目样式 */
        padding: 10px; /* 可选的内边距 */
        border-bottom: 1px solid #ddd; /* 可选的底部边框 */
    }

</style>
