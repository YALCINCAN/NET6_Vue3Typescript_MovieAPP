import { useAlertStore } from './Alert';
import { ErrorResponse } from './../models/Responses/ErrorResponse';
import { SuccessResponse } from './../models/Responses/SuccessResponse';
import { DataResponse } from './../models/Responses/DataResponse';
import { AxiosError } from 'axios';
import { defineStore } from 'pinia';
import { api } from 'src/boot/axios';
import { Category } from 'src/models/Category';
interface CategoryStore {
  categories: Category[];
}
export const useCategoryStore = defineStore({
  id: 'category',
  state: (): CategoryStore => ({
    categories: [],
  }),
  getters: {
    getCategoriesFromState(state: CategoryStore): Category[] {
      return state.categories;
    },
    getCategory(state: CategoryStore) {
      return (id: number) =>
        state.categories.find((category) => category.id === id) as Category;
    },
  },
  actions: {
    async getCategories() {
      await api.get<DataResponse<Category[]>>('categories').then((response) => {
        this.categories = response.data.data;
      });
    },
    async addCategory(category: Category) {
      return await api
        .post<DataResponse<Category>>('categories', category)
        .then((response) => {
          if (response.data.success) {
            this.categories.push(response.data.data);
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
    async updateCategory(category: Category) {
      return await api
        .put<SuccessResponse>('categories', category)
        .then((response) => {
          if (response.data.success) {
            const index = this.categories.findIndex((c) => c.id == category.id);
            if (index > -1) {
              this.categories.splice(index, 1, category);
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
    async removeCategory(id: number) {
      return await api
        .delete<SuccessResponse>(`categories/${id}`)
        .then((response) => {
          if (response.data.success) {
            const index = this.categories.findIndex((c) => c.id == id);
            if (index > -1) {
              this.categories.splice(index, 1);
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
