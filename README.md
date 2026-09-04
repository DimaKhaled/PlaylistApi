# Playlist API

A backend REST API for managing user playlists and songs using **ASP.NET Core 10**.

Users can register and log in, create their own playlists, search for songs using the **iTunes Search API**, add songs to their playlists, view their playlists, update or delete playlists, and remove songs.

Songs are **not created or entered manually by users**. The application retrieves song information from iTunes and stores the required song metadata locally when a song is added to a playlist.

The API uses **ASP.NET Core Identity** for user management and password validation, and **JWT Bearer Authentication** to secure user-specific endpoints.

---

## Features

* User registration and login
* ASP.NET Core Identity for user management
* JWT Bearer authentication
* Create, read, update, and delete playlists
* Search songs through the iTunes API
* Add iTunes songs to playlists
* Remove songs from playlists
* User-specific playlists and ownership protection
* SQL Server database with Entity Framework Core
* Clean Architecture
* Repository Pattern
* DTOs for API contracts
* Dependency Injection
* Global exception handling
* Input validation
* EF Core migrations

---

## How the Application Works

The main flow of the application is:

```text
User
 │
 ├── Register / Login
 │       ↓
 │   ASP.NET Core Identity
 │       ↓
 │   JWT Token
 │
 └── Authenticated Requests
         ↓
      API Controllers
         ↓
    Application Services
         ↓
      Repositories
         ↓
      SQL Server
```

For songs, the flow is different because song information comes from an external service:

```text
User searches for a song
        ↓
SongsController
        ↓
SongService
        ↓
IMusicService
        ↓
ITunesMusicService
        ↓
iTunes Search API
```

When the user adds a song to a playlist, the client only sends the **iTunes external song ID**.

The API then retrieves the song information from iTunes and stores the song metadata locally if it does not already exist.

This means the client does **not** send or create the song title, artist, album, artwork, etc.

---

# Technology Stack

| Technology            | Purpose                                 |
| --------------------- | --------------------------------------- |
| ASP.NET Core 10       | REST API                                |
| C#                    | Programming language                    |
| SQL Server            | Database                                |
| Entity Framework Core | ORM / database access                   |
| ASP.NET Core Identity | User management and password validation |
| JWT Bearer            | Authentication                          |
| iTunes Search API     | External song search and metadata       |
| Clean Architecture    | Application structure                   |
| Git                   | Version control                         |

---

# Architecture

The project follows **Clean Architecture** 

```text
PlaylistApi.API
       ↓
PlaylistApi.Application
       ↓
PlaylistApi.Domain

PlaylistApi.Infrastructure
       ↓
Application + Domain
```

### Projects

**PlaylistApi.Domain**

Contains the core entities:

* `Playlist`
* `Song`
* `PlaylistSong`

**PlaylistApi.Application**

Contains the application/business logic:

* Application services
* Interfaces
* DTOs
* Mappings
* Application exceptions

**PlaylistApi.Infrastructure**

Contains implementations of technical concerns:

* SQL Server / EF Core
* Repositories
* ASP.NET Core Identity
* JWT
* Current user service
* iTunes integration

**PlaylistApi.API**

Contains the HTTP layer:

* Controllers
* Exception middleware
* Application startup and configuration

For more details, see [`docs/ARCHITECTURE.md`](docs/ARCHITECTURE.md).

---

# Database

The project uses **SQL Server** because the application's data is structured and contains well-defined relationships.

The main relationships are:

```text
User
  │
  └── 1 ──── * ──── Playlist
                       │
                       │ *
                       │
                       * 
                  PlaylistSong
                       │
                       │
                       * 
                       │
                       1
                      Song
```

A playlist can contain multiple songs, and a song can belong to multiple playlists. Therefore, `PlaylistSongs` is used as the junction table for the many-to-many relationship.

Song metadata is stored locally after being retrieved from iTunes. This allows playlists to be retrieved without requesting the song information from iTunes every time.

For the complete database design, see [`docs/DATABASE.md`](docs/DATABASE.md).

---

# Prerequisites

Before running the project, install:

### 1. .NET 10 SDK

Install the .NET 10 SDK from the official Microsoft website.

You can verify the installation with:

```bash
dotnet --version
```

The project targets **.NET 10**.

### 2. SQL Server

A SQL Server instance is required.

You can use:

