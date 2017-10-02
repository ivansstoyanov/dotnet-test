import { Component } from '@angular/core';
import { Router } from '@angular/router';

import { AppComponent } from '../app.component';
import { UserService } from '../services/user.service';
import { routerTransition } from '../router.animations';

@Component({
    templateUrl: 'login-page.html',
    styleUrls: ['./login-page.scss'],
    animations: [routerTransition()]
})
export class LoginPage {
    email: string;// = 'test@example.com';
    password: string;//= test12

    constructor(public user: UserService, private router: Router, private appCmp: AppComponent) {
    }

    doLogin(provider: string) {
        var self = this;

        self.user.login(provider).then((resp) => {
            if (resp) {
                self.router.navigate(['/admin']);
            } else {
                self.appCmp.addAlert({
                    id: '111',
                    type: 'danger',
                    message: resp,
                    timeout: 5000
                });
            }
        }, (err) => {
            self.appCmp.addAlert({
                id: '111',
                type: 'danger',
                message: err,
                timeout: 5000
            });
        });
    }

    userSign() {
        var self = this;

        self.user.signWithUserAndPass(self.email, self.password).then((resp) => {
            if (resp) {
                self.router.navigate(['/admin']);
            } else {
                self.appCmp.addAlert({
                    id: '111',
                    type: 'danger',
                    message: resp,
                    timeout: 5000
                });
            }
        }, (err) => {
            self.appCmp.addAlert({
                id: '111',
                type: 'danger',
                message: err,
                timeout: 5000
            });
        });
    }
}
