<template>
    <div class="order-book-log">
        <ul>
            <li v-for="(entry, index) in log" :key="index" class="log-item" :class="{ active: index === activeIndexVar }" @click="activeIndexVar = index">
                <span class="timestamp">{{ formatDate(entry.timestamp) }}</span>
                <span class="price">{{ entry.bids[0]?.price }}</span>
            </li>
        </ul>
    </div>
</template>



<script setup lang="js">
import { ref, watch, defineEmits } from 'vue'

const emit = defineEmits(['update:activeIndex']);
const props = defineProps({
    log: {
        type: Array,
        required: true
    },
    activeIndex: {
        type: Number,
        default: -1
    }
})

const activeIndexVar = ref(props.activeIndex);

watch(() => props.activeIndex, () => {
    activeIndexVar.value = props.activeIndex;
})
watch(activeIndexVar, (newValue) => {
    emit('update:activeIndex', newValue);
});


function formatDate(timestamp) {
    return new Date(timestamp).toLocaleString()
}
</script>

<style scoped>
    .order-book-log {
        max-height: 400px;
        overflow-y: auto;
        padding: 1rem;
        border-radius: 4px;
    }
    .log-item {
        display: flex;
        justify-content: space-between;
        padding: 0.5rem;
        border-bottom: 1px solid #e5e7eb;
        background-color: #1c2833;
        color: #f8f9fa;
        border-radius: 4px;
        margin-bottom: 0.5rem;
        cursor: pointer;
    }
    .log-item.active {
        background-color: #007bff;
        color: #fff;
    }
</style>