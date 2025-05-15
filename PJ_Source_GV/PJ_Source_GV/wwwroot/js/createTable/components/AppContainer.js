Vue.component("app-container", {
  template: `
    <div>
      <standards-list
        v-if="currentPage === 'standards-list'"
        :standards="standards"
        @create="showCreateStandardModal"
        @edit="editStandard"
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
    </div>
  `,
  data() {
    return {
      currentPage: "standards-list",
      showModal: null,
      standards: [],
      currentStandard: { id: "", name: "", tables: [] },
      editingColumn: { tableIndex: -1, columnIndex: -1, name: "", isNew: true },
    };
  },
  created() {
    this.standards.push({
      id: "TC01",
      name: "Tiêu chuẩn 1",
      createdDate: "01/05/2025",
      tables: [
        {
          name: "Bảng 1A",
          columns: [
            { name: "Tiêu chí đánh giá" },
            { name: "Điểm tối đa" },
            { name: "Điểm đánh giá" },
          ],
        },
      ],
    });
  },
  methods: {
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
      const id = "TC" + (this.standards.length + 1).toString().padStart(2, "0");
      this.currentStandard = {
        id,
        name: newStd.name,
        createdDate: new Date().toLocaleDateString("vi-VN"),
        tables: [],
      };
      this.currentPage = "edit-standard";
      this.showModal = null;
    },
    saveStandard(updated) {
      const idx = this.standards.findIndex((s) => s.id === updated.id);
      if (idx >= 0) this.standards.splice(idx, 1, updated);
      else this.standards.push(updated);
      this.currentPage = "standards-list";
    },
    addTable(table) {
      console.log("Adding table:", table);
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
  },
});
