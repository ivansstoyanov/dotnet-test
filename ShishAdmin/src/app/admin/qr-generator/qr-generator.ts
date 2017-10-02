import { Component } from '@angular/core';
import { routerTransition } from '../../router.animations';

import { AppComponent } from '../../app.component';
import { TabaccoService } from '../../services/tabacco.service';
import { QrCodeService } from '../../services/qrcode.service';

import { Tabacco } from '../../models/tabacco.model';

@Component({
  templateUrl: 'qr-generator.html',
  styleUrls: ['./qr-generator.scss'],
  animations: [routerTransition()]
})
export class QRGeneratorPage {
    tabaccos: Tabacco[];
    selectedTabaccos: Tabacco[];

    qrValue: string = '';
  
    constructor(private tabaccoService: TabaccoService, private qrCodeService: QrCodeService, private appCmp: AppComponent) {
        var self = this;
        this.tabaccos = [];
        this.selectedTabaccos = [];

        this.tabaccoService.getAll()
            .subscribe(
            data => {
                self.tabaccos = data;
            },
            error => {
                //TODO handel exception
            });
    }

    tabaccoSelected(tabacco: Tabacco) {
        if (this.selectedTabaccos.length >= 4) {
            this.appCmp.addAlert({
                id: '444',
                type: 'warning',
                message: 'You have reached the maximum of 4 tabaccos',
                timeout: 4000
            });
            return;
        }

        const index: number = this.tabaccos.indexOf(tabacco);
        this.tabaccos.splice(index, 1);

        this.selectedTabaccos.push(tabacco);
    }

    tabaccoDeselected(tabacco: Tabacco) {
        const index: number = this.selectedTabaccos.indexOf(tabacco);
        this.selectedTabaccos.splice(index, 1);

        this.tabaccos.unshift(tabacco);
    }

    generateQR() {
        var self = this;
        var tmpValue = '';
        for (var i = 0; i < this.selectedTabaccos.length; i++) {
            tmpValue += this.selectedTabaccos[i].internalId;
            tmpValue += ',';
        }
        tmpValue = tmpValue.substring(0, tmpValue.length - 1);
        
        this.qrCodeService.create(tmpValue).subscribe(
            data => {
                self.qrValue = data.value + "//" + data.id;
                  for (var i = 0; i < self.selectedTabaccos.length; i++) {
                      this.tabaccos.unshift(self.selectedTabaccos[i]);
                  }
                  self.selectedTabaccos = [];
            },
            error => {
                self.appCmp.addAlert({
                    id: '333',
                    type: 'danger',
                    message: error,
                    timeout: 5000
                });             
           });
    }
}
