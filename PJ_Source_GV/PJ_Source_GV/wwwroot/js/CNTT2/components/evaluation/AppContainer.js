Vue.component("app-container", {
  template: `
    <div class="container-fluid">
      <evaluations-list
        v-if="currentPage === 'evaluations-list'"
        :evaluations="evaluations"
        @create="showCreateEvaluationModal"
        @edit="editEvaluation"
      />
      <modal-create-evaluation
        v-if="showModal === 'create-evaluation'"
        :standards="standards"
        :evaluations="evaluations"
        @created="onCreatedNewEvaluation"
        @close="hideModal"
      />
      <evaluation-form
        v-if="currentPage === 'evaluations-form'"
        :selected-evaluation="selectedEvaluation"
        @back="currentPage = 'evaluations-list'"
      />
      <loading-spinner type="ripple" color="orange" message="Vui lòng chờ..." v-if="showLoading"></loading-spinner>
    </div>
  `,
  data() {
    return {
      currentPage: "evaluations-list",
      showModal: null,
      standards: [],
      evaluations: [],
      currentEvoluation: { id: "", name: "", tables: [] },
      selectedEvaluation: {
        id: 3,
        name: "Đánh giá test",
        description: "Đánh giá tiến độ và chất lượng dự án X trong quý 2",
        startDate: "2025-04-01",
        endDate: "2025-06-30",
        standards: null,
        createdAt: "2025-03-20T00:00:00Z",
      },
      showLoading: false,
    };
  },
  created() {
    this.fetchEvaluations();
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
    async fetchEvaluations() {
      this.showLoading = true;
      try {
        const items = await this.apiRequest(
          "http://localhost:28635/API/evaluation",
          "GET"
        );
        this.evaluations = [...items];
        this.showLoading = false;
      } catch (err) {
        this.showLoading = false;
        console.error("Lỗi khi lấy evaluations:", err);
      }
    },
    showCreateEvaluationModal() {
      this.showModal = "create-evaluation";
    },
    editEvaluation(item) {
      console.log(item);
      this.selectedEvaluation.id = item.id;
      this.selectedEvaluation.name = item.name;
      this.currentPage = "evaluations-form";
    },
    hideModal() {
      this.showModal = null;
    },
    onCreatedNewEvaluation() {
      this.hideModal();
      this.fetchEvaluations();
    },
  },
});
