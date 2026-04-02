import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BikeDetailsComponent } from './bike-details.component';

const routes: Routes = [
  { path: ':ref', component: BikeDetailsComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class BikeDetailsRoutingModule { }

