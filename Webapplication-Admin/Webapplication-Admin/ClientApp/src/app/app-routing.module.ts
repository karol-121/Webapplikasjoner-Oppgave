import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoggInn } from './logg-inn/logg-inn';
import { RoutesManager } from './routes-manager/routes-manager';

const appRoots: Routes = [
  { path: 'Logg-Inn', component: LoggInn },
  { path: 'Manage-Routes', component: RoutesManager }
]

@NgModule({
  imports: [
    RouterModule.forRoot(appRoots)
  ],
  exports: [
    RouterModule
  ]
})

export class AppRoutingModule {}
