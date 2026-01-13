import { ComponentFixture, TestBed } from '@angular/core/testing';
import { MenusMultiSelectModalComponent } from './menus-multi-select-modal';

describe(MenusMultiSelectModalComponent.name, () => {
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MenusMultiSelectModalComponent ]
    })
    .compileComponents();
  });

  it('should create', async () => {
    const fixture: ComponentFixture<MenusMultiSelectModalComponent> = TestBed.createComponent(MenusMultiSelectModalComponent);
    const component: MenusMultiSelectModalComponent = fixture.componentInstance;

    fixture.detectChanges();
    await fixture.whenStable();

    expect(component).toBeTruthy();
  });
});