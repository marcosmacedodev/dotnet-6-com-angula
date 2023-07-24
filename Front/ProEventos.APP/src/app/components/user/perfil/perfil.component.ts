import { ToastrService } from 'ngx-toastr';
import { AccountService } from 'src/app/services/account.service';
import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { User } from 'src/app/models/Users/User';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-perfil',
  templateUrl: './perfil.component.html',
  styleUrls: ['./perfil.component.scss']
})
export class PerfilComponent implements OnInit{

  private _form!: FormGroup;

  constructor(
    private fb : FormBuilder,
    private account: AccountService,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService){}

  ngOnInit(): void
  {
    this.validate();
    this.loadUser();
  }

  private loadUser(): void
  {
    this.spinner.show();
    this.account.getUser().subscribe({
      next: (response: User) => {
        this._form.patchValue(response);
        this.toastr.success("Usuário carregado");
      },
      error: (err) =>{
        this.toastr.error(err.message, err.error);
      },
      complete: () => {}
    }).add(() => this.spinner.hide());
  }

  public get form(): FormGroup
  {
    return this._form;
  }

  public set form(value: FormGroup)
  {
    this._form = value;
  }

  public updateUser(): void
  {
    this.spinner.show();
    this.account.updateUser(this.form.value).subscribe({
      next: () => {
        this.toastr.success("Usuário atualizado");
      },
      error: (err) => {
        this.toastr.error(err.message, err.error)
      },
      complete: () => {}
    }).add(() => this.spinner.hide());
  }

  private validate(): void
  {
    this._form = this.fb.group({
      userName: [''],
      userGrade: ['', Validators.required],
      firstName: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(16)]],
      lastName: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(16)]],
      email: ['', [Validators.required, Validators.email]],
      phoneNumber: ['', [Validators.required]],
      userType: ['', [Validators.required]],
      description: ['', [Validators.maxLength(128)]],
      password: [''],
      confirm_password: ['']
    });
  }
}
