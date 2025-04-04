import { Component, OnInit } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Component({
  selector: 'app-license',
  templateUrl: './license.component.html',
  styleUrls: ['./license.component.css'],
})
export class LicenseComponent implements OnInit {
  company?: string;
  licenseAmount?: number;
  lastPayment?: Date;
  expiryDate?: Date;

  userRole?: string;
  userCompany?: string;

  displayUpdate = 'none';
  displayNoNumber = 'none';

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.getTokenClaims();
    this.fetchLicense();
  }

  fetchLicense() {
    const url = 'https://localhost:44363/license';
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      Authorization: `Bearer ${localStorage.getItem('accessToken')}`,
    });

    return this.http.get<any>(url, { headers: headers }).subscribe({
      next: (res) => {
        this.company = res.company;
        this.licenseAmount = res.actualAmount;
        this.lastPayment = res.lastPayment;
        this.expiryDate = res.expiryDate;
        console.log(res);
      },
      error: (error) => {
        console.error('There was an error!', error);
      },
    });
  }

  updateLicenceAmount(amount: number) {
    const url = 'https://localhost:44363/license';
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      Authorization: `Bearer ${localStorage.getItem('accessToken')}`,
    });

    this.http.put<any>(url, amount, { headers: headers }).subscribe({
      next: (res) => {
        this.licenseAmount = res.actualAmount;
      },
      error: (error) => {
        console.error('There was an error!', error);
      },
    });
  }

  submit(amount: any) {
    if (this.isANumber(amount)) {
      console.log('number ' + amount);

      this.updateLicenceAmount(amount);
      this.closeModelUpdate();
      return;
    }

    this.closeModelUpdate();
    this.openModalNoNumber();
    console.log('nonum');
  }

  getTokenClaims() {
    let token = localStorage.getItem('accessToken');
    if (token == null) {
      return;
    }

    let decodedJWT = JSON.parse(window.atob(token.split('.')[1]));
    this.userRole = decodedJWT.role;
    this.userCompany = decodedJWT.company;
  }

  openModalUpdate() {
    this.displayUpdate = 'block';
  }

  closeModelUpdate() {
    this.displayUpdate = 'none';
  }

  openModalNoNumber() {
    this.displayNoNumber = 'block';
  }

  closeModelNoNumber() {
    this.displayNoNumber = 'none';
  }

  isANumber(amount: any): boolean {
    return !isNaN(amount);
  }
}
