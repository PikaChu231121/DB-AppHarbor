<template>
    <div class="Wallet">
        <alert-box :msg="alert"></alert-box>
        <confirm-box :msg="confirm"></confirm-box>
        <div class="auto-wrapper">
            <div class="info-box">
                <p class="text">钱包余额</p>
                <p class="merchant-credit" v-html="formattedCredit"></p>
                <div class="button-row">
                    <input type="number" v-model="withdrawAmount" placeholder="输入提现金额" class="input-withdraw" />
                    <button type="button" class="button" @click="withdraw">提现</button>
                </div>
            </div>
            <div class="info-box">
                <p class="text">近期收益统计</p>
                <div class="income-stat">
                    <select v-model="selectedPeriod" @change="renderChart">
                        <option value="week">最近一周</option>
                        <option value="month">最近一月</option>
                        <option value="year">最近一年</option>
                    </select>
                    <div v-if="!chartData.datasets[0].data.length">
                        <p class="no-data-text">暂无数据可显示</p>
                    </div>
                    <canvas id="myChart"></canvas>
                </div>
            </div>
        </div>
    </div>
</template>



<script>
import axios from 'axios';
import Cookies from 'js-cookie';
import { Chart as ChartJS, Title, Tooltip, Legend, ArcElement, PieController, CategoryScale } from 'chart.js';
import AlertBox from '../AlertBox.vue';
import ConfirmBox from '../ConfirmBox.vue';

ChartJS.register(Title, Tooltip, Legend, ArcElement, PieController, CategoryScale);

export default {
    components: {
        AlertBox,
        ConfirmBox
    },
    data() {
        return {
            credit: -1,
            withdrawAmount: 0,
            selectedPeriod: 'month',  // 默认选择最近一月
            alert: '',
            confirm: '',
            chartData: {
                labels: [],
                datasets: [
                    {
                        backgroundColor: [
                            '#FF6384',
                            '#36A2EB',
                            '#FFCE56',
                            '#4BC0C0',
                            '#9966FF',
                            '#FF9F40',
                            '#FF9FF3',
                            '#00F2E2',
                            '#FF7F50',
                            '#9FE2BF'
                        ],
                        data: [],
                        salesCount: []  // 售出份数数据
                    }
                ]
            },
            chartOptions: {
                responsive: true,
                plugins: {
                    legend: {
                        position: 'top',
                    },
                    tooltip: {
                        callbacks: {
                            label: function (tooltipItem) {
                                const dataIndex = tooltipItem.dataIndex;
                                const count = tooltipItem.dataset.salesCount[dataIndex] || 0;  // 获取售出份数
                                const value = tooltipItem.raw || 0;
                                const total = tooltipItem.dataset.data.reduce((acc, curr) => acc + curr, 0);
                                const percentage = ((value / total) * 100).toFixed(2);

                                return [
                                    `售出: ${count}份`,
                                    `收入: ￥${value.toLocaleString()}`,
                                    `占比: ${percentage}%`
                                ];
                            }
                        }
                    }
                }
            },
            chartInstance: null  // 用于保存Chart实例
        };
    },
    methods: {
        fetchCredit() {
            const token = Cookies.get('token');
            axios.post(`${this.$Url}/api/merchant/getCredit`, { token })
                .then(response => {
                    this.credit = response.data.credit;
                })
                .catch(error => {
                    console.error('Error fetching merchant data:', error);
                });
        },

        withdraw() {
            if (this.withdrawAmount <= 0) {
                this.alertNotification('请输入有效的提现金额');
                return;
            }
            if (!/^\d+(\.\d{1,2})?$/.test(this.withdrawAmount)) {
                this.alertNotification('请输入最多两位小数的有效金额');
                return;
            }
            const token = Cookies.get('token');
            axios.post(`${this.$Url}/api/merchant/withdrawCredit`, { token, amount: this.withdrawAmount })
                .then(response => {
                    this.credit = response.data.newCredit;
                    this.confirmNotification('提现成功');
                })
                .catch(error => {
                    console.error('Error withdrawing:', error);
                    this.alertNotification('提现失败');
                });
        },

        renderChart() {
            const token = Cookies.get('token');
            const period = this.selectedPeriod;  // 使用选中的时间范围

            axios.post(`${this.$Url}/api/merchant/incomeStat`, { token, period })
                .then(response => {
                    const data = response.data.$values;

                    // console.log(data); // 检查 data 的结构

                    if (Array.isArray(data)) {
                        // 提取应用名称和总收入数据
                        const labels = data.map(item => item.applicationName);
                        const amounts = data.map(item => item.totalAmount);
                        const salesCount = data.map(item => item.purchaseCount);  // 售出份数
                        // 更新图表数据
                        this.chartData = {
                            labels: labels,
                            datasets: [
                                {
                                    backgroundColor: [
                                        '#FF6384',
                                        '#36A2EB',
                                        '#FFCE56',
                                        '#4BC0C0',
                                        '#9966FF',
                                        '#FF9F40',
                                        '#FF9FF3',
                                        '#00F2E2',
                                        '#FF7F50',
                                        '#9FE2BF'
                                    ],
                                    data: amounts,
                                    salesCount: salesCount
                                }
                            ]
                        };

                        // 销毁旧的图表实例
                        if (this.chartInstance) {
                            this.chartInstance.destroy();
                        }

                        // 重新绘制图表
                        const ctx = document.getElementById('myChart').getContext('2d');
                        this.chartInstance = new ChartJS(ctx, {
                            type: 'pie',
                            data: this.chartData,
                            options: this.chartOptions
                        });
                    } else {
                        console.error("Unexpected data format:", data);
                    }
                })
                .catch(error => {
                    console.error('Error fetching income statistics:', error);
                });
        },
        alertNotification(message) {
            this.alert = message;
            setTimeout(() => {
                this.alert = null;
            }, 3000);
        },
        confirmNotification(message) {
            this.confirm = message;
            setTimeout(() => {
                this.confirm = null;
            }, 3000);
        },
    },
    mounted() {
        this.fetchCredit();
        this.renderChart();
    },
    computed: {
        formattedCredit() {
            if (this.credit < 0) {
                return "正在加载";
            }
            const creditStr = this.credit.toFixed(2).split('.');
            const integerPart = creditStr[0];
            const decimalPart = creditStr[1];
            return `<span>￥ </span><span class="integer-part">${integerPart}</span>.<span class="decimal-part">${decimalPart}</span>`;
        }
    }
}


