<template>
    <div class="report-list">
        <h2 style="font-weight: bolder; font-size: 30px">
            <svg xmlns="http://www.w3.org/2000/svg" width="35" height="35" viewBox="0 0 24 24">
                <g fill="none" stroke="#000000" stroke-linecap="round" stroke-linejoin="round" stroke-width="2">
                    <path d="M8 5H6a2 2 0 0 0-2 2v12a2 2 0 0 0 2 2h5.697M18 14v4h4m-4-7V7a2 2 0 0 0-2-2h-2" />
                    <path
                        d="M8 5a2 2 0 0 1 2-2h2a2 2 0 0 1 2 2v0a2 2 0 0 1-2 2h-2a2 2 0 0 1-2-2m6 13a4 4 0 1 0 8 0a4 4 0 1 0-8 0m-6-7h4m-4 4h3" />
                </g>
            </svg>
            我的举报信息
        </h2>
        <table>
            <thead>
                <tr>
                    <th style="font-size: 19px; font-weight: bolder">举报编号</th>
                    <th style="font-size: 19px; font-weight: bolder">举报应用名称</th>
                    <th style="font-size: 19px; font-weight: bolder">举报商家名称</th>
                    <th style="font-size: 19px; font-weight: bolder">举报内容</th>
                    <th style="font-size: 19px; font-weight: bolder">举报时间</th>
                    <th style="font-size: 19px; font-weight: bolder">举报状态</th>
                    <th style="font-size: 19px; font-weight: bolder">操作</th>
                </tr>
            </thead>
            <tbody>
                <tr v-if="reports.length === 0">
                    <td colspan="7" style="text-align: center; font-size: 18px; padding: 20px;">
                        你还没有举报
                    </td>
                </tr>
                <tr v-else v-for="report in reports" :key="report.reportId">
                    <td style="font-size: 16px;">{{ report.reportId }}</td>
                    <td style="font-size: 16px;">{{ report.applicationName }}</td>
                    <td style="font-size: 16px;">{{ report.merchantName }}</td>
                    <td style="font-size: 17px;">
                        <button @click="viewReportContent(report)">
                            查看内容
                        </button>
                    </td>
                    <td style="font-size: 16px;">{{ report.reportTime.replace('T', '-') }}</td>
                    <td :class="getStatusClass(report.reportState)" style="font-size: 16px;">
                        {{ translatedState(report) }}
                    </td>
                    <td style="font-size: 17px;">
                        <span v-if="report.reportState === 'reviewing'">待受理</span>
                        <button v-else @click="viewDetails(report)">查看详细</button>
                    </td>
                </tr>
            </tbody>
        </table>

        <!-- 举报内容弹窗 -->
        <div v-if="selectedContent" class="content-popup">
            <h3 style="font-size: 25px; font-weight: bolder">举报内容</h3>
            <p style="font-size: 18px;">{{ selectedContent }}</p>
            <button @click="closeContent" style="font-size: 16px;">关闭</button>
        </div>

        <!-- 详细信息弹窗 -->
        <div v-if="selectedReport" class="detail-popup">
            <h3 style="font-size: 25px; font-weight: bolder">详细信息</h3>
            <p style="font-size: 17px;">
                <strong style="font-size: 19px; font-weight: bolder">受理时间:&nbsp;&nbsp;&nbsp;</strong> {{
                        selectedReport.reviewInfo.reviewTime.replace('T', '-')
                }}
            </p>
            <p style="font-size: 17px;"><strong
                    style="font-size: 19px; font-weight: bolder">受理结果:&nbsp;&nbsp;&nbsp;</strong> {{
                        selectedReport.reviewInfo.reviewResult }}</p>
            <p style="font-size: 17px;"><strong
                    style="font-size: 19px; font-weight: bolder">受理管理员ID:&nbsp;&nbsp;&nbsp;</strong> {{
                        selectedReport.reviewInfo.adminId }}</p>
            <button @click="closeDetails" style="font-size: 16px;">关闭</button>
        </div>
    </div>
</template>


<script>
import axios from 'axios';
import Cookies from 'js-cookie';

