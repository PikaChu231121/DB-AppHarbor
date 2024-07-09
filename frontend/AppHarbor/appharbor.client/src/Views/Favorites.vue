<template>
    <div>
        <h1>个人收藏</h1>
        <form @submit.prevent="addFolder">
            <input v-model="newFolder" placeholder="新建收藏夹" required />
            <button type="submit">新建</button>
        </form>
        <ul>
            <li v-for="(folder, index) in folders" :key="index">
                <h3>{{ folder.name }}</h3>
                <button @click="removeFolder(index)">删除收藏夹</button>
                <Folder :folder="folder" @updateFolder="updateFplder(index,$event)" />
            </li>
        </ul>
        <AlertBox v-if="alertVisible" :message="alertMessage" @close="alertVisible = false" />
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
            return {
                newFolder: '',
                folders: JSON.parse(localStorage.getItem('folders')) || [{ name: '默认收藏夹', items: [] }]
            };
        },
        methods: {
            addFolder() {
                if (this.newFolder.trim() !== '') {
                    this.folders.push({ name: this.newFolder.trim(), items: [] });
                    this.updateLocalStorage();
                    this.newFolder = '';
                }
            },
            removeFolder(index) {
                this.folders.splice(index, 1);
                this.updateLocalStorage();
            },
            updateFolder(index, updatedFolder) {
                this.$set(this.folders, index, updatedFolder);
                this.updateLocalStorage();
            },
            updateLocalStorage() {
                localStorage.setItem('folders', JSON.stringify(this.folders));
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

    li {
        margin: 5px 0;
        display: flex;
        justify-content: space-between;
        align-items: center;
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
</style>
