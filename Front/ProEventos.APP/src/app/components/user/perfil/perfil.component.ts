import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';

@Component({
  selector: 'app-perfil',
  templateUrl: './perfil.component.html',
  styleUrls: ['./perfil.component.scss']
})
export class PerfilComponent implements OnInit{

  private _form! : FormGroup;

  constructor(private fb : FormBuilder){}

  ngOnInit(): void{
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

  private validate(): void{
    this._form = this.fb.group({
      titulo: ['', Validators.required],
      nome: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(16)]],
      sobrenome: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(16)]],
      email: ['', [Validators.required, Validators.email]],
      telefone: ['', [Validators.required]],
      funcao: ['', [Validators.required]],
      descricao: ['', [Validators.maxLength(128)]],
      senha: ['', [Validators.required, Validators.minLength(8), Validators.maxLength(16)]],
      confirma_senha: ['', [Validators.required]]
    })
  }

}
