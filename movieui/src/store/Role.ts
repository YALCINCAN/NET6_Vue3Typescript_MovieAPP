import { useUserStore } from './User';
import { AssignRole } from './../models/AssignRole';
import { useAlertStore } from './Alert';
import { ErrorResponse } from './../models/Responses/ErrorResponse';
import { SuccessResponse } from './../models/Responses/SuccessResponse';
import { DataResponse } from './../models/Responses/DataResponse';
import { AxiosError } from 'axios';
import { defineStore } from 'pinia';
import { api } from 'src/boot/axios';
import { Role } from 'src/models/Role';

interface RoleStore {
  roles: Role[];
  assignedroles:AssignRole[];    
}
export const useRoleStore = defineStore({
  id: 'role',
  state: (): RoleStore => ({
    roles: [],
    assignedroles:[]
  }),
  getters: {
    getRolesFromState(state: RoleStore): Role[] {
      return state.roles;
    },
    getRole(state: RoleStore) {
      return (id: string) =>
        state.roles.find((role) => role.id === id) as Role;
    },
    getAssignedRolesFromState(state:RoleStore) :AssignRole[]{
      return state.assignedroles;
    }
  },
  actions: {
    async getRoles() {
      await api.get<DataResponse<Role[]>>('roles').then((response) => {
        this.roles = response.data.data;
      });
    },
    async getAssignedRolesByUserId(userid:string){
      await api.get<DataResponse<AssignRole[]>>(`roles/getassignedroles/${userid}`).then(response=>{
        if (response.data.success){
          this.assignedroles=response.data.data;
        }
      }).catch((error: AxiosError) => {
        if (error.response && error.response.data) {
          const errorresponse = error.response.data as ErrorResponse;
          const alertStore = useAlertStore();
          alertStore.setErrors(errorresponse.errors);
        }
      });
    },
    async assignRole(){
      return await api.post<SuccessResponse>('roles/roleassign',this.assignedroles).then(async response=>{
        if(response.data.success){
          const userStore = useUserStore();
          await userStore.getUsersWithRoles();
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
    async addRole(role: Role) {
      return await api
        .post<DataResponse<Role>>('roles', role)
        .then((response) => {
          if (response.data.success) {
            this.roles.push(response.data.data);
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
    async updateRole(role: Role) {
      return await api
        .put<SuccessResponse>('roles', role)
        .then((response) => {
          if (response.data.success) {
            const index = this.roles.findIndex((c) => c.id == role.id);
            if (index > -1) {
              this.roles.splice(index, 1, role);
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
    async removeRole(id: string) {
      return await api
        .delete<SuccessResponse>(`roles/${id}`)
        .then((response) => {
          if (response.data.success) {
            const index = this.roles.findIndex((c) => c.id == id);
            if (index > -1) {
              this.roles.splice(index, 1);
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
