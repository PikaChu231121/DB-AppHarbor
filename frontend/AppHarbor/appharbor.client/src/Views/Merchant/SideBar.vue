<template>
    <div class="sidebar">
        <div class="sidebar-header">
            <i class="icon" />
            <span>Mail</span>
            <span class="count">24</span>
        </div>
        <div class="sidebar-menu">
            <ul>
                <li @click="selectMenuItem('outbox')">
                    <i class="icon-outbox" />
                    <span>应用管理</span>
                </li>
                <li @click="selectMenuItem('releaseApp')">
                    <i class="icon-favorites" />
                    <span>发布应用</span>
                </li>
                <li @click="selectMenuItem('trash')">
                    <i class="icon-trash" />
                    <span>Test</span>
                </li>
                <li @click="selectMenuItem('favorites')">
                    <i class="icon-release" />
                    <span>Test</span>
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
    background-color: #f8f9fa;
    height: 100vh;
    padding: 16px;
}

.sidebar-header {
    display: flex;
    align-items: center;
    margin-bottom: 32px;
}

.sidebar-header .icon {
    width: 24px;
    height: 24px;
    background: #ccc;
    /* Use an icon */
    margin-right: 8px;
}

.sidebar-header .count {
    background-color: #007bff;
    color: #fff;
    border-radius: 12px;
    padding: 4px 8px;
    margin-left: auto;
}

.sidebar-menu ul {
    list-style: none;
    padding: 0;
}

.sidebar-menu ul li {
    display: flex;
    align-items: center;
    padding: 8px 0;
    cursor: pointer;
}

.sidebar-menu ul li .icon-outbox,
.sidebar-menu ul li .icon-favorites,
.sidebar-menu ul li .icon-trash,
.sidebar-menu ul li .icon-release {
    width: 24px;
    height: 24px;
    background: #ccc;
    /* Use appropriate icons */
    margin-right: 8px;
}
</style>
