import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatDividerModule } from '@angular/material/divider';

import { GraphService } from '../../services/graph.service';
import { User } from '@microsoft/microsoft-graph-types';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatProgressSpinnerModule,
    MatButtonModule,
    MatIconModule,
    MatListModule,
    MatSnackBarModule,
    MatDividerModule
  ],
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit {
  profile?: User;
  profilePicture?: string;
  loading = true;
  error?: string;

  constructor(
    private graphService: GraphService,
    private snackBar: MatSnackBar
  ) { }

  ngOnInit(): void {
    this.getProfile();
  }

  getProfile(): void {
    this.loading = true;
    this.error = undefined;

    this.graphService.getUserProfile().subscribe({
      next: (profile) => {
        this.profile = profile;
        this.getProfilePicture();
      },
      error: (error) => {
        console.error('Error getting profile:', error);
        this.error = 'Failed to load user profile. Please try again later.';
        this.snackBar.open('Error loading profile: ' + (error.message || 'Unknown error'), 'Close', {
          duration: 5000
        });
        this.loading = false;
      }
    });
  }

  getProfilePicture(): void {
    this.graphService.getUserPhoto().subscribe({
      next: (photo: Blob) => {
        const reader = new FileReader();
        reader.onload = (event: any) => {
          this.profilePicture = event.target.result;
          this.loading = false;
        };
        reader.readAsDataURL(photo);
      },
      error: () => {
        console.log('No profile picture available');
        this.loading = false;
      }
    });
  }
}
