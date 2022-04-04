import { ActorDetail } from './../models/ActorDetail';
import { useAlertStore } from './Alert';
import { ErrorResponse } from './../models/Responses/ErrorResponse';
import { SuccessResponse } from './../models/Responses/SuccessResponse';
import { DataResponse } from './../models/Responses/DataResponse';
import { AxiosError } from 'axios';
import { defineStore } from 'pinia';
import { api } from 'src/boot/axios';
import { Actor } from 'src/models/Actor';
interface ActorStore {
  actors: Actor[];
  actordetail:ActorDetail
}
export const useActorStore = defineStore({
  id: 'actor',
  state: (): ActorStore => ({
    actors: [],
    actordetail:{} as ActorDetail
  }),
  getters: {
    getActorsFromState(state: ActorStore): Actor[] {
      return state.actors;
    },
    getActor(state: ActorStore) {
      return (id: number) =>
        state.actors.find((actor) => actor.id === id) as Actor;
    },
    getActorDetailFromState(state:ActorStore){
      return state.actordetail
    }
  },
  actions: {
    async getActors() {
      await api.get<DataResponse<Actor[]>>('actors').then((response) => {
        this.actors = response.data.data;
      });
    },
    async getActorDetail(slug:string){
       await api.get<DataResponse<ActorDetail>>(`actors/getactordetail/${slug}`).then(response=>{
         if(response.data.success){
           this.actordetail=response.data.data;
         }
       }).catch((error: AxiosError) => {
        if (error.response && error.response.data) {
          const errorresponse = error.response.data as ErrorResponse;
          const alertStore = useAlertStore();
          alertStore.setErrors(errorresponse.errors);
        }
      });
    },
    async addActor(actor: FormData) {
      return await api
        .post<DataResponse<Actor>>('actors', actor)
        .then((response) => {
          if (response.data.success) {
            this.actors.push(response.data.data);
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
    async updateActor(actor: FormData) {
      return await api
        .put<SuccessResponse>('actors', actor)
        .then(async (response) => {
            await this.getActors();
            return response.data.message
        })
        .catch((error: AxiosError) => {
          if (error.response && error.response.data) {
            const errorresponse = error.response.data as ErrorResponse;
            const alertStore = useAlertStore();
            alertStore.setErrors(errorresponse.errors);
          }
        });
    },
    async removeActor(id: number) {
      return await api
        .delete<SuccessResponse>(`actors/${id}`)
        .then((response) => {
          if (response.data.success) {
            const index = this.actors.findIndex((c) => c.id == id);
            if (index > -1) {
              this.actors.splice(index, 1);
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
