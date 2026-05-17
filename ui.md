# UI Design Guide - Customer Management System

## Overview
This document provides comprehensive UI/UX guidelines for the customer management system built with Blazor, including design patterns, components, and authorization flow.

---

## Design Principles

### 1. **User-Centric Design**
- Intuitive navigation and clear workflows
- Minimize clicks to complete tasks
- Consistent terminology and UI patterns
- Accessible to all users (WCAG 2.1 AA)

### 2. **Visual Consistency**
- Unified color scheme and typography
- Consistent spacing and sizing
- Standardized component styles
- Predictable interactions

### 3. **Responsive Design**
- Mobile-first approach
- Fluid layouts that adapt to all screen sizes
- Touch-friendly interactions on mobile
- Desktop optimization for productivity

### 4. **Performance-Focused**
- Fast loading times
- Smooth interactions
- Lazy loading where appropriate
- Optimized assets

---

## Color Palette

### Primary Colors
```
Primary Blue:     #007BFF
Secondary Blue:   #0056B3
Accent Blue:      #0D6EFD

Primary Gray:     #6C757D
Light Gray:       #E9ECEF
Dark Gray:        #212529
```

### Semantic Colors
```
Success:          #28A745
Warning:          #FFC107
Danger:           #DC3545
Info:             #17A2B8
Light:            #F8F9FA
Dark:             #343A40
```

### State Colors
```
Hover:            Darker shade (20% darker)
Active:           Primary color
Disabled:         Light gray (disabled state)
Focus:            Blue outline (#80BDFF)
```

---

## Typography

### Font Stack
```css
Font Family:      -apple-system, BlinkMacSystemFont, "Segoe UI", Roboto, "Helvetica Neue", Arial, sans-serif
Fallback:         sans-serif
```

### Font Sizes
```
H1 (Headings):    2.5rem (40px)
H2:               2rem (32px)
H3:               1.75rem (28px)
H4:               1.5rem (24px)
H5:               1.25rem (20px)
H6:               1rem (16px)
Body:             1rem (16px) - default
Small:            0.875rem (14px)
Tiny:             0.75rem (12px)
```

### Font Weights
```
Light:            300
Regular:          400
Semibold:         600
Bold:             700
```

---

## Spacing System

### Consistent Spacing Scale
```
xs:  4px   (0.25rem)
sm:  8px   (0.5rem)
md:  16px  (1rem)
lg:  24px  (1.5rem)
xl:  32px  (2rem)
2xl: 48px  (3rem)
3xl: 64px  (4rem)
```

### Application
- Margins: Use spacing scale
- Padding: Use spacing scale
- Gaps: Use spacing scale between grid items

---

## Component Library

### 1. **Navigation Bar**
```
┌──────────────────────────────────────────────────────┐
│  Logo    Home  Customers  Reports  Settings   User ▼ │
│          [Dashboard]                           Profile │
│                                                Logout  │
└──────────────────────────────────────────────────────┘
```

**Features:**
- Logo/brand on left
- Main navigation links
- User menu on right
- Active state highlighting
- Mobile hamburger menu
- Breadcrumb navigation below on detail pages

### 2. **Sidebar Navigation (Optional)**
```
┌────────────┐
│ ≡ Menu     │
├────────────┤
│ Dashboard  │
│ Customers  │
│ ├─ List    │
│ ├─ Add     │
│ └─ Import  │
│ Reports    │
│ Settings   │
│ Help       │
└────────────┘
```

### 3. **Search Bar Component**
```
┌─────────────────────────────────────────┐
│ 🔍 Search customers...             [×]  │
│                                         │
│ Advanced Filters ▼                      │
│ ┌───────────────────────────────────┐   │
│ │ Status: [Active ▼]                │   │
│ │ City:   [________]                │   │
│ │ From:   [Date Picker]             │   │
│ │ [Clear Filters] [Search]          │   │
│ └───────────────────────────────────┘   │
└─────────────────────────────────────────┘
```

**Features:**
- Real-time search with debouncing
- Advanced filter toggle
- Multi-field search
- Clear all filters button
- Search suggestions (optional)

