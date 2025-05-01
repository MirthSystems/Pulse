## Overview of Authentication and Authorization Structure

When a user logs in through Auth0, they become an `ApplicationUser` in my system. Their Auth0 `UserObjectId` (e.g., "auth0|12345abcde") links their external identity to my database. From there, I manage two levels of authorization:

1. **Application-Wide Roles and Permissions**: These apply globally across the entire system, not tied to any specific venue.
2. **Venue-Specific Roles and Permissions**: These apply only to the venues a user is associated with, allowing granular control.

My goal is to support users like system administrators who can manage everything, as well as venue-specific users like owners or managers who only have access to certain venues with specific permissions.

## Application-Wide Authorization

As an `ApplicationUser`, I can have application-wide roles and permissions that give me broad access to the system. These are stored in the following entities:

- **`ApplicationRole`**: A role that applies system-wide, like "System Administrator" or "Content Manager". Each role has a `Value` (e.g., "System.Administrator") and a `DisplayName` for readability.
- **`ApplicationPermission`**: A specific action I can perform, like "venues:edit" or "specials:edit". Permissions use a colon-notation (e.g., "resource:action") and are the building blocks of authorization.
- **`ApplicationUserRole`**: Links me to my application roles, tracking when and by whom the role was assigned.
- **`ApplicationUserPermission`**: Allows me to have permissions directly assigned, bypassing roles for fine-grained control.

For example, if I have the "System Administrator" role, it might include the "venues:edit" permission. This means I can edit any venue in the system—create, update, or delete them—without needing to be tied to a specific venue. Similarly, a permission like "specials:edit" would let me manage specials across all venues. These permissions are checked first when determining my access, and if I have them, I don’t need venue-specific roles for those actions.

## Venue-Specific Authorization

For more granular control, I can be associated with specific venues through the `VenueUser` entity. This links me (an `ApplicationUser`) to a `Venue`, and I can have multiple roles for each venue. The related entities are:

- **`Venue`**: Represents a physical location (e.g., "The Rusty Anchor Pub") with details like name, address, and operating schedules.
- **`VenueUser`**: Connects me to a venue, with properties like `CreatedAt` and `IsActive` for auditing and control.
- **`VenueRole`**: A role specific to a venue, like "Venue Owner" or "Venue Manager", with a `Value` (e.g., "Venue.Owner") and permissions tied to it.
- **`VenueUserRole`**: Links my `VenueUser` record to one or more `VenueRole`s, allowing me to have multiple roles per venue (e.g., both "Owner" and "Manager").
- **`VenueRolePermission`**: Associates `VenueRole`s with `ApplicationPermission`s, reusing the same permission set as application-wide roles.

For instance, if I’m associated with Venue 1 as a "Venue Owner" and Venue 2 as a "Venue Manager":
- As "Venue Owner" for Venue 1, I might have "venues:edit" and "specials:edit", letting me edit all details and specials for that venue.
- As "Venue Manager" for Venue 2, I might only have "specials:edit", restricting me to managing specials without editing venue details.

I could also be linked to Venue 3 with a different role, and my permissions would adjust accordingly. The `VenueUserRole` entity tracks each role assignment with details like `AssignedAt` and `AssignedByUserId`, ensuring I have an audit trail.

## How Permissions Are Checked

When I try to perform an action, like editing a venue or a special, the system checks my permissions in this order:

1. **Application-Wide Check**:
   - Do I have the required permission (e.g., "venues:edit") through my `ApplicationRole`s or direct `ApplicationUserPermission`s?
   - If yes, I can perform the action on any venue—no further checks needed.

2. **Venue-Specific Check** (if no application-wide permission):
   - Am I associated with the target venue via a `VenueUser` record?
   - If yes, do any of my `VenueRole`s for that venue (via `VenueUserRole`) include the required permission through `VenueRolePermission`?
   - If yes, I can perform the action, but only for that specific venue.

For example:
- If I’m a "System Administrator" with "venues:edit", I can edit Venue 1, Venue 2, or any other venue.
- If I have no application-wide roles but I’m a "Venue Owner" for Venue 1 and Venue 2 (with "venues:edit" and "specials:edit") and a "Venue Manager" for Venue 3 (with "specials:edit"), then:
  - I can edit details and specials for Venue 1 and Venue 2.
  - I can only edit specials for Venue 3.

This hierarchy ensures flexibility: global admins don’t need venue associations, while venue-specific users get tailored access.

## Database Structure Details

Here’s how my entities tie together:

- **`ApplicationUser`**:
  - Links to `ApplicationUserRole` (my roles) and `ApplicationUserPermission` (my direct permissions).
  - Links to `VenueUser` (my venue associations).
- **`ApplicationRole`**:
  - Links to `ApplicationRolePermission` (permissions in the role).
- **`ApplicationPermission`**:
  - Reused across application-wide (`ApplicationRolePermission`, `ApplicationUserPermission`) and venue-specific (`VenueRolePermission`) contexts.
- **`Venue`**:
  - Links to `VenueUser` (users associated with it), `Address`, `OperatingSchedule`, and `Special`.
- **`VenueUser`**:
  - Links an `ApplicationUser` to a `Venue`.
  - Links to `VenueUserRole` (my roles for that venue).
- **`VenueRole`**:
  - Links to `VenueRolePermission` (permissions in the role).
- **`VenueUserRole`**:
  - Links `VenueUser` to `VenueRole`, allowing multiple roles per venue.

Additional entities like `Address` (with spatial `Point` data), `OperatingSchedule` (using NodaTime’s `LocalTime`), and `Special` (with `LocalDate` and cron schedules) support venue functionality but don’t directly impact permissions.

## Ensuring Robustness

I’ve designed this structure to be robust by:
- **Reusing Permissions**: `ApplicationPermission` works for both levels, keeping my permission set consistent (e.g., "venues:edit" means the same thing everywhere).
- **Flexibility**: I can have multiple roles per venue or none, and application-wide roles can stand alone.
- **Auditability**: Timestamps and user IDs (e.g., `AssignedByUserId`) track changes.
- **Efficiency**: The check order (application-wide first, then venue-specific) minimizes unnecessary queries.
- **Granularity**: Direct permissions and multiple venue roles per user allow precise control.

To optimize, I might cache permissions in the application layer, but the database structure supports efficient joins and queries via Entity Framework Core.

## Example Scenarios

1. **As a System Administrator**:
   - Role: "System Administrator" with "venues:edit" and "specials:edit".
   - I can edit all venues and specials system-wide, no `VenueUser` needed.

2. **As a Venue-Specific User**:
   - No application roles.
   - Venue 1: "Venue Owner" ("venues:edit", "specials:edit").
   - Venue 2: "Venue Owner" ("venues:edit", "specials:edit").
   - Venue 3: "Venue Manager" ("specials:edit").
   - I can edit details and specials for Venue 1 and 2, but only specials for Venue 3.