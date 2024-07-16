<template>
    <div class="release-app">
        <h2>发布应用</h2>
        <form @submit.prevent="handleSubmit">
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
            <button type="submit" :disabled="!isFormValid">发布</button>
        </form>
    </div>
</template>

<script>
    export default {
        name: 'ReleaseApp',
        data() {
            return {
                appName: '',
                appVersion: '',
                appImage: null,
                appFile: null,
                appDescription: '',
                appPrice: '',
                appCategory: ''
            }
        },
        computed: {
            isFormValid() {
                return this.appName && this.appVersion && this.appDescription && this.appPrice && this.appCategory;
            }
        },
        methods: {
            handleImageUpload(event) {
                this.appImage = event.target.files[0];
            },
            handleFileUpload(event) {
                this.appFile = event.target.files[0];
            },
            handleSubmit() {
                if (this.isFormValid) {
                    const formData = new FormData();
                    formData.append('appName', this.appName);
                    formData.append('appVersion', this.appVersion);
                    formData.append('appImage', this.appImage);
                    formData.append('appFile', this.appFile);
                    formData.append('appDescription', this.appDescription);
                    formData.append('appPrice', this.appPrice);
                    formData.append('appCategory', this.appCategory);

                    // Submit the form data to the server
                    // axios.post('/api/releaseApp', formData)
                    //   .then(response => {
                    //     alert('应用发布成功');
                    //   })
                    //   .catch(error => {
                    //     alert('应用发布失败');
                    //   });
                }
            }
        }
    }
</script>

<style scoped>
    .release-app {
        background-color: #fff;
        padding: 16px;
        border-radius: 8px;
        box-shadow: 0 2px 4px rgba(0,0,0,0.1);
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
