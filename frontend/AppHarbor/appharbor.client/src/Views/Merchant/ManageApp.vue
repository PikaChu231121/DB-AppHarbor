<template>
    <div class="manage-app">
        <alert-box :msg="alert"></alert-box>
        <confirm-box :msg="confirm"></confirm-box>
        <h1>应用管理</h1>
        <div class="search-bar">
            <input v-model="searchQuery" placeholder="输入您的应用的任何相关内容..." class="global-search" />
            <button @click="initiateSearch" class="search-button">全局搜索</button>
            <button @click="toggleAdvancedSearch" class="advanced-search-toggle">
                {{ showAdvancedSearch ? '隐藏高级检索' : '高级检索' }}
            </button>
            <button @click="refreshPage" class="refresh-button">刷新全部应用</button>
            <div v-if="showAdvancedSearch" class="advanced-search">
                <input v-model="searchName" placeholder="应用名称搜索..." />
                <input v-model="searchCategory" placeholder="应用种类搜索..." />
                <input v-model="searchVersion" placeholder="应用版本搜索..." />
                <input v-model="searchState" placeholder="应用状态搜索..." />
            </div>
        </div>
        <div class="app-list-container">
            <div class="app-list">
                <table>
                    <thead>
                        <tr>
                            <th @click="changeSort('appId')" class="sortable">
                                应用ID
                                <span class="sort-icons">
                                    <span v-if="sortBy === 'appId' && sortOrder === 'asc'" class="arrow-up">▲</span>
                                    <span v-if="sortBy === 'appId' && sortOrder === 'desc'" class="arrow-down">▼</span>
                                </span>
                            </th>
                            <th @click="changeSort('name')" class="sortable">
                                应用名称
                                <span class="sort-icons">
                                    <span v-if="sortBy === 'name' && sortOrder === 'asc'" class="arrow-up">▲</span>
                                    <span v-if="sortBy === 'name' && sortOrder === 'desc'" class="arrow-down">▼</span>
                                </span>
                            </th>
                            <th @click="changeSort('version')" class="sortable">
                                版本
                                <span class="sort-icons">
                                    <span v-if="sortBy === 'version' && sortOrder === 'asc'" class="arrow-up">▲</span>
                                    <span v-if="sortBy === 'version' && sortOrder === 'desc'"
                                          class="arrow-down">▼</span>
                                </span>
                            </th>
                            <th @click="changeSort('releaseState')" class="sortable">
                                状态
                                <span class="sort-icons">
                                    <span v-if="sortBy === 'releaseState' && sortOrder === 'asc'"
                                          class="arrow-up">▲</span>
                                    <span v-if="sortBy === 'releaseState' && sortOrder === 'desc'"
                                          class="arrow-down">▼</span>
                                </span>
                            </th>
                            <th>操作</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr v-for="app in apps" :key="app.id">
                            <td>{{ app.id }}</td>
                            <td>{{ app.name }}</td>
                            <td>{{ app.version }}</td>
                            <td :class="getStateClass(app.releaseState)">
                                {{ getStateInChinese(app.releaseState) }}
                            </td>
                            <td>
                                <button v-if="app.releaseState !== 'banned'" @click="openEditModal(app)" class="advanced-search-toggle">编辑</button>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <div class="pagination">
                <button @click="prevPage" :disabled="currentPage === 1">上一页</button>
                <span>第 {{ currentPage }} 页</span>
                <span>/ 共 {{ totalPages }} 页</span> <!-- 添加总页数提示 -->
                <button @click="nextPage" :disabled="currentPage === totalPages || totalPages === 1">下一页</button>
            </div>
            <div v-if="showEditModal" class="modal">
                <div class="modal-content">
                    <span class="close" @click="closeEditModal">&times;</span>
                    <h2>编辑应用</h2>

                    <div class="form-group-row">
                        <div class="form-group">
                            <label>应用ID:</label>
                            <p>{{ selectedApp.id }}</p>
                        </div>
                        <div class="form-group">
                            <label>名称:</label>
                            <p>{{ selectedApp.name }}</p>
                        </div>
                    </div>

                    <div class="form-group-row">
                        <div class="form-group">
                            <label>分类:</label>
                            <p>{{ selectedApp.category }}</p>
                        </div>
                        <div class="form-group">
                            <label>下载量:</label>
                            <p>{{ selectedApp.downloadCount }}</p>
                        </div>
                    </div>

                    <div class="form-group-row">
                        <div class="form-group">
                            <label>状态:</label>
                            <p>{{ translatedState  }}</p>
                        </div>
                        <div class="form-group">
                            <label>版本:</label>
                            <input v-model="selectedApp.version" placeholder="请输入新的版本号..." type="text" />
                        </div>
                    </div>

                    <div class="form-group">
                        <label>描述:</label>
                        <textarea v-model="selectedApp.description" rows="4" placeholder="请输入应用描述..."></textarea>
                    </div>

                    <div class="form-group">
                        <label>当前图标:</label>
                        <div class="image-preview">
                            <img :src="getFullImageUrl(originalAppData.image)" alt="应用图标" />
                        </div>
                    </div>

                    <div class="form-group">
                        <label>更新图标:</label>
                        <input type="file" @change="handleNewImageUpload" />
                    </div>

                    <!-- 显示上传后的新图标 -->
                    <div class="form-group" v-if="selectedImageFile">
                        <div class="image-preview">
                            <img :src="newImageUrl" alt="新应用图标预览" />
                        </div>
                    </div>

                    <div class="form-group">
                        <label>更新应用包:</label>
                        <input type="file" @change="handleNewFileUpload" />
                    </div>

                    <div class="form-group-row">

                        <div class="form-group">
                            <label>原价:</label>
                            <div class="input-container">
                                <span class="currency-symbol">￥</span>
                                <input v-model="selectedApp.price" placeholder="请输入原价..." class="price-input" />
                            </div>
                        </div>

                        <div class="form-group">
                            <label>折扣:</label>
                            <select v-model="selectedApp.discount" :disabled="selectedApp.price == 0" :title="selectedApp.price == 0 ? '免费应用不能设置折扣' : ''">
                                <option value="1.00">无折扣</option>
                                <option value="0.90">9折</option>
                                <option value="0.80">8折</option>
                                <option value="0.75">75折</option>
                                <option value="0.70">7折</option>
                                <option value="0.60">6折</option>
                                <option value="0.50">5折</option>
                            </select>
                        </div>
                    </div>
                    <div v-if="selectedApp.discount !== '1.00'" class="form-group">
                        <span style="display: block; text-align: center;">折后价: ￥{{ discountedPrice }}</span>
                    </div>

                    <div class="button-group">
                        <button @click="saveAppChanges" class="save-button" :disabled="!isModified">保存</button>
                        <button @click="confirmDelete" class="delete-button">下架应用</button>
                    </div>

                    <div v-if="showConfirmDelete" class="modal">
                        <div class="modal-content" style="text-align: center;">
                            <span class="close" @click="closeConfirmDelete">&times;</span>
                            <h2>确认下架</h2>
                            <p class="confirmation-text" style="margin: 15px 0;">您确定要下架此应用吗？此操作无法撤销。</p>
                            <div class="button-group" style="justify-content: center; gap: 20px;">
                                <button @click="deleteApp" class="confirm-yes-button">是</button>
                                <button @click="closeConfirmDelete" class="confirm-no-button">否</button>
                            </div>
                        </div>
                    </div>
                </div>
                </div>
        </div>
    </div>
