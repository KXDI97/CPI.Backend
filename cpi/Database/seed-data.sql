/* ============================================================
   Proyecto CPI – Datos de Prueba (seed-data.sql)
   Empresa importadora colombiana – datos realistas 2023-2026
   ============================================================ */

-- Limpiar datos existentes (orden inverso de FK)
DELETE FROM "InventoryMovements";
DELETE FROM "PurchaseReceiptDetails";
DELETE FROM "PurchaseReceipts";
DELETE FROM "SalesDetails";
DELETE FROM "Sales";
DELETE FROM "Transactions";
DELETE FROM "LogicalCosts";
DELETE FROM "PurchaseOrderDetails";
DELETE FROM "PurchaseOrders";
DELETE FROM "Products";
DELETE FROM "Clients";
DELETE FROM "Suppliers";

-- Resetear secuencias
ALTER SEQUENCE "Suppliers_SupplierId_seq"             RESTART WITH 1;
ALTER SEQUENCE "Clients_ClientId_seq"                  RESTART WITH 1;
ALTER SEQUENCE "PurchaseOrders_PurchaseOrderId_seq"    RESTART WITH 1;
ALTER SEQUENCE "PurchaseOrderDetails_PurchaseOrderDetailId_seq" RESTART WITH 1;
ALTER SEQUENCE "Transactions_Transaction_Number_seq"   RESTART WITH 1;
ALTER SEQUENCE "PurchaseReceipts_ReceiptId_seq"        RESTART WITH 1;
ALTER SEQUENCE "PurchaseReceiptDetails_ReceiptDetailId_seq"     RESTART WITH 1;
ALTER SEQUENCE "Sales_InvoiceId_seq"                   RESTART WITH 1;
ALTER SEQUENCE "SalesDetails_InvoiceDetailId_seq"      RESTART WITH 1;
ALTER SEQUENCE "InventoryMovements_MovementId_seq"     RESTART WITH 1;

-- ============================================================
-- SUPPLIERS (10)
-- ============================================================
INSERT INTO "Suppliers" ("Name", "Contact", "Phone", "Email", "Address") VALUES
  ('Qualiprint International',   'Zhang Wei',         '+86 21 5678 9012', 'sales@qualiprint.cn',      'No. 88 Zhongshan Rd, Shanghai, China'),
  ('Sun Chemical Corporation',   'Michael Thompson',  '+1 201 478 5000',  'info@sunchemical.com',     '35 Waterview Blvd, Parsippany, NJ, USA'),
  ('BASF Colors & Effects GmbH', 'Hans Müller',       '+49 621 60 0',     'colors@basf.com',          'Carl-Bosch-Str. 38, Ludwigshafen, Germany'),
  ('Nazdar Ink Technologies',    'Robert Martinez',   '+1 913 422 1888',  'orders@nazdar.com',        '8501 Hedge Lane Ter, Shawnee, KS, USA'),
  ('INX International Ink Co.',  'Sarah Johnson',     '+1 630 382 1600',  'inx@inxinternational.com', '150 N Martingale Rd, Schaumburg, IL, USA'),
  ('Toyo Ink SC Holdings',       'Kenji Nakamura',    '+81 3 3272 5731',  'info@toyoink.com',         '3-13 Kyobashi 2-chome, Tokyo, Japan'),
  ('Flint Group GmbH',           'Klaus Bauer',       '+352 2 4850 0',    'info@flintgroup.com',      '26 bd Royal, Luxembourg City, Luxembourg'),
  ('Siegwerk Druckfarben AG',    'Thomas Weber',      '+49 2241 304 0',   'info@siegwerk.com',        'Alfred-Keller-Str. 55, Siegburg, Germany'),
  ('hubergroup Deutschland',     'Maria Schmidt',     '+49 89 462 0',     'contact@hubergroup.com',   'Werner-von-Siemens-Str. 1, Munich, Germany'),
  ('Marabu GmbH & Co. KG',       'Andreas Fischer',   '+49 7141 691 0',   'info@marabu.com',          'Asperger Str. 4, Tamm, Germany');

-- ============================================================
-- CLIENTS (15)
-- ============================================================
INSERT INTO "Clients" ("Name", "ClientType", "DocumentType", "DocumentID", "Email", "Phone", "Website", "Address") VALUES
  ('Impresiones Bogotá S.A.S.',   'Empresa',  'NIT', '900.123.456-7',  'compras@imbogota.co',     '+57 601 321 9000', 'www.imbogota.co',       'Cra 7 # 45-12, Bogotá, Cundinamarca'),
  ('Gráficas del Valle Ltda.',    'Empresa',  'NIT', '800.456.789-1',  'gerencia@grafivalle.com', '+57 602 889 1234', 'www.grafivalle.com',    'Av. 3N # 10-45, Cali, Valle del Cauca'),
  ('Litografía Medellín S.A.',    'Empresa',  'NIT', '811.234.567-0',  'pedidos@litomede.co',     '+57 604 444 5678', 'www.litomede.co',       'Calle 52 # 49-10, Medellín, Antioquia'),
  ('Carlos Andrés Ruiz Mora',     'Persona',  'CC',  '79.456.123',     'carlos.ruiz@gmail.com',   '+57 310 456 7890', NULL,                    'Cra 15 # 82-60, Bogotá, Cundinamarca'),
  ('Barranquilla Print Center',   'Empresa',  'NIT', '890.345.678-3',  'admin@bprintcenter.co',   '+57 605 330 1122', 'www.bprintcenter.co',   'Calle 74 # 43-180, Barranquilla, Atlántico'),
  ('Tecno Sistemas de Impresión', 'Empresa',  'RUT', '901.567.890-2',  'ventas@tecnosistemas.co', '+57 601 654 3210', 'www.tecnosistemas.co',  'Cra 50 # 13-30, Bogotá, Cundinamarca'),
  ('Ana María Torres Jiménez',    'Persona',  'CC',  '52.789.012',     'ana.torres@hotmail.com',  '+57 315 789 0123', NULL,                    'Calle 10 # 9-32, Manizales, Caldas'),
  ('Soluciones Gráficas SAS',     'Empresa',  'NIT', '900.789.012-5',  'info@solgraficas.co',     '+57 601 987 6543', 'www.solgraficas.co',    'Cra 13A # 38-32, Bogotá, Cundinamarca'),
  ('Packaging Pereira Ltda.',     'Empresa',  'NIT', '816.234.567-9',  'compras@pkgpereira.com',  '+57 606 335 7890', NULL,                    'Av. 30 de Agosto # 35-10, Pereira, Risaralda'),
  ('Juan Pablo Gómez Salcedo',    'Persona',  'CC',  '1.023.456.789',  'jgomez.print@gmail.com',  '+57 320 012 3456', NULL,                    'Calle 65 # 4-100, Bucaramanga, Santander'),
  ('Colombiana de Etiquetas S.A', 'Empresa',  'NIT', '830.123.456-4',  'pedidos@coletiq.co',      '+57 601 265 8900', 'www.coletiq.co',        'Zona Ind. Puente Aranda, Bogotá'),
  ('Artes Gráficas Cúcuta',       'Empresa',  'NIT', '891.012.345-8',  'agcucuta@gmail.com',      '+57 607 577 2211', NULL,                    'Av. Libertadores # 9-34, Cúcuta, N. Santander'),
  ('María Fernanda Ospina',       'Persona',  'CC',  '43.567.890',     'mf.ospina@outlook.com',   '+57 311 567 8901', NULL,                    'Cra 8 # 12-20, Pasto, Nariño'),
  ('Flexoprint del Caribe',       'Empresa',  'NIT', '892.345.678-6',  'ventas@flexocaribe.co',   '+57 605 420 3344', 'www.flexocaribe.co',    'Calle 30 # 15-22, Cartagena, Bolívar'),
  ('Impresos Industriales Norte', 'Empresa',  'RUT', '902.678.901-0',  'admin@iindnorte.co',      '+57 607 571 9900', 'www.iindnorte.co',      'Av. Granja # 4-56, Cúcuta, N. Santander');

