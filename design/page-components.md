# Page Components for Wireframing

## Home (`/`)

2. **Create New Schedule Button**
   - Large, primary button at the top of the main content area
   - Label: "Create New Drug Schedule"

3. **Current Schedule Section**
   - **Current Moment of Day**
   - Text or icon indicating the current time segment (e.g., "Morning", "Afternoon", "Evening", "Night")

4. **Drugs To Take List**
   - For each drug scheduled at the current moment:
     - **Drug Name** (e.g., "Aspirin")
     - **Dosage** (e.g., "2 tablets")
     - **Mark as Taken**: Checkbox or button to mark the drug as taken

---

### Example Layout

```
-------------------------------------------------
| App Logo        Home                Profile   |
-------------------------------------------------
| [Create New Drug Schedule]                    |
-------------------------------------------------
| Morning                                      |
-------------------------------------------------
| [ ] Aspirin      2 tablets   [Mark as taken]  |
| [ ] Vitamin D    1 capsule   [Mark as taken]  |
-------------------------------------------------
```

## Drug Confirmation Page (`/drug-confirmation`)

1. **Confirmation Message**
   - Prominent message indicating the action (e.g., "Confirm Drug Schedule")
   - (Optional) Subtext with additional instructions or information
2. **Drug Details Section (editable)**
   - **Drug Name** (e.g., "Aspirin")
   - **Dosage** (e.g., "2 tablets")
   - **Time of day** (e.g., "Morning", "Morning & Evening")
   - (Optional) Additional notes or instructions

3. **Action Buttons**
   - **Confirm Button**: Large, primary button to confirm adding drug to schedule
   - **Cancel/Back Button**: Secondary button to go back or cancel the action.

---

### Example Layout

```
-------------------------------------------------
|            Confirm Drug Intake                |
-------------------------------------------------
|                                               |
|   [Aspirin]                                   |
|   Dosage: 2 tablets                           |
|   Scheduled Time: Morning                     |
|                                               |
|   Please confirm drug intake data is correct. |
|                                               |
|   [Confirm]   [Cancel]                        |
|                                               |
-------------------------------------------------
```
