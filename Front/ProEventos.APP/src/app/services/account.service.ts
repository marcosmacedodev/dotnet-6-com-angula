import { User } from '../models/Users/User';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, map, take } from 'rxjs';
import { Login } from '../models/Users/Login';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AccountService{

  private currentUserSource :BehaviorSubject<User|null>;
  public currentUser$: Observable<User|null>

  baseUrl = "https://localhost:7283/api/account";

  constructor(private http: HttpClient, private router: Router)
  {
    const user = JSON.parse(localStorage.getItem('user')!) as User;
    this.currentUserSource = new BehaviorSubject<User|null>(user);
    this.currentUser$ = this.currentUserSource.asObservable();
  }

  public createUser(body: User): Observable<void> {
    return this.http.post<User>(`${this.baseUrl}/createuser`, body).pipe(take(1), map((response: User) => {
      const user = response;
      if (user) {
        this.setCurrentUser(user);
      }
    }));
  }

  public getUser(): Observable<User> {
    return this.http.get<User>(`${this.baseUrl}/user`).pipe(take(1));
  }

  public updateUser(body: User): Observable<void>{
    return this.http.put<User>(`${this.baseUrl}/updateuser`, body).pipe(take(1), map((response: User) => {
      const user = response;
      if (user) {
        this.setCurrentUser(user);
      }
    }));
  }

  public login(body: Login): Observable<void>
  {
    return this.http.post<User>(`${this.baseUrl}/login`, body).pipe(take(1), map((response: User) => {
      const user = response;
      if (user) {
        this.setCurrentUser(user);
        this.router.navigate(['/home']);
      }
    }));
  }

  public uploadImage(file: File): Observable<void>
  {
    const formData = new FormData();
    formData.append("file", file);
    return this.http.post<void>(`${this.baseUrl}/image`, formData).pipe(take(1));
  }

  public isLogged(): boolean
  {
    const exp1 = localStorage.getItem('user') != null;
    const exp2 = this.currentUserSource.value != null;
    return exp1 || exp2;
  }

  public getUserName(): string|null
  {
    let username = null
    if(this.currentUserSource.value != null)
    {
      username = (this.currentUserSource.value as User).userName;
    }
    return username;
  }

  public logout(): void
  {
    localStorage.removeItem('user');
    this.currentUserSource.next(null);
    this.currentUserSource.complete();
    this.router.navigate(['/usuario/login']);
  }

  private setCurrentUser(user: User): void
  {
    localStorage.setItem('user', JSON.stringify(user));
    this.currentUserSource.next(user);
  }
}
