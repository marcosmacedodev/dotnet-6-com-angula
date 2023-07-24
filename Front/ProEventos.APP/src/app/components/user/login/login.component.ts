
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from 'src/app/services/account.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  private _form!: FormGroup;

  ngOnInit(): void {
    this.validate();
  }

  public get form(): FormGroup{
    return this._form;
  }

  public set form(form: FormGroup){
    this._form = form;
  }

  constructor(
    private fb: FormBuilder,
    private account: AccountService,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService
    ){}


  private validate(): void{
    this._form = this.fb.group({
      userName: ['', Validators.required],
      password: ['', Validators.required]
    });
  }

  public onLogin(): void{
    const messages = [];

    if (!this.form.controls['userName'].valid)
    {
      messages.push("Digite seu usuário");
    }

    if (!this.form.controls['password'].valid)
    {
      messages.push("Digite sua senha");
    }

    if (messages.length)
    {
      this.toastr.info(messages[0]);
      return;
    }
    this.spinner.show();
    this.account.login(this.form.value).subscribe({
      next: (response: any) => {
        console.log(response);
      },
      error: (err) => {
        if(err.status == 401)
        {
          this.toastr.error("usuário ou senha inválido.")
        }
        else
        {
          this.toastr.error(err.message, err.statusText);
        }
        console.error(err);
      }
    }).add(()=>this.spinner.hide());
  }
}
