import { ComponentFixture, TestBed } from '@angular/core/testing';
import { MenusFilterComponent } from './menus-filter';

describe(MenusFilterComponent.name, () => {
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MenusFilterComponent ]
    })
    .compileComponents();
  });

  it('should create', async () => {
    const fixture: ComponentFixture<MenusFilterComponent> = TestBed.createComponent(MenusFilterComponent);
    const component: MenusFilterComponent = fixture.componentInstance;

    fixture.detectChanges();
    await fixture.whenStable();

    expect(component).toBeTruthy();
  });
});