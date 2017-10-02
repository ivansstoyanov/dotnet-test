import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Rx';
import { Http, RequestOptions, Headers } from '@angular/http';
import 'rxjs/add/operator/map';

import { StorageService } from './storage.service';

@Injectable()
export class Api {
    //VS Api
    //url: string = 'http://localhost:6567/api';
    //VS Code Api
    //url: string = 'http://localhost:5000/api';
    //AppHb Api
    url: string = 'http://shishaweb.apphb.com/api';

    constructor(public http: Http, public storageService: StorageService) {
        
    }

    get(endpoint: string, params?: any, action?: any, options?: RequestOptions) {
        let headers = this.initAuthHeaders();
        if (!options) {
            options = new RequestOptions();
            options.headers = headers;
        }

        if (action) {
            endpoint += '/' + action;
        }

        if (params) {
            endpoint += '/' + params;
        }

        return this.http.get(this.url + '/' + endpoint, options);
    }

    post(endpoint: string, body: any, action?: any, options?: RequestOptions) {
        let headers = this.initAuthHeaders();
        if (!options) {
            options = new RequestOptions();
            options.headers = headers;
        }

        if (action) {
            endpoint += '/' + action;
        }

        return this.http.post(this.url + '/' + endpoint, body, options).catch((error) => {
            if (error && error.status == 0 && error.type == 3 && !error.ok) {
                //let toast = this.toastCtrl.create({
                //    message: "Cannot connect to server",
                //    duration: 3000,
                //    position: 'top'
                //});

                //toast.present();
            }

            return Observable.throw(error);
        });
    }

    //put(endpoint: string, body: any, action?: any, options?: RequestOptions) {
    //    let headers = this.initAuthHeaders();
    //    if (!options) {
    //        options = new RequestOptions();
    //        options.headers = headers;
    //    }

    //    if (action) {
    //        endpoint += '/' + action;
    //    }

    //    return this.http.put(this.url + '/' + endpoint, body, options);
    //}

    //delete(endpoint: string, options?: RequestOptions) {
    //    let headers = this.initAuthHeaders();
    //    if (!options) {
    //        options = new RequestOptions();
    //        options.headers = headers;
    //    }

    //    return this.http.delete(this.url + '/' + endpoint, options);
    //}

    private initAuthHeaders(): Headers {
        var headers = new Headers();

        headers.append('Content-Type', 'application/json');
        var token = this.storageService.getValue('token');
        if (token != null) {
            headers.append("Authorization", "Bearer " + token);
        }

        return headers;
    }

    // private handleError(error: any): Promise<any> {
    //   console.error('An error occurred', error);
    //   return Promise.reject(error.message || error);
    // }
}
