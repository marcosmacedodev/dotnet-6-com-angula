import { RedeSocial } from './../../models/RedeSocial';
import { Component, Input, OnInit, OnChanges, TemplateRef } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { RedeSocialService } from 'src/app/services/redeSocial.service';

@Component({
  selector: 'app-redessociais',
  templateUrl: './redessociais.component.html',
  styleUrls: ['./redessociais.component.scss']
})
export class RedessociaisComponent implements OnInit, OnChanges {

  private _form! : FormGroup;
  private modalRef?: BsModalRef;
  @Input() eventoId?: number;

  constructor(
    private fb: FormBuilder,
    private modalService: BsModalService,
    private toastr: ToastrService,
    private rsService: RedeSocialService){}

  ngOnInit(): void {
    this.validate();
    this.loadRedesSociais();
  }
  ngOnChanges() {

  }

  private loadRedesSociais(): void{
    const that = this;
    this.rsService.getRedesSociais(this.eventoId!).subscribe({
      next: (response) => {
         if(response && response.length){
            response.forEach(function(value){
              that.adicionar(value);
            })
         }
      },
      error: (err) => {
        this.toastr.error("Não foi possível carregar as redes sociais");
        console.error(err);
      }
    })
  }

  public get form(): FormGroup{
    return this._form;
  }

  public get redesSociais(): FormArray{
    return this._form.get('redesSociais') as FormArray
  }

  public adicionarRedeSocial(): void{
    this.adicionar({id: 0} as RedeSocial);
  }

  private validate(): void{
    this._form = this.fb.group({
      redesSociais: this.fb.array([])
    })
  }

  private adicionar(redeSocial: RedeSocial): void{
    this.redesSociais.push(this.fb.group({
      id: redeSocial.id,
      nome: [redeSocial.nome, Validators.required],
      url: [redeSocial.url, Validators.required],
      eventoId: redeSocial.eventoId,
      palestranteId: redeSocial.palestranteId
    }));
  }

  public openModal(template: TemplateRef<any>)
  {
    this.modalRef = this.modalService.show(template, {class: 'modal-sm'});
  }

  public confirm(index: number, redeSocial: RedeSocial): void
  {
    this.modalRef?.hide();
    if(redeSocial.id)
    {
      this.rsService.deleteRedeSocial(redeSocial.id, this.eventoId!).subscribe({
        next: (response) => {
          this.removeItem(index);
        },
        error: (err) => {
          this.toastr.error(`Não foi possível excluir rede social com id: ${redeSocial.id}.`);
          console.error(err);
        }
      })
    }
  }

  public decline(): void
  {
    this.modalRef?.hide();
    this.toastr.show("Ação cancelada pelo usuário.")
  }

  public removeItem(index: number): void
  {
    this.redesSociais.removeAt(index)
  }

  public saveRedesSociais(): void{
    this.rsService.saveRedesSociais(this.redesSociais?.value as RedeSocial[], this.eventoId!).subscribe({
      next: (response) => {
        this.redesSociais.patchValue(response);
        this.toastr.success("Redes sociais alteradas com sucesso.")
      },
      error: (err) => {
        this.toastr.error("Erro ao salvar redes sociais");
        console.error(err);
      }
    })
  }

}
