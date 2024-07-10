<template>
    <div class="FilterSection">

        <div class="filter">
            <!-- 标签筛选部分 -->
            <div class="tag-filter">
                <h2>关键词</h2>
                <div class="tag" v-for="tag in selectedTags" :key="tag">
                    <span>{{ tag }}</span>
                    <button class="close-btn" @click="removeTag(tag)">×</button>
                </div>
                <input v-model="newTag" @keyup.enter="addTag" placeholder="添加关键词" />
            </div>

            <!-- 价格筛选部分 -->
            <div class="price-filter">
                <h2>价格</h2>
                <label for="priceRange">价格范围: {{ priceRange[0] }} - {{ priceRange[1] }} 元</label>
                <input type="range"
                       id="priceRangeMin"
                       v-model="priceRange[0]"
                       :min="minPrice"
                       :max="maxPrice"
                       @input="applyFilters" />
                <input type="range"
                       id="priceRangeMax"
                       v-model="priceRange[1]"
                       :min="minPrice"
                       :max="maxPrice"
                       @input="applyFilters" />
            </div>
        </div>

        <div class="app-list">
            <div v-for="app in filteredApps" :key="app.id" class="app-item">
                <h2>{{ app.name }}</h2>
                <p>类别: {{ app.tags.join(', ') }}</p>
                <p>价格: {{ app.price }} 元</p>
            </div>
        </div>
    </div>
</template>

<script>
    export default {
        data() {
            return {
                newTag: '',
                selectedTags: [], // 默认选中的标签
                priceRange: [0, 100], // 价格范围初始值
                minPrice: 0,
                maxPrice: 100,
                apps: [
                    { id: 1, name: '应用A', tags: ['社交'], price: 0 },
                    { id: 2, name: '应用B', tags: ['工具'], price: 50 },
                    { id: 3, name: '应用C', tags: ['生活'], price: 30 },
                    // 其他应用数据
                ],
                filteredApps: [],
            };
        },
        methods: {
            addTag() {
                if (this.newTag && !this.selectedTags.includes(this.newTag)) {
                    this.selectedTags.push(this.newTag);
                    this.newTag = '';
                    this.applyFilters();
                }
            },
            removeTag(tag) {
                this.selectedTags = this.selectedTags.filter(t => t !== tag);
                this.applyFilters();
            },
            applyFilters() {
                this.filteredApps = this.apps.filter(app => {
                    const matchesTags = this.selectedTags.length === 0 || this.selectedTags.some(tag => app.tags.includes(tag));
                    const matchesPrice = app.price >= this.priceRange[0] && app.price <= this.priceRange[1];
                    return matchesTags && matchesPrice;
                });
            },
        },
        mounted() {
            this.filteredApps = this.apps; // 初始化显示所有应用
        },
    };
</script>

<style scoped>
    .filter {
        display: flex;
        flex-direction: column;
        gap: 20px;
        width:150px;
        margin-bottom: 20px;
        margin:auto;
    }

    .tag-filter {
        display: flex;
        flex-direction: column;
        gap: 10px;

    }

    .tag {
        display: inline-block;
        background-color: #f0f0f0;
        padding: 5px 10px;
        margin-right: 5px;
        border-radius: 5px;
        position: relative;
        background-color: #fbb1a2;
        padding-right: 20px; /* 增加右边距，给关闭按钮留空间 */
    }
    .FilterSection {
        height: 90%;
        width: 90%;
        border-radius: 10px;
        background-color: antiquewhite;
        text-align: center;
    }
    .close-btn {
        position: absolute;
        top: 2px;
        right: 2px;
        background: transparent;
        border: none;
        cursor: pointer;
        font-size: 16px;
        color: #888;
        padding: 0;
        line-height: 1;
    }

        .close-btn:hover {
            color: #333;
        }

    .price-filter {
        display: flex;
        flex-direction: column;
        gap: 10px;
    }

    .app-item {
        border: 1px solid #ccc;
        padding: 10px;
        margin-bottom: 10px;
    }
</style>
