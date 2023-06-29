import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Evento } from '../models/Evento';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class EventoService {
  baseUrl = "https://localhost:7283/api/eventos";
  constructor(private http: HttpClient) { }

  public getEventos(): Observable<Evento[]>{
    return this.http.get<Evento[]>(this.baseUrl);
  }

  public getEventosByTema(tema: string): Observable<Evento[]>{
    return this.http.get<Evento[]>(`${this.baseUrl}/${tema}/tema`);
  }

  public getEventoById(id: number): Observable<Evento>{
    return this.http.get<Evento>(`${this.baseUrl}/${id}`);
  }

  public updateEvento(body: Evento): Observable<Evento>{
    return this.http.put<Evento>(`${this.baseUrl}/${body.id}`, body)
  }

  public deleteEvento(id: number): Observable<Evento>{
    return this.http.delete<Evento>(`${this.baseUrl}/${id}`);
  }

  public createEvento(body: Evento): Observable<Evento>{
    return this.http.post<Evento>(`${this.baseUrl}`, body);
  }
}
