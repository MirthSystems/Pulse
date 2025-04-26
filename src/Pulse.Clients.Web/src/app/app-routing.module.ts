import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './pages/home/home.component';
import { ProfileComponent } from './pages/profile/profile.component';
import { ErrorComponent } from './pages/error/error.component';
import { MsalGuard } from '@azure/msal-angular';

const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'home', component: HomeComponent },
  { path: 'profile', component: ProfileComponent, canActivate: [MsalGuard] },
  { path: 'error', component: ErrorComponent },
  { path: 'error/:statusCode', component: ErrorComponent },
  { path: 'error/404', component: ErrorComponent, data: { message: 'Page not found' } },
  { path: '**', redirectTo: '/error/404' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
