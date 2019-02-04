import { BrowserModule } from "@angular/platform-browser";
import { NgModule } from "@angular/core";
import { FormsModule, ReactiveFormsModule } from "@angular/forms"; // < -- NgModel lives here
import { HttpClientModule } from "@angular/common/http";
import { HttpClientInMemoryWebApiModule } from "angular-in-memory-web-api";
import { InMemoryDataService } from "./in-memory-data.service";

import { AppComponent } from "./app.component";
import { HeroesComponent } from "./heroes/heroes.component";
import { HeroDetailComponent } from "./hero-detail/hero-detail.component";
import { MessagesComponent } from "./messages/messages.component";
import { AppRoutingModule } from "./app-routing.module";
import { DashboardComponent } from "./dashboard/dashboard.component";
import { HeroSearchComponent } from "./hero-search/hero-search.component";
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import { LayoutModule } from "@angular/cdk/layout";
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
  MatProgressBarModule
} from "@angular/material";
import { NavComponent } from "./nav/nav.component";
import { PartsComponent } from "./parts/parts.component";
import { PartDetailComponent } from "./part-detail/part-detail.component";
import { SettingsComponent } from "./settings/settings.component";
import { KeysPipe } from "./keys.pipe";
import { SettingsFieldsComponent } from "./settings-fields/settings-fields.component";
import { DiscardChangesDialogComponent } from "./discard-changes-dialog/discard-changes-dialog.component";
import { SettingsPathsComponent } from "./settings-paths/settings-paths.component";

@NgModule({
  declarations: [
    AppComponent,
    HeroesComponent,
    HeroDetailComponent,
    MessagesComponent,
    DashboardComponent,
    HeroSearchComponent,
    NavComponent,
    PartsComponent,
    PartDetailComponent,
    SettingsComponent,
    KeysPipe,
    SettingsFieldsComponent,
    DiscardChangesDialogComponent,
    SettingsPathsComponent
  ],
  imports: [
    BrowserModule,
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
    ReactiveFormsModule
  ],
  entryComponents: [DiscardChangesDialogComponent],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule {}
