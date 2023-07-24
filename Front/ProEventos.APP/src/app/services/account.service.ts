import { User } from '../models/Users/User';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, ReplaySubject, map, take } from 'rxjs';
import { Login } from '../models/Users/Login';

@Injectable({
  providedIn: 'root'
})
export class AccountService{

  private currentUserSource = new ReplaySubject<User | null>(1);
  private currentUser$ = this.currentUserSource.asObservable();

  baseUrl = "https://localhost:7283/api/account";

  constructor(private http: HttpClient)
  {
    let user: User | null;
    if(localStorage.getItem('user'))
      user = JSON.parse(localStorage.getItem('user') ?? '{}');
    else
      user = null;

    if(user)
      this.setCurrentUser(user);
  }

  public get currentUser(): Observable<User|null>
  {
    return this.currentUser$;
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
      }
    }));
  }

  public isLogged(): boolean
  {
    return localStorage.getItem('user') != null;
  }

  public getUserName(): string{
    let userName: string = '';
    if(localStorage.getItem('user'))
    {
      const user: User = JSON.parse(localStorage.getItem('user')?? '{}');
      userName = user?.userName;
    }
    return userName;
  }

  public logout(): void {
    localStorage.removeItem('user');
    this.currentUserSource.next(null);
    this.currentUserSource.complete();
  }

  private setCurrentUser(user: User): void
  {
    localStorage.setItem('user', JSON.stringify(user));
    this.currentUserSource.next(user);
  }
}
