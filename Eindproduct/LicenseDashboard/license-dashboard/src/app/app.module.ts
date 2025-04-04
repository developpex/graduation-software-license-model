import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';

import { AppComponent } from './app.component';
import { HeaderComponent } from './header/header.component';
import { LicenseComponent } from './license/license.component';
import { LoginComponent } from './login/login.component';
import { FooterComponent } from './footer/footer.component';
import { ServiceComponent } from './service/service.component';
import { AdministrationComponent } from './administration/administration.component';

@NgModule({
  declarations: [
    AppComponent,
    HeaderComponent,
    LicenseComponent,
    LoginComponent,
    FooterComponent,
    ServiceComponent,
    AdministrationComponent,
  ],
  imports: [
    BrowserModule,
    RouterModule.forRoot([
      { path: 'license', component: LicenseComponent },
      { path: 'login', component: LoginComponent },
      { path: 'service', component: ServiceComponent },
      { path: 'administration', component: AdministrationComponent },
      { path: '', component: LoginComponent },
    ]),
    HttpClientModule,
    FormsModule,
  ],
  providers: [],
  bootstrap: [AppComponent],
})
export class AppModule {}
