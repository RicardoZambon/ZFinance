import { ComponentFixture, TestBed } from '@angular/core/testing';
import { UsersFilterComponent } from './users-filter';

describe(UsersFilterComponent.name, () => {
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ UsersFilterComponent ]
    })
    .compileComponents();
  });

  it('should create', async () => {
    const fixture: ComponentFixture<UsersFilterComponent> = TestBed.createComponent(UsersFilterComponent);
    const component: UsersFilterComponent = fixture.componentInstance;

    fixture.detectChanges();
    await fixture.whenStable();

    expect(component).toBeTruthy();
  });
});