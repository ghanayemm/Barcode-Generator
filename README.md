# Barcode Generator Desktop Application

## Overview

The **Barcode Generator** is a Windows Forms desktop application developed using **C#**. It is designed to generate unique 13-digit barcodes for products registered in a company's system. The application ensures that each product barcode is unique and follows a structured format where the first two digits represent the abbreviation of the main supplier's name.

The application provides an intuitive interface for managing products, generating barcodes, and exporting data to **Excel** or **PDF** directly from the DataGridView.

---

## Features

- Generate unique 13-digit barcodes for products.
- Automatic encoding of the first two digits based on the main supplier's abbreviation.
- Display all products and their barcodes in a **DataGridView**.
- Export product data to **Excel** (XLSX) or **PDF**.
- User-friendly interface built with **Windows Forms**.
- Prevents duplicate barcodes to ensure data integrity.

---

## Technologies and Libraries Used

- **Programming Language:** C#
- **Framework:** .NET Framework (WinForms)
- **PDF Handling:** iTextSharp — create and manipulate PDF files.
- **Excel Handling:** 
  - ClosedXML — high-level interface for reading/writing Excel files.
  - ClosedXML.Parser — supporting component for parsing Excel.
  - ExcelNumberFormat — helper for number formatting inside Excel.
- **IDE:** Visual Studio

---

## Barcode Format

- **Total Length:** 13 digits
- **First 2 digits:** Supplier abbreviation
- **Remaining 11 digits:** Unique product identifier

---

## Ensuring Unique Barcodes

To guarantee the uniqueness of each barcode:

1. **Supplier Abbreviation:** The first two digits represent the main supplier's abbreviation, grouping barcodes by supplier.  
2. **Unique Number Generation:** The remaining 11 digits are generated sequentially or via the system logic to prevent duplicates.  
3. **Validation:** Before adding a new barcode, the application checks existing barcodes in the system to ensure no repetition.

---

## Usage

1. Open the project in **Visual Studio**.
2. Build and run the solution.
3. Add new products along with their main supplier.
4. Generate a unique barcode for each product.
5. View all products and their barcodes in the DataGridView.
6. Export the data to Excel or PDF as needed.

---

## Contributing

Contributions are welcome! You can improve features, fix bugs, or add new export formats. Fork the repository and create a pull request.

---

## License

This project is open-source and free to use under the MIT License.

