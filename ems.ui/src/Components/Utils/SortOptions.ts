export const sortOptionsUsers = [
  { label: "None", value: null },
  { label: "Date ↑ (Oldest first)", value: "createdate_asc" },
  { label: "Date ↓ (Newest first)", value: "createdate_desc" },
];

export const sortOptionsEmployees = [
  { label: "None", value: null },
  { label: "Salary ↑ (Lowest first)", value: "salary_asc" },
  { label: "Salary ↓ (Highest first)", value: "salary_desc" },
  { label: "Date Of Birth ↑ (Oldest first)", value: "birthDate_asc" },
  { label: "Date Of Birth ↓ (Youngest first)", value: "birthDate_desc" },
  { label: "Employment Date ↑ (Oldest first)", value: "employmentDate_asc" },
  { label: "Employment Date ↓ (Newest first)", value: "employmentDate_desc" },
  {
    label: "Medical Check ↑ (Oldest first)",
    value: "medicalCheckValidUntil_asc",
  },
  {
    label: "Medical Check ↓ (Newest first)",
    value: "medicalCheckValidUntil_desc",
  },
];

export const sortOptionsTasks = [
  { label: "None", value: null },
  { label: "Start Date ↑ (Oldest first)", value: "start_asc" },
  { label: "Start Date ↓ (Newest first)", value: "start_desc" },
  { label: "End Date ↑ (Oldest first)", value: "end_asc" },
  { label: "End Date ↓ (Newest first)", value: "end_desc" },
];

export const sortOptionsLogs = [
  { label: "None", value: null },
  { label: "Date ↑ (Oldest first)", value: "createdate_asc" },
  { label: "Date ↓ (Newest first)", value: "createdate_desc" },
];

export const sortOptionsVehicles = [
  { label: "None", value: null },
  { label: "Mileage ↑ (Lowest first)", value: "mileage_asc" },
  { label: "Mileage ↓ (Highest first)", value: "mileage_desc" },
  { label: "Date Production ↑ (Oldest first)", value: "date_asc" },
  { label: "Date Production ↓ (Newest first)", value: "date_desc" },
  { label: "OC Validity ↑ (Oldest first)", value: "insurance_oc_asc" },
  { label: "OC Validity ↓ (Newest first)", value: "insurance_oc_desc" },
  { label: "Technical Inspection ↑ (Oldest first)", value: "inspection_asc" },
  {
    label: "Technical Inspection ↓ (Newest first)",
    value: "inspection_desc",
  },
  { label: "Insurance Cost ↑ (Lowest first)", value: "insurance_cost_asc" },
  { label: "Insurance Cost ↓ (Highest first)", value: "insurance_cost_desc" },
];

export const sortOptionsReservations = [
  { label: "None", value: null },
  { label: "Check-In ↑ (Earliest first)", value: "start_asc" },
  { label: "Check-In ↓ (Latest first)", value: "start_desc" },
  { label: "Check-Out ↑ (Earliest first)", value: "end_asc" },
  { label: "Check-Out ↓ (Latest first)", value: "end_desc" },
];

export const sortOptionsTransactions = [
  { label: "None", value: null },
  { label: "Date ↑ (Oldest first)", value: "createdate_asc" },
  { label: "Date ↓ (Newest first)", value: "createdate_desc" },
  { label: "Amount ↑ (Lowest first)", value: "amount_asc" },
  { label: "Amount ↓ (Highest first)", value: "amount_desc" },
];
