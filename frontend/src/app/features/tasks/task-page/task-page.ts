import { Component, OnInit, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { TaskDto, TasksService } from '../../../core/tasks.service';

@Component({
  selector: 'app-task-page',
  imports: [FormsModule],
  templateUrl: './task-page.html',
  styleUrl: './task-page.css',
})
export class TaskPage implements OnInit {
  private tasksApi = inject(TasksService);

  tasks = signal<TaskDto[]>([]);
  title = '';
  description = '';
  error = signal('');
  editingId = signal<number | null>(null);

  ngOnInit() {
    this.reload();
  }

  reload() {
    this.tasksApi.getPage().subscribe({
      next: (r) => {
        this.tasks.set(r.items ?? []);
        this.error.set('');
      },
      error: () => this.error.set('Could not load tasks. Login again?'),
    });
  }

  startEdit(task: TaskDto) {
    this.editingId.set(task.id);
    this.title = task.title;
    this.description = task.description ?? '';
  }

  save() {
    if (!this.title.trim()) return;

    const request =
      this.editingId() === null
        ? this.tasksApi.create(this.title, this.description)
        : this.tasksApi.update(this.editingId()!, this.title, this.description);

    request.subscribe({
      next: () => {
        this.editingId.set(null);
        this.title = '';
        this.description = '';
        this.reload();
      },
      error: () => this.error.set('Save failed'),
    });
  }

  toggle(task: TaskDto) {
    this.tasksApi.toggle(task.id).subscribe({ next: () => this.reload() });
  }

  remove(task: TaskDto) {
    this.tasksApi.delete(task.id).subscribe({ next: () => this.reload() });
  }
}