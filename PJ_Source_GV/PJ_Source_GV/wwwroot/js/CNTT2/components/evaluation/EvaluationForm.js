Vue.component("evaluation-form", {
  template: `
    <div>
      <session-modal
        v-if="showSessionModal"
        :show="showSessionModal"
        :evaluation-id="selectedEvaluation.id"
        @close="handleClose"
        @session-created="fetchSessions"
      />

      <div class="page-header">
        <h4 v-if="editSessionMode">Nhập dữ liệu đánh giá: {{ selectedEvaluation.name }}</h4>
        <h3 v-else>
          <i class="fa fa-book-open"></i>
          Lịch sử các phiên đánh giá: {{ selectedEvaluation.name }}
        </h3>
        <button class="btn btn-outline-secondary" @click="$emit('back')">
          <i class="fa fa-arrow-left"></i> Quay lại
        </button>
      </div>   

      <div class="card" v-if="selectedEvaluation && !editSessionMode">
        <div class="card-body">
          <button class="btn btn-primary" @click="openNewSessionModal">
            Tạo phiên đánh giá mới
          </button>
          <ul class="list-group" style="margin: 20px 0" v-if="evaluationSessions.length">
            <li
              class="list-group-item"
              v-for="(session, index) in evaluationSessions"
              :key="index"
            >
              <div>
                <strong>ID:</strong> {{session.id}}<br />
                <strong>Phiên:</strong> {{ formatDate(session.createdAt) }} <br />
                <strong>Tiêu chuẩn:</strong>
                <ul>
                  <li v-for="s in session.standardNames" :key="s">{{ s }}</li>
                </ul>
                <strong>Miêu tả:</strong> {{ session.desc || 'Không có' }}
              </div>
              <div class="list-group-item-action">
                <div class="status" :class="getStatusClass(session.status)">
                  {{getStatusLabel(session.status)}}
                </div>
                <button v-if="session.status === 0" class="btn btn-primary" @click="editSession(session)">
                  <i class="fa fa-pencil"></i> Nhập dữ liệu đánh giá
                </button>
                <button v-if="session.status === 1" class="btn btn-primary" @click="editSession(session)">
                  <i class="fa fa-pencil"></i> Nhập tiếp
                </button>
                <button v-if="session.status === 2" class="btn btn-primary" @click="editSession(session)">
                  Xem đánh giá
                </button>
              </div>
            </li>
          </ul>
          <div class="empty-state" v-else>
            <i class="fa fa-inbox empty-icon"></i>
            <p>Chưa có phiên đánh giá nào. Hãy tạo phiên mới!</p>
          </div>
        </div>
      </div>

      <div class="card" v-if="editSessionMode">
        <div class="card-body" v-for="standard in sessionForm.standards" :key="standard.id">
          <div class="evaluation-form" v-if="sessionForm">
            <div style="margin-bottom: 3rem;">
              <h3>{{ standard.name }}</h3>
              <div v-for="table in standard.tables" :key="table.id" style="margin-bottom: 2rem;">
                <h5 style="text-align: center;">{{ table.name }}</h5>
                <table class="table table-bordered">
                  <thead>
                    <tr>
                      <th v-for="column in table.columns" :key="column.id">{{ column.name }}</th>
                      <th class="row-actions">Hành động</th>
                    </tr>
                  </thead>
                  <tbody>
                    <tr v-for="(row, rowIndex) in sessionData[table.id]" :key="row.id">
                      <td v-for="column in table.columns" :key="column.id">
                        <input
                          v-if="column.type === inputType.string"
                          type="text"
                          class="form-control"
                          v-model.trim="row.cells[column.id].value"
                          :placeholder="'Nhập ' + column.name"
                        />
                        <input
                          v-if="column.type === inputType.number || column.type === inputType.dateTime"
                          type="number"
                          class="form-control"
                          v-model.number="row.cells[column.id].value"
                          :placeholder="'Nhập ' + column.name"
                          :min="0"
                          :max="column.type === inputType.number ? '' : row.cells[table.columns.find(c => c.type === inputType.number)?.id]?.value || 100"
                        />
                        <div
                          class="error-message"
                          v-if="errors.data && errors.data[table.id] && errors.data[table.id][rowIndex] && errors.data[table.id][rowIndex][column.id]"
                        >
                          {{ errors.data[table.id][rowIndex][column.id] }}
                        </div>
                      </td>
                      <td class="row-actions">
                        <button
                          class="btn btn-danger btn-sm"
                          @click="showConfirmDelete(table.id, rowIndex, row.cells[table.columns[0].id].value)"
                          v-if="sessionData[table.id].length > 1"
                        >
                          <i class="fa fa-trash" aria-hidden="true"></i>
                        </button>
                      </td>
                    </tr>
                  </tbody>
                </table>
                <div class="table-actions">
                  <button class="btn btn-primary btn-sm" @click="addRow(table.id)">Thêm dòng</button>
                </div>
              </div>
            </div>
          </div>
        </div>

        <div style="padding: 20px">
          <div class="form-group">
            <label>Ghi chú đánh giá (tùy chọn)</label>
            <textarea
              class="form-control"
              v-model="sessionNote"
              placeholder="Thêm ghi chú cho đánh giá này"
              rows="4"
            ></textarea>
          </div>
          <div class="evaluation-meta">
            <p>Đã lưu nháp lần cuối: {{ lastSavedDraft || 'Chưa lưu' }}</p>
          </div>
          <button class="btn btn-default" @click="submitEvaluation('draft')">Lưu nháp</button>
          <button class="btn btn-primary" @click="submitEvaluation">Lưu</button>
          <button class="btn btn-success" @click="exportToExcel" style="margin-left: 10px;" v-if="currenSession.status === 2">
            <i class="fa fa-download"></i> Xuất Excel
          </button>
        </div>
        <loading-spinner type="ripple" color="orange" message="Vui lòng chờ..." v-if="showLoading"></loading-spinner>
        <modal-confirm-delete
          v-if="showConfirmDeleteModal"
          :type="'dòng'"
          :name="confirmDeleteName"
          @cancel="showConfirmDeleteModal = false"
          @confirm="confirmDeleteRow"
        />
      </div>
    </div>
  `,
  props: ["selectedEvaluation"],
  data() {
    return {
      showConfirmDeleteModal: false,
      confirmDeleteTableId: null,
      confirmDeleteRowIndex: null,
      confirmDeleteName: "",
      sessionForm: null,
      sessionData: {},
      sessionNote: "",
      lastSavedDraft: "",
      errors: {
        standardId: "",
        name: "",
        startDate: "",
        endDate: "",
        data: {},
      },
      inputType: {
        string: 0,
        number: 1,
        dateTime: 2,
      },
      evaluationSessions: [],
      showSessionModal: false,
      editSessionMode: false,
      currenSession: null,
      showLoading: false,
    };
  },
  mounted() {
    if (this.selectedEvaluation) {
      // this.configSessionForm();
      this.fetchSessions();
    }
  },
  methods: {
    configSessionForm(standards = this.selectedEvaluation.standards) {
      this.sessionForm = {
        standards: standards.map((standard) => ({
          id: standard.id,
          name: standard.name,
          tables: standard.tables.map((table) => ({
            id: table.id,
            name: table.name,
            columns: table.columns,
          })),
        })),
      };

      // Initialize sessionData
      this.sessionData = this.sessionForm.standards.reduce((acc, standard) => {
        standard.tables.forEach((table) => {
          const tableId = table.id;
          const columns = table.columns;

          // Nếu đã có dữ liệu cells thì gán vào sessionData
          if (this.currenSession?.cells?.length > 0) {
            // Lọc các cell theo table hiện tại
            const tableCells = this.currenSession.cells.filter((cell) =>
              columns.some((col) => col.id === cell.columnId)
            );

            if (tableCells.length > 0) {
              // Sắp xếp cells theo ID để đảm bảo thứ tự
              const sortedCells = tableCells.sort((a, b) => a.id - b.id);

              // Gom nhóm cells thành rows dựa trên số lượng columns
              const columnsPerRow = columns.length;
              const rows = [];

              for (let i = 0; i < sortedCells.length; i += columnsPerRow) {
                const rowCells = sortedCells.slice(i, i + columnsPerRow);
                const rowIndex = Math.floor(i / columnsPerRow);
                const order =
                  rowCells[0]?.order !== undefined
                    ? rowCells[0].order
                    : rowIndex;

                const row = {
                  id: `row_${tableId}_${rowIndex + 1}`,
                  order: order,
                  displayIndex: rowIndex + 1,
                  cells: {},
                };

                // Thêm cells vào row
                rowCells.forEach((cell) => {
                  row.cells[cell.columnId] = {
                    id: cell.id,
                    columnId: cell.columnId,
                    evaluationId: cell.evaluationSessionId,
                    value: cell.value || "",
                    order: cell.order,
                  };
                });

                rows.push(row);
              }

              acc[tableId] = rows;

              // Đảm bảo mỗi row có đầy đủ cells cho tất cả columns
              acc[tableId].forEach((row) => {
                columns.forEach((col) => {
                  if (!row.cells[col.id]) {
                    row.cells[col.id] = {
                      id: null,
                      columnId: col.id,
                      evaluationId: this.selectedEvaluation?.id || null,
                      value: "",
                      order: row.order,
                    };
                  }
                });
              });
            } else {
              // Không có cell nào cho table này, khởi tạo row trống
              acc[tableId] = this.createEmptyRow(tableId, columns, 0);
            }
          } else {
            // Không có dữ liệu cells thì khởi tạo rỗng
            acc[tableId] = this.createEmptyRow(tableId, columns, 0);
          }
        });
        return acc;
      }, {});

      console.log(
        "this.currenSession cells:",
        JSON.stringify(this.currenSession?.cells, null, 2),
        this.currenSession?.cells
      );
      console.log(
        "sessionData:",
        JSON.stringify(this.sessionData, null, 2),
        this.sessionData
      );

      this.sessionNote = "";
      this.lastSavedDraft = "";
      this.errors = {
        standardId: "",
        name: "",
        startDate: "",
        endDate: "",
        data: {},
      };
    },

    createEmptyRow(tableId, columns, order = 0) {
      return [
        {
          id: `row_${tableId}_${order + 1}`,
          order: order,
          displayIndex: order + 1,
          cells: columns.reduce((colAcc, col) => {
            colAcc[col.id] = {
              id: null,
              columnId: col.id,
              evaluationId: this.selectedEvaluation?.id || null,
              value: "",
              order: order,
            };
            return colAcc;
          }, {}),
        },
      ];
    },

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
      return response.status === 204 ? null : response.json();
    },
    async fetchSessions() {
      try {
        const sessions = await this.apiRequest(
          `http://localhost:28635/API/evaluation/session/${this.selectedEvaluation.id}`
        );
        this.evaluationSessions = sessions;
      } catch (err) {
        console.error("Lỗi khi lấy sessions:", err);
        toastr.error("Lỗi khi lấy danh sách phiên!");
      }
    },
    formatDate(date) {
      return new Date(date).toLocaleString("vi-VN", {
        day: "2-digit",
        month: "2-digit",
        year: "numeric",
        hour: "2-digit",
        minute: "2-digit",
        second: "2-digit",
        hour12: false,
      });
    },
    addRow(tableId) {
      const standard = this.sessionForm.standards.find((std) =>
        std.tables.some((t) => t.id === tableId)
      );
      const table = standard.tables.find((t) => t.id === tableId);
      this.sessionData[tableId].push({
        id: `row_${tableId}_${this.sessionData[tableId].length + 1}`,
        cells: table.columns.reduce((acc, col) => {
          acc[col.id] = {
            id: null,
            columnId: col.id,
            evaluationId: this.selectedEvaluation.id,
            value: "",
            order: this.sessionData[tableId].length + 1,
          };
          return acc;
        }, {}),
      });
    },
    showConfirmDelete(tableId, rowIndex, name) {
      this.confirmDeleteTableId = tableId;
      this.confirmDeleteRowIndex = rowIndex;
      this.confirmDeleteName = name || `Dòng ${rowIndex + 1}`;
      this.showConfirmDeleteModal = true;
    },
    confirmDeleteRow() {
      this.sessionData[this.confirmDeleteTableId].splice(
        this.confirmDeleteRowIndex,
        1
      );

      this.sessionData[this.confirmDeleteTableId].forEach((row, index) => {
        Object.values(row.cells).forEach((cell) => {
          cell.order = index + 1;
        });
      });
      this.$delete(this.errors.data, this.confirmDeleteTableId);
      toastr.success("Đã xóa dòng!");
      this.showConfirmDeleteModal = false;
    },
    saveDraft() {
      this.$emit("save-draft");
      toastr.success("Đã lưu nháp!");
    },
    async submitEvaluation(mode = "saved") {
      this.errors = {
        standardId: "",
        name: "",
        startDate: "",
        endDate: "",
        data: {},
      };
      let isValid = true;
      this.showLoading = true;
      if (mode === "draft") {
        this.sessionForm.standards.forEach((standard) => {
          standard.tables.forEach((table) => {
            this.sessionData[table.id].forEach((row, rowIndex) => {
              table.columns.forEach((column) => {
                const cell = row.cells[column.id];
                const value = cell.value;
                if (!value && value !== 0) {
                  if (!this.errors.data[table.id])
                    this.errors.data[table.id] = {};
                  if (!this.errors.data[table.id][rowIndex])
                    this.errors.data[table.id][rowIndex] = {};
                  this.errors.data[table.id][rowIndex][column.id] =
                    "Vui lòng nhập dữ liệu";
                  isValid = false;
                } else if (column.type === this.inputType.dateTime) {
                  // Optional: Re-enable max score validation if needed
                  /*
                  const maxScore = row.cells[
                    table.columns.find((c) => c.type === this.inputType.number)?.id
                  ]?.value;
                  if (value > maxScore) {
                    if (!this.errors.data[table.id]) this.errors.data[table.id] = {};
                    if (!this.errors.data[table.id][rowIndex])
                      this.errors.data[table.id][rowIndex] = {};
                    this.errors.data[table.id][rowIndex][column.id] =
                      "Điểm đánh giá không được vượt quá điểm tối đa";
                    isValid = false;
                  }
                  */
                } else if (
                  (column.type === this.inputType.number ||
                    column.type === this.inputType.dateTime) &&
                  value < 0
                ) {
                  if (!this.errors.data[table.id])
                    this.errors.data[table.id] = {};
                  if (!this.errors.data[table.id][rowIndex])
                    this.errors.data[table.id][rowIndex] = {};
                  this.errors.data[table.id][rowIndex][column.id] =
                    "Điểm không được nhỏ hơn 0";
                  isValid = false;
                }
              });
            });
          });
        });
      }

      if (isValid) {
        try {
          const evaluationData = {
            evaluationId: this.selectedEvaluation.id,
            sessionNote: this.sessionNote || null,
            tables: Object.keys(this.sessionData).map((tableId) => ({
              tableId: parseInt(tableId),
              rows: this.sessionData[tableId].map((row) => ({
                cells: Object.values(row.cells).map((cell) => ({
                  id: cell.id,
                  columnId: cell.columnId,
                  evaluationId: cell.evaluationId,
                  value: cell.value.toString(),
                  order: cell.order,
                })),
              })),
            })),
          };

          console.log("Evaluation Data:", evaluationData);

          // Uncomment to enable API submission

          await this.apiRequest(
            `http://localhost:28635/API/evaluation/add-session-value/${
              mode === "draft" ? 1 : 2
            }`,
            "POST",
            {
              ...this.currenSession,
              ...evaluationData,
            }
          );
          if (mode === "draft") {
            toastr.success("Đã lưu nháp!");
          } else {
            toastr.success("Đã lưu đánh giá!");
          }
          // this.sessionForm = null;
          // this.sessionData = {};
          this.sessionNote = "";
          this.lastSavedDraft = "";
          this.showLoading = false;
        } catch (err) {
          this.showLoading = false;
          console.error("Lỗi khi lưu đánh giá:", err);
          toastr.error("Lỗi khi lưu đánh giá: " + err.message);
        }
      }
      this.$emit("submit-evaluation");
    },
    openNewSessionModal() {
      this.showSessionModal = true;
    },
    handleClose() {
      console.log("Modal closed");
      this.showSessionModal = false;
    },
    getStatusClass(status) {
      switch (status) {
        case 1:
          return "warning";
        case 2:
          return "success";
        default:
          return "edit";
      }
    },
    getStatusLabel(status) {
      switch (status) {
        case 1:
          return "Draft";
        case 2:
          return "Saved";
        default:
          return "Edit";
      }
    },
    async editSession(item) {
      this.currenSession = item;
      console.log(item)
      switch (item.status) {
        case 1:
          // Draft: Load existing session data (not implemented in original)
          break;
        case 2:
          // Saved: Load existing session data (not implemented in original)
          break;
        default:
          // Edit: Fetch standards by IDs and configure form
          break;
      }
      try {
        const standards = await this.apiRequest(
          `http://localhost:28635/api/standard/GetByIds${
            item.id && item.status !== 0 ? `/${item.id}` : ""
          }`,
          "POST",
          item.standardIds
        );
        this.configSessionForm(standards);
      } catch (err) {
        console.error("Lỗi khi lấy tiêu chuẩn:", err);
        toastr.error("Lỗi khi lấy tiêu chuẩn!");
        return;
      }
      this.editSessionMode = true;
    },
    createSheetName(standardName, tableName) {
      const separator = " - ";
      const maxLength = 31;

      let fullName = `${standardName}${separator}${tableName}`;

      if (fullName.length <= maxLength) {
        return fullName;
      }

      const availableLength = maxLength - separator.length;
      const halfLength = Math.floor(availableLength / 2);

      const shortStandard =
        standardName.length > halfLength
          ? standardName.substring(0, halfLength - 1) + "…"
          : standardName;

      const shortTable =
        tableName.length > halfLength
          ? tableName.substring(0, halfLength - 1) + "…"
          : tableName;

      return `${shortStandard}${separator}${shortTable}`;
    },

    // Hàm tạo style cho header
    createHeaderStyle() {
      return {
        fill: {
          fgColor: { rgb: "4472C4" }, // Màu xanh chuyên nghiệp
        },
        font: {
          bold: true,
          color: { rgb: "FFFFFF" },
          size: 12,
          name: "Arial",
        },
        alignment: {
          horizontal: "center",
          vertical: "center",
        },
        border: {
          top: { style: "thin", color: { rgb: "000000" } },
          bottom: { style: "thin", color: { rgb: "000000" } },
          left: { style: "thin", color: { rgb: "000000" } },
          right: { style: "thin", color: { rgb: "000000" } },
        },
      };
    },

    // Hàm tạo style cho data cells
    createDataStyle(isEvenRow = false) {
      return {
        fill: {
          fgColor: { rgb: isEvenRow ? "F2F2F2" : "FFFFFF" }, // Xen kẽ màu
        },
        font: {
          size: 11,
          name: "Arial",
          color: { rgb: "000000" },
        },
        alignment: {
          horizontal: "left",
          vertical: "center",
          wrapText: true,
        },
        border: {
          top: { style: "thin", color: { rgb: "D4D4D4" } },
          bottom: { style: "thin", color: { rgb: "D4D4D4" } },
          left: { style: "thin", color: { rgb: "D4D4D4" } },
          right: { style: "thin", color: { rgb: "D4D4D4" } },
        },
      };
    },

    // Hàm apply style cho worksheet
    applyWorksheetStyle(ws, wsData) {
      const range = XLSX.utils.decode_range(ws["!ref"]);

      // Style cho header (hàng đầu tiên)
      for (let col = range.s.c; col <= range.e.c; col++) {
        const cellAddress = XLSX.utils.encode_cell({ r: 0, c: col });
        if (!ws[cellAddress]) continue;

        ws[cellAddress].s = this.createHeaderStyle();
      }

      // Style cho data rows
      for (let row = 1; row <= range.e.r; row++) {
        const isEvenRow = row % 2 === 0;

        for (let col = range.s.c; col <= range.e.c; col++) {
          const cellAddress = XLSX.utils.encode_cell({ r: row, c: col });
          if (!ws[cellAddress]) continue;

          ws[cellAddress].s = this.createDataStyle(isEvenRow);
        }
      }

      // Tự động điều chỉnh độ rộng cột
      const columnWidths = [];
      for (let col = range.s.c; col <= range.e.c; col++) {
        let maxWidth = 10; // Độ rộng tối thiểu

        for (let row = range.s.r; row <= range.e.r; row++) {
          const cellAddress = XLSX.utils.encode_cell({ r: row, c: col });
          if (ws[cellAddress] && ws[cellAddress].v) {
            const cellLength = ws[cellAddress].v.toString().length;
            maxWidth = Math.max(maxWidth, Math.min(cellLength + 2, 50)); // Tối đa 50
          }
        }

        columnWidths.push({ wch: maxWidth });
      }

      ws["!cols"] = columnWidths;

      // Freeze header row
      ws["!freeze"] = { xSplit: 0, ySplit: 1 };

      return ws;
    },

    // Hàm tạo summary sheet
    createSummarySheet() {
      const summaryData = [
        ["BÁO CÁO ĐÁNH GIÁ"],
        [""],
        ["Thông tin chung:"],
        ["Tên đánh giá:", this.selectedEvaluation?.name || ""],
        ["Ngày xuất:", new Date().toLocaleDateString("vi-VN")],
        ["Thời gian xuất:", new Date().toLocaleTimeString("vi-VN")],
        [""],
        ["Thống kê:"],
        ["Số tiêu chuẩn:", this.sessionForm?.standards?.length || 0],
        ["Tổng số bảng:", this.getTotalTables()],
        ["Tổng số dữ liệu:", this.getTotalDataRows()],
        [""],
        ["Ghi chú:"],
        [this.sessionNote || "Không có ghi chú"],
      ];

      const ws = XLSX.utils.aoa_to_sheet(summaryData);

      // Style cho summary sheet
      const range = XLSX.utils.decode_range(ws["!ref"]);

      // Title style
      if (ws["A1"]) {
        ws["A1"].s = {
          fill: { fgColor: { rgb: "1F4E79" } },
          font: {
            bold: true,
            size: 16,
            color: { rgb: "FFFFFF" },
            name: "Arial",
          },
          alignment: { horizontal: "center", vertical: "center" },
        };
      }

      // Section headers style
      ["A3", "A8", "A13"].forEach((cell) => {
        if (ws[cell]) {
          ws[cell].s = {
            font: {
              bold: true,
              size: 12,
              color: { rgb: "1F4E79" },
              name: "Arial",
            },
            fill: { fgColor: { rgb: "E7F3FF" } },
          };
        }
      });

      // Merge title cell
      ws["!merges"] = [{ s: { r: 0, c: 0 }, e: { r: 0, c: 1 } }];

      // Column widths
      ws["!cols"] = [{ wch: 25 }, { wch: 40 }];

      return ws;
    },

    // Hàm helper
    getTotalTables() {
      return (
        this.sessionForm?.standards?.reduce((total, standard) => {
          return total + (standard.tables?.length || 0);
        }, 0) || 0
      );
    },

    getTotalDataRows() {
      let total = 0;
      this.sessionForm?.standards?.forEach((standard) => {
        standard.tables?.forEach((table) => {
          total += this.sessionData[table.id]?.length || 0;
        });
      });
      return total;
    },

    exportToExcel() {
      if (typeof XLSX === "undefined") {
        alert("Thư viện Excel chưa được tải. Vui lòng thử lại sau.");
        return;
      }

      try {
        const wb = XLSX.utils.book_new();

        // Thêm summary sheet đầu tiên
        const summaryWs = this.createSummarySheet();
        XLSX.utils.book_append_sheet(wb, summaryWs, "📊 Tổng quan");

        // Tạo các sheet cho từng table
        this.sessionForm.standards.forEach((standard, standardIndex) => {
          standard.tables.forEach((table, tableIndex) => {
            const wsData = [table.columns.map((col) => col.name)]; // Header row

            // Add data rows
            this.sessionData[table.id].forEach((row) => {
              const rowData = table.columns.map(
                (col) => row.cells[col.id].value || ""
              );
              wsData.push(rowData);
            });

            const ws = XLSX.utils.aoa_to_sheet(wsData);

            // Apply professional styling
            this.applyWorksheetStyle(ws, wsData);

            const sheetName = this.createSheetName(standard.name, table.name);
            XLSX.utils.book_append_sheet(wb, ws, sheetName);
          });
        });

        // Add session note as a separate sheet if it exists
        if (this.sessionNote) {
          const noteData = [
            ["GHI CHÚ ĐÁNH GIÁ"],
            [""],
            ["Nội dung:"],
            [this.sessionNote],
            [""],
            ["Ngày tạo:", new Date().toLocaleDateString("vi-VN")],
            ["Thời gian:", new Date().toLocaleTimeString("vi-VN")],
          ];

          const noteWs = XLSX.utils.aoa_to_sheet(noteData);

          // Style note sheet
          if (noteWs["A1"]) {
            noteWs["A1"].s = {
              fill: { fgColor: { rgb: "70AD47" } },
              font: {
                bold: true,
                size: 14,
                color: { rgb: "FFFFFF" },
                name: "Arial",
              },
              alignment: { horizontal: "center", vertical: "center" },
            };
          }

          noteWs["!cols"] = [{ wch: 80 }];
          noteWs["!merges"] = [{ s: { r: 0, c: 0 }, e: { r: 0, c: 0 } }];

          XLSX.utils.book_append_sheet(wb, noteWs, "📝 Ghi chú");
        }

        // Thêm metadata cho workbook
        wb.Props = {
          Title: `${this.selectedEvaluation?.name || "Đánh giá"} - Báo cáo`,
          Subject: "Báo cáo đánh giá",
          Author: "Hệ thống đánh giá",
          CreatedDate: new Date(),
          Company: "Tổ chức đánh giá",
        };

        // Generate filename with timestamp
        const timestamp = new Date()
          .toISOString()
          .slice(0, 19)
          .replace(/:/g, "-");
        const filename = `${
          this.selectedEvaluation?.name || "Evaluation"
        }_${timestamp}.xlsx`;

        // Download file
        XLSX.writeFile(wb, filename);

        // Thông báo thành công
        toastr.success("Xuất file Excel thành công!");
      } catch (error) {
        console.error("Lỗi khi xuất Excel:", error);
        toastr.error("Có lỗi xảy ra khi xuất file Excel");
      }
    },
  },
});
