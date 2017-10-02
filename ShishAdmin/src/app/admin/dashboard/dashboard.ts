import { Component } from '@angular/core';
import { routerTransition } from '../../router.animations';

@Component({
  selector: 'admin-dashboard-page',
  templateUrl: 'dashboard.html',
  styleUrls: ['./dashboard.scss'],
  animations: [routerTransition()]
})
export class AdminDashboardPage {
    constructor() {        
    }
}
