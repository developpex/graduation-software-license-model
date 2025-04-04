import { Component, OnInit } from '@angular/core';
import { FormsModule, NgModel } from '@angular/forms';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { License } from '../models/license.model';

@Component({
  selector: 'app-service',
  templateUrl: './service.component.html',
  styleUrls: ['./service.component.css'],
})
export class ServiceComponent implements OnInit {
  public licenses: Array<License> = [];

  public selectedCompany: string = '';
  public actualAmount?: any;

  displayGenerate = 'none';
  displayNoNumber = 'none';

  constructor(private http: HttpClient) {}

  ngOnInit() {
    console.log('...');

    this.fetchLicenses();
  }

  submit(company: string, actualAmount?: any) {
    this.generateCertificate(company, actualAmount);
    this.closeModelGenerate();
  }

  fetchLicenses() {
    const url = 'https://localhost:44363/licenses';
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      Authorization: `Bearer ${localStorage.getItem('accessToken')}`,
    });

    return this.http.get<any>(url, { headers: headers }).subscribe({
      next: (res) => {
        this.licenses = res;
      },
      error: (error) => {
        console.error('There was an error!', error);
      },
    });
  }

  generateCertificate(company: string, actualAmount?: any) {
    const url = 'https://localhost:44363/generatecertificate';
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      Authorization: `Bearer ${localStorage.getItem('accessToken')}`,
    });

    this.http
      .put<any>(url, { company, actualAmount }, { headers: headers })
      .subscribe({
        next: (res) => {
          console.log('res:' + res);
        },
        error: (error) => {
          console.log(error);
        },
      });
  }

  openModalGenerate(actualAmount?: any) {
    if (!this.isANumber(actualAmount)) {
      this.openModalNoNumber();
      return;
    }

    this.displayGenerate = 'block';
  }

  closeModelGenerate() {
    this.displayGenerate = 'none';
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
