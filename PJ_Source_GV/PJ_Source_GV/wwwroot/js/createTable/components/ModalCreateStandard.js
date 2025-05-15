Vue.component('modal-create-standard', {
  props: ['existing'],
  data() {
    return {
      name: '',
      error: ''
    };
  },
  methods: {
    validateAndCreate() {
      if (!this.name) {
        this.error = "Vui lòng nhập tên tiêu chuẩn.";
        return;
      }
      if (this.existing.some(e => e.name === this.name)) {
        this.error = "Tên tiêu chuẩn đã tồn tại.";
        return;
      }
      this.$emit('create', { name: this.name });
    }
  },
  template: `
    <div class="modal in" style="display:block;">
      <div class="modal-dialog">
        <div class="modal-content">
          <div class="modal-header">
            <button class="close" @click="$emit('cancel')"><span>×</span></button>
            <h4 class="modal-title">Tạo tiêu chuẩn mới</h4>
          </div>
          <div class="modal-body">
            <div class="form-group">
              <label for="new-standard-name">Tên tiêu chuẩn:</label>
              <input v-model="name" placeholder="Nhập tên tiêu chuẩn..." class="form-control" />
            </div>
            <div class="alert alert-danger" v-if="error">{{ error }}</div>
          </div>
          <div class="modal-footer">
            <button class="btn btn-default" @click="$emit('cancel')">Hủy</button>
            <button class="btn btn-primary" @click="validateAndCreate">Tạo</button>
          </div>
        </div>
      </div>
    </div>
  `
});