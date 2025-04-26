import { Component, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet } from '@angular/router';
import { MatSidenav } from '@angular/material/sidenav';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { MatMenuModule } from '@angular/material/menu';
import { MsalService } from '@azure/msal-angular';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  standalone: true,
  imports: [
    CommonModule,
    RouterOutlet,
    MatToolbarModule,
    MatButtonModule,
    MatSidenavModule,
    MatIconModule,
    MatListModule,
    MatMenuModule
  ]
})
export class AppComponent {
  @ViewChild('sidenav') sidenav!: MatSidenav;
  title = 'Pulse';
  isLoggedIn = false;

  constructor(private msalService: MsalService) {
    this.isLoggedIn = this.msalService.instance.getAllAccounts().length > 0;
  }

  toggleSidenav() {
    this.sidenav.toggle();
  }

  login() {
    this.msalService.loginRedirect();
  }

  logout() {
    this.msalService.logout();
  }
}
