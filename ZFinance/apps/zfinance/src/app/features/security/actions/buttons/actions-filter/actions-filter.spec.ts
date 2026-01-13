import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ActionsFilterComponent } from './actions-filter';

describe(ActionsFilterComponent.name, () => {
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ActionsFilterComponent ]
    })
    .compileComponents();
  });

  it('should create', async () => {
    const fixture: ComponentFixture<ActionsFilterComponent> = TestBed.createComponent(ActionsFilterComponent);
    const component: ActionsFilterComponent = fixture.componentInstance;

    fixture.detectChanges();
    await fixture.whenStable();

    expect(component).toBeTruthy();
  });
});