-- ============================================================
-- PRODUCTS (30) – Stock inicial 0; los triggers lo actualizan
-- ============================================================
INSERT INTO "Products" ("ProductId", "Name", "Value", "Category", "Description", "Stock", "SupplierId") VALUES
  -- Inks (10) –– SupplierId: 1,2,3,4,5
  ('INK-QP-5009',  'LAMU 5009 Cyan Ink',           145.50, 'Ink',       'Cyan UV flexo ink, 1 kg cartridge',          0, 1),
  ('INK-QP-5010',  'LAMU 5010 Magenta Ink',         145.50, 'Ink',       'Magenta UV flexo ink, 1 kg cartridge',       0, 1),
  ('INK-QP-5011',  'LAMU 5011 Yellow Ink',          145.50, 'Ink',       'Yellow UV flexo ink, 1 kg cartridge',        0, 1),
  ('INK-QP-5012',  'LAMU 5012 Black Ink',           130.00, 'Ink',       'Black UV flexo ink, 1 kg cartridge',         0, 1),
  ('INK-SC-LA0056','LA 0056 Solvent Ink',            98.75, 'Ink',       'Solvent-based ink for wide-format printers',  0, 2),
  ('INK-SC-LA0057','LA 0057 Eco-Solvent Ink',        88.00, 'Ink',       'Eco-solvent ink, 500 ml',                    0, 2),
  ('INK-BF-UVG10', 'UV Gold 10 Metallic Ink',       210.00, 'Ink',       'UV-curable metallic gold ink, 1 kg',         0, 3),
  ('INK-NZ-HR200', 'Nazdar HR-200 Ink',             175.00, 'Ink',       'High-resolution thermal inkjet ink, 1 kg',   0, 4),
  ('INK-NZ-HR210', 'Nazdar HR-210 Ink White',       190.00, 'Ink',       'White HR ink for dark substrates, 1 kg',     0, 4),
  ('INK-IX-F900',  'INX F900 Flexo Black',          120.00, 'Ink',       'Water-based flexo black ink, 5 kg bucket',   0, 5),

  -- Additives (8) –– SupplierId: 1,2,6,7
  ('ADD-QP-A100',  'LAMU A100 Adhesion Promoter',    55.00, 'Additives', 'Adhesion promoter for non-porous substrates',0, 1),
  ('ADD-QP-A200',  'LAMU A200 Viscosity Reducer',    42.00, 'Additives', 'Viscosity reducer for UV inks',              0, 1),
  ('ADD-SC-OV10',  'SC Overprint Varnish',           75.00, 'Additives', 'Gloss overprint varnish, 5 kg',              0, 2),
  ('ADD-SC-MA20',  'SC Matte Additive',              68.00, 'Additives', 'Matte finish additive for flexo inks',       0, 2),
  ('ADD-TY-WA01',  'Toyo Wax Additive',              39.00, 'Additives', 'Slip and rub-resistance wax compound',       0, 6),
  ('ADD-TY-DE01',  'Toyo Defoamer D1',               34.00, 'Additives', 'Antifoam agent for water-based systems',     0, 6),
  ('ADD-FL-MR10',  'Flint MR-10 Migration Reducer',  92.00, 'Additives', 'Migration barrier additive for food pack.',  0, 7),
  ('ADD-FL-CT05',  'Flint CT-05 Catalyzer',         115.00, 'Additives', 'UV cure rate accelerator',                   0, 7),

  -- Tapes (7) –– SupplierId: 1,8,9
  ('TAP-QP-T33',   'TVSA Tape 33×700mm',             48.00, 'Tapes',     'Double-sided mounting tape 33 mm × 700 m',  0, 1),
  ('TAP-QP-T50',   'TVSA Tape 50×700mm',             58.00, 'Tapes',     'Double-sided mounting tape 50 mm × 700 m',  0, 1),
  ('TAP-SW-F250',  'Siegwerk F250 Foam Tape',         82.00, 'Tapes',     'High-density foam mounting tape 25 mm',      0, 8),
  ('TAP-SW-G300',  'Siegwerk G300 Gapless Tape',      95.00, 'Tapes',     'Gapless compressible foam tape 30 mm',       0, 8),
  ('TAP-HG-A100',  'Huber A100 Adhesive Tape',        35.00, 'Tapes',     'Standard transfer adhesive tape roll',       0, 9),
  ('TAP-HG-B200',  'Huber B200 Bridge Tape',          67.00, 'Tapes',     'Plate-to-sleeve bridge tape 3 mm',           0, 9),
  ('TAP-MR-CL01',  'Marabu ClearLam Tape',            44.00, 'Tapes',     'Transparent lamination tape 50 cm × 50 m',  0, 10),

  -- Devices (5) –– SupplierId: 2,5,10
  ('DEV-SC-POLY',  'Polyjet TU UV Curing Lamp',    15000.00, 'Devices',  'Industrial UV LED curing lamp 120 W',        0, 2),
  ('DEV-IX-VISC',  'INX Viscometer VS-3000',          850.00, 'Devices',  'Rotary viscometer for ink quality control',  0, 5),
  ('DEV-IX-SPEC',  'INX Spectrophotometer SP-10',    2200.00, 'Devices',  'Benchtop colour spectrophotometer',          0, 5),
  ('DEV-MR-CURE',  'Marabu LedCure Compact',         3500.00, 'Devices',  'Compact UV LED curing unit, portable',       0, 10),
  ('DEV-MR-PRBE',  'Marabu Tack Probe TP-2',          490.00, 'Devices',  'Automatic inkometer tack probe',             0, 10);

