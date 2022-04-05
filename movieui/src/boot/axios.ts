/* eslint-disable @typescript-eslint/no-unsafe-assignment */
/* eslint-disable @typescript-eslint/no-unsafe-member-access */
/* eslint-disable-next-line @typescript-eslint/no-unsafe-member-access*/

import { DataResponse } from './../models/Responses/DataResponse';
import { TokenDTO } from './../models/TokenDTO';
import { useAuthStore } from 'src/store/Auth';
import { boot } from 'quasar/wrappers';
import axios, { AxiosInstance, AxiosRequestConfig } from 'axios';

declare module '@vue/runtime-core' {
  interface ComponentCustomProperties {
    $axios: AxiosInstance;
  }
}
const baseUrl = 'http://localhost:7188/api/';
// Be careful when using SSR for cross-request state pollution
// due to creating a Singleton instance here;
// If any client changes this (global) instance, it might be a
// good idea to move this instance creation inside of the
// "export default () => {}" function below (which runs individually
// for each client)
const api = axios.create({ baseURL: baseUrl });
api.interceptors.request.use(
  (config: AxiosRequestConfig): AxiosRequestConfig => {
    const accessToken = localStorage.getItem('accessToken');
    if (accessToken) config.headers.Authorization = `Bearer ${accessToken}`;
    return config;
  }
);

api.interceptors.response.use(
  (config) => config,
  async (error) => {
    const originalRequest = error.config;

    if (
      error.response &&
      error.response.status === 401 &&
      error.config &&
      !error.config._isRetry
    ) {
      originalRequest._isRetry = true;
      const refreshToken = localStorage.getItem('refreshToken');
      const payload = {
        RefreshToken: refreshToken,
      };
      try {
        await axios
          .post<DataResponse<TokenDTO>>(`${baseUrl}auth/refreshtoken`, payload)
          .then(async (response) => {
            if (response.data.success) {
              localStorage.setItem(
                'accessToken',
                response.data.data.accessToken
              );
              localStorage.setItem(
                'refreshToken',
                response.data.data.refreshToken
              );
              const authStore = useAuthStore();
              await authStore.getAuthenticatedUser();
            }
          });
        return api.request(originalRequest);
      } catch (e) {
        throw new Error('Unauthtorized');
      }
    } else throw error;
  }
);

export default boot(({ app }) => {
  // for use inside Vue files (Options API) through this.$axios and this.$api

  app.config.globalProperties.$axios = axios;
  // ^ ^ ^ this will allow you to use this.$axios (for Vue Options API form)
  //       so you won't necessarily have to import axios in each vue file

  app.config.globalProperties.$api = api;
  // ^ ^ ^ this will allow you to use this.$api (for Vue Options API form)
  //       so you can easily perform requests against your app's API
});

export { api };
