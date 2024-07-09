<template>
    <div v-if="visible" class="alert-box">
        <div class="alert-content">
            <p>{{ message }}</p>
            <button @click="closeAlert">¹Ø±Õ</button>
        </div>
    </div>
</template>

<script>
export default {
  props: ['message'],
  data() {
    return {
      visible: true
    };
  },
  methods: {
    closeAlert() {
      this.visible = false;
    }
  }
}
</script>

<style scoped>
    .alert-box {
        position: fixed;
        top: 50%;
        left: 50%;
        transform: translate(-50%, -50%);
        background: white;
        border: 1px solid #ccc;
        padding: 20px;
        box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
        z-index: 1000;
    }

    .alert-content {
        text-align: center;
    }

    button {
        margin-top: 10px;
        background-color: #42b983;
        color: white;
        border: none;
        padding: 5px 10px;
        cursor: pointer;
    }

        button:hover {
            background-color: #369f79;
        }
</style>
