import { AdminComment } from './../models/AdminComment';
import { useAlertStore } from './Alert';
import { ErrorResponse } from './../models/Responses/ErrorResponse';
import { SuccessResponse } from './../models/Responses/SuccessResponse';
import { DataResponse } from './../models/Responses/DataResponse';
import { AxiosError } from 'axios';
import { defineStore } from 'pinia';
import { api } from 'src/boot/axios';
import { Comment } from 'src/models/Comment';
interface CommentStore {
  admincomments:AdminComment[]
}
export const useCommentStore = defineStore({
  id: 'comment',
  state: (): CommentStore => ({
    admincomments: [],
  }),
  getters: {
    getAdminCommentsFromState(state: CommentStore): AdminComment[] {
      return state.admincomments;
    },
  },
  actions: {
    async getComments() {
      await api.get<DataResponse<AdminComment[]>>('comments').then((response) => {
        this.admincomments = response.data.data;
      });
    },
    async addComment(comment: Comment) {
      return await api
        .post<DataResponse<Comment>>('comments', comment)
        .then((response) => {
          if (response.data.success) {
            return response.data.message;
          }
        })
        .catch((error: AxiosError) => {
          if (error.response && error.response.data) {
            const errorresponse = error.response.data as ErrorResponse;
            const alertStore = useAlertStore();
            alertStore.setErrors(errorresponse.errors);
            console.log(errorresponse)
          }
        });
    },
    async removeComment(id: number) {
      return await api
        .delete<SuccessResponse>(`comments/removecommentbyadmin/${id}`)
        .then((response) => {
          if (response.data.success) {
            const index = this.admincomments.findIndex((c) => c.id == id);
            if (index > -1) {
              this.admincomments.splice(index, 1);
            }
            return response.data.message;
          }
        })
        .catch((error: AxiosError) => {
          if (error.response && error.response.data) {
            const errorresponse = error.response.data as ErrorResponse;
            const alertStore = useAlertStore();
            alertStore.setErrors(errorresponse.errors);
          }
        });
    },
  },
});