-- ============================================================
-- PURCHASE ORDERS (50)
-- ============================================================
INSERT INTO "PurchaseOrders" ("SupplierId", "OrderDate", "Status") VALUES
  -- 2023 (12 órdenes)
  (1, '2023-02-14 08:30:00-05', 'Received'),
  (2, '2023-03-05 09:00:00-05', 'Received'),
  (3, '2023-04-20 10:15:00-05', 'Received'),
  (4, '2023-05-10 11:00:00-05', 'Received'),
  (5, '2023-06-18 08:00:00-05', 'Received'),
  (1, '2023-07-02 14:00:00-05', 'Received'),
  (6, '2023-08-15 09:30:00-05', 'Received'),
  (7, '2023-09-22 10:00:00-05', 'Received'),
  (2, '2023-10-05 11:30:00-05', 'Cancelled'),
  (8, '2023-11-12 08:45:00-05', 'Received'),
  (9, '2023-11-28 09:00:00-05', 'Received'),
  (10,'2023-12-15 10:30:00-05', 'Received'),

  -- 2024 (15 órdenes)
  (1, '2024-01-08 08:00:00-05', 'Received'),
  (2, '2024-02-14 09:30:00-05', 'Received'),
  (3, '2024-03-01 10:00:00-05', 'Received'),
  (4, '2024-04-10 11:15:00-05', 'Received'),
  (5, '2024-04-25 08:30:00-05', 'Received'),
  (1, '2024-05-20 09:00:00-05', 'Received'),
  (6, '2024-06-05 10:30:00-05', 'Received'),
  (7, '2024-07-15 11:00:00-05', 'Received'),
  (8, '2024-08-02 08:00:00-05', 'Received'),
  (9, '2024-09-18 09:30:00-05', 'Received'),
  (10,'2024-10-05 10:00:00-05', 'Received'),
  (2, '2024-11-12 11:30:00-05', 'Cancelled'),
  (3, '2024-12-20 08:45:00-05', 'Received'),

  -- 2025 (15 órdenes)
  (1, '2025-01-10 09:00:00-05', 'Received'),
  (4, '2025-02-05 10:00:00-05', 'Received'),
  (5, '2025-03-12 08:30:00-05', 'Received'),
  (2, '2025-04-08 11:00:00-05', 'Received'),
  (6, '2025-05-20 09:30:00-05', 'Received'),
  (7, '2025-06-03 10:15:00-05', 'Received'),
  (8, '2025-06-25 08:00:00-05', 'Received'),
  (1, '2025-07-14 09:00:00-05', 'Received'),
  (9, '2025-08-01 10:30:00-05', 'Received'),
  (10,'2025-09-10 11:00:00-05', 'Approved'),
  (3, '2025-10-05 08:45:00-05', 'Approved'),
  (4, '2025-10-28 09:15:00-05', 'Pending'),
  (5, '2025-11-18 10:00:00-05', 'Pending'),
  (1, '2025-12-02 08:30:00-05', 'Pending'),
  (2, '2025-12-20 09:00:00-05', 'Pending'),

  -- 2026 (8 órdenes)
  (1, '2026-01-07 09:00:00-05', 'Received'),
  (3, '2026-01-22 10:30:00-05', 'Received'),
  (6, '2026-02-10 08:00:00-05', 'Received'),
  (7, '2026-03-01 11:00:00-05', 'Approved'),
  (8, '2026-03-15 09:30:00-05', 'Pending'),
  (2, '2026-04-05 10:00:00-05', 'Pending'),
  (9, '2026-04-20 08:45:00-05', 'Pending'),
  (10,'2026-05-10 09:00:00-05', 'Pending');

