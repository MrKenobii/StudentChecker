import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VerifyStudentAccountComponent } from './verify-student-account.component';

describe('VerifyStudentAccountComponent', () => {
  let component: VerifyStudentAccountComponent;
  let fixture: ComponentFixture<VerifyStudentAccountComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ VerifyStudentAccountComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(VerifyStudentAccountComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
