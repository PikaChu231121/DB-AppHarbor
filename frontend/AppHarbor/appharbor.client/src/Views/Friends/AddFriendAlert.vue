<template>
    <div v-if="visible" class="alert-box">
        <div class="alert-content">
            <p>{{ message }}</p>
        </div>
    </div>
</template>

<script>
    export default {
        name: 'LoginAlert',
        props: ['message'],
        data() {
            return {
                visible: true
            };
        },
        mounted() {
            setTimeout(() => {
                this.closeAlert();
            }, 3000); // Close the alert after 2 seconds
        },
        methods: {
            closeAlert() {
                this.visible = false;
                this.$emit('close');
            }
        }
    };
</script>

<style scoped>
    .alert-box {
        position: fixed;
        top: 0;
        left: 50%;
        transform: translateX(-50%);
        width: 100%;
        max-width: 180px;
        display: flex;
        justify-content: center;
        align-items: center;
        height: 150px; /* 或者你需要的高度 */
        z-index: 1000;
    }

    .alert-content {

        text-align: center;
        margin-top: 30px; /* Add some margin to position it below the top edge */
    }
</style>