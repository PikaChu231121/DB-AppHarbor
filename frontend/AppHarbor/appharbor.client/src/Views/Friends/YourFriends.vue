<template>
    <div class="container">
        <div class="sidebar">
            <div class="group"
                 v-for="group in groups"
                 :key="group.name"
                 :class="{ selected: selectedGroup && selectedGroup.name === group.name }"
                 @click="toggleGroupSelection(group)">
                <div class="group-header">{{ group.Chinese }}</div>
                <div class="group-friend-icons">
                    <template v-for="(friend, index) in group.friends.slice(0, 3)">
                        <img v-if="index < 3"
                             :key="friend.id"
                             :src="getAvatarUrl(friend.avatar)"
                             class="friend-icon" />
                    </template>
                    <div v-if="group.friends.length > 3" class="friend-icon more">
                        +{{ group.friends.length - 3 }}
                    </div>
                </div>
            </div>
        </div>

        <div class="main">
            <div class="header">
                <div class="header-title">{{ selectedGroupName }}</div>
                <el-input v-model="searchQuery"
                          placeholder="Search"
                          prefix-icon="el-icon-search"
                          class="search-input"></el-input>
            </div>

            <div class="friends-list">
                <div class="friend-item-container" v-if="selectedGroup !== null">
                    <div class="friend-item" v-for="friend in filtered2Friends" :key="friend.id">
                        <img :src="getAvatarUrl(friend.avatar)" class="avatar" />
                        <div class="friend-info">
                            <div class="friend-title">{{ friend.nickname }}</div>
                            <div class="friend-description" :class="translateState(friend.state).class">
                                {{ translateState(friend.state).text }}
                            </div>
                        </div>
                    </div>
                </div>
                <div class="friend-item-container" v-else>
                    <div class="friend-item" v-for="friend in friends" :key="friend.id">
                        <img :src="getAvatarUrl(friend.avatar)" class="avatar" />
                        <div class="friend-info">
                            <div class="friend-title">{{ friend.nickname }}</div>
                            <div class="friend-description" :class="translateState(friend.state).class">
                                {{ translateState(friend.state).text }}
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

    export default {
        data() {
            return {
                searchQuery: '',
                selectedGroup: null,
                groups: [
                    { name: 'Family', friends: [], Chinese: '家人' },
                    { name: 'Friend', friends: [], Chinese: '朋友' },
                    { name: 'Classmate', friends: [], Chinese: '同学' },
                ],
                friends: [],
            };
        },
        computed: {
            filteredFriends() {
                if (!this.selectedGroup) {
                    return this.friends.filter(friend =>
                        friend.title.toLowerCase().includes(this.searchQuery.toLowerCase())
                    );
                }
                return this.selectedGroup.friends.filter(friend =>
                    friend.name.toLowerCase().includes(this.searchQuery.toLowerCase())
                );
            },
            selectedGroupName() {
                return this.selectedGroup ? this.selectedGroup.Chinese : '全部好友';
            },
            filtered2Friends() {
                if (!this.selectedGroup.name) {
                    return [];
                }
                const group = this.groups.find(group => group.name === this.selectedGroup.name);
                return group ? group.friends : [];
            },
        },
        methods: {
            getfriend() {
                var token = Cookies.get('token');

                let formData1 = new FormData();
                formData1.append('token', token);
                formData1.append('relationship', "family");
                axios.post('http://localhost:5118/api/relationship/findmysubscriber', formData1)
                    .then(response => {
                        this.groups.forEach(group => {
                            if (group.name === 'Family') {
                                group.friends = response.data.data.$values;
                            }
                        });
                    })
                    .catch(error => {
                        this.alertMessage = error.response.data;
                    });

                let formData2 = new FormData();
                formData2.append('token', token);
                formData2.append('relationship', 'friend');
                axios.post('http://localhost:5118/api/relationship/findmysubscriber', formData2)
                    .then(response => {
                        this.groups.forEach(group => {
                            if (group.name === 'Friend') {
                                group.friends = response.data.data.$values;
                            }
                        });
                    })
                    .catch(error => {
                        this.alertMessage = error.response.data;
                    });

                let formData3 = new FormData();
                formData3.append('token', token);
                formData3.append('relationship', 'classmate');
                axios.post('http://localhost:5118/api/relationship/findmysubscriber', formData3)
                    .then(response => {
                        this.groups.forEach(group => {
                            if (group.name === 'Classmate') {
                                group.friends = response.data.data.$values;
                            }
                        });
                    })
                    .catch(error => {
                        this.alertMessage = error.response.data;
                    });

                let formData4 = new FormData();
                formData4.append('token', token);
                axios.post('http://localhost:5118/api/relationship/findall', formData4)
                    .then(response => {
                        this.groups.forEach(group => {
                            this.friends = response.data.data.$values;
                        });
                    })
                    .catch(error => {
                        console.log(error);
                    });
            },

            toggleGroupSelection(group) {
                if (this.selectedGroup && this.selectedGroup.name === group.name) {
                    this.selectedGroup = null;
                } else {
                    this.selectedGroup = group;
                }
            },

            getAvatarUrl(avatarPath) {
                if (avatarPath) {
                    return `http://localhost:5118${avatarPath}`;
                }
                return '../../../public/default.png'; // Default avatar path
            },

            translateState(state) {
                switch (state) {
                    case 'active':
                        return { text: '• 活跃', class: 'status-active' };
                    case 'banned':
                        return { text: '• 被封禁', class: 'status-banned' };
                    default:
                        return { text: state, class: '' };
                }
            }

        },
        mounted() {
            this.getfriend();
        }
    };
