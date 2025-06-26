Vue.component("modal-create-evaluation", {
  template: `
    <div class="modal in" id="createEvaluationModal" tabindex="-1" role="dialog" style="display:block;">
      <div class="modal-dialog modal-lg">
        <div class="modal-content">
          <div class="modal-header">
            <button type="button" class="close" data-dismiss="modal" aria-label="Close" @click="onClose">
              <span aria-hidden="true">×</span>
            </button>
            <h4 class="modal-title">Tạo đợt đánh giá mới</h4>
          </div>
          <div class="modal-body">
            <form>
              <div class="row">
                <div class="col-md-12">
                  <div class="form-group" :class="{ 'has-error': errors.name }">
                    <label for="evaluationName">Tên đợt đánh giá <span style="color: #ef4444">*</span></label>
                    <input
                      type="text"
                      class="form-control"
                      id="evaluationName"
                      v-model.trim="newEvaluation.name"
                      placeholder="VD: Đánh giá Q1 2025"
                    />
                    <div class="error-message" v-if="errors.name">{{ errors.name }}</div>
                  </div>
                </div>
              </div>
              <div class="form-group">
                <label for="evaluationDescription">Mô tả</label>
                <textarea
                  class="form-control"
                  id="evaluationDescription"
                  v-model="newEvaluation.desc"
                  placeholder="Mô tả chi tiết về đợt đánh giá"
                  rows="3"
                ></textarea>
              </div>
              <div class="row">
                <div class="col-md-6">
                  <div class="form-group" :class="{ 'has-error': errors.startDate }">
                    <label for="startDate">Thời gian bắt đầu <span style="color: #ef4444">*</span></label>
                    <input type="date" class="form-control" id="startDate" v-model="newEvaluation.startDate" />
                    <div class="error-message" v-if="errors.startDate">{{ errors.startDate }}</div>
                  </div>
                </div>
                <div class="col-md-6">
                  <div class="form-group" :class="{ 'has-error': errors.endDate }">
                    <label for="endDate">Thời gian kết thúc <span style="color: #ef4444">*</span></label>
                    <input type="date" class="form-control" id="endDate" v-model="newEvaluation.endDate" />
                    <div class="error-message" v-if="errors.endDate">{{ errors.endDate }}</div>
                  </div>
                </div>
              </div>
            </form>
          </div>
          <div class="modal-footer">
            <button type="button" class="btn btn-default" data-dismiss="modal" @click="onClose">Hủy</button>
            <button type="button" class="btn btn-primary" @click="createEvaluation">Tạo đợt đánh giá</button>
          </div>
        </div>
      </div>
      <loading-spinner type="ripple" color="orange" message="Vui lòng chờ..." v-if="showLoading"></loading-spinner>
    </div>
  `,
  props: ["standards", "evaluations"],
  data() {
    return {
      newEvaluation: {
        name: "",
        desc: "",
        startDate: "",
        endDate: "",
      },
      errors: {
        name: "",
        startDate: "",
        endDate: "",
      },
      showLoading: false,
    };
  },
  methods: {
    async apiRequest(url, method = "GET", data = null) {
      const options = {
        method,
        headers: {
          "Content-Type": "application/json",
        },
      };
      if (data) {
        options.body = JSON.stringify(data);
      }
      const response = await fetch(url, options);
      if (!response.ok) {
        const errorText = await response.text();
        throw new Error(`HTTP ${response.status}: ${errorText}`);
      }
      if (response.status === 204 || response.status === 201) {
        return null; // No content
      } else {
        return response.json();
      }
    },
    async createEvaluation() {
      this.errors = { standardId: "", name: "", startDate: "", endDate: "" };
      this.showLoading = true;
      let isValid = true;
      if (!this.newEvaluation.name.trim()) {
        this.errors.name = "Vui lòng nhập tên đợt đánh giá";
        isValid = false;
      } 
      // else if (
      //   this.evaluations.some(
      //     (e) => e.name.toLowerCase() === this.newEvaluation.name.toLowerCase()
      //   )
      // ) {
      //   this.errors.name = "Tên đợt đánh giá đã tồn tại";
      //   isValid = false;
      // }
      if (!this.newEvaluation.startDate) {
        this.errors.startDate = "Vui lòng chọn thời gian bắt đầu";
        isValid = false;
      }
      if (!this.newEvaluation.endDate) {
        this.errors.endDate = "Vui lòng chọn thời gian kết thúc";
        isValid = false;
      } else if (
        this.newEvaluation.startDate &&
        this.newEvaluation.endDate < this.newEvaluation.startDate
      ) {
        this.errors.endDate = "Thời gian kết thúc phải sau thời gian bắt đầu";
        isValid = false;
      }

      if (isValid) {
        const evaluation = {
          name: this.newEvaluation.name,
          desc: this.newEvaluation.desc,
          startDate: this.newEvaluation.startDate,
          endDate: this.newEvaluation.endDate,
        };
        await this.saveEvoluation(evaluation);
        this.$emit("created");
        this.newEvaluation = {
          name: "",
          description: "",
          startDate: "",
          endDate: "",
        };
        this.showLoading = false;
        toastr.success("Đã tạo đợt đánh giá mới!");
      }
    },

    async saveEvoluation(updated) {
      console.log(updated);
      try {
        await this.apiRequest(
          "http://localhost:28635/api/evaluation",
          updated.id ? "PUT" : "POST",
          updated
        );
      } catch (error) {
        console.error("Error saving evoluation:", error);
      }
    },

    onClose() {
      this.newEvaluation = {
        name: "",
        description: "",
        startDate: "",
        endDate: "",
      };
      this.$emit("close");
    },
  },
});
