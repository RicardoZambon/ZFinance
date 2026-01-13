import { ComponentFixture, TestBed } from '@angular/core/testing';
import { RolesFilterComponent } from './roles-filter';

describe(RolesFilterComponent.name, () => {
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RolesFilterComponent ]
    })
    .compileComponents();
  });

  it('should create', async () => {
    const fixture: ComponentFixture<RolesFilterComponent> = TestBed.createComponent(RolesFilterComponent);
    const component: RolesFilterComponent = fixture.componentInstance;

    fixture.detectChanges();
    await fixture.whenStable();

    expect(component).toBeTruthy();
  });
});