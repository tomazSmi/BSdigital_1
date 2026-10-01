<template>
  <div class="order-book">
    <svg :viewBox="`0 0 ${width} ${height}`"
         :width="width"
         :height="height"
         role="img"
         aria-label="Stock exchange order book">
      <!-- Background -->
      <rect x="0"
            y="0"
            :width="width"
            :height="height"
            rx="8"
            fill="#0f172a" />

      <!-- Header -->
      <text :x="padding"
            y="28"
            fill="#94a3b8"
            font-size="12"
            font-weight="600">
        {{ title }}
      </text>

      <text :x="width - padding"
            y="28"
            text-anchor="end"
            fill="#64748b"
            font-size="11">
        {{ symbol }}
      </text>

      <!-- Column headings -->
      <g :transform="`translate(${padding}, 50)`">
        <text fill="#64748b" font-size="10">PRICE</text>

        <text :x="priceWidth"
              text-anchor="middle"
              fill="#64748b"
              font-size="10">
          SIZE
        </text>

        <text :x="width - padding * 2 - priceWidth"
              text-anchor="end"
              fill="#64748b"
              font-size="10">
          TOTAL
        </text>
      </g>

      <!-- ASK rows -->
      <g v-for="(order, index) in asks.slice().reverse()"
         :key="`ask-${order.price}`"
         :transform="`translate(0, ${askStartY + index * rowHeight})`">
        <!-- Depth bar -->
        <rect :x="padding"
              :y="3"
              :width="depthWidth(order.total)"
              :height="rowHeight - 6"
              fill="#ef4444"
              fill-opacity="0.12" />

        <text :x="padding"
              :y="rowTextY"
              fill="#f87171"
              font-size="11"
              font-family="monospace">
          {{ formatPrice(order.price) }}
        </text>

        <text :x="padding + priceWidth"
              :y="rowTextY"
              text-anchor="middle"
              fill="#e2e8f0"
              font-size="11"
              font-family="monospace">
          {{ formatSize(order.amount) }}
        </text>

        <text :x="width - padding"
              :y="rowTextY"
              text-anchor="end"
              fill="#94a3b8"
              font-size="11"
              font-family="monospace">
          {{ formatSize(order.total) }}
        </text>
      </g>

      <!-- Spread -->
      <g :transform="`translate(0, ${spreadY})`">
        <line :x1="padding"
              :x2="width - padding"
              y1="0"
              y2="0"
              stroke="#334155"
              stroke-width="1" />

        <rect :x="padding"
              y="8"
              :width="width - padding * 2"
              :height="spreadHeight - 16"
              rx="4"
              fill="#1e293b" />

        <text :x="padding + 8"
              :y="25"
              fill="#64748b"
              font-size="10"
              font-weight="600">
          SPREAD
        </text>

        <text :x="width / 2"
              :y="25"
              text-anchor="middle"
              fill="#f8fafc"
              font-size="11"
              font-family="monospace">
          {{ formatPrice(spread) }}
        </text>

        <text :x="width - padding - 8"
              :y="25"
              text-anchor="end"
              fill="#64748b"
              font-size="10">
          {{ spreadBps.toFixed(1) }} bps
        </text>
      </g>

      <!-- BID rows -->
      <g v-for="(order, index) in bids"
         :key="`bid-${order.price}`"
         :transform="`translate(0, ${bidStartY + index * rowHeight})`">
        <!-- Depth bar -->
        <rect :x="padding"
              :y="3"
              :width="depthWidth(order.total)"
              :height="rowHeight - 6"
              fill="#22c55e"
              fill-opacity="0.12" />

        <text :x="padding"
              :y="rowTextY"
              fill="#4ade80"
              font-size="11"
              font-family="monospace">
          {{ formatPrice(order.price) }}
        </text>

        <text :x="padding + priceWidth"
              :y="rowTextY"
              text-anchor="middle"
              fill="#e2e8f0"
              font-size="11"
              font-family="monospace">
          {{ formatSize(order.amount) }}
        </text>

        <text :x="width - padding"
              :y="rowTextY"
              text-anchor="end"
              fill="#94a3b8"
              font-size="11"
              font-family="monospace">
          {{ formatSize(order.total) }}
        </text>
      </g>

      <!-- Mid price -->
      <g :transform="`translate(0, ${midY})`">
        <line :x1="padding"
              :x2="width - padding"
              y1="0"
              y2="0"
              stroke="#f59e0b"
              stroke-width="1"
              stroke-dasharray="4 3"
              opacity="0.75" />

        <rect :x="width - padding - 74"
              y="-10"
              width="74"
              height="20"
              rx="4"
              fill="#f59e0b" />

        <text :x="width - padding - 37"
              y="4"
              text-anchor="middle"
              fill="#111827"
              font-size="10"
              font-weight="700"
              font-family="monospace">
          {{ formatPrice(midPrice) }}
        </text>
      </g>
    </svg>
  </div>
