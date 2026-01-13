import { ComponentFixture, TestBed } from '@angular/core/testing';
import { RolesUsersChildListComponent } from './roles-users-child-list';

describe(RolesUsersChildListComponent.name, () => {
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RolesUsersChildListComponent ]
    })
    .compileComponents();
  });

  it('should create', async () => {
    const fixture: ComponentFixture<RolesUsersChildListComponent> = TestBed.createComponent(RolesUsersChildListComponent);
    const component: RolesUsersChildListComponent = fixture.componentInstance;

    fixture.detectChanges();
    await fixture.whenStable();

    expect(component).toBeTruthy();
  });
});