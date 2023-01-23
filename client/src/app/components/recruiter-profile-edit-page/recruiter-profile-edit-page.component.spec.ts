import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RecruiterProfileEditPageComponent } from './recruiter-profile-edit-page.component';

describe('RecruiterProfileEditPageComponent', () => {
  let component: RecruiterProfileEditPageComponent;
  let fixture: ComponentFixture<RecruiterProfileEditPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RecruiterProfileEditPageComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RecruiterProfileEditPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