-- ============================================================
-- PURCHASE ORDER DETAILS
-- (dos líneas por orden para las órdenes relevantes)
-- ============================================================
INSERT INTO "PurchaseOrderDetails" ("PurchaseOrderId", "ProductId", "Quantity", "UnitPrice") VALUES
  -- PO 1 (2023)
  (1, 'INK-QP-5009', 50, 145.50), (1, 'INK-QP-5010', 50, 145.50),
  -- PO 2
  (2, 'INK-SC-LA0056', 40, 98.75), (2, 'ADD-SC-OV10', 20, 75.00),
  -- PO 3
  (3, 'INK-BF-UVG10', 20, 210.00), (3, 'ADD-FL-CT05', 15, 115.00),
  -- PO 4
  (4, 'INK-NZ-HR200', 30, 175.00), (4, 'ADD-QP-A100', 25, 55.00),
  -- PO 5
  (5, 'INK-IX-F900', 60, 120.00), (5, 'ADD-TY-WA01', 30, 39.00),
  -- PO 6
  (6, 'INK-QP-5011', 50, 145.50), (6, 'INK-QP-5012', 50, 130.00),
  -- PO 7
  (7, 'TAP-QP-T33', 100, 48.00), (7, 'ADD-TY-DE01', 40, 34.00),
  -- PO 8
  (8, 'TAP-SW-F250', 60, 82.00), (8, 'ADD-FL-MR10', 25, 92.00),
  -- PO 9 (Cancelled – valores para referencia)
  (9, 'DEV-SC-POLY', 1, 15000.00),
  -- PO 10
  (10,'TAP-SW-G300', 50, 95.00), (10,'TAP-HG-A100', 80, 35.00),
  -- PO 11
  (11,'TAP-HG-B200', 60, 67.00), (11,'TAP-MR-CL01', 70, 44.00),
  -- PO 12
  (12,'DEV-MR-PRBE', 3, 490.00), (12,'ADD-QP-A200', 40, 42.00),

  -- 2024
  (13,'INK-QP-5009', 60, 145.50), (13,'INK-QP-5010', 60, 145.50),
  (14,'INK-SC-LA0057', 50, 88.00), (14,'ADD-SC-MA20', 30, 68.00),
  (15,'INK-BF-UVG10', 25, 210.00), (15,'ADD-FL-CT05', 20, 115.00),
  (16,'INK-NZ-HR210', 35, 190.00), (16,'ADD-QP-A100', 30, 55.00),
  (17,'INK-IX-F900', 70, 120.00), (17,'ADD-TY-WA01', 40, 39.00),
  (18,'INK-QP-5011', 55, 145.50), (18,'INK-QP-5012', 55, 130.00),
  (19,'TAP-QP-T50', 90, 58.00), (19,'ADD-TY-DE01', 50, 34.00),
  (20,'TAP-SW-G300', 55, 95.00), (20,'ADD-FL-MR10', 30, 92.00),
  (21,'TAP-HG-B200', 65, 67.00), (21,'TAP-MR-CL01', 75, 44.00),
  (22,'INK-NZ-HR200', 40, 175.00),
  (23,'DEV-IX-VISC', 2, 850.00), (23,'DEV-MR-CURE', 1, 3500.00),
  (24,'INK-SC-LA0056', 45, 98.75), (24,'ADD-SC-OV10', 25, 75.00),
  -- PO 25 (Cancelled)
  -- 2025
  (26,'INK-QP-5009', 70, 148.00), (26,'INK-QP-5010', 70, 148.00),
  (27,'INK-NZ-HR200', 45, 178.00), (27,'ADD-QP-A200', 50, 43.00),
  (28,'INK-IX-F900', 80, 122.00), (28,'ADD-TY-DE01', 55, 35.00),
  (29,'INK-SC-LA0057', 55, 90.00), (29,'ADD-SC-MA20', 35, 70.00),
  (30,'TAP-QP-T33', 120, 50.00), (30,'TAP-QP-T50', 100, 60.00),
  (31,'TAP-SW-F250', 70, 84.00), (31,'ADD-FL-MR10', 35, 95.00),
  (32,'INK-BF-UVG10', 30, 215.00), (32,'ADD-FL-CT05', 25, 118.00),
  (33,'INK-QP-5011', 60, 148.00), (33,'INK-QP-5012', 60, 133.00),
  (34,'TAP-HG-A100', 90, 36.00), (34,'TAP-HG-B200', 70, 69.00),
  -- Pending orders (35–40)
  (35,'DEV-IX-SPEC', 1, 2200.00), (35,'DEV-MR-CURE', 2, 3500.00),
  (36,'INK-QP-5009', 80, 148.00), (36,'INK-QP-5010', 80, 148.00),
  (37,'ADD-SC-OV10', 40, 77.00), (37,'ADD-SC-MA20', 40, 70.00),
  (38,'INK-NZ-HR210', 50, 193.00), (38,'TAP-SW-G300', 60, 97.00),
  (39,'INK-IX-F900', 90, 122.00), (39,'ADD-TY-WA01', 60, 40.00),
  (40,'DEV-SC-POLY', 1, 15000.00), (40,'DEV-MR-PRBE', 5, 495.00),

  -- 2026
  (41,'INK-QP-5009', 90, 150.00), (41,'INK-QP-5012', 90, 135.00),
  (42,'TAP-QP-T33', 130, 52.00), (42,'TAP-QP-T50', 110, 62.00),
  (43,'ADD-FL-MR10', 40, 96.00), (43,'ADD-FL-CT05', 30, 120.00),
  -- Approved/Pending 2026
  (44,'DEV-IX-VISC', 3, 860.00), (44,'TAP-SW-F250', 75, 85.00),
  (45,'INK-BF-UVG10', 35, 218.00), (45,'ADD-QP-A100', 40, 57.00),
  (46,'INK-SC-LA0056', 50, 100.00), (46,'INK-SC-LA0057', 50, 90.00),
  (47,'INK-NZ-HR200', 55, 180.00), (47,'ADD-QP-A200', 60, 44.00),
  (48,'TAP-HG-B200', 80, 70.00), (48,'TAP-MR-CL01', 90, 46.00);

