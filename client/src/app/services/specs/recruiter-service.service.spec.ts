import { TestBed } from '@angular/core/testing';

import { RecruiterServiceService } from '../recruiter/recruiter-service.service';

describe('RecruiterServiceService', () => {
  let service: RecruiterServiceService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(RecruiterServiceService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
