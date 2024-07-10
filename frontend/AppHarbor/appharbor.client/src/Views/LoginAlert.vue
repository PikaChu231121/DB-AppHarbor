<template>
    <div v-if="visible" class="login-alert">
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
                    }, 2000); // œ˚œ¢œ‘ æ2√Î
                }
            }
        }
    };
</script>

<style>
    .login-alert {
        position: fixed;
        bottom: 10px;
        left: 50%;
        transform: translateX(-50%);
        background: #f8d7da;
        color: #721c24;
        padding: 10px 20px;
        border: 1px solid #f5c6cb;
        border-radius: 5px;
    }
</style>