-- ============================================================
-- LOGICAL COSTS (for received orders)
-- ============================================================
INSERT INTO "LogicalCosts" ("Order_Number", "International_Transport", "Local_Transport", "Nationalization", "Cargo_Insurance", "Storage", "Others") VALUES
  (1,  450.00, 80.00,  320.00, 55.00, 40.00, 20.00),
  (2,  380.00, 70.00,  260.00, 45.00, 35.00, 15.00),
  (3,  520.00, 90.00,  400.00, 65.00, 50.00, 25.00),
  (4,  310.00, 65.00,  220.00, 40.00, 30.00, 18.00),
  (5,  490.00, 85.00,  350.00, 58.00, 42.00, 22.00),
  (6,  470.00, 82.00,  340.00, 56.00, 41.00, 21.00),
  (7,  360.00, 68.00,  250.00, 42.00, 32.00, 16.00),
  (8,  410.00, 75.00,  290.00, 50.00, 38.00, 19.00),
  (10, 430.00, 78.00,  310.00, 52.00, 39.00, 20.00),
  (11, 395.00, 72.00,  275.00, 46.00, 36.00, 17.00),
  (12, 340.00, 66.00,  235.00, 41.00, 31.00, 16.00),
  (13, 480.00, 84.00,  345.00, 57.00, 43.00, 23.00),
  (14, 355.00, 67.00,  245.00, 43.00, 33.00, 17.00),
  (15, 500.00, 88.00,  365.00, 60.00, 45.00, 24.00),
  (16, 420.00, 76.00,  300.00, 51.00, 38.00, 20.00),
  (17, 460.00, 81.00,  330.00, 54.00, 40.00, 21.00),
  (18, 475.00, 83.00,  342.00, 56.00, 42.00, 22.00),
  (19, 370.00, 69.00,  258.00, 44.00, 33.00, 17.00),
  (20, 415.00, 75.00,  295.00, 50.00, 38.00, 19.00),
  (21, 390.00, 71.00,  272.00, 46.00, 35.00, 18.00),
  (23, 610.00, 95.00,  480.00, 75.00, 55.00, 28.00),
  (24, 345.00, 66.00,  240.00, 42.00, 32.00, 16.00),
  (26, 490.00, 86.00,  355.00, 58.00, 44.00, 23.00),
  (27, 325.00, 64.00,  228.00, 40.00, 31.00, 16.00),
  (28, 465.00, 82.00,  335.00, 55.00, 41.00, 21.00),
  (29, 350.00, 67.00,  244.00, 42.00, 33.00, 17.00),
  (30, 385.00, 71.00,  268.00, 45.00, 35.00, 18.00),
  (31, 405.00, 74.00,  285.00, 49.00, 37.00, 19.00),
  (32, 510.00, 89.00,  372.00, 61.00, 46.00, 24.00),
  (33, 455.00, 80.00,  328.00, 54.00, 41.00, 21.00),
  (34, 380.00, 70.00,  262.00, 44.00, 34.00, 17.00),
  (41, 495.00, 87.00,  358.00, 59.00, 44.00, 23.00),
  (42, 372.00, 69.00,  260.00, 44.00, 34.00, 17.00),
  (43, 330.00, 64.00,  232.00, 41.00, 31.00, 16.00);

-- ============================================================
-- TRANSACTIONS (for received/approved orders)
-- ============================================================
INSERT INTO "Transactions" ("PurchaseOrderId", "Invoice_Number", "Reminder", "Transaction_Status", "Payment_Date") VALUES
  (1,  'INV-2023-001', NULL,               'Paid',      '2023-03-01'),
  (2,  'INV-2023-002', NULL,               'Paid',      '2023-04-05'),
  (3,  'INV-2023-003', NULL,               'Paid',      '2023-05-20'),
  (4,  'INV-2023-004', NULL,               'Paid',      '2023-06-10'),
  (5,  'INV-2023-005', NULL,               'Paid',      '2023-07-18'),
  (6,  'INV-2023-006', NULL,               'Paid',      '2023-08-05'),
  (7,  'INV-2023-007', NULL,               'Paid',      '2023-09-15'),
  (8,  'INV-2023-008', NULL,               'Paid',      '2023-10-22'),
  (10, 'INV-2023-010', NULL,               'Paid',      '2023-12-12'),
  (11, 'INV-2023-011', NULL,               'Paid',      '2023-12-28'),
  (12, 'INV-2023-012', NULL,               'Paid',      '2024-01-15'),
  (13, 'INV-2024-001', NULL,               'Paid',      '2024-02-08'),
  (14, 'INV-2024-002', NULL,               'Paid',      '2024-03-14'),
  (15, 'INV-2024-003', NULL,               'Paid',      '2024-04-01'),
  (16, 'INV-2024-004', NULL,               'Paid',      '2024-05-10'),
  (17, 'INV-2024-005', NULL,               'Paid',      '2024-05-25'),
  (18, 'INV-2024-006', NULL,               'Paid',      '2024-06-20'),
  (19, 'INV-2024-007', NULL,               'Paid',      '2024-07-15'),
  (20, 'INV-2024-008', NULL,               'Paid',      '2024-08-02'),
  (21, 'INV-2024-009', NULL,               'Paid',      '2024-09-18'),
  (23, 'INV-2024-011', NULL,               'Paid',      '2025-01-20'),
  (24, 'INV-2024-012', NULL,               'Paid',      '2025-01-05'),
  (26, 'INV-2025-001', NULL,               'Paid',      '2025-02-10'),
  (27, 'INV-2025-002', NULL,               'Paid',      '2025-03-05'),
  (28, 'INV-2025-003', NULL,               'Paid',      '2025-04-12'),
  (29, 'INV-2025-004', NULL,               'Paid',      '2025-05-08'),
  (30, 'INV-2025-005', NULL,               'Paid',      '2025-06-20'),
  (31, 'INV-2025-006', NULL,               'Paid',      '2025-07-03'),
  (32, 'INV-2025-007', NULL,               'Paid',      '2025-07-25'),
  (33, 'INV-2025-008', NULL,               'Paid',      '2025-08-14'),
  (34, 'INV-2025-009', NULL,               'Paid',      '2025-09-10'),
  (35, 'INV-2025-010', 'Pago 30 días',     'Pending',   NULL),
  (36, 'INV-2025-011', 'Pago 30 días',     'Pending',   NULL),
  (37, 'INV-2025-012', 'Vence 2026-01-18', 'Pending',   NULL),
  (38, 'INV-2025-013', 'Vence 2026-01-28', 'Pending',   NULL),
  (39, 'INV-2025-014', 'Vence 2026-02-02', 'Pending',   NULL),
  (40, 'INV-2025-015', 'Vence 2026-02-20', 'Pending',   NULL),
  (41, 'INV-2026-001', NULL,               'Paid',      '2026-02-07'),
  (42, 'INV-2026-002', NULL,               'Paid',      '2026-02-22'),
  (43, 'INV-2026-003', NULL,               'Paid',      '2026-03-10'),
  (44, 'INV-2026-004', 'Pago 30 días',     'Pending',   NULL),
  (45, 'INV-2026-005', 'Pago 30 días',     'Pending',   NULL),
  (46, 'INV-2026-006', 'Vence 2026-06-05', 'Pending',   NULL),
  (47, 'INV-2026-007', 'Vence 2026-06-20', 'Pending',   NULL),
  (48, 'INV-2026-008', 'Vence 2026-07-10', 'Pending',   NULL);

