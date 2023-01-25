import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RecruiterPrivacySettingsComponent } from './recruiter-privacy-settings.component';

describe('RecruiterPrivacySettingsComponent', () => {
  let component: RecruiterPrivacySettingsComponent;
  let fixture: ComponentFixture<RecruiterPrivacySettingsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RecruiterPrivacySettingsComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RecruiterPrivacySettingsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
