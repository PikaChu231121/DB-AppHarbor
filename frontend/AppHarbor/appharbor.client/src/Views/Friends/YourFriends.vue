<template>
    <div class="container light">
        <div class="sidebar">
            <div class="group"
                 v-for="group in groups"
                 :key="group.name"
                 :class="{ selected: selectedGroup && selectedGroup.name === group.name }"
                 @click="toggleGroupSelection(group)">
                <div class="group-header">{{ group.name }}</div>
                <div class="group-friend-icons">
                    <template v-for="(friend, index) in group.friends.slice(0, 3)">
                        <img v-if="index < 3"
                             :key="friend.id"
                             :src="friend.avatar"
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
                <div class="friend-item" v-for="friend in friends" :key="friend.id">
                    <img :src="friend.avatar" class="avatar" />
                    <div class="friend-info">
                        <div class="friend-title">{{ friend.name }}</div>
                        <div class="friend-description">{{ friend.description }}</div>
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
                groups: [{ name: 'Family', friends: [] },
                        { name: 'Friend', friends: [] },
                        { name: 'Classmate', friends: [] },],
                friends: [],
                /*                    { id: 1, avatar: 'avatar1.png', title: 'Title 1', description: 'Description 1' },
                                      { id: 2, avatar: 'avatar2.png', title: 'Title 2', description: 'Description 2' },
                                      { id: 3, avatar: 'avatar3.png', title: 'Title 3', description: 'Description 3' },*/
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
                return this.selectedGroup ? this.selectedGroup.name : 'All Friends';
            }
        },
        methods: {
            getfriend() {
                var token = Cookies.get('token');

                let formData4 = new FormData;
                formData4.append('token', token);
                axios.post('http://localhost:5118/api/relationship/findall', formData4)
                    .then(response => {
                        groups.forEach(group => {
                            friends = response.data.data.$values;
                            console.log(friends);
                        });

                    })
                    .catch(error => {
                        this.alertMessage = error.response.data.msg;
                    });

                let formData1 = new FormData();
                formData1.append('token', token);
                formData1.append('relationship', "family");
                axios.post('http://localhost:5118/api/relationship/findmysubscriber', formData1)
                    .then(response => {
                        this.groups.forEach(group => {
                            if (group.name === 'Family') {
                                group.friends=response.data.data.$values;
                                //console.log(response.data.data);
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
                                group.friends=response.data.data.$values;
                                console.log(response.data.data);
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
                                //console.log(response.data.data);
                            }
                        });
                    })
                    .catch(error => {
                        this.alertMessage = error.response.data;
                    });


            
            },

            toggleGroupSelection(group) {
                if (this.selectedGroup && this.selectedGroup.name === group.name) {
                    this.selectedGroup = null;
                } else {
                    this.selectedGroup = group;
                }
            },
        },
        mounted() {
            this.getfriend();
        }
    };
</script>


