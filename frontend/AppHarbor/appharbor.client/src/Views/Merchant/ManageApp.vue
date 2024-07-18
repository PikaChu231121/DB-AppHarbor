<template>
    <div class="card-container">
        <div class="card" v-for="item in items" :key="item.id">
            <div class="card-header">
                <div class="app-img" :style="{ backgroundImage: `url(${item.img_url})` }"></div>
                <div class="text-content">
                    <div class="header">{{ item.app_name }}</div>
                    <div class="subhead">{{ item.app_detail }}</div>
                </div>
                <div class="toggle-container" @mouseenter="showTooltip(item.enabled, $event)" @mouseleave="hideTooltip"
                    @mousemove="updateTooltipPosition(item.enabled, $event)">
                    <ToggleSwitch class="toggle-switch" :value="item.enabled" onColor="#448080" offColor="#cccccc"
                        @change="handleToggleChange(item, $event)" />
                </div>
                <div class="more">⋮</div>
            </div>
        </div>
    </div>
    <!-- <div v-if="tooltip.visible" :style="tooltipStyle" class="tooltip">
        {{ tooltip.content }}
    </div> -->
</template>

<script>
import ToggleSwitch from '../Tools/ToggleSwitch.vue';
export default {
    name: 'ParentComponent',
    components: {
        ToggleSwitch
    },
    data() {
        return {
            items: [
                { id: 1, img_url: 'https://via.placeholder.com/40', app_name: 'app1', app_detail: '111', enabled: true },
                { id: 2, img_url: 'https://via.placeholder.com/40', app_name: 'app2', app_detail: '222', enabled: false },
                { id: 3, img_url: 'https://via.placeholder.com/40', app_name: 'app3', app_detail: '333', enabled: true }
            ],
            // tooltip: {
            //     visible: false,
            //     content: '',
            //     x: 0,
            //     y: 0
            // }
        }
    },
    methods: {
        handleToggleChange(item, newValue) {
            item.enabled = newValue;
            console.log(`Item with id ${id} is now ${newValue ? 'enabled' : 'disabled'}`);
            // 在此处添加其他函数逻辑，例如更新状态到服务器
        },
        // showTooltip(enabled, event) {
        //     this.tooltip.content = enabled ? '已开启' : '未开启';
        //     this.updateTooltipPosition(enabled, event);
        //     this.tooltip.visible = true;
        // },
        // hideTooltip() {
        //     this.tooltip.visible = false;
        // },
        // updateTooltipPosition(enabled, event) {
        //     this.tooltip.content = enabled ? '已开启' : '未开启';
        //     this.tooltip.x = event.clientX + 10; // 鼠标右下方
        //     this.tooltip.y = event.clientY + 10; // 鼠标右下方
        // }
    },
    computed: {
        // tooltipStyle() {
        //     return {
        //         position: 'absolute',
        //         left: `${this.tooltip.x}px`,
        //         top: `${this.tooltip.y}px`
        //     };
        // }
    }
}
</script>

<style scoped>
body {
    font-family: Arial, sans-serif;
    background-color: #e6f4f1;
    margin: 0;
    padding: 20px;
}

.card-container {
    display: flex;
    flex-direction: column;
    gap: 20px;
}

.card {
    display: flex;
    align-items: center;
    background-color: #ffffff;
    border-radius: 10px;
    padding: 10px;
    box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
    transition: 0.2s;
}

.card:hover {
    box-shadow: 0 4px 8px rgba(90, 176, 150, 0.405);
    transform: scale(1.01);
}

.card-header {
    display: flex;
    align-items: center;
    width: 100%;
}

.app-img {
    background-color: #73c2b5;
    border-radius: 10px;
    color: white;
    display: flex;
    align-items: center;
    justify-content: center;
    width: 40px;
    height: 40px;
    font-size: 24px;
    margin-right: 10px;
    background-size: cover;
    background-position: center;
}

.toggle-switch {
    right: 20px;
}

.text-content {
    flex: 1;
}

.header {
    font-weight: bold;
    font-size: 16px;
}

.subhead {
    color: #777;
    font-size: 14px;
}

.more {
    cursor: pointer;
    font-size: 20px;
    color: #888;
}

/* .tooltip {
    overflow: hidden;
    background-color: #333;
    color: white;
    padding: 5px 10px;
    border-radius: 5px;
    white-space: nowrap;
    z-index: 1000;
    font-size: 12px;
} */
</style>