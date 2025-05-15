Vue.component("table-editor", {
  props: ["table", "tableIndex"],
  template: `
    <div class="table-container">
      <div class="table-header">
        <div class="row">
          <div class="col-md-6">
            <input id="table-name" class="form-control" v-model="table.name" />
          </div>
          <div class="col-md-6 text-right">
            <button class="btn btn-sm btn-success" @click="$emit('add-column')">
              <i class="fa fa-plus"></i> Thêm cột
            </button>
            <button class="btn btn-sm btn-danger" @click="$emit('remove-table')">
              <i class="fa fa-trash"></i> Xóa
            </button>
          </div>
        </div>
      </div>
      <hr />
      <column-list
        :columns="table.columns"
        :table-index="tableIndex"
        @edit-column="$emit('edit-column', $event)"
        @remove-column="$emit('remove-column', $event)"
        @update-columns="$emit('update-columns', $event)"
      />
    </div>
  `,
});
