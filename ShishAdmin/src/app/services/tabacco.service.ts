import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import 'rxjs/add/operator/map';

import { Api } from './api.service';

import { Tabacco } from '../models/tabacco.model';

@Injectable()
export class TabaccoService {
    tabaccos: Tabacco[] = [];

    defaultItem: any = {
    };

    constructor(public http: Http, public api: Api) {
    }

    getAll() {
        return this.api.get('tabaccos', null, 'getall')
            .map(resp => resp.json());
    }

    query(params?: any) {
        return this.api.get('tabaccos', params)
            .map(resp => resp.json());
    }

}
