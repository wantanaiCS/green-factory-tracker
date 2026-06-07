# 🌿 Green Factory — Carbon & Energy Tracker

ระบบติดตามการใช้พลังงานและการปล่อยคาร์บอนสำหรับโรงงานอุตสาหกรรม  
รองรับมาตรฐาน ESG และการรายงาน Net Zero

---

## 🛠️ Tech Stack

| Layer | Technology |
|---|---|
| Language | C# (.NET 10) |
| Frontend | Blazor Server |
| UI Library | MudBlazor |
| ORM | Entity Framework Core |
| Database | SQLite |

---

## ✨ Features

- 📊 **Dashboard** — แสดงสรุปพลังงาน น้ำ และ Carbon วันนี้แบบ Real-time
- ✏️ **บันทึกข้อมูล** — กรอก kWh / น้ำ / เลือก Line พร้อมคำนวณ Carbon อัตโนมัติ
- 📋 **KPI Status** — เปรียบเทียบค่าจริงกับ Target และ Threshold แต่ละ Line
- 🧮 **Carbon Calculation** — คำนวณตามมาตรฐาน EGAT และการประปา

---

## ⚡ Carbon Emission Factors

| ประเภท | Factor |
|---|---|
| ไฟฟ้า (Scope 2) | 0.4999 kgCO₂e / kWh |
| น้ำประปา (Scope 3) | 0.708 kgCO₂e / ลบ.ม. |

---

## 🚀 Getting Started

```bash
# Clone โปรเจค
git clone https://github.com/wantanaiCS/green-factory-tracker.git
cd green-factory-tracker/GreenFactory

# Run
dotnet run
```

เปิด browser ที่ `http://localhost:5258`

---

## 📁 Project Structure

```
GreenFactory/
├── Components/
│   ├── Pages/
│   │   ├── Home.razor        # Dashboard
│   │   ├── DataEntry.razor   # บันทึกข้อมูล
│   │   └── Report.razor      # รายงาน (coming soon)
│   └── Layout/
│       ├── MainLayout.razor
│       └── NavMenu.razor
├── Data/
│   ├── AppDbContext.cs
│   └── SeedData.cs
├── Models/
│   ├── EnergyRecord.cs
│   └── KpiTarget.cs
└── Services/
    └── EnergyService.cs
```
