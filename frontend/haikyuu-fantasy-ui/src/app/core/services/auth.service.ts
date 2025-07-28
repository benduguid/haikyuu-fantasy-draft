import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { ApiService, AuthResponse, LoginRequest, RegisterRequest } from './api.service';

export interface User {
  email: string;
  displayName?: string;
  token: string;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private currentUserSubject = new BehaviorSubject<User | null>(null);
  public currentUser$ = this.currentUserSubject.asObservable();

  constructor(private apiService: ApiService) {
    // Check if user is already logged in on app start
    this.loadUserFromStorage();
  }

  private loadUserFromStorage(): void {
    const storedUser = localStorage.getItem('currentUser');
    if (storedUser) {
      const user = JSON.parse(storedUser);
      // TODO: Check if token is still valid
      this.currentUserSubject.next(user);
    }
  }

  register(request: RegisterRequest): Observable<AuthResponse> {
    return this.apiService.register(request).pipe(
      tap(response => this.handleAuthSuccess(response))
    );
  }

  login(request: LoginRequest): Observable<AuthResponse> {
    return this.apiService.login(request).pipe(
      tap(response => this.handleAuthSuccess(response))
    );
  }

  logout(): void {
    localStorage.removeItem('currentUser');
    this.currentUserSubject.next(null);
  }

  private handleAuthSuccess(response: AuthResponse): void {
    const user: User = {
      email: response.email,
      displayName: response.displayName,
      token: response.token
    };

    localStorage.setItem('currentUser', JSON.stringify(user));
    this.currentUserSubject.next(user);
  }

  get currentUserValue(): User | null {
    return this.currentUserSubject.value;
  }

  get isLoggedIn(): boolean {
    return this.currentUserValue !== null;
  }

  get token(): string | null {
    return this.currentUserValue?.token || null;
  }
}
