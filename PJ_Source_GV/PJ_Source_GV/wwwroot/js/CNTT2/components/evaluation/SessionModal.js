Vue.component("session-modal", {
  template: `
    <div class="modal in session-modal-overlay" id="newSessionModal" style="display:block;">
        <div class="modal-dialog modal-lg">
            <div class="modal-content session-modal-content">
                <!-- Header với gradient và icon -->
                <div class="modal-header session-modal-header">
                    <button
                        type="button"
                        class="close session-close-btn"
                        @click="$emit('close')"
                        aria-label="Close"
                    >
                        <span aria-hidden="true">&times;</span>
                    </button>
                    <div class="session-header-content">
                        <h4 class="modal-title session-title">Tạo phiên đánh giá mới</h4>
                    </div>
                </div>

                <!-- Body với layout cải tiến -->
                <div class="modal-body session-modal-body">
                    <form class="session-form">
                        <!-- Standards Selection với search -->
                        <div class="form-group session-form-group">
                            <label class="session-label">
                                <i class="fa fa-list-ul session-label-icon"></i>
                                Chọn tiêu chuẩn áp dụng
                                <span class="session-required">*</span>
                            </label>
                            
                            <!-- Search box -->
                            <div class="session-search-box">
                                <input 
                                    type="text" 
                                    class="form-control session-search-input"
                                    v-model="searchTerm"
                                    placeholder="Tìm kiếm tiêu chuẩn..."
                                >
                                <i class="fa fa-search session-search-icon"></i>
                            </div>

                            <!-- Standards grid -->
                            <div class="session-standards-container">
                                <div class="session-standards-grid">
                                    <div 
                                        v-for="standard in filteredStandards" 
                                        :key="standard.id"
                                        class="session-standard-item"
                                        :class="{ 'selected': newSession.selectedStandardIds.includes(standard.id) }"
                                    >
                                        <label class="session-checkbox-label">
                                            <input
                                                type="checkbox"
                                                v-model="newSession.selectedStandardIds"
                                                :value="standard.id"
                                                class="session-checkbox"
                                            />
                                            <span class="session-checkmark"></span>
                                            <div class="session-standard-content">
                                                <span class="session-standard-name">{{ standard.name }}</span>
                                                <span v-if="standard.description" class="session-standard-desc">
                                                    {{ standard.description }}
                                                </span>
                                            </div>
                                        </label>
                                    </div>
                                </div>
                                
                                <!-- Empty state -->
                                <div v-if="filteredStandards.length === 0" class="session-empty-state">
                                    <i class="fa fa-search session-empty-icon"></i>
                                    <p>Không tìm thấy tiêu chuẩn nào phù hợp</p>
                                </div>
                            </div>

                            <!-- Selected count -->
                            <div class="session-selected-count" v-if="newSession.selectedStandardIds.length > 0">
                                <i class="fa fa-check-circle"></i>
                                Đã chọn {{ newSession.selectedStandardIds.length }} tiêu chuẩn
                            </div>
                        </div>

                        <!-- Description -->
                        <div class="form-group session-form-group">
                            <label class="session-label">
                                <i class="fa fa-sticky-note session-label-icon"></i>
                                Ghi chú
                            </label>
                            <div class="session-textarea-container">
                                <textarea
                                    class="form-control session-textarea"
                                    v-model="newSession.desc"
                                    placeholder="Nhập ghi chú cho phiên đánh giá (tùy chọn)..."
                                    rows="4"
                                ></textarea>
                                <div class="session-char-count">
                                    {{ newSession.desc.length }}/500 ký tự
                                </div>
                            </div>
                        </div>
                    </form>
                </div>

                <!-- Footer với buttons nâng cấp -->
                <div class="modal-footer">
                  <button 
                    class="btn btn-default" 
                    @click="$emit('close')"
                  >
                    <i class="fa fa-times"></i>
                    Hủy
                  </button>
                  <button 
                      class="btn btn-primary" 
                      @click="createSession"
                      :disabled="newSession.selectedStandardIds.length === 0"
                  >
                      <i class="fa fa-plus"></i>
                      Tạo phiên đánh giá
                  </button>
                </div>
            </div>
        </div>
    </div>`,
  props: [],
  data() {
    return {
      standards: [],
      searchTerm: "",
      newSession: {
        selectedStandardIds: [],
        desc: "",
      },
    };
  },
  computed: {
    filteredStandards() {
      if (!this.searchTerm) return this.standards;
      return this.standards.filter(
        (standard) =>
          standard.name.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
          (standard.description &&
            standard.description
              .toLowerCase()
              .includes(this.searchTerm.toLowerCase()))
      );
    },
  },
  created() {
    this.fetchStandards();
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
        return null;
      } else {
        return response.json();
      }
    },
    async fetchStandards() {
      try {
        const items = await this.apiRequest(
          "http://localhost:28635/API/standard",
          "GET"
        );
        this.standards = [...items];
        console.log("Standards fetched:", items);
      } catch (err) {
        console.error("Lỗi khi lấy standards:", err);
      }
    },
    async createSession() {
      try {
        if (this.newSession.selectedStandardIds.length === 0) {
          toastr.error("Vui lòng chọn ít nhất một tiêu chuẩn!");
          return;
        }

        if (this.newSession.desc.length > 500) {
          toastr.error("Ghi chú không được vượt quá 500 ký tự!");
          return;
        }

        const sessionData = {
          evaluationId: this.$parent.selectedEvaluation.id,
          desc: this.newSession.desc || null,
          standardIds: this.newSession.selectedStandardIds,
        };

        console.log("sessionData", sessionData);

        await this.apiRequest(
          "http://localhost:28635/API/evaluation/session",
          "POST",
          sessionData
        );

        toastr.success("Đã tạo phiên đánh giá mới!");
        this.$emit("session-created");
        this.$emit("close");
        this.resetForm();
      } catch (err) {
        console.error("Lỗi khi tạo phiên đánh giá:", err);
        toastr.error("Lỗi khi tạo phiên đánh giá: " + err.message);
      }
    },
    resetForm() {
      this.newSession = {
        selectedStandardIds: [],
        desc: "",
      };
      this.searchTerm = "";
    },
  },
});
