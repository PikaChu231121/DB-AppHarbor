<template>
    <div :class="confirmClass">
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
                    }, 1500);
                }
            }
        },
        computed: {
            confirmClass() {
                return {
                    'confirm-box': true,
                    'visible': this.visible
                };
            }
        }
    };
</script>

<style>
    .confirm-box {
        position: fixed;
        bottom: 20px;
        left: 50%;
        transform: translateX(-50%);
        background: #40bb45;
        color: #ffffff;
        padding: 10px 20px;
        border: 1px solid #f5c6cb;
        border-radius: 5px;
        z-index: 9998;
        opacity: 0;
        transition: opacity 0.5s ease-in-out; /* 淡入淡出效果 */
    }

        .confirm-box.visible {
            opacity: 1;
        }
</style>

