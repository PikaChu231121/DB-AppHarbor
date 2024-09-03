<template>
    <img class="background-image" src="@/assets/merchant.svg">
    <div class="main-layout">
        <SideBar @menuItemSelected="handleMenuSelection" />
        <div class="content">
            <div class="main-content">
                <div v-if="selectedMenu === 'manageApp'">
                    <ManageApp />
                </div>
                <div v-if="selectedMenu === 'releaseApp'">
                    <ReleaseApp />
                </div>
                <div v-if="selectedMenu === 'records'">
                    <TransactionRecords />
                </div>
                <div v-if="selectedMenu === 'wallet'" class="main-content-wallet">
                <MerchantWallet />
                </div>
                <div v-if="selectedMenu === 'announcement'">
                    <MerchantAnnouncement />
                </div>
            </div>
        </div>
    </div>
</template>

<script>
import SideBar from './SideBar.vue'
import ReleaseApp from './ReleaseApp.vue'
import TransactionRecords from './TransactionRecords.vue';
import ManageApp from './ManageApp.vue';
import MerchantWallet from './MerchantWallet.vue'
import MerchantAnnouncement from './MerchantAnnouncement.vue';
export default {
    name: 'MainLayout',
    components: {
        SideBar,
        ReleaseApp,
        TransactionRecords,
        ManageApp,
        MerchantWallet,
        MerchantAnnouncement
    },
    data() {
        return {
            selectedMenu: 'manageApp'
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
    background-color: #dbefff;
}

.header {
    display: flex;
    padding: 16px;
    background-color: #fff;
    box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
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
    flex: 1;
    display: center;
    padding: 16px;
}

.main-content-wallet {
    flex: 1;
    display: grid;
    padding: 16px;
}

    .background-image {
        position: absolute;
        top: 0;
        left: 0;
        width: 100%;
        height: 100%;
        background-size: cover;
        background-position: center;
        z-index: -1;
    }
</style>
