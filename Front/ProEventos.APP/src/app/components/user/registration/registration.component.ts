import { AccountService } from 'src/app/services/account.service';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { User } from 'src/app/models/Users/User';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.scss']
})
export class RegistrationComponent implements OnInit{

  private _form!: FormGroup;

  constructor(
    private account: AccountService,
    private fb: FormBuilder,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService
    ){}

  ngOnInit(): void {
    this.validate();
  }

  public set form(value: FormGroup){
    this._form = value;
  }

  public get form(): FormGroup{
    return this._form;
  }

  private validate(): void{
    this._form = this.fb.group({
      firstName: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(16)]],
      lastName: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(16)]],
      email: ['', [Validators.required, Validators.email]],
      userName: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(64)]],
      password: ['', [Validators.required, Validators.minLength(8), Validators.maxLength(32)]],
      confirm_password: ['', [Validators.required]],
      agree: [false, Validators.requiredTrue]
    });
  }

  public createUser(): void{
    this.spinner.show();
    this.account.createUser(this.form.value).subscribe({
      next: () => {
        this.toastr.success(`Usuário criado com sucesso`, '');
        this.form.reset();
      },
      error: (err) => {
        this.toastr.error(err.message, err.statusText)
        console.error(err)
      },
      complete: () => {

      }
    }).add({unsubscribe: () =>this.spinner.hide()});
  }

}
