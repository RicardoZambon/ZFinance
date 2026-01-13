import { ComponentFixture, TestBed } from '@angular/core/testing';
import { UsersMultiSelectModalComponent } from './users-multi-select-modal';

describe(UsersMultiSelectModalComponent.name, () => {
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ UsersMultiSelectModalComponent ]
    })
    .compileComponents();
  });

  it('should create', async () => {
    const fixture: ComponentFixture<UsersMultiSelectModalComponent> = TestBed.createComponent(UsersMultiSelectModalComponent);
    const component: UsersMultiSelectModalComponent = fixture.componentInstance;

    fixture.detectChanges();
    await fixture.whenStable();

    expect(component).toBeTruthy();
  });
});