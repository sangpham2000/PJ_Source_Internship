Vue.component("modal-edit-column", {
  props: ["editingColumn"],
  data() {
    return {
      error: "",
    };
  },
  methods: {
    validateAndSave() {
      if (!this.editingColumn.name) {
        this.error = "Vui lòng nhập tên cột.";
        return;
      }
      this.$emit("save", this.editingColumn);
      this.error = "";
    },
  },
  template: `
    <div class="modal in" style="display:block;">
      <div class="modal-dialog">
        <div class="modal-content">
          <div class="modal-header">
            <button class="close" @click="$emit('cancel')"><span>&times;</span></button>
            <h4 class="modal-title">{{ editingColumn.isNew ? 'Thêm cột mới' : 'Sửa cột' }}</h4>
          </div>
          <div class="modal-body">
            <div class="form-group" :class="{ 'has-error': error }">
              <input
                v-model="editingColumn.name"
                placeholder="Tên cột..."
                class="form-control"
              />
              <span class="help-block" v-if="error">{{ error }}</span>
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
