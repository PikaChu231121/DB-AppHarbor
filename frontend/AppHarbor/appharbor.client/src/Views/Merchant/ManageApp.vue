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
            <button @click="refreshPage" class="refresh-button">全部应用</button>
            <div v-if="showAdvancedSearch" class="advanced-search">
                <input v-model="searchName" placeholder="应用名称搜索..." />
                <input v-model="searchCategory" placeholder="应用种类搜索..." />
                <input v-model="searchVersion" placeholder="版本搜索..." />
                <input v-model="searchState" placeholder="状态搜索..." />
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
                    <div>
                        <label>应用ID: {{ selectedApp.id }}</label>
                    </div>
                    <div>
                        <label>名称: {{ selectedApp.name }}</label>
                    </div>
                    <div>
                        <label>分类: {{ selectedApp.category }}</label>
                    </div>
                    <div>
                        <label>下载量: {{ selectedApp.downloadCount }}</label>
                    </div>
                    <div>
                        <label>状态: {{ selectedApp.releaseState }}</label>
                    </div>
                    <div>
                        <label>版本:</label>
                        <input v-model="selectedApp.version" />
                    </div>
                    <div>
                        <label style="vertical-align: top;">描述:</label>
                        <textarea v-model="selectedApp.description" rows="4"
                                  style="resize: none; width: 80%;"></textarea>
                    </div>
                    <div>
                        <label>当前图标:<br /></label>
                        <div style="display: flex; justify-content: center;">
                            <img :src="getFullImageUrl(selectedApp.image)" alt="应用图标" style="width: 100px; height: 100px;" />
                        </div>
                    </div>
                    <div>
                        <label>更新图标:</label>
                        <input type="file" @change="handleNewImageUpload" />
                    </div>
                    <div>
                        <label>更新应用包:</label>
                        <input type="file" @change="handleNewFileUpload" />
                    </div>
                    <div>
                        <label>原价:</label>
                        <input v-model="selectedApp.price"
                               @input="selectedApp.price == 0 ? selectedApp.discount = '1.00' : null" />
                    </div>
                    <div>
                        <label>折扣:</label>
                        <select v-model="selectedApp.discount" :disabled="selectedApp.price == 0"
                                :title="selectedApp.price == 0 ? '免费应用不能设置折扣' : ''">
                            <option value="1.00">无折扣</option>
                            <option value="0.90">9折</option>
                            <option value="0.80">8折</option>
                            <option value="0.75">75折</option>
                            <option value="0.70">7折</option>
                            <option value="0.60">6折</option>
                            <option value="0.50">5折</option>
                        </select>
                    </div>
                    <div>
                        <span v-if="selectedApp.discount !== '1.00'" style="margin-left: 10px;">
                            折后价: ￥{{ discountedPrice}}
                        </span>
                    </div>
                    <div>
                        <button @click="saveAppChanges" class="save-button" :disabled="!isModified">保存</button>
                        <button @click="confirmDelete" class="delete-button">删除应用</button>
                    </div>

                    <div v-if="showConfirmDelete" class="modal">
                        <div class="modal-content">
                            <p>确定要删除这个应用吗？</p>
                            <button @click="deleteApp()" class="confirm-button">是</button>
                            <button @click="closeConfirmDelete" class="cancel-button">否</button>
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
                selectedAppFile: null
            };
        },
        computed: {
            discountedPrice() {
                return (this.selectedApp.price * this.selectedApp.discount).toFixed(2);
                // 折后价格规约到两位小数
            }
        },
        methods: {
            fetchApps(page = 1) {
                const token = Cookies.get('token');
                let formData = new FormData();
                formData.append('token', token);
                formData.append('search', this.searchQuery);
                formData.append('name', this.searchName);
                formData.append('category', this.searchCategory);
                formData.append('state', this.searchState);
                formData.append('version', this.searchVersion);
                formData.append('page', page);
                formData.append('sortBy', this.sortBy);
                formData.append('sortOrder', this.sortOrder);

                axios.post('http://localhost:5118/api/merchant/getApps', formData)
                    .then(response => {
                        this.apps = response.data.apps.$values.map(app => {
                            // 格式化 discount 值为带有两位小数的形式
                            app.discount = parseFloat(app.discount).toFixed(2);
                            return app;
                        });
                        this.totalPages = response.data.totalPages;
                        this.currentPage = page;
                        this.merchantId = response.data.merchantId;
                    })
                    .catch(error => {
                        console.error('Error fetching apps:', error);
                        this.alertNotification('获取应用失败，请稍后重试！');
                    });
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
                axios.post('http://localhost:5118/api/merchant/updateApp', formData)
                    .then(() => {
                        this.confirmNotification('应用信息修改成功！');
                        this.fetchApps(this.currentPage); // 刷新应用列表
                    })
                    .catch(error => {
                        console.error('Error updating app:', error);
                        this.alertNotification('更新应用失败，请稍后重试！');
                        return;
                    });
            },
            deleteApp() {
                let formData = new FormData();
                formData.append('appId', this.selectedApp.id);
                formData.append('merchantId', this.merchantId);

                axios.post('http://localhost:5118/api/merchant/deleteApp', formData)
                    .then(() => {
                        this.confirmNotification('应用删除成功！');
                        this.closeConfirmDelete();
                        this.closeEditModal();
                        this.fetchApps(this.currentPage);
                    })
                    .catch(error => {
                        console.error('Error deleting app:', error);
                        this.alertNotification('删除应用失败，请稍后重试！');
                        return;
                    });
            },
            changeSort(column) {
                if (this.sortBy === column) {
                    this.sortOrder = this.sortOrder === 'asc' ? 'desc' : 'asc';
                }
                else {
                    this.sortBy = column;
                    this.sortOrder = 'asc'; // 默认升序
                }
                this.currentPage = 1;
                this.initiateSearch();
            },
            async uploadNewImage() {
                let formDataImg = new FormData();
                formDataImg.append('file', this.selectedImageFile);
                formDataImg.append('id', this.selectedApp.id);

                try {
                    await axios.post('http://localhost:5118/api/Image/upload-app-img', formDataImg);
                    this.confirmNotification('图标上传成功！');
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
                    await axios.post('http://localhost:5118/api/application/uploadapp', formDataAppFile);
                    this.confirmNotification('应用包上传成功！');
                } catch (error) {
                    console.error('Error uploading app file:', error);
                    this.alertNotification('应用包上传失败，请稍后重试！');
                }
            },
            handleNewImageUpload(event) {
                this.selectedImageFile = event.target.files[0]; // 获取上传的图标文件
            },
            handleNewFileUpload(event) {
                this.selectedAppFile = event.target.files[0]; // 获取上传的应用包文件
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
                if (releaseState === 'banned') {
                    return 'status-banned';
                } else if (releaseState === 'released') {
                    return 'status-released';
                } else if (releaseState === 'test') {
                    return 'status-test';
                }
                return '';
            },
            getStateInChinese(releaseState) {
                switch (releaseState) {
                    case 'banned':
                        return '已删除';
                    case 'released':
                        return '已发布';
                    case 'test':
                        return '待审核';
                    default:
                        return releaseState;
                }
            },
            getFullImageUrl(imagePath) {
                const baseUrl = 'http://localhost:5118';
                return imagePath ? `${baseUrl}${imagePath}` : '';
            },
        },
        mounted() {
            this.fetchApps();
        },
        computed: {
            isModified() {
                return JSON.stringify(this.selectedApp) !== JSON.stringify(this.originalAppData);
            }
        }
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
        .refresh-button:hover{
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
        background-color: #1e88e5; /* 表头背景颜色为蓝色 */
        color: #fff; /* 表头字体颜色为白色 */
        position: sticky;
        top: 0;
    }

    td {
        padding: 8px;
        text-align: center;
        border: 1px solid #ddd;
        background-color: #fff; /* 表格内容背景颜色为白色 */
        color: #000; /* 表格内容字体颜色为黑色 */
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

    @media (max-width: 768px) {

        .advanced-search-toggle,
        .search-button,
        .global-search {
            width: 100%;
        }

        .pagination {
            flex-wrap: wrap;
        }
    }

    /* 模态框背景 */
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

    /* 模态框内容 */
    .modal-content {
        background-color: #fff;
        padding: 20px;
        border-radius: 10px;
        box-shadow: 0 4px 8px rgba(0, 0, 0, 0.2);
        width: 300px;
        position: relative;
    }

    /* 关闭按钮 */
    .close {
        position: absolute;
        top: 10px;
        right: 10px;
        font-size: 20px;
        cursor: pointer;
    }

    .save-button {
        background-color: #5cb85c; /*#28a745*/
        color: #fff;
        padding: 10px 20px;
        border: none;
        border-radius: 5px;
        cursor: pointer;
        transition: background-color 0.3s ease;
        margin-right: 25px;
        margin-top: 20px;
        margin-left: 20px;
    }

        .save-button:hover {
            background-color: #4cae4c;
/*            #218838;*/
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
        margin-left: 10px;
        margin-right: 20px; /* 不靠近右边界 */
    }

        .delete-button:hover {
            background-color: #c82333;
        }

    .confirm-button {
        background-color: #dc3545;
        color: #fff;
        padding: 8px 16px;
        border: none;
        border-radius: 5px;
        cursor: pointer;
        transition: background-color 0.3s ease;
        margin-top: 10px;
        margin-left: 5px;
        margin-right: 55px;
    }

        .confirm-button:hover {
            background-color: #c82333;
        }

    .cancel-button {
        background-color: #6c757d;
        color: #fff;
        padding: 8px 16px;
        border: none;
        border-radius: 5px;
        cursor: pointer;
        transition: background-color 0.3s ease;
    }

        .cancel-button:hover {
            background-color: #5a6268;
        }

    .status-banned {
        color: #e32636;
        font-weight:bold;
    }

    .status-released {
        color: #40bb45;
        font-weight: bold
    }

    .status-test {
        color: #ffa500;
        font-weight: bold
    }
</style>
