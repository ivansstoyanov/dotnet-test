import { Component } from '@angular/core';
import { Router } from '@angular/router';

import { AppComponent } from '../app.component';
import { UserService } from '../services/user.service';
import { routerTransition } from '../router.animations';

@Component({
    templateUrl: 'register-page.html',
    styleUrls: ['./register-page.scss'],
    animations: [routerTransition()]
})
export class RegisterPage {
    username: string;
    email: string;
    password: string;
    password2: string;
  
    constructor(public userService: UserService, private router: Router, private appCmp: AppComponent) {

    }

    userSign() {
        var self = this;

        if (this.password != this.password2) {
            self.appCmp.addAlert({
                id: '33',
                type: 'warning',
                message: 'Passwords are different',
                timeout: 5000
            });
        }

        self.userService.createFirebaseUser(self.email, self.password, self.username).then((resp) => {
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
