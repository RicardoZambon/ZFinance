import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ActionsMultiSelectModalComponent } from './actions-multi-select-modal';

describe(ActionsMultiSelectModalComponent.name, () => {
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ActionsMultiSelectModalComponent ]
    })
    .compileComponents();
  });

  it('should create', async () => {
    const fixture: ComponentFixture<ActionsMultiSelectModalComponent> = TestBed.createComponent(ActionsMultiSelectModalComponent);
    const component: ActionsMultiSelectModalComponent = fixture.componentInstance;

    fixture.detectChanges();
    await fixture.whenStable();

    expect(component).toBeTruthy();
  });
});