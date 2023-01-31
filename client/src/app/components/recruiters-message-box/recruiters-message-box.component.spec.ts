import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RecruitersMessageBoxComponent } from './recruiters-message-box.component';

describe('RecruitersMessageBoxComponent', () => {
  let component: RecruitersMessageBoxComponent;
  let fixture: ComponentFixture<RecruitersMessageBoxComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RecruitersMessageBoxComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RecruitersMessageBoxComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
