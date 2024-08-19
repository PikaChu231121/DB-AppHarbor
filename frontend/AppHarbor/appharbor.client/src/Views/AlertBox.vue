<template>
    <div :class="alertClass">
            {{ msg }}
    </div>
</template>

<script>
    export default {
        props: ['msg'],
        data() {
            return {
                visible: false
            };
        },
        watch: {
            msg(newMessage) {
                if (newMessage) {
                    this.visible = true;
                    setTimeout(() => {
                        this.visible = false;
                    }, 2000);
                }
            }
        },
        computed: {
            alertClass() {
                return {
                    'alert-box': true,
                    'visible': this.visible
                };
            }
        }
    };
</script>

<style>
    .alert-box {
        position: fixed;
        bottom: 20px;
        left: 50%;
        transform: translateX(-50%);
        background: #e32636;
        color: #ffffff;
        padding: 10px 20px;
        border: 1px solid #f5c6cb;
        border-radius: 5px;
        z-index: 9999;
        opacity: 0;
        transition: opacity 0.5s ease-in-out; /* 淡入淡出效果 */
    }
        .alert-box.visible {
            opacity: 1;
        }
</style>

