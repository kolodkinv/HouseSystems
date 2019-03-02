import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HouseDetailComponent } from "./house/house-detail/house-detail.component";
import { HouseListComponent } from "./house/house-list/house-list.component";
import { WaterMeterComponent } from './water-meter/water-meter.component';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HouseDetailComponent,
    HouseListComponent,
    WaterMeterComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { path: '', component: HouseListComponent, pathMatch: 'full' },
    ])
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
