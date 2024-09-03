<template>
    <div class="report-list">
        <h2>举报信息列表</h2>
        <table>
            <thead>
                <tr>
                    <th>举报ID</th>
                    <th>被举报应用</th>
                    <th>被举报应用商家</th>
                    <th>举报用户</th>
                    <th>举报时间</th>
                    <th>操作</th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="report in reports" :key="report.id">
                    <td>{{ report.reportId }}</td>
                    <td>{{ report.applicationName }}</td>
                    <td>{{ report.merchantNickname }}</td>
                    <td>{{ report.userNickname }}</td>
                    <td>{{ report.time.replace(' ', '-') }}</td>
                    <td>
                        <button @click="viewReport(report)">受理</button>
                    </td>
                </tr>
            </tbody>
        </table>

        <!-- Popup for report details -->
        <div v-if="selectedReport" class="popup">
            <div class="popup-content">
                <h3>举报内容详情</h3>
                <p><strong>举报内容:&nbsp;&nbsp;&nbsp;</strong> {{ selectedReport.content }}<br><br></p>
                <p style="font-size:15px"><strong>举报用户:</strong> {{ selectedReport.userNickname }}&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<strong>该用户ID:</strong> {{ selectedReport.userId }}</p>
                <p style="font-size:15px"><strong>被举报应用:</strong> {{ selectedReport.applicationName }}&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<strong>该应用ID:</strong> {{ selectedReport.applicationId }}</p>
                <p style="font-size:15px"><strong>被举报应用商家:</strong> {{ selectedReport.merchantNickname }}&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<strong>该商家ID:</strong> {{ selectedReport.merchantId }}</p>
                <p style="font-size:15px"><strong>举报时间:</strong> {{ selectedReport.time.replace(' ', '-') }}&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<strong>该举报ID:</strong> {{ selectedReport.reportId }}</p>

                <!-- New input and buttons -->
                <div class="form-group">
                    <label for="response-content" style="font-weight:bold">请填写受理内容:</label>
                    <textarea id="response-content" v-model="responseContent" rows="4" placeholder="请输入处理结果"></textarea>
                </div>
                <div class="button-group">
                    <button class="submit-button" @click="acceptreports">通过</button>
                    <button class="submit-button" @click="refusereports">不通过</button>
                    <button class="cancel-button" @click="closePopup">取消</button>
                </div>
            </div>
        </div>

        <!-- Success message popup -->
        <div v-if="successMessage" class="popup success-popup">
            <div class="popup-content">
                <h3><svg xmlns="http://www.w3.org/2000/svg" width="1em" height="1em" viewBox="0 0 24 24"><g fill="none" stroke="currentColor" stroke-linejoin="round"><circle cx="12" cy="12" r="9" stroke-linecap="round" stroke-width="2" /><path stroke-width="3" d="M12 16h.01v.01H12z" /><path stroke-linecap="round" stroke-width="2" d="M12 12V8" /></g></svg>
                管理员操作成功</h3>
                <p>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;举报通过，{{selectedReport.applicationName}}应用已成功下架！</p>
            </div>
        </div>

        <!-- Refuse message popup -->
        <div v-if="RefuseMessage" class="popup success-popup">
            <div class="popup-content">
                <h3><svg xmlns="http://www.w3.org/2000/svg" width="1em" height="1em" viewBox="0 0 24 24"><g fill="none" stroke="currentColor" stroke-linejoin="round"><circle cx="12" cy="12" r="9" stroke-linecap="round" stroke-width="2" /><path stroke-width="3" d="M12 16h.01v.01H12z" /><path stroke-linecap="round" stroke-width="2" d="M12 12V8" /></g></svg>
                管理员操作成功</h3>
                <p>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;举报不通过，{{selectedReport.applicationName}}应用保留！</p>
            </div>
        </div>
    </div>
</template>