</template>

<script>
    import Cookies from 'js-cookie';
    import axios from 'axios';
    import AlertBox from '../AlertBox.vue';
    import ConfirmBox from '../ConfirmBox.vue';

    export default {
        components: {
            AlertBox,
            ConfirmBox
        },
        data() {
            return {
                merchantId: '',
                apps: [],
                currentPage: 1,
                totalPages: 1,
                searchQuery: '',
                searchName: '',
                searchCategory: '',
                searchState: '',
                searchVersion: '',
                showAdvancedSearch: false,
                sortBy: 'appId', // 默认按应用ID排序
                sortOrder: 'asc', // 默认升序
                showEditModal: false,
                showConfirmDelete: false,
                selectedApp: null,
                originalAppData: null, // 用于保存原始数据的副本
                alert: '',
                confirm: '',
                selectedImageFile: null,
                selectedAppFile: null,
                newImageUrl: null
            };
        },
        computed: {
            discountedPrice() {
                return (this.selectedApp.price * this.selectedApp.discount).toFixed(2);
                // 折后价格规约到两位小数
            },
            isModified() {
                return JSON.stringify(this.selectedApp) !== JSON.stringify(this.originalAppData);
            },
            translatedState() {
                // 状态映射
                const stateMapping = {
                    'banned': '已下架',
                    'released': '已发布',
                    'test': '待审核'
                };
                // 根据 selectedApp.releaseState 映射对应的中文状态
                return stateMapping[this.selectedApp.releaseState] || this.selectedApp.releaseState;
            }
        },
        methods: {
             async fetchApps(page = 1) {
                const token = Cookies.get('token');
                const searchMapping = {
                    '已下架': 'banned',
                    '已发布': 'released',
                    '待审核': 'test'
                };

                let formData = new FormData();
                formData.append('token', token);
                formData.append('search', searchMapping[this.searchQuery] || this.searchQuery);
                formData.append('name', this.searchName);
                formData.append('category', this.searchCategory);
                formData.append('state', searchMapping[this.searchState] || this.searchState);
                formData.append('version', this.searchVersion);
                formData.append('page', page);
                formData.append('sortBy', this.sortBy);
                formData.append('sortOrder', this.sortOrder);

                try {
                    const response = await axios.post(`${this.$Url}/api/merchant/getApps`, formData);
                    this.apps = response.data.apps.$values.map(app => ({
                        ...app,
                        discount: parseFloat(app.discount).toFixed(2)
                    }));
                    this.totalPages = response.data.totalPages;
                    this.currentPage = page;
                    this.merchantId = response.data.merchantId;
                } catch (error) {
                    console.error('Error fetching apps:', error);
                    this.alertNotification('获取应用失败，请稍后重试！');
                }
            },
            async saveAppChanges() {
                try {
                    // 如果上传了新图标，上传新图标
                    if (this.selectedImageFile) {
                        await this.uploadNewImage();
                    }

                    // 如果上传了新应用包，上传新应用包
                    if (this.selectedAppFile) {
                        await this.uploadNewAppFile();
                    }

                    await this.updateApp(this.selectedApp); // 更新应用信息
                    this.fetchApps(this.currentPage);
                    this.closeEditModal();
                } catch (error) {
                    console.error('Error saving app changes:', error);
                    this.alertNotification('保存失败，请稍后重试！');
                }
            },
            async updateApp(app) {
                if (!app.name || !app.version) {
                    this.alertNotification('应用名称和版本号不能为空！');
                    return;
                }

                let formData = new FormData();
                formData.append('merchantId', this.merchantId);
                formData.append('appId', this.selectedApp.id);
                formData.append('version', this.selectedApp.version);
                formData.append('state', this.selectedApp.releaseState);
                formData.append('description', this.selectedApp.description);
                formData.append('price', this.selectedApp.price);
                formData.append('discount', this.selectedApp.discount);

                try {
                    await axios.post(`${this.$Url}/api/merchant/updateApp`, formData);
                    this.confirmNotification('应用信息修改成功！');
                    this.fetchApps(this.currentPage);
                } catch (error) {
                    if (error.response && error.response.status === 400) {
                        this.alertNotification(error.response.data);
                    } else {
                        console.error('Error updating app:', error);
                        this.alertNotification('保存失败，请稍后重试！');
                    }
                }
            },
            async deleteApp() {
                let formData = new FormData();
                formData.append('appId', this.selectedApp.id);
                formData.append('merchantId', this.merchantId);

                try {
                    await axios.post(`${this.$Url}/api/merchant/deleteApp`, formData);
                    this.confirmNotification('应用下架成功！');
                    this.closeConfirmDelete();
                    this.closeEditModal();
                    this.fetchApps(this.currentPage);
                } catch (error) {
                    console.error('Error deleting app:', error);
                    this.alertNotification('下架应用失败，请稍后重试！');
                }
            },
            changeSort(column) {
                this.sortOrder = this.sortBy === column ? (this.sortOrder === 'asc' ? 'desc' : 'asc') : 'asc'; // 默认升序
                this.sortBy = column;
                this.currentPage = 1;
                this.initiateSearch();
            },
            async uploadNewImage() {
                let formDataImg = new FormData();
                formDataImg.append('file', this.selectedImageFile);
                formDataImg.append('id', this.selectedApp.id);

                try {
                    await axios.post(`${this.$Url}/api/merchant/upload-app-img`, formDataImg);
                    /*this.confirmNotification('图标上传成功！');*/
                } catch (error) {
                    console.error('Error uploading image:', error);
                    this.alertNotification('图标上传失败，请稍后重试！');
                }
            },
            async uploadNewAppFile() {
                let formDataAppFile = new FormData();
                formDataAppFile.append('file', this.selectedAppFile);
                formDataAppFile.append('id', this.selectedApp.id);

                try {
                    await axios.post(`${this.$Url}/api/merchant/uploadapp`, formDataAppFile);
                    /*this.confirmNotification('应用包上传成功！');*/
                } catch (error) {
                    console.error('Error uploading app file:', error);
                    this.alertNotification('应用包上传失败，请稍后重试！');
                }
            },
            handleNewImageUpload(event) {
                const file = event.target.files[0]; // 获取上传的图片文件
                const validTypes = ['image/png', 'image/jpeg']; // 定义允许的文件类型

                if (file && !validTypes.includes(file.type)) {
                    this.alertNotification('文件类型不为.png/.jpg，请稍后重试！');
                    this.selectedImageFile = null; // 清空选中的文件
                    event.target.value = ''; // 清空输入框
                    return;
                }

                this.selectedImageFile = file;
                this.newImageUrl = URL.createObjectURL(this.selectedImageFile); // 生成新图片的预览 URL
            },
            handleNewFileUpload(event) {
                const file = event.target.files[0]; // 获取上传的应用包文件

                // 判断文件是否为 .exe 类型
                if (file && file.name.split('.').pop().toLowerCase() !== 'exe') {
                    this.alertNotification('文件类型不为.exe，请稍后重试！');
                    event.target.value = ''; // 清空文件输入框，允许重新选择
                    return;
                }

                this.selectedAppFile = file;
                this.selectedApp.appFile = file.name;
            },
            initiateSearch() {
                this.currentPage = 1;
                this.fetchApps();
            },
            fetchPage(page) {
                if (page > 0 && page <= this.totalPages) {
                    this.fetchApps(page);
                }
            },
            prevPage() {
                if (this.currentPage > 1) {
                    this.fetchApps(this.currentPage - 1);
                }
            },
            nextPage() {
                if (this.currentPage < this.totalPages) {
                    this.fetchApps(this.currentPage + 1);
                }
            },
            toggleAdvancedSearch() {
                this.showAdvancedSearch = !this.showAdvancedSearch;
            },
            confirmDelete() {
                this.showConfirmDelete = true;
            },
            closeConfirmDelete() {
                this.showConfirmDelete = false;
            },
            openEditModal(app) {
                this.selectedApp = { ...app }; // 复制应用数据以便修改
                this.originalAppData = { ...app }; // 保存原始数据的副本

                // 清空之前上传但未保存的图片数据
                this.newImageUrl = null;
                this.selectedImageFile = null;

                this.showEditModal = true;
            },
            closeEditModal() {
                this.showEditModal = false;
            },
            refreshPage() {
                this.currentPage = 1;
                this.searchQuery = '';
                this.searchName = '';
                this.searchCategory = '';
                this.searchState = '';
                this.searchVersion = '';
                this.showAdvancedSearch = false;
                this.fetchApps();
            },
            alertNotification(message) {
                this.alert = '';
                this.$nextTick(() => {
                    this.alert = message;
                });
            },
            confirmNotification(message) {
                this.confirm = '';
                this.$nextTick(() => {
                    this.confirm = message;
                });
            },
            getStateClass(releaseState) {
                const classes = {
                    'banned': 'status-banned',
                    'released': 'status-released',
                    'test': 'status-test'
                };
                return classes[releaseState] || '';
            },
            getStateInChinese(releaseState) {
                const stateMapping = {
                    'banned': '已下架',
                    'released': '已发布',
                    'test': '待审核'
                };
                return stateMapping[releaseState] || releaseState;
            },
            getFullImageUrl(imagePath) {
                const baseUrl = `${this.$Url}`;
                return imagePath ? `${baseUrl}${imagePath}` : '';
            },
        },
        mounted() {
            this.fetchApps();
        },
    };
