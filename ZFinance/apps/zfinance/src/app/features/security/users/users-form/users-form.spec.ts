import { ComponentFixture, TestBed } from '@angular/core/testing';
import { UsersFormComponent } from './users-form';

describe(UsersFormComponent.name, () => {
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ UsersFormComponent ]
    })
    .compileComponents();
  });

  it('should create', async () => {
    const fixture: ComponentFixture<UsersFormComponent> = TestBed.createComponent(UsersFormComponent);
    const component: UsersFormComponent = fixture.componentInstance;

    fixture.detectChanges();
    await fixture.whenStable();

    expect(component).toBeTruthy();
  });
});