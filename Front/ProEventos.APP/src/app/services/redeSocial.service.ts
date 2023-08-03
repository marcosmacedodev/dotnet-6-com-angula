import { RedeSocial } from './../models/RedeSocial';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Evento } from '../models/Evento';
import { Palestrante } from '../models/Palestrante';
import { Observable, take } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class RedeSocialService {

baseUrl = "https://localhost:7283/api/redessociais";

constructor(private http: HttpClient) { }

public getRedesSociais(eventoId : number|undefined): Observable<RedeSocial[]>{
  if(eventoId)
  {
    return this.http.get<RedeSocial[]>(`${this.baseUrl}/evento/${eventoId}`).pipe(take(1));
  }

  return this.http.get<RedeSocial[]>(`${this.baseUrl}/palestrante`).pipe(take(1));
}

public saveRedesSociais(redesSociais: RedeSocial[], eventoId : number|undefined): Observable<RedeSocial[]>{

  if(eventoId)
  {
    return this.http.put<RedeSocial[]>(`${this.baseUrl}/evento/${eventoId}`, redesSociais).pipe(take(1));
  }

  return this.http.put<RedeSocial[]>(`${this.baseUrl}/palestrante`, redesSociais).pipe(take(1));
}

public deleteRedeSocial(redeSocialId: number, eventoId : number|undefined): Observable<void>{
  if(eventoId)
  {
    return this.http.delete<void>(`${this.baseUrl}/${redeSocialId}/evento/${eventoId}`).pipe(take(1));
  }

  return this.http.delete<void>(`${this.baseUrl}/${redeSocialId}/palestrante`).pipe(take(1));
}


}