### 4. **Data Table**
```
┌─────────────────────────────────────────────────────────┐
│ Name ▲  │ Email            │ City      │ Status │ Actions │
├─────────────────────────────────────────────────────────┤
│ John D. │ john@example.com │ New York  │ Active │ [✎] [🗑] │
│ Jane S. │ jane@example.com │ Boston    │ Active │ [✎] [🗑] │
│ Bob M.  │ bob@example.com  │ Chicago   │ Inactive│ [✎] [🗑] │
└─────────────────────────────────────────────────────────┘

Showing 1 to 10 of 42 results
[< Previous] [1] [2] [3] [4] [5] [Next >]
```

**Features:**
- Sortable columns (click header)
- Alternate row colors
- Hover effects on rows
- Inline actions (edit, delete, view)
- Pagination controls
- Empty state message
- Loading skeleton
- Checkbox for bulk selection (optional)

### 5. **Form Component**
```
┌──────────────────────────────────────┐
│ New Customer                  [×]    │
├──────────────────────────────────────┤
│                                      │
│ First Name *                         │
│ [_________________________] ✓         │
│                                      │
│ Last Name *                          │
│ [_________________________]           │
│                                      │
│ Email *                              │
│ [_________________________] ✗         │
│ Invalid email format                 │
│                                      │
│ Phone                                │
│ [_________________________]           │
│                                      │
│ Address                              │
│ [_________________________]           │
│                                      │
│ City                                 │
│ [_________________________]           │
│                                      │
│ State/Province                       │
│ [_________________________]           │
│                                      │
│ Postal Code                          │
│ [_________________________]           │
│                                      │
│ Country                              │
│ [__________ ▼]                       │
│                                      │
│ Status                               │
│ ○ Active  ○ Inactive  ○ Archived    │
│                                      │
│          [Cancel]  [Save]            │
└──────────────────────────────────────┘
```

**Features:**
- Clear field labels
- Required field indicators (*)
- Real-time validation feedback
- Input field status icons (✓, ✗)
- Error messages below fields
- Helper text for complex fields
- Submit and cancel buttons
- Disabled state while saving
- Loading spinner on submit

### 6. **Pagination Component**
```
Showing 11 to 20 of 427 results

[< Previous] [1] [2] [3] [4] [5] ... [42] [43] [Next >]

Items per page: [10 ▼]
```

**Features:**
- Previous/Next buttons
- Page numbers
- Jump to page
- Results info text
- Items per page selector
- Disabled state at boundaries

### 7. **Notification/Toast**
```
Top-right (stacking):

┌─────────────────────────────┐
│ ✓ Customer saved successfully│  (2s, auto-dismiss)
└─────────────────────────────┘

┌──────────────────────────────┐
│ ✗ Error: Email already exists│  (sticky, manual dismiss)
│                         [×]  │
└──────────────────────────────┘
```

**Types:**
- Success (green, 2s auto-dismiss)
- Error (red, sticky)
- Warning (orange, sticky)
- Info (blue, 4s auto-dismiss)

**Position:** Top-right (customizable)

### 8. **Modal/Dialog**
```
┌────────────────────────────────────────┐
│ Delete Customer                    [×] │
├────────────────────────────────────────┤
│                                        │
│ Are you sure you want to delete        │
│ "John Doe"? This action cannot be      │
│ undone.                                │
│                                        │
│                [Cancel]  [Delete]      │
└────────────────────────────────────────┘
```

**Features:**
- Modal overlay (prevents background interaction)
- Close button
- Clear title and message
- Confirmation buttons
- Focus trap (keyboard navigation)
- Escape key to close
- Scrollable for long content

### 9. **Buttons**
```
Primary Button:
┌─────────────────┐
│  Save Changes   │ (Background: #007BFF, Text: white)
└─────────────────┘

Secondary Button:
┌─────────────────┐
│     Cancel      │ (Background: #E9ECEF, Text: dark)
└─────────────────┘

Danger Button:
┌─────────────────┐
│     Delete      │ (Background: #DC3545, Text: white)
└─────────────────┘

Disabled State:
┌─────────────────┐
│   Processing    │ (Gray background, opacity 0.6)
└─────────────────┘
```

**States:**
- Normal
- Hover (darker color)
- Active/Pressed
- Disabled (grayed out, no interaction)
- Loading (spinner inside)