</template>

<script setup lang="js">
  import { computed } from 'vue'

  const title = 'Order Book'
  const props = defineProps({
    symbol: {
      type: String,
      default: '' //'AAPL'
    },

    bids: {
      type: Array,
      default: () => [
        // { price: 219.31, size: 420 },
        // { price: 219.30, size: 180 },
        // { price: 219.29, size: 735 },
        // { price: 219.28, size: 310 },
        // { price: 219.27, size: 1250 },
        // { price: 219.26, size: 490 },
        // { price: 219.25, size: 820 }
      ]
    },

    asks: {
      type: Array,
      default: () => [
        // { price: 219.33, size: 260 },
        // { price: 219.34, size: 540 },
        // { price: 219.35, size: 185 },
        // { price: 219.36, size: 910 },
        // { price: 219.37, size: 630 },
        // { price: 219.38, size: 340 },
        // { price: 219.39, size: 1180 }
      ]
    },

    width: {
      type: Number,
      default: 500
    },

    height: {
      type: Number,
      default: 700
    }
  })

  const padding = 18
  const rowHeight = 28
  const spreadHeight = 38

  const priceWidth = 105

  const rowTextY = 18

  const askStartY = 72

  const spreadY =
    askStartY +
    props.asks.length * rowHeight +
    4

  const bidStartY =
    spreadY +
    spreadHeight +
    4

  const midY = spreadY + spreadHeight / 2

  const maxDepth = computed(() => {
    const all = [
      ...props.bids,
      ...props.asks
    ]

    return Math.max(
      1,
      ...all.map(order => cumulativeTotal(
        order,
        props.bids,
        props.asks
      ))
    )
  })

  function cumulativeTotal(order, side, otherSide) {
    const orders = side === props.bids
      ? side.filter(x => x.price >= Number(order.price))
      : side.filter(x => x.price <= Number(order.price))

    return orders.reduce((sum, x) => sum + Number(x.amount), 0)
  }

  function withTotals(orders, isBid) {
    let total = 0

    return orders.map(order => {
      total += Number(order.amount)

      return {
        ...order,
        total
      }
    })
  }

  const bids = computed(() => {
    return withTotals(
      [...props.bids].sort((a, b) => b.price - a.price),
      true
    )
  })

  const asks = computed(() => {
    return withTotals(
      [...props.asks].sort((a, b) => a.price - b.price),
      false
    )
  })

  const bestBid = computed(() => bids.value[0]?.price ?? 0)
  const bestAsk = computed(() => asks.value[0]?.price ?? 0)

  const spread = computed(() =>
    Math.max(0, Number(bestAsk.value) - Number(bestBid.value))
  )

  const midPrice = computed(() =>
    (Number(bestAsk.value) + Number(bestBid.value)) / 2
  )

  const spreadBps = computed(() => {
    if (!midPrice.value) return 0
    return (spread.value / midPrice.value) /* * 10_000*/
  })

  function depthWidth(total) {
    const maxBarWidth = props.width - padding * 2

    return (total / maxCumulative.value) * maxBarWidth
  }

  const maxCumulative = computed(() => {
    const totals = [
      ...bids.value.map(x => x.total),
      ...asks.value.map(x => x.total)
    ]

    return Math.max(1, ...totals)
  })

  function formatPrice(value) {
    return Number(value).toFixed(2)
  }

  function formatSize(value) {
    return new Intl.NumberFormat('en-US', {
      maximumFractionDigits: 10
    }).format(value)
  }</script>

<style scoped>
  .order-book {
    width: 100%;
    max-width: 420px;
  }

  svg {
    display: block;
    width: 100%;
    height: auto;
    overflow: visible;
  }
</style>
