import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StudentsMessageBoxComponent } from './students-message-box.component';

describe('StudentsMessageBoxComponent', () => {
  let component: StudentsMessageBoxComponent;
  let fixture: ComponentFixture<StudentsMessageBoxComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ StudentsMessageBoxComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(StudentsMessageBoxComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
