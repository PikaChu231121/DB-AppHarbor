<template>
    <transition name="popup">
        <div v-if="show" class="notification-popup">
            <div class="popup-content">
                <p>{{ message }}</p>
            </div>
        </div>
    </transition>
</template>

<script>
    export default {
        props: {
            show: Boolean,
            message: String
        },
        watch: {
            show(newVal) {
                if (newVal) {
                    // Start the timer to automatically hide the popup after 2 seconds
                    this.autoHideTimer = setTimeout(() => {
                        this.close();
                    }, 2000);
                } else {
                    // Clear timer if popup is manually closed
                    clearTimeout(this.autoHideTimer);
                }
            }
        },
        data() {
            return {
                autoHideTimer: null
            }
        },
        methods: {
            close() {
                this.$emit('update:show', false);
            }
        }
    }
</script>

<style scoped>
    @import url('https://fonts.googleapis.com/css2?family=Poppins:wght@600&display=swap'); /* Import Poppins font */

    .notification-popup {
        position: fixed;
        top: 20px;
        right: 100px;
        background-color: rgba(0, 0, 0, 0.8);
        color: #fff;
        border-radius: 8px;
        padding: 20px;
        box-shadow: 0 2px 10px rgba(0, 0, 0, 0.2);
        z-index: 1000;
        width: 300px;
        transition: opacity 0.3s ease;
        display: flex;
        flex-direction: column;
        align-items: center;
    }

    .popup-content {
        display: flex;
        flex-direction: column;
        align-items: center;
    }

        .popup-content p {
            margin: 0;
            font-family: 'Poppins', sans-serif;
            font-weight: 600;
            font-size: 18px;
            color: #fff;
            text-align: center;
        }
</style>
