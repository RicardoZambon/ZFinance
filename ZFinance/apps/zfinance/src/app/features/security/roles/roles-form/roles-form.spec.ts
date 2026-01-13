import { ComponentFixture, TestBed } from '@angular/core/testing';
import { RolesFormComponent } from './roles-form';

describe(RolesFormComponent.name, () => {
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RolesFormComponent ]
    })
    .compileComponents();
  });

  it('should create', async () => {
    const fixture: ComponentFixture<RolesFormComponent> = TestBed.createComponent(RolesFormComponent);
    const component: RolesFormComponent = fixture.componentInstance;

    fixture.detectChanges();
    await fixture.whenStable();

    expect(component).toBeTruthy();
  });
});