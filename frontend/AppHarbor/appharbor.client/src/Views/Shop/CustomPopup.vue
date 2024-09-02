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
        border-radius: 15px;
        padding: 25px;
        z-index: 1000;
        width: 320px;
        box-shadow: 0px 12px 24px rgba(0, 0, 0, 0.2);
        animation: popup-fade-in 0.3s ease-out, popup-scale-up 0.3s ease-out;
    }

    @keyframes popup-fade-in {
        from {
            opacity: 0;
        }

        to {
            opacity: 1;
        }
    }

    @keyframes popup-scale-up {
        from {
            transform: scale(0.9) translate(-50%, -50%);
        }

        to {
            transform: scale(1) translate(-50%, -50%);
        }
    }

    .custom-popup-content {
        text-align: center;
    }

        .custom-popup-content p {
            font-size: 18px;
            color: #333;
            margin-bottom: 25px;
            font-weight: bold;
            font-family: 'Comic Sans MS', cursive, sans-serif;
        }

        .custom-popup-content button {
            background-color: #ebbfb8;
            color: #fff;
            border: none;
            padding: 12px 25px;
            border-radius: 8px;
            cursor: pointer;
            font-weight: bold;
            font-size: 16px;
            transition: background-color 0.3s ease, transform 0.2s ease;
            box-shadow: 0px 6px 12px rgba(0, 0, 0, 0.15);
        }

            .custom-popup-content button:hover {
                background-color: #f3c7ba;
                transform: translateY(-3px);
            }

            .custom-popup-content button:active {
                background-color: #ebbfb8;
                transform: translateY(1px);
            }
</style>
