Vue.component("app-container", {
  template: `
    <div class="container-fluid">
      <evaluations-list
        v-if="currentPage === 'evaluations-list'"
        @create="showCreateEvaluationModal"
        @edit="editEvaluation"
        ref="evaluationsList"
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
  methods: {
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
      this.$refs.evaluationsList.fetchEvaluations();
    },
  },
});
