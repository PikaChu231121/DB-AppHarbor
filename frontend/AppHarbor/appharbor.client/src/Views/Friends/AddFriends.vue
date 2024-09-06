<template>
    <div class="friend-manager">
        <Loading :loading="isLoading" />
        <!LoginAlert v-if="alertMessage" style="z-index: 1;" :message="alertMessage" @close="alertMessage = ''" />
        <!-- Alert Message Toast -->
        <div v-if="alertMessage" class="notification-toast">
            <p>{{ alertMessage }}</p>
        </div>
        <!-- Left Section -->
        <div class="left-section">
            <h2 class="main-title">管理好友</h2>
            <p class="sub-title">您的好友</p>
            <div class="friend-list">
                <div v-for="friend in friends" :key="friend.id" class="friend-item">
                    <img :src="getAvatarUrl(friend.avatar)" class="avatar" alt="Friend Avatar">
                    <div class="friend-details">
                        <p class="friend-name">{{ friend.nickname }}</p>
                        <p class="friend-group">{{ friend.group }}</p>
                    </div>
                    <button @click="openDeleteFriendDialog(friend.id)" class="remove-button">-</button>
                </div>
            </div>
        </div>

        <!-- Right Section -->
        <div class="right-section">
            <div class="search-bar">
                <input type="text" v-model="searchQuery" placeholder="输入用户ID进行搜索" class="search-input">
                <button @click="searchUsers" class="search-button">搜索</button>
            </div>
            <div class="search-results">
                <div v-if="searchResults.length === 0" class="no-results">
                    未搜索到用户
                </div>
                <div v-else>
                    <div v-for="result in searchResults" :key="result.id" class="search-result">
                        <img :src="getAvatarUrl(result.avatar)" class="avatar" alt="Search Result Avatar">
                        <div class="result-details">
                            <p class="result-name">{{ result.nickname }}</p>
                            <p class="result-group">{{ result.group }}</p>
                        </div>
                        <button @click.stop="openAddFriendDialog(result.id)" class="add-button">+</button>
                    </div>
                </div>
            </div>
        </div>

        <!-- Add Friend Popup -->
        <transition name="popup">
            <div v-if="showAddFriendPopup" class="add-friend-popup" ref="addFriendPopup" @click.stop>
                <p>请选择添加关系的种类：</p>
                <button class="popup-item family" @click.stop="confirmAddFriend('family')">
                    <span>家人</span>
                </button>
                <button class="popup-item friend" @click.stop="confirmAddFriend('friend')">
                    <span>朋友</span>
                </button>
                <button class="popup-item classmate" @click.stop="confirmAddFriend('classmate')">
                    <span>同学</span>
                </button>
                <button class="popup-item cancel-button" @click.stop="cancelAddFriend">
                    <span>取消</span>
                </button>
            </div>
        </transition>

        <!-- Delete Friend Popup -->
        <transition name="popup">
            <div v-if="showDeleteFriendPopup" class="delete-friend-popup" ref="deleteFriendPopup" @click.stop>
                <p>你确定要删除好友{{ friendToDeleteNickname }}吗？</p>
                <button class="popup-item confirm-button" @click.stop="confirmDeleteFriend">
                    <span>确定</span>
                </button>
                <button class="popup-item cancel-button" @click.stop="cancelDeleteFriend">
                    <span>取消</span>
                </button>
            </div>
        </transition>
    </div>
</template>