</script>

<style scoped>
    .manage-app {
        display: flex;
        flex-direction: column;
        background-color: #f0f9ff;
        padding: 20px;
        border-radius: 10px;
        box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
        max-width: 1200px;
        margin: 0 auto;
        font-family: 'Baloo 2', cursive, Arial, sans-serif;
        font-size: 16px;
        color: #333;
        min-height: 85vh;
    }

    h1 {
        color: #1976d2;
    }

    .search-bar {
        display: flex;
        flex-wrap: wrap;
        gap: 5px;
        margin-bottom: 10px;
    }

    .global-search,
    .search-button,
    .advanced-search-toggle {
        flex: 1 1 150px;
        padding: 8px;
        border: 1px solid #ccc;
        border-radius: 5px;
        background-color: #fff;
        transition: border-color 0.3s, box-shadow 0.3s;
    }

    .search-button,
    .advanced-search-toggle {
        cursor: pointer;
        background-color: #1e88e5;
        color: #fff;
    }

        .advanced-search-toggle:hover,
        .search-button:hover,
        .refresh-button:hover {
            background-color: #1565c0;
        }

    .refresh-button {
        margin-left: 10px;
        padding: 5px 10px;
        background-color: #1e88e5;
        color: white;
        border: none;
        border-radius: 4px;
        cursor: pointer;
    }

    .advanced-search {
        width: 100%;
        display: flex;
        flex-wrap: wrap;
        gap: 5px;
        margin-top: 5px;
    }

        .advanced-search input {
            flex: 1 1 150px;
        }

    .app-list-container {
        flex: 1;
        width: 100%;
        overflow-x: auto;
        max-height: 65vh;
        overflow-y: scroll;
    }

    .app-list table {
        width: 100%;
        border-collapse: collapse;
        table-layout: fixed;
        word-wrap: break-word;
        max-height: 100%;
        background-color: #fff;
    }

    th {
        padding: 8px;
        text-align: center;
        border: 1px solid #ddd;
        background-color: #1e88e5;
        color: #fff;
        position: sticky;
        top: 0;
    }

    td {
        padding: 8px;
        text-align: center;
        border: 1px solid #ddd;
        background-color: #fff;
        color: #000;
    }

        td .app-image {
            width: 50px;
            height: 50px;
            object-fit: cover;
            margin-bottom: 5px;
        }

        td input[type="file"] {
            display: block;
            margin-top: 5px;
        }

    .sortable {
        cursor: pointer;
    }

    .sort-icons {
        margin-left: 5px;
    }

    .arrow-up,
    .arrow-down {
        font-size: 12px;
    }

    .pagination {
        display: flex;
        justify-content: center;
        gap: 10px;
        padding: 10px 0;
    }

        .pagination button {
            padding: 5px 10px;
            background-color: #1e88e5;
            color: #fff;
            border: none;
            border-radius: 5px;
            cursor: pointer;
            transition: background-color 0.3s, transform 0.3s, color 0.3s;
            font-family: 'Baloo 2', cursive;
            font-size: 14px;
        }

            .pagination button:disabled {
                cursor: not-allowed;
                background-color: #ccc;
            }

            .pagination button:hover:enabled {
                background-color: #1565c0;
                transform: scale(1.05);
            }

        .pagination span {
            display: flex;
            align-items: center;
        }

    .modal {
        display: flex;
        justify-content: center;
        align-items: center;
        position: fixed;
        top: 0;
        left: 0;
        width: 100%;
        height: 100%;
        background-color: rgba(0, 0, 0, 0.5);
        z-index: 1000;
    }

    .modal-content {
        background-color: #f0f9ff;
        padding: 20px 20px 20px 20px;
        border-radius: 10px;
        box-shadow: 0 4px 8px rgba(0, 0, 0, 0.2);
        width: 600px;
        max-height: 80vh;
        overflow-y: auto;
        position: relative;
        font-family: 'Baloo 2', cursive, Arial, sans-serif;
        color: #333;
    }

    h2 {
        color: #1976d2;
        text-align: center;
        margin-bottom: 20px;
    }

    .input-container {
        position: relative;
        display: flex;
        align-items: center;
    }

    .currency-symbol {
        position: absolute;
        left: 10px;
        color: #000;
        font-size: 14px;
        pointer-events: none;
    }

    .price-input {
        width: 100%;
        padding: 12px 8px;
        padding-left: 30px; /* 额外留出空间以避免与“￥”符号重叠 */
        border: 1px solid #1e88e5;
        border-radius: 5px;
        font-size: 14px;
        font-family: 'Baloo 2', cursive, Arial, sans-serif;
        transition: border-color 0.3s, box-shadow 0.3s;
        width: 100%;
    }

        .price-input:focus {
            border-color: #1565c0;
            box-shadow: 0 0 5px rgba(21, 101, 192, 0.5);
        }

    .form-group {
        margin-bottom: 10px;
        position: relative;
    }
        .form-group label {
            display: block;
            margin-bottom: 5px;
            color: #1976d2;
            font-weight: bold;
        }

        .form-group input[type="text"],
        .form-group input[type="number"],
        .form-group textarea,
        .form-group select {
            width: 100%;
            padding: 12px 8px;
            border: 1px solid #1e88e5;
            border-radius: 5px;
            font-size: 14px;
            font-family: 'Baloo 2', cursive, Arial, sans-serif;
            transition: border-color 0.3s, box-shadow 0.3s;
        }

        .form-group textarea {
            resize: none;
        }

        .form-group .price-input {
            padding-left: 30px;
        }

    .form-group-row {
        display: flex;
        justify-content: space-between;
        margin-bottom: 10px;
    }

        .form-group-row .form-group {
            flex: 0 0 48%;
        }

    .image-preview {
        display: flex;
        justify-content: center;
        margin-bottom: 10px;
    }

        .image-preview img {
            width: 80px;
            height: 80px;
            object-fit: cover;
            border-radius: 5px;
            border: 1px solid #ccc;
        }

    .button-group {
        display: flex;
        justify-content: space-between;
        margin-top: 20px;
    }

    .close {
        position: sticky;
        top: 10px;
        left: 530px;
        font-size: 20px;
        cursor: pointer;
        color: #1976d2;
        z-index: 1001;
        margin: 10px;
        border-radius: 50%;
        width: 30px;
        height: 30px;
        display: flex;
        justify-content: center;
        align-items: center;
        transition: transform 0.2s ease-in-out;
    }

        .close:hover {
            transform: rotate(180deg); 
        }

    .save-button {
        background-color: #5cb85c;
        color: #fff;
        padding: 10px 20px;
        border: none;
        border-radius: 5px;
        cursor: pointer;
        transition: background-color 0.3s ease;
        flex: 1;
        margin-right: 10px;
        text-align: center;
        font-family: 'Baloo 2', cursive, Arial, sans-serif;
        font-size: 14px;
    }

        .save-button:hover {
            background-color: #4cae4c;
        }

        .save-button:disabled {
            background-color: #cccccc;
            cursor: not-allowed;
        }

    .delete-button {
        background-color: #dc3545;
        color: #fff;
        padding: 10px 20px;
        border: none;
        border-radius: 5px;
        cursor: pointer;
        transition: background-color 0.3s ease;
        flex: 1;
        margin-left: 10px;
        text-align: center;
        font-family: 'Baloo 2', cursive, Arial, sans-serif;
        font-size: 14px;
    }

        .delete-button:hover {
            background-color: #c82333;
        }

    .confirm-yes-button {
        background-color: #dc3545;
        color: #fff;
        padding: 12px 24px;
        border: none;
        border-radius: 5px;
        cursor: pointer;
        transition: background-color 0.3s ease;
        font-family: 'Baloo 2', cursive, Arial, sans-serif;
        font-size: 16px;
    }

        .confirm-yes-button:hover {
            background-color: #c82333;
        }

    .confirm-no-button {
        background-color: #6c757d;
        color: #fff;
        padding: 12px 24px;
        border: none;
        border-radius: 5px;
        cursor: pointer;
        transition: background-color 0.3s ease;
        font-family: 'Baloo 2', cursive, Arial, sans-serif;
        font-size: 16px;
    }

        .confirm-no-button:hover {
            background-color: #5a6268;
        }

    .status-banned {
        color: #e32636;
        font-weight: bold;
    }

    .status-released {
        color: #40bb45;
        font-weight: bold
    }

    .status-test {
        color: #ffa500;
        font-weight: bold
    }

    @media (max-width: 500px) {
        .modal-content {
            width: 90%; /* 适应小屏幕 */
        }

        .button-group {
            flex-direction: column;
        }

        .save-button, .delete-button {
            margin: 5px 0;
        }
    }
</style>
