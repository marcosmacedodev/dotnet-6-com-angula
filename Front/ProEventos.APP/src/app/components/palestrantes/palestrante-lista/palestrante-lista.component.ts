import { PageChangedEvent } from 'ngx-bootstrap/pagination';
import { PalestranteService } from './../../../services/palestrante.service';
import { Component, OnInit } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { PageModel } from 'src/app/models/PageModel';
import { Palestrante } from 'src/app/models/Palestrante';

@Component({
  selector: 'app-palestrante-lista',
  templateUrl: './palestrante-lista.component.html',
  styleUrls: ['./palestrante-lista.component.scss']
})
export class PalestranteListaComponent implements OnInit {

  private _page: PageModel<Palestrante> = {pageSize: 3} as PageModel<Palestrante>;

  public ngOnInit(): void {
    this.loadPalestrantes();
  }

  constructor(
    private service: PalestranteService,
    private spinner: NgxSpinnerService,
    private toastr: ToastrService){}

  public get palestrantes(): Palestrante[]
  {
    return this._page.items;
  }

  public get page() : PageModel<Palestrante>{
    return this._page;
  }

  private loadPalestrantes(): void{
    this.spinner.show();
    this.service.getPalestrantes(this._page).subscribe({
      next: (response) => {
        this._page = response;
      },
      error: (err) => {
        this.toastr.error(`Erro ao carregar palestrantes`);
        console.error(err);
      }
    }).add(()=>this.spinner.hide())
  }

  public pageChanged(event: PageChangedEvent): void {
    this._page.pageNumber = event.page;
    this.loadPalestrantes();
  }

}
