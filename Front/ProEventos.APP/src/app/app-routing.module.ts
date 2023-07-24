import { NgModule, Component, inject } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { EventosComponent } from './components/eventos/eventos.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { PalestrantesComponent } from './components/palestrantes/palestrantes.component';
import { ContatosComponent } from './components/contatos/contatos.component';
import { PerfilComponent } from './components/user/perfil/perfil.component';
import { EventoListaComponent } from './components/eventos/evento-lista/evento-lista.component';
import { EventoDetalheComponent } from './components/eventos/evento-detalhe/evento-detalhe.component';
import { UserComponent } from './components/user/user.component';
import { LoginComponent } from './components/user/login/login.component';
import { RegistrationComponent } from './components/user/registration/registration.component';
import { AuthGuard } from './guard/auth.guard';
import { HomeComponent } from './components/home/home.component';

const routes: Routes = [

  {path: 'usuario', component: UserComponent,
    children: [
      {path: 'login', component: LoginComponent},
      {path: 'registrar', component: RegistrationComponent}
  ]},
  {
    path: 'home', component: HomeComponent
  },
  {path: '',
  runGuardsAndResolvers: 'always',
  canActivate: [() => inject(AuthGuard).canActivate()],
  children:[
    {path: 'eventos', redirectTo: 'eventos/lista'},
    {path: 'eventos', component: EventosComponent,
    children: [
      {path: 'lista', component: EventoListaComponent},
      {path: 'detalhe/:id', component: EventoDetalheComponent},
      {path: 'detalhe', component: EventoDetalheComponent}
    ]},
    {path: 'dashboard', component: DashboardComponent},
    {path: 'palestrantes', component: PalestrantesComponent},
    {path: 'contatos', component: ContatosComponent},
    //{path: '', redirectTo: 'dashboard', pathMatch: 'full'},
    {path: 'usuario/perfil', component: PerfilComponent}
  ]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
