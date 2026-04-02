# Solution Overview

This solution implements a bike catalogue with:

1. A .NET 9 REST API backed by SQLite (EF Core).
2. Angular 17 frontend pages for browsing, creating, and viewing bike details.
3. A bug fix for the “My Favourites” feature to prevent duplicate favourites.

---

## Design Decisions & Assumptions

### Backend (.NET) / API

- **Database**: SQLite (`bikes.db`) for simplicity and local execution. The database is created automatically on startup and seeded with the initial bikes from the existing frontend JSON (`src/assets/data/bikes.json`).
- **API endpoints** (REST):
  - `GET /api/bikes` - returns all bikes.
  - `GET /api/bikes/{ref}` - returns a bike by its reference.
  - `POST /api/bikes` - creates a new bike.
- **Data model**:
  - The bike’s unique identifier is the `ref` field (string/GUID stored as string).
  - `img_url` is preserved exactly as expected by the frontend.
- **DDD / SOLID approach (lightweight for a challenge)**:
  - `Domain`: `Bike` entity.
  - `Application`: `BikeAppService` + `IBikeRepository` abstraction.
  - `Infrastructure`: `BikesDbContext`, `EfBikeRepository`, and `BikeSeeder`.
- **Error handling**:
  - Validation errors return `400 Bad Request`.
  - Conflicts (e.g., duplicate `ref`) return `409 Conflict`.
- **CORS**: enabled for all origins to simplify local development (Angular runs on a different port than the API).

### Frontend (Angular)

- **API integration**:
  - `BikeService` now calls the backend API (`http://localhost:5111/api/bikes`) instead of reading `assets/data/bikes.json`.
- **Favourites bug fix**:
  - `HomeComponent` now initializes each bike’s `inFavourites` state from `localStorage` on page load.
  - When adding to favourites, it deduplicates using a `Set` before writing back to `localStorage`.
  - `MyFavouritesComponent` deduplicates stored favourite refs before mapping them to bike objects.
- **New pages**:
  - `Create Bike` page: form submits to `POST /api/bikes`, then navigates to the created bike’s details.
  - `Bike Details` page: displays all bike fields and includes Add/Remove to/from favourites actions.
- **Navigation UX**:
  - The bike card is clickable: it routes to `/details/:ref`.
  - Header includes a “Create Bike” link.

---

## Testing

- Angular unit test `bike.service.spec.ts` was updated to assert the new `GET /api/bikes` request URL.
- Backend test project `BikeShop.API.Tests` (xUnit) was added.
- `BikeAppService` unit tests cover:
  - returning mapped bikes for list queries
  - returning `null` for non-existent refs
  - input validation for create requests
  - create flow persistence behavior via in-memory repository stub

---

## Improvements With More Time

- Backend
  - Add migrations instead of `EnsureCreated()` + seeding.
  - Introduce a global exception handler (`ProblemDetails`) for consistent error responses.
  - Expand backend test coverage to repository/integration tests (SQLite in-memory) and controller-level tests.
  - Use stronger validation (e.g., FluentValidation) for the create request.
- Frontend
  - Replace hardcoded API base URL with Angular environments configuration.
  - Add proper form validation (required formats, URL validation, better feedback).
  - Add tests for the new create/details components.

