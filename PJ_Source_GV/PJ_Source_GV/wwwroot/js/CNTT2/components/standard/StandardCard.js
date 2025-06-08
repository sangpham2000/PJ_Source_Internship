Vue.component("standard-card", {
  props: ["standard"],
  data() {
    return {
      showConfirmDeleteModal: false,
      confirmDeleteName: "",
    };
  },
  methods: {
    confirmDeleteRow() {
      this.$emit("remove", this.standard);
    },
    formatDate(date) {
      return new Date(date).toLocaleDateString("vi-VN", {
        year: "numeric",
        month: "2-digit",
        day: "2-digit",
      });
    },
    showConfirmDelete() {
      this.confirmDeleteName = this.standard.name;
      this.showConfirmDeleteModal = true;
    },
  },
  template: `
    <div>
      <div class="col-md-4 col-sm-6">
        <div class="panel panel-primary">
          <div class="panel-heading">
            <h3 class="panel-title">{{ standard.name }}</h3>
          </div>
          <div class="panel-body">
            <p><strong>Mã:</strong> TC{{ standard.id }}</p>
            <p><strong>Số bảng:</strong> {{ standard.tables.length }}</p>
            <p><strong>Ngày tạo:</strong> {{ formatDate(standard.createdAt) }}</p>
          </div>
          <div class="panel-footer">
            <button class="btn btn-sm btn-primary" @click="$emit('edit', standard)">
              <i class="fa fa-pencil"></i> Chỉnh sửa
            </button>
            <button class="btn btn-sm btn-danger pull-right" @click="showConfirmDelete">
              <i class="fa fa-trash"></i> Xóa
            </button>
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
});
