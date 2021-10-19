import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoggInn } from './logg-inn/logg-inn'

const appRoots: Routes = [
  { path: 'Logg-Inn', component: LoggInn }
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
