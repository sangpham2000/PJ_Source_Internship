Vue.component("modal-edit-column", {
  props: ["editingColumn"],
  data() {
    return {
      error: "",
      columnTypes: ["Text", "Number", "Date"], // Available column types
    };
  },
  methods: {
    validateAndSave() {
      this.error = "";
      if (!this.editingColumn.name) {
        this.error = "Vui lòng nhập tên cột.";
        return;
      }
      if (!this.editingColumn.type && this.editingColumn.type !== 0) {
        this.error = "Vui lòng chọn kiểu dữ liệu.";
        return;
      }
      console.log(this.editingColumn);
      this.$emit("save", this.editingColumn);
      this.error = "";
    },
  },
  template: `
    <div class="modal in" style="display:block;">
      <div class="modal-dialog">
        <div class="modal-content">
          <div class="modal-header">
            <button class="close" @click="$emit('cancel')"><span>×</span></button>
            <h4 class="modal-title">{{ editingColumn.isNew ? 'Thêm cột mới' : 'Sửa cột' }}</h4>
          </div>
          <div class="modal-body">
            <div class="form-group" :class="{ 'has-error': error }">
              <label for="column-name">Tên cột:</label>
              <input
                id="column-name"
                v-model="editingColumn.name"
                placeholder="Tên cột..."
                class="form-control"
              />
              <span class="help-block" v-if="error">{{ error }}</span>
            </div>
            <div class="form-group">
              <label for="column-type">Kiểu dữ liệu:</label>
              <select
                id="column-type"
                v-model="editingColumn.type"
                class="form-control"
              >
                <option v-for="(type, idx) in columnTypes" :value="idx">{{ type }}</option>
              </select>
            </div>
          </div>
          <div class="modal-footer">
            <button class="btn btn-default" @click="$emit('cancel')">Hủy</button>
            <button class="btn btn-primary" @click="validateAndSave">Lưu</button>
          </div>
        </div>
      </div>
    </div>
  `,
});
