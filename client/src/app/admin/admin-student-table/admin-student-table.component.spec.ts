import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminStudentTableComponent } from './admin-student-table.component';

describe('AdminStudentTableComponent', () => {
  let component: AdminStudentTableComponent;
  let fixture: ComponentFixture<AdminStudentTableComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdminStudentTableComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdminStudentTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
