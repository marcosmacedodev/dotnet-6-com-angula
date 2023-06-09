import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-eventos',
  templateUrl: './eventos.component.html',
  styleUrls: ['./eventos.component.scss']
})
export class EventosComponent implements OnInit {

  public eventos: any = [];
  public eventosFiltrado: any = [];
  private _filtroLista: string = '';

  constructor(private http: HttpClient){}

  ngOnInit(): void {
    this.getEventos();
  }

  public get filtroLista(): string{
    return this._filtroLista;
  }

  public set filtroLista(value: string){
    this._filtroLista = value;
    this.eventosFiltrado = this.filtroLista ? this.filtrarEventos(this.filtroLista): this.eventos;
  }

  filtrarEventos(filtrarPor: string): any{
    filtrarPor = filtrarPor.toLocaleLowerCase();
    return this.eventos.filter(
      (evento: any) => evento.tema.toLocaleLowerCase().indexOf(filtrarPor) !== -1 ||
      evento.local.toLocaleLowerCase().indexOf(filtrarPor) !== -1
      )
  }

  public getEventos(): void{
    this.http.get("https://localhost:7283/api/eventos").subscribe({
      next : (response) => {this.eventos = response; this.eventosFiltrado = this.eventos},
      error: (err) => console.log(err),
      complete: () => console.info('complete')
  });
  }

}
