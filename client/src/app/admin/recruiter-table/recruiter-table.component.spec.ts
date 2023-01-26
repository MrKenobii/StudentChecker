import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RecruiterTableComponent } from './recruiter-table.component';

describe('RecruiterTableComponent', () => {
  let component: RecruiterTableComponent;
  let fixture: ComponentFixture<RecruiterTableComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RecruiterTableComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RecruiterTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