-- ============================================================
-- PURCHASE RECEIPTS (for "Received" orders)
-- ============================================================
INSERT INTO "PurchaseReceipts" ("PurchaseOrderId", "ReceiptDate") VALUES
  (1,  '2023-02-20 10:00:00-05'),
  (2,  '2023-03-12 11:00:00-05'),
  (3,  '2023-04-28 09:30:00-05'),
  (4,  '2023-05-18 10:15:00-05'),
  (5,  '2023-06-26 08:45:00-05'),
  (6,  '2023-07-10 11:30:00-05'),
  (7,  '2023-08-22 09:00:00-05'),
  (8,  '2023-09-30 10:30:00-05'),
  (10, '2023-11-20 08:30:00-05'),
  (11, '2023-12-06 09:15:00-05'),
  (12, '2023-12-23 10:00:00-05'),
  (13, '2024-01-16 08:45:00-05'),
  (14, '2024-02-22 09:30:00-05'),
  (15, '2024-03-10 10:15:00-05'),
  (16, '2024-04-18 11:00:00-05'),
  (17, '2024-05-03 08:30:00-05'),
  (18, '2024-05-28 09:00:00-05'),
  (19, '2024-06-13 10:30:00-05'),
  (20, '2024-07-23 11:00:00-05'),
  (21, '2024-08-10 08:45:00-05'),
  (23, '2024-12-28 09:30:00-05'),
  (24, '2024-11-20 10:00:00-05'),
  (26, '2025-01-18 08:30:00-05'),
  (27, '2025-02-13 09:15:00-05'),
  (28, '2025-03-20 10:00:00-05'),
  (29, '2025-04-16 11:15:00-05'),
  (30, '2025-05-28 08:45:00-05'),
  (31, '2025-07-03 09:30:00-05'),
  (32, '2025-07-22 10:15:00-05'),
  (33, '2025-08-12 11:00:00-05'),
  (34, '2025-09-18 08:30:00-05'),
  (41, '2026-01-15 09:00:00-05'),
  (42, '2026-01-30 10:30:00-05'),
  (43, '2026-02-18 11:00:00-05');

-- ============================================================
-- PURCHASE RECEIPT DETAILS
-- (triggers actualizan Products.Stock y crean InventoryMovements)
-- ============================================================
INSERT INTO "PurchaseReceiptDetails" ("ReceiptId", "ProductId", "QuantityReceived", "UnitCost") VALUES
  (1,  'INK-QP-5009',  50, 145.50), (1,  'INK-QP-5010',  50, 145.50),
  (2,  'INK-SC-LA0056',40,  98.75), (2,  'ADD-SC-OV10',   20,  75.00),
  (3,  'INK-BF-UVG10', 20, 210.00), (3,  'ADD-FL-CT05',   15, 115.00),
  (4,  'INK-NZ-HR200', 30, 175.00), (4,  'ADD-QP-A100',   25,  55.00),
  (5,  'INK-IX-F900',  60, 120.00), (5,  'ADD-TY-WA01',   30,  39.00),
  (6,  'INK-QP-5011',  50, 145.50), (6,  'INK-QP-5012',   50, 130.00),
  (7,  'TAP-QP-T33',  100,  48.00), (7,  'ADD-TY-DE01',   40,  34.00),
  (8,  'TAP-SW-F250',  60,  82.00), (8,  'ADD-FL-MR10',   25,  92.00),
  (9,  'TAP-SW-G300',  50,  95.00), (9,  'TAP-HG-A100',   80,  35.00),
  (10, 'TAP-HG-B200',  60,  67.00), (10, 'TAP-MR-CL01',   70,  44.00),
  (11, 'DEV-MR-PRBE',   3, 490.00), (11, 'ADD-QP-A200',   40,  42.00),
  (12, 'INK-QP-5009',  60, 145.50), (12, 'INK-QP-5010',   60, 145.50),
  (13, 'INK-SC-LA0057',50,  88.00), (13, 'ADD-SC-MA20',   30,  68.00),
  (14, 'INK-BF-UVG10', 25, 210.00), (14, 'ADD-FL-CT05',   20, 115.00),
  (15, 'INK-NZ-HR210', 35, 190.00), (15, 'ADD-QP-A100',   30,  55.00),
  (16, 'INK-IX-F900',  70, 120.00), (16, 'ADD-TY-WA01',   40,  39.00),
  (17, 'INK-QP-5011',  55, 145.50), (17, 'INK-QP-5012',   55, 130.00),
  (18, 'TAP-QP-T50',   90,  58.00), (18, 'ADD-TY-DE01',   50,  34.00),
  (19, 'TAP-SW-G300',  55,  95.00), (19, 'ADD-FL-MR10',   30,  92.00),
  (20, 'TAP-HG-B200',  65,  67.00), (20, 'TAP-MR-CL01',   75,  44.00),
  (21, 'DEV-IX-VISC',   2, 850.00), (21, 'DEV-MR-CURE',    1, 3500.00),
  (22, 'INK-SC-LA0056',45,  98.75), (22, 'ADD-SC-OV10',    25,  75.00),
  (23, 'INK-QP-5009',  70, 148.00), (23, 'INK-QP-5010',   70, 148.00),
  (24, 'INK-NZ-HR200', 45, 178.00), (24, 'ADD-QP-A200',   50,  43.00),
  (25, 'INK-IX-F900',  80, 122.00), (25, 'ADD-TY-DE01',   55,  35.00),
  (26, 'INK-SC-LA0057',55,  90.00), (26, 'ADD-SC-MA20',   35,  70.00),
  (27, 'TAP-QP-T33',  120,  50.00), (27, 'TAP-QP-T50',   100,  60.00),
  (28, 'TAP-SW-F250',  70,  84.00), (28, 'ADD-FL-MR10',   35,  95.00),
  (29, 'INK-BF-UVG10', 30, 215.00), (29, 'ADD-FL-CT05',   25, 118.00),
  (30, 'INK-QP-5011',  60, 148.00), (30, 'INK-QP-5012',   60, 133.00),
  (31, 'TAP-HG-A100',  90,  36.00), (31, 'TAP-HG-B200',   70,  69.00),
  (32, 'INK-QP-5009',  90, 150.00), (32, 'INK-QP-5012',   90, 135.00),
  (33, 'TAP-QP-T33',  130,  52.00), (33, 'TAP-QP-T50',   110,  62.00),
  (34, 'ADD-FL-MR10',  40,  96.00), (34, 'ADD-FL-CT05',   30, 120.00);

