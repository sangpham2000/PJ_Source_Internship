Vue.component("session-modal", {
  template: `
    <div class="modal in" id="newSessionModal" style="display:block;">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <!-- Header với gradient và icon -->
                <div class="modal-header">
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

                <div class="modal-body session-modal-body">
                    <form class="session-form">
                        <!-- Standards Selection với search -->
                        <div class="form-group">
                            <label class="session-label">
                                <i class="fa fa-list-ul session-label-icon"></i>
                                Chọn tiêu chuẩn áp dụng
                                <span class="session-required">*</span>
                            </label>
                            
                            <!-- Search box -->
                            <div class="session-search-box">
                                <input 
                                    type="text" 
                                    class="form-control"
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

                        <!-- Department Assignment Section -->
                        <div class="form-group">
                            <label>Phân công khoa và người đánh giá <span style="color: #ef4444">*</span></label>
                            <div class="panel panel-default">
                                <div class="panel-body">
                                    <!-- Department Selection -->
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group" :class="{ 'has-error': errors.selectedDepartment }">
                                                <label for="departmentSelect">Chọn khoa</label>
                                                <select 
                                                    class="form-control" 
                                                    id="departmentSelect" 
                                                    v-model="selectedDepartment"
                                                    @change="addDepartment"
                                                >
                                                    <option value="">-- Chọn khoa --</option>
                                                    <option 
                                                        v-for="dept in availableDepartments" 
                                                        :key="dept.id" 
                                                        :value="dept.id"
                                                    >
                                                        {{ dept.name }}
                                                    </option>
                                                </select>
                                                <div class="error-message" v-if="errors.selectedDepartment">{{ errors.selectedDepartment }}</div>
                                            </div>
                                        </div>
                                    </div>

                                    <!-- Assigned Departments List -->
                                    <div v-if="newSession.assignedDepartments.length > 0">
                                        <h5>Khoa được phân công:</h5>
                                        <div class="assigned-departments">
                                            <div 
                                                v-for="assignedDept in newSession.assignedDepartments" 
                                                :key="assignedDept.departmentId"
                                                class="panel panel-info"
                                                style="margin-bottom: 15px;"
                                            >
                                                <div class="panel-heading">
                                                    <div class="row">
                                                        <div class="col-md-10">
                                                            <strong>{{ getDepartmentName(assignedDept.departmentId) }}</strong>
                                                            <span class="badge" style="margin-left: 10px;">
                                                                {{ assignedDept.staffIds.length }} giảng viên
                                                            </span>
                                                        </div>
                                                        <div class="col-md-2 text-right">
                                                            <button 
                                                                type="button" 
                                                                class="btn btn-danger btn-xs"
                                                                @click="removeDepartment(assignedDept.departmentId)"
                                                            >
                                                                <i class="fa fa-trash"></i>
                                                            </button>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="panel-body">
                                                    <!-- Staff Selection for this department -->
                                                    <div class="row">
                                                        <div class="col-md-12">
                                                            <label>Chọn giảng viên:</label>
                                                            <select 
                                                                class="form-control" 
                                                                v-model="assignedDept.selectedStaffId"
                                                                @change="addStaffToDepartment(assignedDept.departmentId)"
                                                            >
                                                                <option value="">-- Chọn giảng viên --</option>
                                                                <option 
                                                                    v-for="staff in getAvailableStaff(assignedDept.departmentId, assignedDept.staffIds)" 
                                                                    :key="staff.id" 
                                                                    :value="staff.id"
                                                                >
                                                                    {{ staff.name }} - {{ staff.position }}
                                                                </option>
                                                            </select>
                                                        </div>
                                                    </div>

                                                    <!-- Assigned Staff List -->
                                                    <div v-if="assignedDept.staffIds.length > 0" style="margin-top: 15px;">
                                                        <label>Giảng viên được chọn:</label>
                                                        <div class="assigned-staff">
                                                            <span 
                                                                v-for="staffId in assignedDept.staffIds" 
                                                                :key="staffId"
                                                                class="label label-default"
                                                                style="margin-right: 5px; margin-bottom: 5px; display: inline-block;"
                                                            >
                                                                {{ getStaffName(staffId) }}
                                                                <button 
                                                                    type="button" 
                                                                    class="btn btn-xs"
                                                                    style="margin-left: 5px; padding: 0; border: none; background: none; color: #fff;"
                                                                    @click="removeStaffFromDepartment(assignedDept.departmentId, staffId)"
                                                                >
                                                                    ×
                                                                </button>
                                                            </span>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="error-message" v-if="errors.assignedDepartments">{{ errors.assignedDepartments }}</div>
                                </div>
                            </div>
                        </div>

                        <!-- Description -->
                        <div class="form-group session-form-group">
                            <label class="session-label">
                                <i class="fa fa-sticky-note session-label-icon"></i>
                                Miêu tả
                            </label>
                            <div class="session-textarea-container">
                                <textarea
                                    class="form-control"
                                    v-model="newSession.desc"
                                    placeholder="Nhập miêu tả cho phiên đánh giá..."
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
                      :disabled="newSession.selectedStandardIds.length === 0 || newSession.assignedDepartments.length === 0"
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
        assignedDepartments: [], // Array of { departmentId, staffIds: [], selectedStaffId: "" }
      },
      errors: {
        selectedDepartment: "",
        assignedDepartments: "",
      },
      selectedDepartment: "",
      // Mock data - có thể thay thế bằng API calls
      departments: [
        { id: 1, name: "Khoa Công nghệ Thông tin" },
        { id: 2, name: "Khoa Tài chính" },
        { id: 3, name: "Khoa Ngoại ngữ" },
        { id: 4, name: "Khoa Kỹ thuật" },
        { id: 5, name: "Khoa Dược" },
        { id: 6, name: "Khoa Luật" },
        { id: 7, name: "Khoa Mỹ thuật" },
      ],
      staff: [
        {
          id: 1,
          departmentId: 1,
          name: "TS. Nguyễn Văn An",
          position: "Trưởng khoa",
        },
        {
          id: 2,
          departmentId: 1,
          name: "ThS. Trần Thị Bình",
          position: "Phó trưởng khoa",
        },
        {
          id: 3,
          departmentId: 1,
          name: "TS. Lê Anh Cường",
          position: "Giảng viên chính",
        },
        {
          id: 4,
          departmentId: 1,
          name: "ThS. Phạm Minh Đức",
          position: "Giảng viên",
        },
        {
          id: 5,
          departmentId: 2,
          name: "PGS.TS. Hoàng Thị Em",
          position: "Trưởng khoa",
        },
        {
          id: 6,
          departmentId: 2,
          name: "TS. Võ Văn Phương",
          position: "Phó trưởng khoa",
        },
        {
          id: 7,
          departmentId: 2,
          name: "ThS. Đặng Thị Giang",
          position: "Giảng viên chính",
        },
        {
          id: 8,
          departmentId: 3,
          name: "TS. Ngô Văn Hoa",
          position: "Trưởng khoa",
        },
        {
          id: 9,
          departmentId: 3,
          name: "ThS. Bùi Thị Tuấn",
          position: "Giảng viên chính",
        },
        {
          id: 10,
          departmentId: 3,
          name: "ThS. Lý Văn Kim",
          position: "Giảng viên",
        },
        {
          id: 11,
          departmentId: 4,
          name: "PGS.TS. Trương Thị Long",
          position: "Trưởng khoa",
        },
        {
          id: 12,
          departmentId: 4,
          name: "TS. Đinh Văn Mai",
          position: "Phó trưởng khoa",
        },
        {
          id: 13,
          departmentId: 4,
          name: "ThS. Phan Thị Nam",
          position: "Giảng viên",
        },
        {
          id: 14,
          departmentId: 5,
          name: "GS.TS. Lưu Văn Oanh",
          position: "Trưởng khoa",
        },
        {
          id: 15,
          departmentId: 5,
          name: "TS. Huỳnh Thị Phúc",
          position: "Phó trưởng khoa",
        },
        {
          id: 16,
          departmentId: 6,
          name: "TS. Cao Văn Quang",
          position: "Trưởng khoa",
        },
        {
          id: 17,
          departmentId: 6,
          name: "ThS. Đỗ Thị Rồng",
          position: "Giảng viên chính",
        },
        {
          id: 18,
          departmentId: 7,
          name: "PGS.TS. Vũ Văn Sơn",
          position: "Trưởng khoa",
        },
        {
          id: 19,
          departmentId: 7,
          name: "TS. Mai Thị Tâm",
          position: "Phó trưởng khoa",
        },
      ],
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
    availableDepartments() {
      const assignedIds = this.newSession.assignedDepartments.map(
          (d) => d.departmentId
      );
      return this.departments.filter((dept) => !assignedIds.includes(dept.id));
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
    async fetchStandards(page = 1, pageSize = 10000, name = "") {
      try {
        const params = new URLSearchParams({
          page: page.toString(),
          pageSize: pageSize.toString(),
        });

        // Add name filter if provided
        if (name && name.trim()) {
          params.append("name", name.trim());
        }

        const response = await this.apiRequest(
            `http://localhost:28635/API/standard?${params.toString()}`,
            "GET"
        );
        this.standards = [...response.items];
        console.log("Standards fetched:", response.items);
      } catch (err) {
        console.error("Lỗi khi lấy standards:", err);
      }
    },

    // Department assignment methods
    addDepartment() {
      if (!this.selectedDepartment) return;

      // Check if department already assigned (shouldn't happen with computed property)
      const existingDept = this.newSession.assignedDepartments.find(
          (dept) => dept.departmentId === parseInt(this.selectedDepartment)
      );

      if (existingDept) {
        toastr.warning("Khoa này đã được thêm!");
        return;
      }

      this.newSession.assignedDepartments.push({
        departmentId: parseInt(this.selectedDepartment),
        staffIds: [],
        selectedStaffId: "",
      });

      this.selectedDepartment = "";
    },

    removeDepartment(departmentId) {
      this.newSession.assignedDepartments =
          this.newSession.assignedDepartments.filter(
              (dept) => dept.departmentId !== departmentId
          );
    },

    addStaffToDepartment(departmentId) {
      const department = this.newSession.assignedDepartments.find(
          (dept) => dept.departmentId === departmentId
      );

      if (!department || !department.selectedStaffId) return;

      // Check if staff already assigned
      if (department.staffIds.includes(parseInt(department.selectedStaffId))) {
        toastr.warning("Giảng viên này đã được thêm!");
        department.selectedStaffId = "";
        return;
      }

      department.staffIds.push(parseInt(department.selectedStaffId));
      department.selectedStaffId = "";
    },

    removeStaffFromDepartment(departmentId, staffId) {
      const department = this.newSession.assignedDepartments.find(
          (dept) => dept.departmentId === departmentId
      );

      if (department) {
        department.staffIds = department.staffIds.filter(
            (id) => id !== staffId
        );
      }
    },

    getDepartmentName(departmentId) {
      const dept = this.departments.find((d) => d.id === departmentId);
      return dept ? dept.name : "";
    },

    getStaffName(staffId) {
      const staff = this.staff.find((s) => s.id === staffId);
      return staff ? staff.name : "";
    },

    getAvailableStaff(departmentId, assignedStaffIds) {
      return this.staff.filter(
          (s) =>
              s.departmentId === departmentId && !assignedStaffIds.includes(s.id)
      );
    },

    async createSession() {
      try {
        // Reset errors
        this.errors = {
          selectedDepartment: "",
          assignedDepartments: "",
        };

        if (this.newSession.selectedStandardIds.length === 0) {
          toastr.error("Vui lòng chọn ít nhất một tiêu chuẩn!");
          return;
        }

        if (this.newSession.assignedDepartments.length === 0) {
          this.errors.assignedDepartments = "Vui lòng chọn ít nhất một khoa";
          return;
        }

        // Check if all departments have at least one staff assigned
        const departmentsWithoutStaff =
            this.newSession.assignedDepartments.filter(
                (dept) => dept.staffIds.length === 0
            );

        if (departmentsWithoutStaff.length > 0) {
          this.errors.assignedDepartments =
              "Mỗi khoa phải có ít nhất một giảng viên được chọn";
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
          assignedDepartments: JSON.stringify(
              this.newSession.assignedDepartments
          ),
        };

        // console.log("sessionData", sessionData);
        // return;
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
        assignedDepartments: [], // Array of { departmentId, staffIds: [], selectedStaffId: "" }
      };
      this.searchTerm = "";
    },
  },
});
