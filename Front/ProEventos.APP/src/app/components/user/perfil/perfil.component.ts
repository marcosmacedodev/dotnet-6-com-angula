import { NgxSpinnerService } from 'ngx-spinner';
import { AccountService } from 'src/app/services/account.service';

import { Component, OnInit } from '@angular/core';
import { User } from 'src/app/models/Users/User';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-perfil',
  templateUrl: './perfil.component.html',
  styleUrls: ['./perfil.component.scss']
})
export class PerfilComponent implements OnInit{

  user = {} as User;

  constructor(private account: AccountService,
    private toastr : ToastrService,
    private spinner: NgxSpinnerService){}

  ngOnInit(): void {

  }

  public get isPalestrante(): boolean{
    return this.user.userType?.toLowerCase() === 'palestrante';
  }

  public changeValue(user: User): void{
    this.user = user;
  }

  onFileSelected(event: any) {

    const file:File = event.target.files[0];

    if (file) {
      this.spinner.show();
        this.account.uploadImage(file).subscribe
        ({
          next: () => {
            this.toastr.success("Update de imagem realizado com sucesso.");
          },
          error: (err) => {
            this.toastr.error("Upload de imagem falhou");
            console.error(err);
          }
        }).add(() => this.spinner.hide());
    }
  }

}