<script>
    import axios from 'axios';
    import Cookies from 'js-cookie';
    import LoginAlert from './AddFriendAlert.vue';
    import Loading from './Loading.vue';

    export default {
        components: {
            LoginAlert,
            Loading,
        },
        data() {
            return {
                friends: [],
                searchQuery: '',
                searchResults: [],
                showAddFriendPopup: false,
                showDeleteFriendPopup: false,
                selectedUserId: null,
                friendToDeleteNickname: '',
                alertMessage: '',
                isLoading: false
            };
        },
        methods: {
            showToast() {
                // 确保 alertMessage 能被显示
                if (this.alertMessage) {
                    setTimeout(() => {
                        this.alertMessage = ''; // 4秒后清除消息
                    }, 4000);
                }
            },
            fetchFriends() {
                var token = Cookies.get('token');
                let formData1 = new FormData();
                formData1.append('token', token);
                axios.post(`${this.$Url}/api/relationship/findall`, formData1)
                    .then(response => {
                        this.friends = response.data.data.$values;
                    })
                    .catch(error => {
                        console.error('Error fetching friends:', error);
                        // Cookies.remove('token');
                        // this.$router.push('/').then(() => {
                        //     // 刷新登录页面
                        //     window.location.reload();
                        // });
                        // alert("token异常，请重新登陆！");
                    });
            },
            searchUsers() {
                //this.isLoading = true;

                let formData = new FormData();
                let userId = Number(this.searchQuery);
                formData.append('inputId', userId);
                axios.post(`${this.$Url}/api/user/searchid`, formData)
                    .then(response => {
                        
                        if (response.data && response.data.id) {
                            this.searchResults = [response.data];
                        } else {
                    
                            this.searchResults = [];
                        }
                    })
                    .catch(error => {
                        setTimeout(() => {
                            //this.isLoading = false; // Hide loading animation

                        }, 2000); // Delay for 2 seconds
                        console.error('Error searching users:', error);
                        this.searchResults = [];
                    });
            },
            openAddFriendDialog(userId) {
                this.selectedUserId = userId;
                this.showAddFriendPopup = true;
            },
            confirmAddFriend(relationType) {
                this.addFriend(this.selectedUserId, relationType);
                this.showAddFriendPopup = false;
            },
            addFriend(userId, relationType) {
                
                var token = Cookies.get('token');
                let formData = new FormData();
                formData.append('token', token);
                formData.append('friendId', userId);
                formData.append('relationship', relationType);
                axios.post(`${this.$Url}/api/relationship/addfriend`, formData)
                    .then(() => {
                        //this.isLoading = true;
                        this.fetchFriends();
                        this.showToast(); // 确保调用显示气泡的方法
                        this.alertMessage = `添加成功`;
                    })
                    .catch(error => {
                        if (error.response.data.data == 5) {
                            //alert('好友不能是自己');
                            this.alertMessage = `好友不能是自己 `;
                            this.showToast(); // 确保调用显示气泡的方法

                        }
                        else {
                            this.alertMessage = `好友` + userId + `已经是您的好友,不需要重复添加`;
                            this.showToast(); // 确保调用显示气泡的方法
                            
                            //alert('好友' + userId + '已经是您的好友,不需要重复添加');
                            console.error('Error adding friend:', error);
                        }

                    });
            },
            openDeleteFriendDialog(userId) {
                const friend = this.friends.find(f => f.id === userId);
                if (friend) {
                    this.friendToDeleteNickname = friend.nickname;
                }
                this.selectedUserId = userId;
                this.showDeleteFriendPopup = true;
            },
            confirmDeleteFriend() {
                this.deleteFriend(this.selectedUserId);
                this.showDeleteFriendPopup = false;
            },
            deleteFriend(userId) {
                var token = Cookies.get('token');
                let formData = new FormData();
                formData.append('token', token);
                formData.append('friendid', userId);
                axios.post(`${this.$Url}/api/relationship/deletefriend`, formData)
                    .then(() => {
                        //this.isLoading = true;
                        this.friends = this.friends.filter(friend => friend.id !== userId);
                        this.alertMessage = `删除成功`;
                        this.showToast(); // 确保调用显示气泡的方法
                    })
                    .catch(error => {
                        console.error('Error removing friend:', error);
                    });
            },
            cancelDeleteFriend() {
                this.showDeleteFriendPopup = false;
            },
            getAvatarUrl(avatarPath) {
                if (avatarPath) {
                    return `${this.$Url}${avatarPath}`;
                }
                return require('../../../public/default.png');
            },
            cancelAddFriend() {
                this.showAddFriendPopup = false;
            },
            handleClickOutside(event) {
                console.log('Handling click outside');
                // 检查点击是否在添加好友弹窗内
                if (this.showAddFriendPopup && !this.$refs.addFriendPopup.contains(event.target)) {
                    console.log('Clicked outside add friend popup');
                    this.showAddFriendPopup = false;
                }
            }
        },
        mounted() {
            this.fetchFriends();
            document.addEventListener('click', this.handleClickOutside);
        },
        beforeDestroy() {
            document.removeEventListener('click', this.handleClickOutside);
        }
    };
