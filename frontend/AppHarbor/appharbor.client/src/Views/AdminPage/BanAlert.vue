<template>
    <div v-if="visible" class="alert-box">
        {{ message }}
    </div>
</template>

<script>
    export default {
        props: ['message'],
        data() {
            return {
                visible: false
            };
        },
        watch: {
            message(newMessage) {
                if (newMessage) {
                    this.visible = true;
                    setTimeout(() => {
                        this.visible = false;
                    }, 3000); // 消息显示3秒
                }
            }
        }
    };
</script>

<style scoped>
    .alert-box {
        position: fixed;
        bottom: 10px;
        left: 50%;
        transform: translateX(-50%);
        background: #f8d7da;
        color: #721c24;
        padding: 10px 20px;
        border: 1px solid #f5c6cb;
        border-radius: 5px;
        z-index:1;
    }
</style>

