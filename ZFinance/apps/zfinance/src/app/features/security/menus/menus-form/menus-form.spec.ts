import { ComponentFixture, TestBed } from '@angular/core/testing';
import { MenusFormComponent } from './menus-form';

describe(MenusFormComponent.name, () => {
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MenusFormComponent ]
    })
    .compileComponents();
  });

  it('should create', async () => {
    const fixture: ComponentFixture<MenusFormComponent> = TestBed.createComponent(MenusFormComponent);
    const component: MenusFormComponent = fixture.componentInstance;

    fixture.detectChanges();
    await fixture.whenStable();

    expect(component).toBeTruthy();
  });
});