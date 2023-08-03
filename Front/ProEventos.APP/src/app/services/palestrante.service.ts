import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, take } from 'rxjs';
import { Palestrante } from '../models/Palestrante';
import { PageModel } from '../models/PageModel';

@Injectable({
  providedIn: 'root'
})
export class PalestranteService {

baseUrl = "https://localhost:7283/api/palestrantes";

constructor(private http: HttpClient) { }

public getPalestrantes(params: any): Observable<PageModel<Palestrante>>{
  return this.http
  .get<PageModel<Palestrante>>(`${this.baseUrl}/all`,{params: params})
  .pipe(take(1));
}

public getPalestrante(): Observable<Palestrante>{
  return this.http
  .get<Palestrante>(`${this.baseUrl}`)
  .pipe(take(1));
}

public createPalestrante(): Observable<Palestrante>{
  return this.http
  .post<Palestrante>(`${this.baseUrl}`, {} as Palestrante)
  .pipe(take(1));
}

public savePalestrante(body: Palestrante): Observable<Palestrante>{
  return this.http
  .put<Palestrante>(`${this.baseUrl}`, body)
  .pipe(take(1));
}

}
