import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { NgxElectronModule } from 'ngx-electron';
import { FormsModule, ReactiveFormsModule } from '@angular/forms'; // < -- NgModel lives here
import { HttpClientModule } from '@angular/common/http';
import { HttpClientInMemoryWebApiModule } from 'angular-in-memory-web-api';
import { InMemoryDataService } from './in-memory-data.service';

import { AppComponent } from './app.component';
import { MessagesComponent } from './messages/messages.component';
import { AppRoutingModule } from './app-routing.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { LayoutModule } from '@angular/cdk/layout';
import {
  MatToolbarModule,
  MatButtonModule,
  MatSidenavModule,
  MatIconModule,
  MatListModule,
  MatTableModule,
  MatPaginatorModule,
  MatSortModule,
  MatInputModule,
  MatSelectModule,
  MatRadioModule,
  MatCardModule,
  MatExpansionModule,
  MatDialogModule,
  MatProgressSpinnerModule,
  MatProgressBarModule,
  MatAutocompleteModule,
  MatMenuModule,
  MatSnackBarModule,
  MatTooltipModule
} from '@angular/material';
import { PartsComponent } from './parts/parts.component';
import { PartDetailComponent } from './part-detail/part-detail.component';
import { SettingsComponent } from './settings/settings.component';
import { KeysPipe } from './keys.pipe';
import { SettingsFieldsComponent } from './settings-fields/settings-fields.component';
import { DiscardChangesDialogComponent } from './discard-changes-dialog/discard-changes-dialog.component';
import { SettingsPathsComponent } from './settings-paths/settings-paths.component';
import { DeleteDialogComponent } from './delete-dialog/delete-dialog.component';
import { AboutComponent } from './about/about.component';

@NgModule({
  declarations: [
    AppComponent,
    MessagesComponent,
    PartsComponent,
    PartDetailComponent,
    SettingsComponent,
    KeysPipe,
    SettingsFieldsComponent,
    DiscardChangesDialogComponent,
    SettingsPathsComponent,
    DeleteDialogComponent,
    AboutComponent
  ],
  imports: [
    BrowserModule,
    NgxElectronModule,
    FormsModule,
    AppRoutingModule,
    HttpClientModule,
    // The HttpClientInMemoryWebApiModule module intercepts HTTP requests
    // and returns simulated server responses.
    // Remove it when a real server is ready to receive requests.
    HttpClientInMemoryWebApiModule.forRoot(InMemoryDataService, {
      dataEncapsulation: false
    }),
    BrowserAnimationsModule,
    LayoutModule,
    MatToolbarModule,
    MatButtonModule,
    MatSidenavModule,
    MatIconModule,
    MatListModule,
    MatTableModule,
    MatPaginatorModule,
    MatSortModule,
    MatInputModule,
    MatSelectModule,
    MatRadioModule,
    MatCardModule,
    MatExpansionModule,
    MatDialogModule,
    MatProgressSpinnerModule,
    MatProgressBarModule,
    MatAutocompleteModule,
    MatMenuModule,
    MatSnackBarModule,
    MatTooltipModule,
    ReactiveFormsModule
  ],
  entryComponents: [DiscardChangesDialogComponent, DeleteDialogComponent],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule {}
