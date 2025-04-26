import { TestBed } from '@angular/core/testing';
import { RouterModule } from '@angular/router';
import { AppComponent } from './app.component';
import { MsalService } from '@azure/msal-angular';

describe('AppComponent', () => {
  let msalService: jasmine.SpyObj<MsalService>;

  beforeEach(async () => {
    const msalServiceSpy = jasmine.createSpyObj('MsalService', ['instance']);

    await TestBed.configureTestingModule({
      imports: [
        RouterModule.forRoot([])
      ],
      declarations: [
        AppComponent
      ],
      providers: [{ provide: MsalService, useValue: msalServiceSpy }]
    }).compileComponents();

    msalService = TestBed.inject(MsalService) as jasmine.SpyObj<MsalService>;
  });

  it('should create the app', () => {
    const fixture = TestBed.createComponent(AppComponent);
    const app = fixture.componentInstance;
    expect(app).toBeTruthy();
  });

  it(`should have as title 'Pulse'`, () => {
    const fixture = TestBed.createComponent(AppComponent);
    const app = fixture.componentInstance;
    expect(app.title).toEqual('Pulse');
  });

  it('should render title', () => {
    const fixture = TestBed.createComponent(AppComponent);
    fixture.detectChanges();
    const compiled = fixture.nativeElement as HTMLElement;
    expect(compiled.querySelector('h1')?.textContent).toContain('Hello, Pulse');
  });
});
