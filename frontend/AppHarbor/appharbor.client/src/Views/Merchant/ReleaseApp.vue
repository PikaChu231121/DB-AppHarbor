<template>
    <div class="release-app">
        <h2>发布应用</h2>
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
                    <input type="text" id="appCategory" v-model="appCategory" required />
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
export default {
    name: 'ReleaseApp',
    data() {
        return {
            appName: '',
            appVersion: '',
            appImage: null,
            appImageUrl: '../../../public/logo.png',
            appFile: null,
            appDescription: '',
            appPrice: 0,
            appCategory: '',
            appId: ''
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
            let formData = new FormData();
            formData.append('file', this.appFile);
            formData.append('Name', this.appName);
            formData.append('Version', this.appVersion);
            formData.append('Description', this.appDescription);
            formData.append('Price', this.appPrice);
            formData.append('Category', this.appCategory);
            console.log(this.appFile);
            axios.post('http://localhost:5118/api/application/uploadapp', formData, {
                headers: {
                    'Content-Type': 'multipart/form-data'
                }
            })
                .then(response => {
                    this.appId = response.data.applicationId;
                    let formDataImg = new FormData();
                    formDataImg.append('file', this.appImage);
                    formDataImg.append('id', this.appId);
                    axios.post('http://localhost:5118/api/Image/upload-app-img', formDataImg)
                        .then(response => {
                            console.log(response);
                        })
                        .catch(error => {
                            console.error('Error uploading avatar:', error);
                            alert('图片上传失败');
                        });
                    alert('应用发布成功');
                })
                .catch(error => {
                    console.log(error);
                    alert('应用发布失败');
                });
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
    min-width: 900px;
    margin: 0 auto;
    font-family: 'Baloo 2', cursive, Arial, sans-serif;
    font-size: 16px;
}

.release-app h2 {
    margin-bottom: 20px;
    color: #1976d2;
    /* 更改字体颜色为蓝色调 */
    text-align: center;
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
</style>