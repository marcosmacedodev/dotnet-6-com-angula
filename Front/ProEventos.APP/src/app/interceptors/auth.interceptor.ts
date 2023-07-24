import { AccountService } from 'src/app/services/account.service';
import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable, take } from 'rxjs';
import { User } from '../models/Users/User';
import { NgxSpinnerService } from 'ngx-spinner';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {

  constructor(
    private account:AccountService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    let currentUser: User;
    this.account.currentUser?.pipe(take(1)).subscribe(user =>{
      currentUser = user!;
      if(currentUser!){
        request = request.clone({
          setHeaders:{
            Authorization: `Bearer ${currentUser.token}`
          }
        });
      }
    });

    return next.handle(request);
  }
}
