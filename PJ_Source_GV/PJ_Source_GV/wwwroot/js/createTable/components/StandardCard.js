Vue.component('standard-card', {
  props: ['standard'],
  template: `
    <div class="col-md-4">
      <div class="panel panel-primary">
        <div class="panel-heading">
          <h3 class="panel-title">{{ standard.name }}</h3>
        </div>
        <div class="panel-body">
          <p><strong>Mã:</strong> {{ standard.id }}</p>
          <p><strong>Số bảng:</strong> {{ standard.tables.length }}</p>
          <p><strong>Ngày tạo:</strong> {{ standard.createdDate }}</p>
        </div>
        <div class="panel-footer">
          <button class="btn btn-sm btn-primary" @click="$emit('edit', standard)">
            <i class="fa fa-pencil"></i> Chỉnh sửa
          </button>
          <button class="btn btn-sm btn-danger pull-right">
            <i class="fa fa-trash"></i> Xóa
          </button>
        </div>
      </div>
    </div>
  `
});