### 10. **Loading States**
```
Full Page Loading:
┌───────────────────────────┐
│                           │
│        Loading...         │
│         [Spinner]         │
│                           │
└───────────────────────────┘

Skeleton Loader (Table):
┌─────────────────────────────────┐
│ ▓▓▓▓ │ ▓▓▓▓▓▓ │ ▓▓▓▓  │ ▓▓▓▓  │
│ ▓▓▓▓ │ ▓▓▓▓▓▓ │ ▓▓▓▓  │ ▓▓▓▓  │
│ ▓▓▓▓ │ ▓▓▓▓▓▓ │ ▓▓▓▓  │ ▓▓▓▓  │
└─────────────────────────────────┘

Inline Loading:
[Loading...] [×]
```

---

## Pages Layout

### 1. **Dashboard Page**
```
┌─────────────────────────────────────────────────────┐
│ Dashboard                                           │
├─────────────────────────────────────────────────────┤
│                                                     │
│ ┌──────────┐  ┌──────────┐  ┌──────────┐          │
│ │ Total    │  │ Active   │  │ Inactive │          │
│ │Customers │  │ Customers│  │Customers │          │
│ │   1,234  │  │    890   │  │   344    │          │
│ └──────────┘  └──────────┘  └──────────┘          │
│                                                     │
│ Recent Activity                                     │
│ ┌─────────────────────────────────────────────────┐ │
│ │ [Chart/Graph showing trends]                    │ │
│ └─────────────────────────────────────────────────┘ │
│                                                     │
│ Recent Customers                                    │
│ ┌─────────────────────────────────────────────────┐ │
│ │ Name          │ Email           │ Date        │ │
│ │ John Doe      │ john@example... │ Today      │ │
│ │ Jane Smith    │ jane@example... │ Yesterday  │ │
│ └─────────────────────────────────────────────────┘ │
└─────────────────────────────────────────────────────┘
```

### 2. **Customer List Page**
```
┌──────────────────────────────────────────────────────┐
│ Customers                          [+ New Customer] │
├──────────────────────────────────────────────────────┤
│                                                      │
│ 🔍 Search customers... [Advanced ▼]                 │
│                                                      │
│ Showing 1 to 10 of 42                               │
│ ┌──────────────────────────────────────────────────┐│
│ │Name │ Email  │ Phone │ City │ Status │ Actions │ │
│ ├──────────────────────────────────────────────────┤│
│ │John │john@.. │555-01│ NYC  │ Active │ [✎] [🗑]│ │
│ │Jane │jane@.. │555-02│ BOS  │ Active │ [✎] [🗑]│ │
│ └──────────────────────────────────────────────────┘│
│                                                      │
│ [< Previous] [1] [2] [3] [Next >]                  │
└──────────────────────────────────────────────────────┘
```

### 3. **Customer Detail Page**
```
┌──────────────────────────────────────────────────────┐
│ < Back to List                    [Edit] [Delete]    │
├──────────────────────────────────────────────────────┤
│                                                      │
│ John Doe                                            │
│                                                      │
│ Email:        john@example.com                      │
│ Phone:        (555) 012-3456                        │
│ Address:      123 Main Street                       │
│ City:         New York                              │
│ State:        NY                                    │
│ Postal Code:  10001                                 │
│ Country:      United States                         │
│ Status:       Active                                │
│                                                      │
│ Created:      Jan 15, 2024                          │
│ Last Updated: May 17, 2026                          │
│                                                      │
│ Notes:                                              │
│ Premium customer since 2020                         │
│                                                      │
└──────────────────────────────────────────────────────┘
```

### 4. **Customer Form Page (Create/Edit)**
```
┌──────────────────────────────────────────────────────┐
│ New Customer                                   [×]   │
├──────────────────────────────────────────────────────┤
│                                                      │
│ [Tabs: Basic Info | Contact Info | Preferences]    │
│                                                      │
│ Basic Information                                   │
│                                                      │
│ First Name *                                        │
│ [_____________________________]                      │
│                                                      │
│ Last Name *                                         │
│ [_____________________________]                      │
│                                                      │
│ Email *                                             │
│ [_____________________________]                      │
│ Invalid email format                                │
│                                                      │
│ Status                                              │
│ ○ Active  ○ Inactive  ○ Archived                   │
│                                                      │
│                      [Cancel]  [Save]               │
└──────────────────────────────────────────────────────┘
```

