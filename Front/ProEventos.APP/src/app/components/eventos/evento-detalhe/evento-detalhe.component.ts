import { EventoService } from 'src/app/services/evento.service';
import { LoteService } from 'src/app/services/lote.service';
import { Component, OnInit, TemplateRef } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators  } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { BsLocaleService } from 'ngx-bootstrap/datepicker';
import { Evento } from 'src/app/models/Evento';
import { ToastrService } from 'ngx-toastr';
import { Lote } from 'src/app/models/Lote';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
//import { listLocales } from 'ngx-bootstrap/chronos';


@Component({
  selector: 'app-evento-detalhe',
  templateUrl: './evento-detalhe.component.html',
  styleUrls: ['./evento-detalhe.component.scss']
})
export class EventoDetalheComponent implements OnInit {

  private _form!: FormGroup;
  private locale = 'pt-br';
  private modalRef?: BsModalRef;
  //Cópia
  //private _evento = {} as Evento;
  //locales = listLocales();
  private _visible = false;

  constructor(private fb: FormBuilder,
    private localeService: BsLocaleService,
    private activatedRouter: ActivatedRoute,
    private router: Router,
    private eventoService: EventoService,
    private loteService: LoteService,
    private modalService: BsModalService,
    private toastr: ToastrService){}

  ngOnInit(): void {
    this.validate();
    this.localeService.use(this.locale);
    this.loadEvento();
  }

  openModal(template: TemplateRef<any>) {
    this.modalRef = this.modalService.show(template, {class: 'modal-sm'});
  }

  confirm(index: number, lote: Lote): void{
    this.modalRef?.hide();
    if(lote.eventoId)
    {
      this.deleteLote(lote.eventoId, lote.id, index);
    }
  }

  removeItem(index: number): void{
    this.lotes.removeAt(index);
  }

  decline(): void{
    this.modalRef?.hide();
    this.toastr.show("Ação cancelada pelo usuário.")
  }

  onSaveEvento(): void{
    if (this.form.value?.id)
    {
      this.updateEvento(this.form.value);
    }
    else
    {
      this.createEvento(this.form.value);
    }
  }

  onSaveLotes(): void{
    this.saveLotes(this.form.value?.id, this.lotes?.value);
  }

  public get visibleLote(): boolean{
    return this._visible;
  }

  public set visibleLote(value : boolean){
    this._visible = value;
  }

  private loadEvento(): void{
    const eventoIdParam = this.activatedRouter.snapshot.paramMap.get('id');
    if(eventoIdParam != null)
    {
      this.eventoService.getEventoById(+eventoIdParam).subscribe({
        next: (evento: Evento) => {
          //this._evento = {...evento};
          this._form.patchValue(evento);
          this._visible = true;
          this.loadLotes(evento.id)
        },
        error: (err) => {},
        complete: () => {}
      });
    }
  }

  private createEvento(entity: Evento) : void{
    //this._evento = {...this._form.value};
    this.eventoService.createEvento(entity).subscribe({
      next: (evento: Evento) => {
        //this._evento = evento;
        this.toastr.success(`Código de evento: ${evento.id}, foi salvo com successo.`, 'Criar');
        this.router.navigate([`eventos/detalhe/${evento.id}`]);
      },
      error: (err) => {console.error(err)},
      complete: () => {}
    });
  }

  private saveLotes(eventoId: number, entities: Lote[]): void{
    //this.lotes.value as Lote[]
    this.loteService.saveLotes(eventoId, entities).subscribe({
      next: (lotes: Lote[]) => {
        this.lotes.reset(lotes);
        lotes.forEach((value: Lote) =>{
          this.toastr.success(`Código de Lote: ${value.id}, foi salvo com sucesso.`, 'Salvar');
        });
      },
      error: (err) => {console.error(err)},
      complete: () => {}
    });
  }

  private deleteLote(eventoId: number, loteId: number, indexItem: number): void{
    this.loteService.deleteLote(eventoId, loteId).subscribe({
      next: (lote: Lote) => {
        this.router.navigate([`eventos/detalhe/${eventoId}`]);
        this.toastr.success(`Código de Lote: ${loteId}, foi removido com sucesso.`, 'Excluir');
        this.removeItem(indexItem);
      },
      error: (err) => {console.error(err)},
      complete: () => {}
    });
  }

  private updateEvento(entity: Evento) : void{
    //this._evento = {id: this.evento.id, ... this._form.value};
    this.eventoService.updateEvento(entity).subscribe({
      next: (evento: Evento) => {
        this.toastr.success(`Código de evento: ${evento.id}, foi salvo com sucesso.`, 'Atualizar');
      },
      error: (err) => {console.error(err)},
      complete: () => {}
    });
  }

  public get bsConfig(): any{
    return {adaptivePosition: true,
      showClearButton: false,
      clearButtonLabel: 'Limpar',
      showTodayButton: false,
      todayButtonLabel: 'Hoje',
      dateInputFormat: 'DD/MM/YYYY hh:mm',
      useUtc: false,
      withTimepicker: true,
      isAnimated: true,
      showWeekNumbers: false}
  }

  public get form(){
    return this._form;
  }

  public set form(value: FormGroup){
    this._form = value;
  }

  public getField(fieldname: string): any{
    return this._form.get(fieldname);
  }

  public get lotes()
  {
    return (this._form.get('lotes') as FormArray);
  }

  private validate(): void{
    this._form = this.fb.group({
      id: [0],
      tema: ['', [Validators.required, Validators.minLength(4),Validators.maxLength(50)]],
      local: ['', Validators.required],
      dataEvento: ['', Validators.required],
      qtdPessoas: ['', [Validators.required, Validators.min(1), Validators.max(120000)]],
      telefone: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      imagemUrl: ['', Validators.required],
      lotes: this.fb.array([])
    });
  }

  public adicionarLote(): void{
    this.loadLote({id: 0} as Lote)
  }

  private loadLote(lote: Lote): void{
    this.lotes.push(this.fb.group({
      id: lote.id,
      nome: [lote.nome, Validators.required],
      quantidade: [lote.quantidade, Validators.required],
      preco: [lote.preco, Validators.required],
      dataInicio: [lote.dataInicio],
      dataFim: [lote.dataFim],
      eventoId: [lote.eventoId]
    })
    );
  }

  private loadLotes(eventoId: number): void {
    this.loteService.getLotesByEventoId(eventoId).subscribe({
      next: (lotes: Lote[]) => {
        lotes.forEach((value: Lote) =>{
          this.loadLote(value);
        });
      },
      error: (err) => {console.error(err)},
      complete: () => {console.info('complete')}

    });
  }

}
