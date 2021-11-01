import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SettingsPathsComponent } from './settings-paths.component';

describe('SettingsPathsComponent', () => {
  let component: SettingsPathsComponent;
  let fixture: ComponentFixture<SettingsPathsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SettingsPathsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SettingsPathsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