-- ============================================================
-- SALES (40)
-- ============================================================
INSERT INTO "Sales" ("ClientId", "InvoiceDate", "Status", "ExchangeRate", "Subtotal", "Tax", "Total") VALUES
  -- 2023 (8)
  (1,  '2023-03-15 10:00:00-05', 'Emitida',   4050.00, 0, 0, 0),
  (2,  '2023-04-20 11:00:00-05', 'Emitida',   4100.00, 0, 0, 0),
  (3,  '2023-06-05 09:30:00-05', 'Emitida',   4120.00, 0, 0, 0),
  (4,  '2023-07-22 10:15:00-05', 'Emitida',   4050.00, 0, 0, 0),
  (5,  '2023-08-10 08:45:00-05', 'Emitida',   4080.00, 0, 0, 0),
  (6,  '2023-09-28 11:30:00-05', 'Emitida',   4100.00, 0, 0, 0),
  (7,  '2023-11-14 09:00:00-05', 'Emitida',   4150.00, 0, 0, 0),
  (8,  '2023-12-05 10:30:00-05', 'Emitida',   4180.00, 0, 0, 0),
  -- 2024 (10)
  (1,  '2024-01-20 09:00:00-05', 'Emitida',   4200.00, 0, 0, 0),
  (9,  '2024-02-28 10:30:00-05', 'Emitida',   4210.00, 0, 0, 0),
  (10, '2024-03-15 08:45:00-05', 'Emitida',   4180.00, 0, 0, 0),
  (11, '2024-04-22 11:00:00-05', 'Emitida',   4200.00, 0, 0, 0),
  (2,  '2024-05-08 09:30:00-05', 'Emitida',   4220.00, 0, 0, 0),
  (12, '2024-06-18 10:00:00-05', 'Emitida',   4230.00, 0, 0, 0),
  (3,  '2024-07-30 08:30:00-05', 'Emitida',   4200.00, 0, 0, 0),
  (13, '2024-09-05 11:15:00-05', 'Emitida',   4250.00, 0, 0, 0),
  (4,  '2024-10-12 09:45:00-05', 'Emitida',   4230.00, 0, 0, 0),
  (14, '2024-11-20 10:30:00-05', 'Emitida',   4260.00, 0, 0, 0),
  -- 2025 (14)
  (1,  '2025-01-15 09:00:00-05', 'Emitida',   4300.00, 0, 0, 0),
  (5,  '2025-02-08 10:15:00-05', 'Emitida',   4280.00, 0, 0, 0),
  (6,  '2025-03-05 08:45:00-05', 'Emitida',   4300.00, 0, 0, 0),
  (15, '2025-03-25 11:30:00-05', 'Emitida',   4320.00, 0, 0, 0),
  (7,  '2025-04-10 09:00:00-05', 'Emitida',   4300.00, 0, 0, 0),
  (8,  '2025-05-02 10:30:00-05', 'Emitida',   4310.00, 0, 0, 0),
  (9,  '2025-06-14 08:30:00-05', 'Emitida',   4330.00, 0, 0, 0),
  (10, '2025-07-08 09:15:00-05', 'Emitida',   4320.00, 0, 0, 0),
  (11, '2025-08-20 10:00:00-05', 'Emitida',   4350.00, 0, 0, 0),
  (2,  '2025-09-03 11:30:00-05', 'Emitida',   4340.00, 0, 0, 0),
  (12, '2025-10-15 09:45:00-05', 'Emitida',   4360.00, 0, 0, 0),
  (13, '2025-11-05 08:30:00-05', 'Emitida',   4370.00, 0, 0, 0),
  (3,  '2025-12-02 10:00:00-05', 'Emitida',   4380.00, 0, 0, 0),
  (14, '2025-12-20 11:15:00-05', 'Emitida',   4390.00, 0, 0, 0),
  -- 2026 (8)
  (1,  '2026-01-10 09:00:00-05', 'Emitida',   4400.00, 0, 0, 0),
  (4,  '2026-01-28 10:30:00-05', 'Emitida',   4420.00, 0, 0, 0),
  (5,  '2026-02-12 08:45:00-05', 'Emitida',   4400.00, 0, 0, 0),
  (15, '2026-03-05 11:00:00-05', 'Emitida',   4430.00, 0, 0, 0),
  (6,  '2026-03-22 09:30:00-05', 'Emitida',   4410.00, 0, 0, 0),
  (7,  '2026-04-08 10:00:00-05', 'Emitida',   4450.00, 0, 0, 0),
  (8,  '2026-04-25 08:30:00-05', 'Emitida',   4440.00, 0, 0, 0),
  (9,  '2026-05-10 09:15:00-05', 'Emitida',   4460.00, 0, 0, 0);

