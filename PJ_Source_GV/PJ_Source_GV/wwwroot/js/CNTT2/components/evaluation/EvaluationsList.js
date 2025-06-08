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
      <modal-confirm-delete
        v-if="showConfirmDeleteModal"
        :name="confirmDeleteName"
        @cancel="showConfirmDeleteModal = false"
        @confirm="confirmDeleteRow"
      />
    </div>
    `,
  props: ["evaluations"],
  data() {
    return {
      showConfirmDeleteModal: false,
      confirmDeleteName: "",
    };
  },
  methods: {
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
  },
});
