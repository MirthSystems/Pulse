import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MsalService } from '@azure/msal-angular';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule
  ],
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent {
  get isLoggedIn(): boolean {
    return !!this.authService.instance.getActiveAccount() ||
           this.authService.instance.getAllAccounts().length > 0;
  }

  constructor(private authService: MsalService) { }

  login(): void {
    this.authService.loginRedirect();
  }
}
