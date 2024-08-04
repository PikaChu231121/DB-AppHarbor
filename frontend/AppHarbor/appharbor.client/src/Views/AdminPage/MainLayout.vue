<template>
    <div class="main-layout">
        <div class="sidebar">
            <div class="menu">
                <div class="menu-item" @click="selectseleasing">待审核</div>
                <div class="menu-item" @click="selectseleased">已审核</div>
            </div>
        </div>
        <div class="main-content">
            <div v-for="item in items" :key="item.id" class="app-item">
                <div class="app-header">
                    <div>{{ item.header }}</div>
                    <div>{{ item.subheader }}</div>
                </div>
                <div class="app-actions">
                    <button>上架</button>
                    <button>点击</button>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
    import axios from 'axios';
    import Cookies from 'js-cookie';

    export default {
        name: "MainLayout",
        data() {
            return {
                items: [],
            };
        },
        methods: {
            selectseleasing() {
                axios.post('http://localhost:5118/api/application/selectseleasing')
                    .then(response => {
                        this.items = response.data.$values;
                        console.log(this.items);

                    })
                    .catch(error => {

                    });
            },
            selectseleased() {
                var token = Cookies.get('token');
                let formData = new FormData();
                formData.append('token', token);
                axios.post('http://localhost:5118/api/application/selectseleased', formData)
                    .then(response => {
                        this.items = response.data.$values;
                        console.log(this.items);
                    })
                    .catch(error => {
                        
                    });
            }
        }
    };
</script>

<style scoped>
    .main-layout {
        display: flex;
        height: 100%;
    }

    .sidebar {
        width: 250px;
        background-color: #f4f4f4;
        padding: 20px;
    }

    .menu-item, .label {
        padding: 10px 0;
        cursor: pointer;
    }

        .menu-item:hover, .label:hover {
            background-color: #ddd;
        }

    .main-content {
        padding: 20px;
        flex-grow: 1;
    }

    .app-item {
        border: 1px solid #ddd;
        padding: 10px;
        margin-bottom: 10px;
        display: flex;
        justify-content: space-between;
    }

    .app-actions {
        display: flex;
        gap: 10px;
    }
</style>
