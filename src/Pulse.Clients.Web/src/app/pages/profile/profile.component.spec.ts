import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { of, throwError } from 'rxjs';
import { ProfileComponent } from './profile.component';
import { GraphService } from '../../services/graph.service';
import { User } from '@microsoft/microsoft-graph-types';

describe('ProfileComponent', () => {
  let component: ProfileComponent;
  let fixture: ComponentFixture<ProfileComponent>;
  let graphService: jasmine.SpyObj<GraphService>;

  const mockUser: User = {
    displayName: 'John Doe',
    givenName: 'John',
    surname: 'Doe',
    mail: 'john.doe@example.com',
    userPrincipalName: 'john.doe@example.com',
    id: '12345',
    jobTitle: 'Software Engineer',
    department: 'Engineering',
    officeLocation: 'Building 1'
  };

  beforeEach(async () => {
    const graphServiceSpy = jasmine.createSpyObj('GraphService', ['getUserProfile', 'getUserPhoto']);

    await TestBed.configureTestingModule({
      imports: [HttpClientTestingModule, MatSnackBarModule],
      declarations: [ProfileComponent],
      providers: [{ provide: GraphService, useValue: graphServiceSpy }]
    }).compileComponents();

    fixture = TestBed.createComponent(ProfileComponent);
    component = fixture.componentInstance;
    graphService = TestBed.inject(GraphService) as jasmine.SpyObj<GraphService>;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should load user profile on init', () => {
    graphService.getUserProfile.and.returnValue(of(mockUser));
    graphService.getUserPhoto.and.returnValue(of(new Blob()));

    fixture.detectChanges();

    expect(component.profile).toEqual(mockUser);
    expect(graphService.getUserProfile).toHaveBeenCalled();
    expect(graphService.getUserPhoto).toHaveBeenCalled();
  });

  it('should handle error when loading user profile', () => {
    const errorMessage = 'Failed to load user profile. Please try again later.';
    graphService.getUserProfile.and.returnValue(throwError(() => new Error('Error')));

    fixture.detectChanges();

    expect(component.error).toEqual(errorMessage);
    expect(graphService.getUserProfile).toHaveBeenCalled();
  });

  it('should handle error when loading user photo', () => {
    graphService.getUserProfile.and.returnValue(of(mockUser));
    graphService.getUserPhoto.and.returnValue(throwError(() => new Error('Error')));

    fixture.detectChanges();

    expect(component.profile).toEqual(mockUser);
    expect(component.profilePicture).toBeUndefined();
    expect(graphService.getUserPhoto).toHaveBeenCalled();
  });
});
