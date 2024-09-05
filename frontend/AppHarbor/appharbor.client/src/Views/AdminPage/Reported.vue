<template>
    <div class="report-list">
        <h2>已受理信息列表</h2>
        <table>
            <thead>
                <tr>
                    <th>举报编号</th>
                    <th>被举报应用</th>
                    <th>被举报应用商家</th>
                    <th>受理结果</th>
                    <th>受理时间</th>
                    <th>操作</th> <!-- 添加操作列 -->
                </tr>
            </thead>
            <tbody>
                <tr v-for="reportreview in reportreviews" :key="reportreview.id">
                    <td>{{ reportreview.reportId }}</td>
                    <td>{{ reportreview.applicationName }}</td>
                    <td>{{ reportreview.merchantName }}</td>
                    <td>{{ translatedState(reportreview.state) }}</td>
                    <td>{{ reportreview.time.replace(' ', '-') }}</td>
                    <td>
                        <button @click="showDetails(reportreview)">查看详情</button> <!-- 查看详情按钮 -->
                    </td>
                </tr>
            </tbody>
        </table>

        <!-- 弹窗内容 -->
        <div v-if="showPopup" class="popup">
            <div class="popup-content">
                <button class="close-button" @click="closePopup">X</button>
                <h3>举报受理详情</h3>
                <p>举报编号: {{ selectedReport.reportId }}</p>
                <p>受理意见: {{ selectedReport.result }}</p>
                <p>受理结果: {{ translatedState(selectedReport.state) }}</p>
                <br>
                <p style="font-size:15px">举报用户: {{ selectedReport.userName
                    }}&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;该用户ID:{{
                    selectedReport.userId }}</p>
                <p style="font-size:15px">被举报下架应用: {{ selectedReport.applicationName
                    }}&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;该应用ID:{{
                        selectedReport.applicationId }}</p>
                <p style="font-size:15px">被举报商家: {{ selectedReport.merchantName
                    }}&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;该商家ID:{{
                    selectedReport.merchantId }}</p>
                <p style="font-size:15px">受理时间: {{ selectedReport.time.replace(' ', '-') }}</p>
                <p style="font-size:15px">受理该举报管理员编号: {{ selectedReport.adminId }}</p>
                <!-- 添加更多详细内容 -->
            </div>
        </div>
    </div>
</template>

<script>
import axios from 'axios';

export default {
    data() {
        return {
            reportreviews: [],
            showPopup: false, // 控制弹窗显示的状态
            selectedReport: {}, // 存储当前选中的报告详情
        };
    },
    mounted() {
        this.getReportList();
    },
    methods: {
        getReportList() {
            axios.post(`${this.$Url}/api/reportreview/gethandlelist`)
                .then(response => {
                    this.reportreviews = response.data.$values.map(reportreview => {
                        reportreview.time = this.formatTime(reportreview.time);
                        return reportreview;
                    });
                })
                .catch(error => {
                    console.error('查看举报列表失败:', error);
                });
        },
        formatTime(dateTime) {
            // Replace "T" with a space
            dateTime = dateTime.replace('T', ' ');
            // Convert to Date object
            let date = new Date(dateTime);
            // Add 8 hours
            date.setHours(date.getHours() + 0);
            // Format as YYYY-MM-DD HH:mm:ss
            let year = date.getFullYear();
            let month = ('0' + (date.getMonth() + 1)).slice(-2);
            let day = ('0' + date.getDate()).slice(-2);
            let hours = ('0' + date.getHours()).slice(-2);
            let minutes = ('0' + date.getMinutes()).slice(-2);
            let seconds = ('0' + date.getSeconds()).slice(-2);
            return `${year}-${month}-${day} ${hours}:${minutes}:${seconds}`;
        },
        showDetails(report) {
            this.selectedReport = report; // 设置选中的报告详情
            this.showPopup = true; // 显示弹窗
        },
        closePopup() {
            this.showPopup = false; // 关闭弹窗
            this.selectedReport = {}; // 清空选中的报告详情
        },
        translatedState(state) {
            switch (state) {
                case 'accept':
                    return '通过';
                case 'refuse':
                    return '不通过';
                case 'reviewing':
                    return '待受理';
                default:
                    return '';
            }
        }
    }
};
</script>

