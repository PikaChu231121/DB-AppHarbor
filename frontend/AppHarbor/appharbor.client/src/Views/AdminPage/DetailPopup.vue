<template>
    <div v-if="show" class="detail-popup">
        <div class="popup-content">
            <button class="close-button" @click="$emit('close')">×</button>
            <h3 class="popup-title">{{ announcement.title }}</h3>
            <p class="popup-content">{{ announcement.content }}</p>
            <p class="popup-details">发布公告管理员: <span class="detail-value">{{ announcement.adminNickname }}</span></p>
            <p class="popup-details">发布时间: <span class="detail-value">{{ announcement.publishTime }}</span></p>
        </div>
    </div>
</template>

<script>
export default {
    props: {
        show: {
            type: Boolean,
            required: true
        },
        announcement: {
            type: Object,
            required: true
        }
    }
};
</script>

<style scoped>
    .detail-popup {
        position: fixed;
        top: 0;
        left: 0;
        right: 0;
        bottom: 0;
        background: rgba(0, 0, 0, 0.5);
        display: flex;
        align-items: center;
        justify-content: center;
    }

    .popup-content {
        background: white;
        padding: 20px;
        border-radius: 8px;
        box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
        text-align: center;
        max-width: 600px;
        width: 100%;
        position: relative;
    }

    .close-button {
        position: absolute;
        top: 10px;
        right: 10px;
        background: transparent;
        border: none;
        font-size: 1.5rem;
        cursor: pointer;
        color: #555;
    }

    .popup-title {
        margin: 0 0 10px;
        font-size: 1.5rem;
        color: #333;
    }

    .popup-content {
        font-size: 1rem;
        color: #333;
        line-height: 1.5;
    }

    .popup-details {
        font-size: 0.875rem;
        color: #555;
    }

    .detail-value {
        font-weight: bold;
        color: #000;
    }
</style>
