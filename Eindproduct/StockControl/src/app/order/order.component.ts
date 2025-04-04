import { Component, NgZone } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Component({
  selector: 'app-order',
  templateUrl: './order.component.html',
  styleUrls: ['./order.component.css'],
})
export class OrderComponent {
  public orders: Array<string> = [];
  public interval: number = 1000;
  public expiryDate: Date = new Date();
  intervalId: any;

  constructor(private ngZone: NgZone, private http: HttpClient) {}

  async ngOnInit() {
    await this.fetchCertificate();

    this.intervalId = setInterval(() => {
      this.ngZone.run(() => {
        this.orders.push(
          'Order processed successfully ' + new Date().getSeconds().toString()
        );
      });
    }, 1000);
  }

  async fetchCertificate() {
    const url = 'https://localhost:44356/certificate';
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
    });

    let that = this;
    return this.http.get<any>(url, { headers: headers }).subscribe({
      next: (res) => {
        this.expiryDate = res.expiryDate;
        that.checkExpiryDate();
      },
      error: (error) => {
        console.error('There was an error!', error);
      },
    });
  }

  checkExpiryDate() {
    let today = new Date();
    if (new Date(this.expiryDate) <= today) {
      this.changeInterval(5000);
    }
  }

  changeInterval(newInterval: number) {
    clearInterval(this.intervalId);
    this.intervalId = setInterval(() => {
      this.ngZone.run(() => {
        this.orders.push(
          'Order processed successfully ' + new Date().getSeconds().toString()
        );
      });
    }, newInterval);
  }
}
