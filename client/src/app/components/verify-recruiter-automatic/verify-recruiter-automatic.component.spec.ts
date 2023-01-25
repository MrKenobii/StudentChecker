import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VerifyRecruiterAutomaticComponent } from './verify-recruiter-automatic.component';

describe('VerifyRecruiterAutomaticComponent', () => {
  let component: VerifyRecruiterAutomaticComponent;
  let fixture: ComponentFixture<VerifyRecruiterAutomaticComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ VerifyRecruiterAutomaticComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(VerifyRecruiterAutomaticComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
