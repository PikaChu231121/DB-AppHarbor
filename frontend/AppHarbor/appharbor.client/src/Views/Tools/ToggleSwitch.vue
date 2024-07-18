<template>
    <div class="switch" :class="{ 'switch-on': isOn }" @click="toggle"
        :style="{ backgroundColor: isOn ? onColor : offColor }">
        <div class="switch-handle"></div>
    </div>
</template>

<script>
export default {
    name: 'SwitchToggle',
    props: {
        onColor: {
            type: String,
            default: '#76c7c0' // 默认开启状态颜色
        },
        offColor: {
            type: String,
            default: '#ccc' // 默认关闭状态颜色
        },
        value: {
            type: Boolean,
            default: false
        }
    },
    data() {
        return {
            isOn: this.value
        };
    },
    methods: {
        toggle() {
            this.isOn = !this.isOn;
            this.$emit('change', this.isOn);
        }
    },
    watch: {
        value(newVal) {
            this.isOn = newVal;
        }
    }
};
</script>

<style>
.switch {
    width: 45px;
    height: 25px;
    border-radius: 25px;
    position: relative;
    cursor: pointer;
    transition: 0.2s;
}

.switch-handle {
    width: 23px;
    height: 23px;
    background-color: #fff;
    border-radius: 50%;
    position: absolute;
    top: 1px;
    left: 1px;
    transition: left 0.2s;
}

.switch-on .switch-handle {
    left: 21px;
}
</style>