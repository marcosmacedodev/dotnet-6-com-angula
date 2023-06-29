import { EventoService } from 'src/app/services/evento.service';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators  } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { BsLocaleService } from 'ngx-bootstrap/datepicker';
import { Evento } from 'src/app/models/Evento';
import { ToastrService } from 'ngx-toastr';
//import { listLocales } from 'ngx-bootstrap/chronos';


@Component({
  selector: 'app-evento-detalhe',
  templateUrl: './evento-detalhe.component.html',
  styleUrls: ['./evento-detalhe.component.scss']
})
export class EventoDetalheComponent implements OnInit {

  private _form!: FormGroup;
  private locale = 'pt-br';
  private _evento = {} as Evento;
  //locales = listLocales();

  constructor(private fb: FormBuilder,
    private localeService: BsLocaleService,
    private router: ActivatedRoute,
    private eventoService: EventoService,
    private toastr: ToastrService){}

  ngOnInit(): void {
    this.validate();
    this.localeService.use(this.locale);
    this.loadEvento();
  }

  onSubmit(): void{
    if (this._evento.id)
    {
      this.updateEvento();
    }
    else
    {
      this.createEvento();
    }
  }

  public get evento(): Evento{
    return this._evento;
  }

  private loadEvento(): void{
    const eventoIdParam = this.router.snapshot.paramMap.get('id');
    if(eventoIdParam != null)
    {
      this.eventoService.getEventoById(+eventoIdParam).subscribe({
        next: (response: Evento) => {
          this._evento = {...response};
          this._form.patchValue(this._evento);
        },
        error: (err) => {},
        complete: () => {}
      });
    }
  }

  private createEvento() : void{
    this.eventoService.createEvento({...this._form.value}).subscribe({
      next: (evento: Evento) => {
        this._evento = evento;
        this.toastr.success(`Código de evento: ${evento.id}, foi salvo com successo.`, 'Criar');
      },
      error: (err) => {console.error(err)},
      complete: () => {}
    });
  }

  private updateEvento() : void{
    this._evento = {id: this.evento.id, ... this._form.value};
    this.eventoService.updateEvento(this._evento).subscribe({
      next: (evento: Evento) => {
        this.toastr.success(`Código de evento: ${this._evento.id}, foi salvo com sucesso.`, 'Atualizar');
      },
      error: (err) => {console.error(err)},
      complete: () => {}
    });
  }

  public get bsConfig(): any{
    return {adaptivePosition: true,
      showClearButton: true,
      clearButtonLabel: 'Limpar',
      showTodayButton: true,
      todayButtonLabel: 'Hoje',
      dateInputFormat: 'DD/MM/YYYY hh:mm',
      useUtc: true,
      withTimepicker: true}
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

  private validate(): void{
    this._form = this.fb.group({
      tema: ['', [Validators.required, Validators.minLength(4),Validators.maxLength(50)]],
      local: ['', Validators.required],
      dataEvento: ['', Validators.required],
      qtdPessoas: ['', [Validators.required, Validators.min(1), Validators.max(120000)]],
      telefone: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      imagemUrl: ['', Validators.required]
    });
  }

}
