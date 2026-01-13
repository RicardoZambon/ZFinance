import { ComponentFixture, TestBed } from '@angular/core/testing';
import { UsersListComponent } from './users-list';

describe(UsersListComponent.name, () => {
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ UsersListComponent ]
    })
    .compileComponents();
  });

  it('should create', async () => {
    const fixture: ComponentFixture<UsersListComponent> = TestBed.createComponent(UsersListComponent);
    const component: UsersListComponent = fixture.componentInstance;

    fixture.detectChanges();
    await fixture.whenStable();

    expect(component).toBeTruthy();
  });
});