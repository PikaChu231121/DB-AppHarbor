<template>
    <div class="report-list">
        <h2>举报信息列表</h2>
        <table>
            <thead>
                <tr>
                    <th>举报ID</th>
                    <th>举报内容</th>
                    <th>应用ID</th>
                    <th>用户ID</th>
                    <th>举报时间</th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="report in reports" :key="report.id">
                    <td>{{ report.id }}</td>
                    <td>{{ report.content }}</td>
                    <td>{{ report.applicationId }}</td>
                    <td>{{ report.userId }}</td>
                    <td>{{ report.time }}</td>
                </tr>
            </tbody>
        </table>
    </div>
</template>

<script>
    import axios from 'axios';
    import Cookies from 'js-cookie';


    export default {
        data() {
            return {
                reports:[],
            };
        },
        mounted() {
            this.getreportlist(); 
        },

        methods: {
            getreportlist() {
                axios.post('http://localhost:5118/api/report/getreportlist')
                    .then(response => {
                        this.reports = response.data.$values;
                        //console.log(this.reports);
                    })
                    .catch(error => {
                        console.error('查看举报列表失败:', error);
                    });
            },
        }
    };
</script>

<style scoped>
    .report-list {
        margin: 20px;
    }

    table {
        width: 100%;
        border-collapse: collapse;
    }

    th, td {
        padding: 10px;
        border: 1px solid #ddd;
        text-align: left;
    }

    th {
        background-color: #f4f4f4;
    }
</style>
