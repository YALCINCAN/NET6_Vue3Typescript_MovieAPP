import { useAlertStore } from './Alert';
import { ErrorResponse } from './../models/Responses/ErrorResponse';
import { SuccessResponse } from './../models/Responses/SuccessResponse';
import { DataResponse } from './../models/Responses/DataResponse';
import { AxiosError } from 'axios';
import { defineStore } from 'pinia';
import { api } from 'src/boot/axios';
import { Favorite } from 'src/models/Favorite';
interface FavoriteStore {
  favorites: Favorite[];
}
export const useFavoriteStore = defineStore({
  id: 'favorite',
  state: (): FavoriteStore => ({
    favorites: [],
  }),
  getters: {
    getFavoritesFromState(state: FavoriteStore): Favorite[] {
      return state.favorites;
    },
  },
  actions: {
    async getFavorites() {
      await api.get<DataResponse<Favorite[]>>('favorites/getuserfavorites').then((response) => {
        this.favorites = response.data.data;
      });
    },
    async addToFavorite(movieid:number) {
      const payload = {
          MovieId:movieid
      }
      return await api
        .post<DataResponse<Favorite>>('favorites', payload)
        .then((response) => {
          if (response.data.success) {
            this.favorites.push(response.data.data);
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
    async removeFromFavorite(movieid: number) {
      return await api
        .delete<SuccessResponse>(`favorites/${movieid}`)
        .then((response) => {
          if (response.data.success) {
            const index = this.favorites.findIndex((c) => c.movieId == movieid);
            if (index > -1) {
              this.favorites.splice(index, 1);
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
