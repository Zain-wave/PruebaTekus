import { Component, computed, inject } from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import { MatIconModule } from '@angular/material/icon';
import { MatToolbarModule } from '@angular/material/toolbar';
import { NavigationEnd, Router, RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { filter, map } from 'rxjs';
import { AuthService } from '../../core/services/auth.service';

interface NavItem {
  label: string;
  icon: string;
  path: string;
}

@Component({
  selector: 'app-shell',
  imports: [RouterOutlet, RouterLink, RouterLinkActive, MatToolbarModule, MatIconModule],
  templateUrl: './shell.html',
  styleUrl: './shell.scss',
})
export class Shell {
  private readonly router = inject(Router);
  private readonly authService = inject(AuthService);

  protected readonly navItems: NavItem[] = [
    { label: 'Overview', icon: 'dashboard', path: '/dashboard' },
    { label: 'Providers', icon: 'business', path: '/providers' },
    { label: 'Services', icon: 'inventory_2', path: '/services' },
  ];

  private readonly currentUrl = toSignal(
    this.router.events.pipe(
      filter((event) => event instanceof NavigationEnd),
      map((event) => event.urlAfterRedirects),
    ),
    { initialValue: this.router.url },
  );

  protected readonly screenTitle = computed(() => {
    const url = this.currentUrl();

    if (url.includes('/providers')) {
      return 'Providers';
    }
    if (url.includes('/services')) {
      return 'Services';
    }
    return 'Overview';
  });

  protected signOut(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}
