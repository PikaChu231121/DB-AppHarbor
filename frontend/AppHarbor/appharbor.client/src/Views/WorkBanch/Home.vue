<template>
    <div class="home-container">
        <header class="header">
            <div class="header-left">库</div>
            <div class="header-right">
                <img class="user-avatar" :src="userAvatar" alt="用户头像">
                <div class="user-info">
                    <div class="user-name">{{ userName }}</div>
                    <div class="user-id">ID: {{ userId }}</div>
                </div>
            </div>
        </header>
        <main class="app-library">
            <div v-for="app in apps"
                 :key="app.id"
                 class="app-group"
                 @mouseenter="showDetails(app.id)"
                 @mouseleave="hideDetails">
                <div class="app-icon-container">
                    <img :src="app.icon" alt="应用图标" class="app-icon">
                </div>
                <div class="app-name">{{ app.name }}</div>
                <button @click="downloadApp(app.id)" class="download-button">下载</button>
                <transition name="fade">
                    <div v-if="hoveredApp === app.id" class="app-details-group">
                        <div class="app-details-header">{{ app.name }}</div>
                        <div class="app-details-body">
                            <img :src="app.icon" alt="应用图标" class="app-details-icon">
                            <div class="app-details-info">
                                <div>应用ID: {{ app.id }}</div>
                                <div>版本: {{ app.version }}</div>
                                <div>发行商: {{ app.publisher }}</div>
                                <div>介绍: {{ app.description }}</div>
                            </div>
                        </div>
                    </div>
                </transition>
            </div>
        </main>
    </div>
</template>

<script>
export default {
  data() {
    return {
      userAvatar: 'path/to/avatar.jpg',
      userName: '丁真珍珠',
      userId: '114514',
      hoveredApp: null,
      apps: [
        {
          id: '114',
          name: 'Office tool plus',
          icon: 'path/to/icon.jpg',
          version: '1.0.1',
          publisher: 'xxx',
          description: '应用介绍...'
        },
        // 添加更多应用
      ]
    };
  },
  methods: {
    showDetails(appId) {
      this.hoveredApp = appId;
    },
    hideDetails() {
      this.hoveredApp = null;
    },
    downloadApp(appId) {
      // 执行下载操作
      const appIndex = this.apps.findIndex(app => app.id === appId);
      if (appIndex !== -1) {
        this.$set(this.apps, appIndex, {
          ...this.apps[appIndex],
          downloaded: true
        });
      }
    }
  }
};
</script>

<style scoped>
    @import url('https://fonts.googleapis.com/css2?family=Fredoka+One&family=Poppins:wght@400&display=swap');
    @import url('https://fonts.googleapis.com/css2?family=Baloo+2&family=Quicksand:wght@400;700&family=Fredoka+One&display=swap');

    .header {
        display: flex;
        justify-content: space-between;
        align-items: center;
        padding: 16px;
        background-color: #fff;
        border-bottom: 1px solid #ccc;
    }

    .header-left {
        font-family: 'Fredoka One', cursive;
        font-size: 24px;
    }

    .header-right {
        display: flex;
        align-items: center;
    }

    .user-avatar {
        width: 40px;
        height: 40px;
        border-radius: 50%;
        margin-right: 8px;
    }

    .user-info {
        display: flex;
        flex-direction: column;
    }

    .user-name {
        font-family: 'Poppins', sans-serif;
        font-weight: 700;
    }

    .user-id {
        font-family: 'Poppins', sans-serif;
        font-size: 12px;
        color: #888;
    }

    .app-library {
        display: flex;
        flex-wrap: wrap;
        padding: 16px;
    }

    .app-group {
        position: relative;
        width: 200px;
        height: 100px;
        margin: 8px;
        padding: 16px;
        border: 1px solid #ccc;
        border-radius: 8px;
        display: flex;
        flex-direction: column;
        align-items: center;
        justify-content: center;
        background-color: #fff;
        cursor: pointer;
        transition: background-color 0.3s;
    }

        .app-group:hover {
            background-color: #f4f4f4;
        }

    .app-icon-container {
        width: 40px;
        height: 40px;
        margin-bottom: 8px;
    }

    .app-icon {
        width: 100%;
        height: 100%;
    }

    .app-name {
        font-family: 'Poppins', sans-serif;
        font-size: 14px;
        margin-bottom: 8px;
    }

    .download-button {
        padding: 4px 8px;
        font-family: 'Poppins', sans-serif;
        font-size: 12px;
        color: #fff;
        background-color: #007bff;
        border: none;
        border-radius: 4px;
        cursor: pointer;
        transition: background-color 0.3s;
    }

        .download-button:hover {
            background-color: #0056b3;
        }

    .downloaded {
        background-color: #d4edda;
    }

    .app-details-group {
        position: absolute;
        top: 100%;
        left: 50%;
        transform: translateX(-50%);
        width: 300px;
        padding: 16px;
        background-color: #fff;
        border: 1px solid #ccc;
        border-radius: 8px;
        box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
        z-index: 10;
    }

    .app-details-header {
        font-family: 'Fredoka One', cursive;
        font-size: 18px;
        margin-bottom: 8px;
    }

    .app-details-body {
        display: flex;
        align-items: center;
    }

    .app-details-icon {
        width: 60px;
        height: 60px;
        margin-right: 16px;
    }

    .app-details-info {
        font-family: 'Poppins', sans-serif;
        font-size: 12px;
    }

    .fade-enter-active, .fade-leave-active {
        transition: opacity 0.3s;
    }

    .fade-enter, .fade-leave-to {
        opacity: 0;
    }
</style>
