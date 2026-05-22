import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TrackManagement } from './track-management';

describe('TrackManagement', () => {
  let component: TrackManagement;
  let fixture: ComponentFixture<TrackManagement>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TrackManagement],
    }).compileComponents();

    fixture = TestBed.createComponent(TrackManagement);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