-- ============================================================
-- SALES DETAILS
-- (triggers actualizan Products.Stock y crean InventoryMovements)
-- ============================================================
INSERT INTO "SalesDetails" ("InvoiceId", "ProductId", "Quantity", "UnitPrice") VALUES
  -- 2023
  (1,  'INK-QP-5009',  8, 195.00), (1,  'ADD-QP-A100', 10, 75.00),
  (2,  'INK-QP-5010',  6, 195.00), (2,  'TAP-QP-T33',  15, 65.00),
  (3,  'INK-BF-UVG10', 4, 280.00), (3,  'ADD-FL-CT05',  5, 155.00),
  (4,  'INK-SC-LA0056',8,  132.00), (4,  'ADD-SC-OV10', 8,  100.00),
  (5,  'INK-IX-F900',  10, 160.00), (5,  'ADD-TY-WA01',12,  55.00),
  (6,  'INK-QP-5011',   7, 195.00), (6,  'INK-QP-5012',  7, 175.00),
  (7,  'TAP-SW-F250',   8, 110.00), (7,  'ADD-TY-DE01', 12,  50.00),
  (8,  'TAP-HG-B200',  10,  90.00), (8,  'TAP-MR-CL01', 12,  60.00),
  -- 2024
  (9,  'INK-QP-5009',  10, 198.00), (9,  'ADD-QP-A200',  15,  58.00),
  (10, 'INK-SC-LA0057', 8,  118.00), (10, 'ADD-SC-MA20',  10,  92.00),
  (11, 'INK-NZ-HR200',  7, 234.00), (11, 'ADD-QP-A100',  12,  75.00),
  (12, 'INK-QP-5011',   9, 198.00), (12, 'TAP-QP-T50',   15,  78.00),
  (13, 'INK-IX-F900',  12, 162.00), (13, 'ADD-TY-WA01',  15,  55.00),
  (14, 'TAP-SW-G300',  10, 128.00), (14, 'ADD-FL-MR10',   8, 124.00),
  (15, 'INK-QP-5010',  11, 198.00), (15, 'INK-QP-5012',  11, 175.00),
  (16, 'TAP-HG-A100',  15,  48.00), (16, 'TAP-MR-CL01',  15,  60.00),
  (17, 'INK-NZ-HR210',  6, 254.00), (17, 'ADD-QP-A200',  12,  58.00),
  (18, 'INK-BF-UVG10',  5, 282.00), (18, 'ADD-FL-CT05',   7, 155.00),
  -- 2025
  (19, 'INK-QP-5009',  12, 200.00), (19, 'ADD-QP-A100',  15,  77.00),
  (20, 'INK-SC-LA0056',10, 134.00), (20, 'TAP-QP-T33',   18,  68.00),
  (21, 'INK-IX-F900',  14, 164.00), (21, 'ADD-TY-DE01',  18,  52.00),
  (22, 'INK-QP-5011',  11, 200.00), (22, 'INK-QP-5012',  11, 180.00),
  (23, 'TAP-SW-F250',  10, 113.00), (23, 'ADD-FL-MR10',  10, 128.00),
  (24, 'INK-QP-5010',  13, 200.00), (24, 'TAP-QP-T50',   20,  80.00),
  (25, 'INK-NZ-HR200',  8, 238.00), (25, 'ADD-SC-OV10',  10, 102.00),
  (26, 'INK-BF-UVG10',  6, 288.00), (26, 'ADD-FL-CT05',   8, 158.00),
  (27, 'TAP-SW-G300',  12, 130.00), (27, 'ADD-TY-WA01',  18,  56.00),
  (28, 'INK-QP-5009',  14, 200.00), (28, 'ADD-SC-MA20',  14,  94.00),
  (29, 'TAP-HG-B200',  14,  93.00), (29, 'TAP-MR-CL01',  18,  62.00),
  (30, 'INK-SC-LA0057',12, 120.00), (30, 'ADD-QP-A200',  18,  59.00),
  (31, 'INK-NZ-HR210',  8, 256.00), (31, 'ADD-QP-A100',  15,  77.00),
  (32, 'INK-QP-5011',  13, 200.00), (32, 'TAP-QP-T33',   22,  70.00),
  -- 2026
  (33, 'INK-QP-5009',  15, 202.00), (33, 'ADD-QP-A100',  18,  78.00),
  (34, 'INK-QP-5010',  14, 202.00), (34, 'TAP-QP-T50',   22,  82.00),
  (35, 'INK-IX-F900',  16, 165.00), (35, 'ADD-TY-DE01',  20,  53.00),
  (36, 'INK-BF-UVG10',  7, 290.00), (36, 'ADD-FL-CT05',   9, 160.00),
  (37, 'TAP-SW-F250',  12, 115.00), (37, 'ADD-FL-MR10',  12, 130.00),
  (38, 'INK-QP-5011',  14, 202.00), (38, 'INK-QP-5012',  14, 182.00),
  (39, 'INK-NZ-HR200', 10, 240.00), (39, 'ADD-SC-OV10',  12, 104.00),
  (40, 'TAP-HG-B200',  15,  95.00), (40, 'ADD-SC-MA20',  16,  96.00);

-- Recalcular totales de ventas
DO $$
DECLARE r RECORD;
BEGIN
  FOR r IN SELECT "InvoiceId" FROM "Sales" LOOP
    CALL sp_Sales_RecalculateTotals(r."InvoiceId", NULL);
  END LOOP;
END;
$$;

-- ============================================================
-- Resumen de datos insertados
-- ============================================================
SELECT 'Suppliers'           AS "Table", COUNT(*) AS "Rows" FROM "Suppliers"
UNION ALL
SELECT 'Products',            COUNT(*) FROM "Products"
UNION ALL
SELECT 'Clients',             COUNT(*) FROM "Clients"
UNION ALL
SELECT 'PurchaseOrders',      COUNT(*) FROM "PurchaseOrders"
UNION ALL
SELECT 'PurchaseOrderDetails',COUNT(*) FROM "PurchaseOrderDetails"
UNION ALL
SELECT 'Transactions',        COUNT(*) FROM "Transactions"
UNION ALL
SELECT 'PurchaseReceipts',    COUNT(*) FROM "PurchaseReceipts"
UNION ALL
SELECT 'PurchaseReceiptDetails',COUNT(*) FROM "PurchaseReceiptDetails"
UNION ALL
SELECT 'Sales',               COUNT(*) FROM "Sales"
UNION ALL
SELECT 'SalesDetails',        COUNT(*) FROM "SalesDetails"
UNION ALL
SELECT 'InventoryMovements',  COUNT(*) FROM "InventoryMovements"
ORDER BY 1;
