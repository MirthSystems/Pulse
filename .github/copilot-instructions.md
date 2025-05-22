# GitHub Copilot Instructions for Pulse - Real-Time Nightlife Discovery Platform

## Project Overview

Pulse is a cutting-edge, location-based platform that revolutionizes how users discover and engage with local nightlife venues. The platform leverages real-time data, community-sourced "vibe checks," and AI-driven recommendations to help users find the perfect venue based on what's happening right now.

**Project Lead:** Davis Kolakowski

## Core Architecture & Technology Stack

### Frontend (Progressive Web App)
- **React** with **TypeScript** for type safety and improved developer experience
- **Vite** as the build tool for fast development and optimized production builds
- **MUI (Material-UI)** for consistent Material Design UI components
- **Zustand** for lightweight state management (not Redux as initially planned)
- **Auth0 React SDK** for authentication integration
- **Luxon** via `@mui/x-date-pickers/AdapterLuxon` for date/time handling
- PWA capabilities: installable on mobile devices, offline support, push notifications

### Backend (.NET Core API)
- **.NET 9.0** with **C#** for API services
- **PostgreSQL** database with spatial extensions (PostGIS) for location-based queries
- **Entity Framework Core 9.0** with PostgreSQL provider
- **NodaTime** for precise date/time handling without timezone complications
- **NetTopologySuite** for spatial data operations
- **Auth0** integration for JWT-based authentication and authorization
- **AutoMapper** for object mapping
- **MediatR** for CQRS pattern implementation
- **FluentValidation** for input validation
- **Serilog** for structured logging

### Database & Spatial Data
- **PostgreSQL 14+** with multiple spatial extensions:
  - `postgis` - Core spatial functionality
  - `postgis_topology` - Topological support
  - `postgis_tiger_geocoder` - US address geocoding
  - `address_standardizer` - Address normalization
  - `fuzzystrmatch` - Fuzzy string matching
- **NodaTime** integration for timezone-aware date/time storage
- Spatial indexing for efficient location-based queries

### Authentication & Authorization
- **Auth0** as the identity provider with OAuth/OpenID Connect
- **JWT Bearer tokens** for API authentication
- **Two-tiered authorization system**:
  - Application-level roles (System Administrator, Venue Manager, Application User)
  - Venue-specific permissions (manage:venue, manage:specials, respond:posts, invite:users, manage:users)

## Core Domain Concepts

### Three-Category Classification System

1. **Venue Category** (Fixed/Predefined)
   - Examples: Bar, Restaurant, Cafe, Lounge, Club, Brewery, Concert Venue
   - Each venue has one primary type and up to two secondary types
   - Used for basic categorization and filtering

