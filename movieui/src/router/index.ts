import { JwtPayload } from './../models/JwtPayload';
//import { Pinia } from 'pinia';
import { route } from 'quasar/wrappers';
//import { useAuthStore } from 'src/store/Auth';
import jwtDecode from 'jwt-decode';
import {
  createMemoryHistory,
  createRouter,
  createWebHashHistory,
  createWebHistory,
} from 'vue-router';
import routes from './routes';

/*
 * If not building with SSR mode, you can
 * directly export the Router instantiation;
 *
 * The function below can be async too; either use
 * async/await or return a Promise which resolves
 * with the Router instance.
 */

export default route(function (
  {
    /*store*/
  }
) {
  //const pinia = store as Pinia
  const createHistory = process.env.SERVER
    ? createMemoryHistory
    : process.env.VUE_ROUTER_MODE === 'history'
    ? createWebHistory
    : createWebHashHistory;

  const Router = createRouter({
    scrollBehavior: () => ({ left: 0, top: 0 }),
    routes: routes(),

    // Leave this as is and make changes in quasar.conf.js instead!
    // quasar.conf.js -> build -> vueRouterMode
    // quasar.conf.js -> build -> publicPath
    history: createHistory(
      process.env.MODE === 'ssr' ? void 0 : process.env.VUE_ROUTER_BASE
    ),
  });
  Router.beforeEach((to) => {
    if (to.meta.requiresAuth) {
      const accessToken = window.localStorage.getItem('accessToken');
      if (accessToken != null) {
        const decoded = jwtDecode<JwtPayload>(accessToken);
        const roles =
          decoded[
            'http://schemas.microsoft.com/ws/2008/06/identity/claims/role'
          ];
        if (!roles.includes('Admin')) {
          return {
            path: '/accessdenied',
          };
        }
      }
      else{
        return{
          path:'/login'
        }
      }
    }

    if (to.meta.requiresLogin) {
      const accessToken = window.localStorage.getItem('accessToken');
      if (accessToken == null) {
        return {
          path: '/login',
        };
      }
    }
  });

  return Router;
});
