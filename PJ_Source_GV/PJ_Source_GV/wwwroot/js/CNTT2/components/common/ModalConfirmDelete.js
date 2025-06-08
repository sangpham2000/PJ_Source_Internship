Vue.component("modal-confirm-delete", {
  props: ["type", "name"],
  template: `
      <div class="modal in" style="display:block;">
        <div class="modal-dialog">
          <div class="modal-content">
            <div class="modal-header">
              <button class="close" @click="$emit('cancel')"><span>×</span></button>
              <h4 class="modal-title">Xác nhận xóa</h4>
            </div>
            <div class="modal-body">
              <p>Bạn có chắc chắn muốn xóa {{ type }} "<strong>{{ name }}</strong>" không?</p>
              <p>Hành động này không thể hoàn tác.</p>
            </div>
            <div class="modal-footer">
              <button class="btn btn-default" @click="$emit('cancel')">Hủy</button>
              <button class="btn btn-danger" @click="$emit('confirm')">Xóa</button>
            </div>
          </div>
        </div>
      </div>
    `,
});
