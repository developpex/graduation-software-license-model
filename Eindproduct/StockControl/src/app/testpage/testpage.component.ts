import { Component } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Component({
  selector: 'app-testpage',
  templateUrl: './testpage.component.html',
  styleUrls: ['./testpage.component.css'],
})
export class TestpageComponent {
  constructor(private http: HttpClient) {}

  displayWrongUserNameOrpassword = 'none';

  getCertificate() {}

  getCertificateFromLicenseService() {
    const url = 'https://localhost:44356/licenseservice';
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      //Authorization: `Bearer ${localStorage.getItem('accessToken')}`,
    });

    return this.http.get<any>(url, { headers: headers }).subscribe({
      next: (res) => {
        console.log(res);
      },
      error: (error) => {
        console.error('There was an error!', error);
      },
    });
  }

  closeModelWrongUserNameOrpassword() {
    this.displayWrongUserNameOrpassword = 'none';
  }

  openWrongUserNameOrpassword() {
    this.displayWrongUserNameOrpassword = 'block';
  }
}
