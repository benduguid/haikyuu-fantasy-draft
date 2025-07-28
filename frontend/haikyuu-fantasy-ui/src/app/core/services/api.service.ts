import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

export interface Player {
  id: number;
  name: string;
  school: string;
  position: Position;
  power: number;
  jumping: number;
  stamina: number;
  gameSense: number;
  technique: number;
  speed: number;
  pointCost: number;
  imageUrl?: string;
  description?: string;
  createdAt: Date;
  updatedAt: Date;
}

export enum Position {
  MiddleBlocker = 1,
  WingSpiker = 2,
  Setter = 3,
  Libero = 4,
  Opposite = 5
}

export interface RegisterRequest {
  email: string;
  password: string;
  confirmPassword: string;
  displayName?: string;
}

export interface LoginRequest {
  email: string;
  password: string;
}

export interface AuthResponse {
  token: string;
  email: string;
  displayName?: string;
  expiresAt: Date;
}

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  // Test endpoint
  testConnection(): Observable<any> {
    return this.http.get(`${this.apiUrl}/players/test`);
  }

  // Auth endpoints
  register(request: RegisterRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.apiUrl}/auth/register`, request);
  }

  login(request: LoginRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.apiUrl}/auth/login`, request);
  }

  // Player endpoints
  getPlayers(): Observable<Player[]> {
    return this.http.get<Player[]>(`${this.apiUrl}/players`);
  }

  getPlayer(id: number): Observable<Player> {
    return this.http.get<Player>(`${this.apiUrl}/players/${id}`);
  }

  createPlayer(player: Omit<Player, 'id' | 'createdAt' | 'updatedAt'>): Observable<Player> {
    return this.http.post<Player>(`${this.apiUrl}/players`, player);
  }
}
