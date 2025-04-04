import { Component, OnInit } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { License } from '../models/license.model';
import { Router } from '@angular/router';

@Component({
  selector: 'app-administration',
  templateUrl: './administration.component.html',
  styleUrls: ['./administration.component.css'],
})
export class AdministrationComponent implements OnInit {
  public licenses: Array<License> = [];
  public today: Date = new Date();

  constructor(private http: HttpClient, private router: Router) {}

  ngOnInit() {
    this.today = new Date();
    console.log(this.today);
    this.fetchLicenses();
    //console.log(this.licenses[0].lastPayment.getDate());
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

  completePayment(company: string) {
    console.log(company);

    const url = 'https://localhost:44363/lastpayment';
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      Authorization: `Bearer ${localStorage.getItem('accessToken')}`,
    });

    this.http
      .put<any>(url, JSON.stringify(company), { headers: headers })
      .subscribe({
        next: () => {
          this.ngOnInit();
        },
        error: (error) => {
          console.error('There was an error!', error);
        },
      });
  }
}
