import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StudentPrivacySettingsComponent } from './student-privacy-settings.component';

describe('StudentPrivacySettingsComponent', () => {
  let component: StudentPrivacySettingsComponent;
  let fixture: ComponentFixture<StudentPrivacySettingsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ StudentPrivacySettingsComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(StudentPrivacySettingsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