</script>

<style scoped>
    @import url('https://fonts.googleapis.com/css2?family=Nunito:wght@400;600;700&display=swap');

    .container {
        display: flex;
        height: 100%;
        background-color: white;
        color: #333;
        //font-family: 'Nunito', sans-serif;
        border-radius: 20px;
        padding: 20px;
        //box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
        overflow: hidden;
    }

    .sidebar {
        width: 25%;
        padding: 20px;
        background-color: #fff;
        border-radius: 20px;
        box-shadow: 0 8px 8px rgba(0, 0, 0, 0.05);
        transition: all 0.3s ease;
        border-right: 4px solid #fb8142;
        border-top: 2px solid #fb8142;
        border-bottom: 2px solid #fb8142;
        //border-left: 1px solid #fb8142;
    }

    .group {
        //margin-left: 15px;
        margin-bottom: 15px;
        padding: 15px;
        border-radius: 10px;
        background-color: #fafafa;
        box-shadow: 0 1px 6px rgba(0, 0, 0, 0.05);
        cursor: pointer;
        transition: all 0.3s ease;
    }

    .group-header {
        font-size: 18px;
        font-weight: 600;
        margin-bottom: 10px;
        color: #ff6b6b;
    }

    .group-friend-icons {
        display: flex;
        align-items: center;
    }

    .friend-icon {
        width: 40px;
        height: 40px;
        border-radius: 50%;
        margin-right: 5px;
        border: 2px solid #ff6b6b;
        transition: transform 0.3s ease;
    }

        .friend-icon.more {
            display: flex;
            align-items: center;
            justify-content: center;
            background-color: #ff6b6b;
            color: #fff;
            font-size: 14px;
            font-weight: 600;
        }

    .group:hover {
        background-color: #f5f5f5;
        transform: translateY(-2px);
    }

    .group.selected {
        background-color: #fff;
        box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
    }

    .main {
        width: 70%;
        margin-left: 15px;
        padding: 20px;
        background-color: #fff;
        border-radius: 20px;
        box-shadow: 0 8px 8px rgba(0, 0, 0, 0.05);
        transition: all 0.3s ease;
        overflow-y: auto;
        border-bottom: 2px solid #fb8142;
        border-top: 2px solid #fb8142;
        //border-left: 4px solid #fb8142;
        border-right: 4px solid #fb8142;
    }

    .header {
        display: flex;
        justify-content: space-between;
        align-items: center;
        margin-bottom: 20px;
        padding-bottom: 10px;
        border-bottom: 2px solid #f0f0f0;
    }

    .header-title {
        font-size: 24px;
        font-weight: 700;
        color: #ff6b6b;
    }

    .search-input {
        width: 250px;
        border-radius: 25px;
        box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
        transition: all 0.3s ease;
    }

    .friends-list {
        display: flex;
        flex-wrap: wrap;
        gap: 15px;
    }

    .friend-item-container {
        display: flex;
        flex-wrap: wrap;
        gap: 15px;

    }

    .friend-item {
        display: flex;
        align-items: center;
        background-color: #f9f9f9;
        padding: 20px;
        min-width:180px;
        border-radius: 15px;
        width: calc(50% - 15px);
        box-shadow: 0 1px 6px rgba(0, 0, 0, 0.05);
        transition: all 0.3s ease;
        border-bottom: 3px solid #fb8142;
        border-right: 3px solid #fb8142;
    }

        .friend-item:hover {
            transform: translateY(-2px);
            background-color: #f4f4f4;
        }

    .avatar {
        width: 50px;
        height: 50px;
        border-radius: 50%;
        margin-right: 15px;
        border: 2px solid #ff6b6b;
        transition: all 0.3s ease;
    }

    .friend-info {
        flex: 1;
    }

    .friend-title {
        font-size: 18px;
        font-weight: 600;
        color: #333;
    }

    .friend-description {
        color: #999;
        font-weight: 400;
        margin-top: 5px;
    }

    .status-active {
        color: #3cbfa5;
        font-weight: bold;
    }

    .status-banned {
        color: #e74c3c;
        font-weight: bold;
    }

    /* Smooth transitions for hover effects */
    .group:hover .friend-icon,
    .friend-item:hover .avatar {
        transform: scale(1.05);
    }

    .friend-icon,
    .avatar {
        transition: transform 0.3s ease, box-shadow 0.3s ease;
    }

        .friend-icon:hover,
        .avatar:hover {
            box-shadow: 0 0 0 4px rgba(255, 107, 107, 0.2);
        }
</style>
