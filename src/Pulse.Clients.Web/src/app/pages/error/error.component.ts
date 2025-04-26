import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDividerModule } from '@angular/material/divider';

@Component({
  selector: 'app-error',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatDividerModule,
    RouterModule
  ],
  templateUrl: './error.component.html',
  styleUrls: ['./error.component.scss']
})
export class ErrorComponent implements OnInit {
  errorMessage: string = 'An error occurred while processing your request.';
  statusCode: string = '500';
  requestId: string = '';
  showRequestId: boolean = false;
  isDevelopment: boolean = false;

  constructor(private route: ActivatedRoute) { }

  ngOnInit(): void {
    // Get error data from route if available
    this.route.paramMap.subscribe(params => {
      if (params.has('statusCode')) {
        this.statusCode = params.get('statusCode') || '500';
      }
    });

    this.route.queryParamMap.subscribe(params => {
      if (params.has('message')) {
        this.errorMessage = params.get('message') || this.errorMessage;
      }
      if (params.has('requestId')) {
        this.requestId = params.get('requestId') || '';
        this.showRequestId = this.requestId !== '';
      }
    });

    // Check if environment is development
    this.isDevelopment = window.location.hostname === 'localhost' ||
                         window.location.hostname === '127.0.0.1';
  }
}
