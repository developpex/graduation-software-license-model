import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-footer',
  templateUrl: './footer.component.html',
  styleUrls: ['./footer.component.css'],
})
export class FooterComponent implements OnInit {
  name: string | undefined;
  lastname: string | undefined;
  role: string | undefined;

  ngOnInit(): void {
    this.getTokenClaims();
  }

  getTokenClaims() {
    let token = localStorage.getItem('accessToken');
    if (token == null) {
      return;
    }

    let decodedJWT = JSON.parse(window.atob(token.split('.')[1]));
    this.name = decodedJWT.name;
    this.lastname = decodedJWT.lastname;
    this.role = decodedJWT.role;
  }
}
