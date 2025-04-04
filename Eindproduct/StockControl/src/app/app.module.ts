import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { CommonModule, DatePipe } from '@angular/common';

import { AppComponent } from './app.component';
import { TestpageComponent } from './testpage/testpage.component';
import { FooterComponent } from './footer/footer.component';
import { HeaderComponent } from './header/header.component';
import { LoginComponent } from './login/login.component';
import { ServiceComponent } from './service/service.component';
import { OrderComponent } from './order/order.component';

@NgModule({
  declarations: [
    AppComponent,
    TestpageComponent,
    LoginComponent,
    FooterComponent,
    HeaderComponent,
    ServiceComponent,
    OrderComponent,
  ],
  imports: [
    BrowserModule,
    CommonModule,
    RouterModule.forRoot([
      { path: 'test', component: TestpageComponent },
      { path: 'login', component: LoginComponent },
      { path: 'service', component: ServiceComponent },
      { path: 'order', component: OrderComponent },
      { path: '', component: LoginComponent },
    ]),
    HttpClientModule,
    FormsModule,
  ],
  providers: [DatePipe],
  bootstrap: [AppComponent],
})
export class AppModule {}
