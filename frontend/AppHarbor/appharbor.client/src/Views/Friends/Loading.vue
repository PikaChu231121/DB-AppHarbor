<template>
    <div v-if="loading" class="loading-overlay">
        <div class="loading-container">
            <video autoplay muted loop class="loading-video">
                <source src="@/assets/loading.mp4" type="video/mp4">
                Your browser does not support the video tag.
            </video>
        </div>
    </div>
</template>

<script>
    export default {
        props: {
            loading: {
                type: Boolean,
                default: false,
            },
        },
    };
</script>

<style scoped>
    .loading-overlay {
        position: fixed;
        top: 0;
        left: 0;
        width: 100%;
        height: 100%;
        background-color: rgba(0, 0, 0, 0.5); /* 蒙版颜色和透明度 */
        display: flex;
        justify-content: center;
        align-items: center;
        z-index: 9999;
    }

    .loading-container {
        width: 120px;
        height: 120px;
        border: 8px solid #FCF4F3;
        border-radius: 50%;
        display: flex;
        justify-content: center;
        align-items: center;
        background-color: white; /* 圆形容器背景色 */
        overflow: hidden;
    }

    .loading-video {
        margin-left:8px;
        margin-top:10px;
        width:  340px;
        height: 340px;
        border-radius: 50%; /* 圆形视频 */
    }
</style>
