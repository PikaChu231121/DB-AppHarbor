<!-- NotificationModal.vue -->
<template>
    <transition name="fade">
        <div v-if="visible" class="notification-modal">
            <div class="modal-content">
                <h3 class="modal-title">{{ title }}</h3>
                <p class="modal-message">{{ message }}</p>
                <button class="close-button" @click="close">Close</button>
            </div>
        </div>
    </transition>
</template>

<script>
    export default {
        props: {
            visible: {
                type: Boolean,
                required: true
            },
            title: {
                type: String,
                required: true
            },
            message: {
                type: String,
                required: true
            }
        },
        methods: {
            close() {
                this.$emit('close');
            }
        }
    };
</script>

<style scoped>
    /* Transition for fade effect */
    .fade-enter-active, .fade-leave-active {
        transition: opacity 0.3s;
    }

    .fade-enter, .fade-leave-to /* .fade-leave-active in <2.1.8 */ {
        opacity: 0;
    }

    .notification-modal {
        position: fixed;
        top: 0;
        left: 0;
        width: 100%;
        height: 100%;
        background: rgba(0, 0, 0, 0.4);
        display: flex;
        justify-content: center;
        align-items: center;
        z-index: 9999;
    }

    .modal-content {
        background: #fff;
        padding: 20px;
        border-radius: 12px;
        box-shadow: 0 6px 12px rgba(0, 0, 0, 0.2);
        max-width: 450px;
        width: 90%;
        text-align: center;
    }

    .modal-title {
        font-size: 24px;
        font-weight: 600;
        color: #333;
        margin-bottom: 10px;
    }

    .modal-message {
        font-size: 16px;
        color: #555;
        margin-bottom: 20px;
    }

    .close-button {
        font-family:'Times New Roman';
        background: #fbb1a2;
        border: none;
        color: white;
        padding: 10px 20px;
        border-radius: 8px;
        cursor: pointer;
        font-size: 16px;
        font-weight: 500;
        transition: background 0.3s, transform 0.2s;
    }

        .close-button:hover {
            background: #eaa397;
            transform: scale(1.05);
        }
</style>
