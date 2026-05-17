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

### 3. Set up SendGrid (for email — Render's free tier blocks outbound SMTP)

Render free tier blocks ports 25/465/587 to prevent spam abuse, so Gmail SMTP doesn't work. SendGrid's HTTP API does (it's plain HTTPS).

1. Sign up free at <https://signup.sendgrid.com> (no credit card, 100 emails/day free)
2. **Verify your sender email**:
   - Dashboard → **Settings** → **Sender Authentication** → **Verify a Single Sender**
   - Use `ljupco.semov@gmail.com` (or another address you control). SendGrid emails you a confirmation link — click it.
3. **Create an API key**:
   - **Settings** → **API Keys** → **Create API Key**
   - Name it anything (e.g. "customer-manager"), choose **Restricted Access** → enable **Mail Send: Full Access** → **Create & View**
   - Copy the key (starts with `SG.`). You only see it once.
4. **Add the secrets to Render**:
   - In the Render dashboard, open the **customer-manager** service → **Environment**
   - Fill in the three placeholders (marked `sync: false` in render.yaml so Render didn't generate them):
     - `SendGrid__ApiKey` → the `SG.xxxxx...` key
     - `SendGrid__FromAddress` → the verified sender email (e.g. `ljupco.semov@gmail.com`)
     - `SendGrid__FromName` → `Customer Manager`
   - **Save Changes** — Render auto-restarts the service.

> Note the **double underscore** in env var names — ASP.NET Core maps `SendGrid__ApiKey` to the `SendGrid:ApiKey` config key.

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
