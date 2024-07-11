<template>
    <div class="friend-manager">
        <!-- Left Section -->
        <div class="left-section">
            <h2 class="main-title">ADD FRIEND</h2>
            <p class="sub-title">Your Friends</p>
            <div class="friend-list">
                <div v-for="friend in friends" :key="friend.id" class="friend-item">
                    <img :src="friend.avatar" class="avatar" alt="Friend Avatar">
                    <div class="friend-details">
                        <p class="friend-name">{{ friend.name }}</p>
                        <p class="friend-id">{{ friend.id }}</p>
                    </div>
                </div>
            </div>
        </div>

        <!-- Right Section -->
        <div class="right-section">
            <div class="search-bar">
                <input type="text" v-model="searchQuery" placeholder="Search by User ID" class="search-input">
            </div>
            <div class="search-results">
                <div v-if="searchResults.length === 0" class="no-results">
                    No results found.
                </div>
                <div v-else>
                    <div v-for="result in searchResults" :key="result.id" class="search-result">
                        <img :src="result.avatar" class="avatar" alt="Search Result Avatar">
                        <div class="result-details">
                            <p class="result-name">{{ result.name }}</p>
                            <p class="result-id">{{ result.id }}</p>
                        </div>
                        <button @click="addFriend(result.id)" class="add-button">+</button>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
    import axios from 'axios'; // Assuming you've installed Axios

    export default {
        data() {
            return {
                friends: [
                    { id: 1, name: 'Alice', avatar: 'https://randomuser.me/api/portraits/women/1.jpg' },
                    { id: 2, name: 'Bob', avatar: 'https://randomuser.me/api/portraits/men/1.jpg' },
                    { id: 3, name: 'Charlie', avatar: 'https://randomuser.me/api/portraits/men/2.jpg' },
                    { id: 4, name: 'David', avatar: 'https://randomuser.me/api/portraits/men/3.jpg' },
                    { id: 5, name: 'Eve', avatar: 'https://randomuser.me/api/portraits/women/2.jpg' },
                ],
                searchQuery: '',
                searchResults: [],
            };
        },
        methods: {
            // Simulated API call to get friends list
            fetchFriends() {
                // Replace with actual API endpoint
                axios.get('https://api.example.com/friends')
                    .then(response => {
                        this.friends = response.data;
                    })
                    .catch(error => {
                        console.error('Error fetching friends:', error);
                    });
            },
            // Simulated API call to search for users
            searchUsers() {
                // Replace with actual API endpoint
                axios.get(`https://api.example.com/users?q=${this.searchQuery}`)
                    .then(response => {
                        this.searchResults = response.data;
                    })
                    .catch(error => {
                        console.error('Error searching users:', error);
                        this.searchResults = [];
                    });
            },
            addFriend(userId) {
                // Simulated action to add a friend; replace with actual logic
                console.log(`Adding user with ID ${userId} as a friend.`);
            }
        },
        mounted() {
            // Fetch initial list of friends
            this.fetchFriends();
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

    .search-bar {
        margin-bottom: 20px;
    }

    .search-input {
        width: 100%;
        padding: 10px;
        font-size: 14px;
        border: 1px solid #fbb1a2;
        border-radius: 5px;
        outline: none;
    }

        .search-input::placeholder {
            color: #fbb1a2;
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
</style>
