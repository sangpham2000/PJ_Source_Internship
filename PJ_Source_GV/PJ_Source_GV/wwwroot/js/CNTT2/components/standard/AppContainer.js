Vue.component("app-container", {
  template: `
    <div class="container-fluid">
      <standards-list
        v-if="currentPage === 'standards-list'"
        :standards="standards"
        @create="showCreateStandardModal"
        @edit="editStandard"
        @remove="removeStandard"
      />
      <standard-editor
        v-if="currentPage === 'edit-standard'"
        :standard.sync="currentStandard"
        @back="backToList"
        @save="saveStandard"
        @add-table="showAddTableModal"
        @add-column="showAddColumnModal"
        @edit-column="editColumn"
        @update-column="updateColumns"
        @remove-column="removeColumn"
        @remove-table="removeTable"
      />
      <modal-create-standard
        v-if="showModal === 'create-standard'"
        :existing="standards"
        @cancel="hideModal"
        @create="createStandard"
      />
      <modal-add-table
        v-if="showModal === 'add-table'"
        @cancel="hideModal"
        @add="addTable"
      />
      <modal-edit-column
        v-if="showModal === 'edit-column'"
        :editing-column.sync="editingColumn"
        @cancel="hideModal"
        @save="saveColumn"
      />
      <loading-spinner type="ripple" color="orange" message="Vui lòng chờ..." v-if="showLoading"></loading-spinner>
    </div>
  `,
  data() {
    return {
      currentPage: "standards-list",
      showModal: null,
      standards: [],
      currentStandard: { id: "", name: "", tables: [] },
      editingColumn: { tableIndex: -1, columnIndex: -1, name: "", isNew: true },
      showLoading: false,
    };
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
        return null; // No content
      } else {
        return response.json();
      }
    },
    async fetchStandards() {
      this.showLoading = true;
      try {
        const items = await this.apiRequest(
          "http://localhost:28635/API/standard",
          "GET"
        );
        this.standards = [...items];
        this.showLoading = false;
        console.log("Standards fetched:", items);
      } catch (err) {
        this.showLoading = false;
        console.error("Lỗi khi lấy standards:", err);
      }
    },
    backToList() {
      this.currentPage = "standards-list";
    },
    showCreateStandardModal() {
      this.showModal = "create-standard";
    },
    showAddTableModal() {
      this.showModal = "add-table";
    },
    showAddColumnModal(index) {
      this.editingColumn = {
        tableIndex: index,
        columnIndex: -1,
        name: "",
        isNew: true,
      };
      this.showModal = "edit-column";
    },
    hideModal() {
      this.showModal = null;
    },
    editStandard(std) {
      this.currentStandard = JSON.parse(JSON.stringify(std));
      this.currentPage = "edit-standard";
    },
    createStandard(newStd) {
      this.currentStandard = {
        name: newStd.name,
        tables: [],
      };
      this.currentPage = "edit-standard";
      this.showModal = null;
    },
    async saveStandard(updated) {
      try {
        const savedStandard = await this.apiRequest(
          "http://localhost:28635/api/standard",
          updated.id ? "PUT" : "POST",
          updated
        );
        const idx = this.standards.findIndex((s) => s.id === updated.id);
        if (idx >= 0) {
          this.standards.splice(idx, 1, updated);
        } else {
          this.standards.push(updated);
        }
        this.currentPage = "standards-list";
        this.fetchStandards();
      } catch (error) {
        console.error("Error saving standard:", error);
      }
    },
    async removeStandard(standard) {
      try {
        await this.apiRequest(
          `http://localhost:28635/api/standard/${standard.id}`,
          "DELETE"
        );
        this.fetchStandards();
      } catch (error) {
        console.error("Error saving standard:", error);
      }
    },
    addTable(table) {
      this.currentStandard.tables.push(table);
      this.showModal = null;
    },
    editColumn({ tableIndex, columnIndex }) {
      const col = this.currentStandard.tables[tableIndex].columns[columnIndex];
      this.editingColumn = {
        tableIndex,
        columnIndex,
        name: col.name,
        isNew: false,
      };
      this.showModal = "edit-column";
    },
    updateColumns({ tableIndex, columns }) {
      this.currentStandard.tables[tableIndex].columns = columns;
    },
    saveColumn(col) {
      if (col.isNew) {
        this.currentStandard.tables[col.tableIndex].columns.push({
          name: col.name,
        });
      } else {
        this.currentStandard.tables[col.tableIndex].columns[
          col.columnIndex
        ].name = col.name;
      }
      this.showModal = null;
    },
    removeColumn({ tableIndex, columnIndex }) {
      this.currentStandard.tables[tableIndex].columns.splice(columnIndex, 1);
    },
    removeTable(index) {
      this.currentStandard.tables.splice(index, 1);
    },
    confirmDeleteRow() {},
  },
});
