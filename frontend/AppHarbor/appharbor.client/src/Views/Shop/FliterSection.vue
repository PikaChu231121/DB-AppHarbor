<template>
    <div class="FilterSection">
        <div class="filter">
            <!-- 标签筛选部分 -->
            <div class="tag-filter">
                <h2 class="key">关键词</h2>
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
                <h2 class="price">价格</h2>
                <label for="priceRange">{{ priceRange[0] }} - {{ priceRange[1] }} 元</label>
                <div class="range-container">
                    <input type="range"
                           id="priceRangeMin"
                           v-model="priceRange[0]"
                           :min="minPrice"
                           :max="maxPrice"
                           @input="updatePriceRange" />
                    <!--<input type="range"
                           id="priceRangeMax"
                           v-model="priceRange[1]"
                           :min="minPrice"
                           :max="maxPrice"
                           @input="updatePriceRange" />-->
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
        border-radius: 12px;
        background-color: #FCF8F8; /* 背景颜色 */
        display: flex;
        padding: 5px;
        transition: background-color 1.0s ease, transform 0.3s;
    }

        .FilterSection:hover {
            background-color: white; /* 悬停时背景颜色 */
            transform: scale(1.01);
            box-shadow: 2px 3px 3px rgba(0, 0, 0, 0.05);
            border-right: 4px solid darksalmon; /* 固定右边边框颜色 */
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
        background-color: white;
        color: #67575a;
        padding: 10px 20px; /* 增加内边距 */
        margin-right: 10px; /* 增加标签之间的间隔 */
        border-radius: 20px;
        position: relative;
        transition: background-color 0.3s ease, box-shadow 0.3s ease;
        border: 2px solid #FFE7EA;
    }

    
    .FilterSection {
        border-radius: 12px;
        background-color: #FCF8F8;
        display: flex;
        padding: 5px;
        transition: background-color 0.3s ease;
        transition: transform 0.3s;
    }

    .FilterSection:hover {
        background-color: white;
        transform: scale(1.05);
        box-shadow: 2px 3px 3px rgba(0, 0, 0, 0.05);
    }

    .filter {
        display: flex;
        flex-direction: column;
        gap: 32px;
        margin: auto;
    }

    .tag-filter {
        display: flex;
        flex-direction: column;
        gap: 10px;
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
        background: #fff;
        border-color: #f99c8e;
    }

    .tag.selected .checkbox::after {
        content: '';
        position: absolute;
        top: 50%;
        left: 50%;
        width: 8px;
        height: 8px;
        border-radius: 50%;
        background: pink;
        transform: translate(-50%, -50%);
    }

        /* 设置tag内span的文字为粗体 */
        .tag span {
            font-weight: bold;
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
                background: pink; /* 选中时复选框的内部圆点颜色 */
                transform: translate(-50%, -50%); /* 使圆点居中 */
            }

    .price-filter {
        display: flex;
        flex-direction: column;
        gap: 20px; /* 增大价格部分的间隔 */
    }

        .price-filter label {
            font-size: 18px;
            color: palevioletred;
            text-align: center;
            font-family: 'PingFang SC', 'Microsoft YaHei', 'Source Han Sans', 'Noto Sans CJK SC', 'Hiragino Sans GB', sans-serif !important;
            font-weight: bold !important; /* 设置粗体 */
            //font-weight: 500;
            border-radius: 5px;
            border: 1px solid whitesmoke;
        }

    .range-container {
        display: flex;
        flex-direction: column;
        gap: 16px;
    }

    input[type="range"] {
        appearance: none;
        width: 100%;
        height: 8px; /* Thickness of the slider track */
        background: #fbb1a2; /* Track color */
        border-radius: 5px;
        outline: none;
        opacity: 0.9;
        transition: opacity 0.2s;
    }

    input[type="range"]::-webkit-slider-thumb {
        appearance: none;
        width: 20px;
        height: 20px;
        border-radius: 50%;
        background: #f99c8e; /* Thumb color */
        cursor: pointer;
        transition: background-color 0.3s ease;
    }

    .key {
        font-weight: bold !important; /* 设置粗体 */
    }

    .price {
        font-weight: bold !important; /* 设置粗体 */
    }

</style>