</script>

<style scoped>
.Wallet {
    position: relative;
    display: flex;
    flex-direction: column;
    align-items: center;
    width: 100%;
    height: 100%;
    color: #42a5f5;
}

.auto-wrapper {
    display: flex;
    flex-direction: row;
    align-items: center;
    justify-content: space-between;
    width: 100%;
    height: 100%;
    margin: 10px 10px 3px;
}

.info-box {
    display: flex;
    flex-direction: column;
    align-items: center;
    background: #f0f9ff;
    box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
    padding: 2% 5% 2% 5%;
    width: calc(50% - 8px);
    height: 80vh;
    border-radius: 10px;
    box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
}

.income-stat select {
    margin-bottom: 10px;
    padding: 5px;
    font-size: 16px;
    border: 2px solid #1976d2;
    border-radius: 5px;
    background-color: #42a5f5;
    color: #f0f9ff;
}

.merchant-credit {
    font-size: 30px;
    /* 基本字体大小 */
}

::v-deep .integer-part {
    font-size: 50px;
}

::v-deep .decimal-part {
    font-size: 20px;
}

.input-withdraw {
    padding: 10px;
    font-size: 16px;
    margin-right: 10px;
    margin-left: 120px;
    border: 2px solid #1e99e5;
    border-radius: 5px;
}

.text {
    font-family: Inika;
    font-size: 2rem;
    color: #1976d2;
    line-height: normal;
    text-align: left;
    vertical-align: top;
    font-weight: 400;
    white-space: normal;
    margin-bottom: 1rem;
}

.button-row {
    width: 100%;
    display: flex;
    /* justify-content: space-between; */
    margin-top: 1em;
}

button {
    padding: 10px 20px;
    background: linear-gradient(to right, #1976d2, #115293);
    font-size: 18px;
    color: #fff;
    border: none;
    border-radius: 10px;
    cursor: pointer;
    transition: background-color 0.3s, transform 0.3s, color 0.3s;
}

button:disabled {
    cursor: not-allowed;
}

button:hover:enabled {
    background: linear-gradient(to right, #115293, #1976d2);
    transform: scale(1.05);
    color: #fff;
    box-shadow: 0 8px 15px rgba(0, 0, 0, 0.3);
    transition: background-color 0.3s, transform 0.3s, color 0.3s;
}
</style>
