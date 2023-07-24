import { Component, TemplateRef } from '@angular/core';
import { Evento } from 'src/app/models/Evento';
import { EventoService } from 'src/app/services/evento.service';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-evento-lista',
  templateUrl: './evento-lista.component.html',
  styleUrls: ['./evento-lista.component.scss']
})
export class EventoListaComponent {

  public eventos: Evento[] = [];
  public eventosFiltrado: Evento[] = [];
  private _filtroLista: string = '';
  private _eventoId: number = 0;
  private modalRef?: BsModalRef;

  constructor(
    private eventoService: EventoService,
    private modalService: BsModalService,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService
    ){}

  ngOnInit(): void {
    this.getEventos();
  }

  public deleteEvento(): void{
    this.spinner.show();
    this.eventoService.deleteEvento(this._eventoId).subscribe({
      next: (response: Evento) => {
        this.getEventos();
        this.toastr.success(`Código de evento: ${this._eventoId}, removido`, 'Excluir');
      },
      error: (err) => {
        this.toastr.error(err.message, err.statusText);
        console.error(err);
      },
      complete: () => {}
    }).add(() => this.spinner.hide());
  }

  public get eventoId(): number{
    return this._eventoId;
  }

  openModal(template: TemplateRef<any>, eventoId: number) {
    this._eventoId = eventoId;
    this.modalRef = this.modalService.show(template, {class: 'modal-sm'});
  }

  confirm(): void {
    this.deleteEvento();
    this.modalRef?.hide();
  }

  decline(): void {
    this.modalRef?.hide();
  }

  public get filtroLista(): string{
    return this._filtroLista;
  }

  public set filtroLista(value: string){
    this._filtroLista = value;
    this.eventosFiltrado = this.filtroLista ? this.filtrarEventos(this.filtroLista): this.eventos;
  }

  private filtrarEventos(filtrarPor: string): Evento[]{
    filtrarPor = filtrarPor.toLocaleLowerCase();
    return this.eventos.filter(
      (evento: any) => evento.tema.toLocaleLowerCase().indexOf(filtrarPor) !== -1 ||
      evento.local.toLocaleLowerCase().indexOf(filtrarPor) !== -1
      )
  }

  private getEventos(): void{
    this.spinner.show();
    this.eventoService.getEventos().subscribe({
      next : (response) => {this.eventos = response; this.eventosFiltrado = this.eventos},
      error: (err) => {
        this.toastr.error(err.message, err.statusText);
        console.error(err);
      },
      complete: () => console.info('complete')
    }).add(() => this.spinner.hide());
  }

}
