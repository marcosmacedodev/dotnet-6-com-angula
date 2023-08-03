import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { User } from 'src/app/models/Users/User';
import { AccountService } from 'src/app/services/account.service';
import { PalestranteService } from 'src/app/services/palestrante.service';

@Component({
  selector: 'app-perfil-detalhe',
  templateUrl: './perfil-detalhe.component.html',
  styleUrls: ['./perfil-detalhe.component.scss']
})
export class PerfilDetalheComponent implements OnInit {

  @Output() formChangeValue = new EventEmitter();

  constructor(
    private fb : FormBuilder,
    private account: AccountService,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService,
    private palService: PalestranteService){}

    private _form!: FormGroup;

    ngOnInit(): void
  {
    this.validate();
    this.loadUser();
    this.changeValue();
  }

  private changeValue(): void{
    this.form.valueChanges
    .subscribe(() => this.formChangeValue.emit({... this.form.value}));
  }

  private get isPalestrante(): boolean{
    const user = this._form.value as User;
    return user.userType?.toLowerCase() == "palestrante";
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

    if(this.isPalestrante)
    {
      this.palService.createPalestrante()
      .subscribe({
        next: (response) => {
          this.toastr.success("Função palestrante ativado com sucesso.");
        },
        error: (err) => {
          this.toastr.error("Não foi possível ativar função palestrante.");
          console.error(err);
        }
      });
    }

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
      userGrade: [''],
      firstName: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(16)]],
      lastName: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(16)]],
      email: ['', [Validators.required, Validators.email]],
      phoneNumber: [''],
      userType: [''],
      description: ['', [Validators.maxLength(128)]],
      password: [''],
      imageUrl: [''],
      confirm_password: ['']
    });
  }
}
