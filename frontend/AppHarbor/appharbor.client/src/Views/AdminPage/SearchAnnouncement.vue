<template>
    <div>
        <ul>
            <li v-for="announce in announces" :key="announce.id">
                <h2>{{ announce.title }}</h2>
                <p>{{ announce.content }}</p>
                <p>发布者 ID: {{ announce.adminId }}</p>
                <p>发布时间: {{ announce.publishTime }}</p>
            </li>
        </ul>
    </div>
</template>

<script>
    import axios from 'axios';


    export default {
        data() {
            return {
                announces: [],
            };
        },
        mounted() {
            this.searchAnnouncement();  // 这里调用方法应该用 this
        },
        methods: {
            searchAnnouncement() {
                axios.post('http://localhost:5118/api/announcement/getannouncementlist')
                    .then(response => {
                        this.announces = response.data.$values;  // 确保 response.data.$values 是正确的
                        console.log(this.announces);
                    })
                    .catch(error => {
                        console.error('查看公告失败:', error);
                    });
            },
        },
    };
</script>

<style scoped>
    h1 {
        font-size: 24px;
        margin-bottom: 10px;
    }

    ul {
        list-style-type: none;
        padding: 0;
    }

    li {
        margin-bottom: 20px;
        border-bottom: 1px solid #ccc;
        padding-bottom: 10px;
    }

    h2 {
        margin: 0;
        font-size: 20px;
    }

    p {
        margin: 5px 0;
    }
</style>
