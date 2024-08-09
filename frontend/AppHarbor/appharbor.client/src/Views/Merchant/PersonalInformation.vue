<template>
    <div class="profile-settings">
        <h1>商家信息</h1>
        <div class="user-info">
            <div class="avatar-group">
                <div class="avatar-edit">
                    <img :src="user.merchantAvatar" alt="商家头像" 
                    class="avatar" />
                    <div class="edit-icon">
                        <img src="../../../public/editing.png" @click="triggerFileInput" />
                    </div>
                    <input type="file" ref="fileInput" @change="onFileChange" class="file-input" />
                </div>
            </div>
            <div class="form-group">
                <label>商家ID</label>
                <p class="merchant">{{ user.merchantId }}</p>
            </div>
            <div class="form-group">
                <label>商家昵称</label>
                <div class="nickname-edit">
                    <input type="text"
                           v-model="user.merchantNickname"
                           @input="enableSaveButton"
                           class="nickname-input" />
                    <button :disabled="!isSaveEnabled" @click="save">保存修改</button>
                </div>
            </div>
            <div class="form-group">
                <label>商家注册时间</label>
                <p class="merchant">{{ formattedRegisterTime }}</p>
            </div>
        </div>
    </div>
    <div>
        <NotificationPopup :show="showPopup"
                           :message="popupMessage"
                           @update:show="showPopup = $event" />
    </div>
</template>

<script>
    import axios from 'axios';
    import Cookies from 'js-cookie';
    import NotificationPopup from '../AdminPage/NotificationPopup.vue';

    export default {
        name: 'ProfileSettings',
        components: {
            NotificationPopup
        },
        data() {
            return {
                user: {
                    merchantId: '',
                    merchantAvatar: '',
                    merchantNickname: '',
                    registerTime: ''
                },
                isSaveEnabled: false,
                showPopup: false,
                popupMessage: '',
            }
        },
        mounted() {
            this.fetchUserInfo();
            this.clearPopupStatus();
        },
        methods: {
            fetchUserInfo() {
                const token = Cookies.get('token');
                axios.post('http://localhost:5118/api/merchant/merchantInfo', {token: token})
                    .then(response => {
                        const data = response.data;
                        this.user.merchantId = data.id;
                        this.user.merchantNickname = data.nickName;
                        this.user.merchantAvatar = data.avatar ? `http://localhost:5118${data.avatar}` : '@/../public/default.png';
                        this.user.registerTime = data.registerTime;
                    })
                    .catch(error => {
                        console.error('Error fetching merchant data:', error);
                    });
            },
            enableSaveButton() {
                this.isSaveEnabled = true;
            },
            triggerFileInput() {
                this.$refs.fileInput.click();
            },
            onFileChange(event) {
                const file = event.target.files[0];
                if (file) {
                    let formData = new FormData();
                    formData.append('file', file);
                    formData.append('id', this.user.merchantId);

                    const token = Cookies.get('token');
                    axios.post('http://localhost:5118/api/Image/upload-merchant-image', formData, {
                        headers: {
                            'Authorization': `Bearer ${token}`,
                            'Content-Type': 'multipart/form-data'
                        }
                    })
                        .then(() => {
                            this.popupMessage = '头像上传成功';
                            this.showPopup = true;
                            // Only set popupStatusChecked to true after setting localStorage
                            setTimeout(() => {
                                this.showPopup = false;
                                this.popupMessage = '';
                                window.location.reload();
                            }, 2000);
                        })
                        .catch(error => {
                            console.error('Error uploading avatar:', error);
                            this.popupMessage = '头像上传失败';
                            this.showPopup = true;
                            // Only set popupStatusChecked to true after setting localStorage
                            setTimeout(() => {
                                this.showPopup = false;
                                this.popupMessage = '';
                                window.location.reload();
                            }, 2000);
                        });
                }
            },
            save() {
                if (this.user.merchantNickname === "") {
                    this.popupMessage = "昵称不允许为空，请重新输入";
                    this.showPopup = true;
                    return;
                }
                let formData = new FormData();
                formData.append('id', this.user.merchantId);
                formData.append('nickName', this.user.merchantNickname);
                axios.post('http://localhost:5118/api/merchant/updateMerchantNickname', formData)
                    .then(() => {
                        this.popupMessage = '昵称修改成功';
                        this.showPopup = true;
                        this.isSaveEnabled = false;
                        // Only set popupStatusChecked to true after setting localStorage
                        setTimeout(() => {
                            this.showPopup = false;
                            this.popupMessage = '';
                            window.location.reload();
                        }, 2000);
                    })
                    .catch(error => {
                        console.error('Error updating nickname:', error);
                        this.popupMessage = '更新昵称失败，请重试';
                        this.showPopup = true;
                        return;
                    });
            },
            clearPopupStatus() {
                // Clear any popup status from localStorage
                localStorage.removeItem('popupStatus');
            }
        },
        computed: {
            formattedRegisterTime() {
                if (this.user.registerTime) {
                    return this.user.registerTime.replace('T', ' ');
                }
                return '';
            }
        }
    }
</script>


<style scoped>
    .profile-settings {
        max-width: 500px;
        min-height: 90px;
        margin: 0 auto;
        padding: 32px;
        background-color: #fff;
        border-radius: 8px;
        box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
    }

    h1 {
        margin-bottom: 20px;
        font-size: 30px;
        color: #333;
        text-align: center;
        border: 3px solid #1e88e5;
        border-radius: 8px;
    }

    .user-info {
        display: flex;
        flex-direction: column;
        gap: 20px;
    }

    .form-group {
        display: flex;
        flex-direction: column;
        color: black;
    }

    .avatar-group {
        text-align: center;
        max-width: 240px;
        padding: 15px;
        border: 1px solid #1e88e5;
        border-radius: 8px;
        box-shadow: 0 10px 10px rgba(0, 0, 0, 0.1);
    }

    label {
        margin-bottom: 5px;
        font-weight: bold;
        font-size: 20px;
        color: #333;
    }

    input[type="text"], p {
        padding: 10px;
        border: 2px solid #1e88e5;
        border-radius: 4px;
        font-size: 16px;
    }

    .nickname-edit {
        display: flex;
        gap: 10px;
    }

    .nickname-input:disabled {
        background-color: #f9f9f9;
    }

    .avatar-edit {
        display: flex;
        align-items: center;
        gap: 10px;
        position: relative; /* 使阴影效果相对于父元素 */
    }

    .avatar {
        width: 160px;
        height: 160px;
        border-radius: 50%;
        object-fit: cover;
    }

    .file-input {
        display: none;
    }

    button {
        padding: 10px 20px;
        background-color: #42a5f5;
        font-size: 15px;
        color: #fff;
        border: 3px solid #1e88e5;
        border-radius: 10px;
        cursor: pointer;
        transition: background-color 0.3s, transform 0.3s, color 0.3s;
    }

        button:disabled {
            cursor: not-allowed;
        }

        button:hover:enabled {
            background-color: #1e88e5;
            transform: scale(1.05);
            color: #fff;
            transition: background-color 0.3s, transform 0.3s, color 0.3s;
        }

    .edit-icon {
        cursor: pointer;
        display: flex;
        align-items: center;
    }

        .edit-icon img {
            width: 24px; /* 调整图标大小 */
            height: 24px; /* 调整图标大小 */
            transition: box-shadow 0.3s ease, filter 0.3s ease;
        }

        .edit-icon:hover img {
            box-shadow: 0 1px 1px rgba(0, 0, 0, 0.3);
            filter: brightness(1.1);
        }

    .merchant-id {
        color: black;
    }
</style>
