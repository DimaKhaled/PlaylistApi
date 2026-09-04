# Database Documentation

## 1. Database Selection

### Why SQL?

SQL was selected because the application's data has a **structured and stable schema** with well-defined relationships:

- A user can have multiple playlists (**one-to-many**).
- A playlist can contain multiple songs.
- A song can belong to multiple playlists (**many-to-many**).

These relationships are naturally represented using relational tables, foreign keys, and a junction table.

NoSQL was also considered. Two approaches were evaluated:

1. **Embedding songs inside playlists:** This would duplicate song metadata when the same song belongs to multiple playlists, increasing storage and making updates more difficult.
2. **Referencing songs from playlists:** This avoids duplication but requires operations such as `$lookup` to reconstruct relationships and does not provide the same relational constraints and referential integrity as a relational database.

Therefore, SQL was considered a better fit for the application's structured data and relationships.

### Why SQL Server?

SQL Server was selected over PostgreSQL and MySQL because:

- It provides all the relational features required by the application.
- It integrates well with **ASP.NET Core and Entity Framework Core**.
- It works naturally with the .NET technology stack and ASP.NET Core Identity.
- PostgreSQL and MySQL can also satisfy the requirements, but their additional capabilities are not needed for this project.

The choice is therefore based on the **fit with the application's requirements and technology stack**, rather than SQL Server being universally better than other DBMSs.

---

# 2. Database Schema

The database contains the ASP.NET Core Identity tables, with `AspNetUsers` being the relevant Identity table for the application's relationships.

### AspNetUsers

| Column | Type | Constraints |
|---|---|---|
| `Id` | `uniqueidentifier` | Primary Key |

### Playlists

| Column | Type | Constraints |
|---|---|---|
| `Id` | `int` | Primary Key |
| `UserId` | `uniqueidentifier` | Required, FK → `AspNetUsers.Id` |
| `Name` | `nvarchar(100)` | Required |
| `Description` | `nvarchar(500)` | Nullable |
| `CreatedAt` | `datetime2` | Required |

### Songs

| Column | Type | Constraints |
|---|---|---|
| `Id` | `int` | Primary Key |
| `ExternalId` | `bigint` | Required, Unique |
| `Title` | `nvarchar(255)` | Required |
| `ArtistName` | `nvarchar(255)` | Required |
| `AlbumName` | `nvarchar(255)` | Nullable |
| `DurationSeconds` | `int` | Nullable |
| `ArtworkUrl` | `nvarchar(1000)` | Nullable |
| `ExternalUrl` | `nvarchar(1000)` | Nullable |

`Id` is the application's internal identifier, while `ExternalId` is the ID provided by iTunes. The unique `ExternalId` ensures that the same song is stored only once locally.

The `Songs` table stores **song metadata only**, not the actual audio.

### PlaylistSongs

| Column | Type | Constraints |
|---|---|---|
| `PlaylistId` | `int` | Composite PK, FK → `Playlists.Id` |
| `SongId` | `int` | Composite PK, FK → `Songs.Id` |

`PlaylistSongs` is the junction table for the many-to-many relationship between playlists and songs. Its composite primary key `(PlaylistId, SongId)` prevents the same song from being added to the same playlist more than once.

---

# 3. Relationships

```text
AspNetUsers  1 ──────── N  Playlists

Playlists   1 ──────── N  PlaylistSongs  N ──────── 1  Songs
```

Therefore:

- **User → Playlists:** One-to-many.
- **Playlists ↔ Songs:** Many-to-many through `PlaylistSongs`.

---

# 4. Delete Behavior

- Deleting a **user** deletes their playlists.
- Deleting a **playlist** deletes its `PlaylistSongs` records.
- Deleting a playlist **does not delete its songs**, because a song can be shared by multiple playlists.

```text
Delete User
    ↓
Delete Playlists
    ↓
Delete PlaylistSongs
    ↓
Keep Songs
```

---

# 5. Indexes

| Index | Purpose |
|---|---|
| `Playlists.UserId` | Improves retrieving playlists belonging to a user |
| Unique `Songs.ExternalId` | Efficiently finds an existing iTunes song and prevents duplicates |
| `PlaylistSongs.SongId` | Improves queries involving a specific song |

The database constraints and indexes are configured through **Entity Framework Core** in the Infrastructure layer.