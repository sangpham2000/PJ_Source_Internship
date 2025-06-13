Vue.component("evaluations-list", {
  template: `
    <div>
      <div class="page-header">
        <h3><i class="fa fa-list-alt"></i> Danh sách đợt đánh giá</h3>
        <button
          class="btn btn-primary nav-button"
          @click="$emit('create')"
        >
          <i class="fa fa-plus"></i> Tạo đợt đánh giá mới
        </button>
      </div>
      <div class="row" style="margin-bottom: 20px">
        <div class="col-md-4">
          <div class="input-group">
            <input 
              type="text" 
              class="form-control" 
              placeholder="Tìm kiếm đợt đánh giá..." 
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
        <div class="col-md-4 col-sm-6" v-for="evaluation in evaluations" :key="evaluation.id">
          <div class="panel panel-primary">
            <div class="panel-heading">
              <h3 class="panel-title">{{ evaluation.name }}</h3>
            </div>
            <div class="panel-body">
              <p><strong>ID:</strong> DG{{ evaluation.id }}</p>
              <strong>Thời gian:</strong> {{ formatDate(evaluation.startDate) }} - {{ formatDate(evaluation.endDate) }}
            </div>
            <div class="panel-footer">
              <button class="btn btn-sm btn-primary" @click="onEdit(evaluation)">
                Xem dữ liệu đánh giá
              </button>
              <button class="btn btn-sm btn-danger pull-right" @click="showConfirmDelete(evaluation.name)"><i class="fa fa-trash"></i> Xóa</button>
            </div>
          </div>
        </div>
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
      <modal-confirm-delete
        v-if="showConfirmDeleteModal"
        :name="confirmDeleteName"
        @cancel="showConfirmDeleteModal = false"
        @confirm="confirmDeleteRow"
      />
    </div>
    `,
  data() {
    return {
      evaluations: [],
      showConfirmDeleteModal: false,
      confirmDeleteName: "",
      // Pagination properties
      currentPageValue: 1,
      pageSize: 10,
      totalCount: 0,
      // totalPages: 0,
      hasNextPage: false,
      hasPreviousPage: false,
      // Filter properties
      searchQuery: "",
      // Debounce timer for search
      searchTimer: null,

      currentPage: 1,
      itemsPerPage: 10,
    };
  },
  created() {
    this.fetchEvaluations();
  },
  computed: {
    paginatedStandards() {
      const start = (this.currentPage - 1) * this.itemsPerPage;
      const end = start + this.itemsPerPage;
      return this.evaluations.slice(start, end);
    },
    totalPages() {
      return Math.ceil(this.evaluations.length / this.itemsPerPage);
    },
  },
  methods: {
    async apiRequest(url, method = "GET", data = null) {
      const options = {
        method,
        headers: {
          "Content-Type": "application/json",
        },
      };
      if (data) {
        options.body = JSON.stringify(data);
      }
      const response = await fetch(url, options);
      if (!response.ok) {
        const errorText = await response.text();
        throw new Error(`HTTP ${response.status}: ${errorText}`);
      }
      if (response.status === 204 || response.status === 201) {
        return null;
      } else {
        return response.json();
      }
    },
    updateSearch() {
      clearTimeout(this.searchTimer);
      this.searchTimer = setTimeout(() => {
        this.fetchEvaluations(this.currentPageValue, this.itemsPerPage, this.searchQuery);
      }, 500);
    },
    async fetchEvaluations(page = 1, pageSize = 10, name = "") {
      this.showLoading = true;
      try {
        const params = new URLSearchParams({
          page: page.toString(),
          pageSize: pageSize.toString(),
        });
        if (name && name.trim()) {
          params.append("name", name.trim());
        }
        const response = await this.apiRequest(
            `http://localhost:28635/API/evaluation?${params.toString()}`,
            "GET"
        );
        this.evaluations = [...response.items];
        // this.currentPageValue = response.page;
        // this.pageSize = response.pageSize;
        // this.totalCount = response.totalCount;
        // this.totalPages = response.totalPages;
        // this.hasNextPage = response.hasNextPage;
        // this.hasPreviousPage = response.hasPreviousPage;
        this.showLoading = false;
      } catch (err) {
        this.showLoading = false;
        // this.evaluations = [];
        this.totalCount = 0;
        // this.totalPages = 0;
        console.error("Lỗi khi lấy evaluations:", err);
      }
    },
    formatDate(date) {
      return new Date(date).toLocaleDateString("vi-VN", {
        year: "numeric",
        month: "2-digit",
        day: "2-digit",
      });
    },
    onEdit(item) {
      console.log(item);
      this.$emit("edit", item);
    },
    onDelete(id) {
      console.log(id);
      this.$emit("delete", id);
    },
    showConfirmDelete(name) {
      this.confirmDeleteName = name;
      this.showConfirmDeleteModal = true;
    },
    confirmDeleteRow() {
      console.log(id);
      this.$emit("delete", id);
      toastr.success("Đã xóa dòng!");
      this.showConfirmDeleteModal = false;
    },
    changePage(page) {
      if (page >= 1 && page <= this.totalPages) {
        this.currentPage = page;
      }
    },
  },
});
