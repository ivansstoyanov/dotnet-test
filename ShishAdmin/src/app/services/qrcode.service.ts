import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import 'rxjs/add/operator/map';

import { Api } from './api.service';

//import { Tabacco } from '../models/tabacco.model';

@Injectable()
export class QrCodeService {
    constructor(public http: Http, public api: Api) {
    }

    create(value: string) {
        return this.api.post('qrcode', "'" + value + "'")
            .map(resp => resp.json());
    }

    getAll() {
        return this.api.get('qrgenerator', null, 'getAll')
            .map(resp => resp.json());
    }

}
