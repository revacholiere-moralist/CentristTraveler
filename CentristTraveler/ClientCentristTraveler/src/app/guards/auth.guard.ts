import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';
import { AuthService } from '../services/auth/auth.service';

@Injectable({ providedIn: 'root' })
export class AuthGuard implements CanActivate {

  jwtHelperService = new JwtHelperService();
  constructor(
    private router: Router,
    private authService: AuthService
  ) { }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    let token = this.jwtHelperService.decodeToken(localStorage.getItem('token'));
    
    if (token) {
      // check if route is restricted by role
      if (route.data.roles && token['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'].indexOf(route.data.roles) === -1) {
        // role not authorised so redirect to home page
        this.router.navigate(['/post/posts']);
        return false;
      }

      // authorised so return true
      return true;
    }
    // not logged in so redirect to login page with the return url
    this.router.navigate(['/post/posts']);
    return false;
  }
}
