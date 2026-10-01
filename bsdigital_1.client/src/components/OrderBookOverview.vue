<script setup>
import { ref, reactive, watch, onUnmounted } from 'vue';

import OrderBook from './OrderBook.vue'
import OrderBookLog from './OrderBookLog.vue';
import BuyEstimate from './BuyEstimate.vue';

const ready = ref(false);
const orderBookData = reactive({
  bids: [],
  asks: []
});
const log = ref(null);
const activeIndex = ref(-1);
const refreshActive = ref(false);

async function getData() {
  // Get log data
  var logResponse = await fetch('api/Bitstamp/btceur/orderbook/log');
  console.log('Response from orderbook log:', logResponse);
  if (logResponse.ok) {
    // Get text response from the orderbook endpoint
    const logRaw = await logResponse.arrayBuffer();
    const logText = new TextDecoder().decode(logRaw);
    const logData = JSON.parse(logText);
    log.value = logData;
    console.log('orderbook log:', logData);
  }

  // Get order book data
  var orderBookResponse = await fetch('api/Bitstamp/btceur/orderbook');
  console.log('Response from orderbook:', orderBookResponse);
  if (orderBookResponse.ok) {
    // Get text response from the orderbook endpoint
    const obRaw = await orderBookResponse.arrayBuffer();
    const obText = new TextDecoder().decode(obRaw);
    const obData = JSON.parse(obText);
    orderBookData.bids = obData.bids.slice(0, 10); // Limit to first 10 bids
    orderBookData.asks = obData.asks.slice(10); // Limit to first 10 asks
    console.log('Response from orderbook:', obData);
  }
  ready.value = true;
  console.log('Ready:', ready.value);
}

getData();

function refreshDataClick() {
  ready.value = false;
  refreshActive.value = true;
  //this.$refs.orderBookLogComp.$data.activeIndex = -1; // Reset activeIndex in OrderBookLog component
  getData();
}

// If user click on a log entry, we want to stop the automatic refresh of the order book data. And show the older orderbook.
watch(activeIndex, (newValue) => {
  console.log('Active index changed:', newValue);
  refreshActive.value = false; // Set refreshActive to false when activeIndex changes

  const selectedEntry = log.value[newValue];
  if (selectedEntry) {
    orderBookData.bids = selectedEntry.bids.slice(0, 10); // Limit to first 10 bids
    orderBookData.asks = selectedEntry.asks.slice(10); // Limit to first 10 asks
    console.log('Updated order book data based on active index:', orderBookData);
  }
});

// Set up an interval to refresh the order book data every 3 seconds if refreshActive is true
const intervalId = setInterval(() => {
  if (refreshActive.value) {
    getData();
  }
}, 3000); // Reset refreshActive after 3 seconds

onUnmounted(() => {
  // Clean up any resources or event listeners here if needed
  console.log('OrderBookOverview component is being unmounted.');
  clearInterval(intervalId);
});
</script>

<template>
  <div class="order-book-overview-main">
    <div class="order-book-log-wrapper">
      <button class="refresh-button" :class="{ refreshActive: refreshActive }"
        @click="refreshDataClick">Refresh</button>
      <h3>Log</h3>
      <OrderBookLog v-if="ready" :log="log" ref="orderBookLogComp" @update:activeIndex="activeIndex = $event" />
    </div>
    <div>
      <!-- <div>Ready: {{ ready }}</div> -->
      <OrderBook v-if="ready" symbol="BTC/EUR" :bids="orderBookData.bids" :asks="orderBookData.asks" />
      <!-- <div>data: {{ orderBookData }}</div> -->
    </div>
    <div>
      <BuyEstimate v-if="ready" :asks="orderBookData.asks" />
    </div>
  </div>
</template>

<style scoped>
.order-book-overview-main {
  display: flex;
  flex-direction: row;
  justify-content: space-evenly;
  gap: 2rem;
}

.order-book-log-wrapper {
  margin-top: 2rem;
  min-width: 350px;
  display: flex;
  flex-direction: column;
  align-items: center;
}

.refresh-button {
  margin-bottom: 1rem;
  padding: 0.5rem 1rem;
  background-color: #1c2833;
  color: white;
  border: none;
  border-radius: 4px;
  cursor: pointer;
}

.refresh-button.refreshActive {
  background-color: #0056b3;
}

.refresh-button:hover {
  background-color: #0056b3;
}

h3 {
  margin-bottom: 1rem;
}
</style>