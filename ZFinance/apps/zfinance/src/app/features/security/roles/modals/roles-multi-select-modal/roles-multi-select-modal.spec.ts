import { ComponentFixture, TestBed } from '@angular/core/testing';
import { RolesMultiSelectModalComponent } from './roles-multi-select-modal';

describe(RolesMultiSelectModalComponent.name, () => {
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RolesMultiSelectModalComponent ]
    })
    .compileComponents();
  });

  it('should create', async () => {
    const fixture: ComponentFixture<RolesMultiSelectModalComponent> = TestBed.createComponent(RolesMultiSelectModalComponent);
    const component: RolesMultiSelectModalComponent = fixture.componentInstance;

    fixture.detectChanges();
    await fixture.whenStable();

    expect(component).toBeTruthy();
  });
});