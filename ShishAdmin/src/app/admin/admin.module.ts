import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgbDropdownModule, NgbCarouselModule, NgbAlertModule } from '@ng-bootstrap/ng-bootstrap';
import { NgxQRCodeModule } from 'ngx-qrcode2';

import { AdminComponent } from './admin.component';
import { AdminDashboardPage } from './dashboard/dashboard';
import { QRGeneratorPage } from './qr-generator/qr-generator';

import { AdminRoutingModule } from './admin-routing.module';
import { HeaderComponent, SidebarComponent, StatModule } from '../shared';

@NgModule({
    imports: [
        CommonModule,
        AdminRoutingModule,
        StatModule,

        NgbDropdownModule.forRoot(),
        NgbCarouselModule.forRoot(),
        NgbAlertModule.forRoot(),
        NgxQRCodeModule
    ],
    declarations: [
        AdminComponent,
        AdminDashboardPage,
        QRGeneratorPage,

        HeaderComponent,
        SidebarComponent
    ]
})
export class AdminModule { }