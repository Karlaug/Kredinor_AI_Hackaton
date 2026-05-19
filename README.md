# Ship It Faster Challenge – Deltakerpakke

Velkommen! Denne pakken inneholder alt dere trenger for å løse oppgaven.

## Innhold

```
deltakerpakke/
├── README.md                          ← du leser den
└── src/                               ← C#/.NET kodebase
    ├── README.md
    ├── Program.cs
    ├── TasksController.cs
    ├── TaskService.cs
    ├── NotificationService.cs
    ├── AppDbContext.cs
    └── Models.cs
```

## Slik importerer dere backloggen til Azure DevOps

1. Gå til Boards → Queries → New Query
2. Klikk "Import work items" (eller bruk **Backlogs** → **Import**)
3. Velg `backlog\\\_azure\\\_devops.csv`
4. Bekreft mapping (kolonnene er allerede navngitt korrekt)
5. Importer

Hvis dere ikke har et tomt prosjekt å importere til, lag en ny iterasjon eller bruk en sandkasse-org.

**Alternativ:** Hvis Azure DevOps-import krangler, bruk CSV-filen direkte i den AI-assistenten dere jobber med – CoPilot leser CSV utmerket og kan groome den der.

## Slik kjører dere koden

```
cd src
dotnet restore
dotnet run
```

Swagger-UI tilgjengelig på `https://localhost:5001/swagger`.

## Hva dere skal levere

Se oppstartsbriefen. Minst 3 av 5 leveranser:

1. Backlog-grooming (kjør hele backloggen gjennom AI)
2. Epic-dekomponering (ta epic-en "Make the API production ready")
3. Spec → kode (en faktisk PR på én av ticketene)
4. Code review-agent
5. Dokumentasjon på autopilot

## Lykke til!

