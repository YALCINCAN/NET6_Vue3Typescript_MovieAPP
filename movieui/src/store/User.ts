import { ChangePassword } from './../models/ChangePassword';
import { useAuthStore } from 'src/store/Auth';
import { ResetPassword } from './../models/ResetPassword';
import { ForgotPassword } from './../models/ForgotPassword';
import { ChangePasswordUserByAdmin } from '../models/ChangePasswordUserByAdmin';
import { UserWithRoles } from './../models/UserWithRoles';
import { useAlertStore } from './Alert';
import { ErrorResponse } from './../models/Responses/ErrorResponse';
import { SuccessResponse } from './../models/Responses/SuccessResponse';
import { DataResponse } from './../models/Responses/DataResponse';
import { AxiosError } from 'axios';
import { defineStore } from 'pinia';
import { api } from 'src/boot/axios';
import { User } from 'src/models/User';


interface UserStore {
  userswithroles: UserWithRoles[];
}
export const useUserStore = defineStore({
  id: 'user',
  state: (): UserStore => ({
    userswithroles: [],
  }),
  getters: {
    getUserswithRolesFromState(state: UserStore): UserWithRoles[] {
      return state.userswithroles;
    },
    getUser(state: UserStore) {
      return (id: string) =>
        state.userswithroles.find(
          (userwithroles) => userwithroles.user.id === id
        )?.user;
    },
  },
  actions: {
    async getUsersWithRoles() {
      await api
        .get<DataResponse<UserWithRoles[]>>('users/getuserswithroles')
        .then((response) => {
          this.userswithroles = response.data.data;
        });
    },
    async removeUser(id: string) {
      return await api
        .delete<SuccessResponse>(`users/${id}`)
        .then(async (response) => {
          if (response.data.success) {
            await this.getUsersWithRoles();
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
    async passwordChangeByAdmin(passwordchangedata: ChangePasswordUserByAdmin) {
      return await api
        .put<SuccessResponse>('users/passwordchangebyadmin', passwordchangedata)
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
    async changePassword(changepassworddata:ChangePassword){
      return await api.put<SuccessResponse>('users/changepassword',changepassworddata).then(response=>{
        if(response.data.success){
          return response.data.message;
        }
      }).catch((error: AxiosError) => {
        if (error.response && error.response.data) {
          const errorresponse = error.response.data as ErrorResponse;
          const alertStore = useAlertStore();
          alertStore.setErrors(errorresponse.errors);
        }
      });
    },
    async updateUserByAdmin(user: User) {
      return await api
        .put<SuccessResponse>('users/updateuserbyadmin', user)
        .then(async (response) => {
          if (response.data.success) {
            await this.getUsersWithRoles();
            const authStore = useAuthStore();
            await authStore.getAuthenticatedUser();
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
    async changeImageByAdmin(changeimagedata: FormData) {
      return await api
        .post<SuccessResponse>('users/changeimagebyadmin', changeimagedata)
        .then(async (response) => {
          if (response.data.success) {
            await this.getUsersWithRoles();
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
    async changeImage(changeimagedata: FormData) {
      return await api
        .post<SuccessResponse>('users/changeimage', changeimagedata)
        .then(async (response) => {
          if (response.data.success) {
            const authStore = useAuthStore();
            await authStore.getAuthenticatedUser();
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
    async forgotPassword(forgotpasswordata: ForgotPassword) {
      await api
        .post<SuccessResponse>('users/forgotpassword', forgotpasswordata)
        .then((response) => {
          if (response.data.success) {
            const alertStore = useAlertStore();
            alertStore.setMessage(response.data.message);
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
    async resetPassword(resetpassworddata: ResetPassword) {
      await api
        .post<SuccessResponse>('users/resetpassword', resetpassworddata)
        .then((response) => {
          if (response.data.success) {
            const alertStore = useAlertStore();
            alertStore.setMessage(response.data.message);
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
    async updateUser(user : User){
      return await api.put<SuccessResponse>('users',user).then(response=>{
        if(response.data.success){
          const authStore = useAuthStore();
          authStore.updateUser(user);
          return response.data.message;
        }
      }).catch((error: AxiosError) => {
        if (error.response && error.response.data) {
          const errorresponse = error.response.data as ErrorResponse;
          const alertStore = useAlertStore();
          alertStore.setErrors(errorresponse.errors);
        }
      });
    }
  },
});
