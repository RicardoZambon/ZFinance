import { ComponentFixture, TestBed } from '@angular/core/testing';
import { RolesListComponent } from './roles-list';

describe(RolesListComponent.name, () => {
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RolesListComponent ]
    })
    .compileComponents();
  });

  it('should create', async () => {
    const fixture: ComponentFixture<RolesListComponent> = TestBed.createComponent(RolesListComponent);
    const component: RolesListComponent = fixture.componentInstance;

    fixture.detectChanges();
    await fixture.whenStable();

    expect(component).toBeTruthy();
  });
});