export default {
    data() {
        return {
            reports: [],
            selectedReport: null,
            selectedContent: null,
        };
    },
    mounted() {
        this.fetchReports();
    },
    methods: {
        fetchReports() {
            const token = Cookies.get('token');
            axios.post(`${this.$Url}/api/user/getmyreport`, { token })
                .then(response => {
                    this.reports = response.data.$values;
                })
                .catch(error => {
                    console.error('获取举报信息失败:', error);
                    Cookies.remove('token');
                    this.$router.push('/').then(() => {
                        // 刷新登录页面
                        window.location.reload();
                    });
                    alert("token异常，请重新登陆！");
                });
        },
        viewReportContent(report) {
            this.selectedContent = report.reportContent;
        },
        viewDetails(report) {
            this.selectedReport = report;
        },
        closeDetails() {
            this.selectedReport = null;
        },
        closeContent() {
            this.selectedContent = null;
        },
        getStatusClass(status) {
            switch (status) {
                case 'accept':
                    return 'status-accept';
                case 'refuse':
                    return 'status-refuse';
                case 'reviewing':
                    return 'status-reviewing';
                default:
                    return '';
            }
        },
        translatedState(report) {
            switch (report.reportState) {
                case 'accept':
                    return '通过';
                case'refuse':
                    return '不通过';
                case 'reviewing':
                    return '待受理';
                default:
                    return '';
            }
        }
    },
};
</script>

<style scoped>
.report-list {
    margin: 20px;
    font-weight: bold;
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

td {
    padding: 12px;
    border: 1px solid #ddd;
    text-align: left;
    transition: background-color 0.3s ease;
    font-size: 1.2em;
    /* Increased font size for table cells */
}

th {
    background-color: #ebbfb8;
    color: #fff;
    font-size: 1.2em;
    /* Font size for table headers, already increased */
}

h2 {
    color: black;
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
    background-color: #ebbfb8;
    color: #fff;
    font-size: 1.2em;
    /* Increased font size for table headers */
}

tbody tr {
    transition: background-color 0.3s ease;
}

tbody tr:hover {
    background-color: #f9d9d9;
    /* Changed hover background color to a lighter pink */
}

th,
td {
    font-size: 1em;
}

button {
    background-color: #ebbfb8;
    color: #fff;
    border: none;
    border-radius: 5px;
    cursor: pointer;
    padding: 6px 12px;
    font-size: 0.9em;
    font-weight: bold;
    transition: background-color 0.3s ease;
}

button:hover {
    background-color: #d8a8a0;
}

.content-popup,
.detail-popup {
    position: fixed;
    top: 50%;
    left: 50%;
    transform: translate(-50%, -50%);
    background-color: white;
    padding: 20px;
    box-shadow: 0 4px 8px rgba(0, 0, 0, 0.2);
    border-radius: 8px;
    width: 400px;
    /* Fixed width */
    height: 300px;
    /* Fixed height */
    overflow: auto;
    /* Allow scrolling if content overflows */
}

.content-popup h3,
.detail-popup h3 {
    color: #ebbfb8;
    margin-bottom: 15px;
    text-align: center;
}

.content-popup p,
.detail-popup p {
    margin: 10px 0;
    word-wrap: break-word;
    /* 自动换行 */
    word-break: break-all;
    /* 强制换行，防止长单词超出容器 */
    white-space: normal;
    /* 允许文本换行 */
    max-width: 100%;
    /* 限制宽度，使内容不超出弹窗边界 */
    overflow-wrap: break-word;
    /* 避免单词超出容器边界 */
}

.content-popup button,
.detail-popup button {
    position: absolute;
    /* Position button absolutely within the popup */
    bottom: 20px;
    right: 20px;
    padding: 10px 20px;
    background-color: #ebbfb8;
    color: white;
    border: none;
    border-radius: 5px;
    cursor: pointer;
}

.content-popup button:hover,
.detail-popup button:hover {
    background-color: #d8a8a0;
}

/* Status background colors */
.status-accept {
    background-color: #d4edda;
    /* Light green */
    color: #155724;
    /* Dark green */
}

.status-refuse {
    background-color: #f8d7da;
    /* Light red */
    color: #721c24;
    /* Dark red */
}

.status-reviewing {
    background-color: #fff3cd;
    /* Light yellow */
    color: #856404;
    /* Dark yellow */
}
</style>
