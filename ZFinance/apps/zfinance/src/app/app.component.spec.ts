import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AppComponent } from './app.component';

describe(AppComponent.name, () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ AppComponent ]
    })
    .compileComponents();
  });

  it('should create', () => {
    const fixture: ComponentFixture<AppComponent> = TestBed.createComponent(AppComponent);
    const component: AppComponent = fixture.componentInstance;

    expect(component).toBeTruthy();
  });
});