import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';

export interface TaskDto {
  id: number;
  title: string;
  description?: string;
  isCompleted: boolean;
  createdAt: string;
  updatedAt: string;
}

export interface PagedResult<T> {
  items: T[];
  totalCount: number;
  page: number;
  pageSize: number;
}

@Injectable({ providedIn: 'root' })
export class TasksService {
  private http = inject(HttpClient);
  private api = 'http://localhost:5206/api/tasks';

  getPage(page = 1, pageSize = 10) {
    const params = new HttpParams().set('page', page).set('pageSize', pageSize);
    return this.http.get<PagedResult<TaskDto>>(this.api, { params, withCredentials: true });
  }

  create(title: string, description?: string) {
    return this.http.post<TaskDto>(this.api, { title, description }, { withCredentials: true });
  }

  toggle(id: number) {
    return this.http.patch<TaskDto>(`${this.api}/${id}/complete`, {}, { withCredentials: true });
  }

  delete(id: number) {
    return this.http.delete(`${this.api}/${id}`, { withCredentials: true });
  }
  update(id: number, title: string, description?: string) {
    return this.http.put<TaskDto>(
      `${this.api}/${id}`,
      { title, description },
      { withCredentials: true }
    );
  }
}