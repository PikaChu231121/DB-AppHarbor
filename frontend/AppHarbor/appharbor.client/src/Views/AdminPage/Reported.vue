<template>
    <div class="report-list">
        <h2>已受理信息列表</h2>
        <table>
            <thead>
                <tr>
                    <th>应用ID</th>
                    <th>受理管理员ID</th>
                    <th>受理结果</th>
                    <th>受理原因</th>
                    <th>受理时间</th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="reportreview in reportreviews" :key="reportreview.id">
                    <td>{{ reportreview.applicationId }}</td>
                    <td>{{ reportreview.merchantId }}</td>
                    <td>{{ reportreview.state }}</td>
                    <td>{{ reportreview.result }}</td>
                    <td>{{ reportreview.time }}</td>
                </tr>
            </tbody>
        </table>
    </div>
</template>

<script>
    import Cookies from 'js-cookie';
    import axios from 'axios';

    export default {
        data() {
            return {
                reportreviews: [],
            };
        },
        mounted() {
            this.getReportList();
        },
        methods: {
            getReportList() {
                axios.post('http://localhost:5118/api/reportreview/gethandlelist')
                    .then(response => {
                        this.reportreviews = response.data.$values;
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
</style>