</script>

<style scoped>
    .friend-manager {
        display: flex;
        justify-content: space-between;
        background-color: #fff;
        padding: 20px;
        height: 100%;
        border-radius: 20px;
        box-shadow: 0 8px 8px rgba(0, 0, 0, 0.05);
    }

    .left-section {
        width: 25%;
        padding: 20px;
        background-color: #fffcf4;
        border-radius: 20px;
        box-shadow: 0 8px 8px rgba(0, 0, 0, 0.05);
        border-right: 4px solid #fb8142;
        border-top: 2px solid #fb8142;
        border-bottom: 2px solid #fb8142;
        overflow: auto;
    }

    .right-section {
        width: 70%;
        margin-left: 15px;
        padding: 20px;
        background-color: #ffe2d8;
        border-radius: 20px;
        box-shadow: 0 8px 8px rgba(0, 0, 0, 0.05);
        overflow-y: auto;
        border-bottom: 2px solid #fb8142;
        border-top: 2px solid #fb8142;
        border-right: 4px solid #fb8142;
    }

    .main-title {
        color: #ff6b6b;
        font-size: 25px;
        font-weight: bold;
        margin-bottom: 10px;
    }

    .sub-title {
        color: #ff6b6b;
        font-size: 18px;
        margin-bottom: 20px;
    }

    .friend-list {
        margin-top: 10px;
    }

    .friend-item {
        display: flex;
        align-items: center;
        margin-bottom: 10px;
        padding: 20px;
        background-color: #f9f9f9;
        border-radius: 15px;
        width: 100%;
        box-shadow: 0 1px 6px rgba(0, 0, 0, 0.05);
        transition: all 0.3s ease;
        border-bottom: 3px solid #fb8142;
        border-right: 3px solid #fb8142;
    }

        .friend-item:hover {
            background-color: #f4f4f4;
            transform: translateY(-2px);
        }

    .avatar {
        width: 50px;
        height: 50px;
        border-radius: 50%;
        margin-right: 15px;
        border: 2px solid #ff6b6b;
        transition: transform 0.3s ease, box-shadow 0.3s ease;
    }

    .friend-details {
        flex: 1;
    }

    .friend-name {
        font-weight: bold;
        font-size: 18px;
        color: #333;
    }

    .friend-group {
        color: #999;
        font-size: 12px;
    }

    .remove-button {
        background-color: #ff6b6b;
        border: none;
        color: white;
        padding: 10px 20px;
        border-radius: 20px;
        cursor: pointer;
        transition: background-color 0.3s ease, transform 0.3s ease;
    }

        .remove-button:hover {
            background-color: #e85050;
            transform: scale(1.1);
        }

    .search-bar {
        display: flex;
        margin-bottom: 20px;
        height:10%;
    }

    .search-input {
        flex: 1;
        padding: 10px;
        border: none;
        border-radius: 25px 0 0 25px;
        outline: none;
        box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
        transition: all 0.3s ease;
        font-size:25px;
        font-weight:bold;
        //margin-left:20px;
    }

    .search-button {
        padding: 10px 20px;
        border: none;
        background-color: #ff6b6b;
        color: white;
        border-radius: 0 25px 25px 0;
        cursor: pointer;
        transition: background-color 0.3s ease, transform 0.3s ease;
        font-size: 25px;
        font-weight: bold;
    }

        .search-button:hover {
            background-color: #e85050;
            transform: scale(1.1);
        }

    .search-results {
        margin-top: 10px;
    }

    .search-result {
        display: flex;
        align-items: center;
        margin-bottom: 10px;
        padding: 20px;
        background-color: #f9f9f9;
        border-radius: 15px;
        width: calc(50% - 15px);
        box-shadow: 0 1px 6px rgba(0, 0, 0, 0.05);
        transition: all 0.3s ease;
        border-bottom: 3px solid #fb8142;
        border-right: 3px solid #fb8142;
    }

        .search-result:hover {
            background-color: #f4f4f4;
            transform: translateY(-2px);
        }

    .result-details {
        flex: 1;
    }

    .result-name {
        font-weight: bold;
        font-size: 18px;
        color: #333;
    }

    .result-group {
        color: #999;
        font-size: 12px;
    }

    .add-button {
        background-color: #4caf50;
        border: none;
        color: white;
        padding: 10px 20px;
        border-radius: 20px;
        cursor: pointer;
        transition: background-color 0.3s ease, transform 0.3s ease;
    }

        .add-button:hover {
            background-color: #388e3c;
            transform: scale(1.1);
        }

    .add-friend-popup, .delete-friend-popup {
        position: absolute;
        top: 50%;
        left: 50%;
        width: 400px; /* 固定宽度 */
        padding: 20px;
        background-color: #fff;
        border-radius: 20px;
        box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
        transform: translate(-50%, -50%);
        z-index: 1000;
        font-weight: bold;
        font-family: 'Microsoft YaHei';
        font-size:25px;
        text-align: center; /* 居中对齐 */
    }

    .popup-item {
        margin-top: 10px;
        padding: 15px; /* 增加 padding 以改善按钮大小 */
        border-radius: 10px;
        font-size: 20px; /* 调整字体大小 */
        font-weight: bold;
        cursor: pointer;
        transition: background-color 0.3s ease, transform 0.3s ease;
        border:0;
        margin :10px;
    }

        .popup-item:hover {
            transform: scale(1.05);
        }


        .popup-item.family {
            background-color: #ffb3ba;
            font-weight: bold;
        }

            .popup-item.family:hover {
                background-color: #ff9a9a;
            }

        .popup-item.friend {
            background-color: #ffdfba;
            font-weight: bold;
        }

            .popup-item.friend:hover {
                background-color: #ffcd9a;
            }

        .popup-item.classmate {
            background-color: #ffffba;
            font-weight: bold;
        }

            .popup-item.classmate:hover {
                background-color: #ffff9a;
            }

        .popup-item.cancel-button {
            background-color: #c1c1c1;
            font-weight: bold;
        }

            .popup-item.cancel-button:hover {
                background-color: #a1a1a1;
            }

        .popup-item.confirm-button {
            background-color: #ff6b6b;
            font-weight: bold;
        }

            .popup-item.confirm-button:hover {
                background-color: #e85050;
            }


    .no-results {
        font-size: 25px;
        font-weight: bold;
        margin-left: 45%;
        margin-top: 10%;
    }

    .notification-toast {
        position: fixed;
        bottom: 20px;
        left: 50%;
        transform: translateX(-50%);
        background-color: #17a2b8; /* 青色背景 */
        color: white;
        padding: 10px 20px;
        border-radius: 5px;
        z-index: 1000;
        opacity: 0;
        animation: fadeInOut 4s ease forwards;
        font-weight: bold;
        font-family: 'Microsoft YaHei';
    }

    @keyframes fadeInOut {
        0% {
            opacity: 0;
        }

        10% {
            opacity: 1;
        }

        90% {
            opacity: 1;
        }

        100% {
            opacity: 0;
        }
    }


</style>
