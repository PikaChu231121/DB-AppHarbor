<template>
    <div>
        <alert-box :message="alertMessage"></alert-box>
        <h1>我的收藏夹</h1>
        <button @click="showAddFolderDialog">添加收藏夹</button>

        <div v-if="isAddFolderDialogVisible" class="modal">
            <div class="modal-content">
                <span class="close" @click="closeAddFolderDialog">&times;</span>
                <h2>请输入收藏夹名称</h2>
                <input v-model="newFolderName" placeholder="收藏夹名称">
                <button @click="addFolder">确认</button>
            </div>
        </div>

        <div v-for="folder in folders" :key="folder.id" class="folder">
            <folder :folder="folder" @delete-folder="deleteFolder" @delete-item="deleteItem"></folder>
            <input v-model="newItemName[folder.id]" placeholder="新收藏物名称">
            <button @click="addItem(folder.id)">添加收藏物</button>
        </div>
    </div>
</template>

<script>
    import AlertBox from './AlertBox.vue';
    import Folder from './Folder.vue';

    export default {
        components: {
            AlertBox,
            Folder
        },
        data() {
            // data 默认为空
            return {
                folders: JSON.parse(localStorage.getItem('folders')) || [],
                alertMessage: '',
                newItemName: {},
                isAddFolderDialogVisible: false,
                newFolderName: ''
            };
        },
        methods: {
            saveToLocalStorage() {
                localStorage.setItem('folders', JSON.stringify(this.folders));
            },
            showAddFolderDialog() {
                this.isAddFolderDialogVisible = true;
            },
            closeAddFolderDialog() {
                this.isAddFolderDialogVisible = false;
                this.newFolderName = '';
            },
            addFolder() {
                if (this.newFolderName.trim()) {
                    const newFolder = {
                        id: Date.now(),
                        name: this.newFolderName,
                        items: []
                    };
                    this.folders.push(newFolder);
                    this.saveToLocalStorage();
                    this.showAlert('收藏夹添加成功');
                    this.closeAddFolderDialog();
                } else {
                    this.showAlert('收藏夹名称不能为空');
                }
            },
            addItem(folderId) {
                const folder = this.folders.find(folder => folder.id === folderId);
                if (folder && this.newItemName[folderId]?.trim()) {
                    const newItem = {
                        id: Date.now(),
                        name: this.newItemName[folderId]
                    };
                    folder.items.push(newItem);
                    this.saveToLocalStorage();
                    this.showAlert('收藏物添加成功');
                    this.newItemName[folderId] = '';
                }
            },
            deleteFolder(folderId) {
                this.folders = this.folders.filter(folder => folder.id !== folderId);
                this.saveToLocalStorage();
                this.showAlert('收藏夹删除成功');
            },
            deleteItem(folderId, itemId) {
                const folder = this.folders.find(folder => folder.id === folderId);
                if (folder) {
                    folder.items = folder.items.filter(item => item.id !== itemId);
                    this.saveToLocalStorage();
                    this.showAlert('收藏物删除成功');
                }
            },
            showAlert(message) {
                this.alertMessage = message;
                setTimeout(() => {
                    this.alertMessage = '';
                }, 3000);
            }
        }
    };
</script>

<style scoped>
    h1 {
        color: #42b983;
    }

    form {
        margin-bottom: 20px;
    }

    input {
        margin-right: 10px;
    }

    ul {
        list-style-type: none;
        padding: 0;
    }

    li.folder-item {
        display: flex;
        justify-content: space-between;
        align-items: flex-start;
        margin: 10px 0;
        border: 1px solid #ccc;
        padding: 10px;
        border-radius: 4px;
    }

    .folder-header {
        display: flex;
        flex-direction: column;
        margin-right: 20px;
    }

    button {
        background-color: #42b983;
        color: white;
        border: none;
        padding: 5px 10px;
        cursor: pointer;
    }

    button:hover {
        background-color: #369f79;
    }

    .folder {
        margin-bottom: 20px;
    }

    .modal {
        display: flex;
        justify-content: center;
        align-items: center;
        position: fixed;
        top: 0;
        left: 0;
        width: 100%;
        height: 100%;
        background-color: rgba(0, 0, 0, 0.5);
    }

    .modal-content {
        background-color: #fff;
        padding: 20px;
        border-radius: 5px;
        text-align: center;
        position: relative;
    }

    .close {
        position: absolute;
        top: 10px;
        right: 10px;
        font-size: 20px;
        cursor: pointer;
    }
</style>
