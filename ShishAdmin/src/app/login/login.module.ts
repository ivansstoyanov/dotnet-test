import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { LoginComponent } from './login.component';
import { LoginPage } from './login-page';
import { RegisterPage } from './register-page';

import { LoginRoutingModule } from './login-routing.module';

import { FormsModule } from '@angular/forms';

@NgModule({
    imports: [
        CommonModule,
        LoginRoutingModule,
        FormsModule
    ],
    declarations: [
        LoginComponent,
        LoginPage,
        RegisterPage
    ]
})
export class LoginModule { }