import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VerifyStudentAutomaticComponent } from './verify-student-automatic.component';

describe('VerifyStudentAutomaticComponent', () => {
  let component: VerifyStudentAutomaticComponent;
  let fixture: ComponentFixture<VerifyStudentAutomaticComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ VerifyStudentAutomaticComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(VerifyStudentAutomaticComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
