<template>
    <div class="friend-manager">
        <Loading :loading="isLoading" />
        <LoginAlert v-if="alertMessage" style="z-index: 1;" :message="alertMessage" @close="alertMessage = ''" />
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
                <p style="font-family: 'Baloo 2', cursive;">请选择添加关系的种类：</p>
                <button class="popup-item family" @click.stop="confirmAddFriend('family')">
                    <span style="font-family: 'Baloo 2', cursive;">家人</span>
                </button>
                <button class="popup-item friend" @click.stop="confirmAddFriend('friend')">
                    <span style="font-family: 'Baloo 2', cursive;">朋友</span>
                </button>
                <button class="popup-item classmate" @click.stop="confirmAddFriend('classmate')">
                    <span style="font-family: 'Baloo 2', cursive;">同学</span>
                </button>
                <button class="popup-item cancel-button" @click.stop="cancelAddFriend">
                    <span style="font-family: 'Baloo 2', cursive;">取消</span>
                </button>
            </div>
        </transition>

        <!-- Delete Friend Popup -->
        <transition name="popup">
            <div v-if="showDeleteFriendPopup" class="delete-friend-popup" ref="deleteFriendPopup" @click.stop>
                <p style="font-family: 'Baloo 2', cursive;">你确定要删除好友{{ friendToDeleteNickname }}吗？</p>
                <button class="popup-item confirm-button" @click.stop="confirmDeleteFriend">
                    <span style="font-family: 'Baloo 2', cursive;">确定</span>
                </button>
                <button class="popup-item cancel-button" @click.stop="cancelDeleteFriend">
                    <span style="font-family: 'Baloo 2', cursive;">取消</span>
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
            fetchFriends() {
                var token = Cookies.get('token');
                let formData1 = new FormData();
                formData1.append('token', token);
                axios.post('http://localhost:5118/api/relationship/findall', formData1)
                    .then(response => {
                        this.friends = response.data.data.$values;
                    })
                    .catch(error => {
                        console.error('Error fetching friends:', error);
                    });
            },
            searchUsers() {
                this.isLoading = true;

                let formData = new FormData();
                let userId = Number(this.searchQuery);
                formData.append('inputId', userId);
                axios.post('http://localhost:5118/api/user/searchid', formData)
                    .then(response => {
                        setTimeout(() => {
                            this.isLoading = false; // Hide loading animation

                        }, 2000); // Delay for 2 seconds
                        if (response.data && response.data.id) {
                            this.searchResults = [response.data];
                        } else {
                            setTimeout(() => {
                                this.isLoading = false; // Hide loading animation

                            }, 2000); // Delay for 2 seconds
                            this.searchResults = [];
                        }
                    })
                    .catch(error => {
                        setTimeout(() => {
                            this.isLoading = false; // Hide loading animation

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
                axios.post('http://localhost:5118/api/relationship/addfriend', formData)
                    .then(() => {
                        this.isLoading = true;
                        this.fetchFriends();
                        setTimeout(() => {
                            this.isLoading = false; // Hide loading animation

                        }, 2000); // Delay for 2 seconds
                        this.alertMessage = `添加成功`;
                    })
                    .catch(error => {
                        if (error.response.data.data == 5) {
                            //alert('好友不能是自己');
                            this.alertMessage = `好友不能是自己 `;

                        }
                        else {
                            this.alertMessage = `好友` + userId + `已经是您的好友,不需要重复添加`;

                            setTimeout(() => {
                                this.isLoading = false; // Hide loading animation

                            }, 2000);
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
                axios.post('http://localhost:5118/api/relationship/deletefriend', formData)
                    .then(() => {
                        this.isLoading = true;
                        this.friends = this.friends.filter(friend => friend.id !== userId);
                        this.alertMessage = `删除成功`;

                        setTimeout(() => {
                            this.isLoading = false; // Hide loading animation

                        }, 1000);
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
                    return `http://localhost:5118${avatarPath}`;
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
        background-color: #fbb1a2;
        padding: 20px;
        border-radius: 10px;
        box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
    }

    .left-section {
        width: 30%;
    }

    .right-section {
        width: 70%;
    }

    .main-title {
        color: #fff;
        font-size: 24px;
        margin-bottom: 10px;
    }

    .sub-title {
        color: #fff;
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
        padding: 10px;
        border-radius: 5px;
        background-color: #fff;
        transition: background-color 0.3s ease;
        box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
    }

        .friend-item:hover {
            background-color: #f99d85;
        }

    .avatar {
        width: 40px;
        height: 40px;
        border-radius: 50%;
        margin-right: 10px;
    }

    .friend-details {
        flex: 1;
    }

    .friend-name {
        font-weight: bold;
    }

    .friend-group {
        color: #888;
        font-size: 12px;
    }

    .remove-button {
        background-color: #ff6b6b;
        border: none;
        color: white;
        padding: 5px 10px;
        border-radius: 5px;
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
    }

    .search-input {
        flex: 1;
        padding: 10px;
        border: none;
        border-radius: 5px 0 0 5px;
        outline: none;
    }

    .search-button {
        padding: 10px 20px;
        border: none;
        background-color: #ff6b6b;
        color: white;
        border-radius: 0 5px 5px 0;
        cursor: pointer;
        transition: background-color 0.3s ease, transform 0.3s ease;
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
        padding: 10px;
        border-radius: 5px;
        background-color: #fff;
        transition: background-color 0.3s ease;
        box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
    }

        .search-result:hover {
            background-color: #f99d85;
        }

    .result-details {
        flex: 1;
    }

    .result-name {
        font-weight: bold;
    }

    .result-group {
        color: #888;
        font-size: 12px;
    }

    .add-button {
        background-color: #4caf50;
        border: none;
        color: white;
        padding: 5px 10px;
        border-radius: 5px;
        cursor: pointer;
        transition: background-color 0.3s ease, transform 0.3s ease;
    }

        .add-button:hover {
            background-color: #388e3c;
            transform: scale(1.1);
        }

    .add-friend-popup, .delete-friend-popup {
        position: absolute;
        top: 200px;
        left: 50%;
        transform: translateX(-50%);
        background-color: #fff;
        border-radius: 12px;
        padding: 16px;
        box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
        z-index: 1000;
        opacity: 1;
        transition: opacity 0.3s ease;
    }

    .popup-item {
        display: block;
        width: 100%;
        margin-top: 10px;
        padding: 10px;
        border-radius: 5px;
        text-align: center;
        cursor: pointer;
        border: none;
        transition: background-color 0.3s ease, transform 0.3s ease;
    }

        .popup-item:hover {
            transform: scale(1.05);
        }

        .popup-item.family {
            background-color: #ffb3ba;
        }

            .popup-item.family:hover {
                background-color: #ff9a9a;
            }

        .popup-item.friend {
            background-color: #ffdfba;
        }

            .popup-item.friend:hover {
                background-color: #ffcd9a;
            }

        .popup-item.classmate {
            background-color: #ffffba;
        }

            .popup-item.classmate:hover {
                background-color: #ffff9a;
            }

        .popup-item.cancel-button {
            background-color: #c1c1c1;
        }

            .popup-item.cancel-button:hover {
                background-color: #a1a1a1;
            }

        .popup-item.confirm-button {
            background-color: #ff6b6b;
        }

            .popup-item.confirm-button:hover {
                background-color: #e85050;
            }
</style>
