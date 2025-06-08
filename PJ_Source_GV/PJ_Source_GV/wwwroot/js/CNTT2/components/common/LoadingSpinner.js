Vue.component("loading-spinner", {
  props: {
    message: {
      type: String,
      default: "Đang tải..."
    },
    size: {
      type: String,
      default: "medium", // small, medium, large
      validator: value => ["small", "medium", "large"].includes(value)
    },
    overlay: {
      type: Boolean,
      default: true
    },
    type: {
      type: String,
      default: "dots", // dots, pulse, bars, circle, ripple
      validator: value => ["dots", "pulse", "bars", "circle", "ripple"].includes(value)
    },
    color: {
      type: String,
      default: "gradient" // gradient, blue, green, purple, orange
    }
  },
  computed: {
    containerClasses() {
      return {
        'loading-container': true,
        'loading-container--overlay': this.overlay
      };
    },
    spinnerClasses() {
      return {
        [`loading-${this.type}`]: true,
        [`loading-${this.type}--${this.size}`]: true,
        [`loading-${this.type}--${this.color}`]: true
      };
    }
  },
  template: `
    <div :class="containerClasses" v-show="true">
      <div class="loading-backdrop" v-if="overlay"></div>
      <div class="loading-content">
        <!-- Dots Spinner -->
        <div v-if="type === 'dots'" :class="spinnerClasses">
          <div class="dot"></div>
          <div class="dot"></div>
          <div class="dot"></div>
        </div>
        
        <!-- Pulse Spinner -->
        <div v-else-if="type === 'pulse'" :class="spinnerClasses">
          <div class="pulse-ring"></div>
          <div class="pulse-ring"></div>
          <div class="pulse-ring"></div>
        </div>
        
        <!-- Bars Spinner -->
        <div v-else-if="type === 'bars'" :class="spinnerClasses">
          <div class="bar"></div>
          <div class="bar"></div>
          <div class="bar"></div>
          <div class="bar"></div>
          <div class="bar"></div>
        </div>
        
        <!-- Circle Spinner -->
        <div v-else-if="type === 'circle'" :class="spinnerClasses">
          <div class="circle-path"></div>
        </div>
        
        <!-- Ripple Spinner -->
        <div v-else-if="type === 'ripple'" :class="spinnerClasses">
          <div class="ripple-wave"></div>
          <div class="ripple-wave"></div>
        </div>
        
        <div class="loading-message" v-if="message">
          <div class="message-text">{{ message }}</div>
          <div class="message-dots">
            <span></span>
            <span></span>
            <span></span>
          </div>
        </div>
      </div>
    </div>
  `,
  style: `
    <style scoped>

    </style>
  `
});