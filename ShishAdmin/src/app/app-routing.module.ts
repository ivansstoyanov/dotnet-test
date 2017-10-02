import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { AdminGuard } from './route-guards/admin-guard.service';
import { SelectivePreloadingStrategy } from './route-guards/selective-preloading';

import { IndexPage } from './index';
import { PageNotFound } from './page-not-found';

const appRoutes: Routes = [
    {
        path: '',
        pathMatch: 'full',
        component: IndexPage
    },
    {
        path: 'admin',
        loadChildren: 'app/admin/admin.module#AdminModule',
        canLoad: [AdminGuard]
    },
    {
        path: 'login',
        loadChildren: 'app/login/login.module#LoginModule'
    },
    {
        path: '**',
        component: PageNotFound
    }
];

@NgModule({
  imports: [
    RouterModule.forRoot(
      appRoutes,
      {
        //enableTracing: true, // <-- debugging purposes only
        preloadingStrategy: SelectivePreloadingStrategy,
      }
    )
  ],
  exports: [
    RouterModule
  ],
  providers: [
    SelectivePreloadingStrategy
  ]
})
export class AppRoutingModule { }
