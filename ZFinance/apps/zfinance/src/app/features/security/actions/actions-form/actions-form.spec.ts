import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ActionsFormComponent } from './actions-form';

describe(ActionsFormComponent.name, () => {
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ActionsFormComponent ]
    })
    .compileComponents();
  });

  it('should create', async () => {
    const fixture: ComponentFixture<ActionsFormComponent> = TestBed.createComponent(ActionsFormComponent);
    const component: ActionsFormComponent = fixture.componentInstance;

    fixture.detectChanges();
    await fixture.whenStable();

    expect(component).toBeTruthy();
  });
});