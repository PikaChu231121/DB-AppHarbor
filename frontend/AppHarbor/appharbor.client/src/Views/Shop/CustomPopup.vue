<template>
    <div class="custom-popup" v-if="visible">
        <div class="custom-popup-content">
            <p>{{ message }}</p>
            <button @click="closePopup">确定</button>
        </div>
    </div>
</template>

<script>
    export default {
        name: 'CustomPopup',
        props: {
            message: {
                type: String,
                required: true
            },
            visible: {
                type: Boolean,
                default: false
            }
        },
        methods: {
            closePopup() {
                this.$emit('close');
            }
        }
    };
</script>

<style scoped>
    .custom-popup {
        position: fixed;
        top: 50%;
        left: 50%;
        transform: translate(-50%, -50%);
        background-color: white; 
        border: 2px solid #ebbfb8;
        border-radius: 10px;
        padding: 20px;
        z-index: 1000;
        width: 300px;
        box-shadow: 0px 4px 8px rgba(0, 0, 0, 0.1);
    }

    .custom-popup-content {
        text-align: center;
    }

        .custom-popup-content p {
            font-size: 16px;
            color: #333;
            margin-bottom: 20px;
            font-weight: bold;
            font-family: 'Comic Sans MS', cursive, sans-serif;
        }

        .custom-popup-content button {
            background-color: #ebbfb8;
            color: #fff;
            border: none;
            padding: 10px 20px;
            border-radius: 5px;
            cursor: pointer;
            font-weight: bold;
            transition: background-color 0.3s ease;
        }

            .custom-popup-content button:hover {
                background-color: #f3c7ba;
            }
</style>
