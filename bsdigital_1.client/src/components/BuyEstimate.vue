<template>
    <div class="buy-estimate">
        <h3>Buy Estimate</h3>
        <div v-if="asks.length > 0">
            <span> Amount: </span> <input type="number" v-model.number="amount" min="0" step="0.01" />
            <div><span> Estimated Cost: {{ calculateEstimate(amount, asks).toFixed(2) }} </span></div>
            
        </div>
    </div>
</template>



<script setup lang="js">
import { ref, watch, defineEmits } from 'vue'

const emit = defineEmits(['update:activeIndex']);
const props = defineProps({
    asks: {
        type: Array,
        required: true,
        default: () => [],
    },
})

const amount = ref(0);

function calculateEstimate(amount, asks) {
    let totalCost = 0;
    let remainingAmount = amount;

    for (const ask of asks) {
        if (remainingAmount <= 0) break;

        const askAmount = ask.amount;
        const askPrice = ask.price;

        if (remainingAmount >= askAmount) {
            totalCost += askAmount * askPrice;
            remainingAmount -= askAmount;
        } else {
            totalCost += remainingAmount * askPrice;
            remainingAmount = 0;
        }
    }

    return totalCost;
}
</script>

<style scoped>
.buy-estimate {
    padding: 1rem;
    border-radius: 4px;
    background-color: #1c2833;
    color: #f8f9fa;
    margin-top: 1rem;
}
</style>