<style scoped>
    .light {
        --md-sys-color-primary: rgb(144, 74, 64);
        --md-sys-color-surface-tint: rgb(144, 74, 64);
        --md-sys-color-on-primary: rgb(255, 255, 255);
        --md-sys-color-primary-container: rgb(255, 218, 212);
        --md-sys-color-on-primary-container: rgb(59, 9, 5);
        --md-sys-color-secondary: rgb(143, 76, 51);
        --md-sys-color-on-secondary: rgb(255, 255, 255);
        --md-sys-color-secondary-container: rgb(255, 219, 207);
        --md-sys-color-on-secondary-container: rgb(56, 13, 0);
        --md-sys-color-tertiary: rgb(112, 92, 46);
        --md-sys-color-on-tertiary: rgb(255, 255, 255);
        --md-sys-color-tertiary-container: rgb(251, 223, 166);
        --md-sys-color-on-tertiary-container: rgb(37, 26, 0);
        --md-sys-color-error: rgb(186, 26, 26);
        --md-sys-color-on-error: rgb(255, 255, 255);
        --md-sys-color-error-container: rgb(255, 218, 214);
        --md-sys-color-on-error-container: rgb(65, 0, 2);
        --md-sys-color-background: rgb(255, 248, 246);
        --md-sys-color-on-background: rgb(35, 25, 24);
        --md-sys-color-surface: rgb(255, 248, 247);
        --md-sys-color-on-surface: rgb(34, 25, 26);
        --md-sys-color-surface-variant: rgb(245, 221, 217);
        --md-sys-color-on-surface-variant: rgb(83, 67, 64);
        --md-sys-color-outline: rgb(133, 115, 111);
        --md-sys-color-outline-variant: rgb(216, 194, 189);
        --md-sys-color-shadow: rgb(0, 0, 0);
        --md-sys-color-scrim: rgb(0, 0, 0);
        --md-sys-color-inverse-surface: rgb(56, 46, 46);
        --md-sys-color-inverse-on-surface: rgb(255, 237, 236);
        --md-sys-color-inverse-primary: rgb(255, 180, 168);
        --md-sys-color-primary-fixed: rgb(255, 218, 212);
        --md-sys-color-on-primary-fixed: rgb(59, 9, 5);
        --md-sys-color-primary-fixed-dim: rgb(255, 180, 168);
        --md-sys-color-on-primary-fixed-variant: rgb(115, 52, 43);
        --md-sys-color-secondary-fixed: rgb(255, 219, 207);
        --md-sys-color-on-secondary-fixed: rgb(56, 13, 0);
        --md-sys-color-secondary-fixed-dim: rgb(255, 181, 154);
        --md-sys-color-on-secondary-fixed-variant: rgb(113, 54, 30);
        --md-sys-color-tertiary-fixed: rgb(251, 223, 166);
        --md-sys-color-on-tertiary-fixed: rgb(37, 26, 0);
        --md-sys-color-tertiary-fixed-dim: rgb(222, 195, 140);
        --md-sys-color-on-tertiary-fixed-variant: rgb(86, 68, 25);
        --md-sys-color-surface-dim: rgb(231, 214, 214);
        --md-sys-color-surface-bright: rgb(255, 248, 247);
        --md-sys-color-surface-container-lowest: rgb(255, 255, 255);
        --md-sys-color-surface-container-low: rgb(255, 240, 240);
        --md-sys-color-surface-container: rgb(252, 234, 234);
        --md-sys-color-surface-container-high: rgb(246, 228, 228);
        --md-sys-color-surface-container-highest: rgb(240, 222, 222);
    }

    .container {
        display: flex;
        height: 100%;
        background-color: var(--md-sys-color-background);
        color: var(--md-sys-color-on-background);
        border-radius: 20px;
        padding: 10px;
    }

    .sidebar {
        width: 30%;
        padding: 10px;
        border-radius: 20px;
        border: 2px solid #d3d3d3;
    }

    .group {
        margin-bottom: 10px;
        padding: 10px;
        border-radius: 20px;
        border: 2px solid #d3d3d3;
        transition: all 0.3s ease;
    }

    .group-header {
        font-size: 18px;
        font-weight: bold;
        margin-bottom: 10px;
        color: var(--md-sys-color-primary);
    }

    .group-friend-icons {
        display: flex;
        align-items: center;
        border-radius: 20px;
        border: 2px solid #d3d3d3;
        padding: 5px;
    }

    .friend-icon {
        width: 40px;
        height: 40px;
        border-radius: 50%;
        margin-right: 5px;
        border: 2px solid #d3d3d3;
    }

        .friend-icon.more {
            display: flex;
            align-items: center;
            justify-content: center;
            background-color: var(--md-sys-color-surface-variant);
            font-size: 14px;
            color: var(--md-sys-color-on-surface-variant);
        }

    .group:hover {
        background-color: #f0f0f0;
        transform: scale(1.02);
    }

    .group.selected {
        background-color: #e0e0e0;
    }

    .main {
        width: 70%;
        padding: 10px;
        background-color: var(--md-sys-color-surface);
        border-radius: 20px;
        border: 2px solid #d3d3d3;
    }

    .header {
        display: flex;
        justify-content: space-between;
        align-items: center;
        margin-bottom: 20px;
        padding-bottom: 10px;
        border-bottom: 2px solid #d3d3d3;
    }

    .header-title {
        font-size: 24px;
        font-weight: bold;
        color: var(--md-sys-color-primary);
    }

    .search-input {
        width: 200px;
    }

    .friends-list {
        display: flex;
        flex-wrap: wrap;
    }

    .friend-item {
        display: flex;
        align-items: center;
        background-color: var(--md-sys-color-surface-variant);
        padding: 10px;
        border-radius: 20px;
        margin: 10px;
        width: 200px;
        border: 2px solid #d3d3d3;
    }

    .avatar {
        width: 50px;
        height: 50px;
        border-radius: 50%;
        margin-right: 10px;
        border: 2px solid #d3d3d3;
    }

    .friend-info {
        flex: 1;
    }

    .friend-title {
        font-weight: bold;
        color: var(--md-sys-color-primary);
    }

    .friend-description {
        color: var(--md-sys-color-on-surface-variant);
    }
</style>
