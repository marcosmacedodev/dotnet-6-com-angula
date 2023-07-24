import { Injectable } from '@angular/core';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard {

  constructor(private router: Router){}

  canActivate(): boolean {
    if(localStorage.getItem('user') != null)
     return true;

    this.router.navigate(['/usuario/login']);
    return false;
  }
}


//canActivate: [() => inject(myGuard).canActivate()]
