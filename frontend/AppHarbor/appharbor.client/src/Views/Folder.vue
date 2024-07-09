<template>
    <div>
        <form @submit.prevent="addItem">
            <input v-model="newItem" placeholder="添加收藏项" required />
            <button type="submit">添加</button>
        </form>
        <ul>
            <li v-for="(item, index) in folder.items" :key="index">
                {{ item }}
                <button @click="removeItem(index)">删除</button>
            </li>
        </ul>
    </div>
</template>

<script>
export default {
  props: ['folder'],
  data() {
    return {
      newItem: ''
    };
  },
  methods: {
    addItem() {
      if (this.newItem.trim() !== '') {
        this.folder.items.push(this.newItem.trim());
        this.$emit('updateFolder', this.folder);
        this.newItem = '';
      }
    },
    removeItem(index) {
      this.folder.items.splice(index, 1);
      this.$emit('updateFolder', this.folder);
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
