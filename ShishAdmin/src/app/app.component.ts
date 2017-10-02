import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
    private alerts: Array<any> = [];

    constructor() {
    }

    public addAlert(alert: any) {
        var self = this;
        this.alerts.push(alert);

        if (alert.timeout) {
            setTimeout(() =>
                self.closeAlert(alert),
              alert.timeout);
        }
    }

    public closeAlert(alert: any) {
        const index: number = this.alerts.indexOf(alert);
        this.alerts.splice(index, 1);
    }
}