* SQL Server Developer
* SQL Server Express
* Another accessible SQL Server instance

The project does not use LocalDB.

### 3. Git

Git is required to clone the repository.

Verify it with:

```bash
git --version
```

No API key is required for the iTunes Search API used by this project.

---

# Clone the Repository

Clone the repository:

```bash
git clone <repository-url>
```

Then move into the project directory:

```bash
cd PlaylistApi
```

Restore the dependencies:

```bash
dotnet restore
```

Build the solution:

```bash
dotnet build
```

---

# Configuration

The application uses **.NET User Secrets** to store configuration values that should not be committed to the repository, such as the database connection string and JWT signing key.

## Database Connection String

The API requires a SQL Server connection string.

The connection string is stored as:

```text
ConnectionStrings:DefaultConnection
```

To configure it using Visual Studio:

1. In **Solution Explorer**, right-click the `PlaylistApi.API` project.
2. Select **Manage User Secrets**.
3. Visual Studio will open the project's `secrets.json` file.
4. Add the `ConnectionStrings` section:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=PlaylistApiDb;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

Replace `YOUR_SERVER_NAME` with the name of your SQL Server instance.

For example:

```text
Server=localhost;Database=PlaylistApiDb;Trusted_Connection=True;TrustServerCertificate=True;
```

or, if using SQL Server Express:

```text
Server=YOUR-PC-NAME\SQLEXPRESS;Database=PlaylistApiDb;Trusted_Connection=True;TrustServerCertificate=True;
```

## JWT Configuration

The API also requires a secret key for signing JWT tokens.

In the same `secrets.json` file opened through **Manage User Secrets**, add the `Jwt` section:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=PlaylistApiDb;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "Jwt": {
    "Key": "YOUR_LONG_RANDOM_SECRET_KEY"
  }
}
```

Replace `YOUR_LONG_RANDOM_SECRET_KEY` with a long random secret key.

The `Issuer` and `Audience` values are already included in `appsettings.json`, so they do not need to be added to User Secrets.

For example, the final `secrets.json` can look like:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=PlaylistApiDb;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "Jwt": {
    "Key": "your-long-random-development-secret-key"
  }
}
```

> **Important:** Do not commit the actual `secrets.json` contents, database credentials, or JWT secret key to the repository.


---

# Create the Database

The project already contains an EF Core migration.

From the repository root, run:

```bash
dotnet ef database update --project src/PlaylistApi.Infrastructure --startup-project src/PlaylistApi.API
```

If the `dotnet ef` command is not available, install the EF Core CLI tool:

```bash
dotnet tool install --global dotnet-ef
```

Then run the database update command again.

This creates the application database, including:

* ASP.NET Core Identity tables
* `Playlists`
* `Songs`
* `PlaylistSongs`

---

# Run the Application

From the repository root:

```bash
dotnet run --project src/PlaylistApi.API
```

Alternatively, open the solution in Visual Studio and run the `PlaylistApi.API` project.

The API will start on the URLs displayed in the console.

---

# Authentication

The application uses **ASP.NET Core Identity** for user management and password validation, combined with **JWT** for authentication.

After a successful registration or login, the API returns an access token:

```json
{
  "accessToken": "<jwt-token>"
}
```

The token must be included when calling protected endpoints.

## Using the Token

When testing the API, send the token using the standard HTTP Authorization header:

```http
Authorization: Bearer <access-token>
```

For example:

```text
Authorization: Bearer eyJhbGciOiJIUzI1NiIs...
```

### Postman / API Client

When using Postman:

1. Open the request.
2. Go to the **Authorization** tab.
3. Set **Type** to **Bearer Token**.
4. Paste the access token into the **Token** field.
5. Send the request.

Do **not** select a JWT-specific authorization type. The request should use **Bearer Token**.

ASP.NET Core's JWT Bearer authentication middleware reads the `Authorization: Bearer <token>` header and validates the token automatically.

All playlist and song endpoints require this Bearer token. The registration and login endpoints do not require authentication.


---

# API Endpoints

All endpoints under `Playlists` and `Songs` require an access token sent using `Authorization → Bearer Token`.

## Authentication

### Register

```http
POST /api/Auth/register
```

Creates a new user.

**Request:**

```json
{
  "email": "user@example.com",
  "password": "Password@123"
}
```

**Response:**

