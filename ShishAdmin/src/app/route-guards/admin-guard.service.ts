import { Injectable } from '@angular/core';
import {
    CanActivate, Router,
    ActivatedRouteSnapshot,
    RouterStateSnapshot,
    CanActivateChild,
    NavigationExtras,
    CanLoad, Route
} from '@angular/router';

import { StorageService } from '../services/storage.service';

@Injectable()
export class AdminGuard implements CanActivate, CanActivateChild, CanLoad {
    constructor(private router: Router, public storageService: StorageService) { }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
        //let url: string = state.url;

        return this.checkLogin();
    }

    canActivateChild(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
        return this.canActivate(route, state);
    }

    canLoad(route: Route): boolean {
        //let url = `/${route.path}`;

        return this.checkLogin();
    }

    checkLogin(): boolean {
        if (this.storageService.getValue('token') != null && this.storageService.getValue('currentUser') != null) {
            return true;
        }

        this.router.navigate(['/login']);
        return false;
    }
}