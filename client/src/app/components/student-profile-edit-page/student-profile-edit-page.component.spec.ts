import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StudentProfileEditPageComponent } from './student-profile-edit-page.component';

describe('StudentProfileEditPageComponent', () => {
  let component: StudentProfileEditPageComponent;
  let fixture: ComponentFixture<StudentProfileEditPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ StudentProfileEditPageComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(StudentProfileEditPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
