import { Evento } from './../models/Evento';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Lote } from '../models/Lote';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LoteService {

private baseUrl = "https://localhost:7283/api/lotes";

constructor(private http: HttpClient ) {}

public getLotesByEventoId(eventoId: number): Observable<Lote[]>
{
  return this.http.get<Lote[]>(`${this.baseUrl}/${eventoId}`);
}

public saveLotes(eventoId: number, bodies: Lote[]): Observable<Lote[]>
{
  return this.http.put<Lote[]>(`${this.baseUrl}/${eventoId}`, bodies);
}

public deleteLote(eventoId: number, loteId: number): Observable<any>
{
  return this.http.delete(`${this.baseUrl}/${eventoId}/${loteId}`);
}


}
