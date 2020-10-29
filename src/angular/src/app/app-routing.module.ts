import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PartsComponent } from './parts/parts.component';
import { PartDetailComponent } from './part-detail/part-detail.component';
import { SettingsComponent } from './settings/settings.component';
import { SettingsFieldsComponent } from './settings-fields/settings-fields.component';
import { SettingsPathsComponent } from './settings-paths/settings-paths.component';
import { AboutComponent } from './about/about.component';

const routes: Routes = [
  { path: '', redirectTo: '/parts', pathMatch: 'full' },
  { path: 'parts', component: PartsComponent },
  { path: 'part/:id', component: PartDetailComponent },
  { path: 'settings', component: SettingsComponent },
  { path: 'settings/fields', component: SettingsFieldsComponent },
  { path: 'settings/paths', component: SettingsPathsComponent },
  { path: 'about', component: AboutComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {}
