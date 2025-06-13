Vue.component("standards-list", {
  props: ["standards"],
  data() {
    return {
      currentPage: 1,
      itemsPerPage: 10,
      searchQuery: ""
    };
  },
  computed: {
    paginatedStandards() {
      const start = (this.currentPage - 1) * this.itemsPerPage;
      const end = start + this.itemsPerPage;
      return this.standards.slice(start, end);
    },
    totalPages() {
      return Math.ceil(this.standards.length / this.itemsPerPage);
    },
  },
  methods: {
    changePage(page) {
      if (page >= 1 && page <= this.totalPages) {
        this.currentPage = page;
      }
    },
    updateSearch(event) {
      this.searchQuery = event.target.value;
      this.$emit("search", this.searchQuery);
      this.currentPage = 1; // Reset to first page when searching
    }
  },
  template: `
    <div>
      <div class="page-header">
        <h3><i class="fa fa-list-alt"></i> Danh sách tiêu chuẩn đánh giá</h3>         
        <button class="btn btn-primary" @click="$emit('create')">
          <i class="fa fa-plus"></i> Tạo tiêu chuẩn mới
        </button>
      </div>
      <div class="row" style="margin-bottom: 20px">
        <div class="col-md-4">
          <div class="input-group">
            <input 
              type="text" 
              class="form-control" 
              placeholder="Tìm kiếm tiêu chuẩn..." 
              v-model="searchQuery"
              @input="updateSearch"
            >
            <span class="input-group-addon">
              <i class="fa fa-search"></i>
            </span>
          </div>
        </div>
      </div>
      <div class="row">
        <standard-card
          v-for="standard in standards"
          :key="standard.id"
          :standard="standard"
          @remove="$emit('remove', standard)"
          @edit="$emit('edit', standard)"
        />
      </div>
      <div class="text-center" v-if="totalPages > -1">
        <ul class="pagination">
          <li :class="{ disabled: currentPage === 1 }">
            <a href="#" @click.prevent="changePage(currentPage - 1)">«</a>
          </li>
          <li v-for="page in totalPages" :key="page" :class="{ active: currentPage === page }">
            <a href="#" @click.prevent="changePage(page)">{{ page }}</a>
          </li>
          <li :class="{ disabled: currentPage === totalPages }">
            <a href="#" @click.prevent="changePage(currentPage + 1)">»</a>
          </li>
        </ul>
      </div>
    </div>
  `
});