---

## Authorization & Authentication Pages

### Authorization Page Flow
```
User Access Request
       ↓
┌──────────────────────┐
│ Is User Logged In?   │
└──────────────────────┘
     Yes ↓          ↓ No
  Verify Role   ┌─────────────┐
     ↓          │ Redirect to │
  Has Access?   │   Login     │
    Yes ↓  ↓ No │   Page      │
  Allow  Deny    └─────────────┘
     ↓    ↓
  Proceed Deny
```

### 1. **Login Page**
```
┌─────────────────────────────────────┐
│                                     │
│          Customer Manager           │
│                                     │
│          Welcome Back               │
│                                     │
│ Email or Username *                 │
│ [________________________________]  │
│                                     │
│ Password *                          │
│ [________________________________]  │
│ [✓] Show Password                   │
│                                     │
│ [✓] Remember me                     │
│                                     │
│ [Sign In]                           │
│                                     │
│ [Forgot Password?]                  │
│                                     │
│ Don't have an account? [Sign Up]   │
│                                     │
│ Or continue with:                   │
│ [Google Logo] [Microsoft Logo]      │
│                                     │
└─────────────────────────────────────┘
```

**Features:**
- Email/Username field
- Password field with show/hide toggle
- Remember me checkbox
- Sign in button (disabled while loading)
- Forgot password link
- Sign up link
- Social login options (optional)
- Error messages for failed attempts
- Loading state with spinner

### 2. **Login Component (Razor)**
```csharp
@page "/login"
@layout EmptyLayout
@inject NavigationManager Navigation
@inject IAuthService AuthService
@inject INotificationService Notification

<div class="login-container">
    <div class="login-card">
        <div class="text-center mb-4">
            <h1 class="h3 mb-3">Customer Manager</h1>
            <p class="text-muted">Welcome Back</p>
        </div>

        <EditForm Model="LoginRequest" OnValidSubmit="HandleLogin">
            <DataAnnotationsValidator />

            <div class="mb-3">
                <label for="email" class="form-label">Email or Username *</label>
                <InputText 
                    id="email" 
                    class="form-control" 
                    @bind-Value="LoginRequest.Email"
                    placeholder="Enter your email"
                    disabled="@IsLoading"
                />
                <ValidationMessage For="() => LoginRequest.Email" />
            </div>

            <div class="mb-3">
                <label for="password" class="form-label">Password *</label>
                <div class="input-group">
                    <InputText 
                        id="password" 
                        type="@(ShowPassword ? "text" : "password")"
                        class="form-control" 
                        @bind-Value="LoginRequest.Password"
                        placeholder="Enter your password"
                        disabled="@IsLoading"
                    />
                    <button 
                        class="btn btn-outline-secondary" 
                        type="button"
                        @onclick="TogglePasswordVisibility">
                        @(ShowPassword ? "Hide" : "Show")
                    </button>
                </div>
                <ValidationMessage For="() => LoginRequest.Password" />
            </div>

            <div class="mb-3 form-check">
                <InputCheckbox 
                    id="rememberMe" 
                    class="form-check-input" 
                    @bind-Value="LoginRequest.RememberMe"
                />
                <label class="form-check-label" for="rememberMe">
                    Remember me
                </label>
            </div>

            <button 
                type="submit" 
                class="btn btn-primary w-100"
                disabled="@IsLoading">
                @if (IsLoading)
                {
                    <span class="spinner-border spinner-border-sm me-2"></span>
                    <span>Signing in...</span>
                }
                else
                {
                    <span>Sign In</span>
                }
            </button>
        </EditForm>

        <div class="text-center mt-3">
            <a href="/forgot-password" class="text-decoration-none">
                Forgot Password?
            </a>
        </div>

        <hr class="my-4" />

        <div class="text-center">
            <p class="text-muted">
                Don't have an account? 
                <a href="/register" class="text-decoration-none fw-bold">
                    Sign Up
                </a>
            </p>
        </div>

        <div class="mt-4 pt-3 border-top">
            <p class="text-center text-muted small mb-2">Or continue with:</p>
            <div class="d-flex gap-2 justify-content-center">
                <button class="btn btn-outline-secondary flex-grow-1" type="button">
                    Google
                </button>
                <button class="btn btn-outline-secondary flex-grow-1" type="button">
                    Microsoft
                </button>
            </div>
        </div>
    </div>
</div>

@code {
    private LoginRequest LoginRequest = new();
    private bool ShowPassword = false;
    private bool IsLoading = false;

    private void TogglePasswordVisibility()
    {
        ShowPassword = !ShowPassword;
    }

    private async Task HandleLogin()
    {
        IsLoading = true;
        try
        {
            var result = await AuthService.LoginAsync(LoginRequest);
            if (result.Success)
            {
                Notification.ShowSuccess("Login successful!");
                Navigation.NavigateTo("/dashboard");
            }
            else
            {
                Notification.ShowError(result.Message);
            }
        }
        catch (Exception ex)
        {
            Notification.ShowError("An error occurred during login");
        }
        finally
        {
            IsLoading = false;
        }
    }
}
```

