import { Component } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
})
export class LoginComponent {
  constructor(private http: HttpClient, private router: Router) {}

  displayWrongUserNameOrpassword = 'none';
  displayLicenseAmountExceeded = 'none';
  displayCertificateUnvalid = 'none';
  displayExpiryDate = 'none';

  submit(emailadress: string, password: string) {
    const url = 'https://localhost:44356/user';
    const headers = new HttpHeaders();
    headers.set('Content-Type', 'application/json; charset=utf-8');
    const body = {
      email: emailadress,
      password: password,
    };

    var that = this;
    this.http.post<any>(url, body).subscribe({
      next: (res) => {
        console.log(res);

        if (res.role === 'service') {
          that.router.navigate(['service']);
        } else {
          that.router.navigate(['order']);
        }
      },
      error: (error) => {
        console.error('There was an error!', error);
        if (error.status == 401) {
          that.openWrongUserNameOrpassword();
        }
        if (error.status == 403) {
          that.openModelLicenseAmountExceeded();
        }
        if (error.status == 404) {
          that.openWrongUserNameOrpassword();
        }
        if (error.status == 409) {
          that.openModelExpiryDate();
        }
        if (error.status == 500) {
          that.openCertificateUnvalid();
        }
        return;
      },
    });
  }

  closeModelWrongUserNameOrpassword() {
    this.displayWrongUserNameOrpassword = 'none';
  }

  openWrongUserNameOrpassword() {
    this.displayWrongUserNameOrpassword = 'block';
  }

  closeModelLicenseAmountExceeded() {
    this.displayLicenseAmountExceeded = 'none';
  }

  openModelLicenseAmountExceeded() {
    this.displayLicenseAmountExceeded = 'block';
  }

  closeModelCertificateUnvalid() {
    this.displayCertificateUnvalid = 'none';
  }

  openCertificateUnvalid() {
    this.displayCertificateUnvalid = 'block';
  }

  closeModelExpiryDate() {
    this.displayExpiryDate = 'none';
  }

  openModelExpiryDate() {
    this.displayExpiryDate = 'block';
  }
}
