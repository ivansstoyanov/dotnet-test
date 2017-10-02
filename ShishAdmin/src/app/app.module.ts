import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AngularFireModule } from 'angularfire2';
import { AngularFireAuth } from 'angularfire2/auth';

import { AppComponent } from './app.component';
import { AppRoutingModule } from './app-routing.module';
import { AdminGuard } from './route-guards/admin-guard.service';
import { SystemAdminGuard } from './route-guards/system-admin-guard.service';

import { AuthService } from './services/auth.service';
import { UserService } from './services/user.service';
import { Api } from './services/api.service';
import { StorageService } from './services/storage.service';
import { TabaccoService } from './services/tabacco.service';
import { QrCodeService } from './services/qrcode.service';

import { IndexPage } from './index';
import { PageNotFound } from './page-not-found';

import { NgbAlertModule } from '@ng-bootstrap/ng-bootstrap';

export const firebaseConfig = {
    apiKey: "AIzaSyClgHEjEdNlyEXcv_CBk3QDwXr31ZhNbpA",
    authDomain: "shisha-74b04.firebaseapp.com",
    databaseURL: "https://shisha-74b04.firebaseio.com",
    projectId: "shisha-74b04",
    storageBucket: "shisha-74b04.appspot.com",
    messagingSenderId: "981286106277"
};

@NgModule({
  declarations: [
    AppComponent,
    IndexPage,
    PageNotFound
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    FormsModule,
    HttpModule,
    AppRoutingModule,
    AngularFireModule.initializeApp(firebaseConfig),
    NgbAlertModule.forRoot()
  ],
  providers: [
      AdminGuard,
      SystemAdminGuard,
      AuthService,
      UserService,
      Api,
      StorageService,
      TabaccoService,
      QrCodeService,
      AngularFireAuth
  ],
  bootstrap: [AppComponent]
})
export class AppModule {
  constructor(router: Router) {
  }
}
