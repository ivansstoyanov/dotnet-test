import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import 'rxjs/add/operator/map';

import { Api } from './api.service';
import { StorageService } from './storage.service';

@Injectable()
export class AuthService {
    isLoggedIn = false;

    constructor(public http: Http, public api: Api, public storageService: StorageService) {
    }

    getToken(params?: any) {
        var self = this;

        var promise = new Promise<any>((resolve, reject) => {
            self.api.post('tokenauth', params)
                .map(resp => {
                    let result = resp.json();

                    if (result.state == 1) {
                        let json = result.data as any;

                        self.storageService.setValue('token', json.accessToken);

                        self.api.get('tokenauth').map(
                            userInfo => {
                                self.storageService.setObject('currentUser', userInfo.json());
                                resolve(true);
                            },
                            error => {
                                //todo add global exception handling for 401
                                //todo say if the user data is not returned
                                reject(error);
                            }).subscribe();
                    } else {
                        reject(null);
                    }
                }).subscribe();

        });

        return promise;
    }

    logout() {
        this.storageService.removeValue('token');
        this.storageService.removeValue('currentUser');
    }
}