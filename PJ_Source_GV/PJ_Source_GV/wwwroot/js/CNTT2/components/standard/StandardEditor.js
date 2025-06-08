Vue.component("standard-editor", {
  props: ["standard"],
  methods: {
    saveStandard() {
      this.$emit("save", this.standard);
    },
  },
  template: `
    <div>
      <div class="page-header">
        <h3><i class="fa fa-pencil"></i> Chỉnh sửa tiêu chuẩn: {{ standard.name }}</h3>
        <div class="pull-right">
          <button class="btn btn-success" @click="saveStandard">
            <i class="fa fa-save"></i> Lưu thay đổi
          </button>
          <button class="btn btn-default" @click="$emit('back')" style="margin-left:5px;">
            <i class="fa fa-arrow-left"></i> Quay lại
          </button>
        </div>

      </div>
      <div class="form-group">
        <label for="standard-name">Tên tiêu chuẩn:</label>
        <input
          id="standard-name"
          class="form-control"
          v-model="standard.name"
          placeholder="Nhập tên tiêu chuẩn..."
        />
      </div>
      <div class="panel panel-default">
        <div class="panel-heading">
          <div class="pull-right">
            <button class="btn btn-sm btn-primary" @click="$emit('add-table')">
              <i class="fa fa-plus"></i> Thêm bảng
            </button>
          </div>
          <h4><i class="fa fa-table"></i> Danh sách bảng</h4>
        </div>
        <div class="panel-body">
          <div v-if="standard.tables.length === 0" class="text-center text-muted">
            <p>Chưa có bảng nào. Vui lòng thêm bảng mới.</p>
          </div>
          <table-editor
            v-for="(table, tableIndex) in standard.tables"
            :key="tableIndex"
            :table="table"
            :table-index="tableIndex"
            @add-column="$emit('add-column', tableIndex)"
            @edit-column="$emit('edit-column', $event)"
            @remove-column="$emit('remove-column', $event)"
            @update-columns="$emit('update-columns', $event)"
            @remove-table="$emit('remove-table', tableIndex)"
          />
        </div>
      </div>
    </div>
  `,
});
