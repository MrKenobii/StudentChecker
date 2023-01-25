import { TestBed } from '@angular/core/testing';

import { RecruiterService } from '../recruiter/recruiter-service.service';

describe('RecruiterServiceService', () => {
  let service: RecruiterService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(RecruiterService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