<style scoped>
.report-list {
    margin: 20px;
    font-family: 'Comic Sans MS', cursive, sans-serif;
    color: #333;
    background-color: #f9f9f9;
    border-radius: 8px;
    box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
    padding: 20px;
    transition: background-color 0.3s ease;
}

.report-list:hover {
    background-color: #f1f1f1;
}

h2 {
    color: #6a1b9a;
    margin-bottom: 15px;
    font-size: 1.5em;
    text-align: center;
}

table {
    width: 100%;
    border-collapse: collapse;
    transition: opacity 0.3s ease;
}

th,
td {
    padding: 12px;
    border: 1px solid #ddd;
    text-align: left;
    transition: background-color 0.3s ease;
}

th {
    background-color: #6a1b9a;
    color: #fff;
}

tbody tr {
    transition: background-color 0.3s ease;
}

tbody tr:hover {
    background-color: #f1e4f3;
}

th,
td {
    font-size: 1em;
}

button {
    background-color: #6a1b9a;
    color: white;
    border: none;
    padding: 8px 16px;
    border-radius: 8px;
    cursor: pointer;
    font-family: 'Comic Sans MS', cursive, sans-serif;
    font-size: 1em;
    font-weight: bold;
    transition: background-color 0.3s ease, transform 0.2s ease;
}

button:hover {
    background-color: #ca95f7;
    transform: scale(1.05);
}

button:active {
    background-color: #ca95f7;
    transform: scale(1.05);
}

.popup {
    position: fixed;
    top: 0;
    left: 0;
    right: 0;
    bottom: 0;
    background: rgba(0, 0, 0, 0.6);
    display: flex;
    justify-content: center;
    align-items: center;
    transition: opacity 0.3s ease;
}

.popup-content {
    position: relative;
    background: white;
    padding: 30px;
    border-radius: 12px;
    box-shadow: 0 6px 12px rgba(0, 0, 0, 0.2);
    max-width: 500px;
    width: 100%;
    transition: transform 0.3s ease;
    transform: scale(0.9);
    overflow: auto;
    padding: 20px;
    border-radius: 10px;
    background-color: #fff;
}

.popup-content p {
    margin: 10px 0;
    font-size: 1.2em;
    color: #333;
    word-wrap: break-word;
    white-space: pre-wrap;
}

.popup-content h3 {
    margin: 0;
    font-weight: bold;
    font-size: 1.8em;
    color: #6a1b9a;
    text-align: center;
    font-family: 'SimSun', serif;
}

.form-group {
    margin-top: 20px;
}

textarea {
    width: 100%;
    padding: 10px;
    border: 1px solid #ddd;
    border-radius: 5px;
    font-weight: bold;
    font-family: 'SimSun', serif;
}

.button-group {
    margin-top: 20px;
    display: flex;
    justify-content: flex-end;
}

.submit-button,
.cancel-button {
    padding: 10px 20px;
    border: none;
    border-radius: 5px;
    margin-left: 10px;
    cursor: pointer;
}

.submit-button {
    background-color: #4caf50;
    color: white;
}

.submit-button:hover {
    background-color: #45a049;
}

.cancel-button {
    background-color: #f44336;
    color: white;
}

.cancel-button:hover {
    background-color: #e53935;
}

.close-button {
    background-color: #d9534f;
    color: white;
    border: none;
    cursor: pointer;
}

.close-button:hover {
    background-color: #c9302c;
}

.close-button {
    position: absolute;
    top: 10px;
    right: 10px;
    background: transparent;
    border: none;
    font-size: 1.2em;
    color: white;
    cursor: pointer;
    background-color: #6a1b9a;
    transition: background-color 0.3s ease, transform 0.2s ease;
}

.close-button:hover {
    background-color: #ca95f7;
    transform: scale(1.05);
    color: white;
}

.close-button:active {
    background-color: #ca95f7;
    transform: scale(1.05);
    color: white;
}
</style>
