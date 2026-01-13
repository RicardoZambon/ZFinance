import { ComponentFixture, TestBed } from '@angular/core/testing';
import { RolesMenusChildListComponent } from './roles-menus-child-list';

describe(RolesMenusChildListComponent.name, () => {
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RolesMenusChildListComponent ]
    })
    .compileComponents();
  });

  it('should create', async () => {
    const fixture: ComponentFixture<RolesMenusChildListComponent> = TestBed.createComponent(RolesMenusChildListComponent);
    const component: RolesMenusChildListComponent = fixture.componentInstance;

    fixture.detectChanges();
    await fixture.whenStable();

    expect(component).toBeTruthy();
  });
});