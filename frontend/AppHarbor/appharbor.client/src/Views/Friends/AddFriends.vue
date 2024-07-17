<template>
    <div class="friend-manager">
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
                    <button @click="removeFriend(friend.id)" class="remove-button">-</button>
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
                <button class="popup-item" @click="confirmAddFriend('family')">
                    <span>家人</span>
                </button>
                <button class="popup-item" @click="confirmAddFriend('friend')">
                    <span>朋友</span>
                </button>
                <button class="popup-item" @click="confirmAddFriend('classmate')">
                    <span>同学</span>
                </button>
            </div>
        </transition>
    </div>
</template>

<script>
    import axios from 'axios';
    import Cookies from 'js-cookie';

    export default {
        data() {
            return {
                friends: [],
                searchQuery: '',
                searchResults: [],
                showAddFriendPopup: false,
                selectedUserId: null,
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
                let formData = new FormData();
                let userId = Number(this.searchQuery);
                formData.append('inputId', userId);
                axios.post('http://localhost:5118/api/user/searchid', formData)
                    .then(response => {
                        if (response.data && response.data.id) {
                            this.searchResults = [response.data];
                        } else {
                            this.searchResults = [];
                        }
                    })
                    .catch(error => {
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
                        this.fetchFriends();
                    })
                    .catch(error => {
                        alert('好友' + userId + '已经是您的好友,不需要重复添加');
                        console.error('Error adding friend:', error);
                    });
            },
            removeFriend(userId) {
                var token = Cookies.get('token');
                let formData = new FormData();
                formData.append('token', token);
                formData.append('friendid', userId);
                axios.post('http://localhost:5118/api/relationship/deletefriend', formData)
                    .then(() => {
                        this.friends = this.friends.filter(friend => friend.id !== userId);
                    })
                    .catch(error => {
                        console.error('Error removing friend:', error);
                    });
            },
            getAvatarUrl(avatarPath) {
                if (avatarPath) {
                    return `http://localhost:5118${avatarPath}`;
                }
                return require('../../../public/default.png');
            },
            handleClickOutside(event) {
                if (this.showAddFriendPopup && !this.$refs.addFriendPopup.contains(event.target)) {
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
        font-size: 12px;
        color: #666;
    }

    .remove-button {
        background-color: #FF6347;
        color: white;
        border: none;
        padding: 8px 12px;
        text-align: center;
        text-decoration: none;
        display: inline-block;
        font-size: 14px;
        cursor: pointer;
        border-radius: 50%;
        transition: background-color 0.3s ease;
    }

        .remove-button:hover {
            background-color: #FF4500;
        }

    .search-bar {
        margin-bottom: 20px;
    }

    .search-input {
        width: 80%;
        padding: 10px;
        font-size: 14px;
        border: 1px solid #fbb1a2;
        border-radius: 5px;
        outline: none;
        display: inline-block;
    }

    .search-button {
        padding: 10px 20px;
        font-size: 14px;
        border: none;
        background-color: #4CAF50;
        color: white;
        border-radius: 5px;
        cursor: pointer;
        margin-left: 10px;
    }

        .search-button:hover {
            background-color: #45a049;
        }

    .search-results {
        overflow-y: auto;
        max-height: 400px;
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

    .add-button {
        background-color: #4CAF50;
        color: white;
        border: none;
        padding: 8px 12px;
        text-align: center;
        text-decoration: none;
        display: inline-block;
        font-size: 14px;
        cursor: pointer;
        border-radius: 50%;
        transition: background-color 0.3s ease;
    }

        .add-button:hover {
            background-color: #45a049;
        }

    .no-results {
        color: #ff6347;
    }

    .add-friend-popup {
        position: absolute;
        top: 200px;
        left: 50%;
        transform: translateX(-50%);
        background-color: #fff;
        border-radius: 12px;
        padding: 16px;
        width: 200px;
        box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
        z-index: 1000;
        opacity: 1;
    }

    .popup-item {
        display: flex;
        align-items: center;
        justify-content: center;
        margin-bottom: 8px;
        padding: 8px 16px;
        font-size: medium;
        border: none;
        border-radius: 12px;
        font-family: 'Baloo 2', cursive;
        font-weight: bolder;
        cursor: pointer;
        transition: background-color 0.3s, transform 0.3s, color 0.3s;
        background-color: #f5f5f5;
    }

        .popup-item:hover {
            background-color: #ffe5e5;
            transform: scale(1.05);
        }

        .popup-item:active {
            background-color: #ffcccc;
            transform: scale(0.95);
        }

        .popup-item:last-child {
            margin-bottom: 0;
        }
</style>