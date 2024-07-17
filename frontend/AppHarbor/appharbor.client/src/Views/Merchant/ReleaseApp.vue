<template>
    <div class="release-app">
        <h2>发布应用</h2>
        <form>
            <div class="form-group">
                <label for="appName">应用名称</label>
                <input type="text" id="appName" v-model="appName" required />
            </div>
            <div class="form-group">
                <label for="appVersion">应用版本</label>
                <input type="text" id="appVersion" v-model="appVersion" required />
            </div>
            <div class="form-group">
                <label for="appImage">应用图片</label>
                <input type="file" id="appImage" @change="handleImageUpload" />
            </div>
            <div class="form-group">
                <label for="appFile">应用文件</label>
                <input type="file" id="appFile" @change="handleFileUpload" />
            </div>
            <div class="form-group">
                <label for="appDescription">应用描述</label>
                <textarea id="appDescription" v-model="appDescription" required></textarea>
            </div>
            <div class="form-group">
                <label for="appPrice">应用价格</label>
                <input type="number" id="appPrice" v-model="appPrice" required />
            </div>
            <div class="form-group">
                <label for="appCategory">应用类别</label>
                <input type="text" id="appCategory" v-model="appCategory" required />
            </div>
        </form>
            <button @click="upload"  :disabled="!isFormValid">发布</button>
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
        },
        // handleFileUpload(event) {
        //     this.appFile = event.target.files[0];
        // },
        isValidInput(value) {
            return value && value.trim() !== '';
        },
        upload() {
            let formData = new FormData();
            formData.append('Name', this.appName);
            formData.append('Version', this.appVersion);
            formData.append('Description', this.appDescription);
            formData.append('Price', this.appPrice);
            formData.append('Category', this.appCategory);

            // console.log(this.appName);
            // console.log(this.appVersion);
            // console.log(this.appDescription);
            // console.log(this.appPrice);
            // console.log(this.appCategory);

            axios.post('http://localhost:5118/api/application/uploadapp', formData, {
                headers: {
                    'Content-Type': 'multipart/form-data'
                }
            })
                .then(response => {
                    this.appId = response.data.applicationId;
                    console.log(this.appId);
                    let formDataImg = new FormData();
                    formDataImg.append('file', this.appImage);
                    formDataImg.append('id', this.appId);
                    axios.post('http://localhost:5118/api/Image/upload-app-img', formDataImg)
                        .then(response => {
                            //console.log(this.user.avatar);
                            alert('图片上传成功');
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
                this.appPrice &&
                this.isValidInput(this.appCategory);
                // this.appImage !== null &&
                // this.appFile !== null;
        }
    },

}
</script>

<style scoped>
.release-app {
    background-color: #fff;
    padding: 16px;
    border-radius: 8px;
    box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
}

.release-app h2 {
    margin-bottom: 16px;
}

.form-group {
    margin-bottom: 16px;
}

.form-group label {
    display: block;
    margin-bottom: 8px;
    font-weight: bold;
}

.form-group input,
.form-group textarea {
    width: 100%;
    padding: 8px;
    border: 1px solid #ccc;
    border-radius: 4px;
}

button {
    padding: 8px 16px;
    background-color: #007bff;
    color: #fff;
    border: none;
    border-radius: 4px;
    cursor: pointer;
}

button:disabled {
    background-color: #ccc;
    cursor: not-allowed;
}
</style>