<script>
    import Cookies from 'js-cookie';
    import axios from 'axios';

    export default {
        data() {
            return {
                reports: [],
                selectedReport: null,
                responseContent: '',
                successMessage: false,
                RefuseMessage: false,
            };
        },
        mounted() {
            this.getReportList();
        },
        methods: {
            getReportList() {
                axios.post(`${this.$Url}/api/report/getreportlist`)
                    .then(response => {
                        this.reports = response.data.$values.map(report => {
                            report.time = this.formatTime(report.time);
                            return report;
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
            viewReport(report) {
                this.selectedReport = report;
            },
            closePopup() {
                this.selectedReport = null;
            },
            acceptreports() {
                var token = Cookies.get('token');
                let formData = new FormData();
                formData.append('id', this.selectedReport.reportId);
                formData.append('token', token);
                formData.append('result', this.responseContent);
                axios.post(`${this.$Url}/api/reportreview/acceptreports`, formData)
                    .then(response => {
                        console.log(response.data);
                        this.successMessage = true;
                        setTimeout(() => {
                            this.closePopup();
                            this.successMessage = false;
                        }, 2000); // Close the success message after 2 seconds
                        this.getReportList();
                        responseContent = '';
                        selectedReport = null;
                        
                    })
                    .catch(error => {
                        console.error('受理应用举报失败:', error);
                    });
            },
            refusereports() {
                var token = Cookies.get('token');
                let formData = new FormData();
                formData.append('id', this.selectedReport.reportId);
                formData.append('token', token);
                formData.append('result', this.responseContent);
                axios.post(`${this.$Url}/api/reportreview/refusereports`, formData)
                    .then(response => {
                        console.log(response.data);
                        this.RefuseMessage = true;
                        setTimeout(() => {
                            this.closePopup();
                            this.RefuseMessage = false;
                        }, 2000); // Close the success message after 2 seconds
                        this.getReportList();
                        responseContent = '';
                        selectedReport = null;
                    })
                    .catch(error => {
                        console.error('受理应用举报失败:', error);
                    });
            },
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
        box-shadow: 0 4px 8px rgba(0,0,0,0.1);
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

    th, td {
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

    th, td {
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
        background: rgba(0,0,0,0.6);
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
        box-shadow: 0 6px 12px rgba(0,0,0,0.2);
        max-width: 500px;
        width: 100%;
        transition: transform 0.3s ease;
        transform: scale(0.9);
        overflow: auto; /* Add this line to handle overflow */
        padding: 20px;
        border-radius: 10px;
        background-color: #fff;
    }

        .popup-content p {
            margin: 10px 0;
            font-size: 1.2em; /* Larger font size */
            color: #333; /* Adjust color if needed */
            word-wrap: break-word; /* Ensure long words wrap */
            white-space: pre-wrap; /* Preserve spaces and line breaks */
        }


        .popup-content h3 {
            margin: 0;
            font-weight: bold;
            font-size: 1.8em; /* Larger size */
            color: #6a1b9a; /* Purple color */
            text-align: center; /* Centered */
            font-family: 'SimSun', serif; /* SimSun font */
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
        font-family: 'SimSun', serif; /* Set font to SimSun */
    }

    .button-group {
        margin-top: 20px;
        display: flex;
        justify-content: flex-end;
    }

    .submit-button, .cancel-button {
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
        /* Existing styles */
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
        font-size: 1.2em; /* Smaller size */
        color: white; /* Purple color */
        cursor: pointer;
        background-color: #6a1b9a;
        transition: background-color 0.3s ease, transform 0.2s ease;
    }

        .close-button:hover {
            background-color: #ca95f7;
            transform: scale(1.05);
            color: white; /* Lighter purple on hover */
        }

        .close-button:active {
            background-color: #ca95f7;
            transform: scale(1.05);
            color: white; /* Even lighter purple when clicked */
        }

    .success-popup .popup-content {
        background-color: #d4edda; /* Light green background for success */
    }

    .success-popup h3 {
        color: #155724; /* Dark green text color for success */
    }

    .success-popup .close-button {
        background-color: #6a1b9a;
    }
</style>
