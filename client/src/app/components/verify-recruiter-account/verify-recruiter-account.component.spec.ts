import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VerifyRecruiterAccountComponent } from './verify-recruiter-account.component';

describe('VerifyRecruiterAccountComponent', () => {
  let component: VerifyRecruiterAccountComponent;
  let fixture: ComponentFixture<VerifyRecruiterAccountComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ VerifyRecruiterAccountComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(VerifyRecruiterAccountComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
