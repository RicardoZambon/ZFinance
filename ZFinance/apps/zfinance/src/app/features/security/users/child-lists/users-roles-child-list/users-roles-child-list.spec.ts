import { ComponentFixture, TestBed } from '@angular/core/testing';
import { UsersRolesChildListComponent } from './users-roles-child-list';

describe(UsersRolesChildListComponent.name, () => {
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ UsersRolesChildListComponent ]
    })
    .compileComponents();
  });

  it('should create', async () => {
    const fixture: ComponentFixture<UsersRolesChildListComponent> = TestBed.createComponent(UsersRolesChildListComponent);
    const component: UsersRolesChildListComponent = fixture.componentInstance;

    fixture.detectChanges();
    await fixture.whenStable();

    expect(component).toBeTruthy();
  });
});