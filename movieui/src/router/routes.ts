//import { Pinia } from 'pinia';
import { RouteRecordRaw } from 'vue-router';

export default function (/*store: Pinia*/) {
  if (process.env.CLIENT && process.env.DEV) {
    //console.log(store)
  }
  const routes: RouteRecordRaw[] = [
    {
      path: '/',
      component: () => import('layouts/MainLayout.vue'),
      children: [
        { path: '', name: 'Home', component: () => import('pages/Home.vue') },
        {
          path: '/favorites',
          name: 'Favorites',
          component: () => import('pages/Favorites.vue'),
          meta: { requiresLogin: true },
        },
        {
          path: '/login',
          name: 'Login',
          component: () => import('pages/Login.vue'),
        },
        {
          path: '/register',
          name: 'Register',
          component: () => import('pages/Register.vue'),
        },
        {
          path: '/forgotpassword',
          name: 'ForgotPassword',
          component: () => import('pages/ForgotPassword.vue'),
        },
        {
          path: '/profile',
          name: 'Profile',
          component: () => import('pages/Profile.vue'),
          meta: { requiresLogin: true },
        },
        {
          path: '/changepassword',
          name: 'ChangePassword',
          component: () => import('pages/ChangePassword.vue'),
          meta: { requiresLogin: true },
        },
        {
          path: '/changeimage',
          name: 'ChangeImage',
          component: () => import('pages/ChangeImage.vue'),
          meta: { requiresLogin: true },
        },
        {
          path: '/resetpassword/:email/:token',
          name: 'ResetPassword',
          component: () => import('pages/ResetPassword.vue'),
        },
        {
          path: '/confirmemail/:userId/:token',
          name: 'ConfirmEmail',
          component: () => import('pages/ConfirmEmail.vue'),
        },
        {
          path: '/movie/:slug',
          name: 'MovieDetail',
          component: () => import('pages/MovieDetail.vue'),
        },
        {
          path: '/actor/:slug',
          name: 'ActorDetail',
          component: () => import('pages/ActorDetail.vue'),
        },
      ],
    },
    {
      path: '/dashboard',
      component: () => import('layouts/DashboardLayout.vue'),
      children: [
        {
          path: '/dashboard/movies',
          name: 'DashboardMovies',
          component: () => import('pages/dashboard/Movies.vue'),
          meta: { requiresAuth: true },
        },
        {
          path: '/dashboard/movies/add',
          name: 'DashboardAddMovie',
          component: () => import('pages/dashboard/AddMovie.vue'),
          meta: { requiresAuth: true },
        },
        {
          path: '/dashboard/movies/update/:id',
          name: 'DashboardMovieUpdate',
          component: () => import('pages/dashboard/UpdateMovie.vue'),
          meta: { requiresAuth: true },
        },
        {
          path: '/dashboard/actors',
          name: 'DashboardActors',
          component: () => import('pages/dashboard/Actors.vue'),
          meta: { requiresAuth: true },
        },
        {
          path: '/dashboard/categories',
          name: 'DashboardCategories',
          component: () => import('pages/dashboard/Categories.vue'),
          meta: { requiresAuth: true },
        },
        {
          path: '/dashboard/comments',
          name: 'DashboardComments',
          component: () => import('pages/dashboard/Comments.vue'),
          meta: { requiresAuth: true },
        },
        {
          path: '/dashboard/users',
          name: 'DashboardUsers',
          component: () => import('pages/dashboard/Users.vue'),
          meta: { requiresAuth: true },
        },
        {
          path: '/dashboard/roles',
          name: 'DashboardRoles',
          component: () => import('pages/dashboard/Roles.vue'),
          meta: { requiresAuth: true },
        },
      ],
    },

    // Always leave this as last one,
    // but you can also remove it
    {
      path: '/:catchAll(.*)*',
      component: () => import('pages/Error404.vue'),
    },
    {
      path: '/accessdenied',
      component: () => import('pages/AccessDenied.vue'),
    },
  ];

  return routes;
}
