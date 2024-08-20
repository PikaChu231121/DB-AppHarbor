<template>
    <div class="FilterSection">
        <div class="filter">
            <!-- 标签筛选部分 -->
            <div class="tag-filter">
                <h2>关键词</h2>
                <div class="tag"
                     v-for="tag in Tags"
                     :key="tag"
                     :class="{ selected: selectedTag === tag }"
                     @click="selectTag(tag)">
                    <span class="checkbox"></span>
                    <span>{{ tag }}</span>
                </div>
            </div>

            <!-- 价格筛选部分 -->
            <div class="price-filter">
                <h2>价格</h2>
                <label for="priceRange">价格范围: {{ priceRange[0] }} - {{ priceRange[1] }} 元</label>
                <div class="range-container">
                    <input type="range"
                           id="priceRangeMin"
                           v-model="priceRange[0]"
                           :min="minPrice"
                           :max="maxPrice"
                           @input="updatePriceRange" />
                    <input type="range"
                           id="priceRangeMax"
                           v-model="priceRange[1]"
                           :min="minPrice"
                           :max="maxPrice"
                           @input="updatePriceRange" />
                </div>
            </div>
        </div>
    </div>
</template>

<script>
    export default {
        data() {
            return {
                newTag: '',
                Tags: ['全部', '娱乐', '社交', '购物', '健康养生', '办公', '学习'], 
                selectedTag: '全部',
                priceRange: [0, 1000], // 价格范围初始值
                minPrice: 0,
                maxPrice: 1000,
            };
        },
        methods: {
            selectTag(tag) {
                this.selectedTag = tag;
                this.emitTags(); /*向父组件传递标签信息*/
            },
            emitTags() {
                this.$emit('tag-changed', this.selectedTag);
            },

            updatePriceRange() {
                // 触发事件，将价格区间传递给父组件
                this.$emit('price-range-updated', this.priceRange);
            }
        },
    };
</script>

<style scoped>
    .FilterSection {
        height: 100%;
        width: 90%;
        border-radius: 12px;
        background-color: #f5d7c4; /* 背景颜色 */
        text-align: center;
        display: flex;
        flex-direction: column;
        padding: 20px;
        box-shadow: 0 6px 12px rgba(0, 0, 0, 0.15);
        transition: background-color 0.3s ease;
    }

        .FilterSection:hover {
            background-color: #efc2bb; /* 悬停时背景颜色 */
        }

    .filter {
        display: flex;
        flex-direction: column;
        gap: 32px; /* 增大组件之间的间隔 */
        margin: auto;
    }

    .tag-filter {
        display: flex;
        flex-direction: column;
        gap: 10px; /* 增大标签部分的间隔 */
    }

    .tag {
        display: inline-flex;
        align-items: center;
        background-color: #fbb1a2;
        color: #fff;
        padding: 10px 20px; /* 增加内边距 */
        margin-right: 10px; /* 增加标签之间的间隔 */
        border-radius: 20px;
        position: relative;
        font-size: 16px;
        font-family: 'Comic Sans MS', cursive, sans-serif;
        font-weight: 500;
        box-shadow: 0 2px 6px rgba(0, 0, 0, 0.1);
        transition: background-color 0.3s ease, box-shadow 0.3s ease;
    }

        .tag:hover {
            background-color: #f99c8e;
            box-shadow: 0 4px 10px rgba(0, 0, 0, 0.2);
        }

    .tag .checkbox {
        width: 16px;
        height: 16px;
        border-radius: 50%;
        background: #fff;
        border: 2px solid #fbb1a2;
        margin-right: 10px;
        position: relative;
        display: inline-block;
        transition: background 0.3s ease;
    }

        .tag.selected .checkbox {
            background: #fff; /* 选中时的填充颜色 */
            border-color: #f99c8e; /* 选中时边框颜色 */
        }

            .tag.selected .checkbox::after {
                content: '';
                position: absolute;
                top: 50%;
                left: 50%;
                width: 8px;
                height: 8px;
                border-radius: 50%;
                background: #007bff; /* 选中时复选框的内部圆点颜色 */
                transform: translate(-50%, -50%); /* 使圆点居中 */
            }

    .price-filter {
        display: flex;
        flex-direction: column;
        gap: 20px; /* 增大价格部分的间隔 */
    }

        .price-filter label {
            font-size: 18px;
            color: #444;
            font-family: 'Comic Sans MS', cursive, sans-serif;
            font-weight: 500;
        }

    .range-container {
        display: flex;
        flex-direction: column;
        gap: 16px; /* 增大价格范围部分的间隔 */
    }

    input[type="range"] {
        -webkit-appearance: none;
        width: 100%;
        height: 12px;
        background: #f0a8a2;
        border-radius: 5px;
        outline: none;
        margin: 5px 0;
        transition: background 0.3s ease;
    }

        input[type="range"]::-webkit-slider-thumb {
            -webkit-appearance: none;
            width: 24px;
            height: 24px;
            background: #fbb1a2;
            border-radius: 50%;
            cursor: pointer;
            box-shadow: 0 2px 6px rgba(0, 0, 0, 0.2);
            transition: background 0.3s ease;
        }

            input[type="range"]::-webkit-slider-thumb:hover {
                background: #f99c8e;
            }

        input[type="range"]::-moz-range-thumb {
            width: 24px;
            height: 24px;
            background: #fbb1a2;
            border-radius: 50%;
            cursor: pointer;
            box-shadow: 0 2px 6px rgba(0, 0, 0, 0.2);
            transition: background 0.3s ease;
        }

            input[type="range"]::-moz-range-thumb:hover {
                background: #f99c8e;
            }

        input[type="range"]::-ms-thumb {
            width: 24px;
            height: 24px;
            background: #fbb1a2;
            border-radius: 50%;
            cursor: pointer;
            box-shadow: 0 2px 6px rgba(0, 0, 0, 0.2);
            transition: background 0.3s ease;
        }

            input[type="range"]::-ms-thumb:hover {
                background: #f99c8e;
            }

    input[type="text"] {
        width: calc(100% - 32px); /* 输入框宽度减去左右内边距 */
        padding: 16px 20px; /* 增加内边距，使输入框更长 */
        border: 2px solid #fbb1a2;
        border-radius: 20px;
        font-size: 18px; /* 增加字体大小 */
        font-family: 'Comic Sans MS', cursive, sans-serif;
        color: #333;
        background-color: #fff;
        box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
        transition: border-color 0.3s ease, box-shadow 0.3s ease;
        outline: none;
    }

        input[type="text"]::placeholder {
            color: #aaa;
            font-family: 'Comic Sans MS', cursive, sans-serif;
        }

        input[type="text"]:focus {
            border-color: #f99c8e;
            box-shadow: 0 4px 12px rgba(0, 0, 0, 0.2);
        }
</style>
