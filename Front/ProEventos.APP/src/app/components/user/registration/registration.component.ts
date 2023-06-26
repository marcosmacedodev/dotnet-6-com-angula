import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.scss']
})
export class RegistrationComponent implements OnInit{

  private _form!: FormGroup;

  constructor(private fb: FormBuilder){}

  ngOnInit(): void {
    this.validate();
  }

  public set form(value: FormGroup){
    this._form = value;
  }

  public get form(): FormGroup{
    return this._form;
  }

  public getField(fieldname: string): any{
    return this._form.get(fieldname);
  }

  private validate(): void{
    this._form = this.fb.group({
      nome: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(16)]],
      sobrenome: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(16)]],
      email: ['', [Validators.required, Validators.email]],
      usuario: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(8)]],
      senha: ['', [Validators.required, Validators.minLength(8), Validators.maxLength(16)]],
      confirma_senha: ['', [Validators.required]],
      concorda: [false, Validators.required]
    });
  }

}
