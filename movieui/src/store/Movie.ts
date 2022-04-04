import { useFavoriteStore } from './Favorite';
import { AdminMovie } from './../models/AdminMovie';
import { DataResponse } from './../models/Responses/DataResponse';
import { ErrorResponse } from './../models/Responses/ErrorResponse';
import { useAlertStore } from './Alert';
import { SuccessResponse } from './../models/Responses/SuccessResponse';
import { api } from 'src/boot/axios';
import { defineStore } from 'pinia';
import { MovieDetail } from '../models/MovieDetail';
import { Movie } from 'src/models/Movie';
import { AxiosError } from 'axios';

interface MovieStore {
  movies: Movie[];
  moviedetail: MovieDetail;
  adminmovie: AdminMovie;
}

export const useMovieStore = defineStore({
  id: 'movie',
  state: (): MovieStore => ({
    movies: [],
    moviedetail: {} as MovieDetail,
    adminmovie: {} as AdminMovie,
  }),
  getters: {
    getMoviesFromState(state: MovieStore): Movie[] {
      return state.movies;
    },
    getMovieDetailFromState(state: MovieStore) {
      return state.moviedetail;
    },
    getAdminMovie(state: MovieStore) {
      return state.adminmovie;
    },
    getUserFavoriteMovies(state: MovieStore): Movie[] {
      const favoriteStore = useFavoriteStore();
      return state.movies.filter((x) =>
        favoriteStore.getFavoritesFromState.some((y) => y.movieId == x.id)
      );
    },
    getMovieBySearch(state: MovieStore) {
      return (searchtext: string) =>
        state.movies.filter((movie) => {
          return movie.name.toLowerCase().includes(searchtext.toLowerCase());
        });
    },
  },
  actions: {
    async getMovies() {
      await api.get<DataResponse<Movie[]>>('movies').then((response) => {
        if (response.data.success) {
          this.movies = response.data.data;
        }
      });
    },
    async getAdminMovieById(id: number) {
      await api
        .get<DataResponse<AdminMovie>>(`movies/getadminmovie/${id}`)
        .then((response) => {
          if (response.data.success) {
            this.adminmovie = response.data.data;
          }
        });
    },
    async addMovie(movie: FormData) {
      return await api
        .post<SuccessResponse>('movies', movie)
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
          }
        });
    },
    async updateMovie(movie: FormData) {
      return await api
        .put<SuccessResponse>('movies', movie)
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
          }
        });
    },
    async removeMovie(id: number) {
      return await api
        .delete<SuccessResponse>(`movies/${id}`)
        .then((response) => {
          if (response.data.success) {
            const index = this.movies.findIndex((m) => m.id == id);
            if (index > -1) {
              this.movies.splice(index, 1);
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
    async removeImageFromMovie(id: number) {
      return await api
        .delete<SuccessResponse>(`movieimages/${id}`)
        .then((response) => {
          if (response.data.success) {
            return response.data.message;
          }
        });
    },
    async getMovieDetail(slug: string) {
      await api
        .get<DataResponse<MovieDetail>>(`movies/getmoviedetail/${slug}`)
        .then((response) => {
          if (response.data.success) {
            this.moviedetail = response.data.data;
          }
        });
    },
  },
});
