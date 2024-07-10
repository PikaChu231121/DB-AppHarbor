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
            <input v-model="newItemName[folder.id]" placeholder="新收藏应用名称">
            <button @click="addItem(folder.id)">添加收藏应用</button>
        </div>
    </div>
</template>

<script>
    import AlertBox from './AlertBox.vue';
    import Folder from './Folder.vue';
    import axios from 'axios';

    export default {
        components: {
            AlertBox,
            Folder
        },
        data() {
            
            return {
                user: { id: '' },
                folders: JSON.parse(localStorage.getItem('folders')) || [],
                alertMessage: '',
                newItemName: {},
                isAddFolderDialogVisible: false,
                newFolderName: ''
            };
        },
        mounted() {
            this.fetchUserFavourite();
        },
        methods: {
            fetchUserFavourite() {
                console.log('successfully build connection');
                axios.post('http://localhost:5118/api/favourite/favourite', { id: this.user.id })
                    .then(response => {
                        console.log('successfully get user favourite');
                        this.favourite = response.data; // 假设response.data包含folders数组
                    })
                    .catch(error => {
                        console.error('Error fetching user favourite data:', error);
                    });
            },
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
                    this.showAlert('收藏应用添加成功');
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
                    this.showAlert('收藏应用删除成功');
                }
            },
            showAlert(message) {
                this.alertMessage = message;
                setTimeout(() => {
                    this.alertMessage = '';
                }, 3000);
            }
        },
    };
</script>

<style scoped>
    h1 {
        color: #42b983;
        text-align: center;
        margin-bottom: 20px;
    }

    button {
        background-color: #42b983;
        color: white;
        border: none;
        padding: 10px 20px;
        cursor: pointer;
        border-radius: 4px;
        transition: background-color 0.3s ease;
    }

    button:hover {
        background-color: #369f79;
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
        z-index: 1000;
    }

    .modal-content {
        background-color: #fff;
        padding: 20px;
        border-radius: 5px;
        text-align: center;
        position: relative;
        box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
        max-width: 400px;
        width: 100%;
    }

    .close {
        position: absolute;
        top: 10px;
        right: 10px;
        font-size: 20px;
        cursor: pointer;
        color: #999;
    }

    .close:hover {
        color: #666;
    }

    .folder {
        margin-bottom: 20px;
        padding: 10px;
        background-color: #f9f9f9;
        border: 1px solid #ddd;
        border-radius: 4px;
    }

    .folder input {
        margin-bottom: 5px;
        padding: 5px;
        border: 1px solid #ddd;
        border-radius: 4px;
    }
</style>
