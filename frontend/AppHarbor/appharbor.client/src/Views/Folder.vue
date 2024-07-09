<template>
    <div class="folder">
        <h2>{{ folder.name }}</h2>
        <button @click="deleteFolder">删除收藏夹</button>
        <ul>
            <li v-for="item in folder.items" :key="item.id">
                {{ item.name }}
                <button @click="deleteItem(item.id)">删除</button>
            </li>
        </ul>
    </div>
</template>

<script>
    export default {
        props: ['folder'],
        methods: {
            deleteFolder() {
                this.$emit('delete-folder', this.folder.id);
            },
            deleteItem(itemId) {
                this.$emit('delete-item', this.folder.id, itemId);
            }
        }
    };
</script>

<style scoped>
    form {
        margin-bottom: 10px;
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
