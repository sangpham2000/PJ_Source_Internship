Vue.component('modal-edit-column', {
  props: ['editingColumn'],
  template: `
    <div class="modal in" style="display:block;">
      <div class="modal-dialog">
        <div class="modal-content">
          <div class="modal-header">
            <button class="close" @click="$emit('cancel')"><span>×</span></button>
            <h4 class="modal-title">{{ editingColumn.isNew ? 'Thêm cột mới' : 'Sửa cột' }}</h4>
          </div>
          <div class="modal-body">
            <input v-model="editingColumn.name" placeholder="Tên cột..." class="form-control" />
          </div>
          <div class="modal-footer">
            <button class="btn btn-default" @click="$emit('cancel')">Hủy</button>
            <button class="btn btn-primary" @click="$emit('save', editingColumn)">Lưu</button>
          </div>
        </div>
      </div>
    </div>
  `
});