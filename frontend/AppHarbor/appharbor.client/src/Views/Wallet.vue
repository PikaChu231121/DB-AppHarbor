<template>
    <div class="root-group-1">
        <div class="header">
            <img src="../assets/logo.svg" class="avatar-header" />
        </div>
        <img src="../assets/logo.svg" class="avatar-circle" />
        <div class="auto-wrapper">
            <div class="info-box">
                <p class="text">钱包余额</p>
                <div class="button-row">
                    <button type="button" class="button" @click="goToLogin">充值</button>
                </div>
            </div>
            <div class="info-box">
                <p class="text">交易记录</p>
                <div class="button-row">
                    <button type="button" class="button">上一页</button>
                    <button type="button" class="button">下一页</button>
                </div>
            </div>
        </div>
    </div>
</template>

<script setup>

</script>

<style lang="scss" scoped>
    .root-group-1 {
        position: relative;
        display: flex;
        flex-direction: column;
        align-items: center;
        width: 100%;
    }

    .header{
        height: 14%;
        width: 100%;
        overflow: hidden; /* 裁剪超出部分 */
        box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1); /* 阴影效果 */
        position: relative;
    }

    .avatar-header {
        width: 100%;
        height: 200px;
        // object-fit:cover /* 拉伸图像 */
        overflow: clip; /* 裁剪超出部分 */
        filter: blur(10px);
    }

    .avatar-circle {
    position: absolute;
    background-color: #ffffff;
    left: 2.4%;
    top: 13%;
    width: 12.4%;
    aspect-ratio: 1 / 1;
    border-radius: 50%;
    box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1); /* 阴影效果 */
    // object-fit: cover;
    z-index: 1; /* 使其置于其他内容之上 */

    }

    .auto-wrapper {
        display: flex;
        flex-direction: row;
        align-items: center;
        justify-content: space-between;
        width: 100%;
        height: auto;
        margin: 2.4% 0 0;
    }

    .info-box{
        display: flex;
        flex-direction: column;
        align-items: center;
        background: #d9d9d9;
        padding: 2% 1% 2% 2%;
        width: 49%;
        height: 500px;
        border-radius: 4px;
    }

    .text{
        font-family: Inika;
        font-size: 2rem;
        color: #000000;
        line-height: normal;
        text-align: left;
        vertical-align: top;
        font-weight: 400;
        white-space: normal;
        margin-bottom: 1rem;
    }

    .button-row {
        width: 100%;
        display: flex;
        justify-content: space-between;
        margin-top: 1em;
    }

    .button {
        width: 40%;
        padding: 0.5em;
        border: none;
        border-radius: 4px;
        background-color: #42b983;
        color: white;
        font-size: 1em;
        cursor: pointer;
    }

    .button:hover
    {
        background-color: #3ca676;
        box-shadow: 0 4px 8px rgba(0, 0, 0, 0.3); /* 阴影效果 */
    }

</style>
