import { Component } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Router } from '@angular/router';
import * as shajs from 'sha.js';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
})
export class LoginComponent {
  constructor(private http: HttpClient, private router: Router) {}

  userRole?: string;
  displayWrongUserNameOrpassword = 'none';
  displayNoAccesToken = 'none';

  submit(emailadress: string, password: string) {
    const url = 'https://localhost:44363/user';
    const headers = new HttpHeaders();
    headers.set('Content-Type', 'application/json; charset=utf-8');
    const body = {
      email: emailadress,
      password: shajs('sha256').update(password).digest('hex'),
    };

    var that = this;
    this.http.post<any>(url, body).subscribe({
      next: (res) => {
        localStorage.setItem('accessToken', res);
        that.parseTokenToGetUserRole();
        that.routeToPage();
      },
      error: (error) => {
        console.error('There was an error!', error);
        if (error.status == 404) {
          that.openWrongUserNameOrpassword();
        }
        if (error.status == 406) {
          that.openWrongUserNameOrpassword();
        }
        return;
      },
    });
  }

  routeToPage() {
    if (this.userRole == 'service') {
      this.router.navigate(['service']);
    } else if (this.userRole == 'administration') {
      this.router.navigate(['administration']);
    } else {
      this.router.navigate(['license']);
    }
  }

  parseTokenToGetUserRole() {
    let token = localStorage.getItem('accessToken');
    if (token == null) {
      this.openNoAccesToken();
      this.router.navigate(['login']);
      return;
    }

    let decodedJWT = JSON.parse(window.atob(token.split('.')[1]));
    this.userRole = decodedJWT.role;
  }

  closeModelWrongUserNameOrpassword() {
    this.displayWrongUserNameOrpassword = 'none';
  }

  openWrongUserNameOrpassword() {
    this.displayWrongUserNameOrpassword = 'block';
  }

  closeNoAccesToken() {
    this.displayNoAccesToken = 'none';
  }

  openNoAccesToken() {
    this.displayNoAccesToken = 'block';
  }
}
