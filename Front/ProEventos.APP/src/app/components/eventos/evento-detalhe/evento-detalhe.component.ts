import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators  } from '@angular/forms';

@Component({
  selector: 'app-evento-detalhe',
  templateUrl: './evento-detalhe.component.html',
  styleUrls: ['./evento-detalhe.component.scss']
})
export class EventoDetalheComponent implements OnInit {

  private _form!: FormGroup;

  constructor(private fb: FormBuilder){}

  ngOnInit(): void {
    this.validate();
  }

  public get Form(){
    return this._form;
  }

  public set Form(value: FormGroup){
    this._form = value;
  }

  public getField(fieldname: string): any{
    return this._form.get(fieldname);
  }

  validate(): void{
    this._form = this.fb.group({
      tema: ['', [Validators.required, Validators.minLength(4),Validators.maxLength(50)]],
      local: ['', Validators.required],
      dataEvento: ['', Validators.required],
      qtdPessoas: ['', [Validators.required, Validators.min(1), Validators.max(120000)]],
      telefone: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      imagemURL: ['', Validators.required]
    });
  }

}
