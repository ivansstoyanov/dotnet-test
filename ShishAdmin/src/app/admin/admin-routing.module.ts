import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { AdminComponent } from './admin.component';
import { AdminDashboardPage } from './dashboard/dashboard';
import { QRGeneratorPage } from './qr-generator/qr-generator';

import { AdminGuard } from '../route-guards/admin-guard.service';
import { SystemAdminGuard } from '../route-guards/system-admin-guard.service';

const adminRoutes: Routes = [
  {
    path: '',
    component: AdminComponent,
    canActivate: [AdminGuard],
    children: [
      {
          path: '',
          canActivate: [AdminGuard],
          component: AdminDashboardPage
      }, {
          path: 'qr-generator',
          canActivate: [AdminGuard],
          component: QRGeneratorPage
      }
      //  , { // System Admin Guard for hidden menus 
      //    path: 'qr-generator',
      //    canActivate: [SystemAdminGuard],
      //    component: QRGeneratorPage
      //}
    ]
  }
];

@NgModule({
    imports: [
        RouterModule.forChild(adminRoutes)
    ],
    exports: [
        RouterModule
    ]
})
export class AdminRoutingModule { }