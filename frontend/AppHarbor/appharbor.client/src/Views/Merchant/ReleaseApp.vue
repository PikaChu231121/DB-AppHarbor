<template>
    <div class="release-app">
        <alert-box :msg="alert"></alert-box>
        <confirm-box :msg="confirm"></confirm-box>
        <h1>发布应用</h1>
        <div class="form-container">
            <div class="form-column">
                <div class="form-group">
                    <label for="appName">应用名称</label>
                    <input type="text" id="appName" v-model="appName" required />
                </div>
                <div class="form-group">
                    <label for="appVersion">应用版本</label>
                    <input type="text" id="appVersion" v-model="appVersion" required />
                </div>
                <div class="form-group">
                    <label for="appPrice">应用价格</label>
                    <input type="number" id="appPrice" v-model="appPrice" required />
                </div>
                <div class="form-group">
                    <label for="appCategory">应用类别</label>
                    <select id="appCategory" v-model="appCategory" required>
                        <option value="" disabled selected>请选择类别</option>
                        <option value="娱乐">娱乐</option>
                        <option value="社交">社交</option>
                        <option value="购物">购物</option>
                        <option value="健康养生">健康养生</option>
                        <option value="办公">办公</option>
                        <option value="学习">学习</option>
                    </select>
                </div>
            </div>
            <div class="form-column">
                <div class="form-group">
                    <label for="appImage">应用图片</label>
                    <div class="app-img">
                        <img :src="appImageUrl" alt="应用图片" class="preview-img" />
                        <div class="edit-icon">
                            <img src="../../../public/editing.png" @click="triggerImageUpload" />
                        </div>
                    </div>
                    <input type="file" id="appImage" ref="imageInput" @change="handleImageUpload" class="img-input" />
                </div>
                <div class="form-group">
                    <label for="appFile">应用文件</label>
                    <input type="file" id="appFile" @change="handleFileUpload" class="file-input" />
                </div>
            </div>
        </div>
        <div class="form-group">
            <label for="appDescription">应用描述</label>
            <textarea id="appDescription" v-model="appDescription" required></textarea>
        </div>
        <button @click="upload" :disabled="!isFormValid">发布</button>
    </div>
</template>

<script>
import axios from 'axios';
import Cookies from 'js-cookie';
import AlertBox from '../AlertBox.vue';
import ConfirmBox from '../ConfirmBox.vue';
export default {
    name: 'ReleaseApp',
    components: {
        AlertBox,
        ConfirmBox
    },
    data() {
        return {
            appName: '',
            appVersion: '',
            appImage: null,
            appImageUrl: '@/../public/logo.png',
            appFile: null,
            appDescription: '',
            appPrice: 0,
            appCategory: '',
            appId: '',
            alert: '',
            confirm: ''
        }
    },
    methods: {
        handleImageUpload(event) {
            this.appImage = event.target.files[0];
            this.appImageUrl = URL.createObjectURL(this.appImage);
        },
        handleFileUpload(event) {
            this.appFile = event.target.files[0];
        },
        isValidInput(value) {
            return value && value.trim() !== '';
        },
        triggerImageUpload() {
            this.$refs.imageInput.click();
        },
        upload() {
            const token = Cookies.get('token');
            let formData = new FormData();
            formData.append('file', this.appFile);
            formData.append('Name', this.appName);
            formData.append('Version', this.appVersion);
            formData.append('Description', this.appDescription);
            formData.append('Price', this.appPrice);
            formData.append('Category', this.appCategory);
            formData.append('Token', token);
            console.log(this.appFile);
            axios.post(`${this.$Url}/api/application/uploadapp`, formData, {
                headers: {
                    'Content-Type': 'multipart/form-data'
                }
            })
                .then(response => {
                    this.appId = response.data.applicationId;
                    let formDataImg = new FormData();
                    formDataImg.append('file', this.appImage);
                    formDataImg.append('id', this.appId);
                    axios.post(`${this.$Url}/api/Image/upload-app-img`, formDataImg)
                        .then(response => {
                            console.log(response);
                        })
                        .catch(error => {
                            console.error('Error uploading avatar:', error);
                            this.alertNotification('图片上传失败');
                        });
                    this.resetForm();
                    this.confirmNotification('应用发布成功');
                })
                .catch(error => {
                    console.log(error);
                    this.alertNotification('应用发布失败');
                });
        },
        alertNotification(message) {
            this.alert = message;
        },
        confirmNotification(message) {
            this.confirm = message;
        },
        resetForm() {
            this.appName = '';
            this.appVersion = '';
            this.appImage = null;
            this.appImageUrl = '../../../public/logo.png';
            this.appFile = null;
            this.appDescription = '';
            this.appPrice = 0;
            this.appCategory = '';
            this.alert = '';
            this.confirm = '';
        }
    },
    computed: {
        isFormValid() {
            return this.isValidInput(this.appName) &&
                this.isValidInput(this.appVersion) &&
                this.isValidInput(this.appDescription) &&
                this.appPrice >= 0 &&
                this.isValidInput(this.appCategory) &&
                this.appImage !== null &&
                this.appFile !== null;
        }
    }
}
</script>

