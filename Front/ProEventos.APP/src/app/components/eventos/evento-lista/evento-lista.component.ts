import { Component, TemplateRef } from '@angular/core';
import { Evento } from 'src/app/models/Evento';
import { EventoService } from 'src/app/services/evento.service';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { PageModel } from 'src/app/models/PageModel';
import { PageChangedEvent } from 'ngx-bootstrap/pagination';

@Component({
  selector: 'app-evento-lista',
  templateUrl: './evento-lista.component.html',
  styleUrls: ['./evento-lista.component.scss']
})
export class EventoListaComponent {

  private _page: PageModel<Evento> = {pageSize: 3} as PageModel<Evento>;
  private modalRef?: BsModalRef;

  constructor(
    private eventoService: EventoService,
    private modalService: BsModalService,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService
    ){}

  public ngOnInit(): void {
    this.getEventos();
  }

  public get page(): PageModel<Evento>{
    return this._page;
  }

  public get eventos(): Evento[]{
    return this._page.items;
  }

  private deleteEvento(eventoId: number): void{
    this.spinner.show();
    this.eventoService.deleteEvento(eventoId).subscribe({
      next: (response: Evento) => {
        this.getEventos();
        this.toastr.success(`Código de evento: ${eventoId}, removido`);
      },
      error: (err) => {
        this.toastr.error(`Erro ao excluir evento id ${eventoId}`);
        console.error(err);
      },
      complete: () => {}
    }).add(() => this.spinner.hide());
  }

  public openModal(template: TemplateRef<any>) {
    this.modalRef = this.modalService.show(template, {class: 'modal-sm'});
  }

  public confirm(eventoId: number): void {
    this.deleteEvento(eventoId);
    this.modalRef?.hide();
  }

  public decline(): void {
    this.modalRef?.hide();
  }

  public pageChanged(event: PageChangedEvent): void {
    this._page.pageNumber = event.page;
    this.getEventos();
  }

  private getEventos(): void{
    this.spinner.show();
    this.eventoService.getEventos(this._page).subscribe({
      next : (response) => {
        this._page = response;
      },
      error: (err) => {
        this.toastr.error(`Erro ao carregar eventos`);
        console.error(err);
      },
      complete: () => console.info('complete')
    }).add(() => this.spinner.hide());
  }

}