```json
{
  "accessToken": "<jwt-token>"
}
```

---

### Login

```http
POST /api/Auth/login
```

Authenticates an existing user.

**Request:**

```json
{
  "email": "user@example.com",
  "password": "Password@123"
}
```

**Response:**

```json
{
  "accessToken": "<jwt-token>"
}
```

---

# Playlists

All playlist endpoints require:

```http
Authorization: Bearer Token
```

### Create Playlist

```http
POST /api/Playlists
```

Creates a playlist for the authenticated user.

**Request:**

```json
{
  "name": "My Favorites",
  "description": "Songs I like"
}
```

---

### Get My Playlists

```http
GET /api/Playlists
```

Returns all playlists belonging to the authenticated user.

---

### Get Playlist

```http
GET /api/Playlists/{playlistId}
```

Returns one playlist and its songs.

The API verifies that the playlist belongs to the authenticated user.

---

### Update Playlist

```http
PUT /api/Playlists/{playlistId}
```

Updates the name and description of a playlist owned by the authenticated user.

**Request:**

```json
{
  "name": "Updated Favorites",
  "description": "Updated description"
}
```

---

### Delete Playlist

```http
DELETE /api/Playlists/{playlistId}
```

Deletes a playlist owned by the authenticated user.

The playlist's `PlaylistSongs` records are deleted as part of the relationship, while the shared `Song` records are retained.

---

# Songs

All song endpoints require authentication.

## Search Songs

```http
GET /api/Songs/search?query=Imagine
```

Searches the **iTunes Search API**.

Example response:

```json
[
  {
    "externalId": 123456789,
    "title": "Imagine",
    "artistName": "John Lennon",
    "albumName": "Imagine",
    "durationSeconds": 183,
    "artworkUrl": "https://...",
    "externalUrl": "https://...",
    "previewUrl": "https://..."
  }
]
```

The `externalId` is the song's ID from iTunes.

---

## Add Song to Playlist

```http
POST /api/Songs/playlists/{playlistId}
```

The client sends only the iTunes song ID.

**Request:**

```json
{
  "externalId": 123456789
}
```

The API then:

1. Verifies that the playlist belongs to the authenticated user.
2. Checks whether the song already exists locally.
3. If it does not exist, requests the song from iTunes.
4. Stores the song metadata locally.
5. Creates the playlist-song relationship.
6. Returns the added song.

Therefore, the user does not manually provide the song title, artist, album, artwork, etc.

---

## Remove Song from Playlist

```http
DELETE /api/Songs/playlists/{playlistId}/{songId}
```

Removes a song from the specified playlist.

`playlistId` identifies the playlist, while `songId` is the **internal database ID** of the song.

Removing a song from a playlist does not delete the shared song record from the `Songs` table.

---

# Endpoint Summary

| Method | Endpoint                                     | Authentication | Purpose                        |
| ------ | -------------------------------------------- | -------------- | ------------------------------ |
| POST   | `/api/Auth/register`                         | None             | Register a user and receive JWT               |
| POST   | `/api/Auth/login`                            | None             | Login and receive JWT          |
| POST   | `/api/Playlists`                             | Bearer Token            | Create playlist                |
| GET    | `/api/Playlists`                             | Bearer Token            | Get user's playlists           |
| GET    | `/api/Playlists/{playlistId}`                | Bearer Token            | Get one playlist               |
| PUT    | `/api/Playlists/{playlistId}`                | Bearer Token            | Update playlist                |
| DELETE | `/api/Playlists/{playlistId}`                | Bearer Token            | Delete playlist                |
| GET    | `/api/Songs/search?query={query}`            | Bearer Token            | Search iTunes songs            |
| POST   | `/api/Songs/playlists/{playlistId}`          | Bearer Token            | Add an iTunes song to playlist |
| DELETE | `/api/Songs/playlists/{playlistId}/{songId}` | Bearer Token            | Remove song from playlist      |

---

# Complete API Testing Flow

A complete manual test can be performed in the following order.

## 1. Register

```http
POST /api/Auth/register
Content-Type: application/json

{
  "email": "test@example.com",
  "password": "Password@123"
}
```

Copy the returned `accessToken`.

---

## 2. Login

```http
POST /api/Auth/login
Content-Type: application/json

{
  "email": "test@example.com",
  "password": "Password@123"
}
```

Copy the returned JWT.

---

