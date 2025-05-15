Vue.component("standards-list", {
  props: ["standards"],
  template: `
    <div>
      <div class="page-header">
        <div class="pull-right">
          <button class="btn btn-primary" @click="$emit('create')">
            <i class="fa fa-plus"></i> Tạo tiêu chuẩn mới
          </button>
        </div>
        <h3><i class="fa fa-list-alt"></i> Danh sách tiêu chuẩn đánh giá</h3>
      </div>
      <div class="row">
        <standard-card
          v-for="standard in standards"
          :key="standard.id"
          :standard="standard"
          @edit="$emit('edit', standard)"
        />
      </div>
    </div>
  `,
});
