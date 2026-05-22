import { Injectable, inject } from '@angular/core';
import { MessageService } from 'primeng/api';

@Injectable({
  providedIn: 'root'
})
export class ToastService {
  private messageService = inject(MessageService);

  show(message: string, type: 'success' | 'error' | 'info' | 'warn' = 'info', summary?: string) {
    this.messageService.add({
      severity: type,
      summary: summary ?? type.charAt(0).toUpperCase() + type.slice(1),
      detail: message,
      life: 3500
    });
  }

  success(message: string) {
    this.show(message, 'success', 'Success');
  }

  error(message: string) {
    this.show(message, 'error', 'Error');
  }

  info(message: string) {
    this.show(message, 'info', 'Info');
  }

  loading(message: string) {
    this.messageService.add({
      severity: 'info',
      summary: 'Loading',
      detail: message,
      sticky: true,
      closable: false,
      key: 'loading-toast'
    });
  }

  clearLoading() {
    this.messageService.clear('loading-toast');
  }
}
