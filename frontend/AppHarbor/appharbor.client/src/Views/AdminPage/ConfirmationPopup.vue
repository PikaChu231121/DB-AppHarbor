<template>
    <div v-if="show" class="confirmation-popup">
        <div class="popup-content">
            <p>您确定发布该公告吗？</p>
            <div class="button-group">
                <button class="confirm-button" @click="confirm">确定</button>
                <button class="cancel-button" @click="cancel">取消</button>
            </div>
        </div>
    </div>
</template>

<script>
    export default {
        props: {
            show: {
                type: Boolean,
                required: true
            }
        },
        methods: {
            confirm() {
                this.$emit('confirm');
            },
            cancel() {
                this.$emit('cancel');
            }
        }
    };
</script>

<style scoped>
    .confirmation-popup {
        position: fixed;
        top: 0;
        left: 0;
        right: 0;
        bottom: 0;
        background: rgba(0, 0, 0, 0.6); /* Darker background for better contrast */
        display: flex;
        align-items: center;
        justify-content: center;
        z-index: 1000; /* Ensure the popup is on top */
        animation: fadeIn 0.3s ease-out;
    }

    .popup-content {
        background: #fff;
        padding: 30px;
        border-radius: 10px;
        box-shadow: 0 8px 16px rgba(0, 0, 0, 0.2);
        text-align: center;
        max-width: 400px; /* Restrict the width for better appearance */
        animation: scaleUp 0.3s ease-out;
    }

        .popup-content p {
            font-family: 'Poppins', sans-serif; /* Cute font style */
            font-size: 1.125rem;
            color: #333;
            margin-bottom: 20px;
        }

    .button-group {
        display: flex;
        justify-content: center;
        gap: 15px; /* Space between buttons */
    }

    button {
        padding: 12px 25px;
        font-size: 1rem;
        border: none;
        border-radius: 5px;
        cursor: pointer;
        transition: background-color 0.3s ease, transform 0.2s ease;
        font-family: 'Poppins', sans-serif;
    }

    .confirm-button {
        background-color: #4CAF50; /* Green background for confirm */
        color: #fff;
    }

        .confirm-button:hover {
            background-color: #45a049;
            transform: translateY(-2px);
        }

        .confirm-button:active {
            background-color: #388e3c;
            transform: translateY(0);
        }

    .cancel-button {
        background-color: #f44336; /* Red background for cancel */
        color: #fff;
    }

        .cancel-button:hover {
            background-color: #e53935;
            transform: translateY(-2px);
        }

        .cancel-button:active {
            background-color: #c62828;
            transform: translateY(0);
        }

    @keyframes fadeIn {
        from {
            opacity: 0;
        }

        to {
            opacity: 1;
        }
    }

    @keyframes scaleUp {
        from {
            transform: scale(0.9);
        }

        to {
            transform: scale(1);
        }
    }
</style>
