# Deploy to Render

This app is Render-ready. The flow: push to GitHub → create a Render Blueprint from the repo → set 3 SMTP secrets → done.

---

## What's already wired up

- **`Dockerfile`** — multi-stage .NET 10 SDK → ASP.NET runtime image.
- **`render.yaml`** — Render Blueprint that provisions:
  - A free PostgreSQL database (`customer-manager-db`)
  - A free Docker web service (`customer-manager`)
  - Automatically wires `DATABASE_URL` from the DB into the web service
- **`Program.cs`** — reads `DATABASE_URL` (Render's convention) and converts it to an Npgsql connection string with SSL, and binds to `$PORT`.
- **`.dockerignore`** keeps `bin/`, `obj/`, `appsettings.Development.json`, and the design docs out of the image.
- **`.gitignore`** keeps the dev SMTP password and build output out of source control.

---

## One-time setup

### 1. Push the project to GitHub

The project isn't a git repo yet. From a PowerShell in `C:\blazor_project`:

```powershell
git init
git add .
git commit -m "Initial commit: Blazor customer manager with Identity + Mapster"
git branch -M main
# Create an empty repo on GitHub (https://github.com/new), then:
git remote add origin https://github.com/<your-username>/<repo-name>.git
git push -u origin main
```

> The Gmail App Password lives in `appsettings.Development.json` which is `.gitignore`d, so it won't get pushed. ✅

### 2. Create the Render Blueprint

1. Sign in at <https://dashboard.render.com>
2. Click **New +** → **Blueprint**
3. Connect your GitHub account (first time only) and pick the repo
4. Render reads `render.yaml`, shows a preview of the database + web service, click **Apply**
5. Wait 3–5 min for the first build (subsequent commits auto-deploy in ~1–2 min)

### 3. Add the SMTP secrets

After the first deploy completes:

1. In the Render dashboard, open the **customer-manager** service
2. Go to **Environment**
3. Fill in the three placeholders (they were declared with `sync: false` so Render didn't generate them):
   - `Smtp__Username` → `ljupco.semov@gmail.com`
   - `Smtp__Password` → your 16-char Gmail App Password (`ctlrogjddkzllwfh`)
   - `Smtp__FromAddress` → `ljupco.semov@gmail.com`
4. Click **Save Changes** — Render restarts the service automatically

> Note the **double underscore** in env var names — ASP.NET Core maps `Smtp__Username` to the `Smtp:Username` config key.

### 4. Done

Open the URL Render gives you (something like `https://customer-manager-xxxx.onrender.com`). Register an account, click the link in your email, sign in, manage customers.

---

## Free tier caveats

- **Sleep after 15 min of inactivity** — the next visit takes ~30s to wake up (cold start). Stays warm if used regularly.
- **750 hours/month** of compute — plenty for one always-on free service.
- **Free Postgres expires after 30 days** — Render emails you; you have to manually re-create it (data is lost). Upgrading to a paid DB ($7/mo) removes the limit.
- **Render terminates TLS at its edge proxy** — the app speaks plain HTTP internally, which is why `UseHttpsRedirection` is skipped when `$PORT` is set.

---

## Local development (after the swap)

The app now uses Postgres locally too. Two options:

### Option A: Docker Compose (recommended)

```powershell
docker compose up -d postgres
dotnet run
```

Postgres listens on `localhost:5432`, credentials in `appsettings.json` (user: `postgres`, password: `postgres`, db: `customermanagement`).

### Option B: Native install

Install Postgres 16 from <https://www.postgresql.org/download/windows/>, create a db named `customermanagement` with user `postgres` / password `postgres`, then `dotnet run`.

The schema is created automatically by `EnsureCreated()` on first startup.

---

## Updating the deployment

Every `git push` to `main` triggers an auto-deploy. Watch the build in the **Logs** tab on Render. If a build fails, check that you haven't introduced anything platform-specific (e.g. local file paths, Windows-only APIs).
