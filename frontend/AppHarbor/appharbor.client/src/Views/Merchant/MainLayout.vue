<template>
    <div class="main-layout">
        <SideBar @menuItemSelected="handleMenuSelection" />
        <div class="content">
            <div class="main-content">
                <div v-if="selectedMenu === 'outbox'">Outbox content here</div>
                <div v-if="selectedMenu === 'releaseApp'">
                    <ReleaseApp />
                </div>
                <div v-if="selectedMenu === 'trash'">原神，启动！</div>
                <div v-if="selectedMenu === 'records'">
                    <TransactionRecords />
                </div>
            </div>
        </div>
    </div>
</template>

<script>
    import SideBar from './SideBar.vue'
    import ReleaseApp from './ReleaseApp.vue'
    import TransactionRecords from './TransactionRecords.vue';

    export default {
        name: 'MainLayout',
        components: {
            SideBar,
            ReleaseApp,
            TransactionRecords
        },
        data() {
            return {
                selectedMenu: 'outbox'
            }
        },
        methods: {
            handleMenuSelection(menuItem) {
                this.selectedMenu = menuItem;
            }
        }
    }
</script>

<style scoped>
    .main-layout {
        display: flex;
    }

    .header {
        display: flex;
        padding: 16px;
        background-color: #fff;
        box-shadow: 0 2px 4px rgba(0,0,0,0.1);
    }

        .header input {
            width: 100%;
            padding: 8px 16px;
            border: 1px solid #ccc;
            border-radius: 4px;
        }

    .content {
        flex: 1;
        display: flex;
        flex-direction: column;
    }

    .main-content {
        display: flex;
        padding: 16px;
    }
</style>
