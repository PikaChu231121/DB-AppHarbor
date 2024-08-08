<template>
    <div class="profile-settings">
        <h1>管理员信息</h1>
        <div class="user-info">
            <div class="avatar-group">
                <div class="avatar-edit">
                    <img :src="user.adminAvatar" alt="管理员头像" class="avatar" />
                    <div class="edit-icon">
                        <img src="../../../public/editing.png" @click="triggerFileInput" />
                    </div>
                    <input type="file" ref="fileInput" @change="onFileChange" class="file-input" />
                </div>
            </div>
            <div class="form-group">
                <label>管理员ID</label>
                <p class="admin">{{ user.adminId }}</p>
            </div>
            <div class="form-group">
                <label>管理员昵称</label>
                <div class="nickname-edit">
                    <input type="text"
                           v-model="user.adminNickname"
                           @input="enableSaveButton"
                           class="nickname-input" />
                    <button :disabled="!isSaveEnabled" @click="save">保存修改</button>
                </div>
            </div>
            <div class="form-group">
                <label>管理员注册时间</label>
                <p class="admin">{{ user.registerTime }}</p>
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
    import NotificationPopup from './NotificationPopup.vue';

    export default {
        name: 'ProfileSettings',
        components: {
            NotificationPopup
        },
        data() {
            return {
                user: {
                    adminId: '',
                    adminAvatar: '',
                    adminNickname: '',
                    registerTime: ''
                },
                isSaveEnabled: false,
                alertMessage: '',
                showPopup: false,
                popupMessage: '',
            }
        },
        mounted() {
            this.fetchUserInfo();
        },
        methods: {
            fetchUserInfo() {
                var token = Cookies.get('token');
                let formData = new FormData();
                formData.append('token', token);
                axios.post('http://localhost:5118/api/admin/adminInfo', formData)
                    .then(response => {
                        const data = response.data;
                        this.user.adminId = data.id;
                        this.user.adminNickname = data.nickname;
                        this.user.adminAvatar = data.avatar ? `http://localhost:5118${data.avatar}` : '@/../public/default.png'; // avatar 判空
                        this.user.registerTime = data.registerTime;
                        console.log(this.user.adminId);
                    })
                    .catch(error => {
                        console.error('Error fetching admin data:', error);
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
                    formData.append('id', this.user.id);
                    for (let pair of formData.entries()) {
                        console.log(`${pair[0]}: ${pair[1]}`);
                    };
                    var token = Cookies.get('token');
                    axios.post('http://localhost:5118/api/Image/upload-personal-image', formData)
                        .then(response => {
                            this.user.avatar = response.data.data;
                            //console.log(this.user.avatar);
                            this.showAlert('头像上传成功');
                        })
                        .catch(error => {
                            console.error('Error uploading avatar:', error);
                            this.showAlert('头像上传失败');
                        });
                }
            },
            showAlert(message) {

            },
            save() {
                var token = Cookies.get('token');
                if (this.user.adminNickname == "") {
                    this.popupMessage = "昵称不允许为空，请重新输入";
                    this.showPopup = true;
                    return;
                }
                axios.post('http://localhost:5118/api/admin/updateAdminNickname', {
                    id: this.user.adminId,
                    newnickname: this.user.adminNickname
                })
                    .then(response => {
                        console.log('User nickname updated successfully');
                        this.popupMessage = '昵称修改成功';
                        this.showPopup = true;
                        this.isSaveEnabled = false;
                    })
                    .catch(error => {
                        console.error('Error updating user nickname:', error);
                        this.popupMessage = '更新昵称失败，请重试';
                        this.showPopup = true;
                    });
            },

            getAvatarUrl(avatarPath) {
                if (avatarPath) {
                    return `http://localhost:5118${avatarPath}`;
                }
                return '../../public/default.png'; // 默认头像路径
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
    };
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
        border: 3px solid #6a1b9a;
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
        border: 1px solid #6a1b9a;
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
        border: 2px solid #6a1b9a;
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
        background-color: #d7a6f6;
        font-size: 15px;
        color: #6a1b9a;
        border: 3px solid #6a1b9a;
        border-radius: 10px;
        cursor: pointer;
        transition: background-color 0.3s, transform 0.3s, color 0.3s;
    }

        button:disabled {
            cursor: not-allowed;
        }

        button:hover:enabled {
            background-color: #6a1b9a;
            transform: scale(1.05);
            color: #d7a6f6;
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

    .admin-id {
        color: black;
    }
</style>