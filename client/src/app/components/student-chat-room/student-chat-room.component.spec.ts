import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StudentChatRoomComponent } from './student-chat-room.component';

describe('StudentChatRoomComponent', () => {
  let component: StudentChatRoomComponent;
  let fixture: ComponentFixture<StudentChatRoomComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ StudentChatRoomComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(StudentChatRoomComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
