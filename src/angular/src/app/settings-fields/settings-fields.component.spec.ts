import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SettingsFieldsComponent } from './settings-fields.component';

describe('SettingsFieldsComponent', () => {
  let component: SettingsFieldsComponent;
  let fixture: ComponentFixture<SettingsFieldsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SettingsFieldsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SettingsFieldsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
