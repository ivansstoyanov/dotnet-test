import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { LoginComponent } from './login.component';
import { LoginPage } from './login-page';
import { RegisterPage } from './register-page';

const loginRoutes: Routes = [
  {
    path: '',
    component: LoginComponent,
    children: [
      {
        path: '',
        children: [
          { path: '', component: LoginPage },
          { path: 'register', component: RegisterPage }
        ]
      }
    ]
  }
];

@NgModule({
  imports: [
    RouterModule.forChild(loginRoutes)
  ],
  exports: [
    RouterModule
  ]
})
export class LoginRoutingModule { }