## 3. Create a Playlist

Send the JWT:

```http
Authorization → Type: Bearer Token → Token: <accessToken>
```

Then:

```http
POST /api/Playlists
Content-Type: application/json

{
  "name": "My Favorites",
  "description": "My favorite songs"
}
```

Save the returned `playlistId`.

---

## 4. Get My Playlists

```http
GET /api/Playlists
Authorization → Type: Bearer Token → Token: <accessToken>
```

The newly created playlist should appear.

---

## 5. Get the Playlist

```http
GET /api/Playlists/{playlistId}
Authorization → Type: Bearer Token → Token: <accessToken>
```

Initially, the playlist should contain:

```json
"songs": []
```

---

## 6. Search for a Song

```http
GET /api/Songs/search?query=Imagine
Authorization → Type: Bearer Token → Token: <accessToken>
```

Take the `externalId` of one of the returned songs.

---

## 7. Add the Song

```http
POST /api/Songs/playlists/{playlistId}
Authorization → Type: Bearer Token → Token: <accessToken>
Content-Type: application/json

{
  "externalId": 123456789
}
```

The API retrieves the song metadata from iTunes and adds it to the playlist.

---

## 8. Get the Playlist Again

```http
GET /api/Playlists/{playlistId}
Authorization → Type: Bearer Token → Token: <accessToken>
```

The playlist should now contain the song retrieved from iTunes.

---

## 9. Test Duplicate Song Protection

Send the same request again:

```http
POST /api/Songs/playlists/{playlistId}
Authorization → Type: Bearer Token → Token: <accessToken>
Content-Type: application/json

{
  "externalId": 123456789
}
```

The API should return:

```http
409 Conflict
```

because the song is already in the playlist.

---

## 10. Update the Playlist

```http
PUT /api/Playlists/{playlistId}
Authorization → Type: Bearer Token → Token: <accessToken>
Content-Type: application/json

{
  "name": "Updated Favorites",
  "description": "Updated playlist description"
}
```

Verify the changes with:

```http
GET /api/Playlists/{playlistId}
Authorization → Type: Bearer Token → Token: <accessToken>
```

---

## 11. Remove the Song

Use the internal `songId` returned in the playlist response:

```http
DELETE /api/Songs/playlists/{playlistId}/{songId}
Authorization → Type: Bearer Token → Token: <accessToken>
```

The response should be:

```http
204 No Content
```

---

## 12. Delete the Playlist

```http
DELETE /api/Playlists/{playlistId}
Authorization → Type: Bearer Token → Token: <accessToken>
```

The response should be:

```http
204 No Content
```

---

# Validation and Error Handling

The API validates incoming requests and provides consistent error responses.

Examples of handled errors include:

| Status                      | Meaning                                      |
| --------------------------- | -------------------------------------------- |
| `200 OK`                    | Request completed successfully               |
| `400 Bad Request`           | Invalid request or validation failure        |
| `401 Unauthorized`          | Missing or invalid authentication            |
| `404 Not Found`             | Resource does not exist or is not accessible |
| `409 Conflict`              | Operation conflicts with existing data       |
| `500 Internal Server Error` | Unexpected server error                      |

Application-specific exceptions are handled centrally by:

```text
ExceptionHandlingMiddleware
```

instead of duplicating exception handling code across controllers.

---

# iTunes Integration

The application uses the **iTunes Search API** as its external music provider.

It supports:

* Searching for songs
* Looking up a song by its iTunes ID
* Retrieving song metadata

The application stores selected metadata locally:

* iTunes ID
* Title
* Artist
* Album
* Duration
* Artwork URL
* iTunes URL

The audio itself is **not stored by the application**.


---

# Documentation

Additional documentation is available in:

* [`docs/ARCHITECTURE.md`](docs/ARCHITECTURE.md) — architecture, project responsibilities, dependencies, request flows, and design decisions.
* [`docs/DATABASE.md`](docs/DATABASE.md) — database selection, tables, relationships, constraints, and delete behavior.

---

# Design Principles

The project intentionally uses patterns that provide value for its current size:

* **Clean Architecture** for separation of concerns
* **Service Layer** for application/business logic
* **Repository Pattern** for database abstraction
* **DTOs** to separate API contracts from domain entities
* **Dependency Injection** for loose coupling
* **Global Exception Handling** for consistent error responses

