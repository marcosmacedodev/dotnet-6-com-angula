import { Component, OnInit, Type } from '@angular/core';
import { AccountService } from '../services/account.service';
import { Router, ActivatedRoute } from '@angular/router';
import { LoginComponent } from '../components/user/login/login.component';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.scss']
})
export class NavComponent implements OnInit {

  constructor(
    public account: AccountService,
    public router: Router,
    public aRouter: ActivatedRoute
    ) { }

  ngOnInit() {

  }

  isCollapsed = true;

  public logout(): void{
    this.account.logout();
  }



}
