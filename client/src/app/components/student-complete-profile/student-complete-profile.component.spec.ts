import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StudentCompleteProfileComponent } from './student-complete-profile.component';

describe('StudentCompleteProfileComponent', () => {
  let component: StudentCompleteProfileComponent;
  let fixture: ComponentFixture<StudentCompleteProfileComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ StudentCompleteProfileComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(StudentCompleteProfileComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
