import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoggInn } from './logg-inn/logg-inn';
import { Dashboard } from './dashboard/dashboard';
import { RoutesManager } from './routes-manager/routes-manager';
import { CruisesdetailsManager } from './cruisesdetails-manager/cruisesdetails-manager';
import { CruisesManager } from './cruises-manager/cruises-manager';


const appRoots: Routes = [
  { path: 'Logg-Inn', component: LoggInn },
  { path: 'Dashboard', component: Dashboard },
  { path: 'Manage-Routes', component: RoutesManager },
  { path: 'Manage-Cruisedetails', component: CruisesdetailsManager },
  { path: 'Manage-Cruises', component: CruisesManager },
  { path: '', redirectTo: '/Logg-Inn', pathMatch: 'full' }
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
