import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ActionsListComponent } from './actions-list';

describe(ActionsListComponent.name, () => {
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ActionsListComponent ]
    })
    .compileComponents();
  });

  it('should create', async () => {
    const fixture: ComponentFixture<ActionsListComponent> = TestBed.createComponent(ActionsListComponent);
    const component: ActionsListComponent = fixture.componentInstance;

    fixture.detectChanges();
    await fixture.whenStable();

    expect(component).toBeTruthy();
  });
});