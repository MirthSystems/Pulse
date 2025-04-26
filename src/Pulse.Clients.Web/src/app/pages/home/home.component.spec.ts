import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { MsalService } from '@azure/msal-angular';
import { HomeComponent } from './home.component';

describe('HomeComponent', () => {
  let component: HomeComponent;
  let fixture: ComponentFixture<HomeComponent>;
  let msalService: jasmine.SpyObj<MsalService>;

  beforeEach(async () => {
    const msalServiceSpy = jasmine.createSpyObj('MsalService', ['loginRedirect', 'instance']);

    await TestBed.configureTestingModule({
      imports: [HttpClientTestingModule, RouterTestingModule],
      declarations: [HomeComponent],
      providers: [{ provide: MsalService, useValue: msalServiceSpy }]
    }).compileComponents();

    fixture = TestBed.createComponent(HomeComponent);
    component = fixture.componentInstance;
    msalService = TestBed.inject(MsalService) as jasmine.SpyObj<MsalService>;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should call loginRedirect when login is called', () => {
    component.login();
    expect(msalService.loginRedirect).toHaveBeenCalled();
  });

  it('should return true for isLoggedIn when there is an active account', () => {
    msalService.instance.getActiveAccount.and.returnValue({});
    expect(component.isLoggedIn).toBeTrue();
  });

  it('should return false for isLoggedIn when there are no active accounts', () => {
    msalService.instance.getActiveAccount.and.returnValue(null);
    msalService.instance.getAllAccounts.and.returnValue([]);
    expect(component.isLoggedIn).toBeFalse();
  });
});
