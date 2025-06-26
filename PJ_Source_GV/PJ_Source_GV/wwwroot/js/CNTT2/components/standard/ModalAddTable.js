Vue.component("modal-add-table", {
  data() {
    return {
      name: "",
      columns: [],
      columnCount: 3,
      errors: { name: "", columns: "" },
      columnTypes: ["Text", "Number", "Date"], // Available column types
    };
  },
  methods: {
    validateForm() {
      this.errors = { name: "", columns: "" };
      let isValid = true;
      if (!this.name) {
        this.errors.name = "Vui lòng nhập tên bảng.";
        isValid = false;
      }
      if (this.columns.length === 0) {
        this.errors.columns = "Vui lòng thêm ít nhất một cột.";
        isValid = false;
      }
      // Validate each column has a name and type
      this.columns.forEach((column, index) => {
        if (!column.name) {
          this.errors.columns = `Vui lòng nhập tên cho cột ${index + 1}.`;
          isValid = false;
        }
        if (!column.type && column.type !== 0) {
          this.errors.columns = `Vui lòng chọn kiểu dữ liệu cho cột ${
            index + 1
          }.`;
          isValid = false;
        }
      });
      return isValid;
    },
    generateColumns() {
      this.columns = Array.from({ length: this.columnCount }, (_, i) => ({
        name: "Cột " + (i + 1),
        type: 0, // Default type
      }));
    },
    addColumn() {
      this.columns.push({ name: "", type: 0 });
    },
    removeColumn(index) {
      if (window.confirm("Bạn có chắc muốn xóa cột này?")) {
        this.columns.splice(index, 1);
      }
    },
    addTable() {
      if (!this.validateForm()) return;
      this.$emit("add", { name: this.name, columns: this.columns });
      this.name = "";
      this.columns = [];
      this.columnCount = 3;
    },
  },
  template: `
    <div class="modal in" style="display:block;">
      <div class="modal-dialog">
        <div class="modal-content">
          <div class="modal-header">
            <button class="close" @click="$emit('cancel')"><span>×</span></button>
            <h4 class="modal-title">Thêm bảng mới</h4>
          </div>
          <div class="modal-body">
            <div class="form-group" :class="{ 'has-error': errors.name }">
              <label for="new-table-name">Tên bảng:</label>
              <input
                class="form-control"
                id="new-table-name"
                placeholder="Ví dụ: Bảng 1A"
                type="text"
                v-model="name"
              />
              <span class="help-block" v-if="errors.name">{{ errors.name }}</span>
            </div>

            <div class="form-group">
              <label>Số lượng cột:</label>
              <div class="row">
                <div class="col-md-6">
                  <div class="input-group">
                    <input
                      class="form-control"
                      max="10"
                      min="1"
                      type="number"
                      v-model="columnCount"
                    />
                    <span class="input-group-btn">
                      <button
                        @click="generateColumns"
                        class="btn btn-default"
                        type="button"
                      >
                        Tạo cột
                      </button>
                    </span>
                  </div>
                </div>
              </div>
            </div>

            <div v-if="columns.length > 0">
              <hr />
              <h5>Danh sách cột:</h5>
              <div
                :key="index"
                class="form-group"
                v-for="(column, index) in columns"
              >
                <div class="row">
                  <div class="col-md-6">
                    <div class="input-group">
                      <span class="input-group-addon">{{ index + 1 }}</span>
                      <input
                        :placeholder="'Tên cột ' + (index + 1)"
                        class="form-control"
                        type="text"
                        v-model="column.name"
                      />
                    </div>
                  </div>
                  <div class="col-md-4">
                    <select class="form-control" v-model="column.type">
                      <option value="" disabled>Chọn kiểu dữ liệu</option>
                      <option v-for="(type, idx) in columnTypes" :value="idx">{{ type }}</option>
                    </select>
                  </div>
                  <div class="col-md-2">
                    <button
                      @click="removeColumn(index)"
                      class="btn btn-danger btn-block"
                      type="button"
                    >
                      <i class="fa fa-trash"></i>
                    </button>
                  </div>
                </div>
              </div>
              <button @click="addColumn" class="btn btn-sm btn-success">
                <i class="fa fa-plus"></i> Thêm cột
              </button>
            </div>
            <div class="alert alert-danger" v-if="errors.columns">{{ errors.columns }}</div>
          </div>
          <div class="modal-footer">
            <button class="btn btn-default" @click="$emit('cancel')">Hủy</button>
            <button class="btn btn-primary" @click="addTable">Lưu</button>
          </div>
        </div>
      </div>
    </div>
  `,
});