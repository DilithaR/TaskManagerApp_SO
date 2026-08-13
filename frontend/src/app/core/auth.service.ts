import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { tap } from 'rxjs/operators';

export interface UserDto {
  id: number;
  username: string;
}

@Injectable({ providedIn: 'root' })
export class AuthService {
    private http = inject(HttpClient);
    private api = 'http://localhost:5206/api/auth';
  
    isLoggedIn = false;
  
    login(username: string, password: string) {
      return this.http.post<UserDto>(
        `${this.api}/login`,
        { username, password },
        { withCredentials: true }
      ).pipe(
        tap(() => (this.isLoggedIn = true))
      );
    }
  }