2. **Tags** (Applied to Specials - preceded by #)
   - Examples: #happyhour, #wingsnight, #liveband, #djset, #karaoke
   - Connect users with specific experiences they're seeking
   - Can be followed, searched, and filtered
   - Drive special discovery and promotion

3. **Vibes** (User-Generated Atmosphere - preceded by #)
   - Examples: #busy, #quiet, #greatservice, #goodvibes, #nocover
   - Describe current venue conditions in real-time
   - Temporary and tied to 15-minute post expiration
   - Help users know what to expect right now

### Ephemeral Content Model (15-Minute Rule)
- All user-generated content (posts, photos, videos, vibes) automatically expires after 15 minutes
- Ensures information is always current and relevant
- No historical data or content archives
- Prevents outdated or misleading information
- Encourages active, real-time participation

### Special-Driven Discovery
- **Specials** are the primary connection between venues and users
- Support both one-time and recurring events with CRON scheduling
- Include timing details (start time, end time, expiration dates)
- Associated with Tags for discovery (#wingsnight, #happyhour, etc.)
- Time-sensitive nature creates urgency and real-time engagement

## Key Entities & Data Models

### Core Entities

```csharp
// Special entity with NodaTime for timezone handling
public class Special
{
    public long Id { get; set; }
    public long VenueId { get; set; }
    public required string Content { get; set; }  // "Half-Price Wings Happy Hour"
    public SpecialTypes Type { get; set; }        // Food, Drink, Entertainment
    public LocalDate StartDate { get; set; }      // First occurrence date
    public LocalTime StartTime { get; set; }      // Daily start time
    public LocalTime? EndTime { get; set; }       // Daily end time (optional)
    public LocalDate? EndDate { get; set; }       // Last occurrence (for recurring)
    public LocalDate? ExpirationDate { get; set; } // When special expires
    public bool IsRecurring { get; set; }
    public string? CronSchedule { get; set; }     // "0 17 * * *" for daily 5 PM
    public Instant CreatedAt { get; set; }
    public required string CreatedByUserId { get; set; } // Auth0 user ID
    public virtual Venue? Venue { get; set; }
}
```

### Database Schema Key Points
- **Venues**: Location data with PostGIS Point geometry, primary/secondary types
- **OperatingSchedule**: Business hours with day-of-week support
- **Specials**: Full timing control with NodaTime and CRON scheduling
- **Tags**: Hashtag-style identifiers for special discovery
- **Posts**: Ephemeral user content with 15-minute TTL
- **Vibes**: Atmosphere descriptors linked to posts
- **ApplicationUser**: Local user data with Auth0 integration
- **VenuePermission/VenueUser**: Granular venue-specific access control

## Privacy-First Location System

### Default Approach
- **Manual address entry** as the primary method
- Search around specific addresses with adjustable radius
- City/neighborhood selection for broader discovery
- **NO automatic location tracking by default**

### Optional Location Services
- Opt-in "Use My Current Location" button
- No background location tracking
- No persistent storage of precise location data
- User controls when and how location is used

## Development Guidelines

### Code Style & Patterns

#### Frontend (React/TypeScript)
```typescript
// Use functional components with hooks
export const VenuePage = () => {
  const { id } = useParams<{ id: string }>();
  const { currentVenue, fetchVenueById, isLoading } = useVenuesStore();
  
  useEffect(() => {
    if (id) {
      fetchVenueById(parseInt(id));
    }
  }, [id, fetchVenueById]);

  return (
    <Container maxWidth="lg">
      {/* Component content */}
    </Container>
  );
};
```

#### State Management with Zustand
```typescript
interface VenuesState {
  venues: VenueItemExtended[];
  currentVenue: VenueItemExtended | null;
  isLoading: boolean;
  fetchVenues: () => Promise<void>;
}

export const useVenuesStore = create<VenuesState>((set, get) => ({
  venues: [],
  currentVenue: null,
  isLoading: false,
  fetchVenues: async () => {
    // Implementation
  },
}));
```

#### Backend (.NET Core)
```csharp
// Use CQRS pattern with MediatR
public class GetSpecialByIdQuery : IRequest<SpecialDto>
{
    public long SpecialId { get; set; }
}

public class GetSpecialByIdHandler : IRequestHandler<GetSpecialByIdQuery, SpecialDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public async Task<SpecialDto> Handle(GetSpecialByIdQuery request, CancellationToken cancellationToken)
    {
        // Implementation using EF Core and AutoMapper
    }
}
```

#### Entity Framework Configuration
```csharp
public class SpecialConfiguration : IEntityTypeConfiguration<Special>
{
    public void Configure(EntityTypeBuilder<Special> builder)
    {
        builder.Property(s => s.Content).IsRequired().HasMaxLength(500);
        builder.Property(s => s.StartDate).IsRequired();
        builder.Property(s => s.StartTime).IsRequired();
        builder.Property(s => s.CreatedByUserId).IsRequired().HasMaxLength(100);
        
        // Configure NodaTime properties
        builder.Property(s => s.CreatedAt).IsRequired();
        
        // Relationships
        builder.HasOne(s => s.Venue)
               .WithMany()
               .HasForeignKey(s => s.VenueId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
```

### Authentication Integration

#### Frontend Auth0 Setup
```typescript
// App.tsx - Auth0Provider configuration
const domain = import.meta.env.VITE_AUTH0_DOMAIN as string;
const clientId = import.meta.env.VITE_AUTH0_CLIENT_ID as string;
const audience = import.meta.env.VITE_AUTH0_AUDIENCE as string;

<Auth0Provider
  domain={domain}
  clientId={clientId}
  authorizationParams={{
    redirect_uri: `${window.location.origin}/callback`,
    audience: audience,
  }}
>
  <App />
</Auth0Provider>
```

#### Protected Routes
```typescript
export const ProtectedRoute = ({ children }: { children: React.ReactNode }) => {
  const { isAuthenticated, isLoading } = useAuth0();

  if (isLoading) return <CircularProgress />;
  if (!isAuthenticated) return <Navigate to="/" replace />;
  
  return <>{children}</>;
};
```

#### Backend JWT Configuration
```csharp
// Program.cs - JWT Bearer authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = builder.Configuration["Auth0:Domain"];
        options.Audience = builder.Configuration["Auth0:Audience"];
        options.TokenValidationParameters = new TokenValidationParameters
        {
            NameClaimType = ClaimTypes.NameIdentifier,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true
        };
    });
```

### Spatial Data Handling

#### PostGIS Setup
```csharp
// ApplicationDbContext.cs
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    // Enable PostgreSQL extensions
    modelBuilder.HasPostgresExtension("postgis");
    modelBuilder.HasPostgresExtension("postgis_tiger_geocoder");
    modelBuilder.HasPostgresExtension("address_standardizer");
    
    // Configure spatial properties
    modelBuilder.Entity<Venue>()
        .Property(v => v.Location)
        .HasColumnType("geography (point)");
}
```

#### Location Queries
```csharp
// Distance-based venue search
var nearbyVenues = await _context.Venues
    .Where(v => v.Location.Distance(searchPoint) <= radiusInMeters)
    .OrderBy(v => v.Location.Distance(searchPoint))
    .ToListAsync();
```

### Time Handling with NodaTime

#### Entity Properties
```csharp
// Use NodaTime types for precise time handling
public LocalDate StartDate { get; set; }        // Date without timezone
public LocalTime StartTime { get; set; }        // Time without timezone  
public Instant CreatedAt { get; set; }          // UTC timestamp
public LocalDate? ExpirationDate { get; set; }  // Optional expiration
```

#### CRON Scheduling
```csharp
// Examples of CRON expressions for recurring specials
"0 17 * * *"     // Daily at 5 PM
"0 20 * * 1"     // Every Monday at 8 PM  
"0 16 * * 1-5"   // Weekdays at 4 PM
"0 21 1-7 * 6"   // First Saturday of month at 9 PM
```

## UI/UX Guidelines

### Material Design with MUI
- Use MUI components consistently throughout the application
- Implement both light and dark theme support
- Responsive design for mobile-first experience
- PWA-optimized interactions and performance

### Component Structure
```typescript
// Consistent component organization
export const VenueCard = ({ venue }: { venue: VenueItemExtended }) => {
  const theme = useTheme();
  
  return (
    <Card
      elevation={theme.palette.mode === 'light' ? 1 : 2}
      sx={{
        borderRadius: 3,
        transition: 'all 0.3s ease',
        '&:hover': {
          transform: 'translateY(-2px)',
          boxShadow: theme.shadows[8],
        },
      }}
    >
      {/* Card content */}
    </Card>
  );
};
```

### Theme Configuration
```typescript
// Custom theme with Pulse branding
export const lightThemeOptions: ThemeOptions = {
  palette: {
    mode: 'light',
    primary: {
      main: '#00C2CB', // Pulse cyan
    },
    secondary: {
      main: '#5CF9FF', // Light cyan accent
    },
  },
  // Custom component overrides
};
```

## Feature Implementation Guidelines

### Real-Time Features
- Implement WebSocket connections for live updates
- Use optimistic updates for immediate UI feedback
- Handle connection failures gracefully
- Implement proper loading states and error boundaries

### Special Management
- Support both one-time and recurring specials
- Implement CRON-based scheduling for recurring events
- Provide rich date/time pickers with timezone awareness
- Enable bulk operations for venue managers

### Search & Discovery
- Implement full-text search with PostgreSQL
- Support spatial queries for location-based results
- Enable filtering by Types, Tags, and Vibes
- Provide auto-suggestion and typeahead functionality

### Content Moderation
- Implement automated content filtering
- Provide reporting mechanisms for users
- Enable manual review workflows for administrators
- Enforce 15-minute expiration automatically

## Testing Strategy

### Frontend Testing
- Unit tests with Vitest/Jest and React Testing Library
- Component testing for UI interactions
- Integration tests for API communication
- E2E tests with Playwright for critical user flows

### Backend Testing
- Unit tests for business logic and domain entities
- Integration tests for database operations
- API endpoint testing with proper authentication
- Performance testing for spatial queries

## Deployment & Infrastructure

### Azure Cloud Services
- Containerized deployment with Docker
- Azure App Service for backend APIs
- Azure Database for PostgreSQL with PostGIS
- Azure Static Web Apps for frontend PWA
- Azure CDN for global content delivery

### Environment Configuration
- Separate configurations for Development, Staging, Production
- Secure secret management with Azure Key Vault
- Environment-specific Auth0 tenants
- Database migration strategies

## Security Considerations

### Data Protection
- No persistent storage of precise user location
- Secure handling of Auth0 tokens
- Proper input validation and sanitization
- Rate limiting for API endpoints

### Authentication Security
- JWT token validation on all protected endpoints
- Proper CORS configuration
- Secure cookie handling for session management
- Regular security audits and updates

## Performance Optimization

### Database Performance
- Proper indexing for spatial queries
- Connection pooling and query optimization
- Efficient pagination for large result sets
- Caching strategies for frequently accessed data

### Frontend Performance
- Code splitting and lazy loading
- Image optimization and compression
- Service worker implementation for offline support
- Bundle size optimization with tree shaking

## Monitoring & Analytics

### Application Monitoring
- Structured logging with Serilog
- Performance metrics and APM
- Error tracking and alerting
- User analytics and engagement metrics

### Business Metrics
- Special engagement and conversion rates
- User retention and activity patterns
- Venue performance analytics
- Tag and Vibe usage statistics

## Future Roadmap Considerations

### AI Integration (Post-MVP)
- Migration to Azure Cognitive Services for recommendations
- Machine learning for personalized content
- Predictive analytics for venue trends
- Natural language processing for content analysis

### Mobile Applications
- React Native or native iOS/Android apps
- Enhanced device integration
- Push notification capabilities
- Offline-first architecture

Remember: Pulse prioritizes real-time, authentic experiences over static reviews. Every feature should reinforce the "right now" mentality and encourage active community participation while respecting user privacy.
