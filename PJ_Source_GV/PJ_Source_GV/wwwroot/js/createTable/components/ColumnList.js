Vue.component("column-list", {
  props: ["columns", "tableIndex"],
  mounted() {
    const self = this;
    new Sortable(this.$refs.sortable, {
      animation: 150,
      handle: ".fa-bars",
      onEnd(evt) {
        const columnsCopy = [...self.columns];
        const movedItem = columnsCopy.splice(evt.oldIndex, 1)[0];
        columnsCopy.splice(evt.newIndex, 0, movedItem);
        console.log(columnsCopy, 'dd')
        self.$emit("update-columns", {
          tableIndex: self.tableIndex,
          columns: [...columnsCopy],
        });
      },
    });
  },
  template: `
    <div class="col-md-12">
      <div class="panel panel-default">
        <div class="panel-heading">
          <h5 class="panel-title">Danh sách cột</h5>
        </div>
        <div class="panel-body">
          <div v-if="columns.length === 0" class="text-center text-muted">
            <p>Chưa có cột nào. Vui lòng thêm cột mới.</p>
          </div>

          <div ref="sortable" class="sortable-list">
            <div v-for="(column, columnIndex) in columns" :key="columnIndex" class="column-item">         
              <div class="row">
                <div class="col-xs-1"><i class="fa fa-bars"></i></div>
                <div class="col-xs-7"><strong>{{ column.name }}</strong></div>
                <div class="col-xs-4 text-right">
                  <button class="btn btn-xs btn-primary"
                    @click="$emit('edit-column', { tableIndex, columnIndex })">
                    <i class="fa fa-pencil"></i>
                  </button>
                  <button class="btn btn-xs btn-danger"
                    @click="$emit('remove-column', { tableIndex, columnIndex })"
                    style="margin-left:5px;">
                    <i class="fa fa-trash"></i>
                  </button>
                </div>
              </div>
            </div>
          </div>          
  
        </div>
      </div>
    </div>
  `,
});
