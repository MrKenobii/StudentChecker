import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RecruiterCompleteProfileComponent } from './recruiter-complete-profile.component';

describe('RecruiterCompleteProfileComponent', () => {
  let component: RecruiterCompleteProfileComponent;
  let fixture: ComponentFixture<RecruiterCompleteProfileComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RecruiterCompleteProfileComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RecruiterCompleteProfileComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
