import { Injectable } from '@angular/core';
import {
    CanActivate, Router,
    ActivatedRouteSnapshot,
    RouterStateSnapshot,
    CanActivateChild,
    NavigationExtras,
    CanLoad, Route
} from '@angular/router';
import { AuthService } from '../services/auth.service';

@Injectable()
export class SystemAdminGuard implements CanActivate, CanActivateChild, CanLoad {
    constructor(private authService: AuthService, private router: Router) { }

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
        if (this.authService.isLoggedIn) {
            return true;
        }

        this.router.navigate(['/login']);
        return false;
    }
}