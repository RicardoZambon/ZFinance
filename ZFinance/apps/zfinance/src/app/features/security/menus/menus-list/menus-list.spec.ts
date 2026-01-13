import { ComponentFixture, TestBed } from '@angular/core/testing';
import { MenusListComponent } from './menus-list';

describe(MenusListComponent.name, () => {
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MenusListComponent ]
    })
    .compileComponents();
  });

  it('should create', async () => {
    const fixture: ComponentFixture<MenusListComponent> = TestBed.createComponent(MenusListComponent);
    const component: MenusListComponent = fixture.componentInstance;

    fixture.detectChanges();
    await fixture.whenStable();

    expect(component).toBeTruthy();
  });
});