### 3. **Login Styles (CSS)**
```css
/* Styles for login page */
.login-container {
    display: flex;
    justify-content: center;
    align-items: center;
    min-height: 100vh;
    background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
    padding: 20px;
}

.login-card {
    background: white;
    border-radius: 8px;
    box-shadow: 0 4px 20px rgba(0, 0, 0, 0.15);
    padding: 40px;
    width: 100%;
    max-width: 400px;
}

.login-card .form-label {
    font-weight: 600;
    color: #212529;
    margin-bottom: 8px;
}

.login-card .form-control {
    border: 1px solid #e9ecef;
    padding: 10px 12px;
    font-size: 16px;
    border-radius: 4px;
    transition: border-color 0.3s, box-shadow 0.3s;
}

.login-card .form-control:focus {
    border-color: #007bff;
    box-shadow: 0 0 0 3px rgba(0, 123, 255, 0.25);
}

.login-card .btn-primary {
    padding: 10px 16px;
    font-size: 16px;
    font-weight: 600;
    border-radius: 4px;
}

.login-card .btn-primary:disabled {
    opacity: 0.6;
    cursor: not-allowed;
}

.login-card .text-muted {
    color: #6c757d;
}

/* Responsive */
@media (max-width: 576px) {
    .login-card {
        padding: 24px;
    }
}
```

### 4. **Forgot Password Page**
```
┌─────────────────────────────────────┐
│                                     │
│       Customer Manager              │
│                                     │
│       Forgot Password?              │
│                                     │
│ Enter your email address and we'll  │
│ send you a link to reset your       │
│ password.                           │
│                                     │
│ Email Address *                     │
│ [________________________________]  │
│                                     │
│ [Reset Password]                    │
│                                     │
│ [Back to Login]                     │
│                                     │
└─────────────────────────────────────┘
```

### 5. **Reset Password Page**
```
┌─────────────────────────────────────┐
│                                     │
│       Customer Manager              │
│                                     │
│       Reset Password                │
│                                     │
│ New Password *                      │
│ [________________________________]  │
│ Password strength: Strong ▓▓▓        │
│                                     │
│ Confirm Password *                  │
│ [________________________________]  │
│                                     │
│ [Reset Password]                    │
│                                     │
│ [Back to Login]                     │
│                                     │
└─────────────────────────────────────┘
```

### 6. **Register Page**
```
┌─────────────────────────────────────┐
│                                     │
│       Create Account                │
│                                     │
│ First Name *                        │
│ [________________________________]  │
│                                     │
│ Last Name *                         │
│ [________________________________]  │
│                                     │
│ Email *                             │
│ [________________________________]  │
│                                     │
│ Password *                          │
│ [________________________________]  │
│ Password strength: Strong ▓▓▓        │
│                                     │
│ Confirm Password *                  │
│ [________________________________]  │
│                                     │
│ [✓] I agree to Terms & Conditions  │
│                                     │
│ [Create Account]                    │
│                                     │
│ Already have an account?            │
│ [Sign In]                           │
│                                     │
└─────────────────────────────────────┘
```

---

## Authorization Levels

### Role-Based Access Control (RBAC)

#### Admin
- ✅ View all customers
- ✅ Create customers
- ✅ Edit all customers
- ✅ Delete customers
- ✅ Export data
- ✅ View reports
- ✅ Manage users
- ✅ Access settings

