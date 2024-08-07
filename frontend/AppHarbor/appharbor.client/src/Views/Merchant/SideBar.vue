<template>
    <div class="sidebar">
        <div class="sidebar-header">
            <i class="icon" />
            <span>商家</span>
        </div>
        <div class="sidebar-menu">
            <ul>
                <li @click="selectMenuItem('outbox')">
                    <span>应用管理</span>
                </li>
                <li @click="selectMenuItem('releaseApp')">
                    <span>发布应用</span>
                </li>
                <li @click="selectMenuItem('trash')">
                    <span>Test</span>
                </li>
                <li @click="selectMenuItem('records')">
                    <span>交易记录</span>
                </li>
            </ul>
        </div>
    </div>
</template>

<script>
export default {
    name: 'SideBar',
    methods: {
        selectMenuItem(menuItem) {
            this.$emit('menuItemSelected', menuItem);
        }
    }
}
</script>

<style scoped>  
.sidebar {  
    width: 240px;  
    background-color: #ffffff; /* 改为白色背景 */  
    height: 100vh;  
    padding: 16px;  
    box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1); /* 添加阴影 */  
    display: flex;  
    flex-direction: column;  
}  

.sidebar-header {  
    display: flex;  
    align-items: center;  
    margin-bottom: 32px;  
    font-size: 24px; /* 增加字体大小 */  
    font-weight: bold; /* 加粗 */  
    color: #333; /* 深色字体 */  
}  

.sidebar-header .icon {  
    width: 24px;  
    height: 24px;  
    background: #007bff; /* 修改图标颜色 */  
    margin-right: 8px;  
    border-radius: 50%; /* 使图标圆角 */  
}  

.sidebar-menu ul {  
    list-style: none;  
    padding: 0;  
    margin: 0;  
    flex-grow: 1;  
}  

.sidebar-menu ul li {  
    padding: 12px;  
    cursor: pointer;  
    font-weight: 600;  
    color: #333; /* 修改字体颜色 */  
    transition: background-color 0.3s ease, color 0.3s ease;  
    font-size: 16px; /* 调整字体大小 */  
    font-family: 'Poppins', sans-serif;  
    border-radius: 8px;  
    background-color: #fafafa; /* 改为淡色背景 */  
    margin-bottom: 10px;  
    display: flex; /* 修改为flex布局 */  
    align-items: center;  
    box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1); /* 添加阴影 */  
}  

.sidebar-menu ul li:hover {  
    background-color: #e6f7ff; /* 修改悬停效果颜色 */  
    color: #007bff; /* 修改悬停时字体颜色 */  
}  

.sidebar-menu ul li .icon-outbox,  
.sidebar-menu ul li .icon-records,  
.sidebar-menu ul li .icon-trash,  
.sidebar-menu ul li .icon-release {  
    width: 24px;  
    height: 24px;  
    background: #007bff;  
    margin-right: 8px;  
    border-radius: 50%;  
}  
</style>
