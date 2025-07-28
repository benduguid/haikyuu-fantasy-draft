import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet } from '@angular/router';

// Angular Material imports
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';

import { ApiService } from './core/services/api.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    CommonModule,
    RouterOutlet,
    MatToolbarModule,
    MatButtonModule,
    MatCardModule,
    MatIconModule
  ],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class AppComponent {
  title = 'haikyuu-fantasy-ui';
  apiTestResult: any = null;
  apiError: string | null = null;

  constructor(private apiService: ApiService) {}

  testBackendConnection(): void {
    this.apiTestResult = null;
    this.apiError = null;

    this.apiService.testConnection().subscribe({
      next: (result) => {
        console.log('API Test Success:', result);
        this.apiTestResult = result;
      },
      error: (error) => {
        console.error('API Test Error:', error);
        this.apiError = error.message || 'Failed to connect to backend';
      }
    });
  }
}
