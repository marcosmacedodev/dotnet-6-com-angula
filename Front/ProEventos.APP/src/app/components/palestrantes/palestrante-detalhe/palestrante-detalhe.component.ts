import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { debounceTime, map } from 'rxjs';
import { PalestranteService } from 'src/app/services/palestrante.service';

@Component({
  selector: 'app-palestrante-detalhe',
  templateUrl: './palestrante-detalhe.component.html',
  styleUrls: ['./palestrante-detalhe.component.scss']
})
export class PalestranteDetalheComponent implements OnInit {

  private _form!: FormGroup;
  public corDaDescricao = '';
  public situacaoDoForm = '';

  constructor(
    private fb: FormBuilder,
    private palService: PalestranteService,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService
  ){}

  public get form(): FormGroup{
    return this._form;
  }

  ngOnInit(): void {
    this.validate();
    this.checkFormChanges();
    this.load();
  }

  private validate(): void{
    this._form = this.fb.group({
      miniCurriculo: ['']
    });
  }

  private load(): void{
    this.spinner.show();
    this.palService.getPalestrante()
    .subscribe({next: (response) => {
      this._form.patchValue(response);
    },
    error: (err) => {
      this.toastr.error("Erro ao carregar palestrante");
      console.log(err);
    }}).add(() => this.spinner.hide());
  }

  private checkFormChanges(){
    this._form.valueChanges.pipe(
      map(() => {
      this.situacaoDoForm = 'Minicurrículo está sendo atualizado...';
      this.corDaDescricao = 'text-warning';
    }),
    debounceTime(1000)).subscribe({next: () => {
      this.palService.savePalestrante({... this._form.value}).subscribe({
        next: (response) => {
          this.situacaoDoForm = 'Minicurrículo atualizado';
          this.corDaDescricao = 'text-success';
        },
        error: (err) => {
          this.situacaoDoForm = 'Não foi possível atualizar minicurrículo.';
          this.corDaDescricao = 'text-danger';
          console.error(err);
        }
      });
    }});
  }
}
