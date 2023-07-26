import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Evento } from '../models/Evento';
import { Observable, take } from 'rxjs';
import { PageModel } from '../models/PageModel';

@Injectable({
  providedIn: 'root'
})
export class EventoService {

  baseUrl = "https://localhost:7283/api/eventos";

  constructor(private http: HttpClient) { }

  public getEventos(params: any): Observable<PageModel<Evento>>{
    return this.http.get<PageModel<Evento>>(this.baseUrl, {params: params}).pipe(take(1));
  }

  public getEventoById(id: number): Observable<Evento>{
    return this.http.get<Evento>(`${this.baseUrl}/${id}`).pipe(take(1));
  }

  public updateEvento(body: Evento): Observable<Evento>{
    return this.http.put<Evento>(`${this.baseUrl}/${body.id}`, body).pipe(take(1))
  }

  public deleteEvento(id: number): Observable<Evento>{
    return this.http.delete<Evento>(`${this.baseUrl}/${id}`).pipe(take(1));
  }

  public createEvento(body: Evento): Observable<Evento>{
    return this.http.post<Evento>(`${this.baseUrl}`, body).pipe(take(1));
  }
}