#### Manager
- ✅ View customers (assigned territory)
- ✅ Create customers
- ✅ Edit customers (assigned territory)
- ✅ View reports
- ❌ Delete customers
- ❌ Manage users

#### User
- ✅ View customers (read-only)
- ❌ Create customers
- ❌ Edit customers
- ❌ Delete customers
- ❌ Export data
- ❌ View reports

### Protected Page Pattern
```csharp
@page "/customers"
@attribute [Authorize(Roles = "Admin,Manager")]
@inject NavigationManager Navigation
@inject AuthenticationStateProvider AuthStateProvider

@if (IsAuthorized)
{
    <CustomerList />
}
else
{
    <p>You do not have permission to access this page.</p>
    <a href="/dashboard" class="btn btn-primary">Go to Dashboard</a>
}

@code {
    private bool IsAuthorized = false;

    protected override async Task OnInitializedAsync()
    {
        var authState = await AuthStateProvider.GetAuthenticationStateAsync();
        IsAuthorized = authState.User.Identity.IsAuthenticated;
    }
}
```

---

## Responsive Breakpoints

```
Mobile:        < 576px
Tablet:        576px - 991px
Desktop:       992px - 1199px
Wide Desktop:  ≥ 1200px
```

### Mobile Optimizations
- Stack layout vertically
- Full-width inputs and buttons
- Bottom sheet modals instead of centered
- Touch-friendly button sizes (44x44px minimum)
- Simplified navigation (hamburger menu)

---

## Accessibility Guidelines (WCAG 2.1 AA)

✅ **Keyboard Navigation**
- Tab through all interactive elements
- Escape to close modals/dropdowns
- Enter to submit forms

✅ **Color Contrast**
- Text: 4.5:1 ratio minimum
- UI Components: 3:1 ratio minimum

✅ **Form Labels**
- Every input has associated label
- Required fields marked with *
- Error messages associated with inputs

✅ **Focus Management**
- Visible focus indicator (outline)
- Focus trap in modals
- Focus restored after modal close

✅ **Semantic HTML**
- Proper heading hierarchy (H1, H2, H3)
- Landmark regions (nav, main, footer)
- Proper button/link usage

✅ **ARIA Attributes**
- aria-label for icon buttons
- aria-describedby for error messages
- aria-hidden for decorative elements

---

## UX Patterns & Best Practices

### 1. **Empty States**
```
┌────────────────────────────────┐
│                                │
│        📋 No Customers          │
│                                │
│   You haven't created any       │
│   customers yet.               │
│                                │
│   [+ Create First Customer]    │
│                                │
└────────────────────────────────┘
```

### 2. **Error States**
```
┌────────────────────────────────┐
│ ✗ Error Loading Customers       │
│                                │
│ Something went wrong. Please    │
│ try again.                      │
│                                │
│ [Retry] [Go Back]              │
└────────────────────────────────┘
```

### 3. **Confirmation Pattern**
```
Before destructive action:
1. Show confirmation dialog
2. Clearly state what will happen
3. Require explicit confirmation
4. Provide undo option if possible
```

### 4. **Progressive Disclosure**
```
- Show essential fields first
- Hide advanced options behind toggle
- Use tabs for related information
- Lazy load related data
```

---

## Performance & Animation

### Transitions
- Page transitions: 300ms
- Button hover: 150ms
- Form validation feedback: 300ms
- Fade animations: 200ms

### Micro-interactions
- Button press feedback (50ms)
- Loading spinners (smooth rotation)
- Hover state changes
- Form field focus highlight

---

## Deployment Checklist

- [ ] Test login flow on multiple browsers
- [ ] Verify password reset email delivery
- [ ] Test responsive design on mobile/tablet
- [ ] Run accessibility audit
- [ ] Test keyboard navigation
- [ ] Verify form validation
- [ ] Test error scenarios
- [ ] Check loading states
- [ ] Test with slow network
- [ ] Verify SSL certificate
- [ ] Setup 2FA (if required)
- [ ] Configure email notifications

---

## Resources

- Bootstrap Components: https://getbootstrap.com/docs/5.0/
- Material Design: https://material.io/
- Web Accessibility: https://www.w3.org/WAI/
- WCAG Guidelines: https://www.w3.org/WAI/WCAG21/quickref/

---

**Version**: 1.0  
**Last Updated**: May 17, 2026