<style scoped>
.release-app {
    background-color: #f0f9ff;
    /* 更改背景色为浅蓝色 */
    padding: 20px;
    border-radius: 10px;
    box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
    margin: 0 auto;
    font-family: 'Baloo 2', cursive, Arial, sans-serif;
    font-size: 16px;
    width: 90%;
    height: 75vh;
    display: flex;
    flex-direction: column;
}

.release-app h1 {
    margin-bottom: 20px;
    color: #1976d2;
    /* 更改字体颜色为蓝色调 */
}

.form-container {
    display: flex;
    justify-content: space-between;
}

.form-column {
    width: 48%;
}

.form-group {
    margin-bottom: 20px;
}

.form-group label {
    display: block;
    margin-bottom: 10px;
    font-weight: bold;
    color: #1976d2;
    /* 更改字体颜色为蓝色调 */
}

.form-group input,
.form-group textarea {
    width: 100%;
    padding: 10px;
    border: 1px solid #ccc;
    border-radius: 5px;
    background-color: #ffffff;
    /* 确保输入框背景色为白色 */
}

.form-group input[type="file"] {
    padding: 3px;
}

.app-img {
    margin-bottom: 20px;
    text-align: center;
}

.preview-img {
    margin-top: 10px;
    width: 185px;
    height: 185px;
    padding: 10px;
    border: 1px solid #42a5f5;
    /* 更改边框颜色 */
    background-color: #e3f2fd;
    /* 更改背景色为浅蓝色 */
    box-shadow: 0 10px 10px rgba(0, 0, 0, 0.1);
    border-radius: 5px;
    margin: auto;
}

.file-input {
    width: 100%;
    padding: 10px;
    border: 1px solid #ccc;
    border-radius: 5px;
    background-color: #ffffff;
    /* 确保输入框背景色为白色 */
    font-family: 'Baloo 2', cursive;
    font-size: 16px;
}

.img-input {
    display: none;
}

.edit-icon {
    cursor: pointer;
    align-items: center;
}

.edit-icon img {
    width: 24px;
    /* 调整图标大小 */
    height: 24px;
    /* 调整图标大小 */
    transition: box-shadow 0.3s ease, filter 0.3s ease;
}

.edit-icon:hover img {
    box-shadow: 0 1px 1px rgba(0, 0, 0, 0.3);
    filter: brightness(1.1);
}

button {
    padding: 10px 20px;
    background-color: #1e88e5;
    /* 更改按钮背景色为蓝色 */
    color: #fff;
    border: none;
    border-radius: 5px;
    cursor: pointer;
    font-size: 16px;
    display: block;
    width: 100%;
    font-family: 'Baloo 2', cursive;
    font-size: 26px;
    transition: background-color 0.3s, transform 0.3s, color 0.3s;
}

button:disabled {
    cursor: not-allowed;
}

button:hover:enabled {
    background-color: #1565c0;
    /* 更改悬停时背景色为深蓝色 */
    transform: scale(1.02);
    transition: background-color 0.3s, transform 0.3s, color 0.3s;
}

.form-group select {
    width: 100%;
    padding: 10px;
    border: 1px solid #ccc;
    border-radius: 5px;
    background-color: #ffffff;
    font-family: 'Baloo 2', cursive;
    font-size: 16px;
    appearance: none;
    /* 去除默认的下拉箭头 */
    background-image: url('data:image/svg+xml;utf8,<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 20 20" fill="%231976d2"><path d="M5.5 8l4.5 4 4.5-4H5.5z"/></svg>');
    /* 添加自定义箭头 */
    background-repeat: no-repeat;
    /* 不重复箭头 */
    background-position: right 10px center;
    /* 箭头位置 */
    background-size: 16px 16px;
    /* 箭头大小 */
    transition: border-color 0.3s, box-shadow 0.3s;
    /* 添加过渡效果 */
}

.form-group select:focus {
    border-color: #1976d2;
    /* 聚焦时边框颜色 */
    box-shadow: 0 0 5px rgba(25, 118, 210, 0.5);
    /* 聚焦时的阴影效果 */
    outline: none;
    /* 去除轮廓 */
}

.form-group select:hover {
    border-color: #42a5f5;
    /* 悬停时边框颜色 */
}
</style>