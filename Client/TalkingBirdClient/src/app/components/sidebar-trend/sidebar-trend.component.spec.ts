import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SidebarTrendComponent } from './sidebar-trend.component';

describe('SidebarTrendComponent', () => {
  let component: SidebarTrendComponent;
  let fixture: ComponentFixture<SidebarTrendComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SidebarTrendComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SidebarTrendComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
