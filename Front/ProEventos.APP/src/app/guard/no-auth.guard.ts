import { Injectable } from '@angular/core';
import { Router} from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class NoAuthGuard {

  constructor(private router: Router){}
  canActivateChild(
    ): boolean {
    const user = localStorage.getItem('user');
    if(user == null)
      return true;

    this.router.navigate(['/home']);
    return false;

  }

}
