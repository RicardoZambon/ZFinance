import { ComponentFixture, TestBed } from '@angular/core/testing';
import { RolesActionsChildListComponent } from './roles-actions-child-list';

describe(RolesActionsChildListComponent.name, () => {
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RolesActionsChildListComponent ]
    })
    .compileComponents();
  });

  it('should create', async () => {
    const fixture: ComponentFixture<RolesActionsChildListComponent> = TestBed.createComponent(RolesActionsChildListComponent);
    const component: RolesActionsChildListComponent = fixture.componentInstance;

    fixture.detectChanges();
    await fixture.whenStable();

    expect(component).toBeTruthy();
  });
});